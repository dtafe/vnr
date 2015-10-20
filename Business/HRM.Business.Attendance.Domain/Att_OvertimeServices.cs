using HRM.Data.Attendance.Data;
using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
//using HRM.Data.Attendance.Model;
using System.Linq;
//using HRM.Data.Main.Sql;
using HRM.Business.Attendance.Models;
using System;
using HRM.Business.Category.Models;
using System.Collections.Generic;
using System.Reflection;
using VnResource.Helper.Data;
using HRM.Infrastructure.Utilities;
using HRM.Data.Attendance.Data.Sql.Repositories;
using HRM.Infrastructure.Utilities.Helpers;
using System.Collections;
using HRM.Business.BaseModel;
using HRM.Business.Main.Domain;
using HRM.Business.HrmSystem.Models;
//using HRM.Data.HrmSystem.Model;
//
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using HRM.Business.Hr.Models;
using VnResource.Helper.Linq;
using System.Linq.Expressions;
using System.Data;

namespace HRM.Business.Attendance.Domain
{
    public class Att_OvertimeServices : BaseService
    {
        public static List<Att_OvertimeEntity> lstOvertimeCache = new List<Att_OvertimeEntity>();

        public static string getStatusOTApplyCalcPayroll(string userLogin)
        {
            BaseService _baseService = new BaseService();
            string status = string.Empty;
            var key = AppConfig.HRM_ATT_OT_OVERTIMESTATUS.ToString();
            var lstConfig = _baseService.GetData<Sys_AllSettingEntity>(key, ConstantSql.hrm_sys_sp_get_AllSettingByKey, userLogin, ref status);
            if (lstConfig != null)
            {
                var temp = lstConfig.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(temp.Value1) && !string.IsNullOrEmpty(temp.Value1))
                {
                    return temp.Value1;
                }
                return EnumDropDown.OverTimeStatus.E_APPROVED.ToString();
            }
            return EnumDropDown.OverTimeStatus.E_APPROVED.ToString();

            //string type = AppConfig.E_RANGE_SALARY_MONTH.ToString();
            //Sys_AppConfig objAppConfig = EntityService.Instance.GetEntity<Sys_AppConfig>(context, userid, ui => ui.Info == type);
            ////Get status ot tinh luong
            //string statusOTCalcBasicSalary = objAppConfig.Value8 == null ? OverTimeStatus.E_APPROVED.ToString() : objAppConfig.Value8;
            //return statusOTCalcBasicSalary;
        }

