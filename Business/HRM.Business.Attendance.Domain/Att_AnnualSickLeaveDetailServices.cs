using HRM.Business.Attendance.Models;
using HRM.Data.Attendance.Data;
using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
using System.Linq;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using HRM.Business.HrmSystem.Models;
using System;
using HRM.Business.Hr.Models;
using VnResource.Helper.Data;

namespace HRM.Business.Attendance.Domain
{
    public class Att_AnnualSickLeaveDetailServices : BaseService
    {
        /// <summary>
        /// Button Tìm Kiếm Phép Bệnh
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="orgStructure"></param>
        /// <param name="LstProfileStatus"></param>
        /// <returns></returns>
        public List<Att_AnnualLeaveDetailEntity> SearchAnnualSickLeaveDetail(int Year, string orgStructure, string LstProfileStatus, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoAtt_AnnualLeaveDetail = new CustomBaseRepository<Att_AnnualLeaveDetail>(unitOfWork);
                var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var repoHre_HDTJob = new CustomBaseRepository<Hre_HDTJob>(unitOfWork);

                string E_SICK_LEAVE = AnnualLeaveDetailType.E_SICK_LEAVE.ToString();

                Att_AnnualLeaveDetail AnnualLeaveDetailTop = AnnualLeaveDetailTop = repoAtt_AnnualLeaveDetail
                    .FindBy(m => m.IsDelete == null && m.Type == E_SICK_LEAVE && m.Year == Year).FirstOrDefault();
                if (AnnualLeaveDetailTop == null)
                {
                    return new List<Att_AnnualLeaveDetailEntity>();
                }
                DateTime beginyear = (AnnualLeaveDetailTop == null || AnnualLeaveDetailTop.MonthStart == null) ? new DateTime(Year, 1, 1) : AnnualLeaveDetailTop.MonthStart.Value;
                DateTime endYear = beginyear.AddYears(1).AddMinutes(-1);

                List<object> lstObj = new List<object>();
                lstObj.Add(orgStructure);
                lstObj.Add(null);
                lstObj.Add(null);
                var lstProfileQuery = GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status).ToList();

                if (LstProfileStatus != null)
                {
                    if (LstProfileStatus == StatusEmpLeaveDetail.E_NEWEMPINYEAR.ToString())
                    {
                        lstProfileQuery = lstProfileQuery.Where(m => m.DateHire != null && m.DateHire >= beginyear && m.DateHire <= endYear).ToList();
                    }
                    if (LstProfileStatus == StatusEmpLeaveDetail.E_RESIGNEMPINYEAR.ToString())
                    {
                        lstProfileQuery = lstProfileQuery.Where(m => m.DateQuit != null && m.DateQuit >= beginyear && m.DateQuit <= endYear).ToList();
                    }
                    if (LstProfileStatus == StatusEmpLeaveDetail.E_EMPOFHDT4.ToString())
                    {
                        string Job4 = HDTJobType.E_Four.ToString();
                        List<Guid> lstprofileHDT4 = repoHre_HDTJob
                            .FindBy(m => m.IsDelete == null && m.Type == Job4 && m.DateFrom <= endYear && m.DateTo >= beginyear && m.ProfileID != null)
                            .Select(m => m.ProfileID.Value).ToList<Guid>();
                        lstProfileQuery = lstProfileQuery.Where(m => lstprofileHDT4.Contains(m.ID)).ToList();
                    }
                    if (LstProfileStatus == StatusEmpLeaveDetail.E_EMPOFHDT5.ToString())
                    {
                        string Job5 = HDTJobType.E_Five.ToString();
                        List<Guid> lstprofileHDT5 = repoHre_HDTJob
                            .FindBy(m => m.IsDelete == null && m.Type == Job5 && m.DateFrom <= endYear && m.DateTo >= beginyear && m.ProfileID != null)
                            .Select(m => m.ProfileID.Value).ToList<Guid>();
                        lstProfileQuery = lstProfileQuery.Where(m => lstprofileHDT5.Contains(m.ID)).ToList();
                    }
                }

                List<Guid> lstProfileID = lstProfileQuery.Select(m => m.ID).Distinct().ToList<Guid>();
                List<Att_AnnualLeaveDetailEntity> lstAnnDetail = repoAtt_AnnualLeaveDetail
                    .FindBy(m => m.IsDelete == null && m.Type == E_SICK_LEAVE && m.Year == Year
                    && m.ProfileID != null && lstProfileID.Contains(m.ProfileID.Value)).ToList().Translate<Att_AnnualLeaveDetailEntity>();

