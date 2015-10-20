using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
using System.Linq;
using HRM.Business.Canteen.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using System;
using HRM.Business.Hr.Models;
using VnResource.Helper.Data;
using HRM.Business.HrmSystem.Models;

using HRM.Data.Entity;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities.Helpers;
namespace HRM.Business.Canteen.Domain
{
    public class Can_MealRecordServices : BaseService
    {
        public List<Can_MealRecordEntity> GetMealRecordSummary(string _line, string _catering, string _canteen, DateTime dateStart, DateTime dateEnd, List<Hre_ProfileEntity> lstProfileIDs, string UserLogin)
        {
            dateStart = dateStart.Date;
            dateEnd = dateEnd.Date.AddDays(1).AddMilliseconds(-1);

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCan_MealRecord = new CustomBaseRepository<Can_MealRecord>(unitOfWork);
                var repoCan_TamScanLog = new CustomBaseRepository<Can_TamScanLogCMS>(unitOfWork);
                var repoCan_Line = new CustomBaseRepository<Can_Line>(unitOfWork);
                var repoCan_Canteen = new CustomBaseRepository<Can_Canteen>(unitOfWork);
                var repoCan_Catering = new CustomBaseRepository<Can_Catering>(unitOfWork);
                var repoSys_AllSetting = new CustomBaseRepository<Sys_AllSetting>(unitOfWork);
                string status = string.Empty;
                //string strIDs = string.Join(",", lstProfileIDs.ToArray());
                #region xử lý cấu hình giờ kết thúc ăn - DateStart & DateEnd
                DateTime dateConfig = DateTime.MinValue;
                var rsTimeConfig = GetData<Sys_AllSettingEntity>(AppConfig.HRM_CAN_MEALRECORD_EATEND_CONFIG.ToString(), ConstantSql.hrm_sys_sp_get_AllSettingByKey, UserLogin, ref status).FirstOrDefault();
                if (rsTimeConfig != null && rsTimeConfig.Value1 != null)
                {
                    dateConfig = DateTimeHelper.ConvertStringToDateTime(rsTimeConfig.Value1, ConstantFormat.HRM_Format_YearMonthDay_HoursMinSecond_ffffff.ToString());
                }
                double hourConfig = dateConfig.Hour + (((double)dateConfig.Minute) / 60);
                dateStart = dateStart.Date.AddHours(hourConfig);
                dateEnd = dateEnd.Date.AddDays(1).AddHours(hourConfig).AddMilliseconds(-1);
                #endregion

                #region cấu hình số phút xử lý trùng
                int rsScanMulti = 0;
                var rsScanMultiConfig = GetData<Sys_AllSettingEntity>(AppConfig.HRM_CAN_MEALRECORD_SCANMULTI_CONFIG.ToString(), ConstantSql.hrm_sys_sp_get_AllSettingByKey, UserLogin, ref status).FirstOrDefault();

                if (rsScanMultiConfig != null)
                {
                    rsScanMulti = rsScanMultiConfig.Value1 != null ? int.Parse(rsScanMultiConfig.Value1.ToString()) : 0;
                }
                #endregion

                var tamScanLogs = repoCan_TamScanLog
                    .FindBy(s => dateStart <= s.TimeLog && s.TimeLog <= dateEnd)
                    .Select(s => new { s.MachineCode, s.CardCode, s.TimeLog, s.ID })
                    .ToList();
                var cardCodes = tamScanLogs.Select(s => s.CardCode).ToList();

                //strIDs = Common.DotNetToOracle(strIDs);
                //var profiles = GetData<Hre_ProfileEntity>(strIDs, ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status)
                //    .Where(s => cardCodes.Contains(s.CodeAttendance) && lstProfileIDs.Contains(s.ID))
                var profiles = lstProfileIDs
                    .Where(s => cardCodes.Contains(s.CodeAttendance))
                    .Select(s => new { s.ID, s.CodeAttendance, s.CodeEmp, s.ProfileName, s.OrgStructureName }).ToList();

                tamScanLogs = tamScanLogs.Where(s => profiles.Select(p => p.CodeAttendance).Contains(s.CardCode)).OrderBy(s => s.CardCode).ThenBy(s=>s.TimeLog).ToList();
                var lines = repoCan_Line.FindBy(s => s.IsDelete == null).ToList();
                var canteens = repoCan_Canteen.FindBy(s => s.IsDelete == null).ToList();
                var caterings = repoCan_Catering.FindBy(s => s.IsDelete == null).ToList();
                if (_line != null)
                {
                    List<Guid> lstLine = _line.Split(',').Select(Guid.Parse).ToList();
                    lines = lines.Where(s => lstLine.Contains(s.ID)).ToList();
                }
                if (_canteen != null)
                {
                    List<Guid> lstcanteen = _canteen.Split(',').Select(Guid.Parse).ToList();
                    canteens = canteens.Where(s => lstcanteen.Contains(s.ID)).ToList();
                }
                if (_catering != null)
                {
                    List<Guid> lstcatering = _catering.Split(',').Select(Guid.Parse).ToList();
                    caterings = caterings.Where(s => lstcatering.Contains(s.ID)).ToList();
                }

                List<object> lstObj = new List<object>();
                lstObj.Add(dateStart);
                lstObj.Add(dateEnd);
                List<Can_MealRecordCheckEntity> mealRecordCheckList = GetData<Can_MealRecordCheckEntity>(lstObj, ConstantSql.hrm_can_sp_get_MealRecord_ByDateFromDateTo, UserLogin, ref status);
                Can_MealRecordEntity record = new Can_MealRecordEntity();
                List<Can_MealRecordEntity> lstRecord = new List<Can_MealRecordEntity>();
                List<Can_MealRecord> lstEdit = new List<Can_MealRecord>();
                List<Can_MealRecord> lstAdd = new List<Can_MealRecord>();

                List<Guid> lstDuplicateTamScan = new List<Guid>();
                foreach (var tamScanLog in tamScanLogs)
                {
                    if (lstDuplicateTamScan.Contains(tamScanLog.ID))
                        continue;

                    record = new Can_MealRecordEntity();
                    var profile = profiles.FirstOrDefault(s => s.CodeAttendance == tamScanLog.CardCode);
                    var line = lines.FirstOrDefault(s => tamScanLog.MachineCode == s.MachineCode);

                    if (profile != null && line != null)
                    { 
                        #region xử lý quẹt thẻ trùng theo HRM_CAN_MEALRECORD_SCANMULTI_CONFIG
                        var tamScanLogProfiles = tamScanLogs
                            .Where(s => s.CardCode == tamScanLog.CardCode && s.TimeLog.Value.Date == tamScanLog.TimeLog.Value.Date && s.MachineCode == tamScanLog.MachineCode)
                            .OrderBy(s => s.TimeLog)
                            .ToList();

                        if (tamScanLogProfiles.Count > 1 && rsScanMulti > 0)
                        {
                            for (int i = 0; i < tamScanLogProfiles.Count; i++)
                            {
                                var tamsanFirst = tamScanLogProfiles[i];
                                int j = i + 1;
                                if (j > tamScanLogProfiles.Count - 1)
                                {
                                    j = i;
                                }
                                var tamsanNear = tamScanLogProfiles[j];

                                if (tamsanNear.TimeLog.Value.Subtract(tamsanFirst.TimeLog.Value).TotalMinutes < rsScanMulti 
                                        && tamsanFirst.ID != tamsanNear.ID )
                                {
                                    if (tamsanFirst.MachineCode == tamsanNear.MachineCode)
                                    { 
                                        lstDuplicateTamScan.Add(tamsanNear.ID);
                                        break;
                                    }
                                }
                            }
                        }
                        #endregion
                        
                        #region xử lý add record
                        record.TimeLog = tamScanLog.TimeLog.HasValue ? tamScanLog.TimeLog.Value : DateTime.MinValue;
                        if (record.TimeLog < record.TimeLog.Value.Date.AddHours(hourConfig))
                        {
                            record.WorkDay = record.TimeLog.Value.AddDays(-1);
                        }
                        else
                        {
                            record.WorkDay = record.TimeLog;
                        }

                        if (tamScanLog.MachineCode != null)
                        {
                            record.MachineCode = tamScanLog.MachineCode;
                            if (line != null)
                            {
                                record.LineName = line.LineName;
                                record.LineID = line.ID;
                                record.Amount = (decimal)line.Amount;
                                if (line.CanteenID != null)
                                {
                                    var cant = canteens.Where(c => c.ID == line.CanteenID).FirstOrDefault();
                                    record.CanteenName = cant.CanteenName;
                                    record.CanteenID = line.CanteenID;
                                }
                                if (line.CateringID != null)
                                {
                                    var cater = caterings.Where(c => c.ID == line.CateringID).FirstOrDefault();
                                    record.CateringName = cater.CateringName;
                                    record.CateringID = line.CateringID;
                                }
                            }
                        }
                            
                        record.ProfileName = profile.ProfileName;
                        record.CodeEmp = profile.CodeEmp;
                        record.OrgStructureName = profile.OrgStructureName;
                        record.CodeAttendance = profile.CodeAttendance;
                        record.DateCreate = DateTime.Now;

                        var recordAdd = record.CopyData<Can_MealRecord>();
                        recordAdd.ProfileID = profile.ID;
                        var CheckList = mealRecordCheckList.Where(s => s.ProfileID == profile.ID && s.TimeLog == tamScanLog.TimeLog).ToList();
                        if (CheckList != null)
                        {
                            if (CheckList.Count() > 0)
                            {
                                var idUpdate = CheckList.FirstOrDefault();
                                recordAdd.ID = idUpdate.ID;
                                //lstEdit.Add(recordAdd);
                                repoCan_MealRecord.Edit(recordAdd);
                            }
                            else
                            {
                                recordAdd.ID = Guid.NewGuid();
                                repoCan_MealRecord.Add(recordAdd);
                                //lstAdd.Add(recordAdd);
                            }
                        }
                        record.ID = tamScanLog.ID;
                        lstRecord.Add(record);
                        #endregion
                        
                    }
                }

                repoCan_MealRecord.SaveChanges();


                if (lstRecord.Count > 0)
                {
                    Sys_AllSetting sys = new Sys_AllSetting();
                    Sys_AllSetting rs = GetData<Sys_AllSetting>(AppConfig.HRM_CAN_MEALRECORD_SUMMARY.ToString(), ConstantSql.hrm_sys_sp_get_AllSettingByKey, UserLogin, ref status).FirstOrDefault();
                    sys.Value1 = dateStart.ToString(ConstantFormat.HRM_Format_YearMonthDay_HoursMinSecond.ToString());
                    sys.Value2 = dateEnd.ToString(ConstantFormat.HRM_Format_YearMonthDay_HoursMinSecond.ToString());
                    sys.Name = AppConfig.HRM_CAN_MEALRECORD_SUMMARY.ToString();

                    if (rs != null)
                    {
                        sys.ID = rs.ID;
                        repoSys_AllSetting.Edit(sys);
                    }
                    else
                    {
                        sys.ID = Guid.NewGuid();
                        repoSys_AllSetting.Add(sys);
                    }
                    repoSys_AllSetting.SaveChanges();
                }
                //lstRecord.Where().(s => s.MealAllowanceTypeName = ConstantDisplay.HRM_Enum_Submit.TranslateString()).ToList();
                return lstRecord;
            }
        }
    }
}
