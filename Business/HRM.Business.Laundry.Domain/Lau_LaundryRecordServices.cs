using System.Linq;
using HRM.Business.Laundry.Models;
using HRM.Business.Main.Domain;
using HRM.Data.BaseRepository;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using System;
using HRM.Business.Hr.Models;
using VnResource.Helper.Data;
using HRM.Data.Entity;

namespace HRM.Business.Laundry.Domain
{
    public class Lau_LaundryRecordServices : BaseService
    {
           
        /// <summary>
        /// [Hieu.van] Tổng Hợp giặt là
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="lstProfileIDs"></param>
        /// <returns></returns>
        public List<LMS_LaundryRecordEntity> GetLaundryRecordSummary(string _line, string _locker, string _marker, DateTime dateStart, DateTime dateEnd, List<Hre_ProfileEntity> lstProfileIDs,string UserLogin)
        {
            dateStart = dateStart.Date;
            dateEnd = dateEnd.Date.AddDays(1).AddMilliseconds(-1);

            string status = string.Empty;
            //string strIDs = string.Join(",", lstProfileIDs.ToArray());

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoLau_TamScanLog = new CustomBaseRepository<LMS_TamScanLogLMS>(unitOfWork);
                var repoLau_Line = new CustomBaseRepository<LMS_LineLMS>(unitOfWork);
                var repoLau_Locker = new CustomBaseRepository<LMS_LockerLMS>(unitOfWork);
                var repoLau_Marker = new CustomBaseRepository<LMS_Marker>(unitOfWork);
                var repoLau_LaundryRecord = new CustomBaseRepository<LMS_LaundryRecord>(unitOfWork);
                var repoSys_AllSetting = new CustomBaseRepository<Sys_AllSetting>(unitOfWork);

                #region cấu hình số phút xử lý trùng
                int rsScanMulti = 0;
                var rsScanMultiConfig = GetData<Sys_AllSetting>(AppConfig.HRM_LAU_LAUNDRYRECORD_SCANMULTI_CONFIG.ToString(), ConstantSql.hrm_sys_sp_get_AllSettingByKey,UserLogin, ref status).FirstOrDefault();
                if (rsScanMultiConfig != null)
                {
                    rsScanMulti = rsScanMultiConfig.Value1 != null ? int.Parse(rsScanMultiConfig.Value1.ToString()) : 0;
                    dateStart = dateStart.Date.AddHours(rsScanMulti);
                    dateEnd = dateEnd.Date.AddDays(1).AddHours(rsScanMulti).AddMilliseconds(-1);
                }
                #endregion
                

                var tamScanLogs = repoLau_TamScanLog
                    .FindBy(s => dateStart <= s.TimeLog && s.TimeLog <= dateEnd)
                    .Select(s => new { s.Code, s.TimeLog, s.ID, s.MachineCode })
                    //.Select(s => new { s.MachineCode, s.CardCode, s.TimeLog, s.Id })
                    .ToList();
                var cardCodes = tamScanLogs.Select(s => s.Code).ToList();
                //strIDs = Common.DotNetToOracle(strIDs);
                //var profiles = GetData<Hre_ProfileEntity>(strIDs, ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status)
                //    .Where(s => cardCodes.Contains(s.CodeAttendance) && lstProfileIDs.Contains(s.ID))
                var profiles = lstProfileIDs
                    .Where(s => cardCodes.Contains(s.CodeAttendance))
                    .Select(s => new { s.ID, s.CodeAttendance, s.CodeEmp, s.ProfileName, s.OrgStructureName }).ToList();
                tamScanLogs = tamScanLogs
                    .Where(s => profiles.Select(p => p.CodeAttendance).Contains(s.Code))
                    .OrderBy(s => s.Code)
                    .ThenBy(s => s.TimeLog)
                    .ToList();

                #region lấy line-locker-marker và xử lý khi tìm kiếm
                var lines = repoLau_Line.FindBy(s => s.IsDelete == null).ToList();
                var Lockers = repoLau_Locker.FindBy(s => s.IsDelete == null).ToList();
                var Markers = repoLau_Marker.FindBy(s => s.IsDelete == null).ToList();
                if (_line != null)
                {
                    List<Guid> lstLine = _line.Split(',').Select(Guid.Parse).ToList();
                    lines = lines.Where(s => lstLine.Contains(s.ID)).ToList();
                }
                if (_locker != null)
                {
                    List<Guid> lstLocker = _locker.Split(',').Select(Guid.Parse).ToList();
                    Lockers = Lockers.Where(s => lstLocker.Contains(s.ID)).ToList();
                }
                if (_marker != null)
                {
                    List<Guid> lstMarker = _marker.Split(',').Select(Guid.Parse).ToList();
                    Markers = Markers.Where(s => lstMarker.Contains(s.ID)).ToList();
                }
                #endregion

                List<object> lstObj = new List<object>();
                lstObj.Add(dateStart);
                lstObj.Add(dateEnd);
                List<Lau_LaundryRecordCheckEntity> laundryCheckList = GetData<Lau_LaundryRecordCheckEntity>(lstObj, ConstantSql.hrm_lau_sp_get_LaundryRecord_ByDateFromDateTo, UserLogin, ref status);

                LMS_LaundryRecordEntity laundry = new LMS_LaundryRecordEntity();
                List<LMS_LaundryRecordEntity> lstRecord = new List<LMS_LaundryRecordEntity>();
                List<LMS_LaundryRecord> lstAdd = new List<LMS_LaundryRecord>();
                List<LMS_LaundryRecord> lstEdit = new List<LMS_LaundryRecord>();
                List<Guid> lstDuplicateTamScan = new List<Guid>();
                foreach (var tamScanLog in tamScanLogs)
                {
                    if (lstDuplicateTamScan.Contains(tamScanLog.ID))
                        continue;

                    laundry = new LMS_LaundryRecordEntity();
                    var profile = profiles.FirstOrDefault(s => s.CodeAttendance == tamScanLog.Code);
                    var line = lines.FirstOrDefault(s => tamScanLog.MachineCode == s.MachineCode);

                    if (profile != null && line != null)
                    {
                        #region xử lý bản ghi trùng 
                        var tamScanLogProfiles = tamScanLogs
                            .Where(s => s.Code == tamScanLog.Code && s.TimeLog.Value.Date == tamScanLog.TimeLog.Value.Date && s.MachineCode == tamScanLog.MachineCode)
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
                                if (tamsanNear.TimeLog.Value.Subtract(tamsanFirst.TimeLog.Value).TotalMinutes < rsScanMulti && tamsanFirst.ID != tamsanNear.ID)
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
                        laundry.ProfileID = profile.ID;
                        laundry.ProfileName = profile.ProfileName;
                        laundry.CodeAttendance = profile.CodeAttendance;
                        laundry.TimeLog = tamScanLog.TimeLog;
                        laundry.Status = HRM.Infrastructure.Utilities.LaundryRecordStatus.E_SUBMIT.ToString();
                        laundry.Type = LaundryRecordType.E_AUTO.ToString();
                        if (tamScanLog.MachineCode != null)
                        {
                            laundry.MachineCode = tamScanLog.MachineCode;
                            if (line != null)
                            {
                                laundry.LineLMSName = line.LineLMSName;
                                laundry.LineID = line.ID;
                                laundry.Amount = (decimal)line.Amount;

                                if (line.LockerID != null)
                                {
                                    var locker = Lockers.Where(c => c.ID == line.LockerID).FirstOrDefault();
                                    laundry.LockerLMSName = locker.LockerLMSName;
                                    laundry.LockerID = locker.ID;
                                }
                                if (line.MarkerID != null)
                                {
                                    var marker = Markers.Where(c => c.ID == line.MarkerID).FirstOrDefault();
                                    laundry.MarkerName = marker.MarkerName;
                                    laundry.MarkerID = marker.ID;
                                }


                            }
                        }

                        laundry.DateCreate = DateTime.Now;
                        var recordAdd = laundry.CopyData<LMS_LaundryRecord>();
                        var CheckList = laundryCheckList.Where(s => s.ProfileID == laundry.ProfileID.Value && s.TimeLog == laundry.TimeLog).ToList();
                        if (CheckList != null)
                        {
                            if (CheckList.Count() > 0)
                            {
                                var idUpdate = CheckList.FirstOrDefault();
                                recordAdd.ID = idUpdate.ID;
                                repoLau_LaundryRecord.Edit(recordAdd);
                                //lstEdit.Add(recordAdd);
                            }
                            else
                            {
                                recordAdd.ID = Guid.NewGuid();
                                repoLau_LaundryRecord.Add(recordAdd);
                                //lstAdd.Add(recordAdd);
                            }
                        }
                        laundry.ID = tamScanLog.ID;
                        lstRecord.Add(laundry);
                        #endregion
                    }
                }
                repoLau_LaundryRecord.SaveChanges();
                //if (lstAdd.Any())
                //{
                //    repoLau_LaundryRecord.Add(lstAdd);
                //    repoLau_LaundryRecord.SaveChanges();
                //}
                //if (lstEdit.Any())
                //{
                //    repoLau_LaundryRecord.Edit(lstEdit);
                //    repoLau_LaundryRecord.SaveChanges();
                //}
                if (lstRecord.Count > 0)
                {
                    Sys_AllSetting sys = new Sys_AllSetting();
                    Sys_AllSetting rs = GetData<Sys_AllSetting>(AppConfig.HRM_LAU_LAUNDRYRECORD_SUMMARY.ToString(), ConstantSql.hrm_sys_sp_get_AllSettingByKey,UserLogin, ref status).FirstOrDefault();

                    sys.Value1 = dateStart.ToString(ConstantFormat.HRM_Format_YearMonthDay_HoursMinSecond.ToString());
                    sys.Value2 = dateEnd.ToString(ConstantFormat.HRM_Format_YearMonthDay_HoursMinSecond.ToString());
                    sys.Name = AppConfig.HRM_LAU_LAUNDRYRECORD_SUMMARY.ToString();

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


                return lstRecord;
            }
        }

    }
}