        public bool checkDuplidate_Overtime(List<Att_OvertimeEntity> lstOvertime, string userLogin)
        {
            lstOvertime = lstOvertime.OrderBy(s => s.WorkDate).ToList();

            List<Guid> lstPro = lstOvertime.Select(s => s.ProfileID).Distinct().ToList();
            string status = string.Empty;
            var key = AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY.ToString();
            double configHours = 0.0;
            var lstSysAllSetting = GetData<Sys_AllSettingEntity>(key, ConstantSql.hrm_sys_sp_get_AllSettingByKey, userLogin, ref status);
            if (lstSysAllSetting != null)
            {
                var temp = lstSysAllSetting.Select(s => s.Value1).FirstOrDefault();
                if (temp != null)
                {
                    configHours = Double.Parse(temp.ToString());
                }
            }
            #region code cũ
            //List<object> lstParam = new List<object>();
            //lstParam.Add(null);
            //lstParam.Add(lstOvertime[0].WorkDate.Date);
            //lstParam.Add(lstOvertime[lstOvertime.Count - 1].WorkDate.AddHours(configHours));
            //var lstDataOvertime = GetData<Att_OvertimeEntity>(lstParam, ConstantSql.hrm_att_getdata_Overtime, ref status);
            //lstDataOvertime = lstDataOvertime.Where(s => lstPro.Contains(s.ProfileID)).ToList(); 
            #endregion

            List<object> lstParam = new List<object>();
            lstParam.Add(null);
            lstParam.Add(lstOvertime[0].WorkDate.Date);
            lstParam.Add(lstOvertime[lstOvertime.Count - 1].WorkDate.AddHours(configHours));
            var lstDataOvertime = GetData<Att_OvertimeEntity>(lstParam, ConstantSql.hrm_att_getdata_Overtime, userLogin, ref status);
            lstDataOvertime = lstDataOvertime.Where(s => lstPro.Contains(s.ProfileID)).ToList();

            List<Att_OvertimeEntity> lstCheck = new List<Att_OvertimeEntity>();
            foreach (var item in lstOvertime)
            {
                lstCheck = lstDataOvertime.Where(s => s.ProfileID == item.ProfileID
                    && s.WorkDate < item.WorkDate.AddHours(item.RegisterHours)
                    && item.WorkDate < s.WorkDate.AddHours(s.RegisterHours)).ToList();
                if (lstCheck.Count > 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// chuyển thành trạng thái Sumit
        /// </summary>
        /// <returns></returns>
        public void SubmitOvertime(List<Guid> ids)
        {
            using (var context = new VnrHrmDataContext())
            {
                var lstWorkday = new List<Att_OvertimeEntity>();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Att_OvertimeRepository(unitOfWork);
                lstWorkday = repo.FindBy(m => ids.Contains(m.ID)).ToList().Translate<Att_OvertimeEntity>();
                foreach (var workday in lstWorkday)
                {
                    workday.Status = AttendanceDataStatus.E_SUBMIT.ToString();
                    repo.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Lưu xác nhận tăng ca
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public string SaveOvertimeConfirm(IEnumerable<Att_OvertimeEntity> listOT)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Att_OvertimeRepository(unitOfWork);
                try
                {
                    foreach (var item in listOT)
                    {
                        repo.SaveOvertimeConfirm(item.ID, item.ConfirmHours);
                    }
                    repo.SaveChanges();
                    return "0";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        #region Hàm XÁc Nhận Tăng Ca
        public List<Att_OvertimeEntity> GetOvertimeConfirm(double? RegisterFrom, double? RegisterTo, double? ApproveFrom, double? ApproveTo, DateTime? DateS, DateTime? DateE, List<Hre_ProfileEntity> listProfile, List<Guid?> lstOTType, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Overtime = new CustomBaseRepository<Att_Overtime>(unitOfWork);
                var repoCat_OvertimeType = new CustomBaseRepository<Cat_OvertimeType>(unitOfWork);

                string waitStatus = ProfileStatusSyn.E_WAITING.ToString();
                string E_APPROVED = OverTimeStatus.E_APPROVED.ToString();
                string E_CONFIRM = OverTimeStatus.E_CONFIRM.ToString();
                string HRM_ATT_ROUNDOT = AppConfig.HRM_ATT_ROUNDOT.ToString();
                string HRM_ATT_ROUNDOT_LINEROUND = AppConfig.HRM_ATT_ROUNDOT_LINEROUND.ToString();

                DateTime DateStart = DateS.Value;
                DateTime DateEnd = DateE.Value.Date.AddDays(1).AddMilliseconds(-1);
                if (DateStart == null || DateEnd == null)
                    return new List<Att_OvertimeEntity>();

                listProfile = listProfile.Where(m => m.StatusSyn != waitStatus && (m.DateQuit == null || m.DateQuit.Value > DateStart)).ToList();

                var lstOvertimeQuery = repoAtt_Overtime
                    .FindBy(m => m.IsDelete == null && m.WorkDateRoot != null && m.WorkDateRoot >= DateStart && m.WorkDateRoot <= DateEnd && (m.Status == E_APPROVED || m.Status == E_CONFIRM)).ToList();

                var lstOvertimeType = repoCat_OvertimeType
                    .FindBy(m => m.IsDelete == null)
                    .Select(o => new { o.ID, o.OvertimeTypeName })
                    .ToList();

                if (lstOTType == null)
                {
                    lstOTType = new List<Guid?>();

                    var listGuid = lstOvertimeType.Select(o => o.ID).ToList();
                    foreach (var item in listGuid)
                    {
                        lstOTType.Add((Guid?)item);
                    }
                }
                if (lstOTType.Count > 0)
                {
                    lstOvertimeQuery = lstOvertimeQuery.Where(m => lstOTType.Contains(m.OvertimeTypeID)).ToList();
                }
                if (RegisterFrom != null)
                {
                    double HourRegistStart = RegisterFrom.Value;
                    if (HourRegistStart >= 0)
                    {
                        lstOvertimeQuery = lstOvertimeQuery.Where(m => m.RegisterHours >= HourRegistStart).ToList();
                    }
                }
                if (RegisterTo != null)
                {
                    double HourRegistEnd = RegisterTo.Value;
                    if (HourRegistEnd > 0)
                    {
                        lstOvertimeQuery = lstOvertimeQuery.Where(m => m.RegisterHours <= HourRegistEnd).ToList();
                    }
                }
                if (ApproveFrom != null)
                {
                    double HourApproveStart = ApproveFrom.Value;
                    if (HourApproveStart > 0)
                    {
                        lstOvertimeQuery = lstOvertimeQuery.Where(m => m.ApproveHours != null && m.ApproveHours >= HourApproveStart).ToList();
                    }
                }
                if (ApproveTo != null)
                {
                    double HourApproveTo = ApproveTo.Value;
                    if (HourApproveTo > 0)
                    {
                        lstOvertimeQuery = lstOvertimeQuery.Where(m => m.ApproveHours != null && m.ApproveHours <= HourApproveTo).ToList();
                    }
                }

                var profiles = listProfile
                    .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();

                List<Guid> lstprofileID = listProfile.Select(s => s.ID).ToList();
                lstOvertimeQuery = lstOvertimeQuery.Where(m => lstprofileID.Contains(m.ProfileID)).ToList();

                var lstOvertime = lstOvertimeQuery.ToList().Translate<Att_OvertimeEntity>();

                string status = string.Empty;
                List<object> lstOb = new List<object>();
                lstOb.Add(HRM_ATT_ROUNDOT);
                lstOb.Add(null);
                lstOb.Add(null);
                var config = GetData<Sys_AllSettingEntity>(lstOb, ConstantSql.hrm_sys_sp_get_AllSetting,userLogin, ref status);

                var configSTANDARD = config.Where(s => s.Name == HRM_ATT_ROUNDOT).FirstOrDefault();
                var configLINEROUND = config.Where(s => s.Name == HRM_ATT_ROUNDOT_LINEROUND).FirstOrDefault();
                DateTime? DateMax = lstOvertime.Where(m => m.WorkDateRoot != null).OrderByDescending(m => m.WorkDateRoot).Select(m => m.WorkDateRoot.Value).FirstOrDefault();
                DateTime? DateMin = lstOvertime.Where(m => m.WorkDateRoot != null).OrderBy(m => m.WorkDateRoot).Select(m => m.WorkDateRoot.Value).FirstOrDefault();
                var lstWorkday = new List<Att_Workday>().Select(m => new { m.ProfileID, m.WorkDate, m.ShiftApprove, m.ShiftID, m.InTime1, m.OutTime1 }).ToList();
                if (DateMax != null && DateMin != null)
                {
                    lstWorkday = unitOfWork.CreateQueryable<Att_Workday>(m => m.WorkDate >= DateMin.Value && m.WorkDate <= DateMax.Value && lstprofileID.Contains(m.ProfileID))
                        .Select(m => new { m.ProfileID, m.WorkDate, m.ShiftApprove, m.ShiftID, m.InTime1, m.OutTime1 }).ToList();
                }
                var lstShift = unitOfWork.CreateQueryable<Cat_Shift>().Select(m => new { m.ID, m.InTime, m.CoBreakIn, m.CoBreakOut, m.CoOut, m.InOutDynamic }).ToList();


                foreach (var item in lstOvertime)
                {
                    if (item.ProfileID != null)
                    {
                        var profile = profiles.FirstOrDefault(s => s.ID == item.ProfileID);
                        item.ProfileName = profile != null ? profile.ProfileName : string.Empty;
                        item.CodeEmp = profile != null ? profile.CodeEmp : string.Empty;
                    }
                    if (item.OvertimeTypeID != null)
                    {
                        var overtimeType = lstOvertimeType.FirstOrDefault(s => s.ID == item.OvertimeTypeID);
                        item.OvertimeTypeName = overtimeType != null ? overtimeType.OvertimeTypeName : string.Empty;
                    }
                    var Workday = lstWorkday.Where(m => m.ProfileID == item.ProfileID && m.WorkDate == item.WorkDateRoot.Value).FirstOrDefault();
                    if (Workday == null || Workday.InTime1 == null || Workday.OutTime1 == null)
                        continue;
                    Guid? ShiftID = Workday.ShiftApprove ?? Workday.ShiftID;


                    DateTime BeginOt = item.WorkDate;
                    DateTime endOT = item.WorkDate.AddHours(item.RegisterHours);

                    if (ShiftID != null && item.DurationType != HRM.Infrastructure.Utilities.EnumDropDown.OvertimeDurationType.E_OT_UNLIMIT.ToString())
                    {
                        var shift = lstShift.Where(m => m.ID == ShiftID.Value).FirstOrDefault();
                        if (shift != null)
                        {
                            DateTime BeginShift = item.WorkDateRoot.Value;
                            BeginShift = BeginShift.Add(shift.InTime.TimeOfDay);
                            DateTime EndShift = BeginShift.AddHours(shift.CoOut);
                            if (shift.InOutDynamic != null && shift.InOutDynamic > 0)
                            {
                                double Late = (Workday.InTime1.Value - BeginShift).TotalHours;
                                double delta = Late > shift.InOutDynamic.Value ? shift.InOutDynamic.Value : Late;
                                EndShift = EndShift.AddHours(delta);
                            }

                            if (BeginOt >= BeginShift && endOT <= EndShift)
                            {
                                BeginOt = item.WorkDateRoot.Value;
                                endOT = item.WorkDateRoot.Value;
                            }
                            if (BeginOt < BeginShift && endOT > BeginShift)
                            {
                                endOT = BeginShift;
                            }
                            if (BeginOt < EndShift && endOT > EndShift)
                            {
                                BeginOt = EndShift;
                            }
                        }
                    }
                    DateTime In = Workday.InTime1.Value;
                    DateTime Out = Workday.OutTime1.Value;
                    DateTime Min = DateTime.MinValue;
                    DateTime Max = DateTime.MaxValue;
                    if (In > BeginOt)
                    {
                        Min = In;
                    }
                    else
                    {
                        Min = BeginOt;
                    }

                    if (Out > endOT)
                    {

                        Max = endOT;
                    }
                    else
                    {
                        Max = Out;
                    }
                    item.ConfirmHours = (Max - Min).TotalHours;
                    //item.Status = OverTimeStatus.E_CONFIRM.ToString();
                    if (config != null && configSTANDARD.Value1 != null && configSTANDARD.Value1 == bool.TrueString && configLINEROUND.Value1 != null)
                    {
                        double LineRound = 0;
                        double.TryParse(configLINEROUND.Value1.ToString(), out LineRound);
                        double value = Common.RoundMinuteDown(item.ConfirmHours * 60, LineRound);
                        item.ConfirmHours = value / 60;

                    }
                }
                return lstOvertime;
            }
        }

        #endregion

        #region Lấy Dữ Liệu Phân Tích Tăng Ca (Lấy Tất Cả Data)
        public DataTable LoadDataAnalyzeOvertime(string strOrgStructure, DateTime DateStart, DateTime DateEnd, List<Hre_ProfileEntity> lstProfile, List<string> lstTypeData, Att_OvertimeInfoFillterAnalyze _OvertimeInfoFillterAnalyzeEntity, string userLogin)
        {
            DataTable table = GetSchemaExportExcel();
            string status = string.Empty;

            DateStart = DateStart.Date;
            DateEnd = DateEnd.Date.AddDays(1).AddMilliseconds(-1);
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_WorkDay = new CustomBaseRepository<Att_Workday>(unitOfWork);
                var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);
                var repoCat_ShiftItem = new CustomBaseRepository<Cat_ShiftItem>(unitOfWork);
                var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);
                var repoCat_OvertimeType = new CustomBaseRepository<Cat_OvertimeType>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_OrgStructureType = new CustomBaseRepository<Cat_OrgStructureType>(unitOfWork);

                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                var orgs = repoCat_OrgStructure.FindBy(s => s.IsDelete == null && s.Code != null).ToList();

                List<WorkdayCustom> lstWorkday = new List<WorkdayCustom>();
                List<Guid> lstProfileId = lstProfile.Select(s => s.ID).ToList();

                List<object> lst3ParamSE = new List<object>();
                lst3ParamSE.Add(strOrgStructure);
                lst3ParamSE.Add(DateStart);
                lst3ParamSE.Add(DateEnd);
                if (lstProfile.Count > 0 && strOrgStructure == null)
                {
                    var lstWD = GetData<Att_Workday>(lst3ParamSE, ConstantSql.hrm_att_getdata_Workday, userLogin, ref status);
                    //var lstWD = repoAtt_WorkDay.FindBy(s => s.IsDelete == null && s.WorkDate >= DateStart && s.WorkDate <= DateEnd && lstProfileId.Contains(s.ProfileID)).ToList();
                    if (lstWD.Count > 0)
                    {
                        lstWD = lstWD.Where(s => lstProfileId.Contains(s.ProfileID)).ToList();
                        lstWorkday = lstWD.Translate<WorkdayCustom>();
                    }
                }
                else
                {
                    lstWorkday = GetData<WorkdayCustom>(lst3ParamSE, ConstantSql.hrm_att_getdata_Workday, userLogin, ref status).ToList();
                }

                //var lstWorkdayQuery = repoAtt_WorkDay.FindBy(m => m.IsDelete == null && m.WorkDate >= DateStart && m.WorkDate <= DateEnd);
                //if (lstProfileId.Count > 0)
                //{
                //    lstWorkdayQuery = lstWorkdayQuery.Where(m => lstProfileId.Contains(m.ProfileID));
                //}
                //lstWorkday = lstWorkdayQuery.ToList().Translate<Att_WorkdayEntity>();
                List<Cat_ShiftEntity> lstShift = repoCat_Shift
                    .GetAll().ToList()
                    .Translate<Cat_ShiftEntity>();
                List<Cat_ShiftItemEntity> lstShiftItem = repoCat_ShiftItem
                    .FindBy(s => s.IsDelete == null)
                    .ToList()
                    .Translate<Cat_ShiftItemEntity>();
                List<Cat_DayOffEntity> LstDayOff = repoCat_DayOff
                    .FindBy(m => m.IsDelete == null && m.DateOff >= DateStart && m.DateOff < DateEnd)
                    .ToList()
                    .Translate<Cat_DayOffEntity>();

                List<Cat_OvertimeTypeEntity> lstOvertimeType = repoCat_OvertimeType
                    .FindBy(s => s.IsDelete == null).ToList()
                    .Translate<Cat_OvertimeTypeEntity>();

                string E_CANCEL = OverTimeStatus.E_CANCEL.ToString();
                string E_REJECTED = OverTimeStatus.E_REJECTED.ToString();
                DateTime beforeDateStart = DateStart.Date.AddDays(-1);
                DateTime afterDateEnd = DateEnd.Date.AddDays(2).AddMinutes(-1);
                // string E_HOLIDAY_HLD = HolidayType.E_HOLIDAY_HLD.ToString();
                List<DateTime> lstHoliday = repoCat_DayOff
                    .FindBy(s => s.IsDelete == null)
                    .Select(m => m.DateOff).ToList<DateTime>();
                //lstWorkday = filterData_NonAllowOT_inGrade(lstWorkday);
                List<Att_OvertimeEntity> lstOvertimeCal = AnalyzeOvertime(lstWorkday, lstShift, lstShiftItem, LstDayOff, lstOvertimeType, _OvertimeInfoFillterAnalyzeEntity, userLogin);
                if (lstOvertimeCal.Count == 0)
                {
                    lstOvertimeCache = lstOvertimeCal;
                    return table;
                }
                OvertimePermitEntity overtimePermit = getOvertimePermit(userLogin);
                FilterNonOvertimeByGradeConfig(lstOvertimeCal);
                SetNonOT(lstOvertimeCal);
                SetStatusOvertimeOnWorkday(lstOvertimeCal);
                FilterOvertimeByMaxHourPerDay(lstOvertimeCal, overtimePermit, _OvertimeInfoFillterAnalyzeEntity.MaximumOvertimeHour);
                RoundOT(lstOvertimeCal, userLogin);
                FillterAllowOvertime(context, lstOvertimeCal, overtimePermit, lstWorkday);
                RoundOT(lstOvertimeCal, userLogin);
                lstOvertimeCal = lstOvertimeCal.Where(m => m.RegisterHours != null && m.RegisterHours > 0).ToList();
                SetStatusLeaveOnWorkday(lstOvertimeCal);//Set loại ngày nghỉ cho OT

                if (lstTypeData.Count > 0)
                {
                    Expression<Func<Att_OvertimeEntity, bool>> predicate = VnResource.Helper.Linq.PredicateBuilder.False<Att_OvertimeEntity>();
                    if (lstTypeData.Any(m => m == ComputeLeavedayType.E_DATA_LEAVE.ToString()))
                    {
                        Expression<Func<Att_OvertimeEntity, bool>> predicate1 = m => m.udLeaveTypeCode != null || m.udLeaveTypeCode2 != null;
                        predicate = predicate.Or(predicate1);
                        //lstOvertimeCal = lstOvertimeCal.Where(m => (m.udLeaveTypeCode == null || m.udLeaveTypeCode == string.Empty) && (m.udLeaveTypeCode2 == null || m.udLeaveTypeCode2 == string.Empty)).ToList();
                    }
                    if (lstTypeData.Any(m => m == ComputeLeavedayType.E_DATA_NON_LEAVE.ToString()))
                    {
                        Expression<Func<Att_OvertimeEntity, bool>> predicate2 = m => (m.udLeaveTypeCode == null || m.udLeaveTypeCode == string.Empty) && (m.udLeaveTypeCode2 == null || m.udLeaveTypeCode2 == string.Empty);
                        predicate = predicate.Or(predicate2);
                        //lstOvertimeCal = lstOvertimeCal.Where(m => m.udLeaveTypeCode != null || m.udLeaveTypeCode2 != null).ToList();
                    }
                    if (lstTypeData.Any(m => m == ComputeOvertimeType.E_DATA_OT.ToString()))
                    {
                        Expression<Func<Att_OvertimeEntity, bool>> predicate2 = m => m.udOvertimeStatus != null;
                        predicate = predicate.Or(predicate2);
                    }
                    if (lstTypeData.Any(m => m == ComputeOvertimeType.E_DATA_NON_OT.ToString()))
                    {
                        Expression<Func<Att_OvertimeEntity, bool>> predicate2 = m => m.udOvertimeStatus == null;
                        predicate = predicate.Or(predicate2);
                    }
                    lstOvertimeCal = lstOvertimeCal.AsQueryable().Where(predicate).ToList();
                }

                lstOvertimeCal = lstOvertimeCal.OrderBy(m => m.WorkDate.Date).ThenBy(m => m.ProfileID).ToList();
                //lstOvertimeCache = lstOvertimeCal;
                //BindToGrid(lstOvertimeCache);

                #region process return table

                foreach (var item in lstOvertimeCal)
                {
                    #region code cu tra ve EntityModel
                    //if ((item.udIsLimitHour == null || item.udIsLimitHour == false)
                    //       && (item.udIsLimitHourLv1 == null || item.udIsLimitHourLv1 == false)
                    //       && (item.udIsLimitHourLv2 == null || item.udIsLimitHourLv2 == false))
                    //{
                    //    item.IsValid = true;
                    //}
                    //else
                    //{
                    //    item.IsValid = false;
                    //}

                    //if (item.OvertimeTypeID != Guid.Empty)
                    //{
                    //    item.OvertimeTypeName = lstOvertimeType.Where(s => s.ID == item.OvertimeTypeID).FirstOrDefault().OvertimeTypeName;
                    //}
                    //if (item.ProfileID != Guid.Empty)
                    //{
                    //    var temp = lstProfile.Where(s => s.ID == item.ProfileID).FirstOrDefault();
                    //    item.ProfileName = temp.ProfileName;
                    //    item.CodeEmp = temp.CodeEmp;
                    //    item.OrgStructureName = temp.OrgStructureName;
                    //}
                    //if (item.ShiftID != Guid.Empty)
                    //{
                    //    item.ShiftName = lstShift.Where(s => s.ID == item.ShiftID).FirstOrDefault().ShiftName;
                    //}
                    //item.TotalRow = lstOvertimeCal.Count; 
                    #endregion

                    DataRow row = table.NewRow();
                    Guid? orgId = lstProfile.Where(s => s.ID == item.ProfileID).FirstOrDefault().OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.ID == orgId);
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    row[Att_OvertimeEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.OrgStructureCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.OrgName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;

                    row[Att_OvertimeEntity.FieldNames.WorkDate] = item.WorkDate;
                    row[Att_OvertimeEntity.FieldNames.InTime] = item.InTime;
                    row[Att_OvertimeEntity.FieldNames.OutTime] = item.OutTime;

                    if (item.udIsLimitHour == true || item.udIsLimitHourLv1 == true || item.udIsLimitHourLv2 == true)
                    {
                        if (item.udIsLimitHourLv2 == true)
                        {
                            row[Att_OvertimeEntity.FieldNames.Valid] = "Level3";
                        }
                        else if (item.udIsLimitHourLv1 == true)
                        {
                            row[Att_OvertimeEntity.FieldNames.Valid] = "Level2";
                        }
                        else if (item.udIsLimitHour == true)
                        {
                            row[Att_OvertimeEntity.FieldNames.Valid] = "Level1";
                        }
                    }
                    else
                    {
                        row[Att_OvertimeEntity.FieldNames.Valid] = string.Empty;
                    }

                    if (item.OvertimeTypeID != Guid.Empty)
                    {
                        var OTType = lstOvertimeType.Where(s => s.ID == item.OvertimeTypeID).FirstOrDefault();
                        row[Att_OvertimeEntity.FieldNames.OvertimeTypeID] = item.OvertimeTypeID;
                        row[Att_OvertimeEntity.FieldNames.OvertimeTypeName] = OTType.OvertimeTypeName;

                        row[OTType.Code] = item.RegisterHours;
                        row[OTType.Code + "Confirm"] = item.ApproveHours ?? 0.0;
                    }
                    if (item.ShiftID != Guid.Empty)
                    {
                        var _shift = lstShift.Where(s => s.ID == item.ShiftID).FirstOrDefault();
                        row[Att_OvertimeEntity.FieldNames.ShiftID] = item.ShiftID;
                        row[Att_OvertimeEntity.FieldNames.ShiftName] = _shift.ShiftName;
                    }
                    if (item.ProfileID != Guid.Empty)
                    {
                        var temp = lstProfile.Where(s => s.ID == item.ProfileID).FirstOrDefault();
                        row[Att_OvertimeEntity.FieldNames.ProfileID] = item.ProfileID;
                        row[Att_OvertimeEntity.FieldNames.ProfileName] = temp.ProfileName;
                        row[Att_OvertimeEntity.FieldNames.CodeEmp] = temp.CodeEmp;
                        //row[Att_ReportDetailOvertimeEntity.FieldNames.OrgStructureName] = temp.OrgStructureName;
                    }
                    row[Att_OvertimeEntity.FieldNames.TotalRow] = lstOvertimeCal.Count;
                    row[Att_OvertimeEntity.FieldNames.DateExport] = DateTime.Now;
                    row[Att_OvertimeEntity.FieldNames.AnalyseHour] = item.AnalyseHour;
                    row[Att_OvertimeEntity.FieldNames.udHourByDate] = item.udHourByDate;
                    row[Att_OvertimeEntity.FieldNames.udHourByWeek] = item.udHourByWeek;
                    row[Att_OvertimeEntity.FieldNames.udHourByMonth] = item.udHourByMonth;
                    row[Att_OvertimeEntity.FieldNames.udHourByYear] = item.udHourByYear;
                    row[Att_OvertimeEntity.FieldNames.udLeaveTypeCode] = item.udLeaveTypeCode;
                    row[Att_OvertimeEntity.FieldNames.udOvertimeStatus] = item.udOvertimeStatus;
                    row[Att_OvertimeEntity.FieldNames.RegisterHours] = item.RegisterHours;
                    row[Att_OvertimeEntity.FieldNames.ApproveHours] = item.RegisterHours;
                    table.Rows.Add(row);
                }
                #endregion

                return table;

            }
        }

        #endregion

        #region Lấy Dữ Liệu Phân Tích Tăng Ca Hợp Lệ
        public DataTable LoadDataAnalyzeOvertime_DataNotLimit(string strOrgStructure, DateTime DateStart, DateTime DateEnd, List<Hre_ProfileEntity> lstProfile, List<string> lstTypeData, Att_OvertimeInfoFillterAnalyze _OvertimeInfoFillterAnalyzeEntity, string userLogin)
        {
            DataTable table = GetSchemaExportExcel();
            string status = string.Empty;

            DateStart = DateStart.Date;
            DateEnd = DateEnd.Date.AddDays(1).AddMilliseconds(-1);
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_WorkDay = new Att_WorkDayRepository(unitOfWork);
                var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                var repoCat_ShiftItem = new Cat_ShiftItemRepository(unitOfWork);
                var repoCat_DayOff = new Cat_DayOffRepository(unitOfWork);
                var repoCat_OvertimeType = new Cat_OvertimeTypeRepository(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_OrgStructureType = new CustomBaseRepository<Cat_OrgStructureType>(unitOfWork);

                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                var orgs = repoCat_OrgStructure.FindBy(s => s.IsDelete == null && s.Code != null).ToList();

                List<WorkdayCustom> lstWorkday = new List<WorkdayCustom>();
                List<Guid> lstProfileId = lstProfile.Select(s => s.ID).ToList();

                List<object> lst3ParamSE = new List<object>();
                lst3ParamSE.Add(strOrgStructure);
                lst3ParamSE.Add(DateStart);
                lst3ParamSE.Add(DateEnd);
                if (lstProfile.Count > 0 && strOrgStructure == null)
                {
                    var lstWD = repoAtt_WorkDay.FindBy(s => s.IsDelete == null && s.WorkDate >= DateStart && s.WorkDate <= DateEnd && lstProfileId.Contains(s.ProfileID)).ToList();
                    if (lstWD.Count > 0)
                    {
                        lstWorkday = lstWD.Translate<WorkdayCustom>();
                    }
                }
                else
                {
                    lstWorkday = GetData<WorkdayCustom>(lst3ParamSE, ConstantSql.hrm_att_getdata_Workday, userLogin, ref status).ToList();
                }
                //var lstWorkdayQuery = repoAtt_WorkDay.FindBy(m => m.IsDelete == null && m.WorkDate >= DateStart && m.WorkDate <= DateEnd);
                //if (lstProfileId.Count > 0)
                //{
                //    lstWorkdayQuery = lstWorkdayQuery.Where(m => lstProfileId.Contains(m.ProfileID));
                //}
                //lstWorkday = lstWorkdayQuery.ToList().Translate<Att_WorkdayEntity>();
                List<Cat_ShiftEntity> lstShift = repoCat_Shift
                    .FindBy(s => s.IsDelete == null)
                    .ToList()
                    .Translate<Cat_ShiftEntity>();
                List<Cat_ShiftItemEntity> lstShiftItem = repoCat_ShiftItem
                    .FindBy(s => s.IsDelete == null)
                    .ToList()
                    .Translate<Cat_ShiftItemEntity>();
                List<Cat_DayOffEntity> LstDayOff = repoCat_DayOff
                    .FindBy(m => m.IsDelete == null && m.DateOff >= DateStart && m.DateOff < DateEnd)
                    .ToList()
                    .Translate<Cat_DayOffEntity>();
                List<Cat_OvertimeTypeEntity> lstOvertimeType = repoCat_OvertimeType
                    .FindBy(s => s.IsDelete == null).ToList()
                    .Translate<Cat_OvertimeTypeEntity>();

                string E_CANCEL = OverTimeStatus.E_CANCEL.ToString();
                string E_REJECTED = OverTimeStatus.E_REJECTED.ToString();
                DateTime beforeDateStart = DateStart.Date.AddDays(-1);
                DateTime afterDateEnd = DateEnd.Date.AddDays(2).AddMinutes(-1);
                // string E_HOLIDAY_HLD = HolidayType.E_HOLIDAY_HLD.ToString();
                List<DateTime> lstHoliday = repoCat_DayOff
                    .FindBy(s => s.IsDelete == null)
                    .Select(m => m.DateOff).ToList<DateTime>();
                //lstWorkday = filterData_NonAllowOT_inGrade(lstWorkday);
                List<Att_OvertimeEntity> lstOvertimeCal = AnalyzeOvertime(lstWorkday, lstShift, lstShiftItem, LstDayOff, lstOvertimeType, _OvertimeInfoFillterAnalyzeEntity, userLogin);
                if (lstOvertimeCal.Count == 0)
                {
                    lstOvertimeCache = lstOvertimeCal;
                    return table;
                }
                OvertimePermitEntity overtimePermit = getOvertimePermit(userLogin);
                FilterNonOvertimeByGradeConfig(lstOvertimeCal);
                SetNonOT(lstOvertimeCal);
                SetStatusOvertimeOnWorkday(lstOvertimeCal);
                FilterOvertimeByMaxHourPerDay(lstOvertimeCal, overtimePermit, _OvertimeInfoFillterAnalyzeEntity.MaximumOvertimeHour);
                RoundOT(lstOvertimeCal, userLogin);
                FillterAllowOvertime(context, lstOvertimeCal, overtimePermit, lstWorkday);
                RoundOT(lstOvertimeCal, userLogin);
                lstOvertimeCal = lstOvertimeCal.Where(m => m.RegisterHours != null && m.RegisterHours > 0).ToList();
                SetStatusLeaveOnWorkday(lstOvertimeCal);//Set loại ngày nghỉ cho OT

                if (lstTypeData.Count > 0)
                {
                    Expression<Func<Att_OvertimeEntity, bool>> predicate = VnResource.Helper.Linq.PredicateBuilder.False<Att_OvertimeEntity>();
                    if (lstTypeData.Any(m => m == ComputeLeavedayType.E_DATA_LEAVE.ToString()))
                    {
                        Expression<Func<Att_OvertimeEntity, bool>> predicate1 = m => m.udLeaveTypeCode != null || m.udLeaveTypeCode2 != null;
                        predicate = predicate.Or(predicate1);
                        //lstOvertimeCal = lstOvertimeCal.Where(m => (m.udLeaveTypeCode == null || m.udLeaveTypeCode == string.Empty) && (m.udLeaveTypeCode2 == null || m.udLeaveTypeCode2 == string.Empty)).ToList();
                    }
                    if (lstTypeData.Any(m => m == ComputeLeavedayType.E_DATA_NON_LEAVE.ToString()))
                    {
                        Expression<Func<Att_OvertimeEntity, bool>> predicate2 = m => (m.udLeaveTypeCode == null || m.udLeaveTypeCode == string.Empty) && (m.udLeaveTypeCode2 == null || m.udLeaveTypeCode2 == string.Empty);
                        predicate = predicate.Or(predicate2);
                        //lstOvertimeCal = lstOvertimeCal.Where(m => m.udLeaveTypeCode != null || m.udLeaveTypeCode2 != null).ToList();
                    }
                    if (lstTypeData.Any(m => m == ComputeOvertimeType.E_DATA_OT.ToString()))
                    {
                        Expression<Func<Att_OvertimeEntity, bool>> predicate2 = m => m.udOvertimeStatus != null;
                        predicate = predicate.Or(predicate2);
                    }
                    if (lstTypeData.Any(m => m == ComputeOvertimeType.E_DATA_NON_OT.ToString()))
                    {
                        Expression<Func<Att_OvertimeEntity, bool>> predicate2 = m => m.udOvertimeStatus == null;
                        predicate = predicate.Or(predicate2);
                    }
                    lstOvertimeCal = lstOvertimeCal.AsQueryable().Where(predicate).ToList();
                }

                //lstOvertimeCal = lstOvertimeCal.OrderBy(m => m.WorkDate.Date).ThenBy(m => m.ProfileID).ToList();
                //lstOvertimeCache = lstOvertimeCal;
                //BindToGrid(lstOvertimeCache);

                lstOvertimeCal = lstOvertimeCal.Where(m => (m.udIsLimitHour == null || m.udIsLimitHour == false)
                   && (m.udIsLimitHourLv1 == null || m.udIsLimitHourLv1 == false)
                   && (m.udIsLimitHourLv2 == null || m.udIsLimitHourLv2 == false))
                   .OrderBy(m => m.WorkDate.Date)
                   .ThenBy(m => m.ProfileID)
                   .ToList();

                //lstOvertimeCal.ForEach(s => s.IsValid = true);

                #region process return table

                foreach (var item in lstOvertimeCal)
                {
                    DataRow row = table.NewRow();
                    Guid? orgId = lstProfile.Where(s => s.ID == item.ProfileID).FirstOrDefault().OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.ID == orgId);
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    row[Att_OvertimeEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.OrgStructureCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.OrgName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;

                    row[Att_OvertimeEntity.FieldNames.WorkDate] = item.WorkDate;
                    row[Att_OvertimeEntity.FieldNames.InTime] = item.InTime;
                    row[Att_OvertimeEntity.FieldNames.OutTime] = item.OutTime;

                    if (item.udIsLimitHour == null || item.udIsLimitHour == false)
                    {
                        row[Att_OvertimeEntity.FieldNames.Valid] = "Level1";
                    }
                    else if (item.udIsLimitHourLv1 == null || item.udIsLimitHourLv1 == false)
                    {
                        row[Att_OvertimeEntity.FieldNames.Valid] = "Level2";
                    }
                    else if (item.udIsLimitHourLv2 == null || item.udIsLimitHourLv2 == false)
                    {
                        row[Att_OvertimeEntity.FieldNames.Valid] = "Level3";
                    }
                    else
                    {
                        row[Att_OvertimeEntity.FieldNames.Valid] = string.Empty;
                    }

                    if (item.OvertimeTypeID != Guid.Empty)
                    {
                        var OTType = lstOvertimeType.Where(s => s.ID == item.OvertimeTypeID).FirstOrDefault();
                        row[Att_OvertimeEntity.FieldNames.OvertimeTypeID] = item.OvertimeTypeID;
                        row[Att_OvertimeEntity.FieldNames.OvertimeTypeName] = OTType.OvertimeTypeName;

                        row[OTType.Code] = item.RegisterHours;
                        row[OTType.Code + "Confirm"] = item.ApproveHours ?? 0.0;
                    }
                    if (item.ShiftID != Guid.Empty)
                    {
                        var _shift = lstShift.Where(s => s.ID == item.ShiftID).FirstOrDefault();
                        row[Att_OvertimeEntity.FieldNames.ShiftID] = item.ShiftID;
                        row[Att_OvertimeEntity.FieldNames.ShiftName] = _shift.ShiftName;
                    }
                    if (item.ProfileID != Guid.Empty)
                    {
                        var temp = lstProfile.Where(s => s.ID == item.ProfileID).FirstOrDefault();
                        row[Att_OvertimeEntity.FieldNames.ProfileID] = item.ProfileID;
                        row[Att_OvertimeEntity.FieldNames.ProfileName] = temp.ProfileName;
                        row[Att_OvertimeEntity.FieldNames.CodeEmp] = temp.CodeEmp;
                        //row[Att_ReportDetailOvertimeEntity.FieldNames.OrgStructureName] = temp.OrgStructureName;
                    }
                    row[Att_OvertimeEntity.FieldNames.TotalRow] = lstOvertimeCal.Count;
                    row[Att_OvertimeEntity.FieldNames.DateExport] = DateTime.Now;
                    row[Att_OvertimeEntity.FieldNames.AnalyseHour] = item.AnalyseHour;
                    row[Att_OvertimeEntity.FieldNames.udHourByDate] = item.udHourByDate;
                    row[Att_OvertimeEntity.FieldNames.udHourByWeek] = item.udHourByWeek;
                    row[Att_OvertimeEntity.FieldNames.udHourByMonth] = item.udHourByMonth;
                    row[Att_OvertimeEntity.FieldNames.udHourByYear] = item.udHourByYear;
                    row[Att_OvertimeEntity.FieldNames.udLeaveTypeCode] = item.udLeaveTypeCode;
                    row[Att_OvertimeEntity.FieldNames.udOvertimeStatus] = item.udOvertimeStatus;
                    row[Att_OvertimeEntity.FieldNames.RegisterHours] = item.RegisterHours;
                    row[Att_OvertimeEntity.FieldNames.ApproveHours] = item.RegisterHours;
                    table.Rows.Add(row);
                }
                #endregion
                return table;

            }
        }
        #endregion

        #region Lấy Dữ Liệu Phân Tích Tăng Ca Vượt Mức
        public DataTable LoadDataAnalyzeOvertime_DataLimit(string strOrgStructure, DateTime DateStart, DateTime DateEnd, List<Hre_ProfileEntity> lstProfile, List<string> lstTypeData, Att_OvertimeInfoFillterAnalyze _OvertimeInfoFillterAnalyzeEntity, string userLogin)
        {
            DataTable table = GetSchemaExportExcel();
            string status = string.Empty;

            DateStart = DateStart.Date;
            DateEnd = DateEnd.Date.AddDays(1).AddMilliseconds(-1);
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_WorkDay = new Att_WorkDayRepository(unitOfWork);
                var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                var repoCat_ShiftItem = new Cat_ShiftItemRepository(unitOfWork);
                var repoCat_DayOff = new Cat_DayOffRepository(unitOfWork);
                var repoCat_OvertimeType = new Cat_OvertimeTypeRepository(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_OrgStructureType = new CustomBaseRepository<Cat_OrgStructureType>(unitOfWork);

                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                var orgs = repoCat_OrgStructure.FindBy(s => s.IsDelete == null && s.Code != null).ToList();

                List<WorkdayCustom> lstWorkday = new List<WorkdayCustom>();
                List<Guid> lstProfileId = lstProfile.Select(s => s.ID).ToList();

                List<object> lst3ParamSE = new List<object>();
                lst3ParamSE.Add(strOrgStructure);
                lst3ParamSE.Add(DateStart);
                lst3ParamSE.Add(DateEnd);
                if (lstProfile.Count > 0 && strOrgStructure == null)
                {
                    var lstWD = repoAtt_WorkDay.FindBy(s => s.IsDelete == null && s.WorkDate >= DateStart && s.WorkDate <= DateEnd && lstProfileId.Contains(s.ProfileID)).ToList();
                    if (lstWD.Count > 0)
                    {
                        lstWorkday = lstWD.Translate<WorkdayCustom>();
                    }
                }
                else
                {
                    lstWorkday = GetData<WorkdayCustom>(lst3ParamSE, ConstantSql.hrm_att_getdata_Workday, userLogin, ref status).ToList();
                }
                //var lstWorkdayQuery = repoAtt_WorkDay.FindBy(m => m.IsDelete == null && m.WorkDate >= DateStart && m.WorkDate <= DateEnd);
                //if (lstProfileId.Count > 0)
                //{
                //    lstWorkdayQuery = lstWorkdayQuery.Where(m => lstProfileId.Contains(m.ProfileID));
                //}
                //lstWorkday = lstWorkdayQuery.ToList().Translate<Att_WorkdayEntity>();

                List<Cat_ShiftEntity> lstShift = repoCat_Shift
                    .FindBy(s => s.IsDelete == null)
                    .ToList()
                    .Translate<Cat_ShiftEntity>();
                List<Cat_ShiftItemEntity> lstShiftItem = repoCat_ShiftItem
                    .FindBy(s => s.IsDelete == null)
                    .ToList()
                    .Translate<Cat_ShiftItemEntity>();
                List<Cat_DayOffEntity> LstDayOff = repoCat_DayOff
                    .FindBy(m => m.IsDelete == null && m.DateOff >= DateStart && m.DateOff < DateEnd)
                    .ToList()
                    .Translate<Cat_DayOffEntity>();
                List<Cat_OvertimeTypeEntity> lstOvertimeType = repoCat_OvertimeType
                    .FindBy(s => s.IsDelete == null).ToList()
                    .Translate<Cat_OvertimeTypeEntity>();

                string E_CANCEL = OverTimeStatus.E_CANCEL.ToString();
                string E_REJECTED = OverTimeStatus.E_REJECTED.ToString();
                DateTime beforeDateStart = DateStart.Date.AddDays(-1);
                DateTime afterDateEnd = DateEnd.Date.AddDays(2).AddMinutes(-1);
                // string E_HOLIDAY_HLD = HolidayType.E_HOLIDAY_HLD.ToString();
                List<DateTime> lstHoliday = repoCat_DayOff
                    .FindBy(s => s.IsDelete == null)
                    .Select(m => m.DateOff).ToList<DateTime>();
                //lstWorkday = filterData_NonAllowOT_inGrade(lstWorkday);
                List<Att_OvertimeEntity> lstOvertimeCal = AnalyzeOvertime(lstWorkday, lstShift, lstShiftItem, LstDayOff, lstOvertimeType, _OvertimeInfoFillterAnalyzeEntity, userLogin);
                if (lstOvertimeCal.Count == 0)
                {
                    lstOvertimeCache = lstOvertimeCal;
                    return table;
                }
                OvertimePermitEntity overtimePermit = getOvertimePermit(userLogin);
                FilterNonOvertimeByGradeConfig(lstOvertimeCal);
                SetNonOT(lstOvertimeCal);
                SetStatusOvertimeOnWorkday(lstOvertimeCal);
                FilterOvertimeByMaxHourPerDay(lstOvertimeCal, overtimePermit, _OvertimeInfoFillterAnalyzeEntity.MaximumOvertimeHour);
                RoundOT(lstOvertimeCal, userLogin);
                FillterAllowOvertime(context, lstOvertimeCal, overtimePermit, lstWorkday);
                RoundOT(lstOvertimeCal, userLogin);
                lstOvertimeCal = lstOvertimeCal.Where(m => m.RegisterHours != null && m.RegisterHours > 0).ToList();
                SetStatusLeaveOnWorkday(lstOvertimeCal);//Set loại ngày nghỉ cho OT

                if (lstTypeData.Count > 0)
                {
                    Expression<Func<Att_OvertimeEntity, bool>> predicate = VnResource.Helper.Linq.PredicateBuilder.False<Att_OvertimeEntity>();
                    if (lstTypeData.Any(m => m == ComputeLeavedayType.E_DATA_LEAVE.ToString()))
                    {
                        Expression<Func<Att_OvertimeEntity, bool>> predicate1 = m => m.udLeaveTypeCode != null || m.udLeaveTypeCode2 != null;
                        predicate = predicate.Or(predicate1);
                        //lstOvertimeCal = lstOvertimeCal.Where(m => (m.udLeaveTypeCode == null || m.udLeaveTypeCode == string.Empty) && (m.udLeaveTypeCode2 == null || m.udLeaveTypeCode2 == string.Empty)).ToList();
                    }
                    if (lstTypeData.Any(m => m == ComputeLeavedayType.E_DATA_NON_LEAVE.ToString()))
                    {
                        Expression<Func<Att_OvertimeEntity, bool>> predicate2 = m => (m.udLeaveTypeCode == null || m.udLeaveTypeCode == string.Empty) && (m.udLeaveTypeCode2 == null || m.udLeaveTypeCode2 == string.Empty);
                        predicate = predicate.Or(predicate2);
                        //lstOvertimeCal = lstOvertimeCal.Where(m => m.udLeaveTypeCode != null || m.udLeaveTypeCode2 != null).ToList();
                    }
                    if (lstTypeData.Any(m => m == ComputeOvertimeType.E_DATA_OT.ToString()))
                    {
                        Expression<Func<Att_OvertimeEntity, bool>> predicate2 = m => m.udOvertimeStatus != null;
                        predicate = predicate.Or(predicate2);
                    }
                    if (lstTypeData.Any(m => m == ComputeOvertimeType.E_DATA_NON_OT.ToString()))
                    {
                        Expression<Func<Att_OvertimeEntity, bool>> predicate2 = m => m.udOvertimeStatus == null;
                        predicate = predicate.Or(predicate2);
                    }
                    lstOvertimeCal = lstOvertimeCal.AsQueryable().Where(predicate).ToList();
                }

                //lstOvertimeCal = lstOvertimeCal.OrderBy(m => m.WorkDate.Date).ThenBy(m => m.ProfileID).ToList();
                //lstOvertimeCache = lstOvertimeCal;
                //BindToGrid(lstOvertimeCache);
                lstOvertimeCal = lstOvertimeCal
                    .Where(m => m.udIsLimitHour == true || m.udIsLimitHourLv1 == true || m.udIsLimitHourLv2 == true)
                    .OrderBy(m => m.WorkDate.Date)
                    .ThenBy(m => m.ProfileID)
                    .ToList();

                //lstOvertimeCal.ForEach(s => s.IsValid = false);

                #region process return table

                foreach (var item in lstOvertimeCal)
                {
                    DataRow row = table.NewRow();
                    Guid? orgId = lstProfile.Where(s => s.ID == item.ProfileID).FirstOrDefault().OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.ID == orgId);
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    row[Att_OvertimeEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.OrgStructureCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.OrgName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    row[Att_OvertimeEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;

                    row[Att_OvertimeEntity.FieldNames.WorkDate] = item.WorkDate;
                    row[Att_OvertimeEntity.FieldNames.InTime] = item.InTime;
                    row[Att_OvertimeEntity.FieldNames.OutTime] = item.OutTime;

                    if (item.udIsLimitHour == null || item.udIsLimitHour == false)
                    {
                        row[Att_OvertimeEntity.FieldNames.Valid] = "Level1";
                    }
                    else if (item.udIsLimitHourLv1 == null || item.udIsLimitHourLv1 == false)
                    {
                        row[Att_OvertimeEntity.FieldNames.Valid] = "Level2";
                    }
                    else if (item.udIsLimitHourLv2 == null || item.udIsLimitHourLv2 == false)
                    {
                        row[Att_OvertimeEntity.FieldNames.Valid] = "Level3";
                    }
                    else
                    {
                        row[Att_OvertimeEntity.FieldNames.Valid] = string.Empty;
                    }

                    if (item.OvertimeTypeID != Guid.Empty)
                    {
                        var OTType = lstOvertimeType.Where(s => s.ID == item.OvertimeTypeID).FirstOrDefault();
                        row[Att_OvertimeEntity.FieldNames.OvertimeTypeID] = item.OvertimeTypeID;
                        row[Att_OvertimeEntity.FieldNames.OvertimeTypeName] = OTType.OvertimeTypeName;

                        row[OTType.Code] = item.RegisterHours;
                        row[OTType.Code + "Confirm"] = item.ApproveHours ?? 0.0;
                    }
                    if (item.ShiftID != Guid.Empty)
                    {
                        var _shift = lstShift.Where(s => s.ID == item.ShiftID).FirstOrDefault();
                        row[Att_OvertimeEntity.FieldNames.ShiftID] = item.ShiftID;
                        row[Att_OvertimeEntity.FieldNames.ShiftName] = _shift.ShiftName;
                    }
                    if (item.ProfileID != Guid.Empty)
                    {
                        var temp = lstProfile.Where(s => s.ID == item.ProfileID).FirstOrDefault();
                        row[Att_OvertimeEntity.FieldNames.ProfileID] = item.ProfileID;
                        row[Att_OvertimeEntity.FieldNames.ProfileName] = temp.ProfileName;
                        row[Att_OvertimeEntity.FieldNames.CodeEmp] = temp.CodeEmp;
                        //row[Att_ReportDetailOvertimeEntity.FieldNames.OrgStructureName] = temp.OrgStructureName;
                    }
                    row[Att_OvertimeEntity.FieldNames.TotalRow] = lstOvertimeCal.Count;
                    row[Att_OvertimeEntity.FieldNames.DateExport] = DateTime.Now;
                    row[Att_OvertimeEntity.FieldNames.AnalyseHour] = item.AnalyseHour;
                    row[Att_OvertimeEntity.FieldNames.udHourByDate] = item.udHourByDate;
                    row[Att_OvertimeEntity.FieldNames.udHourByWeek] = item.udHourByWeek;
                    row[Att_OvertimeEntity.FieldNames.udHourByMonth] = item.udHourByMonth;
                    row[Att_OvertimeEntity.FieldNames.udHourByYear] = item.udHourByYear;
                    row[Att_OvertimeEntity.FieldNames.udLeaveTypeCode] = item.udLeaveTypeCode;
                    row[Att_OvertimeEntity.FieldNames.udOvertimeStatus] = item.udOvertimeStatus;
                    row[Att_OvertimeEntity.FieldNames.RegisterHours] = item.RegisterHours;
                    row[Att_OvertimeEntity.FieldNames.ApproveHours] = item.RegisterHours;
                    table.Rows.Add(row);
                }
                #endregion
                return table;

            }
        }
        #endregion

        #region Tạo DataTable cho Hàm Phân Tích Tăng Ca
        DataTable GetSchemaExportExcel()
        {
            string stt = string.Empty;
            DataTable tb = new DataTable();

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_OvertimeType = new CustomBaseRepository<Cat_OvertimeType>(unitOfWork);

                tb.Columns.Add(Att_OvertimeEntity.FieldNames.ProfileID, typeof(Guid));
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.ProfileName);
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.BranchCode);
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.OrgStructureCode);
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.TeamCode);
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.SectionCode);
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.JobtitleName);
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.PositionName);
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.UserExport);
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.BranchName);
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.OrgName);
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.TeamName);
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.SectionName);
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.DateOvertime, typeof(DateTime));
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.DateExport, typeof(DateTime));
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.DateFrom, typeof(DateTime));
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.DateTo, typeof(DateTime));
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.udInTime, typeof(DateTime));
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.udOutTime, typeof(DateTime));
                //tb.Columns.Add(Att_OvertimeEntity.FieldNames.Cat_Shift);
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.ShiftID, typeof(Guid));
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.WorkDate, typeof(DateTime));
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.InTime, typeof(DateTime));
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.OutTime, typeof(DateTime));
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.AnalyseHour, typeof(Double));
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.OvertimeTypeID, typeof(Guid));
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.udHourByDate, typeof(Double));
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.udHourByWeek, typeof(Double));
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.udHourByMonth, typeof(Double));
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.udHourByYear, typeof(Double));
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.udLeaveTypeCode);
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.udOvertimeStatus);
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.RegisterHours, typeof(Double));
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.ApproveHours, typeof(Double));

                tb.Columns.Add(Att_OvertimeEntity.FieldNames.Valid);
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.TotalRow, typeof(Int32));
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.ShiftName);
                tb.Columns.Add(Att_OvertimeEntity.FieldNames.OvertimeTypeName);

                var codes = repoCat_OvertimeType.FindBy(s => s.Code != null).Select(s => s.Code).Distinct().ToList<string>();
                foreach (string code in codes)
                {
                    if (!tb.Columns.Contains(code))
                    {
                        tb.Columns.Add(code, typeof(double));
                    }
                    if (!tb.Columns.Contains(code + "Confirm"))
                    {
                        tb.Columns.Add(code + "Confirm", typeof(double));
                    }
                }
                return tb;
            }
        }
        #endregion

        #region Hàm lọc dữ liệu nhân viên không được phép đăng ký tăng ca vì chế độ lương
        /// <summary>
        /// Hàm lọc dữ liệu nhân viên không được phép đăng ký tăng ca vì chế độ lương
        /// </summary>
        /// <param name="guidContext"></param>
        /// <param name="lstOvertimeInput"></param>
        /// <returns></returns>
        public List<Att_OvertimeEntity> FilterNonOvertimeByGradeConfig(List<Att_OvertimeEntity> lstOvertimeInput)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_GradeAttendance = new CustomBaseRepository<Cat_GradeAttendance>(unitOfWork);
                var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);

                DateTime DateMaxOt = lstOvertimeInput.Where(m => m.WorkDateRoot != null).Max(m => m.WorkDateRoot.Value);
                List<Att_OvertimeEntity> lstOvertimeRemove = new List<Att_OvertimeEntity>();
                var lstProfileIDs = lstOvertimeInput.Select(m => m.ProfileID).Distinct().ToList();
                var lstGradeConfig = repoCat_GradeAttendance
                    .FindBy(s => s.IsDelete == null).Select(m => new { m.ID, m.IsReceiveOvertimeBonus }).ToList();
                var lstGrade = new List<Att_Grade>().Select(m => new { m.ID, m.ProfileID, m.MonthStart, m.GradeAttendanceID });

                if (lstProfileIDs.Count > 2000)
                {
                    lstGrade = repoAtt_Grade.FindBy(m => m.IsDelete == null && m.MonthStart <= DateMaxOt)
                        .Select(m => new { m.ID, m.ProfileID, m.MonthStart, m.GradeAttendanceID }).ToList();
                }
                else
                {
                    lstGrade = repoAtt_Grade.FindBy(m => m.IsDelete == null && m.MonthStart <= DateMaxOt && lstProfileIDs.Contains(m.ProfileID.Value))
                      .Select(m => new { m.ID, m.ProfileID, m.MonthStart, m.GradeAttendanceID }).ToList();
                }
                foreach (var item in lstOvertimeInput)
                {
                    Guid profileID = item.ProfileID;
                    DateTime WorkdayRoot = item.WorkDateRoot ?? item.WorkDate.Date;
                    var Grade = lstGrade.Where(m => m.ProfileID == profileID && m.MonthStart <= WorkdayRoot).OrderByDescending(m => m.MonthStart).FirstOrDefault();
                    if (Grade != null)
                    {
                        var GradeCfg = lstGradeConfig.Where(m => m.ID == Grade.GradeAttendanceID).FirstOrDefault();
                        if (GradeCfg != null && GradeCfg.IsReceiveOvertimeBonus != true)
                        {
                            lstOvertimeRemove.Add(item);
                        }
                    }
                }
                lstOvertimeInput.RemoveRange(lstOvertimeRemove);
                return lstOvertimeInput;
            }
        }
        #endregion


        private void SetNonOT(List<Att_OvertimeEntity> lstOvertime)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                if (lstOvertime.Count == 0)
                    return;
                var repoAtt_NonOverTime = new CustomBaseRepository<Att_NonOverTime>(unitOfWork);
                DateTime minDate = lstOvertime.Min(m => m.WorkDateRoot.Value);
                DateTime maxDate = lstOvertime.Max(m => m.WorkDateRoot.Value);
                List<Guid> lstProfileID = lstOvertime.Select(m => m.ProfileID).Distinct().ToList();
                //List<Att_NonOverTime> LstNonOT = repoAtt_NonOverTime
                //    .FindBy(m => m.WorkDay != null && m.ProfileID != null && m.WorkDay.Value >= minDate && m.WorkDay.Value <= maxDate && lstProfileID.Contains(m.ProfileID.Value))
                //    .ToList();
                List<Att_NonOverTime> LstNonOT = new List<Att_NonOverTime>();
                if (lstProfileID.Count < 2000)
                {
                    LstNonOT = repoAtt_NonOverTime
                    .FindBy(m => m.WorkDay != null && m.ProfileID != null && m.WorkDay.Value >= minDate && m.WorkDay.Value <= maxDate && lstProfileID.Contains(m.ProfileID.Value)).ToList<Att_NonOverTime>();
                }
                else
                {
                    LstNonOT = repoAtt_NonOverTime
                    .FindBy(m => m.WorkDay != null && m.ProfileID != null && m.WorkDay.Value >= minDate && m.WorkDay.Value <= maxDate).ToList<Att_NonOverTime>();
                }
                foreach (var item in lstOvertime)
                {
                    if (LstNonOT.Any(m => m.ProfileID == item.ProfileID && m.OvertimeTypeID == item.OvertimeTypeID && m.WorkDay == item.WorkDateRoot && m.Type == item.udTypeBeginOTWithShift))
                    {
                        item.IsNonOvertime = true;
                    }
                }
            }
        }

        //private List<Att_WorkdayEntity> filterData_NonAllowOT_inGrade(List<Att_WorkdayEntity> lstWorkday)
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);
        //        var repoCat_GradeCfg = new CustomBaseRepository<Cat_GradeAttendance>(unitOfWork);

        //        List<Att_WorkdayEntity> lstResult = new List<Att_WorkdayEntity>();
        //        List<Guid> lstProfileID = lstWorkday.Select(m => m.ProfileID).Distinct().ToList();
        //        var lstSalGrade = repoAtt_Grade
        //            .FindBy( m => m.IsDelete == null && lstProfileID.Contains(m.ProfileID.Value))
        //            .Select(m => new { m.ID, m.ProfileID, m.MonthStart, m.GradeAttendanceID })
        //            .ToList();
        //        var lstGradeConfg = repoCat_GradeCfg
        //            .FindBy(s => s.IsDelete == null)
        //            .Select(m => new { m.ID, m.IsReceiveOvertimeBonus })
        //            .ToList();

        //        foreach (var item in lstWorkday)
        //        {
        //            Guid gradeID = lstSalGrade
        //                .Where(m => m.ProfileID == item.ProfileID && m.MonthStart <= item.WorkDate).OrderByDescending(m => m.MonthStart)
        //                .Select(m => m.GradeAttendanceID.Value)
        //                .FirstOrDefault();

        //            if (gradeID != null && gradeID != Guid.Empty)
        //            {
        //                var gradeConfig = lstGradeConfg.Where(m => m.ID == gradeID).FirstOrDefault();
        //                if (gradeConfig != null && (gradeConfig.IsReceiveOvertimeBonus == true))
        //                {
        //                    lstResult.Add(item);
        //                }
        //            }

        //        }
        //        return lstResult;
        //    }
        //}

        #region Analyze Overtime

        // Kiem tra gioi han, rang buoc OT
        public static OvertimePermitEntity getOvertimePermit(string userLogin)
        {
            BaseService _baseService = new BaseService();
            string HRM_ATT_OT_OTPERMIT_ = AppConfig.HRM_ATT_OT_OTPERMIT_.ToString();
            string status = string.Empty;
            List<object> lstO = new List<object>();
            lstO.Add(HRM_ATT_OT_OTPERMIT_);
            lstO.Add(null);
            lstO.Add(null);

            var config = _baseService.GetData<Sys_AllSettingEntity>(lstO, ConstantSql.hrm_sys_sp_get_AllSetting, userLogin, ref status);

            var configlimitHour_ByDay = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY.ToString()).FirstOrDefault();
            var configlimitHour_ByWeek = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK.ToString()).FirstOrDefault();
            var configlimitHour_ByMonth = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH.ToString()).FirstOrDefault();
            var configlimitHour_ByYear = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR.ToString()).FirstOrDefault();
            var configlimitColor = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR.ToString()).FirstOrDefault();

            var configlimitHour_ByDay_Lev1 = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY_LEV1.ToString()).FirstOrDefault();
            var configlimitHour_ByWeek_Lev1 = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK_LEV1.ToString()).FirstOrDefault();
            var configlimitHour_ByMonth_Lev1 = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH_LEV1.ToString()).FirstOrDefault();
            var configlimitHour_ByYear_Lev1 = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR_LEV1.ToString()).FirstOrDefault();
            var configlimitColor_Lev1 = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR_LEV1.ToString()).FirstOrDefault();

            var configlimitHour_ByDay_Lev2 = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYDAY_LEV2.ToString()).FirstOrDefault();
            var configlimitHour_ByWeek_Lev2 = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYWEEK_LEV2.ToString()).FirstOrDefault();
            var configlimitHour_ByMonth_Lev2 = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYMONTH_LEV2.ToString()).FirstOrDefault();
            var configlimitHour_ByYear_Lev2 = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITHOUR_BYYEAR_LEV2.ToString()).FirstOrDefault();
            var configlimitColor_Lev2 = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_LIMITCOLOR_LEV2.ToString()).FirstOrDefault();

            var configIsAllowOverLimit_Normal = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL.ToString()).FirstOrDefault();
            var configIsAllowOverLimit_Normal_Lev1 = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL_LEV1.ToString()).FirstOrDefault();
            var configIsAllowOverLimit_Normal_Lev2 = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_NORMAL_LEV2.ToString()).FirstOrDefault();
            var configIsAllowOverLimit_AllowOver = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER.ToString()).FirstOrDefault();
            var configIsAllowOverLimit_AllowOver_Lev1 = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER_LEV1.ToString()).FirstOrDefault();
            var configIsAllowOverLimit_AllowOver_Lev2 = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWOVERLIMIT_ALLOWOVER_LEV2.ToString()).FirstOrDefault();
            var configIsAllowSplit = config.Where(s => s.Name == AppConfig.HRM_ATT_OT_OTPERMIT_ISALLOWSPLIT.ToString()).FirstOrDefault();

            OvertimePermitEntity result = new OvertimePermitEntity();

            if (config != null)
            {
                if (configlimitHour_ByDay != null)
                {
                    if (configlimitHour_ByDay.Value1 == null)
                    {
                        result.limitHour_ByDay = null;
                    }
                    else
                    {
                        double limitHour = 0;
                        double.TryParse(configlimitHour_ByDay.Value1, out limitHour);
                        result.limitHour_ByDay = limitHour;
                    }
                }
                if (configlimitHour_ByWeek != null)
                {
                    if (configlimitHour_ByWeek.Value1 == null)
                    {
                        result.limitHour_ByWeek = null;
                    }
                    else
                    {
                        double limitHour = 0;
                        double.TryParse(configlimitHour_ByWeek.Value1, out limitHour);
                        result.limitHour_ByWeek = limitHour;
                    }
                }
                if (configlimitHour_ByMonth != null)
                {
                    if (configlimitHour_ByMonth.Value1 == null)
                    {
                        result.limitHour_ByMonth = null;
                    }
                    else
                    {
                        double limitHour = 0;
                        double.TryParse(configlimitHour_ByMonth.Value1, out limitHour);
                        result.limitHour_ByMonth = limitHour;
                    }
                }
                if (configlimitHour_ByYear != null)
                {
                    if (configlimitHour_ByYear.Value1 == null)
                    {
                        result.limitHour_ByYear = null;
                    }
                    else
                    {
                        double limitHour = 0;
                        double.TryParse(configlimitHour_ByYear.Value1, out limitHour);
                        result.limitHour_ByYear = limitHour;
                    }
                }
                if (configlimitColor != null)
                {
                    result.limitColor = configlimitColor.Value1;
                }

                if (configlimitHour_ByDay_Lev1 != null)
                {
                    if (configlimitHour_ByDay_Lev1.Value1 == null)
                    {
                        result.limitHour_ByDay_Lev1 = null;
                    }
                    else
                    {
                        double limitHour = 0;
                        double.TryParse(configlimitHour_ByDay_Lev1.Value1, out limitHour);
                        result.limitHour_ByDay_Lev1 = limitHour;
                    }
                }
                if (configlimitHour_ByWeek_Lev1 != null)
                {
                    if (configlimitHour_ByWeek_Lev1.Value1 == null)
                    {
                        result.limitHour_ByWeek_Lev1 = null;
                    }
                    else
                    {
                        double limitHour = 0;
                        double.TryParse(configlimitHour_ByWeek_Lev1.Value1, out limitHour);
                        result.limitHour_ByWeek_Lev1 = limitHour;
                    }
                }
                if (configlimitHour_ByMonth_Lev1 != null)
                {
                    if (configlimitHour_ByMonth_Lev1.Value1 == null)
                    {
                        result.limitHour_ByMonth_Lev1 = null;
                    }
                    else
                    {
                        double limitHour = 0;
                        double.TryParse(configlimitHour_ByMonth_Lev1.Value1, out limitHour);
                        result.limitHour_ByMonth_Lev1 = limitHour;
                    }
                }
                if (configlimitHour_ByYear_Lev1 != null)
                {
                    if (configlimitHour_ByYear_Lev1.Value1 == null)
                    {
                        result.limitHour_ByYear_Lev1 = null;
                    }
                    else
                    {
                        double limitHour = 0;
                        double.TryParse(configlimitHour_ByYear_Lev1.Value1, out limitHour);
                        result.limitHour_ByYear_Lev1 = limitHour;
                    }
                }
                if (configlimitColor_Lev1 != null)
                {
                    result.limitColor_Lev1 = configlimitColor_Lev1.Value1;
                }

                if (configlimitHour_ByDay_Lev2 != null)
                {
                    if (configlimitHour_ByDay_Lev2.Value1 == null)
                    {
                        result.limitHour_ByDay_Lev2 = null;
                    }
                    else
                    {
                        double limitHour = 0;
                        double.TryParse(configlimitHour_ByDay_Lev2.Value1, out limitHour);
                        result.limitHour_ByDay_Lev2 = limitHour;
                    }
                }
                if (configlimitHour_ByWeek_Lev2 != null)
                {
                    if (configlimitHour_ByWeek_Lev2.Value1 == null)
                    {
                        result.limitHour_ByWeek_Lev2 = null;
                    }
                    else
                    {
                        double limitHour = 0;
                        double.TryParse(configlimitHour_ByWeek_Lev2.Value1, out limitHour);
                        result.limitHour_ByWeek_Lev2 = limitHour;
                    }
                }
                if (configlimitHour_ByMonth_Lev2 != null)
                {
                    if (configlimitHour_ByMonth_Lev2.Value1 == null)
                    {
                        result.limitHour_ByMonth_Lev2 = null;
                    }
                    else
                    {
                        double limitHour = 0;
                        double.TryParse(configlimitHour_ByMonth_Lev2.Value1, out limitHour);
                        result.limitHour_ByMonth_Lev2 = limitHour;
                    }
                }
                if (configlimitHour_ByYear_Lev2 != null)
                {
                    if (configlimitHour_ByYear_Lev2.Value1 == null)
                    {
                        result.limitHour_ByYear_Lev2 = null;
                    }
                    else
                    {
                        double limitHour = 0;
                        double.TryParse(configlimitHour_ByYear_Lev2.Value1, out limitHour);
                        result.limitHour_ByYear_Lev2 = limitHour;
                    }
                }
                if (configlimitColor_Lev2 != null)
                {
                    result.limitColor_Lev2 = configlimitColor_Lev2.Value1;
                }

                if (configIsAllowOverLimit_Normal != null && configIsAllowOverLimit_Normal.Value1 == bool.TrueString)
                {
                    result.IsAllowOverLimit_Normal = true;
                }
                if (configIsAllowOverLimit_Normal_Lev1 != null && configIsAllowOverLimit_Normal_Lev1.Value1 == bool.TrueString)
                {
                    result.IsAllowOverLimit_Normal_Lev1 = true;
                }
                if (configIsAllowOverLimit_Normal_Lev2 != null && configIsAllowOverLimit_Normal_Lev2.Value1 == bool.TrueString)
                {
                    result.IsAllowOverLimit_Normal_Lev2 = true;
                }
                if (configIsAllowOverLimit_AllowOver != null && configIsAllowOverLimit_AllowOver.Value1 == bool.TrueString)
                {
                    result.IsAllowOverLimit_AllowOver = true;
                }
                if (configIsAllowOverLimit_AllowOver_Lev1 != null && configIsAllowOverLimit_AllowOver_Lev1.Value1 == bool.TrueString)
                {
                    result.IsAllowOverLimit_AllowOver_Lev1 = true;
                }
                if (configIsAllowOverLimit_AllowOver_Lev2 != null && configIsAllowOverLimit_AllowOver_Lev2.Value1 == bool.TrueString)
                {
                    result.IsAllowOverLimit_AllowOver_Lev2 = true;
                }
                if (configIsAllowSplit != null && configIsAllowSplit.Value1 == bool.TrueString)
                {
                    result.IsAllowSplit = true;
                }

            }
            return result;

        }

        private void FillterAllowOvertime(VnrHrmDataContext context, List<Att_OvertimeEntity> lstOvertime, OvertimePermitEntity OtPermit, List<WorkdayCustom> lstWorkday)
        {
            if (lstOvertime.Count == 0)
            {
                return;
            }
            if (OtPermit == null || (OtPermit.limitHour_ByDay == null
                                    && OtPermit.limitHour_ByDay_Lev1 == null
                                    && OtPermit.limitHour_ByDay_Lev2 == null
                                    && OtPermit.limitHour_ByWeek == null
                                    && OtPermit.limitHour_ByWeek_Lev1 == null
                                    && OtPermit.limitHour_ByWeek_Lev2 == null
                                    && OtPermit.limitHour_ByMonth == null
                                    && OtPermit.limitHour_ByMonth_Lev1 == null
                                    && OtPermit.limitHour_ByMonth_Lev2 == null
                                    && OtPermit.limitHour_ByYear == null
                                    && OtPermit.limitHour_ByYear_Lev1 == null
                                    && OtPermit.limitHour_ByYear_Lev2 == null)
                )
            {
                return;
            }
            var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
            var repoAtt_Overtime = new CustomBaseRepository<Att_Overtime>(unitOfWork);
            var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);
            var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);

            #region getData
            DateTime DateMinInlstOvertime = lstOvertime.Min(m => m.WorkDate);
            DateTime DateMaxInlstOvertime = lstOvertime.Max(m => m.WorkDate);
            List<Guid> lstProfileID = lstOvertime.Select(m => m.ProfileID).Distinct().ToList();

            //Lấy ngày Đầu Năm, Đầu Tuần ->> Ngày nào nhỏ nhất thì lấy theo Ngày đó làm mốc
            DateTime DateMin = DateTime.MinValue;
            DateTime DateBeginYear = new DateTime(DateMinInlstOvertime.Year, 1, 1);
            DateTime DateBeginMonth = new DateTime(DateMinInlstOvertime.Year, DateMinInlstOvertime.Month, 1);
            DateTime DateBeginWeek = DateTime.MinValue;
            DateTime DateEndWeek = DateTime.MinValue;
            Common.GetStartEndWeek(DateMinInlstOvertime.Date, out DateBeginWeek, out DateEndWeek);
            DateMin = DateBeginYear < DateBeginWeek ? DateBeginYear : DateBeginWeek;
            DateTime DateMax = DateMaxInlstOvertime.AddYears(1);

            string E_CANCEL = OverTimeStatus.E_CANCEL.ToString();
            string E_APPROVED = OverTimeStatus.E_APPROVED.ToString();
            string E_CONFIRM = OverTimeStatus.E_CONFIRM.ToString();
            string E_FIRST_APPROVED = OverTimeStatus.E_FIRST_APPROVED.ToString();
            string E_REJECTED = OverTimeStatus.E_REJECTED.ToString();
            string E_SUBMIT = OverTimeStatus.E_SUBMIT.ToString();
            string E_SUBMIT_TEMP = OverTimeStatus.E_SUBMIT_TEMP.ToString();
            string E_TEMP = OverTimeStatus.E_TEMP.ToString();
            string E_WAIT_APPROVED = OverTimeStatus.E_WAIT_APPROVED.ToString();

            string E_CASHOUT = MethodOption.E_CASHOUT.ToString();

            List<Guid> lstOvertimeAlreadyID = lstOvertime.Where(m => m.udAlreadyOvertimeID != null).Select(m => m.udAlreadyOvertimeID ?? Guid.Empty).Distinct().ToList();

            var lstOvertimeInDb = repoAtt_Overtime
                .FindBy(m => m.IsDelete == null
                    && m.WorkDate >= DateMin && m.WorkDate < DateMax && lstProfileID.Contains(m.ProfileID)
                    && (m.MethodPayment == null || (m.MethodPayment != null && m.MethodPayment == E_CASHOUT))
                    && m.IsNonOvertime == null
                    && !lstOvertimeAlreadyID.Contains(m.ID)
                    && m.Status != E_CANCEL && m.Status != E_REJECTED)
                .Select(m => new { m.Status, m.ProfileID, m.WorkDate, m.RegisterHours, m.ApproveHours, m.ConfirmHours })
                .ToList();

            var lstShift = repoCat_Shift
                .FindBy(m => m.IsDelete == null)
                .Select(m => new { m.ID, m.InTime, m.CoBreakOut, m.CoBreakIn, m.CoOut })
                .ToList();

            string E_HOLIDAY_HLD = HolidayType.E_HOLIDAY_HLD.ToString();
            List<DateTime> lstHoliday = repoCat_DayOff
                .FindBy(m => m.IsDelete == null && m.Type == E_HOLIDAY_HLD)
                .Select(m => m.DateOff)
                .ToList<DateTime>();

            #endregion

            #region processing

            lstOvertime = lstOvertime.OrderBy(m => m.WorkDateRoot).ThenBy(m => m.WorkDate).ToList();
            foreach (var ProfileID in lstProfileID)
            {
                Double RegisterPlus_Year = 0;
                Double RegisterPlus_Month = 0;
                Double RegisterPlus_Week = 0;
                Double WorkHour_Day = 0;

                DateTime BeginYear = DateTime.MinValue;
                DateTime EndYear = DateTime.MinValue;
                DateTime BeginMonth = DateTime.MinValue;
                DateTime EndMonth = DateTime.MinValue;
                DateTime BeginWeek = DateTime.MinValue;
                DateTime EndWeek = DateTime.MinValue;
                DateTime BeginDate = DateTime.MinValue;
                DateTime EndDate = DateTime.MinValue;

                bool isResetYear = false;
                bool isResetMonth = false;
                // bool isResetWeek = false;
                bool isResetDate = false;

                List<Att_OvertimeEntity> lstOvertime_ByProfile = lstOvertime.Where(m => m.ProfileID == ProfileID).ToList();
                var lstOvertime_ByProfile_DB = lstOvertimeInDb.Where(m => m.ProfileID == ProfileID).ToList();


                foreach (var OT in lstOvertime_ByProfile)
                {
                    DateTime workday = OT.WorkDateRoot ?? OT.WorkDate;
                    #region ResetPlus
                    //reset year
                    if (workday.Date > EndYear)
                    {
                        BeginYear = new DateTime(workday.Year, 1, 1);
                        EndYear = BeginYear.AddYears(1).AddMinutes(-1);
                        if (OT.IsNonOvertime != true)
                            RegisterPlus_Year = OT.RegisterHours;
                        isResetYear = true;
                    }
                    else
                    {
                        if (OT.IsNonOvertime != true)
                            RegisterPlus_Year += OT.RegisterHours;
                        isResetYear = false;
                    }
                    //Reset Month
                    if (workday.Date > EndMonth)
                    {
                        BeginMonth = new DateTime(workday.Year, workday.Month, 1);
                        EndMonth = BeginMonth.AddMonths(1).AddMinutes(-1);
                        if (OT.IsNonOvertime != true)
                            RegisterPlus_Month = OT.RegisterHours;
                        isResetMonth = true;
                    }
                    else
                    {
                        if (OT.IsNonOvertime != true)
                            RegisterPlus_Month += OT.RegisterHours;
                        isResetMonth = false;
                    }
                    ////Reset Week
                    //if (workday.Date > EndWeek)
                    //{
                    //    Common.GetStartEndWeek(workday.Date, out BeginWeek, out EndWeek);
                    //    if (string.IsNullOrEmpty(OT.udOvertimeStatus) && OT.IsNonOvertime != true)
                    //        RegisterPlus_Week = OT.RegisterHours;
                    //    isResetWeek = true;
                    //}
                    //else
                    //{
                    //    if (string.IsNullOrEmpty(OT.udOvertimeStatus) && OT.IsNonOvertime != true)
                    //        RegisterPlus_Week += OT.RegisterHours;
                    //    isResetWeek = false;
                    //}
                    //Reset Day
                    if (workday.Date > BeginDate)
                    {
                        BeginDate = workday.Date;
                        EndDate = BeginDate.AddDays(1).AddMinutes(-1);

                        if (OT.OutTime != null && OT.InTime != null)
                        {
                            // WorkHour_Day = OT.RegisterHours;
                            WorkHour_Day = (OT.OutTime.Value - OT.InTime.Value).TotalMinutes / 60;
                            var shift = lstShift.Where(m => m.ID == OT.ShiftID).FirstOrDefault();
                            if (shift != null && OT.InTime != null && OT.OutTime != null && !lstWorkday.Any(m => m.ProfileID == ProfileID && m.WorkDate == workday && lstHoliday.Contains(m.WorkDate)))
                            {
                                DateTime BeginShift = workday.Date.AddHours(shift.InTime.Hour).AddMinutes(shift.InTime.Minute);
                                DateTime EndShift = BeginShift.AddHours(shift.CoOut);
                                DateTime BeginCoBreakIn = BeginShift.AddHours(shift.CoBreakIn);
                                DateTime EndCoBreakOut = BeginShift.AddHours(shift.CoBreakOut);
                                //Giao nữa ca đầu
                                WorkHour_Day = 0;
                                if (OT.udWorkdayNonShift == null || OT.udWorkdayNonShift == false)
                                {
                                    if (OT.InTime.Value < BeginCoBreakIn && OT.OutTime.Value > BeginShift)
                                    {
                                        DateTime Begin = OT.InTime.Value > BeginShift ? OT.InTime.Value : BeginShift;
                                        DateTime End = OT.OutTime.Value > BeginCoBreakIn ? BeginCoBreakIn : OT.OutTime.Value;
                                        WorkHour_Day += (End - Begin).TotalHours;
                                    }
                                    //Giao nữa ca sau
                                    if (OT.InTime < EndShift && OT.OutTime > EndCoBreakOut)
                                    {
                                        DateTime Begin = OT.InTime.Value > EndCoBreakOut ? OT.InTime.Value : EndCoBreakOut;
                                        DateTime End = OT.OutTime.Value > EndShift ? EndShift : OT.OutTime.Value;
                                        WorkHour_Day += (End - Begin).TotalHours;
                                    }
                                }
                                if (OT.IsNonOvertime != true)
                                    WorkHour_Day += OT.RegisterHours;
                            }
                            else
                            {
                                if (OT.IsNonOvertime != true)
                                    WorkHour_Day += OT.RegisterHours;
                            }

                        }
                        isResetDate = true;
                    }
                    else
                    {
                        if (OT.IsNonOvertime != true)
                            WorkHour_Day += OT.RegisterHours;
                        isResetDate = false;

                    }
                    #endregion
                    #region getDataExactHour
                    //1. Có 3 loại : register, Approve, Confirm
                    //2. Có 4 dạng la: Year, Month, Week, Day
                    //3. các khoảng thời gian đã được xác định phía trên.
                    //4. Cộng khoảng thời gian trên kia lại và truyền vào hàm
                    if (isResetYear)
                    {
                        RegisterPlus_Year += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginYear && m.WorkDate < EndYear && (m.Status == E_SUBMIT || m.Status == E_SUBMIT_TEMP || m.Status == E_FIRST_APPROVED || m.Status == E_WAIT_APPROVED)).Sum(m => m.RegisterHours);
                        RegisterPlus_Year += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginYear && m.WorkDate.Date <= workday && m.Status == E_APPROVED && m.ApproveHours != null).Sum(m => m.ApproveHours.Value);
                        RegisterPlus_Year += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginYear && m.WorkDate < EndYear && m.Status == E_CONFIRM).Sum(m => m.ConfirmHours);
                    }
                    if (isResetMonth)
                    {
                        RegisterPlus_Month += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginMonth && m.WorkDate < EndMonth && (m.Status == E_SUBMIT || m.Status == E_SUBMIT_TEMP || m.Status == E_FIRST_APPROVED || m.Status == E_WAIT_APPROVED)).Sum(m => m.RegisterHours);
                        RegisterPlus_Month += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginMonth && m.WorkDate.Date <= workday && m.Status == E_APPROVED && m.ApproveHours != null).Sum(m => m.ApproveHours.Value);
                        RegisterPlus_Month += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginMonth && m.WorkDate < EndMonth && m.Status == E_CONFIRM).Sum(m => m.ConfirmHours);
                    }
                    //if (isResetWeek)
                    //{
                    //    RegisterPlus_Week += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginWeek && m.WorkDate < EndWeek && (m.Status == E_SUBMIT || m.Status == E_SUBMIT_TEMP || m.Status == E_FIRST_APPROVED || m.Status == E_WAIT_APPROVED)).Sum(m => m.RegisterHours);
                    //    RegisterPlus_Week = lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginWeek && m.WorkDate.Date <= workday && m.Status == E_APPROVED && m.ApproveHours != null).Sum(m => m.ApproveHours.Value);
                    //    RegisterPlus_Week += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginWeek && m.WorkDate < EndWeek && m.Status == E_CONFIRM).Sum(m => m.ConfirmHours);
                    //}
                    if (isResetDate)
                    {
                        WorkHour_Day += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginDate && m.WorkDate < EndDate && (m.Status == E_SUBMIT || m.Status == E_SUBMIT_TEMP || m.Status == E_FIRST_APPROVED || m.Status == E_WAIT_APPROVED)).Sum(m => m.RegisterHours);
                        WorkHour_Day += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginDate && m.WorkDate.Date <= EndDate && m.Status == E_APPROVED && m.ApproveHours != null).Sum(m => m.ApproveHours.Value);
                        WorkHour_Day += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginDate && m.WorkDate < EndDate && m.Status == E_CONFIRM).Sum(m => m.ConfirmHours);
                    }
                    #endregion
                    //Cập nhật lại cho OT voi nhung trang thai can thiet
                    CalOverAllowOT(OT, WorkHour_Day, RegisterPlus_Week, RegisterPlus_Month, RegisterPlus_Year, OtPermit);

                }
                DateTime DateMinCal = lstOvertime_ByProfile.Min(m => m.WorkDate);
                DateTime DateMaxCal = lstOvertime_ByProfile.Max(m => m.WorkDate);
                DateMinCal = DateMinCal.Date;
                DateMaxCal = DateMaxCal.Date.AddDays(1).AddMinutes(-1);
                for (DateTime DateOfOT = DateMinCal; DateOfOT <= DateMaxCal; DateOfOT = DateOfOT.AddDays(1))
                {
                    var lstOvertime_ByProfile_ByDate = lstOvertime_ByProfile.Where(m => m.WorkDateRoot == DateOfOT).ToList();
                    if (lstOvertime_ByProfile_ByDate.Count > 0)
                    {
                        double NumMax = lstOvertime_ByProfile_ByDate.Max(m => m.udHourByDate ?? 0);
                        foreach (var item in lstOvertime_ByProfile_ByDate)
                        {
                            item.udHourByDate = NumMax;
                        }
                    }
                }
            }
            #endregion


        }

        private void FilterOvertimeByMaxHourPerDay(List<Att_OvertimeEntity> lstOvertimeCal, OvertimePermitEntity overtimePermit, double HourMax)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_OvertimeType = new CustomBaseRepository<Cat_OvertimeType>(unitOfWork);

                if (lstOvertimeCal.Count == 0)
                    return;

                if (overtimePermit.IsAllowSplit == true && HourMax > 0)
                {
                    List<Guid> lstProfileID = lstOvertimeCal.Select(m => m.ProfileID).Distinct().ToList();
                    DateTime dateMin = lstOvertimeCal.Min(m => m.WorkDate).Date;
                    DateTime dateMax = lstOvertimeCal.Max(m => m.WorkDate).Date;

                    var lstOvertimeType = repoCat_OvertimeType.FindBy(s => s.IsDelete == null).Select(m => new { m.ID, m.Rate }).ToList();
                    foreach (var item in lstOvertimeCal)
                    {
                        var overtimeType = lstOvertimeType.Where(m => m.ID == item.OvertimeTypeID).FirstOrDefault();
                        if (overtimeType != null && overtimeType.Rate != null)
                        {
                            item.udRateOvertimeType = overtimeType.Rate;
                        }
                        else
                        {
                            item.udRateOvertimeType = 0;
                        }

                    }


                    foreach (var profileID in lstProfileID)
                    {
                        bool IsBeginWeek = false;
                        bool IsBeginMonth = false;
                        bool IsBeginYear = false;

                        double deltaWeek = 0;
                        double deltaMonth = 0;
                        double deltaYear = 0;
                        double deltaDay = 0;

                        for (DateTime dx = dateMin; dx <= dateMax; dx = dx.AddDays(1))
                        {
                            if (dx.DayOfWeek == DayOfWeek.Monday)
                                IsBeginWeek = true;
                            else IsBeginWeek = false;
                            if (dx.Day == 1)
                                IsBeginMonth = true;
                            else IsBeginMonth = false;
                            if (dx.Day == 1 && dx.Month == 1)
                                IsBeginYear = true;
                            else IsBeginYear = false;

                            if (IsBeginWeek)
                                deltaWeek = 0;
                            if (IsBeginMonth)
                                deltaMonth = 0;
                            if (IsBeginYear)
                                deltaYear = 0;
                            var lstOvertimeCal_ByProfile_ByDay = lstOvertimeCal.Where(m => m.WorkDate.Date == dx && m.ProfileID == profileID && m.IsNonOvertime == null).ToList();
                            //foreach (var OTModify in lstOvertimeCal_ByProfile_ByDay)
                            //{
                            //    if (!IsBeginWeek)
                            //    {
                            //        OTModify.udHourByWeek_Validate = OTModify.udHourByWeek_Validate - deltaWeek;
                            //    }
                            //    if (!IsBeginMonth)
                            //    {
                            //        OTModify.udHourByMonth_Validate = OTModify.udHourByMonth_Validate - deltaMonth;
                            //    }
                            //    if (!IsBeginYear)
                            //    {
                            //        OTModify.udHourByYear_Validate = OTModify.udHourByYear_Validate - deltaYear;
                            //    }
                            //}
                            var OtTopInday = lstOvertimeCal_ByProfile_ByDay.OrderByDescending(m => m.udHourByDate).FirstOrDefault();
                            if (OtTopInday != null && OtTopInday.udHourByDate != null && OtTopInday.udHourByDate.Value > HourMax)
                            {
                                deltaDay = OtTopInday.udHourByDate.Value - HourMax;
                                deltaWeek += deltaDay;
                                deltaMonth += deltaDay;
                                deltaYear += deltaDay;
                                var lstOvertimeByProfileOrderByRate = lstOvertimeCal_ByProfile_ByDay.OrderBy(m => m.udRateOvertimeType).ToList();

                                double deltaAgain = 0;
                                deltaAgain = deltaDay;
                                foreach (var item in lstOvertimeByProfileOrderByRate)
                                {
                                    if (deltaAgain > item.RegisterHours)
                                    {
                                        deltaAgain = deltaAgain - item.RegisterHours;
                                        item.RegisterHours = 0;
                                        item.AnalyseHour = 0;
                                    }
                                    else
                                    {
                                        item.RegisterHours = item.RegisterHours - deltaAgain;
                                        item.AnalyseHour = item.AnalyseHour - deltaAgain;
                                    }
                                    item.udHourByDate = item.udHourByDate - deltaDay;
                                    //item.udHourByWeek_Validate = item.udHourByWeek_Validate - deltaDay;
                                    //item.udHourByMonth_Validate = item.udHourByMonth_Validate - deltaDay;
                                    //item.udHourByYear_Validate = item.udHourByYear_Validate - deltaDay;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void RoundOT(List<Att_OvertimeEntity> lstOvertime, string userLogin)
        {
            if (lstOvertime.Count == 0)
                return;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_OvertimeType = new CustomBaseRepository<Cat_OvertimeType>(unitOfWork);
                string HRM_ATT_OT_ROUNDOT = AppConfig.HRM_ATT_ROUNDOT.ToString();
                string status = string.Empty;
                List<object> lstO2 = new List<object>();
                lstO2.Add(HRM_ATT_OT_ROUNDOT);
                lstO2.Add(null);
                lstO2.Add(null);

                var config = GetData<Sys_AllSettingEntity>(lstO2, ConstantSql.hrm_sys_sp_get_AllSetting, userLogin, ref status);
                var configRoundOT = config.Where(s => s.Name == AppConfig.HRM_ATT_ROUNDOT.ToString()).FirstOrDefault();
                var configRoundOT_LineRound = config.Where(s => s.Name == AppConfig.HRM_ATT_ROUNDOT_LINEROUND.ToString()).FirstOrDefault();

                if (configRoundOT != null && configRoundOT.Value1 != null && configRoundOT.Value1 == bool.TrueString && configRoundOT_LineRound.Value1 != null)
                {
                    double LineRound = 0;
                    double.TryParse(configRoundOT_LineRound.Value1.ToString(), out LineRound);
                    foreach (var item in lstOvertime)
                    {
                        double registerHour = item.RegisterHours;
                        double value = Common.RoundMinuteDown(registerHour * 60, LineRound);
                        item.RegisterHours = value / 60;

                        double AnalyseHour = item.AnalyseHour ?? 0;
                        AnalyseHour = Common.RoundMinuteDown(AnalyseHour * 60, LineRound);
                        item.AnalyseHour = AnalyseHour / 60;

                        double HourEveryDay = item.udHourByDate ?? 0;
                        if (HourEveryDay > 0)
                        {
                            HourEveryDay = Common.RoundMinuteDown(HourEveryDay * 60, LineRound);
                            item.udHourByDate = HourEveryDay / 60;
                        }
                        double udHourByWeek = item.udHourByWeek ?? 0;
                        if (udHourByWeek > 0)
                        {
                            udHourByWeek = Common.RoundMinuteDown(udHourByWeek * 60, LineRound);
                            item.udHourByWeek = udHourByWeek / 60;
                        }
                        double udHourByMonth = item.udHourByMonth ?? 0;
                        if (udHourByMonth > 0)
                        {
                            udHourByMonth = Common.RoundMinuteDown(udHourByMonth * 60, LineRound);
                            item.udHourByMonth = udHourByMonth / 60;
                        }
                        double udHourByYear = item.udHourByYear ?? 0;
                        if (udHourByYear > 0)
                        {
                            udHourByYear = Common.RoundMinuteDown(udHourByYear * 60, LineRound);
                            item.udHourByYear = udHourByYear / 60;
                        }
                    }

                }
            }
        }

        //Cập nhật lại cho OT voi nhung trang thai can thiet
        private void CalOverAllowOT(Att_OvertimeEntity Overtime, Double dateHour, Double weekHour, double monthHour, double yearHour, OvertimePermitEntity OtPermit)
        {
            //Theo thu tu Uu tien tu tren xuong duoi
            Overtime.udHourByDate = dateHour;
            Overtime.udHourByWeek = weekHour;
            Overtime.udHourByMonth = monthHour;
            Overtime.udHourByYear = yearHour;

            string Status = OverTimeOverLimitType.E_NONE.ToString();
            #region du lieu thong thuong
            if (OtPermit.limitHour_ByYear_Lev2 != null && yearHour > OtPermit.limitHour_ByYear_Lev2.Value)
            {
                Status = OverTimeOverLimitType.E_YEAR_LV2.ToString();
                Overtime.udIsLimitHourLv2 = true;
            }
            else if (OtPermit.limitHour_ByMonth_Lev2 != null && monthHour > OtPermit.limitHour_ByMonth_Lev2.Value)
            {
                Status = OverTimeOverLimitType.E_MONTH_LV2.ToString();
                Overtime.udIsLimitHourLv2 = true;
            }
            else if (OtPermit.limitHour_ByWeek_Lev2 != null && weekHour > OtPermit.limitHour_ByWeek_Lev2.Value)
            {
                Status = OverTimeOverLimitType.E_WEEK_LV2.ToString();
                Overtime.udIsLimitHourLv2 = true;
            }
            else if (OtPermit.limitHour_ByDay_Lev2 != null && dateHour > OtPermit.limitHour_ByDay_Lev2.Value)
            {
                Status = OverTimeOverLimitType.E_DAY_LV2.ToString();
                Overtime.udIsLimitHourLv2 = true;
            }
            else if (OtPermit.limitHour_ByYear_Lev1 != null && yearHour > OtPermit.limitHour_ByYear_Lev1.Value)
            {
                Status = OverTimeOverLimitType.E_YEAR_LV1.ToString();
                Overtime.udIsLimitHourLv1 = true;
            }
            else if (OtPermit.limitHour_ByMonth_Lev1 != null && monthHour > OtPermit.limitHour_ByMonth_Lev1.Value)
            {
                Status = OverTimeOverLimitType.E_MONTH_LV1.ToString();
                Overtime.udIsLimitHourLv1 = true;
            }
            else if (OtPermit.limitHour_ByWeek_Lev1 != null && weekHour > OtPermit.limitHour_ByWeek_Lev1.Value)
            {
                Status = OverTimeOverLimitType.E_WEEK_LV1.ToString();
                Overtime.udIsLimitHourLv1 = true;
            }
            else if (OtPermit.limitHour_ByDay_Lev1 != null && dateHour > OtPermit.limitHour_ByDay_Lev1.Value)
            {
                Status = OverTimeOverLimitType.E_DAY_LV1.ToString();
                Overtime.udIsLimitHourLv1 = true;
            }
            else if (OtPermit.limitHour_ByYear != null && yearHour > OtPermit.limitHour_ByYear.Value)
            {
                Status = OverTimeOverLimitType.E_YEAR.ToString();
                Overtime.udIsLimitHour = true;
            }
            else if (OtPermit.limitHour_ByMonth != null && monthHour > OtPermit.limitHour_ByMonth.Value)
            {
                Status = OverTimeOverLimitType.E_MONTH.ToString();
                Overtime.udIsLimitHour = true;
            }
            else if (OtPermit.limitHour_ByWeek != null && weekHour > OtPermit.limitHour_ByWeek.Value)
            {
                Status = OverTimeOverLimitType.E_WEEK.ToString();
                Overtime.udIsLimitHour = true;
            }
            else if (OtPermit.limitHour_ByDay != null && dateHour > OtPermit.limitHour_ByDay.Value)
            {
                Status = OverTimeOverLimitType.E_DAY.ToString();
                Overtime.udIsLimitHour = true;
            }
            #endregion
            Overtime.udLimitHourType = Status;

        }


        /// <summary>
        /// Hàm Phân tích OT voi những vi pham vượt trần
        /// </summary>
        /// <param name="Overtime">OT</param>
        /// <param name="dateHour">Giờ từ bắt đầu ngày đến kết thúc (đã có giơ OT hiện tại trong đó)</param>
        /// <param name="weekHour">Giờ từ bắt đầu tuần đến kết thúc  (đã có giơ OT hiện tại trong đó) </param>
        /// <param name="monthHour">Giờ từ bắt đầu tháng đến kết thúc  (đã có giơ OT hiện tại trong đó)</param>
        /// <param name="yearHour">Giờ từ bắt đầu năm đến kết thúc  (đã có giơ OT hiện tại trong đó)</param>
        /// <param name="OtPermit">Những thông số vượt trần</param>
        private void CalOverAllowOT(Att_OvertimeEntity Overtime, Double weekHour, double monthHour, double yearHour, OvertimePermitEntity OtPermit)
        {
            //Theo thu tu Uu tien tu tren xuong duoi
            string Status = OverTimeOverLimitType.E_NONE.ToString();
            #region du lieu thong thuong
            if (OtPermit.limitHour_ByYear_Lev2 != null && yearHour > OtPermit.limitHour_ByYear_Lev2.Value)
            {
                Status = OverTimeOverLimitType.E_YEAR_LV2.ToString();
                Overtime.udIsLimitHourLv2_Validate = true;
            }
            else if (OtPermit.limitHour_ByMonth_Lev2 != null && monthHour > OtPermit.limitHour_ByMonth_Lev2.Value)
            {
                Status = OverTimeOverLimitType.E_MONTH_LV2.ToString();
                Overtime.udIsLimitHourLv2_Validate = true;
            }
            else if (OtPermit.limitHour_ByWeek_Lev2 != null && weekHour > OtPermit.limitHour_ByWeek_Lev2.Value)
            {
                Status = OverTimeOverLimitType.E_WEEK_LV2.ToString();
                Overtime.udIsLimitHourLv2_Validate = true;
            }
            else if (OtPermit.limitHour_ByDay_Lev2 != null && Overtime.udHourByDate > OtPermit.limitHour_ByDay_Lev2.Value)
            {
                Status = OverTimeOverLimitType.E_DAY_LV2.ToString();
                Overtime.udIsLimitHourLv2_Validate = true;
            }
            else if (OtPermit.limitHour_ByYear_Lev1 != null && yearHour > OtPermit.limitHour_ByYear_Lev1.Value)
            {
                Status = OverTimeOverLimitType.E_YEAR_LV1.ToString();
                Overtime.udIsLimitHourLv1_Validate = true;
            }
            else if (OtPermit.limitHour_ByMonth_Lev1 != null && monthHour > OtPermit.limitHour_ByMonth_Lev1.Value)
            {
                Status = OverTimeOverLimitType.E_MONTH_LV1.ToString();
                Overtime.udIsLimitHourLv1_Validate = true;
            }
            else if (OtPermit.limitHour_ByWeek_Lev1 != null && weekHour > OtPermit.limitHour_ByWeek_Lev1.Value)
            {
                Status = OverTimeOverLimitType.E_WEEK_LV1.ToString();
                Overtime.udIsLimitHourLv1_Validate = true;
            }
            else if (OtPermit.limitHour_ByDay_Lev1 != null && Overtime.udHourByDate > OtPermit.limitHour_ByDay_Lev1.Value)
            {
                Status = OverTimeOverLimitType.E_DAY_LV1.ToString();
                Overtime.udIsLimitHourLv1_Validate = true;
            }
            else if (OtPermit.limitHour_ByYear != null && yearHour > OtPermit.limitHour_ByYear.Value)
            {
                Status = OverTimeOverLimitType.E_YEAR.ToString();
                Overtime.udIsLimitHour_Validate = true;
            }
            else if (OtPermit.limitHour_ByMonth != null && monthHour > OtPermit.limitHour_ByMonth.Value)
            {
                Status = OverTimeOverLimitType.E_MONTH.ToString();
                Overtime.udIsLimitHour_Validate = true;
            }
            else if (OtPermit.limitHour_ByWeek != null && weekHour > OtPermit.limitHour_ByWeek.Value)
            {
                Status = OverTimeOverLimitType.E_WEEK.ToString();
                Overtime.udIsLimitHour_Validate = true;
            }
            else if (OtPermit.limitHour_ByDay != null && Overtime.udHourByDate > OtPermit.limitHour_ByDay.Value)
            {
                Status = OverTimeOverLimitType.E_DAY.ToString();
                Overtime.udIsLimitHour_Validate = true;
            }
            #endregion
            Overtime.udLimitHourType_Validate = Status;
        }

        /// <summary>
        /// Hàm phân tích tăng ca
        /// </summary>
        /// <param name="lstWorkday">Ds Overtime đang đăng ký cần phân tích</param>
        /// <param name="lstShift">Ds ca làm việc </param>
        /// <param name="lstShiftItem">Ds ca làm việc Item </param>
        /// <param name="lstDayOff">Ds ngày nghỉ của Cty đã được lọc theo thời gian</param>
        /// <param name="lstOvertimeType">Ds loại OT</param>
        /// <param name="OvertimeInfoFillterAnalyze">Các info để tính toán phân tích tăng ca </param>
        /// <returns></returns>
        public List<Att_OvertimeEntity> StartAnalyzeOvertime(List<Att_OvertimeEntity> lstOvertime, List<Cat_ShiftEntity> lstShift, List<Cat_ShiftItemEntity> lstShiftItem,
           List<Cat_DayOffEntity> lstDayOff, List<Cat_OvertimeTypeEntity> lstOvertimeType, Att_OvertimeInfoFillterAnalyze OvertimeInfoFillterAnalyze, string userLogin)
        {
            List<WorkdayCustom> lstWorkday = new List<WorkdayCustom>();
            WorkdayCustom Workday = new WorkdayCustom();
            foreach (var item in lstOvertime)
            {
                if (item.ProfileID == null || item.ShiftID == null || item.WorkDate == null || item.RegisterHours == null)
                    continue;

                Workday.ProfileID = item.ProfileID;
                Workday.ShiftID = item.ShiftID;
                Workday.WorkDate = item.WorkDate.Date;
                Workday.InTime1 = item.WorkDate;
                Workday.OutTime1 = item.WorkDate.AddHours(item.RegisterHours);
                lstWorkday.Add(Workday);
            }
            var rs = AnalyzeOvertime(lstWorkday, lstShift, lstShiftItem, lstDayOff, lstOvertimeType, OvertimeInfoFillterAnalyze, userLogin);
            return rs;
        }

        /// <summary>
        /// Hàm phân tích tăng ca
        /// </summary>
        /// <param name="lstWorkday">Ds Workday đã được lọc theo ngày tháng , nhân viên v.v</param>
        /// <param name="lstShift">Ds ca làm việc </param>
        /// <param name="lstShiftItem">Ds ca làm việc Item </param>
        /// <param name="lstDayOff">Ds ngày nghỉ của Cty đã được lọc theo thời gian</param>
        /// <param name="lstOvertimeType">Ds loại OT</param>
        /// <param name="OvertimeInfoFillterAnalyze">Các info để tính toán phân tích tăng ca </param>
        /// <returns></returns>
        public List<Att_OvertimeEntity> AnalyzeOvertime(List<WorkdayCustom> lstWorkday, List<Cat_ShiftEntity> lstShift, List<Cat_ShiftItemEntity> lstShiftItem, List<Cat_DayOffEntity> lstDayOff, List<Cat_OvertimeTypeEntity> lstOvertimeType, Att_OvertimeInfoFillterAnalyze OvertimeInfoFillterAnalyze, string userLogin)
        {
            string status = string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Pregnancy = new CustomBaseRepository<Att_Pregnancy>(unitOfWork);

                string E_OVERTIME = ShiftItemType.E_OVERTIME.ToString();
                List<Att_OvertimeEntity> lstOvertimeInsert = new List<Att_OvertimeEntity>();
                #region 1 số thông tin loc dữ liệu

                bool isAllowGetOTOutterShift = OvertimeInfoFillterAnalyze.isAllowGetOTOutterShift;
                bool isAllowGetBeforeShift = OvertimeInfoFillterAnalyze.isAllowGetBeforeShift;
                bool isAllowGetAfterShift = OvertimeInfoFillterAnalyze.isAllowGetAfterShift;
                bool isAllowGetInShift = OvertimeInfoFillterAnalyze.isAllowGetInShift;

                if (isAllowGetBeforeShift == false && isAllowGetAfterShift == false && isAllowGetInShift == false)
                    return lstOvertimeInsert;

                bool isAllowGetTypeBaseOnActualDate = OvertimeInfoFillterAnalyze.isAllowGetTypeBaseOnActualDate;
                bool isAllowGetTypeBaseOnBeginShift = OvertimeInfoFillterAnalyze.isAllowGetTypeBaseOnBeginShift;
                bool isAllowGetTypeBaseOnEndShift = OvertimeInfoFillterAnalyze.isAllowGetTypeBaseOnEndShift;
                bool isAllowGetNightShift = OvertimeInfoFillterAnalyze.isAllowGetNightShift;
                bool isBreakMiddleNight = OvertimeInfoFillterAnalyze.isBreakMiddleNight;
                double MininumOvertimeHour = OvertimeInfoFillterAnalyze.MininumOvertimeHour;

                string HRM_ATT_OT_OTPERMIT_PRENATALDONTREGISTEROT = AppConfig.HRM_ATT_OT_ALLOWREGISTEROTWHENINMATERNITYREGIME.ToString();
                List<object> lstREGISTEROT = new List<object>();
                lstREGISTEROT.Add(HRM_ATT_OT_OTPERMIT_PRENATALDONTREGISTEROT);
                lstREGISTEROT.Add(null);
                lstREGISTEROT.Add(null);
                var configTREGISTEROT = GetData<Sys_AllSettingEntity>(lstREGISTEROT, ConstantSql.hrm_sys_sp_get_AllSetting, userLogin, ref status).FirstOrDefault();

                var lstPrenacy = new List<Att_Pregnancy>().Select(m => new { m.ProfileID, m.DateStart, m.DateEnd });
                if (configTREGISTEROT != null && configTREGISTEROT.Value1 == bool.TrueString && lstWorkday.Count > 0)
                {
                    DateTime DateMin = lstWorkday.Min(m => m.WorkDate);
                    DateTime DateMax = lstWorkday.Max(m => m.WorkDate);
                    List<Guid> lstProfileId = lstWorkday.Select(m => m.ProfileID).Distinct().ToList();
                    //lstPrenacy = repoAtt_Pregnancy
                    //    .FindBy(m => m.DateStart <= DateMax && m.DateEnd > DateMin && lstProfileId.Contains(m.ProfileID))
                    //    .Select(m => new { m.ProfileID, m.DateStart, m.DateEnd })
                    //    .ToList();
                    if (lstProfileId.Count < 2000)
                    {
                        lstPrenacy = repoAtt_Pregnancy
                            .FindBy(m => m.IsDelete == null && m.DateStart <= DateMax && m.DateEnd > DateMin && lstProfileId.Contains(m.ProfileID)).Select(m => new { m.ProfileID, m.DateStart, m.DateEnd }).ToList();
                    }
                    else
                    {
                        lstPrenacy = repoAtt_Pregnancy
                            .FindBy(m => m.IsDelete == null && m.DateStart <= DateMax && m.DateEnd > DateMin).Select(m => new { m.ProfileID, m.DateStart, m.DateEnd }).ToList();
                    }
                }

                #endregion
                foreach (var Workday in lstWorkday)
                {
                    Guid? ShiftID = Workday.ShiftApprove ?? Workday.ShiftID;
                    Cat_ShiftEntity ShiftByWorkday = lstShift.Where(m => m.ID == ShiftID).FirstOrDefault();
                    DateTime? In1 = Workday.InTime1;
                    DateTime? In2 = Workday.InTime2;
                    DateTime? In3 = Workday.InTime3;
                    DateTime? In4 = Workday.InTime4;
                    DateTime? Out1 = Workday.OutTime1;
                    DateTime? Out2 = Workday.OutTime2;
                    DateTime? Out3 = Workday.OutTime3;
                    DateTime? Out4 = Workday.OutTime4;

                    bool isNonShift = false;
                    DateTime Workdate = Workday.WorkDate;
                    //
                    if (Workday.Status == WorkdayType.E_DETECTED_SHIFT.ToString() || Workday.ShiftID == null || Workday.ShiftID == Guid.Empty)
                        isNonShift = true;

                    if (ShiftByWorkday == null)
                    {
                        continue;
                    }
                    Guid ProfileID = Workday.ProfileID;
                    if (configTREGISTEROT != null && configTREGISTEROT.Value1 == bool.TrueString &&
                        lstPrenacy.Any(m => m.ProfileID == ProfileID && m.DateStart <= Workdate && m.DateEnd > Workdate)) //Người Thai phụ Ko được Đk Tăng Ca
                    {
                        continue;
                    }

                    List<Cat_ShiftItemEntity> lstShiftItemByShift = lstShiftItem.Where(m => m.ShiftID == ShiftByWorkday.ID && m.ShiftItemType == E_OVERTIME).ToList();
                    if (lstShiftItemByShift.Count > 0 && isAllowGetOTOutterShift == false) //Tang ca theo cai gio quy dinh cua item trong ca
                    {
                        foreach (var ShiftItem in lstShiftItemByShift)
                        {
                            if (ShiftItem.CoFrom == null || ShiftItem.CoTo == null)
                                continue;
                            DateTime DateBeginAllowOT = Workday.WorkDate.AddHours(ShiftItem.CoFrom);
                            DateTime DateEndAllowOT = Workday.WorkDate.AddHours(ShiftItem.CoTo);
                            if (isNonShift)
                            {
                                DateBeginAllowOT = DateBeginAllowOT.Date;
                                DateEndAllowOT = DateBeginAllowOT.Date;
                            }
                            DateTime? BeginOT = null;
                            DateTime? EndOT = null;
                            #region cặp thứ 1
                            if (In1 != null && Out1 != null) // Tính cặp thứ 1
                            {
                                if (isHaveOverTimeInner(In1.Value, Out1.Value, DateBeginAllowOT, DateEndAllowOT, out BeginOT, out EndOT, ShiftByWorkday.InOutDynamic))
                                {
                                    if (BeginOT != null && EndOT != null) // Có OT // Bước tiếp theo là tách ra khỏi ca đêm
                                    {
                                        CreateOvertimeAnalyzeNightShift(
                                            lstDayOff, lstOvertimeType, lstOvertimeInsert,
                                            Workday, ShiftByWorkday,
                                            BeginOT, EndOT,
                                            isAllowGetNightShift, MininumOvertimeHour,
                                            isAllowGetTypeBaseOnActualDate, isAllowGetTypeBaseOnBeginShift,
                                            isAllowGetTypeBaseOnEndShift, isNonShift, false, userLogin);
                                    }
                                }
                            }
                            #endregion
                            #region cặp thứ 2
                            if (In2 != null && Out2 != null) // Tính cặp thứ 2
                            {
                                if (isHaveOverTimeInner(In2.Value, Out2.Value, DateBeginAllowOT, DateEndAllowOT, out BeginOT, out EndOT, ShiftByWorkday.InOutDynamic))
                                {
                                    if (BeginOT != null && EndOT != null) // Có OT // Bước tiếp theo là tách ra khỏi ca đêm
                                    {
                                        CreateOvertimeAnalyzeNightShift(
                                            lstDayOff, lstOvertimeType, lstOvertimeInsert,
                                            Workday, ShiftByWorkday,
                                            BeginOT, EndOT,
                                            isAllowGetNightShift, MininumOvertimeHour,
                                            isAllowGetTypeBaseOnActualDate, isAllowGetTypeBaseOnBeginShift,
                                            isAllowGetTypeBaseOnEndShift, isNonShift, false, userLogin);
                                    }
                                }
                            }
                            #endregion
                            #region cặp thứ 3
                            if (In3 != null && Out3 != null) // Tính cặp thứ 3
                            {
                                if (isHaveOverTimeInner(In3.Value, Out3.Value, DateBeginAllowOT, DateEndAllowOT, out BeginOT, out EndOT, ShiftByWorkday.InOutDynamic))
                                {
                                    if (BeginOT != null && EndOT != null) // Có OT // Bước tiếp theo là tách ra khỏi ca đêm
                                    {
                                        CreateOvertimeAnalyzeNightShift(
                                            lstDayOff, lstOvertimeType, lstOvertimeInsert, Workday, ShiftByWorkday,
                                            BeginOT, EndOT, isAllowGetNightShift,
                                            MininumOvertimeHour, isAllowGetTypeBaseOnActualDate,
                                            isAllowGetTypeBaseOnBeginShift, isAllowGetTypeBaseOnEndShift, isNonShift, false, userLogin);

                                    }
                                }
                            }
                            #endregion
                            #region cặp thứ 4
                            if (In4 != null && Out4 != null) // Tính cặp thứ 4
                            {
                                if (isHaveOverTimeInner(In4.Value, Out4.Value, DateBeginAllowOT, DateEndAllowOT, out BeginOT, out EndOT, ShiftByWorkday.InOutDynamic))
                                {
                                    if (BeginOT != null && EndOT != null) // Có OT // Bước tiếp theo là tách ra khỏi ca đêm
                                    {
                                        CreateOvertimeAnalyzeNightShift(
                                            lstDayOff, lstOvertimeType, lstOvertimeInsert,
                                            Workday, ShiftByWorkday,
                                            BeginOT, EndOT, isAllowGetNightShift, MininumOvertimeHour,
                                            isAllowGetTypeBaseOnActualDate, isAllowGetTypeBaseOnBeginShift,
                                            isAllowGetTypeBaseOnEndShift, isNonShift, false, userLogin);

                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    else if (isAllowGetOTOutterShift) // Tang ca theo cai gio dau ca cuoi ca
                    {
                        DateTime BeginShift = Workday.WorkDate.Date.AddHours(ShiftByWorkday.InTime.Hour).AddMinutes(ShiftByWorkday.InTime.Minute);
                        DateTime EndShift = BeginShift.AddHours(ShiftByWorkday.CoOut);
                        if (isNonShift)
                        {
                            BeginShift = BeginShift.Date;
                            EndShift = BeginShift.Date;
                        }
                        #region Cặp thứ 1
                        if (In1 != null && Out1 != null)
                        {
                            DateTime? DateStartOT_Infirst;
                            DateTime? DateEndOT_InFirst;
                            DateTime? DateStartOT_InLast;
                            DateTime? DateEndOT_InLast;
                            DateTime? DateStartOT_InShift;
                            DateTime? DateEndOT_InShift;

                            if (isHaveOverTimeOutter(In1.Value, Out1.Value, BeginShift, EndShift, out DateStartOT_Infirst, out DateEndOT_InFirst, out DateStartOT_InLast, out DateEndOT_InLast, ShiftByWorkday.InOutDynamic))
                            {
                                //Có OT
                                #region cặp đầu ca
                                if (DateStartOT_Infirst != null && DateEndOT_InFirst != null && isAllowGetBeforeShift)
                                {
                                    CreateOvertimeAnalyzeNightShift(
                                        lstDayOff, lstOvertimeType, lstOvertimeInsert,
                                        Workday, ShiftByWorkday,
                                        DateStartOT_Infirst, DateEndOT_InFirst, isAllowGetNightShift, MininumOvertimeHour,
                                        isAllowGetTypeBaseOnActualDate, isAllowGetTypeBaseOnBeginShift, isAllowGetTypeBaseOnEndShift, isNonShift, false, userLogin);
                                }
                                #endregion

                                #region cặp cuối ca
                                if (DateStartOT_InLast != null && DateEndOT_InLast != null && isAllowGetAfterShift)
                                {
                                    CreateOvertimeAnalyzeNightShift(
                                        lstDayOff, lstOvertimeType, lstOvertimeInsert, Workday, ShiftByWorkday,
                                        DateStartOT_InLast, DateEndOT_InLast, isAllowGetNightShift, MininumOvertimeHour,
                                        isAllowGetTypeBaseOnActualDate, isAllowGetTypeBaseOnBeginShift, isAllowGetTypeBaseOnEndShift, isNonShift, false, userLogin);
                                }
                                #endregion


                            }
                            #region cặp trong ca
                            if (isAllowGetInShift)
                            {
                                if (In1 >= BeginShift)
                                {
                                    DateStartOT_InShift = In1;
                                }
                                else
                                {
                                    DateStartOT_InShift = BeginShift;
                                }

                                if (Out1 < EndShift)
                                {
                                    DateEndOT_InShift = Out1;
                                }
                                else
                                {
                                    DateEndOT_InShift = EndShift;
                                }
                                CreateOvertimeAnalyzeNightShift(
                                    lstDayOff, lstOvertimeType, lstOvertimeInsert, Workday, ShiftByWorkday, DateStartOT_InShift,
                                    DateEndOT_InShift, isAllowGetNightShift, MininumOvertimeHour, isAllowGetTypeBaseOnActualDate,
                                    isAllowGetTypeBaseOnBeginShift, isAllowGetTypeBaseOnEndShift, isNonShift, false, userLogin);
                            }
                            #endregion
                        }
                        #endregion
                        #region Cặp thứ 2
                        if (In2 != null && Out2 != null)
                        {
                            DateTime? DateStartOT_Infirst;
                            DateTime? DateEndOT_InFirst;
                            DateTime? DateStartOT_InLast;
                            DateTime? DateEndOT_InLast;
                            DateTime? DateStartOT_InShift;
                            DateTime? DateEndOT_InShift;

                            if (isHaveOverTimeOutter(In2.Value, Out2.Value, BeginShift, EndShift, out DateStartOT_Infirst, out DateEndOT_InFirst, out DateStartOT_InLast, out DateEndOT_InLast, ShiftByWorkday.InOutDynamic))
                            {
                                //Có OT
                                #region cặp đầu ca
                                if (DateStartOT_Infirst != null && DateEndOT_InFirst != null && isAllowGetBeforeShift)
                                {
                                    CreateOvertimeAnalyzeNightShift(
                                        lstDayOff, lstOvertimeType, lstOvertimeInsert,
                                        Workday, ShiftByWorkday, DateStartOT_Infirst, DateEndOT_InFirst,
                                        isAllowGetNightShift, MininumOvertimeHour, isAllowGetTypeBaseOnActualDate, isAllowGetTypeBaseOnBeginShift,
                                        isAllowGetTypeBaseOnEndShift, isNonShift, false, userLogin);
                                }
                                #endregion

                                #region cặp cuối ca
                                if (DateStartOT_InLast != null && DateEndOT_InLast != null && isAllowGetAfterShift)
                                {
                                    CreateOvertimeAnalyzeNightShift(
                                        lstDayOff, lstOvertimeType, lstOvertimeInsert,
                                        Workday, ShiftByWorkday, DateStartOT_InLast, DateEndOT_InLast,
                                        isAllowGetNightShift, MininumOvertimeHour, isAllowGetTypeBaseOnActualDate, isAllowGetTypeBaseOnBeginShift,
                                        isAllowGetTypeBaseOnEndShift, isNonShift, false, userLogin);
                                }
                                #endregion


                            }
                            #region cặp trong ca
                            if (isAllowGetInShift)
                            {
                                if (In2 >= BeginShift)
                                {
                                    DateStartOT_InShift = In2;
                                }
                                else
                                {
                                    DateStartOT_InShift = BeginShift;
                                }

                                if (Out2 < EndShift)
                                {
                                    DateEndOT_InShift = Out2;
                                }
                                else
                                {
                                    DateEndOT_InShift = EndShift;
                                }
                                CreateOvertimeAnalyzeNightShift(
                                    lstDayOff, lstOvertimeType, lstOvertimeInsert,
                                    Workday, ShiftByWorkday, DateStartOT_InShift, DateEndOT_InShift, isAllowGetNightShift, MininumOvertimeHour,
                                    isAllowGetTypeBaseOnActualDate, isAllowGetTypeBaseOnBeginShift, isAllowGetTypeBaseOnEndShift, isNonShift, false, userLogin);
                            }
                            #endregion
                        }
                        #endregion
                        #region Cặp thứ 3
                        if (In3 != null && Out3 != null)
                        {
                            DateTime? DateStartOT_Infirst;
                            DateTime? DateEndOT_InFirst;
                            DateTime? DateStartOT_InLast;
                            DateTime? DateEndOT_InLast;
                            DateTime? DateStartOT_InShift;
                            DateTime? DateEndOT_InShift;

                            if (isHaveOverTimeOutter(In3.Value, Out3.Value, BeginShift, EndShift, out DateStartOT_Infirst, out DateEndOT_InFirst, out DateStartOT_InLast, out DateEndOT_InLast, ShiftByWorkday.InOutDynamic))
                            {
                                //Có OT
                                #region cặp đầu ca
                                if (DateStartOT_Infirst != null && DateEndOT_InFirst != null && isAllowGetBeforeShift)
                                {
                                    CreateOvertimeAnalyzeNightShift(
                                        lstDayOff, lstOvertimeType, lstOvertimeInsert,
                                        Workday, ShiftByWorkday, DateStartOT_Infirst, DateEndOT_InFirst,
                                        isAllowGetNightShift, MininumOvertimeHour, isAllowGetTypeBaseOnActualDate,
                                        isAllowGetTypeBaseOnBeginShift, isAllowGetTypeBaseOnEndShift, isNonShift, false, userLogin);
                                }
                                #endregion

                                #region cặp cuối ca
                                if (DateStartOT_InLast != null && DateEndOT_InLast != null && isAllowGetAfterShift)
                                {
                                    CreateOvertimeAnalyzeNightShift(
                                        lstDayOff, lstOvertimeType, lstOvertimeInsert,
                                        Workday, ShiftByWorkday, DateStartOT_InLast, DateEndOT_InLast,
                                        isAllowGetNightShift, MininumOvertimeHour, isAllowGetTypeBaseOnActualDate,
                                        isAllowGetTypeBaseOnBeginShift, isAllowGetTypeBaseOnEndShift, isNonShift, false, userLogin);
                                }
                                #endregion


                            }
                            #region cặp trong ca
                            if (isAllowGetInShift)
                            {
                                if (In3 >= BeginShift)
                                {
                                    DateStartOT_InShift = In3;
                                }
                                else
                                {
                                    DateStartOT_InShift = BeginShift;
                                }

                                if (Out3 < EndShift)
                                {
                                    DateEndOT_InShift = Out3;
                                }
                                else
                                {
                                    DateEndOT_InShift = EndShift;
                                }
                                CreateOvertimeAnalyzeNightShift(
                                    lstDayOff, lstOvertimeType, lstOvertimeInsert, Workday, ShiftByWorkday,
                                    DateStartOT_InShift, DateEndOT_InShift, isAllowGetNightShift, MininumOvertimeHour,
                                    isAllowGetTypeBaseOnActualDate, isAllowGetTypeBaseOnBeginShift,
                                    isAllowGetTypeBaseOnEndShift, isNonShift, false, userLogin);
                            }
                            #endregion
                        }
                        #endregion
                        #region Cặp thứ 4
                        if (In4 != null && Out4 != null)
                        {
                            DateTime? DateStartOT_Infirst;
                            DateTime? DateEndOT_InFirst;
                            DateTime? DateStartOT_InLast;
                            DateTime? DateEndOT_InLast;
                            DateTime? DateStartOT_InShift;
                            DateTime? DateEndOT_InShift;

                            if (isHaveOverTimeOutter(In4.Value, Out4.Value, BeginShift, EndShift, out DateStartOT_Infirst, out DateEndOT_InFirst, out DateStartOT_InLast, out DateEndOT_InLast, ShiftByWorkday.InOutDynamic))
                            {
                                //Có OT
                                #region cặp đầu ca
                                if (DateStartOT_Infirst != null && DateEndOT_InFirst != null && isAllowGetBeforeShift)
                                {
                                    CreateOvertimeAnalyzeNightShift(
                                        lstDayOff, lstOvertimeType, lstOvertimeInsert,
                                        Workday, ShiftByWorkday, DateStartOT_Infirst, DateEndOT_InFirst,
                                        isAllowGetNightShift, MininumOvertimeHour, isAllowGetTypeBaseOnActualDate,
                                        isAllowGetTypeBaseOnBeginShift, isAllowGetTypeBaseOnEndShift, isNonShift, false, userLogin);
                                }
                                #endregion

                                #region cặp cuối ca
                                if (DateStartOT_InLast != null && DateEndOT_InLast != null && isAllowGetAfterShift)
                                {
                                    CreateOvertimeAnalyzeNightShift(
                                        lstDayOff, lstOvertimeType, lstOvertimeInsert,
                                        Workday, ShiftByWorkday, DateStartOT_InLast, DateEndOT_InLast,
                                        isAllowGetNightShift, MininumOvertimeHour, isAllowGetTypeBaseOnActualDate,
                                        isAllowGetTypeBaseOnBeginShift, isAllowGetTypeBaseOnEndShift, isNonShift, false, userLogin);
                                }
                                #endregion


                            }
                            #region cặp trong ca
                            if (isAllowGetInShift)
                            {
                                if (In4 >= BeginShift)
                                {
                                    DateStartOT_InShift = In4;
                                }
                                else
                                {
                                    DateStartOT_InShift = BeginShift;
                                }

                                if (Out4 < EndShift)
                                {
                                    DateEndOT_InShift = Out4;
                                }
                                else
                                {
                                    DateEndOT_InShift = EndShift;
                                }
                                CreateOvertimeAnalyzeNightShift(
                                    lstDayOff, lstOvertimeType, lstOvertimeInsert,
                                    Workday, ShiftByWorkday, DateStartOT_InShift, DateEndOT_InShift,
                                    isAllowGetNightShift, MininumOvertimeHour, isAllowGetTypeBaseOnActualDate,
                                    isAllowGetTypeBaseOnBeginShift, isAllowGetTypeBaseOnEndShift, isNonShift, false, userLogin);
                            }
                            #endregion
                        }
                        #endregion
                    }
                }
                return lstOvertimeInsert;
            }
        }

        /// <summary>
        /// Hàm để tính có OT hay không so với cặp thời gian gắn trong item của Ca (Một số ca chỉ được phép OT trong khoảng thời gian cố định)
        /// </summary>
        /// <param name="DateStart">Dữ liệu IN</param>
        /// <param name="DateEnd">Dữ liệu OUT</param>
        /// <param name="BeginCheck">Giờ bắt đầu được phép OT</param>
        /// <param name="EndCheck">giờ kết thúc được phép OT</param>
        /// <param name="DateStartOT">Giờ bắt đầu OT</param>
        /// <param name="DateEndOT">Giờ kết thúc OT</param>
        /// <returns></returns>
        public bool isHaveOverTimeInner(DateTime DateStart, DateTime DateEnd, DateTime BeginCheck, DateTime EndCheck,
             out DateTime? DateStartOT, out DateTime? DateEndOT, double? DynamicHour)
        {

            bool IsHaveOT = false;
            DateStartOT = null;
            DateEndOT = null;

            if (DynamicHour != null && DynamicHour > 0)
            {
                double Late = (DateStart - BeginCheck).TotalHours;
                if (Late > 0)
                {
                    double delta = Late > DynamicHour.Value ? DynamicHour.Value : Late;
                    DateEnd = DateEnd.AddHours(delta);
                }
            }
            if (DateStart < EndCheck && DateEnd > BeginCheck)
            {
                IsHaveOT = true;
                if (DateStart > BeginCheck)
                {
                    DateStartOT = DateStart;
                }
                else
                {
                    DateStartOT = BeginCheck;
                }
                //----------------
                if (DateEnd < EndCheck)
                {
                    DateEndOT = DateEnd;
                }
                else
                {
                    DateEndOT = EndCheck;
                }
            }


            return IsHaveOT;
        }

        /// <summary>
        /// Hàm để tính có OT hay không và kết quả sẽ kèm theo 2 cặp giá tri OT đầu ca và OT cuối ca
        /// </summary>
        /// <param name="DateStart">Dữ liệu In</param>
        /// <param name="DateEnd">dữ liệu Out</param>
        /// <param name="BeginShift">Giờ bắt đầu của Ca</param>
        /// <param name="EndShift">Giờ kết thúc của Ca</param>
        /// <param name="DateStartOT_Infirst">Giờ bắt đầu OT của đầu ca</param>
        /// <param name="DateEndOT_InFirst">Giờ kết thúc OT của đầu ca</param>
        /// <param name="DateStartOT_InLast">Giờ bắt đầu OT của cuối ca</param>
        /// <param name="DateEndOT_InLast">Giờ kết thúc OT của cuối ca</param>
        /// <returns>boolean</returns>
        public bool isHaveOverTimeOutter(DateTime DateStart, DateTime DateEnd, DateTime BeginShift, DateTime EndShift,
            out DateTime? DateStartOT_Infirst, out DateTime? DateEndOT_InFirst,
            out DateTime? DateStartOT_InLast, out DateTime? DateEndOT_InLast, double? DynamicHourShift)
        {
            DateStartOT_Infirst = null;
            DateEndOT_InFirst = null;
            DateStartOT_InLast = null;
            DateEndOT_InLast = null;
            bool IsHaveOT = false;
            if (DateStart < BeginShift)
            {
                IsHaveOT = true;
                DateStartOT_Infirst = DateStart;
                if (DateEnd > BeginShift)
                {
                    DateEndOT_InFirst = BeginShift;
                }
                else
                {
                    DateEndOT_InFirst = DateEnd;
                }
            }

            //DynamicHourShift: giờ trượt ca - Ca ko cố định và cho phép trượt theo một pham vi đi trễ
            double Late = (DateStart - BeginShift).TotalHours;
            if (DynamicHourShift != null && DynamicHourShift > 0 && Late > 0)
            {
                double deltal = Late > DynamicHourShift.Value ? DynamicHourShift.Value : Late;
                EndShift = EndShift.AddHours(deltal);
            }

            if (DateEnd > EndShift)
            {
                IsHaveOT = true;
                DateEndOT_InLast = DateEnd;
                if (DateStart < EndShift)
                {
                    DateStartOT_InLast = EndShift;
                }
                else
                {
                    DateStartOT_InLast = DateStart;
                }
            }
            return IsHaveOT;
        }

        private void SetStatusLeaveOnWorkday(List<Att_OvertimeEntity> lstOvertime)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_LeaveDay = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);
                var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);

                string status = string.Empty;
                if (lstOvertime == null || lstOvertime.Count == 0)
                    return;
                List<Guid> lstProfileId = lstOvertime.Select(m => m.ProfileID).ToList();
                DateTime DateMin = lstOvertime.Min(m => m.WorkDate);
                DateMin = DateMin.Date;
                DateTime DateMax = lstOvertime.Max(m => m.WorkDate);
                DateMax = DateMax.Date.AddDays(1).AddMinutes(-1);
                string E_CANCEL = LeaveDayStatus.E_CANCEL.ToString();
                string E_REJECTED = LeaveDayStatus.E_REJECTED.ToString();
                //var lstLeaveDay = repoAtt_LeaveDay
                //    .FindBy(m => m.IsDelete == null && m.Status != E_CANCEL && m.Status != E_REJECTED && m.DateStart <= DateMax && m.DateEnd >= DateMin && lstProfileId.Contains(m.ProfileID))
                //    .Select(m => new { m.ID, m.ProfileID, m.DateStart, m.DateEnd, m.DurationType, m.Duration, m.TotalDuration, m.LeaveDayTypeID })
                //    .ToList();
                var lstLeaveDay = new List<Att_LeaveDay>().Select(m => new { m.ID, m.ProfileID, m.DateStart, m.DateEnd, m.DurationType, m.LeaveHours, m.LeaveDays, m.LeaveDayTypeID });
                if (lstProfileId.Count < 2000)
                {
                    lstLeaveDay = repoAtt_LeaveDay
                            .FindBy(m => m.IsDelete == null && m.Status != E_CANCEL && m.Status != E_REJECTED && m.DateStart <= DateMax && m.DateEnd >= DateMin && lstProfileId.Contains(m.ProfileID))
                            .Select(m => new { m.ID, m.ProfileID, m.DateStart, m.DateEnd, m.DurationType, m.LeaveHours, m.LeaveDays, m.LeaveDayTypeID }).ToList();
                }
                else
                {
                    lstLeaveDay = repoAtt_LeaveDay
                            .FindBy(m => m.IsDelete == null && m.Status != E_CANCEL && m.Status != E_REJECTED && m.DateStart <= DateMax && m.DateEnd >= DateMin)
                            .Select(m => new { m.ID, m.ProfileID, m.DateStart, m.DateEnd, m.DurationType, m.LeaveHours, m.LeaveDays, m.LeaveDayTypeID }).ToList();
                }
                var lstLeaveType = repoCat_LeaveDayType
                    .FindBy(s => s.IsDelete == null)
                    .Select(m => new { m.ID, m.Code, m.CodeStatistic })
                    .ToList();
                var lstDayOffHoliday = repoCat_DayOff
                    .FindBy(m => m.IsDelete == null && m.DateOff != null)
                    .Select(m => new { m.DateOff, m.Type })
                    .ToList();
                foreach (var item in lstOvertime)
                {
                    Guid profileId = item.ProfileID;
                    DateTime workDate = item.WorkDate.Date;
                    var Dayoff = lstDayOffHoliday.Where(m => m.DateOff == workDate).FirstOrDefault();
                    if (Dayoff != null)
                    {
                        string code = string.Empty;
                        if (Dayoff.Type == HolidayType.E_HOLIDAY_HLD.ToString())
                        {
                            code = "HLD";
                        }
                        else if (Dayoff.Type == HolidayType.E_WEEKEND_HLD.ToString())
                        {
                            code = "WH";
                        }
                        else
                        {
                            code = "CH";
                        }
                        var leavetype = lstLeaveType.Where(m => m.Code == code).FirstOrDefault();
                        if (leavetype == null)
                        {
                            continue;
                        }
                        string leavedayCode = !string.IsNullOrEmpty(leavetype.CodeStatistic) ? leavetype.CodeStatistic : leavetype.Code;
                        item.udLeaveTypeCode = leavedayCode;
                        continue;
                    }
                    var lstLeaveDayByProfile = lstLeaveDay.Where(m => m.ProfileID == profileId && m.DateStart.Date <= workDate && m.DateEnd.Date >= workDate).ToList();
                    int Num = 0;
                    foreach (var leaveDay in lstLeaveDayByProfile)
                    {
                        Num++;
                        if (Num == 3)
                            break;
                        if (leaveDay != null)
                        {
                            if (Num == 1)
                            {
                                var leavetype = lstLeaveType.Where(m => m.ID == leaveDay.LeaveDayTypeID).FirstOrDefault();
                                string leavedayCode = !string.IsNullOrEmpty(leavetype.CodeStatistic) ? leavetype.CodeStatistic : leavetype.Code;
                                item.udLeaveTypeCode = leavedayCode;
                            }
                            else if (Num == 2)
                            {
                                var leavetype = lstLeaveType.Where(m => m.ID == leaveDay.LeaveDayTypeID).FirstOrDefault();
                                string leavedayCode = !string.IsNullOrEmpty(leavetype.CodeStatistic) ? leavetype.CodeStatistic : leavetype.Code;
                                item.udLeaveTypeCode = leavedayCode;
                            }
                        }
                    }
                }
                return;
            }
        }
        private void SetStatusOvertimeOnWorkday(List<Att_OvertimeEntity> lstOvertime)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Overtime = new CustomBaseRepository<Att_Overtime>(unitOfWork);

                string status = string.Empty;
                if (lstOvertime == null || lstOvertime.Count == 0)
                    return;
                List<Guid> lstProfileId = lstOvertime.Select(m => m.ProfileID).ToList();
                DateTime DateMin = lstOvertime.Min(m => m.WorkDate);
                DateMin = DateMin.Date;
                DateTime DateMax = lstOvertime.Max(m => m.WorkDate);
                DateMax = DateMax.Date.AddDays(1).AddMinutes(-1);
                string E_CANCEL = OverTimeStatus.E_CANCEL.ToString();
                string E_REJECTED = OverTimeStatus.E_REJECTED.ToString();
                //var lstOvertimeInDb = repoAtt_Overtime
                //    .FindBy(m => m.IsDelete == null
                //        && m.Status != E_CANCEL && m.Status != E_REJECTED 
                //        && m.WorkDate >= DateMin && m.WorkDate <= DateMax 
                //        && lstProfileId.Contains(m.ProfileID))
                //    .Select(m => new { m.ID, m.ProfileID, m.WorkDate, m.RegisterHours, m.ApproveHours, m.ConfirmHours, m.Status })
                //    .ToList();
                var lstOvertimeInDb = new List<Att_Overtime>().Select(m => new { m.ID, m.ProfileID, m.WorkDate, m.RegisterHours, m.ApproveHours, m.ConfirmHours, m.Status, m.IsNonOvertime });
                if (lstProfileId.Count < 2000)
                {
                    lstOvertimeInDb = repoAtt_Overtime
                            .FindBy(m => m.IsDelete == null && m.Status != E_CANCEL && m.Status != E_REJECTED && m.WorkDate >= DateMin && m.WorkDate <= DateMax && lstProfileId.Contains(m.ProfileID))
                            .Select(m => new { m.ID, m.ProfileID, m.WorkDate, m.RegisterHours, m.ApproveHours, m.ConfirmHours, m.Status, m.IsNonOvertime }).ToList();
                }
                else
                {
                    lstOvertimeInDb = repoAtt_Overtime
                            .FindBy(m => m.IsDelete == null && m.Status != E_CANCEL && m.Status != E_REJECTED && m.WorkDate >= DateMin && m.WorkDate <= DateMax)
                            .Select(m => new { m.ID, m.ProfileID, m.WorkDate, m.RegisterHours, m.ApproveHours, m.ConfirmHours, m.Status, m.IsNonOvertime }).ToList();
                }

                foreach (var item in lstOvertime)
                {
                    Guid profileId = item.ProfileID;
                    DateTime workDateStart = item.WorkDate;
                    DateTime workDateEnd = workDateStart.AddHours(item.RegisterHours);
                    var lstOvertimeByProfile = lstOvertimeInDb.Where(m => m.ProfileID == profileId && m.Status == OverTimeStatus.E_CONFIRM.ToString()
                        && m.WorkDate <= workDateEnd && m.WorkDate.AddHours(m.ConfirmHours) >= workDateStart).ToList();
                    lstOvertimeByProfile.AddRange(lstOvertimeInDb.Where(m => m.ProfileID == profileId && m.Status == OverTimeStatus.E_APPROVED.ToString()
                        && m.WorkDate <= workDateEnd && m.ApproveHours != null && m.WorkDate.AddHours(m.ApproveHours.Value) >= workDateStart).ToList());
                    lstOvertimeByProfile.AddRange(lstOvertimeInDb.Where(m => m.ProfileID == profileId && m.Status != OverTimeStatus.E_APPROVED.ToString() && m.Status != OverTimeStatus.E_CONFIRM.ToString()
                        && m.WorkDate <= workDateEnd && m.WorkDate.AddHours(m.RegisterHours) >= workDateStart).ToList());
                    if (lstOvertimeByProfile.Count > 0 && lstOvertimeByProfile[0].Status != null)
                    {
                        item.udOvertimeStatus = lstOvertimeByProfile[0].Status.ToString().TranslateString();
                        item.udAlreadyOvertimeID = lstOvertimeByProfile[0].ID;
                        item.IsNonOvertime = lstOvertimeByProfile[0].IsNonOvertime;
                        if (lstOvertimeByProfile[0].Status == OverTimeStatus.E_APPROVED.ToString())
                        {
                            item.RegisterHours = lstOvertimeByProfile[0].ApproveHours ?? 0;
                        }
                        else
                        {
                            item.RegisterHours = lstOvertimeByProfile[0].RegisterHours;
                        }
                    }
                }
                return;
            }
        }

        #region code moi
        /// <summary>
        /// hàm tạo ra OT với sự phân tích ra night shift và cắt giờ theo nightShift
        /// </summary>
        /// <param name="lstDayOff"></param>
        /// <param name="lstOvertimeType"></param>
        /// <param name="lstOvertimeInsert"></param>
        /// <param name="workday"></param>
        /// <param name="shiftByWorkday"></param>
        /// <param name="beginOT"></param>
        /// <param name="endOT"></param>
        private void CreateOvertimeAnalyzeNightShift(List<Cat_DayOffEntity> lstDayOff, List<Cat_OvertimeTypeEntity> lstOvertimeType, List<Att_OvertimeEntity> lstOvertimeInsert,
            WorkdayCustom Workday, Cat_ShiftEntity ShiftByWorkday, DateTime? BeginOT, DateTime? EndOT, bool isAllowGetNightShift, double MininumOvertimeHour
            , bool isAllowGetTypeBaseOnActualDate, bool isAllowGetTypeBaseOnBeginShift, bool isAllowGetTypeBaseOnEndShift, bool isNonShift, bool isBeforeShift, string userLogin)
        {
            DateTime? BeforeNightShiftStart = null;
            DateTime? BeforeNightShiftEnd = null;
            DateTime? NightShiftStart = null;
            DateTime? NightShiftEnd = null;
            DateTime? AfterNightShiftStart = null;
            DateTime? AfterNightShiftEnd = null;

            if ((EndOT.Value - BeginOT.Value).TotalHours < MininumOvertimeHour)
            {
                return;
            }

            DateTime DateChecking = Workday.WorkDate;
            DateTime ShiftBegin = DateChecking.Date.Add(ShiftByWorkday.InTime.TimeOfDay);
            DateTime ShiftEnd = ShiftBegin.AddHours(ShiftByWorkday.CoOut);


            DateTime ShiftStartBreak = DateChecking.Date.Add(ShiftByWorkday.InTime.TimeOfDay).AddHours(ShiftByWorkday.CoBreakIn);
            DateTime ShiftEndBreak = DateChecking.Date.Add(ShiftByWorkday.InTime.TimeOfDay).AddHours(ShiftByWorkday.CoBreakOut);


            if (isAllowGetNightShift && IsOverTimeNightShift(BeginOT.Value, EndOT.Value, ShiftByWorkday, Workday.WorkDate, out BeforeNightShiftStart, out BeforeNightShiftEnd, out NightShiftStart, out NightShiftEnd, out AfterNightShiftStart, out AfterNightShiftEnd, isNonShift))
            {
                //Có Đi Qua OT Đêm
                //Cặp trươc OT Đêm
                if (BeforeNightShiftStart != null && BeforeNightShiftEnd != null)
                {
                    double HourOT = 0;
                    //Logic Mantis:0029025
                    if (isNonShift)
                    {
                        HourOT = HourRoundDownNonShift(BeforeNightShiftStart, BeforeNightShiftEnd, ShiftBegin, ShiftEnd, ShiftStartBreak, ShiftEndBreak, userLogin);
                    }
                    else
                    {
                        if (BeforeNightShiftStart.Value < ShiftEndBreak && ShiftStartBreak < BeforeNightShiftEnd.Value) //co giao nhau
                        {
                            HourOT += (ShiftStartBreak - BeforeNightShiftStart.Value).TotalHours < 0 ? 0 : (ShiftStartBreak - BeforeNightShiftStart.Value).TotalHours;
                            HourOT += (BeforeNightShiftEnd.Value - ShiftEndBreak).TotalHours < 0 ? 0 : (BeforeNightShiftEnd.Value - ShiftEndBreak).TotalHours;
                        }
                        else
                        {
                            HourOT = (BeforeNightShiftEnd.Value - BeforeNightShiftStart.Value).TotalHours;
                        }
                    }
                    Guid? OvertimeTypeID = GetOvertimeType(DateChecking, lstDayOff, lstOvertimeType, false, isNonShift);
                    if (OvertimeTypeID != null)
                    {
                        Att_OvertimeEntity OTInsert = new Att_OvertimeEntity();
                        OTInsert.ID = Guid.NewGuid();
                        // OTInsert.WorkDate = BeginOT.Value;
                        OTInsert.WorkDate = BeforeNightShiftStart.Value;
                        OTInsert.WorkDateRoot = DateChecking;
                        OTInsert.RegisterHours = HourOT;
                        OTInsert.AnalyseHour = HourOT;

                        OTInsert.Status = OverTimeStatus.E_SUBMIT.ToString();
                        OTInsert.ProfileID = Workday.ProfileID;
                        OTInsert.OvertimeTypeID = OvertimeTypeID.Value;
                        //OTInsert.InTime = BeforeNightShiftStart.Value;
                        //OTInsert.OutTime = BeforeNightShiftEnd.Value;
                        OTInsert.InTime = Workday.InTime1;
                        OTInsert.OutTime = Workday.OutTime1;
                        OTInsert.ShiftID = Workday.ShiftApprove ?? Workday.ShiftID;
                        if (isNonShift)
                        {
                            OTInsert.udWorkdayNonShift = true;
                        }
                        if (isBeforeShift)
                        {
                            OTInsert.udTypeBeginOTWithShift = OverTimeBeginType.E_BEFORE_SHIFT.ToString();
                        }
                        else
                        {
                            OTInsert.udTypeBeginOTWithShift = OverTimeBeginType.E_AFTER_SHIFT.ToString();
                        }
                        lstOvertimeInsert.Add(OTInsert);
                    }
                }


                //Cặp sau OT Đêm
                if (AfterNightShiftStart != null && AfterNightShiftEnd != null)
                {
                    if (isAllowGetTypeBaseOnActualDate)
                    {
                        DateChecking = AfterNightShiftStart.Value.Date;
                    }
                    else if (isAllowGetTypeBaseOnEndShift)
                    {
                        DateChecking = AfterNightShiftEnd.Value.Date;

                    }

                    double HourOT = 0;
                    //Logic Mantis:0029025
                    if (isNonShift)
                    {
                        HourOT = HourRoundDownNonShift(AfterNightShiftStart, AfterNightShiftEnd, ShiftBegin, ShiftEnd, ShiftStartBreak, ShiftEndBreak, userLogin);
                    }
                    else
                    {
                        if (AfterNightShiftStart.Value < ShiftEndBreak && ShiftStartBreak < AfterNightShiftEnd.Value) //co giao nhau
                        {
                            HourOT += (ShiftStartBreak - AfterNightShiftStart.Value).TotalHours < 0 ? 0 : (ShiftStartBreak - AfterNightShiftStart.Value).TotalHours;
                            HourOT += (AfterNightShiftEnd.Value - ShiftEndBreak).TotalHours < 0 ? 0 : (AfterNightShiftEnd.Value - ShiftEndBreak).TotalHours;
                        }
                        else
                        {
                            HourOT = (AfterNightShiftEnd.Value - AfterNightShiftStart.Value).TotalHours;
                        }
                    }
                    Guid? OvertimeTypeID = GetOvertimeType(DateChecking, lstDayOff, lstOvertimeType, false, isNonShift);
                    if (OvertimeTypeID != null)
                    {
                        Att_OvertimeEntity OTInsert = new Att_OvertimeEntity();
                        OTInsert.ID = Guid.NewGuid();
                        //  OTInsert.WorkDate = BeginOT.Value;
                        OTInsert.WorkDate = AfterNightShiftStart.Value;
                        OTInsert.WorkDateRoot = DateChecking;
                        OTInsert.RegisterHours = HourOT;
                        OTInsert.AnalyseHour = HourOT;
                        OTInsert.Status = OverTimeStatus.E_SUBMIT.ToString();
                        OTInsert.ProfileID = Workday.ProfileID;
                        OTInsert.OvertimeTypeID = OvertimeTypeID.Value;
                        //OTInsert.InTime = AfterNightShiftStart.Value;
                        //OTInsert.OutTime = AfterNightShiftEnd.Value;
                        OTInsert.InTime = Workday.InTime1;
                        OTInsert.OutTime = Workday.OutTime1;
                        OTInsert.ShiftID = Workday.ShiftApprove ?? Workday.ShiftID;
                        if (isNonShift)
                        {
                            OTInsert.udWorkdayNonShift = true;
                        }
                        if (isBeforeShift)
                        {
                            OTInsert.udTypeBeginOTWithShift = OverTimeBeginType.E_BEFORE_SHIFT.ToString();
                        }
                        else
                        {
                            OTInsert.udTypeBeginOTWithShift = OverTimeBeginType.E_AFTER_SHIFT.ToString();
                        }
                        lstOvertimeInsert.Add(OTInsert);
                    }
                }

                //Cặp OT Đêm
                if (NightShiftStart != null && NightShiftEnd != null)
                {


                    if (isAllowGetTypeBaseOnActualDate)
                    {
                        DateChecking = NightShiftStart.Value.Date;
                    }
                    else if (isAllowGetTypeBaseOnEndShift)
                    {
                        DateChecking = NightShiftEnd.Value.Date;
                    }

                    double HourOT = 0;
                    //Logic Mantis:0029025
                    if (isNonShift)
                    {
                        HourOT = HourRoundDownNonShift(NightShiftStart, NightShiftEnd, ShiftBegin, ShiftEnd, ShiftStartBreak, ShiftEndBreak, userLogin);
                    }
                    else
                    {
                        if (NightShiftStart.Value < ShiftEndBreak && ShiftStartBreak < NightShiftEnd.Value) //co giao nhau
                        {
                            HourOT += (ShiftStartBreak - NightShiftStart.Value).TotalHours < 0 ? 0 : (ShiftStartBreak - NightShiftStart.Value).TotalHours;
                            HourOT += (NightShiftEnd.Value - ShiftEndBreak).TotalHours < 0 ? 0 : (NightShiftEnd.Value - ShiftEndBreak).TotalHours;
                        }
                        else
                        {
                            HourOT = (NightShiftEnd.Value - NightShiftStart.Value).TotalHours;
                        }
                    }
                    //double HourOT = (NightShiftEnd.Value - NightShiftStart.Value).TotalHours;
                    Guid? OvertimeTypeID = GetOvertimeType(DateChecking, lstDayOff, lstOvertimeType, true, isNonShift);
                    if (OvertimeTypeID != null)
                    {
                        Att_OvertimeEntity OTInsert = new Att_OvertimeEntity();
                        OTInsert.ID = Guid.NewGuid();
                        // OTInsert.WorkDate = BeginOT.Value;
                        OTInsert.WorkDate = NightShiftStart.Value;
                        OTInsert.WorkDateRoot = DateChecking;
                        OTInsert.RegisterHours = HourOT;
                        OTInsert.AnalyseHour = HourOT;
                        OTInsert.Status = OverTimeStatus.E_SUBMIT.ToString();
                        OTInsert.ProfileID = Workday.ProfileID;
                        OTInsert.OvertimeTypeID = OvertimeTypeID.Value;
                        //OTInsert.InTime = NightShiftStart.Value;
                        //OTInsert.OutTime = NightShiftEnd.Value;
                        OTInsert.InTime = Workday.InTime1;
                        OTInsert.OutTime = Workday.OutTime1;
                        OTInsert.ShiftID = Workday.ShiftApprove ?? Workday.ShiftID;
                        if (isNonShift)
                        {
                            OTInsert.udWorkdayNonShift = true;
                        }
                        if (isBeforeShift)
                        {
                            OTInsert.udTypeBeginOTWithShift = OverTimeBeginType.E_BEFORE_SHIFT.ToString();
                        }
                        else
                        {
                            OTInsert.udTypeBeginOTWithShift = OverTimeBeginType.E_AFTER_SHIFT.ToString();
                        }
                        lstOvertimeInsert.Add(OTInsert);
                    }
                }



            }
            else
            {
                //Không Đi Qua OT Ca Đêm
                double HourOT = 0;
                //Logic Mantis:0029025
                if (isNonShift)
                {
                    HourOT = HourRoundDownNonShift(BeginOT, EndOT, ShiftBegin, ShiftEnd, ShiftStartBreak, ShiftEndBreak, userLogin);
                }
                else
                {
                    if (BeginOT.Value < ShiftEndBreak && ShiftStartBreak < EndOT.Value) //co giao nhau
                    {
                        HourOT += (ShiftStartBreak - BeginOT.Value).TotalHours < 0 ? 0 : (ShiftStartBreak - BeginOT.Value).TotalHours;
                        HourOT += (EndOT.Value - ShiftEndBreak).TotalHours < 0 ? 0 : (EndOT.Value - ShiftEndBreak).TotalHours;
                    }
                    else
                    {
                        HourOT = (EndOT.Value - BeginOT.Value).TotalHours;
                    }
                }
                Guid? OvertimeTypeID = GetOvertimeType(DateChecking, lstDayOff, lstOvertimeType, false, isNonShift);

                if (OvertimeTypeID != null)
                {
                    Att_OvertimeEntity OTInsert = new Att_OvertimeEntity();
                    OTInsert.ID = Guid.NewGuid();
                    OTInsert.WorkDate = BeginOT.Value;
                    OTInsert.WorkDateRoot = DateChecking;
                    OTInsert.RegisterHours = HourOT;
                    OTInsert.AnalyseHour = HourOT;
                    OTInsert.Status = OverTimeStatus.E_SUBMIT.ToString();
                    OTInsert.ProfileID = Workday.ProfileID;
                    OTInsert.OvertimeTypeID = OvertimeTypeID.Value;
                    //OTInsert.InTime = BeginOT.Value;
                    //OTInsert.OutTime = EndOT.Value;
                    OTInsert.InTime = Workday.InTime1;
                    OTInsert.OutTime = Workday.OutTime1;
                    OTInsert.ShiftID = Workday.ShiftApprove ?? Workday.ShiftID;
                    if (isNonShift)
                    {
                        OTInsert.udWorkdayNonShift = true;
                    }
                    if (isBeforeShift)
                    {
                        OTInsert.udTypeBeginOTWithShift = OverTimeBeginType.E_BEFORE_SHIFT.ToString();
                    }
                    else
                    {
                        OTInsert.udTypeBeginOTWithShift = OverTimeBeginType.E_AFTER_SHIFT.ToString();
                    }
                    lstOvertimeInsert.Add(OTInsert);
                }
            }
        }

        public Guid? GetOvertimeType(DateTime workday, List<Cat_DayOffEntity> lstDayOff, List<Cat_OvertimeTypeEntity> lstOvertimeType, bool isNightShift, bool isNonShift)
        {
            Guid? Result = null;
            string CodeOT = OverTimeType.E_WORKDAY.ToString();
            if (IsHoliday(workday, lstDayOff))
            {
                CodeOT = OverTimeType.E_HOLIDAY.ToString();
            }
            else if (isNonShift || IsWeekend(workday, lstDayOff))
            {
                CodeOT = OverTimeType.E_WEEKEND.ToString();
            }
            if (isNightShift)
            {
                CodeOT = CodeOT + "_NIGHTSHIFT";
            }
            var OvertimeType = lstOvertimeType.Where(m => m.Code == CodeOT).FirstOrDefault();

            if (OvertimeType != null)
            {
                Result = OvertimeType.ID;
            }
            return Result;
        }
        public bool IsHoliday(DateTime workday, List<Cat_DayOffEntity> lstDayOff)
        {
            string E_HOLIDAY_HLD = HolidayType.E_HOLIDAY_HLD.ToString();
            if (lstDayOff.Any(m => m.DateOff == workday && m.Type == E_HOLIDAY_HLD))
            {
                return true;
            }
            return false;
        }
        public bool IsWeekend(DateTime workday, List<Cat_DayOffEntity> lstDayOff)
        {
            string E_WEEKEND_HLD = HolidayType.E_WEEKEND_HLD.ToString();
            if (workday.DayOfWeek == DayOfWeek.Sunday || lstDayOff.Any(m => m.DateOff == workday && m.Type == E_WEEKEND_HLD))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private double HourRoundDownNonShift(DateTime? BeginOT, DateTime? EndOT, DateTime ShiftBegin, DateTime ShiftEnd, DateTime ShiftStartBreak, DateTime ShiftEndBreak, string userLogin)
        {
            DateTime DateS = DateTime.MinValue;
            DateTime DateE = DateTime.MinValue;
            double HourOT = 0;
            double BreakTime = 0;


            //Giờ trong ca làm viêc
            if (BeginOT.Value < ShiftBegin)
            {
                DateS = ShiftBegin;
            }
            else
            {
                DateS = BeginOT.Value;
            }
            if (EndOT.Value > ShiftEnd)
            {
                DateE = ShiftEnd;
            }
            else
            {
                DateE = EndOT.Value;
            }

            if (DateS < ShiftEndBreak && DateE > ShiftStartBreak) // nếu như cặp thời gian này lồng vào nhau
            {
                BreakTime = ((DateE > ShiftEndBreak ? ShiftEndBreak : DateE) - (DateS < ShiftStartBreak ? ShiftStartBreak : DateS)).TotalHours;
            }

            HourOT += (DateE - DateS).TotalHours - BreakTime;

            //Round Down
            //Trước ca làm việc
            if (BeginOT.Value < ShiftBegin)
            {
                HourOT += RoundHour((ShiftBegin - BeginOT.Value).TotalHours, userLogin);
            }
            //Sau ca làm việc
            if (EndOT.Value > ShiftEnd)
            {
                HourOT += RoundHour((EndOT.Value - ShiftEnd).TotalHours, userLogin);
            }
            HourOT = Math.Round(HourOT, 5);
            return HourOT;
        }

        private double RoundHour(double Hour,string userLogin)
        {
            string status = string.Empty;
            string key = AppConfig.HRM_ATT_ROUNDOT.ToString();
            string keyLine = AppConfig.HRM_ATT_ROUNDOT_LINEROUND.ToString();
            List<object> lstHOUR = new List<object>();
            lstHOUR.Add(key);
            lstHOUR.Add(null);
            lstHOUR.Add(null);
            var config = GetData<Sys_AllSettingEntity>(lstHOUR, ConstantSql.hrm_sys_sp_get_AllSetting, userLogin, ref status);
            var configHOUR_ROUND = config.Where(s => s.Name == key).FirstOrDefault();
            var configHOUR_ROUND_LINEROUND = config.Where(s => s.Name == keyLine).FirstOrDefault();

            if (config != null && config.Count > 0 && configHOUR_ROUND.Value1 != null && configHOUR_ROUND.Value1 == bool.TrueString && configHOUR_ROUND_LINEROUND.Value1 != null)
            {
                double LineRound = 0;
                double.TryParse(configHOUR_ROUND_LINEROUND.Value1.ToString(), out LineRound);
                double value = Common.RoundMinuteDown(Hour * 60, LineRound);
                return value / 60;
            }
            return Hour;
        }

        private static TimeSpan _hourStartNightShift;
        private TimeSpan hourStartNightShift
        {
            get
            {
                if (_hourStartNightShift == DateTime.MinValue.TimeOfDay)
                {
                    string status = string.Empty;
                    string key = AppConfig.HRM_ATT_OT_NIGHTSHIFTOFDAYNOTSHIFT_END.ToString();
                    Sys_AllSetting config = GetData<Sys_AllSetting>(key, ConstantSql.hrm_sys_sp_get_AllSettingByKey.ToString(), string.Empty, ref status).FirstOrDefault();
                    if (config != null && config.Value1 != null)
                    {
                        char[] ext = new char[] { ':' };
                        List<string> HourAndMinute = config.Value1.Split(ext, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                        double hour = 0;
                        double minute = 0;
                        if (double.TryParse(HourAndMinute[0], out hour) && double.TryParse(HourAndMinute[1], out minute))
                        {
                            _hourStartNightShift = DateTime.MinValue.AddHours(hour).AddMinutes(minute).TimeOfDay;
                        }
                    }
                    return _hourStartNightShift;
                }
                else
                {
                    return _hourStartNightShift;
                }
            }
        }

        private static TimeSpan _hourEndNightShift;
        private TimeSpan hourEndNightShift
        {

            get
            {
                if (_hourEndNightShift == DateTime.MinValue.TimeOfDay)
                {
                    string status = string.Empty;
                    string key = AppConfig.HRM_ATT_OT_NIGHTSHIFTOFDAYNOTSHIFT_END.ToString();
                    Sys_AllSetting config = GetData<Sys_AllSetting>(key, ConstantSql.hrm_sys_sp_get_AllSettingByKey.ToString(),string.Empty, ref status).FirstOrDefault();
                    if (config != null && config.Value2 != null)
                    {
                        char[] ext = new char[] { ':' };
                        List<string> HourAndMinute = config.Value2.Split(ext, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                        double hour = 0;
                        double minute = 0;
                        if (double.TryParse(HourAndMinute[0], out hour) && double.TryParse(HourAndMinute[1], out minute))
                        {
                            _hourEndNightShift = DateTime.MinValue.AddHours(hour).AddMinutes(minute).TimeOfDay;
                        }
                    }
                    return _hourEndNightShift;
                }
                else
                {
                    return _hourEndNightShift;
                }
            }
        }

        public bool IsOverTimeNightShift(DateTime DateStart, DateTime DateEnd, Cat_ShiftEntity Shift, DateTime Workday,
           out DateTime? BeforeNightShiftStart, out DateTime? BeforeNightShiftEnd,
           out DateTime? NightShiftStart, out DateTime? NightShiftEnd,
           out DateTime? AfterNightShiftStart, out DateTime? AfterNightShiftEnd, bool isNonShift)
        {
            BeforeNightShiftStart = null;
            BeforeNightShiftEnd = null;
            NightShiftStart = null;
            NightShiftEnd = null;
            AfterNightShiftStart = null;
            AfterNightShiftEnd = null;

            bool IsOverTimeNightShift = false;
            //if (Shift == null || Shift.IsNightShift == null || Shift.IsNightShift == false || Shift.NightTimeStart == null || Shift.NightTimeEnd == null)
            if (Shift == null || Shift.NightTimeStart == null || Shift.NightTimeEnd == null)
            {
                return false;
            }
            DateTime NightS = Workday.Date.AddHours(Shift.NightTimeStart.Value.Hour).AddMinutes(Shift.NightTimeStart.Value.Minute);
            DateTime NightE = Workday.Date.AddHours(Shift.NightTimeEnd.Value.Hour).AddMinutes(Shift.NightTimeEnd.Value.Minute);
            if (isNonShift && Shift.CoStartOTNightWithOutShift != null && Shift.CoEndOTNightWithOutShift != null)
            {
                NightS = Workday.Date.Add(Shift.CoStartOTNightWithOutShift.Value.TimeOfDay);
                NightE = Workday.Date.Add(Shift.CoEndOTNightWithOutShift.Value.TimeOfDay);
            }
            if (NightE < NightS)
            {
                NightE = NightE.AddDays(1);
            }

            if (DateStart < NightE && DateEnd > NightS)
            {
                IsOverTimeNightShift = true;
                //Thời gian trước giờ ca đêm
                if (DateStart < NightS)
                {
                    BeforeNightShiftStart = DateStart;
                    BeforeNightShiftEnd = NightS;
                    //-----------------
                    NightShiftStart = NightS;
                }
                else
                {
                    NightShiftStart = DateStart;
                }

                //Thoi gian sau ca dem
                if (DateEnd > NightE)
                {
                    AfterNightShiftStart = NightE;
                    AfterNightShiftEnd = DateEnd;

                    NightShiftEnd = NightE;
                }
                else
                {
                    NightShiftEnd = DateEnd;
                }
            }

            if (IsOverTimeNightShift == false)
            { //Kiểm tra OT ca đêm vào đầu ca | Vd: OT lúc 5h ~ 7h sáng thì .. OT ca đêm là 5h~6h và Ot Ca ngày là 6h và 7h
                NightS = NightS.AddDays(-1);
                NightE = NightE.AddDays(-1);
                if (NightE < NightS)
                {
                    NightE = NightE.AddDays(1);
                }

                if (DateStart < NightE && DateEnd > NightS)
                {
                    IsOverTimeNightShift = true;
                    //Thời gian trước giờ ca đêm
                    if (DateStart < NightS)
                    {
                        BeforeNightShiftStart = DateStart;
                        BeforeNightShiftEnd = NightS;
                        //-----------------
                        NightShiftStart = NightS;
                    }
                    else
                    {
                        NightShiftStart = DateStart;
                    }

                    //Thoi gian sau ca dem
                    if (DateEnd > NightE)
                    {
                        AfterNightShiftStart = NightE;
                        AfterNightShiftEnd = DateEnd;

                        NightShiftEnd = NightE;
                    }
                    else
                    {
                        NightShiftEnd = DateEnd;
                    }
                }
            }


            return IsOverTimeNightShift;

        }


        #endregion

        #endregion

        #region Hàm Tách Tăng Ca
        public void ChangeMethodOverTime_Manual(Att_OvertimeEntity model)
        {

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Overtime = new CustomBaseRepository<Att_Overtime>(unitOfWork);
                var repoAtt_TimeofInLieu = new CustomBaseRepository<Att_TimeOffInLieu>(unitOfWork);



                double TimeOffRealValidate = 0;
                double TimeOffValidate = 0;
                double CashOutValidate = 0;
                if (model.TimeOffReal != null)
                {
                    TimeOffRealValidate = (double)model.TimeOffReal;
                    //double.TryParse(model.TimeOffReal.ToString(), out TimeOffRealValidate);
                }
                if (model.HourToTimeOff != null)
                {
                    TimeOffValidate = (double)model.HourToTimeOff;
                    //double.TryParse(txt_TimeOff.Text, out TimeOffValidate);
                }
                if (model.TimeRegister != null)
                {
                    CashOutValidate = (double)model.TimeRegister;
                    //double.TryParse(txt_CashOut.Text, out CashOutValidate);
                }
                if (TimeOffRealValidate < 0 || TimeOffValidate < 0 || CashOutValidate < 0)
                {
                    return;
                }

                double Cashout = CashOutValidate;
                double Timeoff = TimeOffValidate;

                //if (Ot.Status == OverTimeStatus.E_APPROVED.ToString())
                //{
                //    Common.MessageBoxs(Messages.Msg, Messages.DataCantBeModify.TranslateString(), Ext.Net.MessageBox.Icon.WARNING, string.Empty);
                //    return;
                //}

                Att_Overtime OT_New = new Att_Overtime();

                if (Cashout > 0 && Timeoff <= 0)
                {
                    //Chuyển hết cho trả tiền
                    model.MethodPayment = MethodOption.E_CASHOUT.ToString();

                }
                else if (Cashout <= 0 && Timeoff > 0)
                {
                    //Chuyển hết cho nghỉ bù
                    model.MethodPayment = MethodOption.E_TIMEOFF.ToString();


                }
                else if (Cashout > 0 && Timeoff > 0)
                {
                    //Vửa chuyển tiền vừa nghỉ bù
                    model.MethodPayment = MethodOption.E_CASHOUT.ToString();
                    model.RegisterHours = Cashout;

                    if (model.Status == OverTimeStatus.E_APPROVED.ToString())
                    {
                        model.ApproveHours = Cashout;
                    }
                    if (model.Status == OverTimeStatus.E_CONFIRM.ToString())
                    {
                        model.ApproveHours = Cashout;
                        model.ConfirmHours = Cashout;
                    }
                    OT_New = model.CopyData<Att_Overtime>();
                    OT_New.ID = Guid.NewGuid();
                    OT_New.WorkDate = model.WorkDate.AddHours(model.RegisterHours);
                    OT_New.MethodPayment = MethodOption.E_TIMEOFF.ToString();
                    OT_New.RegisterHours = Timeoff;
                    if (OT_New.Status == OverTimeStatus.E_APPROVED.ToString())
                    {
                        OT_New.ApproveHours = Timeoff;
                    }
                    if (OT_New.Status == OverTimeStatus.E_CONFIRM.ToString())
                    {
                        OT_New.ApproveHours = Timeoff;
                        OT_New.ConfirmHours = Timeoff;
                    }
                    if (model.IsNonOvertime == true)
                    {
                        //model.IsNonOvertime = true;
                        OT_New.IsNonOvertime = true;
                    }
                    //repoAtt_Overtime.FindBy<Att_Overtime>();
                    repoAtt_Overtime.Add(OT_New);
                    var att_overtimeEntity = model.CopyData<Att_Overtime>();
                    repoAtt_Overtime.Edit(att_overtimeEntity);

                    //EntityService.AddEntity<Att_Overtime>(GuidContext, (new List<Att_Overtime>() { OT_New }).ToArray());
                }

                if (model.LeaveDay1 != null || model.LeaveDay2 != null)
                {
                    DateTime? date1 = null;
                    DateTime? date2 = null;
                    if (model.LeaveDay1 != null)
                    {
                        date1 = model.LeaveDay1;
                    }
                    if (model.LeaveDay2 != null)
                    {
                        date2 = model.LeaveDay2;
                    }
                    //string Error = AddTimeOffInLieu(new List<Att_Overtime>() { Ot, OT_New }, date1, date2);
                    //if (Error != string.Empty)
                    //{
                    //    return;
                    //}
                }

                if (model.Status == OverTimeStatus.E_APPROVED.ToString() || model.Status == OverTimeStatus.E_CONFIRM.ToString())
                {
                    List<Att_TimeOffInLieu> lstTimeOffInsert = new List<Att_TimeOffInLieu>();
                    //TimeOffInLieeu
                    //1 Xóa Đi những cái OT liên Quan trong TimeOff
                    //2 Insert lại nhưng cái liên quan đên TimeOff
                    List<Att_TimeOffInLieu> lstTimeOff = repoAtt_TimeofInLieu.FindBy(m => m.OvertimeID == model.ID).ToList<Att_TimeOffInLieu>();
                    foreach (var item in lstTimeOff)
                    {
                        item.IsDelete = null;
                    }

                    if (model.MethodPayment == MethodOption.E_TIMEOFF.ToString())
                    {
                        Att_TimeOffInLieu TimeOffInsert = AddTimeOffInLieu(model.CopyData<Att_Overtime>());
                        TimeOffInsert.ID = Guid.NewGuid();
                        TimeOffInsert.ProfileID = model.ProfileID;
                        lstTimeOffInsert.Add(TimeOffInsert);
                    }
                    if (OT_New.MethodPayment == MethodOption.E_TIMEOFF.ToString())
                    {
                        Att_TimeOffInLieu TimeOffInsert = AddTimeOffInLieu(OT_New);
                        TimeOffInsert.ID = Guid.NewGuid();
                        TimeOffInsert.ProfileID = model.ProfileID;
                        lstTimeOffInsert.Add(TimeOffInsert);
                    }
                    if (lstTimeOffInsert.Count > 0)
                    {
                        for (int i = 0; i < lstTimeOffInsert.Count; i++)
                        {
                            lstTimeOffInsert[i].Hre_Profile = null;
                            repoAtt_TimeofInLieu.Add(lstTimeOffInsert[i]);
                        }
                        // repoAtt_TimeofInLieu.Add(lstTimeOffInsert);
                        //EntityService.AddEntity<Att_TimeOffInLieu>(GuidContext, lstTimeOffInsert.ToArray());
                    }
                }
                unitOfWork.SaveChanges();
                // EntityService.SubmitChanges(GuidContext, LoginUserID.Value);
                //IsSave = false;

                //IsSave = false;
            }
            //return Json(new { });

        }

        public Att_TimeOffInLieu AddTimeOffInLieu(Att_Overtime overtime)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                //Insert vào Att_TimeOffInLieu
                Att_TimeOffInLieu timeOffInLieu = null;
                if (overtime.ID != Guid.Empty)
                {
                    Hre_Profile profile = repoHre_Profile.FindBy(m => m.ID == overtime.ProfileID).FirstOrDefault();
                    timeOffInLieu = new Att_TimeOffInLieu();
                    Cat_OvertimeType overtimeType = overtime.Cat_OvertimeType;
                    double Hour = 0;
                    if (overtime.Status == OverTimeStatus.E_CONFIRM.ToString())
                    {
                        Hour = overtime.ConfirmHours == null ? 0 : overtime.ConfirmHours;
                    }
                    else if (overtime.Status == OverTimeStatus.E_APPROVED.ToString())
                    {
                        Hour = overtime.ApproveHours == null ? 0 : overtime.ApproveHours.Value;
                    }
                    double HourTimeOffInLieu = Hour;
                    double UnusualLeaves = 0;
                    if (overtimeType != null)
                    {
                        UnusualLeaves = HourTimeOffInLieu * (overtimeType.TimeOffInLieuRate == null ? 0 : overtimeType.TimeOffInLieuRate.Value);
                    }
                    double accumulateLeaves = Att_LeavedayServices.getAccumulateLeaves(profile.ID);

                    timeOffInLieu.AccumulateLeaves = accumulateLeaves;
                    timeOffInLieu.UnusualLeaves = UnusualLeaves;
                    timeOffInLieu.TakenLeaves = 0;
                    timeOffInLieu.RemainLeaves = UnusualLeaves + accumulateLeaves;
                    timeOffInLieu.Date = overtime.WorkDate.Date;
                    if (overtime.DateApprove != null)
                        timeOffInLieu.DateApprove = overtime.DateApprove.Value;
                    timeOffInLieu.ConvertedCashHours = UnusualLeaves;
                    timeOffInLieu.Att_Overtime = overtime;
                    timeOffInLieu.Hre_Profile = profile;
                }
                return timeOffInLieu;
            }

        }
        private string SaveLeaveDay(List<Att_Overtime> lstAttOvertimeInput, DateTime? date1, DateTime? date2)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Workday = new CustomBaseRepository<Att_Workday>(unitOfWork);
                var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);
                var repoAtt_LeaveDay = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);
                var repoAtt_TimeOffInLieu = new CustomBaseRepository<Att_TimeOffInLieu>(unitOfWork);
                string E_TIMEOFF = MethodOption.E_TIMEOFF.ToString();
                List<Att_Overtime> lstAttOvertime = lstAttOvertimeInput.Where(m => m.MethodPayment == E_TIMEOFF && m.WorkDateRoot != null).ToList();
                //string validate 
                string Validate = validateSaveLeaveDay(lstAttOvertime, date1, date2);
                if (Validate != string.Empty)
                {
                    return Validate;
                }

                List<Guid> lstProfileIDs = lstAttOvertime.Select(m => m.ProfileID).Distinct().ToList();
                var LstWorkday1 = new List<Att_Workday>().Select(m => new { m.ProfileID, m.WorkDate, m.ShiftApprove, m.ShiftID });
                var LstWorkday2 = new List<Att_Workday>().Select(m => new { m.ProfileID, m.WorkDate, m.ShiftApprove, m.ShiftID });
                if (date1 != null)
                {
                    LstWorkday1 = repoAtt_Workday.FindBy(m => m.WorkDate == date1.Value && lstProfileIDs.Contains(m.ProfileID))
                        .Select(m => new { m.ProfileID, m.WorkDate, m.ShiftApprove, m.ShiftID });
                }
                if (date2 != null)
                {
                    LstWorkday2 = repoAtt_Workday.FindBy(m => m.WorkDate == date2.Value && lstProfileIDs.Contains(m.ProfileID))
                        .Select(m => new { m.ProfileID, m.WorkDate, m.ShiftApprove, m.ShiftID });
                }

                List<Att_LeaveDay> lstLeaveDayInsert = new List<Att_LeaveDay>();
                Guid OvertimeTypeTimeOffID = repoCat_LeaveDayType.FindBy(m => m.IsTimeOffInLieu == true).Select(m => m.ID).FirstOrDefault();
                if (OvertimeTypeTimeOffID == null || OvertimeTypeTimeOffID == Guid.Empty)
                    return string.Empty;
                var lstShift = repoCat_Shift.GetAll().Select(m => new { m.ID, m.WorkHours });
                foreach (var item in lstAttOvertime)
                {
                    if (date1 != null)
                    {
                        Att_LeaveDay leaveDay = new Att_LeaveDay();
                        leaveDay.ID = Guid.Empty;
                        leaveDay.ProfileID = item.ProfileID;
                        leaveDay.LeaveDayTypeID = OvertimeTypeTimeOffID;
                        leaveDay.DateStart = date1.Value;
                        leaveDay.DateEnd = date1.Value;
                        leaveDay.TotalDuration = 1;
                        leaveDay.LeaveDays = 1;
                        leaveDay.Att_Overtime = item;
                        if (item.Status == OverTimeStatus.E_WAIT_APPROVED.ToString())
                            leaveDay.Status = LeaveDayStatus.E_WAIT_APPROVED.ToString();
                        else if (item.Status == OverTimeStatus.E_APPROVED.ToString())
                        {
                            leaveDay.Status = LeaveDayStatus.E_APPROVED.ToString();
                        }
                        else if (item.Status == OverTimeStatus.E_CONFIRM.ToString())
                        {
                            leaveDay.Status = LeaveDayStatus.E_APPROVED.ToString();
                        }
                        else
                        {
                            leaveDay.Status = LeaveDayStatus.E_SUBMIT.ToString();
                        }

                        var workday = LstWorkday1.Where(m => m.ProfileID == item.ProfileID && m.WorkDate == date1.Value).FirstOrDefault();
                        if (workday != null)
                        {
                            Guid? ShiftID = workday.ShiftApprove ?? workday.ShiftID;
                            if (ShiftID != null)
                            {
                                var shift = lstShift.Where(m => m.ID == ShiftID).FirstOrDefault();
                                if (shift != null)
                                {
                                    leaveDay.LeaveHours = shift.WorkHours ?? 0;
                                }
                            }

                        }
                        if (leaveDay.LeaveHours == null || leaveDay.LeaveHours == 0)
                        {
                            leaveDay.LeaveHours = 8;
                        }
                        lstLeaveDayInsert.Add(leaveDay);
                    }
                    if (date2 != null)
                    {
                        Att_LeaveDay leaveDay = new Att_LeaveDay();
                        leaveDay.ID = Guid.Empty;
                        leaveDay.ProfileID = item.ProfileID;
                        leaveDay.LeaveDayTypeID = OvertimeTypeTimeOffID;
                        leaveDay.DateStart = date2.Value;
                        leaveDay.DateEnd = date2.Value;
                        leaveDay.TotalDuration = 1;
                        leaveDay.LeaveDays = 1;
                        leaveDay.Att_Overtime = item;
                        if (item.Status == OverTimeStatus.E_WAIT_APPROVED.ToString())
                            leaveDay.Status = LeaveDayStatus.E_WAIT_APPROVED.ToString();
                        else if (item.Status == OverTimeStatus.E_APPROVED.ToString())
                        {
                            leaveDay.Status = LeaveDayStatus.E_APPROVED.ToString();
                        }
                        else if (item.Status == OverTimeStatus.E_CONFIRM.ToString())
                        {
                            leaveDay.Status = LeaveDayStatus.E_APPROVED.ToString();
                        }
                        else
                        {
                            leaveDay.Status = LeaveDayStatus.E_SUBMIT.ToString();
                        }

                        var workday = LstWorkday2.Where(m => m.ProfileID == item.ProfileID && m.WorkDate == date2.Value).FirstOrDefault();
                        if (workday != null)
                        {
                            Guid? ShiftID = workday.ShiftApprove ?? workday.ShiftID;
                            if (ShiftID != null)
                            {
                                var shift = lstShift.Where(m => m.ID == ShiftID).FirstOrDefault();
                                if (shift != null)
                                {
                                    leaveDay.LeaveHours = shift.WorkHours ?? 0;
                                }
                            }

                        }
                        if (leaveDay.LeaveHours == null || leaveDay.LeaveHours == 0)
                        {
                            leaveDay.LeaveHours = 8;
                        }
                        lstLeaveDayInsert.Add(leaveDay);
                    }
                }

                if (lstLeaveDayInsert.Count > 0)
                {
                    repoAtt_LeaveDay.Add(lstLeaveDayInsert);
                    //EntityService.AddEntity<Att_LeaveDay>(GuidContext, lstLeaveDayInsert.ToArray());
                }

                List<Att_TimeOffInLieu> lstTimeoffInlieu = new List<Att_TimeOffInLieu>();
                foreach (var item in lstLeaveDayInsert)
                {
                    if (item.Status == LeaveDayStatus.E_APPROVED.ToString())
                    {
                        Att_TimeOffInLieu timeOffLieu = AddTimeOffInLieu(item);
                        if (timeOffLieu != null)
                            lstTimeoffInlieu.Add(timeOffLieu);
                    }
                }
                if (lstTimeoffInlieu.Count > 0)
                {
                    repoAtt_TimeOffInLieu.Add(lstTimeoffInlieu);
                    //EntityService.AddEntity<Att_TimeOffInLieu>(GuidContext, lstTimeoffInlieu.ToArray());
                }
                return string.Empty;
            }
        }
        public static Att_TimeOffInLieu AddTimeOffInLieu(Att_LeaveDay leaveDay)
        {
            Att_TimeOffInLieu timeOffInLieu = null;
            if (leaveDay.ID != Guid.Empty)
            {
                Hre_Profile profile = leaveDay.Hre_Profile;
                timeOffInLieu = new Att_TimeOffInLieu();

                //double HourOnWorkDate = 8;
                //Sal_Grade grade = GradeDAO.GetGrade(profile, leaveDay.DateStart, context, userid);
                //if (grade != null)
                //{
                //    Cat_GradeCfg gradecfg = grade.Cat_GradeCfg;
                //    HourOnWorkDate = gradecfg.HourOnWorkDate == null ? 8 : gradecfg.HourOnWorkDate.Value;
                //}

                double ApproveHour;

                if (leaveDay.DurationType == LeaveDayDurationType.E_FULLSHIFT.ToString())
                {
                    ApproveHour = leaveDay.LeaveDays.Value * leaveDay.LeaveHours.Value;
                }
                else
                {
                    ApproveHour = leaveDay.LeaveHours.Value;
                }
                double HourTimeOffInLieu = ApproveHour;
                double accumulateLeaves = Att_LeavedayServices.getAccumulateLeaves(profile.ID);
                double RemainLeaves = accumulateLeaves - HourTimeOffInLieu;

                timeOffInLieu.AccumulateLeaves = accumulateLeaves;
                timeOffInLieu.UnusualLeaves = 0;
                timeOffInLieu.TakenLeaves = HourTimeOffInLieu;
                timeOffInLieu.RemainLeaves = RemainLeaves;
                timeOffInLieu.Date = leaveDay.DateStart;
                if (leaveDay.DateApprove != null)
                    timeOffInLieu.DateApprove = leaveDay.DateApprove.Value;
                // so gio quy ra tien = 0
                timeOffInLieu.ConvertedCashHours = 0;
                timeOffInLieu.Att_LeaveDay = leaveDay;
                timeOffInLieu.Hre_Profile = profile;
                //timeOffInLieu.DateCreate = DateTime.Now;
            }
            return timeOffInLieu;
        }
        private string validateSaveLeaveDay(List<Att_Overtime> lstAttOvertimeInput, DateTime? date1, DateTime? date2)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var repoAtt_LeaveDay = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);
                var repoCat_OvertimeType = new CustomBaseRepository<Cat_OvertimeType>(unitOfWork);

                if (date1 == null && date2 == null)
                    return string.Empty;

                List<Guid> lstProfileIDs = lstAttOvertimeInput.Select(m => m.ProfileID).Distinct().ToList();
                var lstProfile = repoHre_Profile.FindBy(m => lstProfileIDs.Contains(m.ID)).Select(m => new { m.ID, m.CodeEmp, m.ProfileName });
                var lstOvertimeType = repoCat_OvertimeType.GetAll().Select(m => new { m.ID, m.TimeOffInLieuRate });
                string Error = string.Empty;

                foreach (var item in lstAttOvertimeInput)
                {
                    double RateTimeOff = lstOvertimeType.Where(m => m.ID == item.OvertimeTypeID).Select(m => m.TimeOffInLieuRate ?? 0).FirstOrDefault();
                    if (((date1 != null && date2 == null) || (date1 == null && date2 != null)))
                    {
                        double hourByDay = HourInShiftByDate(item.ProfileID, date1.Value);

                        if ((item.RegisterHours * RateTimeOff) < hourByDay)
                        {
                            var profile = lstProfile.Where(m => m.ID == item.ProfileID).FirstOrDefault();
                            if (profile != null)
                            {
                                Error += profile.ProfileName + "[" + profile.CodeEmp + "]; ";
                            }
                        }
                    }
                    else if (date1 != null && date2 != null)
                    {

                        double hourByDay1 = HourInShiftByDate(item.ProfileID, date1.Value);
                        double hourByDay2 = HourInShiftByDate(item.ProfileID, date2.Value);
                        if ((item.RegisterHours * RateTimeOff) < (hourByDay1 + hourByDay2))
                        {
                            var profile = lstProfile.Where(m => m.ID == item.ProfileID).FirstOrDefault();
                            if (profile != null)
                            {
                                Error += profile.ProfileName + "[" + profile.CodeEmp + "]; ";
                            }
                        }
                    }
                }
                if (Error != string.Empty)
                {
                    Error = Error.Substring(0, Error.Length - 2);
                    //Error = string.Format(Messages.EmpNotEnoughTimeToTimeOff.TranslateString(), Error);
                    return Error;
                }

                #region validateDuplicate
                string E_REJECTED = LeaveDayStatus.E_REJECTED.ToString();
                var lstLeaveDay1Query = repoAtt_LeaveDay.FindBy(m => m.Status != E_REJECTED && lstProfileIDs.Contains(m.ProfileID));
                var lstLeaveDay2Query = repoAtt_LeaveDay.FindBy(m => m.Status != E_REJECTED && lstProfileIDs.Contains(m.ProfileID));
                bool Isleaveday1 = false;
                bool Isleaveday2 = false;
                if (date1 != null)
                {
                    Isleaveday1 = true;
                    lstLeaveDay1Query = lstLeaveDay1Query.Where(m => m.DateStart <= date1.Value && m.DateEnd >= date1.Value);
                }
                if (date2 != null)
                {
                    Isleaveday2 = true;
                    lstLeaveDay2Query = lstLeaveDay2Query.Where(m => m.DateStart <= date2.Value && m.DateEnd >= date2.Value);
                }


                var lstLeaveday1 = new List<Att_LeaveDay>().Select(m => new { m.ProfileID, m.DateStart, m.DateEnd });
                var lstLeaveday2 = new List<Att_LeaveDay>().Select(m => new { m.ProfileID, m.DateStart, m.DateEnd });
                if (Isleaveday1)
                {
                    lstLeaveday1 = lstLeaveDay1Query.Select(m => new { m.ProfileID, m.DateStart, m.DateEnd });
                }
                if (Isleaveday2)
                {
                    lstLeaveday2 = lstLeaveDay2Query.Select(m => new { m.ProfileID, m.DateStart, m.DateEnd });
                }
                foreach (var item in lstAttOvertimeInput)
                {

                    if (date1 != null && lstLeaveday1.Any(m => m.ProfileID == item.ProfileID))
                    {
                        var profile = lstProfile.Where(m => m.ID == item.ProfileID).FirstOrDefault();
                        if (profile != null)
                        {
                            Error += profile.ProfileName + "[" + profile.CodeEmp + "]; ";
                        }
                    }
                    if (date2 != null && lstLeaveday2.Any(m => m.ProfileID == item.ProfileID))
                    {
                        var profile = lstProfile.Where(m => m.ID == item.ProfileID).FirstOrDefault();
                        if (profile != null)
                        {
                            Error += profile.ProfileName + "[" + profile.CodeEmp + "]; ";
                        }
                    }

                }
                if (Error != string.Empty)
                {
                    Error = Error.Substring(0, Error.Length - 2);
                    //Error = string.Format(Messages.EmpLeaveDuplicate.TranslateString(), Error);
                    return Error;
                }

                #endregion

                return Error;
            }
        }

        //load nghi bu
        public Guid loadLeavDayTypeID()
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_LeaveDay = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                Cat_LeaveDayType result = repoAtt_LeaveDay.FindBy(m => m.IsTimeOffInLieu == true).FirstOrDefault();
                if (result != null)
                    return result.ID;
                return Guid.Empty;
            }
        }
        public double HourInShiftByDate(Guid ProfileID, DateTime date1)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Workday = new CustomBaseRepository<Att_Workday>(unitOfWork);
                var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);
                var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);
                var repoAtt_Roster = new CustomBaseRepository<Att_Roster>(unitOfWork);
                var repoHre_WorkHistory = new CustomBaseRepository<Hre_WorkHistory>(unitOfWork);
                var repoCat_GradeAttendance = new CustomBaseRepository<Cat_GradeAttendance>(unitOfWork);

                double HourByDay = 0;
                Att_Workday wordayByProfileByDate = repoAtt_Workday.FindBy(m => m.ProfileID == ProfileID && m.WorkDate == date1).FirstOrDefault();
                if (wordayByProfileByDate != null)
                {
                    Guid shiftID = wordayByProfileByDate.ShiftApprove ?? (wordayByProfileByDate.ShiftID ?? Guid.Empty);
                    var Shift = repoCat_Shift.FindBy(m => m.ID == shiftID).FirstOrDefault();
                    if (Shift != null)
                    {
                        HourByDay = Shift.WorkHours ?? 8;
                    }
                }
                else
                {
                    var listGrade = repoAtt_Grade.FindBy(d => d.IsDelete == null && d.MonthStart != null && d.MonthStart <= date1
                                 && d.ProfileID == ProfileID).Select(d => new { d.ProfileID, d.MonthStart, d.GradeAttendanceID }).ToList();
                    var grade = listGrade.OrderByDescending(d => d.MonthStart).FirstOrDefault();
                    List<Att_Roster> lstRosterTypeGroup = new List<Att_Roster>();
                    List<Att_RosterGroup> lstRosterGroup = new List<Att_RosterGroup>();
                    Att_RosterServices.GetRosterGroup(new List<Guid>() { ProfileID }, date1, date1, out lstRosterTypeGroup, out lstRosterGroup);
                    string rosterStatus = RosterStatus.E_APPROVED.ToString();
                    List<Att_Roster> listRosterByProfile = repoAtt_Roster.FindBy(d =>
                        d.Status == rosterStatus && d.ProfileID == ProfileID && d.DateStart <= date1 && d.DateEnd >= date1).ToList<Att_Roster>();
                    List<Hre_WorkHistory> listWorkHistoryByProfile = repoHre_WorkHistory.FindBy(d => d.ProfileID == ProfileID && d.DateEffective <= date1).ToList<Hre_WorkHistory>();


                    if (grade != null)
                    {
                        var gradeCfg = repoCat_GradeAttendance.FindBy(d => d.ID == grade.GradeAttendanceID).FirstOrDefault();
                        if (gradeCfg != null)
                        {
                            Dictionary<DateTime, Cat_Shift> listMonthShifts = Att_RosterServices.GetDailyShifts(ProfileID, gradeCfg, listRosterByProfile, listWorkHistoryByProfile, date1, date1, lstRosterGroup, lstRosterTypeGroup);
                            //-- && 
                            if (listMonthShifts != null && listMonthShifts.ContainsKey(date1))
                            {
                                Cat_Shift shift = listMonthShifts[date1];
                                if (shift != null)
                                {
                                    HourByDay = shift.WorkHours ?? 8;
                                }
                            }
                        }
                    }
                }
                return HourByDay;
            }

        }
        #endregion

        /// <summary>
        /// <Kiet.Chung> phân tích nghỉ bù
        /// </summary>
        /// <returns></returns>
        /// 
        public List<Att_OvertimeEntity> AddTimeOffInLieu(Att_OvertimeEntity objAttOT, DateTime? LeaveDay1, DateTime? LeaveDay2, out string strStatus)
        {
            strStatus = "";
            List<Att_OvertimeEntity> lstOvertime = new List<Att_OvertimeEntity>();
            lstOvertime.Add(objAttOT);
            List<Att_Overtime> lstAtt_OvertimeValidate = lstOvertime.Translate<Att_Overtime>();
            List<Att_OvertimeEntity> lstResult = new List<Att_OvertimeEntity>();
            strStatus = validateSaveLeaveDay(lstAtt_OvertimeValidate, LeaveDay1, LeaveDay2);
            if (strStatus != "")
            {
                strStatus = "Dữ liệu không đủ để tạo ngày nghỉ";
                return lstOvertime;
            }
            double Hour1 = 0, Hour2 = 0;
            if (LeaveDay1.HasValue)
            {
                Hour1 = HourInShiftByDate(objAttOT.ProfileID, LeaveDay1.Value);
                if (Hour1 == 0)
                {
                    strStatus = ConstantMessages.PlsCheckRosterOfEmpByDate.TranslateString();
                    return lstOvertime;
                }
            }
            if (LeaveDay2.HasValue)
            {
                Hour2 = HourInShiftByDate(objAttOT.ProfileID, LeaveDay2.Value);
                if (Hour2 == 0)
                {
                    strStatus = ConstantMessages.PlsCheckRosterOfEmpByDate.TranslateString();
                    return lstOvertime;
                }
            }
            DateTime date = objAttOT.WorkDateRoot ?? objAttOT.WorkDate.Date;
            DateTime EndMonthNext = new DateTime(date.Year, date.Month, 1).AddMonths(2).AddMinutes(-1);
            if (LeaveDay1.HasValue)
            {
                if (LeaveDay1 > EndMonthNext)
                {
                    strStatus = ConstantMessages.DoNotRegisterLeaveOverLastNextMonthOvertimeDay.TranslateString();
                    return lstOvertime;
                }
            }
            if (LeaveDay2.HasValue)
            {
                if (LeaveDay2 > EndMonthNext)
                {
                    strStatus = ConstantMessages.DoNotRegisterLeaveOverLastNextMonthOvertimeDay.TranslateString();
                    return lstOvertime;
                }
            }

            double TotalHour = 0;
            if (objAttOT.Status == OverTimeStatus.E_APPROVED.ToString())
            {
                TotalHour += (objAttOT.ApproveHours ?? 0) * (objAttOT.TimeOffInLieuRate ?? 0);
            }
            else if (objAttOT.Status == OverTimeStatus.E_CONFIRM.ToString())
            {
                TotalHour += objAttOT.ConfirmHours * (objAttOT.TimeOffInLieuRate ?? 0);
            }
            else
            {
                TotalHour += objAttOT.RegisterHours * (objAttOT.TimeOffInLieuRate ?? 0);
            }

            if (TotalHour < (Hour1 + Hour2))
            {
                //lstResult.Add(objAttOT);
                strStatus = ConstantMessages.DataNotEnoughToMakeLeave.TranslateString();
                return lstOvertime;
            }

            objAttOT.MethodPayment = MethodOption.E_TIMEOFF.ToString();
            double Dental = TotalHour - (Hour1 + Hour2);
            double HourByDental = 0;
            if (objAttOT.TimeOffInLieuRate != null && objAttOT.TimeOffInLieuRate != 0)
            {
                HourByDental = Dental / objAttOT.TimeOffInLieuRate ?? 0;
                objAttOT.RegisterHours = objAttOT.RegisterHours - HourByDental;
                if (objAttOT.ApproveHours != null)
                {
                    objAttOT.ApproveHours = objAttOT.ApproveHours - HourByDental;
                }
                objAttOT.AnalyseHour = objAttOT.AnalyseHour - HourByDental;
                if (objAttOT.ConfirmHours > 0)
                {
                    objAttOT.ConfirmHours = objAttOT.ConfirmHours - HourByDental;
                }
            }
            Att_OvertimeEntity OvertimeNew = new Att_OvertimeEntity();
            objAttOT.CopyData(OvertimeNew, "ID");
            OvertimeNew.WorkDate = objAttOT.WorkDate.AddHours(objAttOT.RegisterHours);
            OvertimeNew.MethodPayment = MethodOption.E_CASHOUT.ToString();
            OvertimeNew.RegisterHours = HourByDental;
            OvertimeNew.AnalyseHour = HourByDental;
            if (OvertimeNew.Status == OverTimeStatus.E_APPROVED.ToString())
            {
                OvertimeNew.ApproveHours = HourByDental;
            }
            else if (OvertimeNew.Status == OverTimeStatus.E_CONFIRM.ToString())
            {
                OvertimeNew.ApproveHours = HourByDental;
                OvertimeNew.ConfirmHours = HourByDental;
            }
            lstResult.Add(objAttOT);
            lstResult.Add(OvertimeNew);

            return lstResult;
        }

        /// <summary>
        /// [Hien.NGuyen] - Xử lý form lưu của màn hình phân tích tăng ca
        /// </summary>
        /// <param name="updatedAtt_OvertimeModel"></param>
        /// <returns></returns>
        public bool CreateComputeOvertime(List<Att_OvertimeEntity> lstOvertimeInsert, string PaymentMethod, string strOvertimeStatus, Guid UserAppvoveID, double HourToTimeOff, double TimeRegister, string strReasonOT, Guid UserId, string userLogin)
        {
            string status = string.Empty;
            List<Att_Overtime> lstOvertimeInsertOutput = new List<Att_Overtime>();
            List<Guid> lstProID = lstOvertimeInsert.Select(s => s.ProfileID).Distinct().ToList();

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoSys_UserInfo = new CustomBaseRepository<Sys_UserInfo>(unitOfWork);
                //var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var repoAtt_AllowLimitOvertime = new CustomBaseRepository<Att_AllowLimitOvertime>(unitOfWork);
                var repoCat_EntityLabel = new CustomBaseRepository<Cat_EntityLabel>(unitOfWork);
                var repoSys_GroupLabel = new CustomBaseRepository<Sys_GroupLabel>(unitOfWork);
                var repoAtt_TimeOffInLieu = new CustomBaseRepository<Att_TimeOffInLieu>(unitOfWork);
                var repoAtt_OverTime = new CustomBaseRepository<Att_Overtime>(unitOfWork);

                var lstHre = GetData<Hre_ProfileEntity>(Common.DotNetToOracle(String.Join(",", lstProID)), ConstantSql.hrm_hr_sp_get_ProfileByIds, userLogin, ref status);

                Sys_UserInfo UserAppvove = repoSys_UserInfo.FindBy(pro => pro.ID == UserAppvoveID).FirstOrDefault();
                if (UserAppvove == null)
                {
                    //errorMessage = Messages.plsChooseApproveUser.TranslateString();
                    return false;
                }
                if (strReasonOT == string.Empty)
                    strReasonOT = " ";
                foreach (var item in lstOvertimeInsert)
                {
                    item.ReasonOT = strReasonOT;
                    if (item.MethodPayment == null)
                    {
                        item.MethodPayment = EnumDropDown.PaymentType.E_CASHOUT.ToString();
                    }
                }
                #region Validate Đã Đăng Ký Rồi
                List<Guid> profileHaveOT = new List<Guid>();
                profileHaveOT = lstOvertimeInsert.Where(m => m.udOvertimeStatus != null).Select(m => m.ProfileID).Distinct().ToList();
                if (profileHaveOT.Count > 0)
                {
                    var lstProfile = lstHre.Where(m => profileHaveOT.Contains(m.ID)).Select(m => new { m.ID, m.ProfileName, m.CodeEmp });
                    string ProfileNameErr = string.Empty;
                    foreach (var item in lstProfile)
                    {
                        ProfileNameErr += item.ProfileName + " [" + item.CodeEmp + "]; ";
                    }
                    if (ProfileNameErr.Length > 0)
                    {
                        ProfileNameErr = ProfileNameErr.Substring(0, ProfileNameErr.Length - 2);
                    }
                    //errorMessage = string.Format("Nhân viên {0} Tăng Ca Trùng ", ProfileNameErr);
                    return false;
                }
                #endregion
                #region Validate Không được phép Tăng Ca
                string Err = ValidateOT_DoNotAllow(lstOvertimeInsert, lstHre);
                if (Err != string.Empty)
                {
                    //errorMessage = Err;
                    return false;
                }
                #endregion

                #region phân tích lại và validate thông báo vượt trần mức 300
                List<Att_AllowLimitOvertime> lisProfileAllowLimitOvertime = repoAtt_AllowLimitOvertime.GetAll().ToList();
                double HourInLieu = 0;
                if (PaymentMethod == EnumDropDown.PaymentType.E_CASHOUT.ToString() || PaymentMethod == EnumDropDown.PaymentType.E_CASHOUT_TIMEOFF.ToString())
                {
                    HourInLieu = HourToTimeOff;
                }
                else if (PaymentMethod == MethodOption.E_TIMEOFF.ToString())
                {
                    HourInLieu = double.MaxValue;
                }
                if (HourInLieu > 0)
                {
                    lstOvertimeInsert = SplitOvertimeByInlieuHour(lstOvertimeInsert, HourInLieu);
                }
                OvertimePermitEntity overtimePermit = getOvertimePermit(userLogin);

                List<Att_OvertimeEntity> LimitOTByYear = ValidateLimitOTByYear(lstOvertimeInsert, lisProfileAllowLimitOvertime, overtimePermit);
                if (LimitOTByYear.Count > 0)
                {
                    List<Guid> lstProfileID = LimitOTByYear.Select(m => m.ProfileID).Distinct().ToList();
                    var lstProfile = lstHre.Where(m => lstProfileID.Contains(m.ID)).Select(m => new { m.ID, m.ProfileName, m.CodeEmp });
                    string lstCodeEmpErr = string.Empty;
                    foreach (var item in lstProfile)
                    {
                        lstCodeEmpErr += item.CodeEmp + "; ";
                    }
                    if (lstCodeEmpErr.Length > 0)
                    {
                        lstCodeEmpErr = lstCodeEmpErr.Substring(0, lstCodeEmpErr.Length - 2);
                    }
                    // errorMessage = string.Format("Nhân viên {0} đã vượt trần", lstCodeEmpErr);
                    return false;
                }

                #endregion


                List<Att_TimeOffInLieu> lstTimeOffInLieu = new List<Att_TimeOffInLieu>();

                string Att_Overtime = "Att_Overtime";
                string E_OVERTIME = "E_OVERTIME";
                Cat_EntityLabel EntityLabel = repoCat_EntityLabel.FindBy(m => m.UserID == UserId && m.EntityName == Att_Overtime && m.EntityLabelName == E_OVERTIME).FirstOrDefault();
                if (EntityLabel == null)
                {
                    EntityLabel = new Cat_EntityLabel();
                    EntityLabel.ID = Guid.NewGuid();
                    EntityLabel.UserID = UserId;
                    EntityLabel.EntityName = Att_Overtime;
                    EntityLabel.EntityLabelName = E_OVERTIME;
                    repoCat_EntityLabel.Add(EntityLabel);
                    repoCat_EntityLabel.SaveChanges();
                }
                Sys_GroupLabel GroupLable = repoSys_GroupLabel.FindBy(m => m.EntityLabelID == EntityLabel.ID).FirstOrDefault();
                if (GroupLable == null)
                {
                    GroupLable = new Sys_GroupLabel();
                    GroupLable.ID = Guid.NewGuid();
                    GroupLable.EntityLabelID = EntityLabel.ID;
                    GroupLable.EntityItems = string.Empty;
                    repoSys_GroupLabel.Add(GroupLable);
                    repoSys_GroupLabel.SaveChanges();
                }
                string EntityItems = GroupLable.EntityItems;

                List<Att_Overtime> save = new List<Att_Overtime>();
                List<Att_Overtime> saveAdd = new List<Att_Overtime>();
                List<Att_Overtime> saveEdit = new List<Att_Overtime>();
                Att_Overtime saveModel = null;
                foreach (Att_OvertimeEntity Ot in lstOvertimeInsert)
                {

                    bool InsertToTimeOffInlieu = false;
                    Ot.Status = strOvertimeStatus;//OverTimeStatus.E_APPROVED.ToString();
                    Ot.udOvertimeStatus = strOvertimeStatus.TranslateString();
                    if (strOvertimeStatus == OverTimeStatus.E_APPROVED.ToString())
                    {
                        Ot.ApproveHours = Ot.RegisterHours;
                        if (Ot.MethodPayment == EnumDropDown.PaymentType.E_TIMEOFF.ToString() || Ot.MethodPayment == EnumDropDown.PaymentType.E_CASHOUT_TIMEOFF.ToString())
                            InsertToTimeOffInlieu = true;
                    }
                    else if (strOvertimeStatus == OverTimeStatus.E_CONFIRM.ToString())
                    {
                        Ot.ConfirmHours = Ot.RegisterHours;
                        if (Ot.MethodPayment == EnumDropDown.PaymentType.E_TIMEOFF.ToString() || Ot.MethodPayment == EnumDropDown.PaymentType.E_CASHOUT_TIMEOFF.ToString())
                            InsertToTimeOffInlieu = true;
                    }
                    Ot.ReasonOT = strReasonOT;
                    Ot.UserApproveID = UserAppvove.ID;
                    Guid ProfileID = Ot.ProfileID;
                    DateTime beginOt = Ot.WorkDate;
                    DateTime endOt = beginOt.AddHours(Ot.RegisterHours);
                    Att_OvertimeEntity OvertimeRemove = lstOvertimeCache.Where(m => m.ID == Ot.ID).FirstOrDefault();
                    if (OvertimeRemove != null)
                    {
                        int index = lstOvertimeCache.IndexOf(OvertimeRemove);
                        lstOvertimeCache.Remove(OvertimeRemove);
                        lstOvertimeCache.Insert(index, Ot);
                    }

                    if (InsertToTimeOffInlieu)
                    {
                        Att_TimeOffInLieu timeOffLieu = AddTimeOffInLieu(Ot.CopyData<Att_Overtime>());
                        lstTimeOffInLieu.Add(timeOffLieu);
                        //repoAtt_TimeOffInLieu.Add(timeOffLieu);
                    }
                    EntityItems += Ot.ID.ToString().ToLower() + "|";
                    //Insert vào Đính Nhãn

                    saveModel = Ot.CopyData<Att_Overtime>();
                    save.Add(saveModel);
                }
                if (EntityItems.Length > 0)
                {
                    EntityItems = EntityItems.Substring(0, EntityItems.Length - 1);
                }
                GroupLable.EntityItems = EntityItems;

                saveEdit = save.Where(s => s.ID != Guid.Empty).ToList();
                saveAdd = save.Where(s => s.ID == Guid.Empty).ToList();
                foreach (var item in saveAdd)
                {
                    item.ID = Guid.NewGuid();
                }
                if (saveAdd != null && saveAdd.Count > 0)
                    repoAtt_OverTime.Add(saveAdd);
                if (saveEdit != null && saveEdit.Count > 0)
                    repoAtt_OverTime.Edit(saveEdit);
                if (lstTimeOffInLieu != null && lstTimeOffInLieu.Count > 0)
                    repoAtt_TimeOffInLieu.Add(lstTimeOffInLieu);
                //if (EntityLabel != null)
                //    repoCat_EntityLabel.Add(EntityLabel);
                //if (GroupLable != null)
                //    repoSys_GroupLabel.Add(GroupLable);
                try
                {
                    repoAtt_OverTime.SaveChanges();
                    repoAtt_TimeOffInLieu.SaveChanges();
                    //repoCat_EntityLabel.SaveChanges();
                    //repoSys_GroupLabel.SaveChanges();
                    //unitOfWork.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private string ValidateOT_DoNotAllow(List<Att_OvertimeEntity> lstOvertime, List<Hre_ProfileEntity> lstHre_Profile)
        {

            string result = string.Empty;
            List<Att_OvertimeEntity> lstOvertimeCanNot = lstOvertime.Where(m => m.IsNonOvertime == true).ToList();
            List<Guid> lstProfileID = lstOvertimeCanNot.Select(m => m.ProfileID).Distinct().ToList();
            var lstProfile = lstHre_Profile.Where(m => lstProfileID.Contains(m.ID)).Select(m => new { m.ID, m.ProfileName, m.CodeEmp });

            List<ProfileDateTime> lstProfileAlready = new List<ProfileDateTime>();


            foreach (var item in lstOvertimeCanNot)
            {
                if (lstProfileAlready.Any(m => m.ProfileID == item.ProfileID && m.WorkDate == item.WorkDateRoot))
                {
                    continue;
                }
                else
                {
                    var Profile = lstProfile.Where(m => m.ID == item.ProfileID).FirstOrDefault();
                    result += Profile.ProfileName + "[" + Profile.CodeEmp + "] - " + item.WorkDateRoot.Value.ToShortDateString() + "; ";

                    ProfileDateTime ProfileAlready = new ProfileDateTime();
                    ProfileAlready.ProfileID = item.ProfileID;
                    ProfileAlready.WorkDate = item.WorkDateRoot.Value;
                    lstProfileAlready.Add(ProfileAlready);
                }
            }

            if (result != string.Empty)
            {
                result = result.Substring(0, result.Length - 2);
                //result = string.Format(Messages.EmpDoNotRegisterOvertime.TranslateString(), result);
            }
            return result;
        }
        private List<Att_OvertimeEntity> SplitOvertimeByInlieuHour(List<Att_OvertimeEntity> lstOvertimeInput, double HourInLieu)
        {
            //1. for qua tung OT
            //2. cai nào co so gio nhieu hon HourInLieu thì tach ra
            //3. Cai nào ít hon thi chuyen hoan toan qua OT bu
            List<Att_OvertimeEntity> lstOvertime = new List<Att_OvertimeEntity>();
            foreach (var item in lstOvertimeInput)
            {
                if (item.RegisterHours > HourInLieu)
                {
                    double HourOver = item.RegisterHours - HourInLieu;
                    DateTime WorkdayNew = item.WorkDate.AddHours(HourOver);
                    //New 2 OT
                    Att_OvertimeEntity OT1 = new Att_OvertimeEntity();
                    Att_OvertimeEntity OT2 = new Att_OvertimeEntity();
                    item.CopyData(OT1, Att_OvertimeEntity.FieldNames.ID);
                    item.CopyData(OT2, Att_OvertimeEntity.FieldNames.ID);
                    OT1.RegisterHours = HourOver;
                    OT1.MethodPayment = MethodOption.E_CASHOUT.ToString();

                    OT2.WorkDate = WorkdayNew;
                    OT2.RegisterHours = HourInLieu;
                    OT2.MethodPayment = MethodOption.E_TIMEOFF.ToString();
                    lstOvertime.Add(OT1);
                    lstOvertime.Add(OT2);

                }
                else
                {
                    item.MethodPayment = MethodOption.E_TIMEOFF.ToString();
                    lstOvertime.Add(item);
                }
            }

            return lstOvertime;
        }
        /// <summary>
        /// Hàm Lọc ra những OT đã bị vi phạm
        /// </summary>
        /// <param name="lstOverTimeNotAllow">Danh Sách Overtime Đã bị vượt trần dựa vào type limit thuộc Year và ở mức 1 và mức 2</param>
        /// <param name="lstAllowLimitOvertime">Danh Sách Profile Đc phép vượt trần ở mức độ nào đã được lọc theo ds Profile Của lstOvertime</param>
        /// <returns>Cho phép vượt ở DS OT vượt phép năm level 1
        /// Có thể chặn những người 
        /// </returns>
        private List<Att_OvertimeEntity> ValidateLimitOTByYear(List<Att_OvertimeEntity> lstOverTime, List<Att_AllowLimitOvertime> lstAllowLimitOvertime, OvertimePermitEntity OtPermit)
        {
            FillAllowOTValidate(lstOverTime, OtPermit);

            string E_YEAR = OverTimeOverLimitType.E_YEAR.ToString();
            string E_YEAR_LV1 = OverTimeOverLimitType.E_YEAR_LV1.ToString();
            string E_YEAR_LV2 = OverTimeOverLimitType.E_YEAR_LV2.ToString();
            List<Att_OvertimeEntity> lstOverTimeOver = lstOverTime.Where(m => m.udIsLimitHourLv2_Validate == true || m.udIsLimitHourLv1_Validate == true || m.udIsLimitHour_Validate == true).ToList();
            List<Att_OvertimeEntity> lstOverTimeLimit = new List<Att_OvertimeEntity>();

            if (OtPermit.IsAllowOverLimit_Normal != true
                && OtPermit.IsAllowOverLimit_Normal_Lev1 != true
                && OtPermit.IsAllowOverLimit_Normal_Lev2 != true
                && OtPermit.IsAllowOverLimit_AllowOver != true
                && OtPermit.IsAllowOverLimit_AllowOver_Lev1 != true
                && OtPermit.IsAllowOverLimit_AllowOver_Lev2 != true)
            {
                lstOverTimeLimit = lstOverTimeOver.ToList();
                return lstOverTimeLimit;
            }
            else
            {

                if (OtPermit.IsAllowOverLimit_Normal == true || OtPermit.IsAllowOverLimit_AllowOver == true)
                {
                    List<Att_OvertimeEntity> lstOverTimeNotAllow_Lev0 = lstOverTimeOver.Where(m => m.udIsLimitHour_Validate == true).ToList();
                    foreach (var overtime in lstOverTimeNotAllow_Lev0)
                    {
                        DateTime Workday = overtime.WorkDate;
                        Guid ProfileID = overtime.ProfileID;
                        bool PersonHaveAllowOver = lstAllowLimitOvertime.Any(m => m.ProfileID == ProfileID && m.DateStart <= Workday && m.DateEnd >= Workday);
                        //PersonHaveAllowOver = true; Nhân viên này có trong ds đc phép vượt
                        //PersonHaveAllowOver = true; Nhân viên này không có trong ds đc phép vượt
                        if (OtPermit.IsAllowOverLimit_Normal == true && !PersonHaveAllowOver)
                        {
                            lstOverTimeLimit.Add(overtime);
                        }
                        else if (OtPermit.IsAllowOverLimit_AllowOver == true && PersonHaveAllowOver)
                        {
                            lstOverTimeLimit.Add(overtime);
                        }
                    }
                }
                if (OtPermit.IsAllowOverLimit_Normal_Lev1 == true || OtPermit.IsAllowOverLimit_AllowOver_Lev1 == true)
                {
                    List<Att_OvertimeEntity> lstOverTimeNotAllow_Lev1 = lstOverTimeOver.Where(m => m.udIsLimitHourLv1_Validate == true).ToList();
                    foreach (var overtime in lstOverTimeNotAllow_Lev1)
                    {
                        DateTime Workday = overtime.WorkDate;
                        Guid ProfileID = overtime.ProfileID;
                        bool PersonHaveAllowOver = lstAllowLimitOvertime.Any(m => m.ProfileID == ProfileID && m.DateStart <= Workday && m.DateEnd >= Workday);
                        if (OtPermit.IsAllowOverLimit_Normal_Lev1 == true && !PersonHaveAllowOver)
                        {
                            lstOverTimeLimit.Add(overtime);
                        }
                        else if (OtPermit.IsAllowOverLimit_AllowOver_Lev1 == true && PersonHaveAllowOver)
                        {
                            lstOverTimeLimit.Add(overtime);
                        }
                    }
                }
                if (OtPermit.IsAllowOverLimit_Normal_Lev2 == true || OtPermit.IsAllowOverLimit_AllowOver_Lev2 == true)
                {
                    List<Att_OvertimeEntity> lstOverTimeNotAllow_Lev2 = lstOverTimeOver.Where(m => m.udIsLimitHourLv2_Validate == true).ToList();
                    foreach (var overtime in lstOverTimeNotAllow_Lev2)
                    {
                        DateTime Workday = overtime.WorkDate;
                        Guid ProfileID = overtime.ProfileID;
                        bool PersonHaveAllowOver = lstAllowLimitOvertime.Any(m => m.ProfileID == ProfileID && m.DateStart <= Workday && m.DateEnd >= Workday);
                        if (OtPermit.IsAllowOverLimit_Normal_Lev2 == true && !PersonHaveAllowOver)
                        {
                            lstOverTimeLimit.Add(overtime);
                        }
                        else if (OtPermit.IsAllowOverLimit_AllowOver_Lev2 == true && PersonHaveAllowOver)
                        {
                            lstOverTimeLimit.Add(overtime);
                        }
                    }
                }
            }
            if (lstOverTimeLimit.Count > 0)
            {
                lstOverTimeLimit = lstOverTimeLimit.Distinct().ToList();
            }
            return lstOverTimeLimit;
        }

        private void FillAllowOTValidate(List<Att_OvertimeEntity> lstOvertime, OvertimePermitEntity OtPermit)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Overtime = new CustomBaseRepository<Att_Overtime>(unitOfWork);
                var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);
                var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);

                if (lstOvertime.Count == 0)
                {
                    return;
                }
                if (OtPermit == null || (OtPermit.limitHour_ByDay == null
                                        && OtPermit.limitHour_ByDay_Lev1 == null
                                        && OtPermit.limitHour_ByDay_Lev2 == null
                                        && OtPermit.limitHour_ByWeek == null
                                        && OtPermit.limitHour_ByWeek_Lev1 == null
                                        && OtPermit.limitHour_ByWeek_Lev2 == null
                                        && OtPermit.limitHour_ByMonth == null
                                        && OtPermit.limitHour_ByMonth_Lev1 == null
                                        && OtPermit.limitHour_ByMonth_Lev2 == null
                                        && OtPermit.limitHour_ByYear == null
                                        && OtPermit.limitHour_ByYear_Lev1 == null
                                        && OtPermit.limitHour_ByYear_Lev2 == null)
                    )
                {
                    return;
                }
                #region getData
                DateTime DateMinInlstOvertime = lstOvertime.Min(m => m.WorkDate);
                DateTime DateMaxInlstOvertime = lstOvertime.Max(m => m.WorkDate);
                List<Guid> lstProfileID = lstOvertime.Select(m => m.ProfileID).Distinct().ToList();

                //Lấy ngày Đầu Năm, Đầu Tuần ->> Ngày nào nhỏ nhất thì lấy theo Ngày đó làm mốc
                DateTime DateMin = DateTime.MinValue;
                DateTime DateBeginYear = new DateTime(DateMinInlstOvertime.Year, 1, 1);
                DateTime DateBeginMonth = new DateTime(DateMinInlstOvertime.Year, DateMinInlstOvertime.Month, 1);
                DateTime DateBeginWeek = DateTime.MinValue;
                DateTime DateEndWeek = DateTime.MinValue;
                Common.GetStartEndWeek(DateMinInlstOvertime.Date, out DateBeginWeek, out DateEndWeek);
                DateMin = DateBeginYear < DateBeginWeek ? DateBeginYear : DateBeginWeek;
                DateTime DateMax = DateMaxInlstOvertime.AddYears(1);

                //string E_SUBMIT = OverTimeStatus.E_SUBMIT.ToString();
                //string E_SUBMIT_TEMP = OverTimeStatus.E_SUBMIT_TEMP.ToString();
                //string E_FIRST_APPROVED = OverTimeStatus.E_FIRST_APPROVED.ToString();
                //string E_WAIT_APPROVED = OverTimeStatus.E_WAIT_APPROVED.ToString();
                string E_APPROVED = OverTimeStatus.E_APPROVED.ToString();
                //string E_CONFIRM = OverTimeStatus.E_CONFIRM.ToString();
                string E_CASHOUT = MethodOption.E_CASHOUT.ToString();

                List<Guid> lstOvertimeAlreadyID = lstOvertime.Where(m => m.udAlreadyOvertimeID != null).Select(m => m.udAlreadyOvertimeID ?? Guid.Empty).Distinct().ToList();


                //var lstOvertimeInDb = repoAtt_Overtime.FindBy(m => m.IsDelete == null && m.WorkDate >= DateMin && m.WorkDate < DateMax && lstProfileID.Contains(m.ProfileID)
                //    && (m.MethodPayment == null || (m.MethodPayment != null && m.MethodPayment == E_CASHOUT))
                //&& m.Status == E_APPROVED)
                //    .Select(m => new { m.Status, m.ProfileID, m.WorkDate, m.RegisterHours, m.ApproveHours, m.ConfirmHours });
                var lstOvertimeInDb = new List<Att_Overtime>().Select(m => new { m.Status, m.ProfileID, m.WorkDate, m.RegisterHours, m.ApproveHours, m.ConfirmHours });
                if (lstProfileID.Count < 2000)
                {
                    lstOvertimeInDb = repoAtt_Overtime.FindBy(m => m.IsDelete == null && m.WorkDate >= DateMin && m.WorkDate < DateMax && lstProfileID.Contains(m.ProfileID)
                           && (m.MethodPayment == null || (m.MethodPayment != null && m.MethodPayment == E_CASHOUT))
                       && m.Status == E_APPROVED)
                       .Select(m => new { m.Status, m.ProfileID, m.WorkDate, m.RegisterHours, m.ApproveHours, m.ConfirmHours }).ToList();
                }
                else
                {
                    lstOvertimeInDb = repoAtt_Overtime.FindBy(m => m.IsDelete == null && m.WorkDate >= DateMin && m.WorkDate < DateMax
                           && (m.MethodPayment == null || (m.MethodPayment != null && m.MethodPayment == E_CASHOUT))
                       && m.Status == E_APPROVED)
                       .Select(m => new { m.Status, m.ProfileID, m.WorkDate, m.RegisterHours, m.ApproveHours, m.ConfirmHours }).ToList();
                }

                var lstShift = repoCat_Shift.GetAll().Select(m => new { m.ID, m.InTime, m.CoBreakOut, m.CoBreakIn, m.CoOut });

                string E_HOLIDAY_HLD = HolidayType.E_HOLIDAY_HLD.ToString();
                List<DateTime> lstHoliday = repoCat_DayOff.FindBy(m => m.Type == E_HOLIDAY_HLD).Select(m => m.DateOff).ToList<DateTime>();
                #endregion

                #region processing

                lstOvertime = lstOvertime.OrderBy(m => m.WorkDateRoot).ThenBy(m => m.WorkDate).ToList();
                foreach (var ProfileID in lstProfileID)
                {
                    Double RegisterPlus_Year_Validate = 0;
                    Double RegisterPlus_Month_Validate = 0;
                    Double RegisterPlus_Week_Validate = 0;

                    DateTime BeginYear = DateTime.MinValue;
                    DateTime EndYear = DateTime.MinValue;
                    DateTime BeginMonth = DateTime.MinValue;
                    DateTime EndMonth = DateTime.MinValue;
                    DateTime BeginWeek = DateTime.MinValue;
                    DateTime EndWeek = DateTime.MinValue;
                    DateTime BeginDate = DateTime.MinValue;
                    DateTime EndDate = DateTime.MinValue;

                    bool isResetYear = false;
                    bool isResetMonth = false;
                    bool isResetWeek = false;

                    List<Att_OvertimeEntity> lstOvertime_ByProfile = lstOvertime.Where(m => m.ProfileID == ProfileID).ToList();
                    var lstOvertime_ByProfile_DB = lstOvertimeInDb.Where(m => m.ProfileID == ProfileID).ToList();

                    foreach (var OT in lstOvertime_ByProfile)
                    {
                        DateTime workday = OT.WorkDateRoot ?? OT.WorkDate;
                        #region ResetPlus
                        //reset year
                        if (workday.Date > EndYear)
                        {
                            BeginYear = new DateTime(workday.Year, 1, 1);
                            EndYear = BeginYear.AddYears(1).AddMinutes(-1);
                            if (string.IsNullOrEmpty(OT.udOvertimeStatus))
                                RegisterPlus_Year_Validate = OT.RegisterHours;
                            isResetYear = true;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(OT.udOvertimeStatus))
                                RegisterPlus_Year_Validate += OT.RegisterHours;
                            isResetYear = false;
                        }
                        //Reset Month
                        if (workday.Date > EndMonth)
                        {
                            BeginMonth = new DateTime(workday.Year, workday.Month, 1);
                            EndMonth = BeginMonth.AddMonths(1).AddMinutes(-1);
                            if (string.IsNullOrEmpty(OT.udOvertimeStatus))
                                RegisterPlus_Month_Validate = OT.RegisterHours;
                            isResetMonth = true;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(OT.udOvertimeStatus))
                                RegisterPlus_Month_Validate += OT.RegisterHours;
                            isResetMonth = false;
                        }
                        //Reset Week
                        if (workday.Date > EndWeek)
                        {
                            Common.GetStartEndWeek(workday.Date, out BeginWeek, out EndWeek);
                            if (string.IsNullOrEmpty(OT.udOvertimeStatus))
                                RegisterPlus_Week_Validate = OT.RegisterHours;
                            isResetWeek = true;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(OT.udOvertimeStatus))
                                RegisterPlus_Week_Validate += OT.RegisterHours;
                            isResetWeek = false;
                        }
                        //Reset Day
                        #endregion
                        #region getDataExactHour
                        if (isResetYear)
                        {
                            RegisterPlus_Year_Validate += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginYear && m.WorkDate < EndYear && m.Status == E_APPROVED && m.ApproveHours != null).Sum(m => m.ApproveHours.Value);
                        }
                        if (isResetMonth)
                        {
                            RegisterPlus_Month_Validate += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginMonth && m.WorkDate < EndMonth && m.Status == E_APPROVED && m.ApproveHours != null).Sum(m => m.ApproveHours.Value);
                        }
                        if (isResetWeek)
                        {
                            RegisterPlus_Week_Validate += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginWeek && m.WorkDate < EndWeek && m.Status == E_APPROVED && m.ApproveHours != null).Sum(m => m.ApproveHours.Value);
                        }

                        #endregion
                        //Cập nhật lại cho OT voi nhung trang thai can thiet
                        CalOverAllowOT(OT, RegisterPlus_Week_Validate, RegisterPlus_Month_Validate, RegisterPlus_Year_Validate, OtPermit);

                    }
                }
                //unitOfWork.SaveChanges();
                #endregion
            }
        }

        /// <summary>
        /// [Hien.Nguyen] - Chức năng không tính tăng ca - màng hình Phân Tích Tăng Ca
        /// </summary>
        /// <param name="lstOvertimeEntity"></param>
        public void CalNonAllowOvertime(List<Att_OvertimeEntity> lstOvertime, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_workday = new CustomBaseRepository<Att_Workday>(unitOfWork);
                var repoAtt_NoneOvertime = new CustomBaseRepository<Att_NonOverTime>(unitOfWork);
                if (lstOvertime.Count == 0 || lstOvertime.Where(m => m.WorkDateRoot != null).Count() == 0)
                {
                    return;
                }
                DateTime minDate = lstOvertime.Min(m => m.WorkDateRoot.Value);
                DateTime maxDate = lstOvertime.Max(m => m.WorkDateRoot.Value);
                List<Guid> lstProfileID = lstOvertime.Select(m => m.ProfileID).Distinct().ToList();
                string E_REJECTED = OverTimeStatus.E_REJECTED.ToString();
                // List<ProfileDateTime> lstWorkdayNotOT = new List<ProfileDateTime>();

                List<Guid> lstOvertimeIDChange = new List<Guid>();

                foreach (var item in lstOvertime)
                {

                    if (item.WorkDateRoot == null)
                        continue;
                    if (item.IsNonOvertime == true)
                        continue;
                    if (string.IsNullOrEmpty(item.udOvertimeStatus))
                    {
                        //ProfileDateTime workdayNotOt = new ProfileDateTime();
                        //workdayNotOt.ProfileID = item.ProfileID;
                        //workdayNotOt.WorkDate = item.WorkDateRoot.Value;
                        //workdayNotOt.OvertimeID = item.ID;
                        //lstWorkdayNotOT.Add(workdayNotOt);
                        lstOvertimeIDChange.Add(item.ID);
                    }
                }


                List<Att_OvertimeEntity> lstOvertimeInCache = lstOvertime;
                List<Att_OvertimeEntity> lstOvertimeInCache_Profile = lstOvertimeInCache.Where(m => lstOvertimeIDChange.Contains(m.ID)).ToList();

                List<Att_OvertimeEntity> lstOvertimeNonBe_Insert = new List<Att_OvertimeEntity>();
                foreach (var item in lstOvertimeInCache_Profile)
                {
                    //if (lstWorkdayNotOT.Any(m => m.ProfileID == item.ProfileID && m.WorkDate == item.WorkDateRoot))
                    //{
                    item.IsNonOvertime = true;
                    lstOvertimeNonBe_Insert.Add(item);
                    // }
                }
                lstOvertimeInCache.RemoveRange(lstOvertimeInCache_Profile);

                //Để tính lại lũy kế sau khi xong
                OvertimePermitEntity overtimePermit = getOvertimePermit(userLogin);
                List<WorkdayCustom> lstWorkday = new List<WorkdayCustom>();
                if (lstProfileID.Count < 2000)
                {
                    lstWorkday = repoAtt_workday
                        .FindBy(m => m.IsDelete == null && m.WorkDate >= minDate && m.WorkDate <= maxDate && lstProfileID.Contains(m.ProfileID))
                        .Select(m => new WorkdayCustom() { ID = m.ID, ProfileID = m.ProfileID, ShiftID = m.ShiftID, ShiftApprove = m.ShiftApprove, WorkDate = m.WorkDate, Status = m.Status, InTime1 = m.InTime1, InTime2 = m.InTime2, OutTime1 = m.OutTime1, OutTime2 = m.OutTime2 })
                        .ToList<WorkdayCustom>();
                }
                else
                {
                    lstWorkday = repoAtt_workday
                        .FindBy(m => m.IsDelete == null && m.WorkDate >= minDate && m.WorkDate <= maxDate)
                        .Select(m => new WorkdayCustom() { ID = m.ID, ProfileID = m.ProfileID, ShiftID = m.ShiftID, ShiftApprove = m.ShiftApprove, WorkDate = m.WorkDate, Status = m.Status, InTime1 = m.InTime1, InTime2 = m.InTime2, OutTime1 = m.OutTime1, OutTime2 = m.OutTime2 })
                        .ToList<WorkdayCustom>();
                }
                FillterAllowOvertime(context, lstOvertimeInCache_Profile, overtimePermit, lstWorkday);
                lstOvertimeInCache.AddRange(lstOvertimeInCache_Profile);
                lstOvertimeInCache = lstOvertimeInCache.OrderBy(m => m.WorkDate.Date).ThenBy(m => m.ProfileID).ToList();
                lstOvertimeCache = lstOvertimeInCache;

                List<Att_NonOverTime> LstNonOTInsert = new List<Att_NonOverTime>();
                foreach (var item in lstOvertimeNonBe_Insert)
                {
                    Att_NonOverTime nonOT = new Att_NonOverTime();
                    nonOT.ID = Guid.NewGuid();
                    nonOT.ProfileID = item.ProfileID;
                    nonOT.WorkDay = item.WorkDateRoot.Value;
                    nonOT.OvertimeTypeID = item.OvertimeTypeID;
                    nonOT.Type = item.udTypeBeginOTWithShift;
                    LstNonOTInsert.Add(nonOT);
                    repoAtt_NoneOvertime.Add(nonOT);
                }
                unitOfWork.SaveChanges();
            }
            //BindToGrid(lstOvertimeCache);
        }

        /// <summary>
        /// [Hien.Nguyen] - Chức năng tính tăng ca - màng hình Phân Tích Tăng Ca
        /// </summary>
        /// <param name="lstOvertimeEntity"></param>
        public void CalAllowOvertime(List<Att_OvertimeEntity> lstOvertime, string userLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_workday = new CustomBaseRepository<Att_Workday>(unitOfWork);
                var repoAtt_NoneOvertime = new CustomBaseRepository<Att_NonOverTime>(unitOfWork);
                if (lstOvertime.Count == 0 || lstOvertime.Where(m => m.WorkDateRoot != null).Count() == 0)
                {
                    return;
                }

                DateTime minDate = lstOvertime.Min(m => m.WorkDateRoot.Value);
                DateTime maxDate = lstOvertime.Max(m => m.WorkDateRoot.Value);
                List<Guid> lstProfileID = lstOvertime.Select(m => m.ProfileID).Distinct().ToList();

                List<ProfileDateTime> lstWorkdayCanBeOT = new List<ProfileDateTime>();

                foreach (var item in lstOvertime)
                {
                    if (item.WorkDateRoot == null)
                        continue;
                    if (item.IsNonOvertime == null)
                        continue;

                    ProfileDateTime workdayNotOt = new ProfileDateTime();
                    workdayNotOt.ProfileID = item.ProfileID;
                    workdayNotOt.WorkDate = item.WorkDateRoot.Value;
                    lstWorkdayCanBeOT.Add(workdayNotOt);
                }



                List<Att_OvertimeEntity> lstOvertimeInCache = lstOvertimeCache;
                List<Att_OvertimeEntity> lstOvertimeInCache_Profile = lstOvertimeInCache.Where(m => lstProfileID.Contains(m.ProfileID)).ToList();

                var _tmp = lstOvertimeInCache_Profile;
                for (int i = 0; i < _tmp.Count(); i++)
                    lstOvertimeInCache.Remove(lstOvertimeInCache_Profile[i]);

                List<Att_OvertimeEntity> lstOvertimeNonBe_Delete = new List<Att_OvertimeEntity>();
                foreach (var item in lstOvertimeInCache_Profile)
                {
                    if (lstWorkdayCanBeOT.Any(m => m.ProfileID == item.ProfileID && m.WorkDate == item.WorkDateRoot))
                    {
                        item.IsNonOvertime = null;
                        lstOvertimeNonBe_Delete.Add(item);
                    }
                }
                //Để tính lại lũy kế sau khi xong
                OvertimePermitEntity overtimePermit = getOvertimePermit(userLogin);
                //List<Att_WorkdayEntity> lstWorkday = repoAtt_workday.FindBy(m => m.WorkDate >= minDate && m.WorkDate <= maxDate && lstProfileID.Contains(m.ProfileID)).ToList().Translate<Att_WorkdayEntity>();
                List<WorkdayCustom> lstWorkday = new List<WorkdayCustom>();
                if (lstProfileID.Count < 2000)
                {
                    lstWorkday = repoAtt_workday
                        .FindBy(m => m.IsDelete == null && m.WorkDate >= minDate && m.WorkDate <= maxDate && lstProfileID.Contains(m.ProfileID))
                        .Select(m => new WorkdayCustom() { ID = m.ID, ProfileID = m.ProfileID, ShiftID = m.ShiftID, ShiftApprove = m.ShiftApprove, WorkDate = m.WorkDate, Status = m.Status, InTime1 = m.InTime1, InTime2 = m.InTime2, OutTime1 = m.OutTime1, OutTime2 = m.OutTime2 })
                        .ToList<WorkdayCustom>();
                }
                else
                {
                    lstWorkday = repoAtt_workday
                        .FindBy(m => m.IsDelete == null && m.WorkDate >= minDate && m.WorkDate <= maxDate)
                         .Select(m => new WorkdayCustom() { ID = m.ID, ProfileID = m.ProfileID, ShiftID = m.ShiftID, ShiftApprove = m.ShiftApprove, WorkDate = m.WorkDate, Status = m.Status, InTime1 = m.InTime1, InTime2 = m.InTime2, OutTime1 = m.OutTime1, OutTime2 = m.OutTime2 })
                         .ToList<WorkdayCustom>();
                }

                FillterAllowOvertime(context, lstOvertimeInCache_Profile, overtimePermit, lstWorkday);
                lstOvertimeInCache.AddRange(lstOvertimeInCache_Profile);
                lstOvertimeInCache = lstOvertimeInCache.OrderBy(m => m.WorkDate.Date).ThenBy(m => m.ProfileID).ToList();
                lstOvertimeCache = lstOvertimeInCache;

                //List<Att_NonOverTime> LstNonOTSelect = repoAtt_NoneOvertime.FindBy(m => m.WorkDay >= minDate && m.WorkDay <= maxDate && lstProfileID.Contains(m.ProfileID.Value)).ToList<Att_NonOverTime>();
                List<Att_NonOverTime> LstNonOTSelect = new List<Att_NonOverTime>();
                if (lstProfileID.Count < 2000)
                {
                    LstNonOTSelect = repoAtt_workday
                        .FindBy(m => m.IsDelete == null && m.WorkDate >= minDate && m.WorkDate <= maxDate && lstProfileID.Contains(m.ProfileID)).ToList().Translate<Att_NonOverTime>();
                }
                else
                {
                    LstNonOTSelect = repoAtt_workday
                        .FindBy(m => m.IsDelete == null && m.WorkDate >= minDate && m.WorkDate <= maxDate).ToList().Translate<Att_NonOverTime>();
                }

                foreach (var item in lstOvertimeNonBe_Delete)
                {
                    Att_NonOverTime NonOT = LstNonOTSelect.Where(m => m.ProfileID == item.ProfileID && m.OvertimeTypeID == item.OvertimeTypeID && m.WorkDay == item.WorkDateRoot && m.Type == item.udTypeBeginOTWithShift).FirstOrDefault();
                    if (NonOT != null)
                    {
                        NonOT.IsDelete = true;
                    }
                }
                if (LstNonOTSelect.Count > 0)
                {
                    repoAtt_NoneOvertime.Edit(LstNonOTSelect);
                    unitOfWork.SaveChanges();
                }
                //BindToGrid(lstOvertimeCache);
            }
        }

        // Xử lý dữ liệu của 3 cột tích lũy tháng, năm, không tích lũy trong ds tăng ca
        public void FillterAllowOvertime(List<Att_OvertimeEntity> lstOvertime)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Overtime = new CustomBaseRepository<Att_Overtime>(unitOfWork);
                if (lstOvertime.Count == 0)
                {
                    return;
                }

                #region getData
                DateTime DateMinInlstOvertime = lstOvertime.Min(m => m.WorkDate);
                DateTime DateMaxInlstOvertime = lstOvertime.Max(m => m.WorkDate);
                List<Guid> lstProfileID = lstOvertime.Select(m => m.ProfileID).Distinct().ToList();

                //Lấy ngày Đầu Năm, Đầu Tuần ->> Ngày nào nhỏ nhất thì lấy theo Ngày đó làm mốc
                DateTime DateMin = DateTime.MinValue;
                DateTime DateBeginYear = new DateTime(DateMinInlstOvertime.Year, 1, 1);
                DateTime DateBeginMonth = new DateTime(DateMinInlstOvertime.Year, DateMinInlstOvertime.Month, 1);
                DateTime DateBeginWeek = DateTime.MinValue;
                DateTime DateEndWeek = DateTime.MinValue;
                Common.GetStartEndWeek(DateMinInlstOvertime.Date, out DateBeginWeek, out DateEndWeek);
                DateMin = DateBeginYear < DateBeginWeek ? DateBeginYear : DateBeginWeek;
                DateTime DateMax = DateMaxInlstOvertime.AddYears(1);

                string E_CANCEL = OverTimeStatus.E_CANCEL.ToString();
                string E_APPROVED = OverTimeStatus.E_APPROVED.ToString();
                string E_CONFIRM = OverTimeStatus.E_CONFIRM.ToString();
                string E_FIRST_APPROVED = OverTimeStatus.E_FIRST_APPROVED.ToString();
                string E_REJECTED = OverTimeStatus.E_REJECTED.ToString();
                string E_SUBMIT = OverTimeStatus.E_SUBMIT.ToString();
                string E_SUBMIT_TEMP = OverTimeStatus.E_SUBMIT_TEMP.ToString();
                string E_TEMP = OverTimeStatus.E_TEMP.ToString();
                string E_WAIT_APPROVED = OverTimeStatus.E_WAIT_APPROVED.ToString();

                string E_CASHOUT = MethodOption.E_CASHOUT.ToString();
                //List<Guid> lstOvertimeAlreadyID = lstOvertime.Where(m => m.udAlreadyOvertimeID != null).Select(m => m.udAlreadyOvertimeID ?? Guid.Empty).Distinct().ToList();
                //var lstOvertimeInDb = overtimerespontory.FindBy(m => m.IsDelete == null && m.WorkDate >= DateMin && m.WorkDate < DateMax && lstProfileID.Contains(m.ProfileID)
                //       && (m.MethodPayment == null || (m.MethodPayment != null && m.MethodPayment == E_CASHOUT))
                //       && m.IsNonOvertime == null
                //       && !lstOvertimeAlreadyID.Contains(m.ID)
                //   && m.Status != E_CANCEL && m.Status != E_REJECTED)
                //   .Select(m => new { m.Status, m.ProfileID, m.WorkDate, m.RegisterHours, m.ApproveHours, m.ConfirmHours }).ToList();
                List<Guid> lstOvertimeAlreadyID = lstOvertime.Where(m => m.udAlreadyOvertimeID != null).Select(m => m.udAlreadyOvertimeID ?? Guid.Empty).Distinct().ToList();
                var lstOvertimeInDb = new List<Att_Overtime>().Select(m => new { m.Status, m.ProfileID, m.WorkDate, m.RegisterHours, m.ApproveHours, m.ConfirmHours });
                if (lstProfileID.Count < 2000)
                {
                    lstOvertimeInDb = repoAtt_Overtime.FindBy(m => m.IsDelete == null && m.WorkDate >= DateMin && m.WorkDate < DateMax && lstProfileID.Contains(m.ProfileID)
                            && (m.MethodPayment == null || (m.MethodPayment != null && m.MethodPayment == E_CASHOUT))
                            && m.IsNonOvertime == null
                            && !lstOvertimeAlreadyID.Contains(m.ID)
                        && m.Status != E_CANCEL && m.Status != E_REJECTED)
                        .Select(m => new { m.Status, m.ProfileID, m.WorkDate, m.RegisterHours, m.ApproveHours, m.ConfirmHours }).ToList();
                }
                else
                {
                    lstOvertimeInDb = repoAtt_Overtime.FindBy(m => m.IsDelete == null && m.WorkDate >= DateMin && m.WorkDate < DateMax
                            && (m.MethodPayment == null || (m.MethodPayment != null && m.MethodPayment == E_CASHOUT))
                            && m.IsNonOvertime == null
                            && !lstOvertimeAlreadyID.Contains(m.ID)
                        && m.Status != E_CANCEL && m.Status != E_REJECTED)
                        .Select(m => new { m.Status, m.ProfileID, m.WorkDate, m.RegisterHours, m.ApproveHours, m.ConfirmHours }).ToList();
                }

                #endregion
                #region processing

                lstOvertime = lstOvertime.OrderBy(m => m.WorkDateRoot).ThenBy(m => m.WorkDate).ToList();
                foreach (var ProfileID in lstProfileID)
                {
                    Double RegisterPlus_Year = 0;
                    Double RegisterPlus_Month = 0;
                    DateTime BeginYear = DateTime.MinValue;
                    DateTime EndYear = DateTime.MinValue;
                    DateTime BeginMonth = DateTime.MinValue;
                    DateTime EndMonth = DateTime.MinValue;
                    DateTime BeginWeek = DateTime.MinValue;
                    DateTime EndWeek = DateTime.MinValue;
                    DateTime BeginDate = DateTime.MinValue;
                    DateTime EndDate = DateTime.MinValue;
                    bool isResetYear = false;
                    bool isResetMonth = false;
                    List<Att_OvertimeEntity> lstOvertime_ByProfile = lstOvertime.Where(m => m.ProfileID == ProfileID).ToList();
                    var lstOvertime_ByProfile_DB = lstOvertimeInDb.Where(m => m.ProfileID == ProfileID).ToList();

                    foreach (var OT in lstOvertime_ByProfile)
                    {
                        DateTime workday = OT.WorkDateRoot ?? OT.WorkDate;
                        #region ResetPlus
                        //reset year
                        if (workday.Date > EndYear)
                        {
                            BeginYear = new DateTime(workday.Year, 1, 1);
                            EndYear = BeginYear.AddYears(1).AddMinutes(-1);
                            if (OT.IsNonOvertime != true && OT.Status != E_REJECTED)
                                RegisterPlus_Year = OT.RegisterHours;
                            isResetYear = true;
                        }
                        else
                        {
                            if (OT.IsNonOvertime != true && OT.Status != E_REJECTED)
                                RegisterPlus_Year += OT.RegisterHours;
                            isResetYear = false;
                        }
                        //Reset Month
                        if (workday.Date > EndMonth)
                        {
                            BeginMonth = new DateTime(workday.Year, workday.Month, 1);
                            EndMonth = BeginMonth.AddMonths(1).AddMinutes(-1);
                            if (OT.IsNonOvertime != true && OT.Status != E_REJECTED)
                                RegisterPlus_Month = OT.RegisterHours;
                            isResetMonth = true;
                        }
                        else
                        {
                            if (OT.IsNonOvertime != true && OT.Status != E_REJECTED)
                                RegisterPlus_Month += OT.RegisterHours;
                            isResetMonth = false;
                        }

                        #endregion
                        #region getDataExactHour
                        //1. Có 3 loại : register, Approve, Confirm
                        //2. Có 4 dạng la: Year, Month, Week, Day
                        //3. các khoảng thời gian đã được xác định phía trên.
                        //4. Cộng khoảng thời gian trên kia lại và truyền vào hàm
                        if (isResetYear)
                        {
                            RegisterPlus_Year += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginYear && m.WorkDate < EndYear && (m.Status == E_SUBMIT || m.Status == E_SUBMIT_TEMP || m.Status == E_FIRST_APPROVED || m.Status == E_WAIT_APPROVED)).Sum(m => m.RegisterHours);
                            RegisterPlus_Year += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginYear && m.WorkDate.Date <= workday && m.Status == E_APPROVED && m.ApproveHours != null).Sum(m => m.ApproveHours.Value);
                            RegisterPlus_Year += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginYear && m.WorkDate < EndYear && m.Status == E_CONFIRM).Sum(m => m.ConfirmHours);
                        }
                        if (isResetMonth)
                        {
                            RegisterPlus_Month += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginMonth && m.WorkDate < EndMonth && (m.Status == E_SUBMIT || m.Status == E_SUBMIT_TEMP || m.Status == E_FIRST_APPROVED || m.Status == E_WAIT_APPROVED)).Sum(m => m.RegisterHours);
                            RegisterPlus_Month += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginMonth && m.WorkDate.Date <= workday && m.Status == E_APPROVED && m.ApproveHours != null).Sum(m => m.ApproveHours.Value);
                            RegisterPlus_Month += lstOvertime_ByProfile_DB.Where(m => m.WorkDate > BeginMonth && m.WorkDate < EndMonth && m.Status == E_CONFIRM).Sum(m => m.ConfirmHours);
                        }
                        #endregion
                        OT.udHourByMonth = RegisterPlus_Month;
                        OT.udHourByYear = RegisterPlus_Year;
                    }

                }
                #endregion
            }

        }


        /// <summary>
        /// Xử Lý Duyệt cho Tăng ca
        /// </summary>
        /// <param name="lstOvertimeID"></param>
        public void ApproveOvertime(List<Guid> lstOvertimeID, Guid loginUserID)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                List<string> lstStatusOvertime = new List<string>(){
                    OverTimeStatus.E_REJECTED.ToString(),
                    OverTimeStatus.E_CANCEL.ToString(),
                    OverTimeStatus.E_CONFIRM.ToString(),
                    OverTimeStatus.E_APPROVED.ToString()
                };
                List<Att_Overtime> lstOvertime = unitOfWork.CreateQueryable<Att_Overtime>(m => !lstStatusOvertime.Contains(m.Status) && lstOvertimeID.Contains(m.ID)).ToList();

                int AprroveLevel = 0;
                string HRM_ATT_CONFIG_NUMBER_LEAVE_APPROVE_OVERTIME = AppConfig.HRM_ATT_CONFIG_NUMBER_LEAVE_APPROVE_OVERTIME.ToString();
                var AttConfig = unitOfWork.CreateQueryable<Sys_AllSetting>(m => m.Name == HRM_ATT_CONFIG_NUMBER_LEAVE_APPROVE_OVERTIME).FirstOrDefault();
                if (AttConfig != null)
                {
                    int.TryParse(AttConfig.Value1, out AprroveLevel);
                }
                foreach (var item in lstOvertime)
                {
                    //Logic UserApproveID -- Duyet cap 1
                    //UserApproveID3 -- duyet cap 2
                    //UserApproveID2  -- duyet cap 3 (cap cuoi)
                    item.Status = GetStatusApproveOvertime(item, loginUserID);
                }
                unitOfWork.SaveChanges(loginUserID);
            }


            //Kiểm tra cấu hình xem có tính duyệt 3 cấp hay không
            //Kiểm tra xem nhân viên có quyền duyệt ko
            //từ chối rồi sẽ không duyệt lại
            //khi có cột duyệt thứ 3 và check userloginID để gán status cho người duyệt
            //cập nhật ngày update và giờ update 
            //check trong db xem có cột ngày approve ko thì cũng chuyển luôn
            //Vấn đề gửi mail?

        }
        public string GetStatusApproveOvertime(Att_Overtime overtime, Guid loginUserID)
        {
            string Status = string.Empty;
            if (overtime.UserApproveID2 != null && overtime.UserApproveID2 == loginUserID)
            {
                Status = OverTimeStatus.E_APPROVED.ToString();
            }
            else if (overtime.UserApproveID3 != null && overtime.UserApproveID3 == loginUserID)
            {
                Status = OverTimeStatus.E_APPROVED1.ToString();
            }
            else if (overtime.UserApproveID != null && overtime.UserApproveID == loginUserID)
            {
                Status = OverTimeStatus.E_FIRST_APPROVED.ToString();
            }
            if (Status == OverTimeStatus.E_APPROVED1.ToString() && (overtime.UserApproveID2 == null || overtime.UserApproveID3 == null))
            {
                Status = OverTimeStatus.E_APPROVED.ToString();
            }
            if (Status == string.Empty)
            {
                Status = OverTimeStatus.E_APPROVED.ToString();
            }
            return Status;
        }
    }
}