                foreach (var ann in lstAnnDetail)
                {
                    var pro = lstProfileQuery.Where(s => s.ID == ann.ProfileID).FirstOrDefault();

                    ann.ProfileName = pro.ProfileName ?? string.Empty;
                    ann.CodeEmp = pro.CodeEmp ?? string.Empty;
                    ann.OrgStructureName = pro.OrgStructureName ?? string.Empty;
                    ann.DateHire = pro.DateHire ?? null;
                }
                return lstAnnDetail;

            }
        }

        /// <summary>
        /// Button Tính Phép Bệnh
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="orgStructure"></param>
        /// <param name="LstProfileStatus"></param>
        /// <returns></returns>
        public bool ComputeAnnualSickLeaveDetail(int Year, string orgStructure, string LstProfileStatus, string UserLogin)
        {
            var result = false;
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoAtt_AnnualLeaveDetail = new CustomBaseRepository<Att_AnnualLeaveDetail>(unitOfWork);
                var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var repoHre_HDTJob = new CustomBaseRepository<Hre_HDTJob>(unitOfWork);
                var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);
                var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                var repoAtt_LeaveDay = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);
                var repoCat_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);


                #region FilterInfo
                string E_SICK_LEAVE = AnnualLeaveDetailType.E_SICK_LEAVE.ToString();

                Att_AnnualLeaveDetail AnnualLeaveDetailTop = repoAtt_AnnualLeaveDetail
                    .FindBy(m => m.IsDelete == null && m.Type == E_SICK_LEAVE && m.Year == Year).FirstOrDefault();

                DateTime beginyear = (AnnualLeaveDetailTop == null || AnnualLeaveDetailTop.MonthStart == null) ? new DateTime(Year, 1, 1) : AnnualLeaveDetailTop.MonthStart.Value;
                DateTime endYear = beginyear.AddYears(1).AddMinutes(-1);

                List<object> lstObj = new List<object>();
                lstObj.Add(orgStructure);
                lstObj.Add(null);
                lstObj.Add(null);
                var lstProfileQuery = GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status).ToList();

                if (LstProfileStatus != null)
                {
                    if (LstProfileStatus == StatusEmpLeaveDetail.E_NEWEMPINYEAR.ToString())
                    {
                        lstProfileQuery = lstProfileQuery.Where(m => m.DateHire != null && m.DateHire >= beginyear && m.DateHire <= endYear).ToList();
                    }
                    if (LstProfileStatus == StatusEmpLeaveDetail.E_RESIGNEMPINYEAR.ToString())
                    {
                        lstProfileQuery = lstProfileQuery.Where(m => m.DateQuit != null && m.DateQuit >= beginyear && m.DateQuit <= endYear).ToList();
                    }
                    if (LstProfileStatus == StatusEmpLeaveDetail.E_EMPOFHDT4.ToString())
                    {
                        string Job4 = HDTJobType.E_Four.ToString();
                        List<Guid> lstprofileHDT4 = repoHre_HDTJob
                            .FindBy(m => m.IsDelete == null && m.Type == Job4 && m.DateFrom <= endYear && m.DateTo >= beginyear && m.ProfileID != null)
                            .Select(m => m.ProfileID.Value).ToList<Guid>();
                        lstProfileQuery = lstProfileQuery.Where(m => lstprofileHDT4.Contains(m.ID)).ToList();
                    }
                    if (LstProfileStatus == StatusEmpLeaveDetail.E_EMPOFHDT5.ToString())
                    {
                        string Job5 = HDTJobType.E_Five.ToString();
                        List<Guid> lstprofileHDT5 = repoHre_HDTJob
                            .FindBy(m => m.IsDelete == null && m.Type == Job5 && m.DateFrom <= endYear && m.DateTo >= beginyear && m.ProfileID != null)
                            .Select(m => m.ProfileID.Value).ToList<Guid>();
                        lstProfileQuery = lstProfileQuery.Where(m => lstprofileHDT5.Contains(m.ID)).ToList();
                    }
                }

                List<Guid> lstProfileID = lstProfileQuery.Select(m => m.ID).Distinct().ToList<Guid>();
                #endregion
                #region get Data
                //string E_STANDARD_WORKDAY = AppConfig.E_STANDARD_WORKDAY.ToString();
                //Sys_AppConfig appconfig = EntityService.CreateQueryable<Sys_AppConfig>(false, GuidContext, Guid.Empty, m => m.Info == E_STANDARD_WORKDAY).FirstOrDefault();
                //if (appconfig == null || string.IsNullOrEmpty(appconfig.Value76) || string.IsNullOrEmpty(appconfig.Value77))
                //{
                //    Common.MessageBoxs(Messages.Msg, Messages.PleaseConfigSickLeaveAtTotalConfig, MessageBox.Icon.WARNING, string.Empty);
                //    return;
                //}

                string HRM_ATT_ANNUALSICKLEAVE_ = AppConfig.HRM_ATT_ANNUALSICKLEAVE_.ToString();
                List<object> lstO = new List<object>();
                lstO.Add(HRM_ATT_ANNUALSICKLEAVE_);
                lstO.Add(null);
                lstO.Add(null);
                var config = GetData<Sys_AllSettingEntity>(lstO, ConstantSql.hrm_sys_sp_get_AllSetting, UserLogin, ref status);

                var formular1 = config.Where(s => s.Name == AppConfig.HRM_ATT_ANNUALSICKLEAVE_FORMULARCONFIG.ToString()).FirstOrDefault();
                var formular2 = config.Where(s => s.Name == AppConfig.HRM_ATT_ANNUALSICKLEAVE_FORMULARCOMPUTE.ToString()).FirstOrDefault();

                if (config == null || string.IsNullOrEmpty(formular1.Value1) || string.IsNullOrEmpty(formular2.Value1))
                {
                    //Common.MessageBoxs(Messages.Msg, Messages.PleaseConfigAnnualLeaveAtTotalConfig, MessageBox.Icon.WARNING, string.Empty);
                    return result;
                }

                string formularConfig = formular1.Value1;
                string formularCompute = formular2.Value1;

                ParamGetConfigANL configAnl = new ParamGetConfigANL();
                (new Att_AttendanceServices()).GetConfigANL(formularConfig, out configAnl);

                int MonthBegin = 1;
                List<string> lstCodeLeaveTypeNonAnl = new List<string>();
                if (configAnl != null && configAnl.monthBeginYear != null)
                {
                    MonthBegin = configAnl.monthBeginYear;
                }
                if (configAnl != null && configAnl.lstCodeLeaveNonANL != null)
                {
                    lstCodeLeaveTypeNonAnl = configAnl.lstCodeLeaveNonANL;
                }
                DateTime BeginYear = new DateTime(Year, MonthBegin, 1);
                DateTime EndYear = BeginYear.AddYears(1).AddMinutes(-1);

                string E_APPROVED = LeaveDayStatus.E_APPROVED.ToString();
                List<DateTime> lstDayOff = repoCat_DayOff
                    .FindBy(m => m.IsDelete == null && m.DateOff > BeginYear && m.DateOff <= EndYear)
                    .Select(m => m.DateOff).Distinct().ToList<DateTime>();
                List<Hre_ProfileEntity> lstprofile = lstProfileQuery
                    .Where(m => m.DateQuit == null || (m.DateQuit != null && m.DateQuit > BeginYear))
                    .ToList<Hre_ProfileEntity>();
                List<Guid> lstLeavedayTypeNonAnl = repoCat_LeaveDayType
                    .FindBy(m => m.IsDelete == null && lstCodeLeaveTypeNonAnl.Contains(m.Code)).Select(m => m.ID).ToList<Guid>();
                List<Att_LeaveDay> lstleavedayNonANl = repoAtt_LeaveDay
                    .FindBy(m => m.IsDelete == null && m.Status == E_APPROVED && lstLeavedayTypeNonAnl.Contains(m.LeaveDayTypeID)
                        && lstProfileID.Contains(m.ProfileID)).ToList<Att_LeaveDay>();
                List<Att_LeaveDay> lstleavedayNonANlInYear = lstleavedayNonANl.Where(m => m.DateStart <= EndYear && m.DateEnd >= BeginYear).ToList<Att_LeaveDay>();
                List<Hre_HDTJob> lstHDTJob = repoHre_HDTJob
                    .FindBy(m => m.IsDelete == null && m.ProfileID != null && lstProfileID.Contains(m.ProfileID.Value)).ToList<Hre_HDTJob>();
                List<Cat_JobTitle> lstJobtitle = repoCat_JobTitle.FindBy(s => s.IsDelete == null).ToList();
                #endregion

                List<Att_AnnualLeaveDetail> lstAnnualDetail = new List<Att_AnnualLeaveDetail>();
                foreach (var item in lstprofile)
                {
                    List<Att_LeaveDay> lstLeavedayNonAnlByProfile = lstleavedayNonANl.Where(m => m.ProfileID == item.ID).ToList();
                    List<Att_LeaveDay> lstLeavedayNonAnlByProfileInYear = lstleavedayNonANlInYear.Where(m => m.ProfileID == item.ID).ToList();
                    List<Hre_HDTJob> lstHDTJobByProfile = lstHDTJob.Where(m => m.ProfileID == item.ID).ToList();
                    double AvailabelInYear = (new Att_AttendanceServices()).GetAnnualLeaveAvailableAllYear(Year, null, item.DateHire,
                        item.DateEndProbation, item.DateQuit, formularConfig,
                        formularCompute, lstLeavedayNonAnlByProfileInYear, lstJobtitle, lstDayOff, lstHDTJobByProfile, lstLeavedayNonAnlByProfile);
                    Att_AnnualLeaveDetail annualProfile = new Att_AnnualLeaveDetail();
                    annualProfile.ID = Guid.NewGuid();
                    annualProfile.ProfileID = item.ID;
                    annualProfile.MonthStart = BeginYear;
                    annualProfile.MonthEnd = EndYear;
                    annualProfile.Year = Year;
                    annualProfile.Type = E_SICK_LEAVE;
                    annualProfile.Month1 = AvailabelInYear;
                    annualProfile.Month2 = AvailabelInYear;
                    annualProfile.Month3 = AvailabelInYear;
                    annualProfile.Month4 = AvailabelInYear;
                    annualProfile.Month5 = AvailabelInYear;
                    annualProfile.Month6 = AvailabelInYear;
                    annualProfile.Month7 = AvailabelInYear;
                    annualProfile.Month8 = AvailabelInYear;
                    annualProfile.Month9 = AvailabelInYear;
                    annualProfile.Month10 = AvailabelInYear;
                    annualProfile.Month11 = AvailabelInYear;
                    annualProfile.Month12 = AvailabelInYear;
                    lstAnnualDetail.Add(annualProfile);
                }

                DataErrorCode DataErr = DataErrorCode.Unknown;

                if (lstAnnualDetail.Count > 0)
                {
                    #region lấy dữ liệu dưới DB xóa đi
                    List<Guid> lstProfileID_InDB = lstAnnualDetail.Where(m => m.ProfileID != null).Select(m => m.ProfileID.Value).ToList();


                    List<Att_AnnualLeaveDetail> lstAnnualLeaveDetail_InDB = repoAtt_AnnualLeaveDetail
                        .FindBy(m => m.IsDelete == null && m.Year == Year && m.Type == E_SICK_LEAVE && m.ProfileID != null && lstProfileID_InDB.Contains(m.ProfileID.Value)).ToList<Att_AnnualLeaveDetail>();
                    foreach (var item in lstAnnualLeaveDetail_InDB)
                    {
                        item.IsDelete = true;
                    }
                    #endregion

                    repoAtt_AnnualLeaveDetail.Add(lstAnnualDetail);
                    try
                    {
                        repoAtt_AnnualLeaveDetail.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                    //EntityService.AddEntity<Att_AnnualLeaveDetail>(GuidContext, lstAnnualDetail.ToArray());
                    //DataErr = EntityService.SubmitChanges(GuidContext, LoginUserID);

                    //if (DataErr == DataErrorCode.Success)
                    //{
                    //    Common.MessageBoxs(Messages.Msg, Messages.SaveSuccess, MessageBox.Icon.INFO, string.Empty);
                    //}
                    //else
                    //{
                    //    Common.MessageBoxs(Messages.Msg, Messages.SaveUnSuccess, MessageBox.Icon.INFO, string.Empty);
                    //}
                }
                //else
                //{
                //    Common.MessageBoxs(Messages.Msg, Messages.NoDataToCompute, MessageBox.Icon.WARNING, string.Empty);
                //}
                result = true;
            }
            return result;
        }

        #region Tính Phép Bệnh (Cơ Chế Mới)

        /// <summary>
        /// Button Tính Phép Bệnh (Cơ Chế Mới)
        /// Lưu vào Att_AnnualDetail
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="orgStructure"></param>
        /// <param name="LstProfileStatus"></param>
        /// <returns></returns>
        public bool ComputeAnnualSick(int Year, string orgStructure, string LstProfileStatus, bool isFullEmp, string UserLogin)
        {
            var result = false;
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoCat_GradeAttendance = new CustomBaseRepository<Cat_GradeAttendance>(unitOfWork);
                var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);
                var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);
                var repoAtt_AnnualDetail = new CustomBaseRepository<Att_AnnualDetail>(unitOfWork);
                var repoAtt_RosterGroup = new CustomBaseRepository<Att_RosterGroup>(unitOfWork);
                var repoAtt_AnnualLeave = new CustomBaseRepository<Att_AnnualLeave>(unitOfWork);
                var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);
                var repoSys_AllSetting = new CustomBaseRepository<Sys_AllSetting>(unitOfWork);

                List<Guid> lstLeaveDayApproveIDs = new List<Guid>();
                List<Guid> lstLeaveDayRejectIDs = new List<Guid>();
                var shifts = repoCat_Shift.FindBy(s => s.IsDelete == null).ToList();

                List<object> lstObj = new List<object>();
                lstObj.Add(orgStructure);
                lstObj.Add(null);
                lstObj.Add(null);
                var lstProfile = GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status);

                List<Att_AnnualDetail> lstResult = new List<Att_AnnualDetail>();
                List<Guid> lstProfileID = lstProfile.Select(m => m.ID).ToList();
                int BeginMonth = MonthStartAnl;
                DateTime BeginYear = new DateTime(Year, BeginMonth, 1);
                DateTime EndYear = BeginYear.AddYears(1).AddMinutes(-1);
                DateTime BeginYearToGetData = BeginYear.AddMonths(-1); //trừ một tháng để lấy data (bao những data thuộc những ngày trước tháng dành cho chế độ lương ko phải từ ngày 1)

                var lstGradeCgf = repoCat_GradeAttendance.FindBy(s => s.ID != null).ToList();
                string E_APPROVED = LeaveDayStatus.E_APPROVED.ToString();

                var lstAnnualDetailInDB = new List<Att_AnnualDetail>();
                var lstLeaveDaySick = new List<Att_LeaveDay>();
                var lstAttGrade = new List<Att_Grade>();
                var lstAnualLeaveCfg = new List<Att_AnnualLeave>();
                var lstWorkingHistory = new List<Hre_WorkHistory>();
                var lstRoster = new List<Att_Roster>();

                #region Lấy Dữ Liệu
                List<object> lst2Param = new List<object>();
                lst2Param.Add(null);
                lst2Param.Add(EndYear);

                List<object> lst3Param = new List<object>();
                lst3Param.Add(null);
                lst3Param.Add(null);
                lst3Param.Add(null);

                List<object> lst4Param = new List<object>();
                lst4Param.Add(null);
                lst4Param.Add(BeginYearToGetData);
                lst4Param.Add(EndYear);
                lst4Param.Add(E_APPROVED);

                var dataAtt_LeaveDay = GetData<Att_LeaveDay>(lst3Param, ConstantSql.hrm_att_getdata_LeaveDay_Inner, UserLogin, ref status).ToList();
                var dataHre_WorkHistory = GetData<Hre_WorkHistory>(lst2Param, ConstantSql.hrm_hre_getdata_WorkHistory, UserLogin, ref status).ToList();
                var dataAtt_Roster_Inner = GetData<Att_Roster>(lst4Param, ConstantSql.hrm_att_getdata_Roster_Inner, UserLogin, ref status).ToList();
                #endregion

                if (isFullEmp != null && isFullEmp == true)
                {
                    lstAnnualDetailInDB = repoAtt_AnnualDetail.FindBy(m => m.IsDelete == null && m.ProfileID != null && m.Year == Year).ToList<Att_AnnualDetail>();
                    lstLeaveDaySick = dataAtt_LeaveDay
                                        .Where(m => m.Status == E_APPROVED && m.DateStart <= EndYear && m.DateEnd >= BeginYearToGetData && m.Cat_LeaveDayType != null && m.Cat_LeaveDayType.Code == LeavedayTypeCode.SICK.ToString() && lstProfileID.Contains(m.ProfileID)).ToList<Att_LeaveDay>();
                    lstAttGrade = repoAtt_Grade.FindBy(m => m.IsDelete == null).ToList();
                    lstAnualLeaveCfg = repoAtt_AnnualLeave.FindBy(m => m.IsDelete == null && m.Year == Year).ToList<Att_AnnualLeave>();
                    lstWorkingHistory = dataHre_WorkHistory.Where(m => m.DateEffective <= EndYear).OrderByDescending(m => m.DateEffective).ToList();
                    lstRoster = dataAtt_Roster_Inner.Where(m => m.Status == E_APPROVED && m.DateStart <= EndYear && m.DateEnd >= BeginYearToGetData).ToList<Att_Roster>();
                }
                else
                {
                    lstAnnualDetailInDB = repoAtt_AnnualDetail.FindBy(m => m.IsDelete == null && m.ProfileID != null && lstProfileID.Contains(m.ProfileID.Value) && m.Year == Year).ToList<Att_AnnualDetail>();
                    lstLeaveDaySick = dataAtt_LeaveDay
                                        .Where(m => m.Status == E_APPROVED && m.DateStart <= EndYear && m.DateEnd >= BeginYearToGetData && m.Cat_LeaveDayType != null && m.Cat_LeaveDayType.Code == LeavedayTypeCode.SICK.ToString() && lstProfileID.Contains(m.ProfileID)).ToList<Att_LeaveDay>();
                    lstAttGrade = repoAtt_Grade.FindBy(m => m.IsDelete == null && lstProfileID.Contains(m.ProfileID.Value)).ToList<Att_Grade>();
                    lstAnualLeaveCfg = repoAtt_AnnualLeave.FindBy(m => m.IsDelete == null && m.Year == Year && lstProfileID.Contains(m.ProfileID)).ToList<Att_AnnualLeave>();
                    lstWorkingHistory = dataHre_WorkHistory.Where(m => m.DateEffective <= EndYear && lstProfileID.Contains(m.ProfileID)).OrderByDescending(m => m.DateEffective).ToList<Hre_WorkHistory>();
                    lstRoster = dataAtt_Roster_Inner.Where(m => m.Status == E_APPROVED && m.DateStart <= EndYear && m.DateEnd >= BeginYearToGetData && lstProfileID.Contains(m.ProfileID)).ToList<Att_Roster>();
                }

                if (lstLeaveDayApproveIDs != null && lstLeaveDayApproveIDs.Count > 0)
                {
                    lstLeaveDaySick.AddRange(dataAtt_LeaveDay.Where(m => lstLeaveDayApproveIDs.Contains(m.ID)).ToList<Att_LeaveDay>());
                    lstLeaveDaySick = lstLeaveDaySick.Distinct().ToList();
                }
                if (lstLeaveDayRejectIDs != null && lstLeaveDayRejectIDs.Count > 0)
                {
                    lstLeaveDaySick = lstLeaveDaySick.Where(m => !lstLeaveDayRejectIDs.Contains(m.ID)).ToList();
                }
                var lstRosterGroup = repoAtt_RosterGroup.FindBy(m => m.IsDelete == null && m.DateStart <= EndYear && m.DateEnd >= BeginYearToGetData).ToList<Att_RosterGroup>();
                var lstDayOff = repoCat_DayOff.FindBy(m => m.IsDelete == null && m.DateOff >= BeginYearToGetData && m.DateOff <= EndYear).ToList<Cat_DayOff>();

                // Phép Năm chốt Từ Ngày 1 -> 31 Hàng Tháng 
                bool IsFrom1To31 = false;
                string type2 = AppConfig.HRM_ATT_ANNUALLEAVE_ANNUALBEGINMONTHTOENDMONTH.ToString();
                Sys_AllSetting sys_ANNUAL_BEGINMONTHTO_ENDMONTH = repoSys_AllSetting.FindBy(sy => sy.IsDelete == null && sy.Name == type2).FirstOrDefault();
                if (sys_ANNUAL_BEGINMONTHTO_ENDMONTH != null && sys_ANNUAL_BEGINMONTHTO_ENDMONTH.Value1 == bool.TrueString)
                {
                    IsFrom1To31 = true;
                }

                foreach (var profile in lstProfile)
                {
                    var lstGradeByProfile = lstAttGrade.Where(m => m.ProfileID == profile.ID).OrderByDescending(m => m.MonthStart).ToList();
                    var lstLeaveDaySickByprofile = lstLeaveDaySick.Where(m => m.ProfileID == profile.ID).ToList();
                    var AnualLeaveCfg = lstAnualLeaveCfg.Where(m => m.ProfileID == profile.ID).FirstOrDefault();
                    var lstRosterByProfile = lstRoster.Where(m => m.ProfileID == profile.ID).ToList();
                    var lstWorkHistoryByProfile = lstWorkingHistory.Where(m => m.ProfileID == profile.ID).ToList();

                    List<Att_AnnualDetail> lstAnnualDetailByProfile = AnalyzeAnnualSickPerProfile(profile, lstGradeCgf, lstGradeByProfile,
                        BeginYear, EndYear, lstLeaveDaySickByprofile, lstAnnualDetailInDB, AnualLeaveCfg, lstRosterByProfile,
                        lstRosterGroup, lstWorkHistoryByProfile, lstDayOff, IsFrom1To31, shifts);

                    //lstResult.AddRange(lstAnnualDetailByProfile);
                    repoAtt_AnnualDetail.Add(lstAnnualDetailByProfile);
                }
                repoAtt_AnnualDetail.SaveChanges();


                //DataErrorCode DataErr = DataErrorCode.Unknown;
                //if (lstAnnualDetail.Count > 0)
                //{
                //    #region lấy dữ liệu dưới DB xóa đi
                //    List<Guid> lstProfileID_InDB = lstAnnualDetail.Where(m => m.ProfileID != null).Select(m => m.ProfileID.Value).ToList();


                //    List<Att_AnnualDetail> lstAnnualLeaveDetail_InDB = repoAtt_AnnualDetail
                //        .FindBy(m => m.IsDelete == null && m.Year == Year && m.Type == E_SICK_LEAVE && m.ProfileID != null && lstProfileID_InDB.Contains(m.ProfileID.Value)).ToList<Att_AnnualDetail>();
                //    foreach (var item in lstAnnualLeaveDetail_InDB)
                //    {
                //        item.IsDelete = true;
                //    }
                //    #endregion

                //    repoAtt_AnnualDetail.Add(lstAnnualDetail);
                //    try
                //    {
                //        repoAtt_AnnualDetail.SaveChanges();
                //    }
                //    catch (Exception)
                //    {
                //        return false;
                //    }
                //}
                result = true;
            }
            return result;
        }

        private List<Att_AnnualDetail> AnalyzeAnnualSickPerProfile(Hre_ProfileEntity Profile, List<Cat_GradeAttendance> lstGradeCfg, List<Att_Grade> lstGrade,
            DateTime BeginYear, DateTime EndYear, List<Att_LeaveDay> lstLeaveSick, List<Att_AnnualDetail> lstAnnualDetailInDB, Att_AnnualLeave AnnualLeave,
            List<Att_Roster> lstRosterInYear, List<Att_RosterGroup> lstRosterGroup, List<Hre_WorkHistory> lstWorkHistory, List<Cat_DayOff> lstDayOff,
            bool IsFrom1To31, List<Cat_Shift> shifts)
        {
            List<Att_AnnualDetail> lstResult = new List<Att_AnnualDetail>();
            double leaveBeginYearToMonth = 0;
            double leaveBeginYearToMonth_Init = 0;
            string E_ROSTERGROUP = RosterType.E_ROSTERGROUP.ToString();
            List<Att_Roster> lstRoster_byProfile = lstRosterInYear.Where(m => m.ProfileID == Profile.ID && m.Type != E_ROSTERGROUP).ToList();
            List<Att_Roster> lstRosterTypeGroup = lstRosterInYear.Where(m => m.ProfileID == Profile.ID && m.Type == E_ROSTERGROUP).ToList();

            for (DateTime Month = BeginYear; Month <= EndYear; Month = Month.AddMonths(1))
            {
                var gradeByProfileByTime = lstGrade.Where(m => m.ProfileID == Profile.ID && m.MonthStart <= Month).OrderByDescending(m => m.MonthStart).FirstOrDefault();
                if (gradeByProfileByTime == null)
                    continue;
                var GradeCfg = lstGradeCfg.Where(m => m.ID == gradeByProfileByTime.GradeAttendanceID).FirstOrDefault();
                if (GradeCfg == null)
                    continue;

                DateTime BeginMonth = Month;
                DateTime EndMonth = BeginMonth.AddMonths(1).AddMinutes(-1);
                if (!IsFrom1To31)
                {
                    Att_AttendanceServices.GetRangeMaxMinGrade(new List<Cat_GradeAttendance>() { GradeCfg }, Month, out BeginMonth, out EndMonth);
                }
                double Availale = AnnualLeave == null ? 0 : AnnualLeave.InitSickValue;
                double LeaveInMonth = 0;
                List<Att_LeaveDay> lstSickInMonth = lstLeaveSick.Where(m => m.ProfileID == Profile.ID && m.DateStart <= EndMonth && m.DateEnd >= BeginMonth).ToList();

                var listRosterEntity = lstRoster_byProfile.Select(d => new Att_RosterEntity
                {
                    ID = d.ID,
                    ProfileID = d.ProfileID,
                    RosterGroupName = d.RosterGroupName,
                    Type = d.Type,
                    Status = d.Status,
                    DateEnd = d.DateEnd,
                    DateStart = d.DateStart,
                    MonShiftID = d.MonShiftID,
                    TueShiftID = d.TueShiftID,
                    WedShiftID = d.WedShiftID,
                    ThuShiftID = d.ThuShiftID,
                    FriShiftID = d.FriShiftID,
                    SatShiftID = d.SatShiftID,
                    SunShiftID = d.SunShiftID,
                    MonShift2ID = d.MonShiftID,
                    TueShift2ID = d.TueShift2ID,
                    WedShift2ID = d.WedShift2ID,
                    ThuShift2ID = d.ThuShift2ID,
                    FriShift2ID = d.FriShift2ID,
                    SatShift2ID = d.SatShift2ID,
                    SunShift2ID = d.SunShift2ID
                }).ToList();

                var listRosterGroupEntity = lstRosterGroup.Select(d => new Att_RosterGroupEntity
                {
                    ID = d.ID,
                    DateEnd = d.DateEnd,
                    DateStart = d.DateStart,
                    MonShiftID = d.MonShiftID,
                    TueShiftID = d.TueShiftID,
                    WedShiftID = d.WedShiftID,
                    ThuShiftID = d.ThuShiftID,
                    FriShiftID = d.FriShiftID,
                    SatShiftID = d.SatShiftID,
                    SunShiftID = d.SunShiftID,
                    RosterGroupName = d.RosterGroupName
                }).ToList();

                foreach (var item in lstSickInMonth)
                {
                    if (item.DateStart >= BeginMonth && item.DateEnd <= EndMonth)
                    {
                        //Chi cần lấy TotalDuration hoặc lấy giờ rồi lấy ca
                        if (item.DurationType == LeaveDayDurationType.E_FULLSHIFT.ToString())
                        {
                            LeaveInMonth += item.TotalDuration ?? 0;
                        }
                        else
                        {

                            //Lấy ra ca làm việc 
                            DateTime DateBeginLeave = item.DateStart.Date;
                            DateTime DateEndLeave = item.DateEnd.Date;

                            Dictionary<DateTime, Cat_Shift> dicShift = Att_AttendanceLib.GetDailyShifts(Profile != null ? Profile.ID : Guid.Empty, DateBeginLeave, DateEndLeave, listRosterEntity, listRosterGroupEntity, shifts);

                            if (dicShift != null && dicShift.ContainsKey(DateBeginLeave))
                            {
                                Cat_Shift shift = dicShift[DateBeginLeave];
                                if (shift != null)
                                {
                                    double HourWorkday = 8;
                                    if (shift.WorkHours != null && shift.WorkHours != 0)
                                    {
                                        HourWorkday = shift.WorkHours ?? 8;
                                    }
                                    LeaveInMonth += item.Duration / HourWorkday;
                                }
                            }
                        }
                    }
                    else
                    {
                        DateTime DateBegin = BeginMonth > item.DateStart ? BeginMonth : item.DateStart;
                        DateTime DateEnd = EndMonth < item.DateEnd ? BeginMonth : item.DateEnd;
                        Dictionary<DateTime, Cat_Shift> dicShift = Att_AttendanceLib.GetDailyShifts(Profile != null ? Profile.ID : Guid.Empty, DateBegin, DateEnd, listRosterEntity, listRosterGroupEntity, shifts);
                        for (DateTime dateCheck = DateBegin; dateCheck <= DateEnd; dateCheck = dateCheck.AddDays(1))
                        {
                            if (Att_WorkDayHelper.IsWorkDay(dateCheck, GradeCfg, dicShift, lstDayOff) && !lstDayOff.Any(m => m.DateOff == dateCheck))
                            {
                                LeaveInMonth++;
                            }
                        }
                    }
                }
                Att_AnnualDetail AnnualDetail = lstAnnualDetailInDB.Where(m => m.ProfileID == Profile.ID && m.MonthYear == Month).FirstOrDefault();
                if (AnnualDetail == null)
                {
                    AnnualDetail = new Att_AnnualDetail();
                }
                if (Profile.DateQuit != null)
                {
                    DateTime MonthQuit = Profile.DateQuit.Value.Day >= 15 ? Profile.DateQuit.Value.AddMonths(1) : Profile.DateQuit.Value;
                    MonthQuit = new DateTime(MonthQuit.Year, MonthQuit.Month, 1);
                    if (Month >= MonthQuit)
                    {
                        AnnualDetail.IsDelete = true;
                    }
                }
                AnnualDetail.Available = Availale;
                AnnualDetail.InitAvailable = AnnualLeave == null ? 0 : AnnualLeave.InitSickValue;
                AnnualDetail.ProfileID = Profile.ID;
                AnnualDetail.Year = BeginYear.Year;
                AnnualDetail.MonthYear = Month;
                AnnualDetail.MonthBeginInYear = MonthStartAnl; //todo: Phần tử này để trong cấu hình chung
                if (AnnualLeave != null && AnnualLeave.MonthResetAnlOfBeforeYear != null)
                {
                    AnnualDetail.MonthResetInitAvailable = AnnualLeave.MonthResetAnlOfBeforeYear.Value;//todo: MonthReset Này để trong chế độ lương
                }
                else
                {
                    AnnualDetail.MonthResetInitAvailable = 12;//todo: MonthReset Này để trong chế độ lương
                }
                AnnualDetail.MonthStartProfile = AnnualLeave == null ? 1 : AnnualLeave.MonthStart;
                if (AnnualDetail.MonthResetInitAvailable != null && AnnualDetail.MonthResetInitAvailable != 12 && AnnualDetail.Year != null)
                {
                    DateTime MonthReset = new DateTime(AnnualDetail.Year.Value, AnnualDetail.MonthResetInitAvailable.Value, 1);
                    AnnualDetail.IsHaveResetInitAvailable = true;
                    if (Month <= MonthReset) //Trong những tháng có
                    {
                        double delta = leaveBeginYearToMonth_Init + LeaveInMonth - AnnualDetail.InitAvailable.Value;
                        if (delta > 0)
                        {
                            AnnualDetail.TotalLeaveBefFromInitAvailable = leaveBeginYearToMonth_Init;
                            AnnualDetail.LeaveInMonthFromInitAvailable = LeaveInMonth - delta;
                            AnnualDetail.LeaveInMonth = delta;
                            AnnualDetail.TotalLeaveBef = leaveBeginYearToMonth;
                            leaveBeginYearToMonth += delta;
                            leaveBeginYearToMonth_Init += LeaveInMonth - delta;
                        }
                        else
                        {
                            AnnualDetail.TotalLeaveBefFromInitAvailable = leaveBeginYearToMonth_Init;
                            AnnualDetail.LeaveInMonthFromInitAvailable = LeaveInMonth;
                            AnnualDetail.TotalLeaveBef = leaveBeginYearToMonth;
                            AnnualDetail.LeaveInMonth = 0;
                            leaveBeginYearToMonth_Init += LeaveInMonth;
                        }
                    }
                    else //sau những tháng kho có
                    {
                        AnnualDetail.InitAvailable = 0;
                        AnnualDetail.TotalLeaveBef = leaveBeginYearToMonth;
                        AnnualDetail.LeaveInMonth = LeaveInMonth;
                        leaveBeginYearToMonth += LeaveInMonth;
                    }
                }
                else //Bình thường
                {
                    AnnualDetail.IsHaveResetInitAvailable = false;
                    AnnualDetail.TotalLeaveBef = leaveBeginYearToMonth;
                    AnnualDetail.LeaveInMonth = LeaveInMonth;
                    leaveBeginYearToMonth += LeaveInMonth;
                }
                double remain = (AnnualDetail.Available ?? 0) + (AnnualDetail.InitAvailable ?? 0) - (AnnualDetail.TotalLeaveBefFromInitAvailable ?? 0) - (AnnualDetail.LeaveInMonthFromInitAvailable ?? 0) - (AnnualDetail.TotalLeaveBef ?? 0) - (AnnualDetail.LeaveInMonth ?? 0);
                AnnualDetail.Remain = remain;
                AnnualDetail.Type = AnnualLeaveDetailType.E_SICK_LEAVE.ToString();
                lstResult.Add(AnnualDetail);
            }
            return lstResult;

        }
        #endregion

        private int? _MonthStartAnl;
        private int MonthStartAnl
        {
            get
            {
                if (_MonthStartAnl == null)
                {
                    int result = 1;
                    BaseService _baseService = new BaseService();
                    string status = string.Empty;
                    var key = AppConfig.HRM_ATT_ANNUALDETAIL_MONTHBEGININYEAR.ToString();
                    var lstConfig = _baseService.GetData<Sys_AllSettingEntity>(key, ConstantSql.hrm_sys_sp_get_AllSettingByKey, string.Empty,ref status);

                    if (lstConfig.Count > 0)
                    {
                        var temp = lstConfig.FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(temp.Value1) && !string.IsNullOrEmpty(temp.Value1))
                        {
                            int.TryParse(temp.Value1, out result);
                        }

                    }
                    _MonthStartAnl = result;
                }
                if (_MonthStartAnl == 0)
                    _MonthStartAnl = 1;
                return _MonthStartAnl ?? 1;////Todo: Thêm vào cấu hình chung tháng chỗ này cho biết là chu kỳ tháng bắt đầu tính của Công ty là tháng mấy vd: Honda (Tháng 4 đến cuối tháng 3 năm sau).
            }
        }



    }
}
