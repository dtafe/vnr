using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
using System.Linq;
using HRM.Business.Attendance.Models;
using System.Collections.Generic;
using System;
using HRM.Business.Category.Models;
using HRM.Infrastructure.Utilities;
using HRM.Business.Category.Domain;
using System.Collections;
using System.Data;
using HRM.Data.Entity;
using HRM.Data.Attendance.Data.Sql.Repositories;
using HRM.Business.Main.Domain;
using HRM.Business.Hr.Models;
using VnResource.Helper.Data;
using HRM.Business.Hr.Domain;
using HRM.Business.HrmSystem.Models;
using System.Globalization;
using VnResource.Helper.Linq;

namespace HRM.Business.Attendance.Domain
{
    public class Att_ReportServices : BaseService
    {
        private bool isTimeOfInLieu
        {
            get
            {
                string status = string.Empty;
                var key = AppConfig.HRM_ATT_LEAVEDAY_ALLOWFINALIZATIONHOLIDAYS.ToString();
                var sys_AppConfig = GetData<Sys_AllSettingEntity>(key, ConstantSql.hrm_sys_sp_get_AllSettingByKey, string.Empty, ref status).Select(s => s.Value1).FirstOrDefault();
                if (sys_AppConfig != null && sys_AppConfig.ToString() == Boolean.TrueString)
                    return true;
                return false;
            }
        }
        private int Fjk_GetBreakfastCount(List<Att_Overtime> listOvertimes, DateTime from, DateTime to, string status)
        {
            int res = 0;
            int startTimeHour = 0;
            List<Att_Overtime> listOvertime = listOvertimes.Where(ot => ot.IsDelete == null && ot.WorkDate >= from && ot.WorkDate <= to && ot.Status == status).ToList();
            foreach (Att_Overtime overtime in listOvertime)
            {
                Cat_Shift shift = overtime.Cat_Shift;
                if (shift != null && shift.Code == "02")
                {
                    startTimeHour = shift.udCoOut.Hour;
                    if (overtime.WorkDate.Hour == startTimeHour && overtime.ConfirmHours >= 2)
                        res++;
                }
            }
            return res;
        }
        string GetType(Cat_OvertimeType type)
        {
            if (type == null || type.CodeStatistic.IsNullOrEmpty())
                return string.Empty;
            return type.CodeStatistic + "-";
        }
        // vinguyen 
        bool IsMergeOvertimeHour = true;

        #region BC Tăng Ca

        #region BC Tăng Ca Hàng Tháng
        DataTable getSchemaReportMonthlyOvertime(DateTime DateStart, DateTime DateEnd)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                var repoCat_OvertimeType = new CustomBaseRepository<Cat_OvertimeType>(unitOfWork);
                DataTable tblData = new DataTable("Att_ReportMonthlyOvertimeEntity");

                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.CodeEmp, typeof(String));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.ProfileName, typeof(String));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.PositionName, typeof(String));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.JobTitleName, typeof(String));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.OrgStructure, typeof(string));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.OrgStructureCode, typeof(String));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.DeptPath, typeof(String));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.Section, typeof(String));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.DepartmentInRoster, typeof(String));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.WorkPlace, typeof(String));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.ALLOWANCE_BREAKF, typeof(String));

                int dataidx = 1;
                for (DateTime idx = DateStart; idx <= DateEnd; idx = idx.AddDays(1))
                {
                    DataColumn column = new DataColumn(Att_ReportMonthlyOvertimeEntity.FieldNames.Data + dataidx, typeof(String));
                    column.Caption = dataidx.ToString("N0");
                    tblData.Columns.Add(column);
                    //LamLe - 20121013 - Ca dem + tang ca ca dem
                    tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.DataNightShift + dataidx, typeof(String));
                    dataidx++;
                }

                List<string> lstOvertimeType = repoCat_OvertimeType.FindBy(s => s.IsDelete == null).Select(m => m.Code).ToList<string>();
                foreach (string ot in lstOvertimeType)
                {

                    string code = ot.Replace('.', '_');

                    tblData.Columns.Add(code, typeof(Double));
                }

                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.NightShift, typeof(Double));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.AdditionHours_NonWorkDay, typeof(Double));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.AdditionHours_LeaveDay, typeof(Double));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.LeaveHours, typeof(Double));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.Totals, typeof(Double));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.CumulativeOT, typeof(Double));
                tblData.Columns.Add(OvertimeReasonCode.OT_EARLY.ToString(), typeof(Double));
                tblData.Columns.Add(OvertimeReasonCode.OT_HOME.ToString(), typeof(Double));
                tblData.Columns.Add(OvertimeReasonCode.OT_PROVIDER.ToString(), typeof(Double));
                tblData.Columns.Add(OvertimeReasonCode.OT_BU.ToString(), typeof(Double));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.Department, typeof(String));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.DepartmentCode, typeof(String));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.WorkingPlace, typeof(string));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.DateFrom, typeof(DateTime));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.DateTo, typeof(DateTime));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.UserExport);
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.DateExport, typeof(DateTime));
                tblData.Columns.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.ReasonOT, typeof(string));
                return tblData;
            }
        }

        public DataTable GetReportMonthlyOvertime(List<Hre_ProfileEntity> lstProfiles, Att_ReportMonthlyOvertimeConditionEntity condition, string userExport, bool IsCreateTemplate, string UserLogin)
        {
            string status = string.Empty;
            DateTime dateStart = condition.DateFrom.Date;
            DateTime dateEnd = condition.DateTo.Date.AddDays(1).AddMinutes(-1);
            DataTable tblData = getSchemaReportMonthlyOvertime(dateStart, dateEnd);
            if (IsCreateTemplate)
            {
                return tblData.ConfigTable();
            }

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)new UnitOfWork(context);

                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_PayrollGroup = new CustomBaseRepository<Cat_PayrollGroup>(unitOfWork);
                var repoCat_Position = new CustomBaseRepository<Cat_Position>(unitOfWork);
                var repoCat_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                var repoCat_OvertimeType = new CustomBaseRepository<Cat_OvertimeType>(unitOfWork);
                var repoCat_OvertimeReson = new CustomBaseRepository<Cat_OvertimeReson>(unitOfWork);
                var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);
                var repoCat_GradeAttendance = new CustomBaseRepository<Cat_GradeAttendance>(unitOfWork);
                var repoCat_UsualAllowance = new CustomBaseRepository<Cat_UsualAllowance>(unitOfWork);
                var repoCat_ShiftItem = new CustomBaseRepository<Cat_ShiftItem>(unitOfWork);
                var repoSal_GradeAllowance = new CustomBaseRepository<Sal_GradeAllowance>(unitOfWork);
                var repoIns_InsuranceRecord = new CustomBaseRepository<Ins_InsuranceRecord>(unitOfWork);
                var repoAtt_AttendanceTable = new CustomBaseRepository<Att_AttendanceTable>(unitOfWork);
                var repoAtt_Pregnancy = new CustomBaseRepository<Att_Pregnancy>(unitOfWork);

                List<Cat_OrgStructure> lstOrg = new List<Cat_OrgStructure>();
                //if (condition.OrgStructureID != null) // Chỉ lấy phòng ban theo sự lựa chọn
                //{
                //    List<int> lstOrderNumber = condition.OrgStructureID.Split(',').Distinct().Select(s => int.Parse(s)).ToList();
                //    lstOrg = repoCat_OrgStructure.FindBy(m => m.IsDelete == null && lstOrderNumber.Contains(m.OrderNumber)).ToList();
                //}
                //else //Lấy hêt phòng ban
                //{ 
                lstOrg = repoCat_OrgStructure.FindBy(m => m.Code != null).ToList();
                //}

                List<Cat_PayrollGroup> lstPayrollGroup = new List<Cat_PayrollGroup>();
                if (condition.PayrollGroup != null) // Chỉ lấy phòng ban theo sự lựa chọn
                {
                    lstPayrollGroup = repoCat_PayrollGroup.FindBy(m => condition.PayrollGroup.Contains(m.ID)).ToList();
                }
                //else //Lấy hêt Nhóm lương
                //{
                //    lstPayrollGroup = repoCat_PayrollGroup.FindBy(m => m.IsDelete == null).ToList();
                //}
                var lstPosition = repoCat_Position.FindBy(m => m.IsDelete == null).Select(m => new { m.ID, m.PositionName }).ToList();
                var lstJobtitle = repoCat_JobTitle.FindBy(m => m.IsDelete == null).Select(m => new { m.ID, m.JobTitleName }).ToList();
                List<Hre_Profile> listProfile = GetListProfile(lstProfiles, dateStart, dateEnd, condition.StatusEmployee, lstOrg, lstPayrollGroup, UserLogin); //Hàm GetListProfile đã xử lý lấy dữ liệu của profile lên dựa theo Status, Org, PayrollGroup
                List<Guid> lstProfileID = listProfile.Select(pr => pr.ID).ToList();
                if (lstProfileID.Count == 0)
                {
                    return tblData.ConfigTable();
                }
                bool _isCompensation = isTimeOfInLieu; //Load từ config value33 (Cho phép quyết toán ngày nghỉ) 
                DateTime CumulativeDate = new DateTime(condition.DateTo.Year, 1, 1);//Lấy từ đầu năm
                CumulativeDate = CumulativeDate.AddDays(-15); //Lấy trước 15 ngày để trường hợp lương kỳ của công ty từ và đến
                string statusCum = condition.OvertimeStatus; //Lấy loại duyệt hay yêu cầu hay xác nhận
                string statusHourCum = condition.TypeHour; //Lấy theo giờ duyệt hay yêu cầu hay xác nhận
                List<Att_Overtime> lstOvertimeAll = new List<Att_Overtime>(); //Lấy OverTime Trong khoảng thời gian search
                var lstCumOvertimeAll = new List<Att_Overtime>()
                 .Select(m => new { m.ID, m.ProfileID, m.OvertimeTypeID, m.WorkDate, m.ApproveHours, m.RegisterHours, m.ConfirmHours }).ToList();// dành cho lũy kế
                List<Guid> lstOvertimeTypeIDs = new List<Guid>();

                List<object> lstParamOT = new List<object>();
                lstParamOT.Add(condition.OrgStructureID);
                lstParamOT.Add(CumulativeDate);
                lstParamOT.Add(dateEnd);
                var dataOvertimeAll = GetData<Att_OvertimeEntity>(lstParamOT, ConstantSql.hrm_att_getdata_Overtime, UserLogin, ref status).Translate<Att_Overtime>();

                if (statusCum == null || statusCum == string.Empty)
                {
                    lstOvertimeAll = dataOvertimeAll.Where(ot => ot.WorkDate >= dateStart && ot.WorkDate <= dateEnd && lstProfileID.Contains(ot.ProfileID)).ToList();
                }
                else
                {
                    lstOvertimeAll = dataOvertimeAll.Where(ot => ot.WorkDate >= dateStart && ot.WorkDate <= dateEnd && ot.Status == statusCum && lstProfileID.Contains(ot.ProfileID)).ToList();
                }
                if (condition.Cumulative == true)//nếu tính lũy kế
                {
                    if (statusCum == string.Empty)
                    {
                        lstCumOvertimeAll = dataOvertimeAll.Where(ot => ot.WorkDate >= CumulativeDate && ot.WorkDate <= dateEnd && lstProfileID.Contains(ot.ProfileID))
                          .Select(m => new { m.ID, m.ProfileID, m.OvertimeTypeID, m.WorkDate, m.ApproveHours, m.RegisterHours, m.ConfirmHours })
                          .ToList();
                    }
                    else
                    {
                        lstCumOvertimeAll = dataOvertimeAll.Where(ot => ot.WorkDate >= CumulativeDate && ot.WorkDate <= dateEnd && ot.Status == statusCum
                            && lstProfileID.Contains(ot.ProfileID))
                         .Select(m => new { m.ID, m.ProfileID, m.OvertimeTypeID, m.WorkDate, m.ApproveHours, m.RegisterHours, m.ConfirmHours })
                         .ToList();
                    }
                    lstOvertimeTypeIDs = condition.CumulativeType;
                    if (lstOvertimeTypeIDs.Contains(Guid.Empty)) // nếu như chọn tất cả thì lấy toàn bộ danh sách các ID của loại tăng ca
                    {
                        lstOvertimeTypeIDs = repoCat_OvertimeType.FindBy(s => s.IsDelete == null).Select(m => m.ID).ToList<Guid>();
                    }
                }
                List<Att_Overtime> lstOvertime = lstOvertimeAll;
                //Logic mới. là bỏ đi loại tăng ca la Làm bù 100% của Thúy Fujikura 20130202 09:00
                //trietmai 20130202
                if (condition.RemoveCompensation100 == true)
                {
                    string E_COMPENSATION = OverTimeType.E_COMPENSATION.ToString();
                    List<Guid> lstOvertimeTypeCompensation = repoCat_OvertimeType.FindBy(m => m.IsDelete == null && m.Code == E_COMPENSATION).Select(m => m.ID).ToList<Guid>();
                    lstOvertime = lstOvertime.Where(m => m.OvertimeTypeID != null && !lstOvertimeTypeCompensation.Contains(m.OvertimeTypeID)).ToList();
                }
                string OT_ELSE = OvertimeReasonCode.OT_ELSE.ToString();
                string OT_HOME = OvertimeReasonCode.OT_HOME.ToString();
                string OT_BU = OvertimeReasonCode.OT_BU.ToString();
                string OT_PROVIDER = OvertimeReasonCode.OT_PROVIDER.ToString();
                var lstOvertimeReasonAll = repoCat_OvertimeReson.FindBy(m => m.IsDelete == null).Select(m => new { m.ID, m.Code }).ToList();
                List<Guid> lstOvertimeReasonTypeElse = lstOvertimeReasonAll.Where(m => m.Code == OT_ELSE).Select(m => m.ID).ToList<Guid>();
                List<Guid> lstOvertimeReasonTypeHome = lstOvertimeReasonAll.Where(m => m.Code == OT_HOME).Select(m => m.ID).ToList<Guid>();
                List<Guid> lstOvertimeReasonTypeBU = lstOvertimeReasonAll.Where(m => m.Code == OT_BU).Select(m => m.ID).ToList<Guid>();
                List<Guid> lstOvertimeReasonTypeProvider = lstOvertimeReasonAll.Where(m => m.Code == OT_PROVIDER).Select(m => m.ID).ToList<Guid>();
                //lấy ra DS OT loại bỏ phần OT với lý do là KHÁC
                List<Att_Overtime> lstOvertimeReason = lstOvertime.Where(p => p.OvertimeResonID != null && !lstOvertimeReasonTypeElse.Contains(p.OvertimeResonID ?? Guid.Empty)).ToList();
                List<Cat_OvertimeType> lstOvertimeType = repoCat_OvertimeType.FindBy(m => m.IsDelete == null).ToList();
                List<Guid> lstOvertimeTypeNightShiftIDs = lstOvertimeType.Where(m => m.Code == OverTimeType.E_WORKDAY_NIGHTSHIFT.ToString()
                    || m.Code == OverTimeType.E_WEEKEND_NIGHTSHIFT.ToString()
                    || m.Code == OverTimeType.E_HOLIDAY_NIGHTSHIFT.ToString()).Select(m => m.ID).ToList();
                List<Guid> lstOvertimeTypeHolidayAndHolidateNightIDs = lstOvertimeType.Where(m =>
                     m.Code == OverTimeType.E_HOLIDAY.ToString()
                    || m.Code == OverTimeType.E_HOLIDAY_NIGHTSHIFT.ToString()).Select(m => m.ID).ToList();

                List<Guid> lstOvertimeTypeRate3IDs = lstOvertimeType.Where(m => m.Rate == 3).Select(m => m.ID).ToList();


                List<Att_Grade> lstGrade = Att_GradeServices.getAllGrade(lstProfileID, dateEnd);
                List<Guid> lstUsualAllowanceBreakf = repoCat_UsualAllowance.FindBy(m => m.Code == "BREAKF").Select(m => m.ID).ToList<Guid>();
                List<Guid> lstGradeIDs = lstGrade.Select(m => m.ID).ToList();
                List<Sal_GradeAllowance> lstGradeAllowanceTotal = repoSal_GradeAllowance
                    .FindBy(m => lstGradeIDs.Contains(m.GradeID) && lstUsualAllowanceBreakf.Contains(m.UsualAllowanceID))
                    .ToList<Sal_GradeAllowance>();
                List<Cat_GradeAttendance> lstGradeCfg = repoCat_GradeAttendance.FindBy(s => s.GradeAttendanceName != null).ToList();
                List<Cat_DayOff> lstHoliday = repoCat_DayOff.FindBy(m => m.IsDelete == null && m.DateOff >= dateStart && m.DateOff <= dateEnd).ToList();
                var lstAllIns = repoIns_InsuranceRecord.FindBy(m => m.DateStart <= dateEnd && m.DateEnd >= dateStart && lstProfileID.Contains(m.ProfileID))
                    .Select(m => new { m.ID, m.ProfileID, m.DateStart, m.DateEnd }).ToList();
                string e_WEEKEND_HLD = HolidayType.E_WEEKEND_HLD.ToString();

                //var repoAtt_Workday = new CustomBaseRepository<Att_Workday>(unitOfWork);
                //List<Att_Workday> lstInOut = repoAtt_Workday.FindBy(inout => inout.WorkDate >= dateStart
                //                                                    && inout.WorkDate <= dateEnd
                //                                                    && lstProfileID.Contains(inout.ProfileID)).ToList();

                List<object> lstParamWD = new List<object>();
                lstParamWD.Add(condition.OrgStructureID);
                lstParamWD.Add(dateStart);
                lstParamWD.Add(dateEnd);
                List<Att_Workday> lstInOut = GetData<Att_Workday>(lstParamWD, ConstantSql.hrm_att_getdata_Workday, UserLogin, ref status)
                                                .Where(inout => lstProfileID.Contains(inout.ProfileID))
                                                .ToList();
                List<Cat_ShiftItem> lstAllShiftItems = repoCat_ShiftItem.FindBy(s => s.IsDelete == null).ToList();
                //Lâm Lê : Lỗi xuất line Lấy theo hiện tại lớn hơn dateEnd
                //List<Att_Roster> lstRoster = EntityService.CreateQueryable<Att_Roster>(GuidContext, LoginUserID,
                //    roster => roster.DateStart <= dateEnd && roster.DateEnd >= dateStart && lstProfileID.Contains(roster.ProfileID)).ToList<Att_Roster>();
                List<object> lstParamRT = new List<object>();
                lstParamRT.Add(condition.OrgStructureID);
                lstParamRT.Add(null);
                lstParamRT.Add(null);
                lstParamRT.Add(null);
                List<Att_Roster> lstRoster = GetData<Att_Roster>(lstParamRT, ConstantSql.hrm_att_getdata_Roster, UserLogin, ref status)
                                                .Where(roster => roster.DateStart <= dateEnd && roster.DateEnd >= dateStart && lstProfileID.Contains(roster.ProfileID))
                                                .ToList();

                List<Guid> lstOrgIDs = repoCat_OrgStructure.FindBy(s => s.IsDelete == null).Select(m => m.ID).ToList<Guid>();
                List<object> lstParamWH = new List<object>();
                lstParamWH.Add(condition.OrgStructureID);
                lstParamWH.Add(dateEnd);
                List<Hre_WorkHistory> lstWHistory = GetData<Hre_WorkHistory>(lstParamWH, ConstantSql.hrm_hre_getdata_WorkHistory, UserLogin, ref status)
                    .Where(m => lstProfileID.Contains(m.ProfileID)
                    && lstOrgIDs.Contains(m.OrganizationStructureID ?? Guid.Empty)).ToList();

                if (condition.RemoveNotHasWorkday == true)
                {
                    int month = dateEnd.Month;
                    int year = dateEnd.Year;
                    List<Guid> guids = repoAtt_AttendanceTable
                        .FindBy(s => s.IsDelete == null && s.MonthYear.Value.Year == year && s.MonthYear.Value.Month == month && s.RealWorkDayCount > 0)
                        .Select(s => s.ProfileID).ToList<Guid>();
                    listProfile = listProfile.Where(s => guids.Contains(s.ID)).ToList();
                }
                String typePre = PregnancyStatus.E_LEAVE_EARLY.ToString();
                List<Att_Pregnancy> lstPregnacyAll = repoAtt_Pregnancy
                    .FindBy(p => p.IsDelete == null && p.DateStart <= dateEnd && p.DateEnd >= dateStart && p.Type == typePre)
                    .ToList<Att_Pregnancy>();

                //string type2 = AppConfig.E_RANGE_SALARY_MONTH.ToString();
                //Sys_AppConfig appConfig = EntityService.CreateQueryable<Sys_AppConfig>(false, EntityService.GuidMainContext, LoginUserID, sy => sy.Info == type2).FirstOrDefault();
                List<Guid> lstProfileIDs = listProfile.Select(m => m.ID).ToList();
                List<Att_Roster> lstRosterTypeGroup = new List<Att_Roster>();
                List<Att_RosterGroup> lstRosterGroup = new List<Att_RosterGroup>();
                Att_RosterServices.GetRosterGroup(lstProfileIDs, dateStart, dateEnd, out lstRosterTypeGroup, out lstRosterGroup);

                foreach (Hre_Profile profile in listProfile)
                {
                    //Tinh WordDay de tinh ca dem va gio lam viec.
                    List<Att_Workday> lstProfileInOut = lstInOut.Where(inout => inout.ProfileID == profile.ID)
                                                                      .OrderBy(inout => inout.WorkDate)
                                                                      .ThenBy(inout => inout.InTime1)
                                                                      .ToList();
                    List<Att_Roster> lstProfileRoster = lstRoster.Where(rt => rt.ProfileID == profile.ID).ToList();
                    Double LeaveHours = 0.0;
                    List<Att_Overtime> lstProOvertime = new List<Att_Overtime>();
                    Att_Grade grade = lstGrade.Where(gd => gd.ProfileID == profile.ID).FirstOrDefault();
                    //LamLe - 20121024 - Kiem tra grade bi null thi thong bao len
                    if (grade == null)
                    {

                        DataRow dr1 = tblData.NewRow();
                        dr1[Att_ReportMonthlyOvertimeEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                        dr1[Att_ReportMonthlyOvertimeEntity.FieldNames.ProfileName] = profile.ProfileName;
                        dr1[Att_ReportMonthlyOvertimeEntity.FieldNames.WorkPlace] = profile.Cat_WorkPlace;
                        //dr1[Att_ReportMonthlyOvertimeEntity.FieldNames.Department] = LanguageManager.GetString(Att_ReportMonthlyOvertimeEntity.FieldNames.GradeIsNull);
                        dr1[Att_ReportMonthlyOvertimeEntity.FieldNames.DepartmentInRoster] = Att_AttendanceServices.GetLineInRoster(profile, lstRoster, dateStart, dateEnd);
                        dr1[Att_ReportMonthlyOvertimeEntity.FieldNames.DateFrom] = dateStart;
                        dr1[Att_ReportMonthlyOvertimeEntity.FieldNames.DateTo] = dateEnd;
                        dr1[Att_ReportMonthlyOvertimeEntity.FieldNames.UserExport] = userExport;
                        dr1[Att_ReportMonthlyOvertimeEntity.FieldNames.DateExport] = DateTime.Now;
                        tblData.Rows.Add(dr1);
                        continue;
                    }

                    Cat_GradeAttendance gradeCfg = lstGradeCfg.Where(m => m.ID == grade.GradeAttendanceID).FirstOrDefault();
                    Double hourWorkDate = gradeCfg.HourOnWorkDate.HasValue ? gradeCfg.HourOnWorkDate.Value : 0;
                    List<WorkDay> lstWorkday = new List<WorkDay>();
                    if (gradeCfg.IsReceiveOvertimeBonus.Value)
                    {
                        lstProOvertime = lstOvertime.Where(ot => ot.ProfileID == profile.ID).ToList();
                    }

                    List<Att_Pregnancy> lstPregnacyPro = lstPregnacyAll.Where(p => p.ProfileID == profile.ID).ToList();
                    lstWorkday = Att_WorkDayHelper.ComputeWorkDays(profile, lstProfileInOut, lstAllShiftItems, lstProfileRoster, lstPregnacyPro, gradeCfg, UserLogin);
                    List<Att_Overtime> lstProOvertimeReason = lstOvertimeReason.Where(ot => ot.ProfileID == profile.ID).ToList();
                    List<Sal_GradeAllowance> lstAlwTemp = lstGradeAllowanceTotal.Where(m => m.GradeID == grade.ID).ToList();
                    Sal_GradeAllowance alw = lstAlwTemp.Count() > 0 ? lstAlwTemp.First() : null;
                    double valueALLOWANCE_BREAKF = 0;
                    if (alw != null)
                    {
                        int breakfastCount = Fjk_GetBreakfastCount(lstProOvertime, dateStart, dateEnd, statusCum);
                        valueALLOWANCE_BREAKF = breakfastCount * alw.Cat_UsualAllowanceLevel.Amount;
                    }
                    Hre_WorkHistory wHistoryNow = lstWHistory.Where(wh => wh.ProfileID == profile.ID && wh.DateEffective < dateEnd).OrderByDescending(wh => wh.DateEffective).FirstOrDefault();
                    DataRow dr = tblData.NewRow();
                    dr[Att_ReportMonthlyOvertimeEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    dr[Att_ReportMonthlyOvertimeEntity.FieldNames.ProfileName] = profile.ProfileName;
                    dr[Att_ReportMonthlyOvertimeEntity.FieldNames.WorkPlace] = profile.Cat_WorkPlace;
                    string Position = string.Empty;
                    string JobTitle = string.Empty;
                    string Department = string.Empty;
                    string DeptPath = string.Empty;
                    if (wHistoryNow != null)
                    {
                        //position
                        if (wHistoryNow.PositionID != null)
                        {
                            var PositionC = lstPosition.Where(m => m.ID == wHistoryNow.PositionID).FirstOrDefault();
                            Position = PositionC != null ? PositionC.PositionName : string.Empty;
                        }
                        else
                        {
                            if (profile.PositionID != null)
                            {
                                var PositionC = lstPosition.Where(m => m.ID == profile.PositionID).FirstOrDefault();
                                Position = PositionC != null ? PositionC.PositionName : string.Empty;
                            }
                            else
                            {
                                Position = string.Empty;
                            }
                        }

                        //Jobtitle
                        if (wHistoryNow.JobTitleID != null)
                        {
                            var JobTitleC = lstJobtitle.Where(m => m.ID == wHistoryNow.JobTitleID).FirstOrDefault();
                            JobTitle = JobTitleC != null ? JobTitleC.JobTitleName : string.Empty;
                        }
                        else
                        {
                            if (profile.JobTitleID != null)
                            {
                                var JobTitleC = lstJobtitle.Where(m => m.ID == profile.JobTitleID).FirstOrDefault();
                                JobTitle = JobTitleC != null ? JobTitleC.JobTitleName : string.Empty;
                            }
                            else
                            {
                                JobTitle = string.Empty;
                            }
                        }

                        //Phong Ban
                        if (wHistoryNow.OrganizationStructureID != null)
                        {
                            var OrgC = lstOrg.Where(m => m.ID == wHistoryNow.OrganizationStructureID).FirstOrDefault();
                            Department = OrgC != null ? OrgC.OrgStructureName : string.Empty;
                        }
                        else
                        {
                            if (profile.OrgStructureID != null)
                            {
                                var OrgC = lstOrg.Where(m => m.ID == profile.OrgStructureID).FirstOrDefault();
                                Department = OrgC != null ? OrgC.OrgStructureName : string.Empty;
                            }
                            else
                            {
                                Department = string.Empty;
                            }
                        }
                        //DeptPath = wHistoryNow.udLinkDepartmentCode;
                    }
                    else
                    {
                        //position
                        if (profile.PositionID != null)
                        {
                            var PositionC = lstPosition.Where(m => m.ID == profile.PositionID).FirstOrDefault();
                            Position = PositionC != null ? PositionC.PositionName : string.Empty;
                        }
                        else
                        {
                            Position = string.Empty;
                        }
                        //Jobtitle

                        if (profile.JobTitleID != null)
                        {
                            var JobTitleC = lstJobtitle.Where(m => m.ID == profile.JobTitleID).FirstOrDefault();
                            JobTitle = JobTitleC != null ? JobTitleC.JobTitleName : string.Empty;
                        }
                        else
                        {
                            JobTitle = string.Empty;
                        }
                        //Phong Ban

                        if (profile.OrgStructureID != null)
                        {
                            var OrgC = lstOrg.Where(m => m.ID == profile.OrgStructureID).FirstOrDefault();
                            Department = OrgC != null ? OrgC.OrgStructureName : string.Empty;
                        }
                        else
                        {
                            Department = string.Empty;
                        }
                        //DeptPath = profile.udLinkDepartmentCode;
                    }

                    dr[Att_ReportMonthlyOvertimeEntity.FieldNames.PositionName] = Position;
                    dr[Att_ReportMonthlyOvertimeEntity.FieldNames.JobTitleName] = JobTitle;
                    dr[Att_ReportMonthlyOvertimeEntity.FieldNames.Department] = Department;
                    dr[Att_ReportMonthlyOvertimeEntity.FieldNames.DeptPath] = DeptPath;
                    dr[Att_ReportMonthlyOvertimeEntity.FieldNames.WorkingPlace] = profile.WorkingPlace;
                    dr[Att_ReportMonthlyOvertimeEntity.FieldNames.Section] = "";
                    dr[Att_ReportMonthlyOvertimeEntity.FieldNames.DepartmentInRoster] = Att_AttendanceServices.GetLineInRoster(profile, lstRoster, dateStart, dateEnd);
                    dr[Att_ReportMonthlyOvertimeEntity.FieldNames.ALLOWANCE_BREAKF] = valueALLOWANCE_BREAKF;
                    List<Hre_WorkHistory> lstWHistoryPro = lstWHistory.Where(wh => wh.ProfileID == profile.ID).ToList();
                    Hashtable htRoster = Att_RosterServices.GetRosterTable(profile, lstProfileRoster, lstWHistoryPro, gradeCfg, dateStart, dateEnd, lstRosterGroup, lstRosterTypeGroup);
                    DateTime adjDate = DateTime.Now;
                    int dataidx = 1;
                    double hourNightShift = 0;
                    for (DateTime dateStartCheck = dateStart; dateStartCheck <= dateEnd; dateStartCheck = dateStartCheck.AddDays(1))
                    {

                        //Tinh ca dem
                        double hourNightShiftDay = 0;
                        WorkDay workday = lstWorkday.Where(wd => wd.WorkDate == dateStartCheck).FirstOrDefault();
                        if (workday != null && Att_AttendanceServices.IsWorkDay(gradeCfg, htRoster, lstHoliday, dateStartCheck))
                        {
                            hourNightShiftDay = workday.NightShiftDuration;
                            hourNightShift += hourNightShiftDay;
                        }
                        if (condition.RecordIns == true)
                        {
                            var lstIns = lstAllIns.Where(ins => dateStartCheck >= ins.DateStart && dateStartCheck <= ins.DateEnd && ins.ProfileID == profile.ID).ToList();
                            if (lstIns.Count >= 1)
                            {
                                dataidx++;
                                continue;
                            }
                        }

                        if (lstProOvertime.Count > 0)
                        {
                            List<Att_Overtime> lstDateOt = new List<Att_Overtime>();
                            lstDateOt = lstProOvertime.Where(s => dateStartCheck.Date == (s.WorkDateRoot != null ? s.WorkDateRoot.Value.Date : s.WorkDate.Date)
                                && s.OvertimeTypeID != null).ToList();
                            //LamLe - 20121013 - danh sach OT tang ca ca dem. de tinh so gio tang ca ca dem cho tung ngay
                            List<Att_Overtime> lstDateNightOt = lstDateOt.Where(ot => lstOvertimeTypeNightShiftIDs.Contains(ot.OvertimeTypeID)).ToList();
                            Double otNightValue = Math.Round(lstDateNightOt.Sum(ot => ot.ApproveHours.Value), 2);
                            hourNightShiftDay += otNightValue;
                            String showStr = "";
                            Double storedValue = 0;
                            if (IsMergeOvertimeHour)
                            {
                                if (condition.TypeHour == OverTimeStatus.E_CONFIRM.ToString())//"ConfirmHours")
                                    storedValue = Math.Round(lstDateOt.Sum(ot => ot.ConfirmHours), 2);
                                if (condition.TypeHour == OverTimeStatus.E_APPROVED.ToString())//"ApproveHours")
                                    storedValue = Math.Round(lstDateOt.Sum(ot => ot.ApproveHours == null ? 0 : ot.ApproveHours.Value), 2);
                                if (condition.TypeHour == OverTimeStatus.E_SUBMIT.ToString())//"RegisterHours")
                                    storedValue = Math.Round(lstDateOt.Sum(ot => ot.RegisterHours), 2);
                                if (storedValue > 0)
                                    showStr = storedValue.ToString();
                                #region tan.do 20120123 fixed load overtime code type follow code static customize
                                if (lstDateOt.Count > 1 && condition.OvertimeDetail == true)
                                {
                                    showStr = string.Empty;
                                    int i = 0;
                                    foreach (Att_Overtime attOvertime in lstDateOt)
                                    {
                                        Cat_OvertimeType OvertimeType = lstOvertimeType.Where(m => m.ID == attOvertime.OvertimeTypeID).FirstOrDefault();
                                        if (condition.TypeHour == OverTimeStatus.E_CONFIRM.ToString())
                                        {
                                            if (i == 0)
                                            {
                                                showStr += GetType(OvertimeType) + attOvertime.ConfirmHours;
                                            }
                                            else
                                            {
                                                showStr += "|" + GetType(OvertimeType) + attOvertime.ConfirmHours;
                                            }
                                            i++;
                                        }
                                        if (condition.TypeHour == OverTimeStatus.E_APPROVED.ToString())
                                        {
                                            if (i == 0)
                                            {
                                                showStr += GetType(OvertimeType) + attOvertime.ApproveHours;
                                            }
                                            else
                                            {
                                                showStr += "|" + GetType(OvertimeType) + attOvertime.ApproveHours;
                                            }
                                            i++;
                                        }
                                        if (condition.TypeHour == OverTimeStatus.E_SUBMIT.ToString())
                                        {
                                            if (i == 0)
                                            {
                                                showStr += GetType(OvertimeType) + attOvertime.RegisterHours;
                                            }
                                            else
                                            {
                                                showStr += "|" + GetType(OvertimeType) + attOvertime.RegisterHours;
                                            }
                                            i++;
                                        }
                                    }
                                }
                                #endregion
                                if (storedValue >= 2.0)
                                {
                                    // TMI 1 ngay tang ca hon 2 tieng , co 0.5 tieng nghi.
                                    // Chu nhat tang ca >10tieng , co 0.5 tieng nghi 
                                    List<Att_Overtime> lstDateOtS = lstDateOt.Where(ot => ot.WorkDate.Date == dateStartCheck.Date && lstOvertimeTypeRate3IDs.Contains(ot.OvertimeTypeID)).ToList();
                                    if (lstDateOtS.Count > 1)
                                    {
                                        if (storedValue - 8 >= 2)
                                        {
                                            LeaveHours += 0.5;
                                        }
                                    }
                                    else
                                    {
                                        LeaveHours += 0.5;
                                    }
                                }
                            }
                            else
                            {
                                foreach (Att_Overtime subot in lstDateOt)
                                {
                                    if (condition.TypeHour == OverTimeStatus.E_CONFIRM.ToString())//"ConfirmHours")
                                    {
                                        if (subot.ConfirmHours > 0)
                                        {
                                            showStr += subot.ConfirmHours + "|";
                                        }
                                    }
                                    if (condition.TypeHour == OverTimeStatus.E_APPROVED.ToString())//"ApproveHours")
                                    {
                                        if (subot.ApproveHours.Value > 0)
                                        {
                                            showStr += subot.ApproveHours.Value + "|";
                                        }
                                    }
                                    if (condition.TypeHour == OverTimeStatus.E_SUBMIT.ToString())//"RegisterHours")
                                    {
                                        if (subot.RegisterHours > 0)
                                        {
                                            showStr += subot.RegisterHours + "|";
                                        }
                                    }
                                }
                                if (showStr.Length > 0)
                                    showStr = showStr.Remove(showStr.Length - 1, 1);
                            }
                            List<Att_Overtime> lstDateOtHoliday = lstDateOt.Where(ot => ot.WorkDate.Date == dateStartCheck.Date && (lstOvertimeTypeHolidayAndHolidateNightIDs.Contains(ot.OvertimeTypeID))).ToList();
                            //xu ly chuoi hien thi ra timesheet
                            dr[Att_ReportMonthlyOvertimeEntity.FieldNames.Data + dataidx] = showStr;

                            if (storedValue > hourWorkDate
                                && hourWorkDate > 0 && gradeCfg != null && lstDateOtHoliday.Count() <= 0)
                            {
                                if (dateStartCheck.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    Double temp = 0;
                                    if (dr[Att_ReportMonthlyOvertimeEntity.FieldNames.AdditionHours_LeaveDay] == DBNull.Value)
                                        dr[Att_ReportMonthlyOvertimeEntity.FieldNames.AdditionHours_LeaveDay] = 0;

                                    temp = Convert.ToDouble(dr[Att_ReportMonthlyOvertimeEntity.FieldNames.AdditionHours_LeaveDay].ToString())
                                        + (storedValue - hourWorkDate);

                                    dr[Att_ReportMonthlyOvertimeEntity.FieldNames.AdditionHours_LeaveDay] = temp;
                                }
                                //Trường hợp T7 là ngày lễ thì không được tính ở đây
                                else if (lstHoliday.Exists(lo => lo.DateOff.Date == dateStartCheck.Date
                                        && lo.Type == e_WEEKEND_HLD))
                                {
                                    Double temp = 0;
                                    if (dr[Att_ReportMonthlyOvertimeEntity.FieldNames.AdditionHours_NonWorkDay] == DBNull.Value)
                                        dr[Att_ReportMonthlyOvertimeEntity.FieldNames.AdditionHours_NonWorkDay] = 0;

                                    temp = Convert.ToDouble(dr[Att_ReportMonthlyOvertimeEntity.FieldNames.AdditionHours_NonWorkDay].ToString())
                                        + (storedValue - hourWorkDate);

                                    dr[Att_ReportMonthlyOvertimeEntity.FieldNames.AdditionHours_NonWorkDay] = temp;
                                }
                            }

                            dr[Att_ReportMonthlyOvertimeEntity.FieldNames.LeaveHours] = LeaveHours;
                        }
                        dr[Att_ReportMonthlyOvertimeEntity.FieldNames.DataNightShift + dataidx] = hourNightShiftDay;
                        dataidx++;
                    }
                    dr[Att_ReportMonthlyOvertimeEntity.FieldNames.NightShift] = hourNightShift;
                    foreach (Cat_OvertimeType otType in lstOvertimeType)
                    {
                        Cat_OvertimeType type = otType;
                        Double val = 0.0;
                        if (condition.TypeHour == OverTimeStatus.E_CONFIRM.ToString())//"ConfirmHours")
                            val = lstProOvertime.Where(ot => ot.OvertimeTypeID == type.ID)
                                .Sum(ot => ot.ConfirmHours);
                        if (condition.TypeHour == OverTimeStatus.E_APPROVED.ToString())//"ApproveHours")
                            val = lstProOvertime.Where(ot => ot.OvertimeTypeID == type.ID)
                                .Sum(ot => ot.ApproveHours == null ? 0 : ot.ApproveHours.Value);
                        if (condition.TypeHour == OverTimeStatus.E_SUBMIT.ToString())//"RegisterHours")
                            val = lstProOvertime.Where(ot => ot.OvertimeTypeID == type.ID)
                                .Sum(ot => ot.RegisterHours);

                        string code = otType.Code.Replace('.', '_');
                        dr[code] = Math.Round(val, 2);
                    }

                    //Lay ca 2 cot la OT_EARLY va cot E_OT_BREAK Theo Yeu cau FUjikura ngay 25/04/2013 0018746 Email ngay 25/04/2013 gui boi Lam.Le
                    var OtType = lstProOvertime.Where(m => m.WorkDate >= dateStart && m.WorkDate <= dateEnd
                                                   && (m.DurationType == EnumDropDown.OvertimeDurationType.E_OT_EARLY.ToString() || m.DurationType == EnumDropDown.OvertimeDurationType.E_OT_BREAK.ToString())).ToList();

                    double OTEarly = 0.0;

                    if (condition.TypeHour == OverTimeStatus.E_CONFIRM.ToString())//"ConfirmHours")
                    {
                        dr[Att_ReportMonthlyOvertimeEntity.FieldNames.Totals] = Math.Round(lstProOvertime.Sum(ot => ot.ConfirmHours), 2);
                        OTEarly = OtType.Count() > 0 ? OtType.Sum(p => p.ConfirmHours == null ? 0 : p.ConfirmHours) : 0;
                    }
                    if (condition.TypeHour == OverTimeStatus.E_SUBMIT.ToString())//"RegisterHours")
                    {
                        dr[Att_ReportMonthlyOvertimeEntity.FieldNames.Totals] = Math.Round(lstProOvertime.Sum(ot => ot.RegisterHours), 2);
                        OTEarly = OtType.Count() > 0 ? OtType.Sum(p => p.RegisterHours == null ? 0 : p.RegisterHours) : 0;
                    }
                    if (condition.TypeHour == OverTimeStatus.E_APPROVED.ToString())//"ApproveHours")
                    {
                        dr[Att_ReportMonthlyOvertimeEntity.FieldNames.Totals] = Math.Round(lstProOvertime.Sum(ot => ot.ApproveHours == null ? 0 : ot.ApproveHours.Value), 2);
                        OTEarly = OtType.Count() > 0 ? OtType.Sum(p => p.ApproveHours == null ? 0 : p.ApproveHours.Value) : 0;
                    }

                    dr[Att_ReportMonthlyOvertimeEntity.FieldNames.DateFrom] = dateStart;
                    dr[Att_ReportMonthlyOvertimeEntity.FieldNames.DateTo] = dateEnd;

                    // xem gio tang ca tu dau nam den gio
                    // 

                    //trietmai Cột lũy kế chỉ tính time. 1.5

                    double cumOT1 = 0;
                    if (condition.Cumulative == true)
                    {
                        DateTime TimebeginMonth = new DateTime(dateStart.Year, 1, 1);
                        DateTime TimeEndMonth = TimebeginMonth.AddMonths(1).AddMinutes(-1);
                        //getStartEndDate(profile, TimebeginMonth, out TimebeginMonth, out TimeEndMonth, lstGrade, appConfig);
                        if (statusHourCum == OverTimeStatus.E_APPROVED.ToString())
                        {
                            cumOT1 = lstCumOvertimeAll.Where(ot => ot.ProfileID == profile.ID
                                                                && lstOvertimeTypeIDs.Contains(ot.OvertimeTypeID)
                                                                && ot.WorkDate >= TimebeginMonth
                                                                ).Sum(re => re.ApproveHours ?? 0);
                        }
                        else if (statusHourCum == OverTimeStatus.E_CONFIRM.ToString())
                        {
                            cumOT1 = lstCumOvertimeAll.Where(ot => ot.ProfileID == profile.ID
                                                               && lstOvertimeTypeIDs.Contains(ot.OvertimeTypeID)
                                                                && ot.WorkDate >= TimebeginMonth
                                                               ).Sum(re => re.ConfirmHours);
                        }
                        else if (statusHourCum == OverTimeStatus.E_SUBMIT.ToString())
                        {
                            cumOT1 = lstCumOvertimeAll.Where(ot => ot.ProfileID == profile.ID
                                                              && lstOvertimeTypeIDs.Contains(ot.OvertimeTypeID)
                                                               && ot.WorkDate >= TimebeginMonth
                                                               ).Sum(re => re.RegisterHours);
                        }
                    }

                    dr[Att_ReportMonthlyOvertimeEntity.FieldNames.CumulativeOT] = cumOT1;

                    if (lstProOvertimeReason.Count() > 0)
                    {
                        List<Att_Overtime> lstOVReson_temp = lstProOvertimeReason.Where(p => lstOvertimeReasonTypeHome.Contains(p.OvertimeResonID ?? Guid.Empty)).ToList();
                        dr[OvertimeReasonCode.OT_HOME.ToString()] = lstOVReson_temp.Count() > 0 ? lstOVReson_temp.Sum(p => p.ApproveHours == null ? 0 : p.ApproveHours.Value) : 0;

                        lstOVReson_temp = lstProOvertimeReason.Where(p => lstOvertimeReasonTypeProvider.Contains(p.OvertimeResonID ?? Guid.Empty)).ToList();
                        dr[OvertimeReasonCode.OT_PROVIDER.ToString()] = lstOVReson_temp.Count() > 0 ? lstOVReson_temp.Sum(p => p.ApproveHours == null ? 0 : p.ApproveHours.Value) : 0;

                        lstOVReson_temp = lstProOvertimeReason.Where(p => lstOvertimeReasonTypeBU.Contains(p.OvertimeResonID ?? Guid.Empty)).ToList();
                        dr[OvertimeReasonCode.OT_BU.ToString()] = lstOVReson_temp.Count() > 0 ? lstOVReson_temp.Sum(p => p.ApproveHours == null ? 0 : p.ApproveHours.Value) : 0;


                    }
                    //trietmai Côt oT_EArly ở đây là Loại đăng ký tăng ca
                    //var OtType = adjDate lstProOvertimeReason.Where(p => p.Cat_OvertimeReson.Code == OvertimeReasonCode.OT_EARLY.ToString()).ToList();
                    //dr[OvertimeReasonCode.OT_EARLY.ToString()] = lstOVReson_temp.Count() > 0 ? lstOVReson_temp.Sum(p => p.ApproveHours) : 0;
                    //Theo dinh nghĩa Lâm.Le là ko co cty nào sai cột này nên co thể sưa 20130425_0904
                    dr[OvertimeReasonCode.OT_EARLY.ToString()] = OTEarly;

                    dr[Att_ReportMonthlyOvertimeEntity.FieldNames.DateExport] = DateTime.Now;
                    var orgTemp = lstOrg.Where(s => s.ID == profile.OrgStructureID).FirstOrDefault();
                    if (orgTemp != null)
                    {
                        dr[Att_ReportMonthlyOvertimeEntity.FieldNames.OrgStructure] = orgTemp.OrgStructureName;
                        dr[Att_ReportMonthlyOvertimeEntity.FieldNames.OrgStructureCode] = orgTemp.Code;

                    }

                    tblData.Rows.Add(dr);
                }

                //if (tblData.Rows.Count > 0)
                //{
                //    foreach (DataRow dataRow in tblData.Rows)
                //    {             
                //        if (lstOrg.Count > 0)
                //        {
                //            dataRow[Att_ReportMonthlyOvertimeEntity.FieldNames.OrgStructure] = lstOrg[0].OrgStructureName;
                //            dataRow[Att_ReportMonthlyOvertimeEntity.FieldNames.OrgStructureCode] = lstOrg[0].Code;

                //        }
                //        //else dataRow[Att_ReportMonthlyOvertimeEntity.FieldNames.OrgStructure] = LanguageManager.GetString(Att_ReportMonthlyOvertimeEntity.FieldNames.AllOfCompany);
                //    }
                //}
                var configs = new Dictionary<string, Dictionary<string, object>>();
                var confighidden = new Dictionary<string, object>();
                confighidden.Add("hidden", true);
                configs.Add("DateFrom", confighidden);
                configs.Add("DateTo", confighidden);
                configs.Add("UserExport", confighidden);
                configs.Add("DateExport", confighidden);
                var config90 = new Dictionary<string, object>();
                config90.Add("width", 90);
                configs.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.CodeEmp, config90);
                configs.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.Department, config90);
                configs.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.PositionName, config90);
                configs.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.JobTitleName, config90);
                configs.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.Section, config90);
                var config135 = new Dictionary<string, object>();
                config135.Add("width", 135);
                configs.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.ProfileName, config135);
                var config110 = new Dictionary<string, object>();
                config110.Add("width", 110);
                configs.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.DepartmentCode, config110);
                configs.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.DeptPath, config110);
                configs.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.DepartmentInRoster, config110);
                configs.Add(Att_ReportMonthlyOvertimeEntity.FieldNames.WorkPlace, config110);

                return tblData.ConfigTable(configs);
            }
        }

        #region get profile
        public List<Hre_Profile> GetListProfile(List<Hre_ProfileEntity> listPro, DateTime dateStart, DateTime dateEnd, string StatusEmployee, List<Cat_OrgStructure> lstOrg, List<Cat_PayrollGroup> lstPayrollGroup, string UserLogin)
        {
            string status = string.Empty;
            DateTime _monthYear = dateEnd;
            List<object> lstWH = new List<object>();
            lstWH.Add(null);
            lstWH.Add(_monthYear);
            List<Hre_WorkHistory> lstWorkingHistory = GetData<Hre_WorkHistory>(lstWH, ConstantSql.hrm_hre_getdata_WorkHistory, UserLogin, ref status)
                                                            .OrderByDescending(m => m.DateEffective)
                                                            .ToList<Hre_WorkHistory>();

            List<Hre_Profile> lstProfile = LoadProfileStatus(listPro, StatusEmployee, dateStart, dateEnd);
            lstProfile = LoadProfileByOrg(lstProfile, lstWorkingHistory, _monthYear, lstOrg);
            if (lstPayrollGroup != null)
            {
                lstProfile = LoadProfileByPayrollGroup(lstProfile, lstWorkingHistory, _monthYear, lstPayrollGroup);
            }
            return lstProfile;

        }
        private List<Hre_Profile> LoadProfileStatus(List<Hre_ProfileEntity> listPro, String status, DateTime From, DateTime To)
        {
            List<Hre_Profile> lstProfile = new Hre_ProfileServices().GetWorkingProfile(listPro, From, To);
            if (status == StatusEmployee.E_WORKING.ToString())
            {
                lstProfile = lstProfile.Where(pro => (pro.DateQuit == null || pro.DateQuit > To) && pro.DateHire < From).ToList();
            }
            else if (status == StatusEmployee.E_NEWEMPLOYEE.ToString())
            {
                lstProfile = lstProfile.Where(pro => pro.DateHire < To && pro.DateHire > From).ToList();
            }
            else if (status == StatusEmployee.E_STOPWORKING.ToString())
            {
                lstProfile = lstProfile.Where(pro => pro.DateQuit < To && pro.DateQuit > From).ToList();
            }
            else if (status == StatusEmployee.E_WORKINGANDNEW.ToString())
            {
                lstProfile = lstProfile.Where(pro => pro.DateQuit == null || pro.DateQuit > To).ToList();
            }
            else
            {
                return lstProfile;
            }
            return lstProfile;
        }
        private List<Hre_Profile> LoadProfileByOrg(List<Hre_Profile> lstProfile, List<Hre_WorkHistory> lstWorkingHistory, DateTime _monthYear, List<Cat_OrgStructure> lstOrg)
        {
            if (lstOrg.Count > 0)
            {
                List<Hre_Profile> lstResult = new Hre_ProfileServices().GetProfileWithOrgByHistory(lstProfile, lstOrg, lstWorkingHistory, _monthYear, false);
                return lstResult;
            }
            return lstProfile;
        }
        private List<Hre_Profile> LoadProfileByPayrollGroup(List<Hre_Profile> lstProfile, List<Hre_WorkHistory> lstWorkingHistory, DateTime _monthYear, List<Cat_PayrollGroup> lstPayrollGroup)
        {
            if (lstPayrollGroup.Count > 0)
            {
                List<Hre_Profile> lstResult = new Hre_ProfileServices().GetProfileWithPayrollGroupByHistory(lstProfile, lstPayrollGroup, lstWorkingHistory, _monthYear, false);
                return lstResult;
            }
            return lstProfile;
        }
        #endregion

        #endregion

        #region BC Chi Tiết Ngày Làm Thêm - V7.3.01.31(20140808.3)
        // Sonvo - 20140807 - sửa lại logic giống bản 7
        DataTable CreateReportDetailOvertimeSchema()
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                DataTable tb = new DataTable("Att_ReportDetailOvertimeEntity");
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.ProfileName);
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.CodeOrg);
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.CodeJobtitle);
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.CodePosition);
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.CodeBranch);
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.CodeTeam);
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.CodeSection);
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.BranchName);
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.OrgName);
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.TeamName);
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.SectionName);
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.DateOvertime, typeof(DateTime));
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.udInTime, typeof(DateTime));
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.udOutTime, typeof(DateTime));
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.ShiftName);
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.udSubmitApproveHour);
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.ReasonOT);
                var repoOvertimeType = new Cat_OvertimeTypeRepository(unitOfWork);
                var lstCodeOvertimeType = repoOvertimeType.FindBy(s => s.Code != null && s.IsDelete == null).Select(s => s.Code).Distinct().ToList();
                //DataRow row = tb.NewRow();

                foreach (string code in lstCodeOvertimeType)
                {
                    if (!tb.Columns.Contains(code))
                    {
                        tb.Columns.Add(code);
                        //row[code] = code;
                    }
                    if (!tb.Columns.Contains(code + "_Confirm"))
                    {
                        tb.Columns.Add(code + "_Confirm");
                    }
                    if (!tb.Columns.Contains(code + "_Approve"))
                    {
                        tb.Columns.Add(code + "_Approve");
                    }
                }

                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.DateFrom, typeof(DateTime));
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.DateTo, typeof(DateTime));
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.UserExport);
                tb.Columns.Add(Att_ReportDetailOvertimeEntity.FieldNames.DateExport, typeof(DateTime));
                //tb.Rows.Add(row);
                return tb;
            }
        }
        public DataTable GetReportDetailOvertime(string userExport, DateTime dateStart, DateTime dateEndSearch, string ProfileName, string CodeEmp, string OrgIDString, List<Guid?> lstovertimeTypeIds, bool noDisplay0Data, bool isIncludeQuitEmp)
        {
            DataTable table = CreateReportDetailOvertimeSchema();
            DateTime dateEnd = dateEndSearch.AddDays(1).AddMinutes(-1);

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Overtime = new Att_OvertimeRepository(unitOfWork);
                string key = OverTimeStatus.E_APPROVED.ToString();
                string E_CASHOUT = MethodOption.E_CASHOUT.ToString();



                var overtimesQuery = unitOfWork.CreateQueryable<Att_Overtime>(s => s.IsDelete == null &&
                    (s.RegisterHours > 0 || s.ApproveHours > 0) && s.Status == key && dateStart <= s.WorkDate
                    && s.WorkDate <= dateEnd && (s.MethodPayment == null || s.MethodPayment == E_CASHOUT));
                if (!string.IsNullOrEmpty(OrgIDString))
                {
                    List<Guid> OrgIDs = new List<Guid>();
                    List<string> OrgIDsArr = OrgIDString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                    foreach (var item in OrgIDsArr)
                    {
                        Guid OrgID = Guid.Empty;
                        Guid.TryParse(item, out OrgID);
                        if (OrgID != Guid.Empty)
                        {
                            OrgIDs.Add(OrgID);
                        }
                    }
                    overtimesQuery = overtimesQuery.Where(m => m.OrgStructureID != null && OrgIDs.Contains(m.OrgStructureID.Value));
                }
                if (!string.IsNullOrEmpty(ProfileName))
                {
                    List<string> lstProfileName = ProfileName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                    if (lstProfileName.Count > 1)
                    {
                        overtimesQuery = overtimesQuery.Where(m => m.Hre_Profile != null && lstProfileName.Contains(m.Hre_Profile.ProfileName));
                    }
                    else
                    {
                        overtimesQuery = overtimesQuery.Where(m => m.Hre_Profile != null && m.Hre_Profile.ProfileName != null && (m.Hre_Profile.ProfileName.Contains(ProfileName)));
                    }

                }

                if (!string.IsNullOrEmpty(CodeEmp))
                {
                    List<string> lstCodeEmp = CodeEmp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                    if (lstCodeEmp.Count > 1)
                    {
                        overtimesQuery = overtimesQuery.Where(m => m.Hre_Profile != null && lstCodeEmp.Contains(m.Hre_Profile.CodeEmp));
                    }
                    else
                    {
                        overtimesQuery = overtimesQuery.Where(m => m.Hre_Profile != null && m.Hre_Profile.CodeEmp != null && (m.Hre_Profile.CodeEmp.Contains(CodeEmp)));
                    }
                }
                if (noDisplay0Data)
                {
                    overtimesQuery = overtimesQuery.Where(s => s.ApproveHours > 0);
                }
                if (lstovertimeTypeIds != null && lstovertimeTypeIds[0] != null && lstovertimeTypeIds[0] != Guid.Empty)
                {
                    overtimesQuery = overtimesQuery.Where(s => lstovertimeTypeIds.Contains(s.OvertimeTypeID));
                }
                var overtimes = overtimesQuery.Select(s => new { s.ProfileID, s.WorkDate, s.OvertimeTypeID, s.ApproveHours, s.RegisterHours, s.AnalyseHour, s.ConfirmHours, s.OrgStructureID, s.ReasonOT }).ToList();

                var profileIds = overtimes.Select(s => s.ProfileID).Distinct().ToList();

                var repoAtt_Workday = new Att_WorkDayRepository(unitOfWork);
                var workDays = repoAtt_Workday.FindBy(s => s.IsDelete == null & dateStart <= s.WorkDate && s.WorkDate <= dateEnd)
             .Select(s => new { s.ProfileID, s.ShiftID, s.InTime1, s.OutTime1, s.WorkDate }).ToList();

                var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoHre_Profile.FindBy(s => s.IsDelete == null && profileIds.Contains(s.ID)).Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();

                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.IsDelete == null && s.Code != null).ToList();

                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var positions = repoCat_Position.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();

                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var jobtitles = repoCat_JobTitle.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();

                var repoCat_OvertimeType = new Cat_OvertimeTypeRepository(unitOfWork);
                var overtimeTypes = repoCat_OvertimeType.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => new { s.ID, s.Code }).ToList();

                var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                var shifts = repoCat_Shift.FindBy(s => s.IsDelete == null).ToList();

                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                }

                foreach (var profile in profiles)
                {
                    for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                    {
                        var overtimeProfiles = overtimes.Where(s => s.WorkDate.Date == date.Date && s.ProfileID == profile.ID).ToList();
                        if (overtimeProfiles.Count == 0)
                            continue;

                        var lastOvertimeByProfile = overtimeProfiles.OrderByDescending(m => m.WorkDate).FirstOrDefault();


                        DataRow row = table.NewRow();
                        Guid? orgId = profile.OrgStructureID;
                        if (lastOvertimeByProfile != null)
                            orgId = lastOvertimeByProfile.OrgStructureID;

                        var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                        var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                        var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                        var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                        var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                        row[Att_ReportDetailOvertimeEntity.FieldNames.CodeBranch] = orgBranch != null ? orgBranch.Code : string.Empty;
                        row[Att_ReportDetailOvertimeEntity.FieldNames.CodeOrg] = orgOrg != null ? orgOrg.Code : string.Empty;
                        row[Att_ReportDetailOvertimeEntity.FieldNames.CodeTeam] = orgTeam != null ? orgTeam.Code : string.Empty;
                        row[Att_ReportDetailOvertimeEntity.FieldNames.CodeSection] = orgSection != null ? orgSection.Code : string.Empty;
                        row[Att_ReportDetailOvertimeEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                        row[Att_ReportDetailOvertimeEntity.FieldNames.OrgName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                        row[Att_ReportDetailOvertimeEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                        row[Att_ReportDetailOvertimeEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                        row[Att_ReportDetailOvertimeEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                        row[Att_ReportDetailOvertimeEntity.FieldNames.ProfileName] = profile.ProfileName;
                        row[Att_ReportDetailOvertimeEntity.FieldNames.DateExport] = DateTime.Now;
                        row[Att_ReportDetailOvertimeEntity.FieldNames.ReasonOT] = lastOvertimeByProfile.ReasonOT;
                        var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                        var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
                        var workDay = workDays.FirstOrDefault(s => s.ProfileID == profile.ID && s.WorkDate == date);
                        var shift = shifts.FirstOrDefault(s => workDay != null && s.ID == workDay.ShiftID);
                        row[Att_ReportDetailOvertimeEntity.FieldNames.CodePosition] = positon != null ? positon.Code : string.Empty;
                        row[Att_ReportDetailOvertimeEntity.FieldNames.CodeJobtitle] = jobtitle != null ? jobtitle.Code : string.Empty;
                        row[Att_ReportDetailOvertimeEntity.FieldNames.DateFrom] = dateStart;
                        row[Att_ReportDetailOvertimeEntity.FieldNames.DateTo] = dateEnd;
                        row[Att_ReportDetailOvertimeEntity.FieldNames.DateExport] = DateTime.Now;
                        row[Att_ReportDetailOvertimeEntity.FieldNames.UserExport] = userExport;
                        row[Att_ReportDetailOvertimeEntity.FieldNames.udSubmitApproveHour] = overtimeProfiles.Where(m => m.AnalyseHour != null).Sum(m => m.AnalyseHour);

                        row[Att_ReportDetailOvertimeEntity.FieldNames.DateOvertime] = date;
                        if (workDay != null && workDay.InTime1 != null)
                        {
                            //row[Att_ReportDetailOvertimeEntity.FieldNames.udInTime] = workDay.InTime1.Value.ToString("HH:mm:ss");
                            row[Att_ReportDetailOvertimeEntity.FieldNames.udInTime] = workDay.InTime1.Value;
                        }
                        if (workDay != null && workDay.OutTime1 != null)
                        {
                            //row[Att_ReportDetailOvertimeEntity.FieldNames.udOutTime] = workDay.OutTime1.Value.ToString("HH:mm:ss");
                            row[Att_ReportDetailOvertimeEntity.FieldNames.udOutTime] = workDay.OutTime1.Value;
                        }

                        if (shift != null)
                        {

                            row[Att_ReportDetailOvertimeEntity.FieldNames.ShiftName] = shift.ShiftName;
                        }
                        foreach (var item in overtimeTypes)
                        {
                            var sum = overtimeProfiles.Where(s => s.OvertimeTypeID == item.ID).Sum(s => s.RegisterHours);
                            row[item.Code] = sum > 0 ? (object)sum : DBNull.Value;
                            sum = overtimeProfiles.Where(s => s.OvertimeTypeID == item.ID && s.ApproveHours > 0).Sum(s => s.ApproveHours.Value);
                            row[item.Code + "_Approve"] = sum > 0 ? (object)sum : DBNull.Value;
                            sum = overtimeProfiles.Where(s => s.OvertimeTypeID == item.ID && s.ConfirmHours != null && s.ConfirmHours > 0).Sum(s => s.ConfirmHours);
                            row[item.Code + "_Confirm"] = sum > 0 ? (object)sum : DBNull.Value;
                        }
                        table.Rows.Add(row);
                    }
                }
                var configs = new Dictionary<string, Dictionary<string, object>>();
                var config = new Dictionary<string, object>();
                var configtime = new Dictionary<string, object>();
                var configwidthCode = new Dictionary<string, object>();
                var configwidthName = new Dictionary<string, object>();
                var configwidthshift = new Dictionary<string, object>();
                var configwidthudSubmitApproveHour = new Dictionary<string, object>();


                configwidthCode.Add("width", 80);
                configs.Add("CodeBranch", configwidthCode);
                configs.Add("CodeOrg", configwidthCode);
                configs.Add("CodeTeam", configwidthCode);
                configs.Add("CodeSection", configwidthCode);
                configs.Add("CodeJobtitle", configwidthCode);
                configs.Add("CodePosition", configwidthCode);

                configwidthName.Add("width", 110);
                configs.Add("BranchName", configwidthName);
                configs.Add("OrgName", configwidthName);
                configs.Add("TeamName", configwidthName);
                configs.Add("SectionName", configwidthName);

                configwidthudSubmitApproveHour.Add("width", 120);
                configs.Add("udSubmitApproveHour", configwidthudSubmitApproveHour);

                configwidthshift.Add("width", 90);
                configs.Add("DateOvertime", configwidthshift);
                configs.Add("ShiftName", configwidthshift);

                config.Add("hidden", true);
                configs.Add("DateFrom", config);
                configs.Add("DateTo", config);
                configs.Add("UserExport", config);
                configs.Add("DateExport", config);

                configtime.Add("width", 80);
                configtime.Add("format", "{0:HH:mm:ss}");
                configs.Add("udOutTime", configtime);
                configs.Add("udInTime", configtime);
                // return table.ConfigDatatable();
                return table.ConfigTable(configs);
            }
        }
        #endregion

        #region BC Thống Kê Tăng Ca - V7.3.01.31(20140808.3)
        DataTable CreateReportSummaryOvertimeMonthSchema()
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                DataTable tb = new DataTable("Att_ReportSummaryOvertimeMonthEntity");
                // DataRow dr = tb.NewRow();
                //tb.Columns.Add(ConstantDisplay.HRM_Att_Report_CodeEmp.TranslateString());
                //tb.Columns.Add(ConstantDisplay.HRM_Att_Report_ProfileName.TranslateString());
                //tb.Columns.Add(ConstantDisplay.HRM_Att_Report_CodeOrg.TranslateString());
                tb.Columns.Add(Att_ReportSummaryOvertimeMonthEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Att_ReportSummaryOvertimeMonthEntity.FieldNames.ProfileName);
                tb.Columns.Add(Att_ReportSummaryOvertimeMonthEntity.FieldNames.CodeOrg);
                tb.Columns.Add(Att_ReportSummaryOvertimeMonthEntity.FieldNames.CodeTeam);
                tb.Columns.Add(Att_ReportSummaryOvertimeMonthEntity.FieldNames.CodeSection);
                tb.Columns.Add(Att_ReportSummaryOvertimeMonthEntity.FieldNames.CodeJobtitle);
                tb.Columns.Add(Att_ReportSummaryOvertimeMonthEntity.FieldNames.CodePosition);
                tb.Columns.Add(Att_ReportSummaryOvertimeMonthEntity.FieldNames.CodeBranch);
                tb.Columns.Add(Att_ReportSummaryOvertimeMonthEntity.FieldNames.BranchName);
                tb.Columns.Add(Att_ReportSummaryOvertimeMonthEntity.FieldNames.OrgName);
                tb.Columns.Add(Att_ReportSummaryOvertimeMonthEntity.FieldNames.TeamName);
                tb.Columns.Add(Att_ReportSummaryOvertimeMonthEntity.FieldNames.SectionName);
                tb.Columns.Add(Att_ReportSummaryOvertimeMonthEntity.FieldNames.DateFrom, typeof(DateTime));
                tb.Columns.Add(Att_ReportSummaryOvertimeMonthEntity.FieldNames.DateTo, typeof(DateTime));
                tb.Columns.Add(Att_ReportSummaryOvertimeMonthEntity.FieldNames.UserExport);
                tb.Columns.Add(Att_ReportSummaryOvertimeMonthEntity.FieldNames.DateExport, typeof(DateTime));
                var repoOvertimeType = new Cat_OvertimeTypeRepository(unitOfWork);

                var lstCodeOvertimeType = repoOvertimeType.FindBy(s => s.Code != null).Select(s => s.OvertimeTypeName).Distinct().ToList();

                foreach (string code in lstCodeOvertimeType)
                {
                    if (!tb.Columns.Contains(code))
                    {
                        tb.Columns.Add(code);
                        tb.Columns.Add(code + "Approve");
                    }
                }
                return tb;
            }
        }
        public DataTable GetReportSummaryOvertimeMonth(DateTime dateStart, DateTime dateEndSearch, string orgStructureID, List<Hre_ProfileEntity> lstProfileObjects, Guid[] lstOvertimetypeIDs, Guid[] lstShiftIDs, bool IsDisplay0Data, bool isIncludeQuitEmp, string userExport, string UserLogin)
        {
            DataTable table = CreateReportSummaryOvertimeMonthSchema();
            DateTime dateEnd = dateEndSearch.AddDays(1).AddSeconds(-1);
            using (var context = new VnrHrmDataContext())
            {
                BaseService basevice = new BaseService();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                string key = AttendanceDataStatus.E_APPROVED.ToString();
                //   var repoAtt_Overtime = new Att_OvertimeRepository(unitOfWork);
                string status = string.Empty;
                List<object> paraOvertime = new List<object>();
                paraOvertime.AddRange(new object[3]);
                paraOvertime[0] = orgStructureID;
                paraOvertime[1] = dateStart;
                paraOvertime[2] = dateEnd;
                var overtimeDays_Actual = basevice.GetData<Att_OvertimeEntity>(paraOvertime, ConstantSql.hrm_att_getdata_Overtime, UserLogin, ref status).Where(s => s.Status == key).ToList();
                string E_CASHOUT = MethodOption.E_CASHOUT.ToString();

                var lstProfileIds = lstProfileObjects.Select(p => p.ID).ToList();
                //var overtimeDays = repoAtt_Overtime.FindBy(s => s.IsDelete == null && s.Status == key
                //    && dateStart <= s.WorkDate && s.WorkDate <= dateEnd).Select(s =>
                //new { s.ProfileID, s.OvertimeTypeID, s.ApproveHours, s.RegisterHours }).ToList();
                var profileIds = overtimeDays_Actual.Select(s => s.ProfileID).Distinct().ToList();
                var profiles = lstProfileObjects.Select(s => new
                {
                    s.ID,
                    s.DateQuit,
                    s.OrgStructureID,
                    s.ProfileName,
                    s.CodeEmp,
                    s.PositionID,
                    s.JobTitleID
                }).ToList();
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.IsDelete == null && s.Code != null).ToList();

                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var positions = repoCat_Position.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => new { s.ID, s.Code }).ToList();

                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var jobtitles = repoCat_JobTitle.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => new { s.ID, s.Code }).ToList();

                var repoCat_OvertimeType = new Cat_OvertimeTypeRepository(unitOfWork);
                var overtimeTypeIds = repoCat_OvertimeType.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => new { s.ID, s.Code, s.OvertimeTypeName }).Distinct().ToList();

                //   var repoAtt_Workday = new Att_WorkDayRepository(unitOfWork);
                //var workDays = repoAtt_Workday.FindBy(s => s.IsDelete == null && dateStart <= s.WorkDate && s.WorkDate <= dateEnd)
                //    .Select(s => new { s.ProfileID, s.ShiftID }).ToList();
                var workDays = basevice.GetData<Att_WorkdayEntity>(paraOvertime, ConstantSql.hrm_att_getdata_Workday, UserLogin, ref status).ToList();

                //if (lstProfileIds != null && lstProfileIds[0] != null && lstProfileIds[0] != Guid.Empty)
                //{
                //    profiles = profiles.Where(s => lstProfileIds.Contains(s.ID)).ToList();
                //}
                if (lstOvertimetypeIDs != null)
                {
                    overtimeDays_Actual = overtimeDays_Actual.Where(s => lstOvertimetypeIDs.Contains(s.OvertimeTypeID)).ToList();
                }
                var overtimeDays_Remain = overtimeDays_Actual.Where(m => m.MethodPayment == E_CASHOUT).ToList();
                if (lstShiftIDs != null)
                {
                    workDays = workDays.Where(s => s.ShiftID.HasValue && lstShiftIDs.Contains(s.ShiftID.Value)).ToList();
                    profileIds = workDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }
                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                }
                List<Guid> guids = profiles.Select(s => s.ID).ToList();
                overtimeDays_Actual = overtimeDays_Actual.Where(s => guids.Contains(s.ProfileID)).ToList();
                if (IsDisplay0Data)
                {
                    var profileIDs = overtimeDays_Actual.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIDs.Contains(s.ID)).ToList();
                }

                foreach (var profile in profiles)
                {
                    DataRow row = table.NewRow();
                    Guid? orgId = profile.OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    row[Att_ReportSummaryOvertimeMonthEntity.FieldNames.CodeBranch] = orgBranch != null ? orgBranch.Code : string.Empty;
                    row[Att_ReportSummaryOvertimeMonthEntity.FieldNames.CodeTeam] = orgTeam != null ? orgTeam.Code : string.Empty;
                    row[Att_ReportSummaryOvertimeMonthEntity.FieldNames.CodeSection] = orgSection != null ? orgSection.Code : string.Empty;
                    row[Att_ReportSummaryOvertimeMonthEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    row[Att_ReportSummaryOvertimeMonthEntity.FieldNames.OrgName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    row[Att_ReportSummaryOvertimeMonthEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    row[Att_ReportSummaryOvertimeMonthEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                    var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                    var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
                    row[Att_ReportSummaryOvertimeMonthEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    row[Att_ReportSummaryOvertimeMonthEntity.FieldNames.ProfileName] = profile.ProfileName;
                    row[Att_ReportSummaryOvertimeMonthEntity.FieldNames.CodeOrg] = orgOrg != null ? orgOrg.Code : string.Empty;
                    row[Att_ReportSummaryOvertimeMonthEntity.FieldNames.CodePosition] = positon != null ? positon.Code : string.Empty;
                    row[Att_ReportSummaryOvertimeMonthEntity.FieldNames.CodeJobtitle] = jobtitle != null ? jobtitle.Code : string.Empty;
                    row[Att_ReportSummaryOvertimeMonthEntity.FieldNames.DateExport] = DateTime.Now;
                    row[Att_ReportSummaryOvertimeMonthEntity.FieldNames.DateFrom] = dateStart;
                    row[Att_ReportSummaryOvertimeMonthEntity.FieldNames.DateTo] = dateEnd;
                    row[Att_ReportSummaryOvertimeMonthEntity.FieldNames.UserExport] = userExport;
                    foreach (var overtime in overtimeTypeIds)
                    {
                        if (overtime != null && !string.IsNullOrEmpty(overtime.OvertimeTypeName) && table.Columns.Contains(overtime.OvertimeTypeName))
                        {
                            var sum = overtimeDays_Actual.Where(s => s.ProfileID == profile.ID && s.OvertimeTypeID == overtime.ID).Sum(s => s.RegisterHours);
                            row[overtime.OvertimeTypeName] = sum > 0 ? (object)sum : DBNull.Value;
                            var sum1 = overtimeDays_Remain.Where(s => s.ProfileID == profile.ID && s.OvertimeTypeID == overtime.ID).Sum(s => s.ApproveHours);
                            row[overtime.OvertimeTypeName + "Approve"] = sum1 > 0 ? (object)sum1 : DBNull.Value;
                        }
                    }
                    table.Rows.Add(row);
                }
                var configs = new Dictionary<string, Dictionary<string, object>>();
                var confighidden = new Dictionary<string, object>();
                var configwidthCodeEmp = new Dictionary<string, object>();
                var configwidthCodeOrg = new Dictionary<string, object>();
                var configwidthNameOrg = new Dictionary<string, object>();
                configwidthCodeEmp.Add("width", 88);
                configs.Add("CodeEmp", configwidthCodeEmp);
                configwidthCodeOrg.Add("width", 90);
                configs.Add("CodeOrg", configwidthCodeOrg);
                configs.Add("CodeBranch", configwidthCodeOrg);
                configs.Add("CodeTeam", configwidthCodeOrg);
                configs.Add("CodePosition", configwidthCodeOrg);
                configs.Add("CodeJobtitle", configwidthCodeOrg);
                configs.Add("CodeSection", configwidthCodeOrg);
                configwidthNameOrg.Add("width", 110);
                configs.Add("BranchName", configwidthNameOrg);
                configs.Add("OrgName", configwidthNameOrg);
                configs.Add("TeamName", configwidthNameOrg);
                configs.Add("SectionName", configwidthNameOrg);
                confighidden.Add("hidden", true);
                configs.Add("DateFrom", confighidden);
                configs.Add("DateTo", confighidden);
                configs.Add("UserExport", confighidden);
                configs.Add("DateExport", confighidden);
                return table.ConfigTable(configs);
            }
        }
        DataTable CreateReportMonthlyRosterSchema(List<Cat_Shift> lstShift)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                DataTable tb = new DataTable("Att_ReportMonthlyRosterEntity");
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.ProfileName);
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.DepartmentCode);
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.BranchCode);
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.GroupCode);
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.TeamCode);
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.SectionCode);
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.DivisionCode);
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.PositionName);
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.JobtitleName);
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.DepartmentName);
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.BranchName);
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.GroupName);
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.TeamName);
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.SectionName);
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.DivisionName);

                for (int i = 1; i <= 31; i++)
                {
                    tb.Columns.Add("Data" + i);
                    tb.Columns.Add("Header" + i);
                }
                foreach (Cat_Shift item in lstShift)
                {
                    if (tb.Columns.Contains(item.ShiftName)
                        || tb.Columns.Contains(item.Code.ToString()))
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(item.Code))
                    {
                        tb.Columns.Add(item.Code, typeof(string));
                    }
                    else
                    {
                        tb.Columns.Add(item.ShiftName, typeof(string));
                    }
                }

                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.UserExport);
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.DateFrom, typeof(DateTime));
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.DateTo, typeof(DateTime));
                tb.Columns.Add(Att_ReportMonthlyRosterEntity.FieldNames.DateExport, typeof(DateTime));
                return tb;
            }

        }


        public DataTable GetReportMonthlyRoster(DateTime DateStart, DateTime DateEnd, string orgStructureID, List<Hre_ProfileEntity> profiles, bool _onlyHolydayNotHaveRoster, Guid[] lstpayrollIDs, string userExport, string UserLogin)
        {

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (UnitOfWork)(new UnitOfWork(context));
                BaseService basevervice = new BaseService();
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);
                var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null && s.IsDelete == null).ToList();
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                var positions = repoCat_Position.FindBy(s => s.Code != null && s.IsDelete == null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null && s.IsDelete == null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();
                if (DateEnd != null)
                    DateEnd = DateEnd.AddDays(1).AddMilliseconds(-1);
                if (lstpayrollIDs != null)
                {
                    profiles = profiles.Where(p => p.PayrollGroupID.HasValue && lstpayrollIDs.Contains(p.PayrollGroupID.Value)).ToList();
                }
                string key = RosterStatus.E_APPROVED.ToString();
                string key1 = RosterType.E_TIME_OFF.ToString();
                List<Att_Roster> rosters = null;
                var listProfileID = profiles.Select(d => d.ID).ToList();
                string status = string.Empty;
                List<object> paraAtt_Roster = new List<object>();
                paraAtt_Roster.AddRange(new object[4]);
                paraAtt_Roster[0] = (object)orgStructureID;
                paraAtt_Roster[1] = DateStart;
                paraAtt_Roster[2] = DateEnd;
                paraAtt_Roster[3] = key;
                rosters = basevervice.GetData<Att_RosterEntity>(paraAtt_Roster, ConstantSql.hrm_att_getdata_Roster, UserLogin, ref status).Where(s => listProfileID.Contains(s.ProfileID)).ToList().Translate<Att_Roster>();
                var listShift = repoCat_Shift.FindBy(s => s.IsDelete == null).ToList();
                DataTable table = CreateReportMonthlyRosterSchema(listShift);
                if (profiles == null)
                    return table;
                int stt = 0;
                foreach (var profile in profiles)
                {
                    stt++;
                    DataRow row = table.NewRow();
                    var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                    var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
                    Guid? orgId = profile.OrgStructureID;
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgGroup = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    var orgDivision = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                    row[Att_ReportMonthlyRosterEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.ProfileName] = profile.ProfileName;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.GroupCode] = orgGroup != null ? orgGroup.Code : string.Empty;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.GroupName] = orgGroup != null ? orgGroup.OrgStructureName : string.Empty;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.DivisionCode] = orgDivision != null ? orgDivision.Code : string.Empty;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.DivisionName] = orgDivision != null ? orgDivision.OrgStructureName : string.Empty;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.ProfileName] = profile.ProfileName;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.PositionName] = positon != null ? positon.PositionName : string.Empty;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.JobtitleName] = jobtitle != null ? jobtitle.JobTitleName : string.Empty;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.DateFrom] = DateStart;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.DateTo] = DateEnd;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.DateExport] = DateTime.Today;
                    row[Att_ReportMonthlyRosterEntity.FieldNames.UserExport] = userExport;


                    var attRosters = rosters.Where(s => s.ProfileID == profile.ID && s.Type != key1).OrderBy(s => s.DateStart).ToList();
                    #region CheckData
                    foreach (Att_Roster attRoster in attRosters)
                    {
                        for (DateTime date = attRoster.DateStart; date <= attRoster.DateEnd; date = date.AddDays(1))
                        {
                            #region GetHeaderRow
                            if (stt == 1)
                            {
                                row["Header" + date.Day] = date.ToString("dd-MMM");
                            }
                            else if (stt == 2)
                            {
                                row["Header" + date.Day] = date.DayOfWeek.ToString();
                            }
                            #endregion
                            #region GetShiff
                            if (DateStart > date || date > DateEnd)
                            {
                                break;
                            }

                            if (attRoster.Type == RosterType.E_CHANGE_SHIFT.ToString())
                            {
                                var shift = listShift.Where(d => d.ID == attRoster.MonShiftID).FirstOrDefault();
                                row["Data" + date.Day] = shift == null ? "" : shift.Code;
                            }
                            else if (attRoster.Type == RosterType.E_TIME_OFF.ToString())
                            {
                                row["Data" + date.Day] = "";
                            }
                            else if (date.DayOfWeek == DayOfWeek.Monday)
                            {
                                var shift = listShift.Where(d => d.ID == attRoster.MonShiftID).FirstOrDefault();
                                row["Data" + date.Day] = shift == null ? "" : shift.Code;
                            }
                            else
                            {
                                if (date.DayOfWeek == DayOfWeek.Tuesday)
                                {
                                    var shift = listShift.Where(d => d.ID == attRoster.TueShiftID).FirstOrDefault();
                                    row["Data" + date.Day] = shift == null ? "" : shift.Code;
                                }
                                else if (date.DayOfWeek == DayOfWeek.Wednesday)
                                {
                                    var shift = listShift.Where(d => d.ID == attRoster.WedShiftID).FirstOrDefault();
                                    row["Data" + date.Day] = shift == null ? "" : shift.Code;
                                }
                                else if (date.DayOfWeek == DayOfWeek.Thursday)
                                {
                                    var shift = listShift.Where(d => d.ID == attRoster.ThuShiftID).FirstOrDefault();
                                    row["Data" + date.Day] = shift == null ? "" : shift.Code;
                                }
                                else if (date.DayOfWeek == DayOfWeek.Friday)
                                {
                                    var shift = listShift.Where(d => d.ID == attRoster.FriShiftID).FirstOrDefault();
                                    row["Data" + date.Day] = shift == null ? "" : shift.Code;
                                }
                                else if (date.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    var shift = listShift.Where(d => d.ID == attRoster.SatShiftID).FirstOrDefault();
                                    row["Data" + date.Day] = shift == null ? "" : shift.Code;
                                }
                                else if (date.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    var shift = listShift.Where(d => d.ID == attRoster.SunShiftID).FirstOrDefault();
                                    row["Data" + date.Day] = shift == null ? "" : shift.Code;
                                }
                            }
                            #endregion

                        }
                    }
                    #endregion



                    table.Rows.Add(row);
                }
                if (_onlyHolydayNotHaveRoster)
                {
                    DataTable tblData = CreateReportMonthlyRosterSchema(listShift);
                    List<Cat_DayOff> dayOffs = repoCat_DayOff.FindBy(s => s.IsDelete == null && s.DateOff.Year == DateStart.Year).ToList();
                    foreach (DataRow dataRow in table.Rows)
                    {
                        foreach (Cat_DayOff catDayOff in dayOffs)
                        {
                            if (dataRow["Data" + catDayOff.DateOff.Day].IsNullOrEmpty())
                            {
                                DataRow row = tblData.NewRow();
                                foreach (DataColumn dataColumn in table.Columns)
                                {
                                    row[dataColumn.ColumnName] = dataRow[dataColumn.ColumnName];
                                }
                                for (int j = 1; j <= 31; j++)
                                {
                                    row["Data" + j] = "";
                                }
                                row["Data" + catDayOff.DateOff.Day] = "HLD";
                                table.Rows.Add(row);
                                break;
                            }
                        }
                    }
                    table = tblData;
                }
                DataTable table1 = CreateReportMonthlyRosterSchema(listShift);
                foreach (DataRow dataRow in table.Rows)
                {
                    DataRow row = table1.NewRow();
                    bool isCheck = false;

                    foreach (DataColumn dataColumn in table.Columns)
                    {
                        if (dataColumn.ColumnName.IndexOf("Data") > -1 && !isCheck)
                        {
                            int i = 1;

                            for (DateTime date = DateStart; date <= DateEnd; date = date.AddDays(1))
                            {
                                if (table1.Columns.Contains("Data" + i))
                                {
                                    row["Data" + i] = dataRow["Data" + date.Day];
                                    row["Header" + i] = dataRow["Header" + date.Day];
                                }

                                i++;
                            }
                            isCheck = true;
                        }
                        else if (dataColumn.ColumnName.IndexOf("Data") == -1)
                        {
                            row[dataColumn.ColumnName] = dataRow[dataColumn.ColumnName];
                        }
                    }
                    table1.Rows.Add(row);
                }
                return table1;
            }

        }
        #endregion

        #region BC Thống Kê Làm Thêm Năm  - 20140926
        DataTable CreateReportStatisticsOvertimeByYearSchema()
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                DataTable tb = new DataTable("Att_ReportStatisticsOvertimeByYearEntity");
                tb.Columns.Add(Att_ReportStatisticsOvertimeByYearEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Att_ReportStatisticsOvertimeByYearEntity.FieldNames.ProfileName);
                tb.Columns.Add(Att_ReportStatisticsOvertimeByYearEntity.FieldNames.CodeOrg);
                tb.Columns.Add(Att_ReportStatisticsOvertimeByYearEntity.FieldNames.CodeJobtitle);
                tb.Columns.Add(Att_ReportStatisticsOvertimeByYearEntity.FieldNames.CodePosition);
                tb.Columns.Add(Att_ReportStatisticsOvertimeByYearEntity.FieldNames.SumHourCO, typeof(double));
                for (int i = 1; i <= 12; i++)
                {
                    tb.Columns.Add("M" + i, typeof(double));
                }
                tb.Columns.Add(Att_ReportStatisticsOvertimeByYearEntity.FieldNames.CodeBranch);
                tb.Columns.Add(Att_ReportStatisticsOvertimeByYearEntity.FieldNames.CodeTeam);
                tb.Columns.Add(Att_ReportStatisticsOvertimeByYearEntity.FieldNames.CodeSection);
                tb.Columns.Add(Att_ReportStatisticsOvertimeByYearEntity.FieldNames.BranchName);
                tb.Columns.Add(Att_ReportStatisticsOvertimeByYearEntity.FieldNames.OrgName);
                tb.Columns.Add(Att_ReportStatisticsOvertimeByYearEntity.FieldNames.TeamName);
                tb.Columns.Add(Att_ReportStatisticsOvertimeByYearEntity.FieldNames.SectionName);
                tb.Columns.Add(Att_ReportStatisticsOvertimeByYearEntity.FieldNames.DateFrom, typeof(DateTime));
                tb.Columns.Add(Att_ReportStatisticsOvertimeByYearEntity.FieldNames.DateTo, typeof(DateTime));
                tb.Columns.Add(Att_ReportStatisticsOvertimeByYearEntity.FieldNames.UserExport);
                tb.Columns.Add(Att_ReportStatisticsOvertimeByYearEntity.FieldNames.DateExport, typeof(DateTime));
                return tb;
            }
        }
        public DataTable GetReportStatisticsOvertimeByYear(int Year, List<Hre_ProfileEntity> profiles, List<Guid?> lstOvertimeTypeIds, bool isNotAllowZero, bool isIncludeQuitEmp, string userExport, string UserLogin)
        {
            DataTable table = CreateReportStatisticsOvertimeByYearSchema();
            using (var context = new VnrHrmDataContext())
            {
                //int year = Year.Year;
                int year = Year;
                DateTime dateFrom = new DateTime(Year, 1, 1);
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                DateTime dateStart = new DateTime(year, 1, 1);
                DateTime dateEnd = new DateTime(year, 12, DateTime.DaysInMonth(year, 12)).AddDays(1).AddMinutes(-1);
                //DateTime dateEnd = new DateTime(year, 12, DateTime.DaysInMonth(year, 12));
                string key = OverTimeStatus.E_APPROVED.ToString();
                //  var repoAtt_Overtime = new Att_OvertimeRepository(unitOfWork);
                var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                // var repoCat_LeaveDayType = new Cat_LeaveDayTypeRepository(unitOfWork);
                List<object> para = new List<object>();
                para.AddRange(new object[3]);
                para[0] = null;
                para[1] = dateStart;
                para[2] = dateEnd;
                var basevices = new BaseService();
                string status = string.Empty;
                var overtimes = basevices.GetData<Att_OvertimeEntity>(para, ConstantSql.hrm_att_getdata_Overtime, UserLogin, ref status).Where(s => s.Status == key).ToList();

                //var overtimes = repoAtt_Overtime.FindBy(s => s.IsDelete == null && s.Status == key && dateStart <= s.WorkDate && s.WorkDate <= dateEnd)
                // .Select(s => new { s.ProfileID, s.ApproveHours, s.WorkDate, s.OvertimeTypeID }).ToList();

                if (isNotAllowZero)
                {
                    overtimes = overtimes.Where(s => s.ApproveHours > 0).ToList();
                }

                var profileIds = overtimes.Select(s => s.ProfileID).Distinct().ToList();
                //  var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                //var profiles = repoHre_Profile.FindBy(s => s.IsDelete == null && profileIds.Contains(s.ID))
                // .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
                profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();

                //Guid TypeCoLeaveID = EntityService.CreateQueryable<Cat_LeaveDayType>(false, GuidContext, Guid.Empty, m => m.IsTimeOffInLieu == true).Select(m => m.ID).FirstOrDefault();
                Guid TypeCoLeaveID = repoCat_LeaveDayType.FindBy(s => s.IsDelete == null && s.IsTimeOffInLieu == true).Select(s => s.ID).FirstOrDefault();
                var lstLeaveDay = basevices.GetData<Att_LeaveDayEntity>(para, ConstantSql.hrm_att_getdata_LeaveDay, UserLogin, ref status).Where(s => s.LeaveDayTypeID == TypeCoLeaveID && profileIds.Contains(s.ProfileID)).ToList();
                //var lstLeaveDay = EntityService.CreateQueryable<Att_LeaveDay>(false, GuidContext, Guid.Empty, m => m.LeaveDayTypeID == TypeCoLeaveID && m.DateStart <= dateEnd && m.DateEnd >= dateStart && profileIds.Contains(m.ProfileID))
                //    .Select(m => new { m.ID, m.ProfileID, m.LeaveHours, m.TotalDuration }).Execute();

                //    var repoTimeOffInLieu = new Att_TimeOffInLieuRepository(unitOfWork);
                //    var timeOfInLieus = repoTimeOffInLieu.FindBy(s => s.IsDelete == null && profileIds.Contains(s.ID) && s.OvertimeID != null && dateStart <= s.Date && s.Date <= dateEnd)
                //.Select(s => new { s.AccumulateLeaves, s.ProfileID }).ToList();

                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null && s.IsDelete == null).ToList();

                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var positions = repoCat_Position.FindBy(s => s.Code != null && s.IsDelete == null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();

                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null && s.IsDelete == null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();

                var repoCat_OvertimeType = new Cat_OvertimeTypeRepository(unitOfWork);
                var overtimeTypes = repoCat_OvertimeType.FindBy(s => s.Code != null && s.IsDelete == null).Select(s => new { s.ID, s.Code }).Distinct().ToList();
                if (lstOvertimeTypeIds != null && lstOvertimeTypeIds[0] != Guid.Empty && lstOvertimeTypeIds[0] != null)
                {
                    overtimes = overtimes.Where(s => lstOvertimeTypeIds.Contains(s.OvertimeTypeID)).ToList();
                    profileIds = overtimes.Select(s => s.ProfileID).Distinct().ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }
                //if (lstProfileIDs.Any() && lstProfileIDs[0] != Guid.Empty && lstProfileIDs[0] != null)
                //{
                //    profiles = profiles.Where(s => lstProfileIDs.Contains(s.ID)).ToList();
                //}
                if (isNotAllowZero)
                {
                    profileIds = overtimes.Select(s => s.ProfileID).Distinct().ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }
                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateStart).ToList();
                }

                foreach (var profile in profiles)
                {
                    DataRow row = table.NewRow();
                    Guid? orgId = profile.OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.CodeBranch] = orgBranch != null ? orgBranch.Code : string.Empty;
                    row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.CodeOrg] = orgOrg != null ? orgOrg.Code : string.Empty;
                    row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.CodeTeam] = orgTeam != null ? orgTeam.Code : string.Empty;
                    row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.CodeSection] = orgSection != null ? orgSection.Code : string.Empty;
                    row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.OrgName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                    row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.ProfileName] = profile.ProfileName;
                    row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.DateExport] = DateTime.Now;
                    row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.UserExport] = userExport;
                    var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                    var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
                    row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.ProfileName] = profile.ProfileName;
                    row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.CodePosition] = positon != null ? positon.Code : string.Empty;
                    row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.CodeJobtitle] = jobtitle != null ? jobtitle.Code : string.Empty;

                    //var timeOffInLieuProfiles = timeOfInLieus.Where(s => s.ProfileID == profile.ID).ToList();
                    //var sumHour = timeOffInLieuProfiles.Sum(s => s.AccumulateLeaves);

                    //if (sumHour > 0)
                    //    row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.SumHourCO] = sumHour;
                    var lstLeaveDayByProfile = lstLeaveDay.Where(m => m.ProfileID == profile.ID).ToList();
                    if (lstLeaveDayByProfile.Count > 0)
                    {
                        row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.SumHourCO] = lstLeaveDayByProfile.Sum(m => m.LeaveHours * (m.LeaveDays ?? 0));
                    }
                    for (int i = 1; i <= 12; i++)
                    {
                        dateStart = new DateTime(year, i, 1);
                        dateEnd = new DateTime(year, i, DateTime.DaysInMonth(year, i));
                        // row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.DateFrom] = Year;
                        row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.DateFrom] = dateFrom;
                        row[Att_ReportStatisticsOvertimeByYearEntity.FieldNames.DateTo] = dateEnd;
                        var month = overtimes.Where(s => s.ProfileID == profile.ID && dateStart <= s.WorkDate && s.WorkDate <= dateEnd).Sum(s => s.ApproveHours);
                        if (month > 0)
                            row["M" + i] = month;
                    }
                    table.Rows.Add(row);
                }
                return table;
            }
        }
        #endregion

        #region BC Cảnh Báo Tăng Ca
        DataTable getSchema_ReportWarringOvertimeList()
        {
            DataTable tblData = new DataTable("Att_ReportWarringOvertimeEntity");
            tblData.Columns.Add(Att_ReportWarringOvertimeEntity.FieldNames.CodeEmp, typeof(String));
            tblData.Columns.Add(Att_ReportWarringOvertimeEntity.FieldNames.ProfileName, typeof(String));
            tblData.Columns.Add(Att_ReportWarringOvertimeEntity.FieldNames.OrgStructureCode, typeof(String));
            tblData.Columns.Add(Att_ReportWarringOvertimeEntity.FieldNames.OrgStructureName, typeof(String));
            tblData.Columns.Add(Att_ReportWarringOvertimeEntity.FieldNames.JobTitleCode, typeof(String));
            tblData.Columns.Add(Att_ReportWarringOvertimeEntity.FieldNames.JobTitleName, typeof(String));
            tblData.Columns.Add(Att_ReportWarringOvertimeEntity.FieldNames.PositionName, typeof(String));
            tblData.Columns.Add(Att_ReportWarringOvertimeEntity.FieldNames.PositionCode, typeof(String));
            tblData.Columns.Add(Att_ReportWarringOvertimeEntity.FieldNames.DateStart, typeof(DateTime));
            tblData.Columns.Add(Att_ReportWarringOvertimeEntity.FieldNames.DateEnd, typeof(DateTime));
            tblData.Columns.Add(Att_ReportWarringOvertimeEntity.FieldNames.HourOverTime, typeof(double));
            tblData.Columns.Add(Att_ReportWarringOvertimeEntity.FieldNames.Color, typeof(String));
            tblData.Columns.Add(Att_ReportWarringOvertimeEntity.FieldNames.UserExport);
            tblData.Columns.Add(Att_ReportWarringOvertimeEntity.FieldNames.DateExport, typeof(DateTime));
            return tblData;
        }

        public DataTable GetReportWarringOvertimeList(DateTime DateStart, DateTime DateEnd, string OrgStructureTree, string Type, string userExport, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                var repoCat_Position = new CustomBaseRepository<Cat_Position>(unitOfWork);
                var repoAtt_Overtime = new CustomBaseRepository<Att_Overtime>(unitOfWork);

                var status = string.Empty;
                DateEnd = DateEnd.Date.AddDays(1).AddMinutes(-1);
                DataTable tblData = getSchema_ReportWarringOvertimeList();

                #region get data
                string statusOvertimeInConfig = Att_OvertimeServices.getStatusOTApplyCalcPayroll(UserLogin); //OverTimeStatus.E_APPROVED.ToString(); //Lấy từ trong cấu hình lương ra loại của trang thái overtime
                List<Hre_ProfileEntity> lstProfile = new List<Hre_ProfileEntity>();

                List<object> lstOrgIDs = new List<object>();
                lstOrgIDs.AddRange(new object[3]);
                lstOrgIDs[0] = (object)OrgStructureTree;
                lstOrgIDs[1] = null;
                lstOrgIDs[2] = null;
                lstProfile = GetData<Hre_ProfileEntity>(lstOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status);
                //if (OrgStructureTree.SelectedValues.Count == 0)
                //{
                //    //lấy hết profile
                //    lstProfile = EntityService.GetEntityList<Hre_Profile>(true, GuidContext, LoginUserID.Value, m => m.DateQuit == null || (m.DateQuit != null && m.DateQuit > DateStart)).ToList();
                //}
                //else
                //{
                //    //lây profile Dựa theo phòng ban 
                //    lstProfile = EntityService.GetEntityList<Hre_Profile>(true, GuidContext, LoginUserID.Value, m => m.OrgStructureID != null
                //        && OrgStructureTree.SelectedValues.Contains(m.OrgStructureID ?? Guid.Empty)
                //        && (m.DateQuit == null || (m.DateQuit != null && m.DateQuit > DateStart))).ToList();
                //}

                #region Logic Đăc Thù Cho TMI
                //Logic Đăc Thù Cho TMI
                //if (Common.ClientID == ClienID.E_TMI.GetHashCode())
                //{
                //    //Loại bỏ nhân viên thuộc nhóm phòng ban là bảo vệ
                //    string BAOVE = "BAOVE";
                //    List<Guid> lstOrgGuard = EntityService.CreateQueryable<Cat_OrgStructure>(false, GuidContext, Guid.Empty, m => m.OrgGroupName == BAOVE).Select(m => m.ID).ToList<Guid>();
                //    lstProfile = lstProfile.Where(m => !lstOrgGuard.Contains(m.OrgStructureID ?? Guid.Empty)).ToList();
                //}
                #endregion

                List<Guid> lstProfileIDs = lstProfile.Select(m => m.ID).ToList();

                //DS phòng ban sort theo 2 cấp độ 1 là phòng ban 2 là tên nhân viên
                List<Cat_OrgStructure> lstOrgStructure = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                List<Cat_JobTitle> lstJobtitle = repoCat_JobTitle.FindBy(s => s.Code != null).ToList();
                List<Cat_Position> lstPosition = repoCat_Position.FindBy(s => s.Code != null).ToList();


                List<Att_Overtime> lstOvertime = repoAtt_Overtime.FindBy(m => m.IsDelete == null && m.Status == statusOvertimeInConfig
                        && m.WorkDate >= DateStart && m.WorkDate <= DateEnd).ToList();
                lstOvertime = lstOvertime.Where(m => lstProfileIDs.Contains(m.ProfileID)).ToList();

                //string type = AppConfig.E_CONFIG_WARNING_OVERTIME.ToString();
                //Sys_AppConfig Config = EntityService.GetEntity<Sys_AppConfig>(EntityService.GuidMainContext, LoginUserID.Value, ui => ui.Info == type);

                OvertimePermitEntity Config = Att_OvertimeServices.getOvertimePermit(UserLogin);
                if (Config == null)
                {
                    //Common.MessageBoxs(Messages.Msg, LanguageManager.GetString(Messages.PlsConfigWarningOvertime), Ext.Net.MessageBox.Icon.INFO, string.Empty);
                    return tblData;
                }
                //Tuần
                double num1 = 0.5; // các số trong cấu hình để xép màu
                double num2 = 0;
                double num3 = 0;
                double num4 = 0;
                double num5 = 0;
                //Tháng
                double num6 = 0.5; // các số trong cấu hình để xép màu
                double num7 = 0;
                double num8 = 0;
                double num9 = 0;
                double num10 = 0;
                //Năm
                double num11 = 0.5; // các số trong cấu hình để xép màu
                double num12 = 0;
                double num13 = 0;
                double num14 = 0;
                double num15 = 0;
                try
                {
                    num1 = 0.5;
                    if (Config.limitHour_ByWeek != null)
                    {
                        num2 = Config.limitHour_ByWeek.Value;
                    }
                    if (Config.limitHour_ByWeek_Lev1 != null)
                    {
                        num3 = Config.limitHour_ByWeek_Lev1.Value;
                    }
                    if (Config.limitHour_ByWeek_Lev2 != null)
                    {
                        num4 = Config.limitHour_ByWeek_Lev2.Value;
                    }
                    num5 = double.MaxValue - 1;

                    num6 = 0.5;
                    if (Config.limitHour_ByMonth != null)
                    {
                        num7 = Config.limitHour_ByMonth.Value;
                    }
                    if (Config.limitHour_ByMonth_Lev1 != null)
                    {
                        num8 = Config.limitHour_ByMonth_Lev1.Value;
                    }
                    if (Config.limitHour_ByMonth_Lev2 != null)
                    {
                        num9 = Config.limitHour_ByMonth_Lev2.Value;
                    }
                    num10 = double.MaxValue - 1;

                    num11 = 0.5;
                    if (Config.limitHour_ByYear != null)
                    {
                        num12 = Config.limitHour_ByYear.Value;
                    }
                    if (Config.limitHour_ByYear_Lev1 != null)
                    {
                        num13 = Config.limitHour_ByYear_Lev1.Value;
                    }
                    if (Config.limitHour_ByYear_Lev2 != null)
                    {
                        num14 = Config.limitHour_ByYear_Lev2.Value;
                    }
                    num15 = double.MaxValue - 1;

                }
                catch
                {
                    //Common.MessageBoxs(Messages.Msg, LanguageManager.GetString(Messages.PlsConfigWarningOvertime), Ext.Net.MessageBox.Icon.INFO, string.Empty);
                    return tblData;
                }

                #endregion

                #region process
                foreach (var profile in lstProfile)
                {
                    double hourOvertime = 0;
                    bool isAddRow = false;
                    ConsoleColor color = ConsoleColor.Black;

                    if (statusOvertimeInConfig == OverTimeStatus.E_APPROVED.ToString()) // lấy giờ theo duyệt
                    {
                        hourOvertime = lstOvertime.Where(m => m.ProfileID == profile.ID).Sum(m => m.ApproveHours ?? 0);
                    }
                    else // lấy giờ theo xác nhận
                    {
                        hourOvertime = lstOvertime.Where(m => m.ProfileID == profile.ID).Sum(m => m.ConfirmHours);
                    }

                    //Có được giờ rồi thì phân chia ra 3 loại theo tuần tháng và năm để mà kiểm tra xem là có bị cảnh báo hay ko?
                    if (Type == "OnWeek")// theo tuần
                    {
                        if (hourOvertime >= num1 && num1 != 0) // tạo record
                        {
                            isAddRow = true;
                        }
                        if (hourOvertime >= num1 && hourOvertime < num2)
                        {
                            color = ConsoleColor.Yellow;
                        }
                        else if (hourOvertime >= num3 && hourOvertime < num4)
                        {
                            color = ConsoleColor.DarkYellow;
                        }
                        else if (hourOvertime >= num5)
                        {
                            color = ConsoleColor.Red;
                        }

                    }
                    else if (Type == "OnMonth") // theo tháng
                    {
                        if (hourOvertime >= num6 && num6 != 0) // tạo record
                        {
                            isAddRow = true;
                        }
                        if (hourOvertime >= num6 && hourOvertime < num7)
                        {
                            color = ConsoleColor.Yellow;
                        }
                        else if (hourOvertime >= num8 && hourOvertime < num9)
                        {
                            color = ConsoleColor.DarkYellow;
                        }
                        else if (hourOvertime >= num10)
                        {
                            color = ConsoleColor.Red;
                        }
                    }
                    else // theo năm
                    {
                        if (hourOvertime >= num11 && num11 != 0) // tạo record
                        {
                            isAddRow = true;
                        }
                        if (hourOvertime >= num11 && hourOvertime < num12)
                        {
                            color = ConsoleColor.Yellow;
                        }
                        else if (hourOvertime >= num13 && hourOvertime < num14)
                        {
                            color = ConsoleColor.DarkYellow;
                        }
                        else if (hourOvertime >= num15)
                        {
                            color = ConsoleColor.Red;
                        }
                    }

                    if (isAddRow) //nếu tạo row
                    {
                        DataRow dr = tblData.NewRow();

                        dr[Att_ReportWarringOvertimeEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                        dr[Att_ReportWarringOvertimeEntity.FieldNames.ProfileName] = profile.ProfileName;

                        if (profile.OrgStructureID != null)
                        {
                            Cat_OrgStructure org = lstOrgStructure.Where(m => m.ID == profile.OrgStructureID).FirstOrDefault();
                            if (org != null)
                            {
                                dr[Att_ReportWarringOvertimeEntity.FieldNames.OrgStructureCode] = org.OrgStructureName;
                                dr[Att_ReportWarringOvertimeEntity.FieldNames.OrgStructureName] = org.Code;
                            }

                        }
                        if (profile.JobTitleID != null)
                        {
                            Cat_JobTitle Jobtitle = lstJobtitle.Where(m => m.ID == profile.JobTitleID).FirstOrDefault();
                            if (Jobtitle != null)
                            {
                                dr[Att_ReportWarringOvertimeEntity.FieldNames.JobTitleName] = Jobtitle.JobTitleName;
                                dr[Att_ReportWarringOvertimeEntity.FieldNames.JobTitleCode] = Jobtitle.Code;
                            }

                        }
                        if (profile.PositionID != null)
                        {
                            Cat_Position Position = lstPosition.Where(m => m.ID == profile.PositionID).FirstOrDefault();
                            if (Position != null)
                            {
                                dr[Att_ReportWarringOvertimeEntity.FieldNames.PositionName] = Position.PositionName;
                                dr[Att_ReportWarringOvertimeEntity.FieldNames.PositionCode] = Position.Code;
                            }

                        }
                        dr[Att_ReportWarringOvertimeEntity.FieldNames.DateStart] = DateStart;
                        dr[Att_ReportWarringOvertimeEntity.FieldNames.DateEnd] = DateEnd;
                        dr[Att_ReportWarringOvertimeEntity.FieldNames.HourOverTime] = hourOvertime;
                        dr[Att_ReportWarringOvertimeEntity.FieldNames.UserExport] = userExport;
                        dr[Att_ReportWarringOvertimeEntity.FieldNames.DateExport] = DateTime.Today;
                        dr[Att_ReportWarringOvertimeEntity.FieldNames.Color] = color;
                        //dr = ChangeColor(dr, color);
                        tblData.Rows.Add(dr);
                    }
                }
                #endregion

                return tblData;
            }
        }
        #endregion

        #endregion

        #region Thống Kê Ca Làm Việc
        DataTable GetSchema_SumaryShiftMonth()
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);

                //DataTable tb = new DataTable();
                //tb.Columns.Add("CodeEmp");
                //tb.Columns.Add("ProfileName");
                //tb.Columns.Add("BranchCode");
                //tb.Columns.Add("OrgStructureCode");
                //tb.Columns.Add("TeamCode");
                //tb.Columns.Add("SectionCode");
                //tb.Columns.Add("JobtitleCode");
                //tb.Columns.Add("PositionCode");
                //tb.Columns.Add("BranchName");
                //tb.Columns.Add("OrgStructureName");
                //tb.Columns.Add("TeamName");
                //tb.Columns.Add("SectionName");
                //tb.Columns.Add("DateExport", typeof(DateTime));
                //tb.Columns.Add("DateFrom", typeof(DateTime));
                //tb.Columns.Add("DateTo", typeof(DateTime));
                //tb.Columns.Add("UserExport");

                string status = string.Empty;
                DataTable tb = new DataTable("Att_ReportSumaryShiftMonthEntity");
                tb.Columns.Add(Att_ReportSumaryShiftMonthEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Att_ReportSumaryShiftMonthEntity.FieldNames.ProfileName);
                tb.Columns.Add(Att_ReportSumaryShiftMonthEntity.FieldNames.BranchCode);
                tb.Columns.Add(Att_ReportSumaryShiftMonthEntity.FieldNames.OrgStructureCode);
                tb.Columns.Add(Att_ReportSumaryShiftMonthEntity.FieldNames.TeamCode);
                tb.Columns.Add(Att_ReportSumaryShiftMonthEntity.FieldNames.SectionCode);
                tb.Columns.Add(Att_ReportSumaryShiftMonthEntity.FieldNames.JobtitleCode);
                tb.Columns.Add(Att_ReportSumaryShiftMonthEntity.FieldNames.PositionCode);
                tb.Columns.Add(Att_ReportSumaryShiftMonthEntity.FieldNames.BranchName);
                tb.Columns.Add(Att_ReportSumaryShiftMonthEntity.FieldNames.OrgStructureName);
                tb.Columns.Add(Att_ReportSumaryShiftMonthEntity.FieldNames.TeamName);
                tb.Columns.Add(Att_ReportSumaryShiftMonthEntity.FieldNames.SectionName);
                tb.Columns.Add(Att_ReportSumaryShiftMonthEntity.FieldNames.UserExport);
                tb.Columns.Add(Att_ReportSumaryShiftMonthEntity.FieldNames.DateExport, typeof(DateTime));
                tb.Columns.Add(Att_ReportSumaryShiftMonthEntity.FieldNames.DateFrom, typeof(DateTime));
                tb.Columns.Add(Att_ReportSumaryShiftMonthEntity.FieldNames.DateTo, typeof(DateTime));

                var codes = repoCat_Shift.FindBy(s => s.Code != null).Select(s => s.Code).Distinct().ToList<string>();
                foreach (string code in codes)
                {
                    if (!tb.Columns.Contains(code))
                    {
                        tb.Columns.Add(code, typeof(double));
                    }
                }
                return tb;
            }
        }

        public DataTable GetReportSumaryShiftMonthList(DateTime dateStart, DateTime dateEnd, string OrgStructureTree, string ProfileDetail, string strShift, bool isNotAllowZero, bool isIncludeQuitEmp, string UserLogin)
        {
            string status = string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_Position = new CustomBaseRepository<Cat_Position>(unitOfWork);
                var repoCat_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);

                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                List<Guid> orgIDs = new List<Guid>();
                if (OrgStructureTree != null)
                {
                    orgIDs = OrgStructureTree.Split(',').Select(s => Guid.Parse(s)).ToList();
                }
                List<Guid> shiftIDs = new List<Guid>();
                if (strShift != null)
                {
                    shiftIDs = strShift.Split(',').Select(s => Guid.Parse(s)).ToList();
                }

                List<object> lst3ParamSE = new List<object>();
                lst3ParamSE.Add(null);
                lst3ParamSE.Add(dateStart);
                lst3ParamSE.Add(dateEnd);
                var dataAtt_Workday = GetData<Att_Workday>(lst3ParamSE, ConstantSql.hrm_att_getdata_Workday, UserLogin, ref status).ToList();

                var workDays = dataAtt_Workday.Where(s => dateStart.Date <= s.WorkDate && s.WorkDate <= dateEnd.Date)
                   .Select(s => new { s.ProfileID, s.ShiftApprove, s.WorkDate }).ToList();
                var profileIds = workDays.Select(s => s.ProfileID).Distinct().ToList();

                #region xử lý lấy lstProfileIds theo OrgStructureID
                List<Hre_ProfileEntity> lstProfileIDs = new List<Hre_ProfileEntity>();
                List<object> lstParamPro = new List<object>();
                lstParamPro.Add(OrgStructureTree);
                lstParamPro.Add(null);
                lstParamPro.Add(null);
                if (ProfileDetail != null)
                {
                    List<Hre_ProfileEntity> _lstTemp = new List<Hre_ProfileEntity>();
                    List<Hre_ProfileEntity> _lstContains = new List<Hre_ProfileEntity>();
                    Hre_ProfileEntity proEntity = new Hre_ProfileEntity();

                    var lst = ProfileDetail.Split(',').Select(s => Guid.Parse(s)).ToList();
                    foreach (var item in lst)
                    {
                        proEntity = new Hre_ProfileEntity();
                        proEntity.ID = item;
                        _lstTemp.Add(proEntity);
                    }
                    if (OrgStructureTree != null)
                    {
                        lstProfileIDs = GetData<Hre_ProfileEntity>(lstParamPro, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status);
                        _lstContains = lstProfileIDs.Where(m => !_lstTemp.Contains(m)).ToList();

                        if (_lstContains.Count > 0)
                        {
                            string selectedIds = Common.DotNetToOracle(String.Join(",", _lstContains.Select(s => s.ID.ToString()).ToList<string>()));
                            _lstTemp = GetData<Hre_ProfileEntity>(selectedIds, ConstantSql.hrm_hr_sp_get_ProfileByIds, UserLogin, ref status);
                            lstProfileIDs.AddRange(_lstTemp);
                        }
                    }
                    else
                    {
                        string selectedIds = Common.DotNetToOracle(ProfileDetail);
                        lstProfileIDs = GetData<Hre_ProfileEntity>(selectedIds, ConstantSql.hrm_hr_sp_get_ProfileByIds, UserLogin, ref status);
                    }
                }
                else
                {
                    lstProfileIDs = GetData<Hre_ProfileEntity>(lstParamPro, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status);
                }
                #endregion


                var profiles = lstProfileIDs.Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                var positions = repoCat_Position.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code }).ToList();
                var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code }).ToList();
                var shifts = repoCat_Shift.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code }).ToList();
                //var codeEmp = listFilterInfo.GetFilterValue1<string>(Hre_Profile.FieldNames.CodeEmp);
                if (orgIDs.Count > 0)
                {
                    profiles = profiles.Where(s => s.OrgStructureID != null && orgIDs.Contains(s.OrgStructureID.Value)).ToList();
                }
                if (shiftIDs.Count > 0)
                {
                    workDays = workDays.Where(s => s.ShiftApprove.HasValue && shiftIDs.Contains(s.ShiftApprove.Value)).ToList();
                    profileIds = workDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }
                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                }
                //if (codeEmp.IsNotNullOrEmpty())
                //{
                //    profiles = profiles.Where(s => s.CodeEmp == codeEmp).ToList();
                //}
                profileIds = profiles.Select(s => s.ID).ToList();
                workDays = workDays.Where(s => profileIds.Contains(s.ProfileID)).ToList();

                var dataAtt_LeaveDay = GetData<Att_LeaveDayEntity>(lst3ParamSE, ConstantSql.hrm_att_getdata_LeaveDay_Inner, UserLogin, ref status).ToList();
                var leaveDays = dataAtt_LeaveDay.Where(s => profileIds.Contains(s.ProfileID) && dateStart <= s.DateEnd && s.DateStart <= dateEnd)
                    .Select(s => new { s.ProfileID, s.DateStart, s.DateEnd }).ToList();
                DataTable table = GetSchema_SumaryShiftMonth();
                var codes = repoCat_Shift.FindBy(s => s.Code != null).Select(s => s.Code).Distinct().ToList<string>();
                if (isNotAllowZero)
                {
                    var profileIDs = workDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIDs.Contains(s.ID)).ToList();
                }
                foreach (var profile in profiles)
                {
                    var workDayProfiles = workDays.Where(s => s.ProfileID == profile.ID).ToList();
                    var leaveDayProfiles = leaveDays.Where(s => s.ProfileID == profile.ID).ToList();
                    List<DateTime> dateLeavdays = new List<DateTime>();
                    foreach (var leaveDayProfile in leaveDayProfiles)
                    {
                        for (DateTime date = leaveDayProfile.DateStart; date <= leaveDayProfile.DateEnd; date = date.AddDays(1))
                        {
                            dateLeavdays.Add(date);
                        }
                    }
                    workDayProfiles = workDayProfiles.Where(s => !dateLeavdays.Contains(s.WorkDate)).ToList();
                    DataRow row = table.NewRow();

                    Guid? orgId = profile.OrgStructureID;
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);

                    row[Att_ReportSumaryShiftMonthEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                    row[Att_ReportSumaryShiftMonthEntity.FieldNames.OrgStructureCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                    row[Att_ReportSumaryShiftMonthEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                    row[Att_ReportSumaryShiftMonthEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                    row[Att_ReportSumaryShiftMonthEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    row[Att_ReportSumaryShiftMonthEntity.FieldNames.OrgStructureName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    row[Att_ReportSumaryShiftMonthEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    row[Att_ReportSumaryShiftMonthEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                    var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                    var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
                    row[Att_ReportSumaryShiftMonthEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    row[Att_ReportSumaryShiftMonthEntity.FieldNames.ProfileName] = profile.ProfileName;
                    row[Att_ReportSumaryShiftMonthEntity.FieldNames.PositionCode] = positon != null ? positon.Code : string.Empty;
                    row[Att_ReportSumaryShiftMonthEntity.FieldNames.JobtitleCode] = jobtitle != null ? jobtitle.Code : string.Empty;
                    row[Att_ReportSumaryShiftMonthEntity.FieldNames.DateExport] = DateTime.Now;
                    row[Att_ReportSumaryShiftMonthEntity.FieldNames.DateFrom] = dateStart;
                    row[Att_ReportSumaryShiftMonthEntity.FieldNames.DateTo] = dateEnd;
                    //row["UserExport"] = Session[SessionObjects.UserLogin];
                    foreach (string code in codes)
                    {
                        var shift = shifts.FirstOrDefault(s => s.Code == code);
                        var sum = workDayProfiles.Count(s => s.ShiftApprove == shift.ID);
                        row[code] = sum > 0 ? (object)sum : DBNull.Value;
                    }
                    table.Rows.Add(row);
                }
                var configs = new Dictionary<string, Dictionary<string, object>>();
                var config90 = new Dictionary<string, object>();
                var configdate = new Dictionary<string, object>();
                configdate.Add("format", "{0:dd/MM/yyyy}");
                configdate.Add("width", 110);
                config90.Add("width", 90);
                if (!configs.ContainsKey("CodeEmp"))
                    configs.Add("CodeEmp", config90);
                if (!configs.ContainsKey("TeamCode"))
                    configs.Add("TeamCode", config90);
                if (!configs.ContainsKey("SectionCode"))
                    configs.Add("SectionCode", config90);
                if (!configs.ContainsKey("BranchName"))
                    configs.Add("BranchName", config90);
                if (!configs.ContainsKey("OrgStructureName"))
                    configs.Add("OrgStructureName", config90);
                if (!configs.ContainsKey("TeamName"))
                    configs.Add("TeamName", config90);
                if (!configs.ContainsKey("SectionName"))
                    configs.Add("SectionName", config90);

                var config110 = new Dictionary<string, object>();
                config110.Add("width", 110);
                if (!configs.ContainsKey("BranchCode"))
                    configs.Add("BranchCode", config110);
                if (!configs.ContainsKey("OrgStructureCode"))
                    configs.Add("OrgStructureCode", config110);
                if (!configs.ContainsKey("JobtitleCode"))
                    configs.Add("JobtitleCode", config110);
                if (!configs.ContainsKey("PositionCode"))
                    configs.Add("PositionCode", config110);
                //configs.Add("DateExport", config110);
                //configs.Add("DateFrom", config110);
                //configs.Add("DateTo", config110);
                if (!configs.ContainsKey("UserExport"))
                    configs.Add("UserExport", config110);
                if (!configs.ContainsKey("DateExport"))
                    configs.Add("DateExport", configdate);
                if (!configs.ContainsKey("DateFrom"))
                    configs.Add("DateFrom", configdate);
                if (!configs.ContainsKey("DateTo"))
                    configs.Add("DateTo", configdate);

                return table.ConfigTable(configs);
            }
        }
        #endregion

        #region BC Chi Tiết Ca Làm Việc

        /// <summary>
        /// Khởi Tạo DataTable cho BC Ca Và Lịch Làm Việc
        /// </summary>
        /// <returns></returns>
        DataTable CreateReportDetailShiftSchema()
        {

            DataTable tb = new DataTable("Att_ReportDetailShiftEntity");
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.ProfileName);
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.PositionName);
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.JobtitleName);
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.GroupCode);
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.TeamCode);
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.BranchCode);
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.SectionCode);
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.DepartmentName);
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.DateFrom, typeof(DateTime));
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.DateTo, typeof(DateTime));
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.Division);
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.Date, typeof(DateTime));
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.ShiftName);
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.InTime, typeof(DateTime));
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.OutTime, typeof(DateTime));
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.Remark);
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.WorkDate);
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.DeptPath);
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.DateExport, typeof(DateTime));
            tb.Columns.Add(Att_ReportDetailShiftEntity.FieldNames.UserExport);
            return tb;
        }


        /// <summary>
        /// Lấy Dữ Liệu BC Chi Tiết Ca Làm Việc
        /// </summary>
        /// <returns></returns>
        public DataTable GetReportDetailShift(DateTime DateFrom, DateTime DateTo, List<Hre_ProfileEntity> profiles, string userExport, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                if (DateTo != null)
                    DateTo = DateTo.AddDays(1).AddMilliseconds(-1);
                DataTable table = CreateReportDetailShiftSchema();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                List<Guid> ProfileIDs = profiles.Select(s => s.ID).ToList();
                var repoAtt_Workday = new Att_WorkDayRepository(unitOfWork);
                var basevice = new BaseService();
                //var workDays = repoAtt_Workday.FindBy(s => s.IsDelete == null &&
                //  DateFrom <= s.WorkDate && s.WorkDate <= DateTo
                //  && s.ProfileID != null && ProfileIDs.Contains(s.ProfileID))
                //  .Select(s => new { s.ProfileID, s.ShiftID, s.ShiftApprove, s.WorkDate, s.InTime1, s.OutTime1,s.Note }).ToList();
                string status = string.Empty;
                List<object> para = new List<object>();
                para.AddRange(new object[3]);
                para[1] = DateFrom;
                para[2] = DateTo;

                var workDays = basevice.GetData<Att_WorkdayEntity>(para, ConstantSql.hrm_att_sp_getdata_reportWorDay, UserLogin, ref status).Where(s => ProfileIDs.Contains(s.ProfileID)).ToList();
                var profileIds = workDays.Select(s => s.ProfileID).Distinct().ToList();


                // var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                //var profiles = repoHre_Profile.FindBy(s => profileIds.Contains(s.ID) && s.IsDelete == null)
                //  .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
                profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();

                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                var shifts = repoCat_Shift.GetAll().ToList();

                List<Guid> guids = profiles.Select(s => s.ID).ToList();
                workDays = workDays.Where(s => guids.Contains(s.ProfileID)).ToList();

                foreach (var profile in profiles)
                {
                    for (DateTime date = DateFrom.Date; date <= DateTo; date = date.AddDays(1))
                    {
                        var workDayProfiles = workDays.Where(s => s.WorkDate.Date == date && s.ProfileID == profile.ID).ToList();
                        if (workDayProfiles.Count > 0)
                        {
                            DataRow row = table.NewRow();
                            Guid? orgId = profile.OrgStructureID;
                            var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                            var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                            var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                            var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);

                            row[Att_ReportDetailShiftEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                            row[Att_ReportDetailShiftEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.Code : string.Empty;
                            row[Att_ReportDetailShiftEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                            row[Att_ReportDetailShiftEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                            row[Att_ReportDetailShiftEntity.FieldNames.PositionName] = profile.PositionName;
                            row[Att_ReportDetailShiftEntity.FieldNames.JobtitleName] = profile.JobTitleName;
                            row[Att_ReportDetailShiftEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                            row[Att_ReportDetailShiftEntity.FieldNames.ProfileName] = profile.ProfileName;


                            var workDay1 = workDayProfiles.FirstOrDefault();
                            row[Att_ReportDetailShiftEntity.FieldNames.Date] = date;
                            row[Att_ReportDetailShiftEntity.FieldNames.DateFrom] = DateFrom.Date;
                            row[Att_ReportDetailShiftEntity.FieldNames.DateTo] = DateTo;
                            row[Att_ReportDetailShiftEntity.FieldNames.DateExport] = DateTime.Now;
                            row[Att_ReportDetailShiftEntity.FieldNames.UserExport] = userExport;
                            //    row[Att_ReportSumaryInOutEntity.FieldNames.UserExport] = Session[SessionObjects.UserLogin];
                            if (workDay1 != null)
                            {
                                Guid? ShiftID = workDay1.ShiftApprove ?? workDay1.ShiftID;
                                var shift = shifts.FirstOrDefault(s => s.ID == ShiftID);
                                row[Att_ReportDetailShiftEntity.FieldNames.ShiftName] = shift != null ? shift.ShiftName : string.Empty;
                                if (workDay1.InTime1 != null)
                                    row[Att_ReportDetailShiftEntity.FieldNames.InTime] = workDay1.InTime1.Value;
                                if (workDay1.OutTime1 != null)
                                    row[Att_ReportDetailShiftEntity.FieldNames.OutTime] = workDay1.OutTime1.Value;
                                row[Att_ReportDetailShiftEntity.FieldNames.WorkDate] = workDay1.WorkDate != null ? (DateTime?)workDay1.WorkDate : null;
                            }
                            row[Att_ReportDetailShiftEntity.FieldNames.Remark] = workDay1.Note;
                            table.Rows.Add(row);
                        }
                    }
                }

                return table;

            }
        }
        #region Code Cũ
        //public List<Att_ReportDetailShiftEntity> GetReportDetailShift(DateTime DateFrom, DateTime DateTo, List<Guid> lstProfileIDs)
        //{
        //    #region Logic Hiển thị Hiện nay
        //    //1. lấy theo profile, workday => shift=> shift.intime, shift.outtime
        //    //2. Phòng ban thì theo cái phòng ban loại DepartmentName và Division là của phòng ban hiện tại của nhân viên
        //    #endregion
        //    List<Att_ReportDetailShiftEntity> lstDetailShiftEntity = new List<Att_ReportDetailShiftEntity>();
        //    #region getData
        //    var lstWorkday = new List<Att_Workday>().Select(m => new { m.ProfileID, m.WorkDate, m.ShiftID, m.FirstInTime, m.LastOutTime }).ToList();
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_WorkDayRepository(unitOfWork);
        //        if (lstProfileIDs != null && lstProfileIDs.Count > 0)
        //        {
        //            lstWorkday = repo.FindBy(m => m.WorkDate >= DateFrom && m.WorkDate < DateTo
        //                && m.ProfileID != null && lstProfileIDs.Contains(m.ProfileID))
        //                    .Select(m => new { m.ProfileID, m.WorkDate, m.ShiftID, m.FirstInTime, m.LastOutTime }).ToList();
        //        }
        //        else
        //        {
        //            lstWorkday = repo.FindBy(m => m.WorkDate >= DateFrom && m.WorkDate < DateTo && lstProfileIDs.Contains(m.ProfileID))
        //                   .Select(m => new { m.ProfileID, m.WorkDate, m.ShiftID, m.FirstInTime, m.LastOutTime }).ToList();
        //        }

        //    }
        //    var lstProfile = new List<Hre_Profile>().Select(m => new { m.ID, m.OrgStructureID, m.CodeEmp, m.ProfileName }).ToList();
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Hre_ProfileRepository(unitOfWork);
        //        if (lstProfileIDs != null && lstProfileIDs.Count > 0)
        //        {
        //            lstProfile = repo.FindBy(m => lstProfileIDs.Contains(m.ID)).Select(m => new { m.ID, m.OrgStructureID, m.CodeEmp, m.ProfileName }).ToList();
        //        }
        //        else
        //        {
        //            lstProfile = repo.GetAll().Select(m => new { m.ID, m.OrgStructureID, m.CodeEmp, m.ProfileName }).ToList();
        //        }
        //    }
        //    List<Guid> lstProfileid = lstProfile.Select(m => m.ID).ToList();
        //    var lstShift = new List<Cat_Shift>().Select(m => new { m.ID, m.ShiftName, m.InTime, m.CoOut }).ToList();
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Cat_ShiftRepository(unitOfWork);
        //        lstShift = repo.GetAll().Select(m => new { m.ID, m.ShiftName, m.InTime, m.CoOut }).ToList();
        //    }

        //    #endregion
        //    #region get org nhieu cap
        //    List<OrgTiny> lstOrgAll = new List<OrgTiny>();
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repoOrg = new Cat_OrgStructureRepository(unitOfWork);
        //        lstOrgAll = repoOrg.GetAll().Select(m => new OrgTiny { ID = m.ID, OrgCode = m.Code, OrgName = m.OrgStructureName, ParentID = m.ParentID.Value, TypeID = m.TypeID }).ToList();
        //    }
        //    List<Guid?> lstOrgIDs = lstProfile.Select(m => m.OrgStructureID).Distinct().ToList();
        //    Dictionary<Guid?, OrgLevelTypeName> OrgNameAllLevel = Cat_OrgStructureServices.GetFullLinkOrg(lstOrgIDs, lstOrgAll);
        //    #endregion

        //    foreach (var ProfileID in lstProfileid)
        //    {
        //        if (ProfileID == null)
        //            continue;


        //        var lstWorkdayByProfile = lstWorkday.Where(m => m.ProfileID == ProfileID).ToList();
        //        foreach (var WorkdayByProfile in lstWorkdayByProfile)
        //        {
        //            if (WorkdayByProfile.WorkDate == null)
        //            {
        //                continue;
        //            }

        //            Att_ReportDetailShiftEntity ReportDetailShiftEntity = new Att_ReportDetailShiftEntity();
        //            var profileInfomation = lstProfile.Where(m => m.ID == ProfileID).FirstOrDefault();
        //            #region thông tin chung
        //            ReportDetailShiftEntity.DateFrom = DateFrom;

        //            ReportDetailShiftEntity.WorkDate = WorkdayByProfile.WorkDate;

        //            #endregion
        //            #region thông tin Profile
        //            if (profileInfomation != null)
        //            {
        //                ReportDetailShiftEntity.ProfileName = profileInfomation.ProfileName;
        //                ReportDetailShiftEntity.CodeEmp = profileInfomation.CodeEmp;
        //            }
        //            #endregion
        //            #region thông tin Phòng ban
        //            Guid? orgID = null;
        //            if (profileInfomation != null)
        //            {
        //                orgID = profileInfomation.OrgStructureID;
        //            }
        //            if (orgID != null)
        //            {
        //                var orgByProfile = lstOrgAll.Where(m => m.ID == orgID).FirstOrDefault();
        //                if (orgByProfile != null)
        //                {
        //                    ReportDetailShiftEntity.DepartmentName = orgByProfile.OrgName;
        //                    ReportDetailShiftEntity.DeptPath = orgByProfile.OrgCode;
        //                }

        //            }
        //            if (orgID != null && OrgNameAllLevel.ContainsKey(orgID))
        //            {
        //                var OrgNameFull = OrgNameAllLevel[orgID];
        //                if (OrgNameFull != null)
        //                {

        //                    ReportDetailShiftEntity.SectionCode = OrgNameFull.SectionCode;
        //                    ReportDetailShiftEntity.GroupCode = OrgNameFull.TeamCode;
        //                    string Path = string.Empty;
        //                    if (!string.IsNullOrEmpty(OrgNameFull.BrandCode))
        //                    {
        //                        Path += OrgNameFull.BrandCode;
        //                    }
        //                    if (!string.IsNullOrEmpty(OrgNameFull.SectionCode))
        //                    {
        //                        Path += "/" + OrgNameFull.SectionCode;
        //                    }
        //                    if (!string.IsNullOrEmpty(OrgNameFull.DepartmentCode))
        //                    {
        //                        Path += "/" + OrgNameFull.DepartmentCode;
        //                    }
        //                    if (!string.IsNullOrEmpty(OrgNameFull.TeamCode))
        //                    {
        //                        Path += "/" + OrgNameFull.TeamCode;
        //                    }
        //                    ReportDetailShiftEntity.DeptPath = Path;
        //                }
        //            }

        //            #endregion
        //            #region thông tin Shift
        //            //public string ShiftName { get; set; }
        //            if (WorkdayByProfile.ShiftID != null)
        //            {
        //                var Shift = lstShift.Where(m => m.ID == WorkdayByProfile.ShiftID).FirstOrDefault();
        //                if (Shift != null)
        //                {
        //                    ReportDetailShiftEntity.ShiftName = Shift.ShiftName;
        //                    if (Shift.InTime != null && Shift.CoOut != null)
        //                    {
        //                        DateTime Workday = WorkdayByProfile.WorkDate.Date;
        //                        ReportDetailShiftEntity.InTime = Workday.AddHours(Shift.InTime.Hour).AddMinutes(Shift.InTime.Minute);
        //                        ReportDetailShiftEntity.OutTime = ReportDetailShiftEntity.InTime.Value.AddHours(Shift.CoOut);
        //                    }
        //                }
        //            }
        //            #endregion
        //            #region thông tin công

        //            ReportDetailShiftEntity.DateTo = DateTo;

        //            #endregion
        //            lstDetailShiftEntity.Add(ReportDetailShiftEntity);

        //        }
        //    }

        //    return lstDetailShiftEntity;
        //}
        #endregion

        #endregion


        #region BC Chi Tiết Giờ Vào/Giờ Ra

        /// <summary>
        /// Khởi Tạo DataTable cho BC Chi Tiết Giờ Vào/Giờ Ra
        /// </summary>
        /// <returns></returns>
        DataTable CreateReportSummaryInOutSchema()
        {

            DataTable tb = new DataTable("Att_ReportSumaryInOutEntity");
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.ProfileName);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.PositionName);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.JobtitleName);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.GroupCode);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.GroupName);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.TeamCode);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.TeamName);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.BranchCode);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.BranchName);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.SectionCode);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.SectionName);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.DepartmentCode);

            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.Division);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.Date, typeof(DateTime));
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.ShiftName);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.ApprovedShift);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.udTimeIn, typeof(DateTime));
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.udTimeOut, typeof(DateTime));
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.InTime, typeof(DateTime));
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.OutTime, typeof(DateTime));
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.Remark);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.DepartmentName);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.DivisionName);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.DateFrom, typeof(DateTime));
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.DateTo, typeof(DateTime));
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.UserExport);
            tb.Columns.Add(Att_ReportSumaryInOutEntity.FieldNames.DateExport, typeof(DateTime));

            return tb;
        }

        /// <summary>
        /// Lấy Dữ Liệu BC Báo cáo chi tiết giờ vào giờ ra
        /// </summary>
        /// <param name="DateFrom">Ngày bắt đầu</param>
        /// <param name="DateTo">Ngày kết thúc (Cuối ngày 23:59:59)</param>
        /// <param name="lstProfileIDs">Ds ProfileIDs</param>
        /// <returns></returns>
        public DataTable GetReportSumaryInOut(DateTime DateFrom, DateTime DateTo, List<Hre_ProfileEntity> profiles, Guid[] ShiftIDs, string CodeEmp, bool isIncludeQuitEmp, string userExport, string UserLogin)
        {
            List<Guid> lstProfileIDs = profiles.Select(s => s.ID).ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Workday = new Att_WorkDayRepository(unitOfWork);

                var workDays = repoAtt_Workday.FindBy(s => s.IsDelete == null &&
                    DateFrom <= s.WorkDate && s.WorkDate <= DateTo
                    && (s.InTime1 != null || s.OutTime1 != null))
                    .Select(s => new { s.ProfileID, s.ShiftID, s.ShiftApprove, s.WorkDate, s.InTime1, s.OutTime1 }).ToList();
                //if(lstProfileIDs != null){
                //    workDays = workDays.Where(s => lstProfileIDs.Contains(s.ProfileID)).ToList();
                //}
                if (ShiftIDs != null)
                {
                    workDays = workDays.Where(s => s.ShiftID.HasValue && ShiftIDs.Contains(s.ShiftID.Value)).ToList();
                }

                var profileIds = workDays.Select(s => s.ProfileID).Distinct().ToList();

                // var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                //var profiles = repoHre_Profile.FindBy(s => s.IsDelete == null && profileIds.Contains(s.ID))
                //  .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
                profiles = profiles.Where(s => s.IsDelete == null && profileIds.Contains(s.ID)).ToList();
                // .Select(s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();

                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var positions = repoCat_Position.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();

                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();

                var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                var shifts = repoCat_Shift.GetAll().ToList();

                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > DateTo).ToList();
                }

                if (!string.IsNullOrEmpty(CodeEmp))
                {
                    profiles = profiles.Where(s => s.CodeEmp == CodeEmp).ToList();
                }
                List<Guid> guids = profiles.Select(s => s.ID).ToList();
                workDays = workDays.Where(s => guids.Contains(s.ProfileID)).ToList();
                string E_FULLSHIFT = LeaveDayDurationType.E_FULLSHIFT.ToString();
                string E_APPROVED = LeaveDayStatus.E_APPROVED.ToString();

                var repoAtt_LeaveDay = new Att_LeavedayRepository(unitOfWork);
                var leavedayProfiles = repoAtt_LeaveDay.FindBy(s => s.DurationType == E_FULLSHIFT
                     & s.Status == E_APPROVED && DateFrom <= s.DateEnd && s.DateStart <= DateTo).Select(s => new { s.ProfileID, s.DateStart, s.DateEnd }).ToList();
                //if (isNotAllowZero)
                //{
                //    profileIds = workDays.Select(s => s.ProfileID).ToList();
                //    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                //}
                DataTable table = CreateReportSummaryInOutSchema();
                foreach (var profile in profiles)
                {
                    for (DateTime date = DateFrom.Date; date <= DateTo; date = date.AddDays(1))
                    {
                        var leaday = leavedayProfiles.FirstOrDefault(s => s.ProfileID == profile.ID && s.DateStart <= date && date <= s.DateEnd);
                        var workDayProfiles = workDays.Where(s => s.WorkDate.Date == date && s.ProfileID == profile.ID).ToList();
                        if (workDayProfiles.Count > 0 && leaday == null)
                        {
                            DataRow row = table.NewRow();

                            var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                            var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);

                            Guid? orgId = profile.OrgStructureID;
                            var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                            var orgGroup = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);
                            var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                            var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                            var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                            var orgDivision = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);


                            row[Att_ReportSumaryInOutEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                            row[Att_ReportSumaryInOutEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                            row[Att_ReportSumaryInOutEntity.FieldNames.GroupCode] = orgGroup != null ? orgGroup.Code : string.Empty;
                            row[Att_ReportSumaryInOutEntity.FieldNames.GroupName] = orgGroup != null ? orgGroup.OrgStructureName : string.Empty;
                            row[Att_ReportSumaryInOutEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                            row[Att_ReportSumaryInOutEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                            row[Att_ReportSumaryInOutEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                            row[Att_ReportSumaryInOutEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                            row[Att_ReportSumaryInOutEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                            row[Att_ReportSumaryInOutEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                            row[Att_ReportSumaryInOutEntity.FieldNames.Division] = orgDivision != null ? orgDivision.Code : string.Empty;
                            row[Att_ReportSumaryInOutEntity.FieldNames.DivisionName] = orgDivision != null ? orgDivision.OrgStructureName : string.Empty;


                            row[Att_ReportSumaryInOutEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                            row[Att_ReportSumaryInOutEntity.FieldNames.ProfileName] = profile.ProfileName;

                            row[Att_ReportSumaryInOutEntity.FieldNames.PositionName] = positon != null ? positon.PositionName : string.Empty;
                            row[Att_ReportSumaryInOutEntity.FieldNames.JobtitleName] = jobtitle != null ? jobtitle.JobTitleName : string.Empty;
                            var workDay1 = workDayProfiles.FirstOrDefault();
                            row[Att_ReportSumaryInOutEntity.FieldNames.Date] = date;
                            row[Att_ReportSumaryInOutEntity.FieldNames.DateFrom] = DateFrom.Date;
                            row[Att_ReportSumaryInOutEntity.FieldNames.DateTo] = DateTo;
                            row[Att_ReportSumaryInOutEntity.FieldNames.DateExport] = DateTime.Now;
                            row[Att_ReportSumaryInOutEntity.FieldNames.UserExport] = userExport;
                            //    row[Att_ReportSumaryInOutEntity.FieldNames.UserExport] = Session[SessionObjects.UserLogin];
                            if (workDay1 != null)
                            {
                                //Guid? ShiftID = workDay1.ShiftApprove ?? workDay1.ShiftID;
                                var shift = shifts.FirstOrDefault(s => s.ID == workDay1.ShiftID);
                                row[Att_ReportSumaryInOutEntity.FieldNames.ShiftName] = shift != null ? shift.ShiftName : string.Empty;
                                row[Att_ReportSumaryInOutEntity.FieldNames.udTimeIn] = workDay1.InTime1 != null ? workDay1.InTime1.Value : DateTime.MinValue;
                                if (workDay1.OutTime1 != null)
                                    row[Att_ReportSumaryInOutEntity.FieldNames.udTimeOut] = workDay1.OutTime1 != null ? workDay1.OutTime1.Value : DateTime.MinValue;
                                if (workDay1.ShiftApprove != Guid.Empty)
                                {
                                    shift = shifts.FirstOrDefault(s => s.ID == workDay1.ShiftApprove);
                                    row[Att_ReportSumaryInOutEntity.FieldNames.ApprovedShift] = shift != null ? shift.ShiftName : string.Empty;
                                }

                            }
                            table.Rows.Add(row);
                        }
                    }
                }
                return table;
            }
        }
        //public List<Att_ReportSumaryInOutEntity> GetReportSumaryInOut(DateTime DateFrom, DateTime DateTo, List<Guid> lstProfileIDs,List<Guid?> ShiftIDs,string CodeEmp)
        //{




        //    #region Code Cũ
        //    #region Logic Hiển thị Hiện nay
        //    //1. lấy theo profile, workday firstin lastout 
        //    //2. Phòng ban thì theo cái phòng ban loại DepartmentName và Division là của phòng ban hiện tại của nhân viên
        //    #endregion
        //    List<Att_ReportSumaryInOutEntity> lstReportSumaryInOut = new List<Att_ReportSumaryInOutEntity>();
        //    #region getData
        //    var lstWorkday = new List<Att_Workday>().Select(m => new { m.ProfileID, m.WorkDate, m.ShiftID, m.FirstInTime, m.LastOutTime }).ToList();
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_WorkDayRepository(unitOfWork);
        //        if (lstProfileIDs != null && lstProfileIDs.Count > 0)
        //        {
        //            lstWorkday = repo.FindBy(m => m.WorkDate >= DateFrom && m.WorkDate < DateTo
        //            && m.ProfileID != null && lstProfileIDs.Contains(m.ProfileID))
        //                .Select(m => new { m.ProfileID, m.WorkDate, m.ShiftID, m.FirstInTime, m.LastOutTime }).ToList();
        //        }
        //        else
        //        {
        //            lstWorkday = repo.FindBy(m => m.WorkDate >= DateFrom && m.WorkDate < DateTo)
        //               .Select(m => new { m.ProfileID, m.WorkDate, m.ShiftID, m.FirstInTime, m.LastOutTime }).ToList();
        //        }
        //    }
        //    var lstProfile = new List<Hre_Profile>().Select(m => new { m.ID, m.OrgStructureID, m.CodeEmp, m.ProfileName }).ToList();
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Hre_ProfileRepository(unitOfWork);
        //        if (lstProfileIDs != null && lstProfileIDs.Count > 0)
        //        {
        //            lstProfile = repo.FindBy(m => lstProfileIDs.Contains(m.ID)).Select(m => new { m.ID, m.OrgStructureID, m.CodeEmp, m.ProfileName }).ToList();
        //        }
        //        else
        //        {
        //            lstProfile = repo.GetAll().Select(m => new { m.ID, m.OrgStructureID, m.CodeEmp, m.ProfileName }).ToList();

        //        }
        //    }
        //    List<Guid> lstProfileid = lstProfile.Select(m => m.ID).ToList();

        //    var lstShift = new List<Cat_Shift>().Select(m => new { m.ID, m.ShiftName }).ToList();
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Cat_ShiftRepository(unitOfWork);
        //        lstShift = repo.GetAll().Select(m => new { m.ID, m.ShiftName }).ToList();
        //    }

        //    #endregion
        //    #region get org nhieu cap
        //    List<OrgTiny> lstOrgAll = new List<OrgTiny>();
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repoOrg = new Cat_OrgStructureRepository(unitOfWork);
        //        lstOrgAll = repoOrg.GetAll().Select(m => new OrgTiny { ID = m.ID, OrgCode = m.Code, OrgName = m.OrgStructureName, ParentID = m.ParentID, TypeID = m.TypeID }).ToList();
        //    }
        //    List<Guid?> lstOrgIDs = lstProfile.Select(m => m.OrgStructureID).Distinct().ToList();
        //    Dictionary<Guid?, OrgLevelTypeName> OrgNameAllLevel = Cat_OrgStructureServices.GetFullLinkOrg(lstOrgIDs, lstOrgAll);
        //    #endregion
        //    foreach (var ProfileID in lstProfileid)
        //    {
        //        if (ProfileID == null)
        //            continue;

        //        var lstWorkdayByProfile = lstWorkday.Where(m => m.ProfileID == ProfileID).ToList();
        //        foreach (var WorkdayByProfile in lstWorkdayByProfile)
        //        {


        //            Att_ReportSumaryInOutEntity ReportSumaryInOutEntity = new Att_ReportSumaryInOutEntity();
        //            var profileInfomation = lstProfile.Where(m => m.ID == ProfileID).FirstOrDefault();
        //            #region thông tin chung
        //            ReportSumaryInOutEntity.DateFrom = DateFrom;
        //            ReportSumaryInOutEntity.DateTo = DateTo;
        //            ReportSumaryInOutEntity.Date = WorkdayByProfile.WorkDate;


        //            #endregion
        //            #region thông tin Profile
        //            if (profileInfomation != null)
        //            {
        //                ReportSumaryInOutEntity.ProfileName = profileInfomation.ProfileName;
        //                ReportSumaryInOutEntity.CodeEmp = profileInfomation.CodeEmp;
        //            }
        //            #endregion
        //            #region thông tin Phòng ban
        //            Guid? orgID = null;
        //            if (profileInfomation != null)
        //            {
        //                orgID = profileInfomation.OrgStructureID;
        //            }
        //            if (orgID != null)
        //            {
        //                var orgByProfile = lstOrgAll.Where(m => m.ID == orgID).FirstOrDefault();
        //                if (orgByProfile != null)
        //                {
        //                    ReportSumaryInOutEntity.DepartmentName = orgByProfile.OrgName;
        //                    ReportSumaryInOutEntity.Division = orgByProfile.OrgCode;
        //                }

        //            }
        //            if (orgID != null && OrgNameAllLevel.ContainsKey(orgID))
        //            {
        //                var OrgNameFull = OrgNameAllLevel[orgID];
        //                if (OrgNameFull != null)
        //                {

        //                    ReportSumaryInOutEntity.SectionCode = OrgNameFull.SectionCode;
        //                    ReportSumaryInOutEntity.GroupCode = OrgNameFull.TeamCode;
        //                }
        //            }
        //            #endregion
        //            #region thông tin Shift
        //            //public string ShiftName { get; set; }
        //            if (WorkdayByProfile.ShiftID != null)
        //            {
        //                var Shift = lstShift.Where(m => m.ID == WorkdayByProfile.ShiftID).FirstOrDefault();
        //                if (Shift != null)
        //                {
        //                    ReportSumaryInOutEntity.ShiftName = Shift.ShiftName;
        //                }
        //            }
        //            #endregion

        //            #region thông tin FirstIn LastOut
        //            ReportSumaryInOutEntity.InTime = WorkdayByProfile.FirstInTime;
        //            ReportSumaryInOutEntity.OutTime = WorkdayByProfile.LastOutTime;
        //            #endregion

        //            lstReportSumaryInOut.Add(ReportSumaryInOutEntity);

        //        }
        //    }
        //    return lstReportSumaryInOut;
        //    #endregion

        //}


        #endregion

        DataTable CreateReportProfileAllowLimitOvertime()
        {
            DataTable tb = new DataTable("Att_ReportProfileAllowLimitOvertimeEntity");
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.ProfileName);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.BranchCode);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.BranchName);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.GroupCode);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.GroupName);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.DepartmentCode);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.DepartmentName);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.TeamCode);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.TeamName);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.SectionCode);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.SectionName);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.DivisionCode);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.DivisionName);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.Position);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.JobTitle);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.UserExport);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.DateExport);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.DateFrom);
            tb.Columns.Add(Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.DateTo);
            return tb;
        }
        public DataTable GetReportProfileAllowLimitOvertime(DateTime DateFrom, DateTime DateTo, List<Hre_ProfileEntity> profiles, string orgStructureID, string userExport, bool isCreateTemplate, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                DataTable table = CreateReportProfileAllowLimitOvertime();
                if (isCreateTemplate)
                {
                    return table;
                }
                if (profiles.Count == 0)
                    return table;
                BaseService baservice = new BaseService();
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var positions = repoCat_Position.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
                var jobtitles = repoCat_JobTitle.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();
                string status = string.Empty;
                List<object> paraAtt_AllowLimitOvertime = new List<object>();
                paraAtt_AllowLimitOvertime.AddRange(new object[3]);
                paraAtt_AllowLimitOvertime[0] = (object)orgStructureID;
                paraAtt_AllowLimitOvertime[1] = DateFrom;
                paraAtt_AllowLimitOvertime[2] = DateTo;
                // var lstAllowLimit = new List<Att_AllowLimitOvertime>().Select(m => new { m.ProfileID, m.DateStart, m.DateEnd });
                var lstAllowLimit = baservice.GetData<Att_AllowLimitOvertimeEntity>(paraAtt_AllowLimitOvertime, ConstantSql.hrm_att_getdata_AllowLimitOvertime, UserLogin, ref status).ToList().Translate<Att_AllowLimitOvertime>();
                if (lstAllowLimit.Count == 0)
                    return table;
                List<Guid> lstProfileIds = lstAllowLimit.Select(m => m.ProfileID ?? Guid.Empty).ToList();
                profiles = profiles.Where(m => lstProfileIds.Contains(m.ID)).ToList();
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                foreach (var AllowLimit in lstAllowLimit)
                {
                    DataRow row = table.NewRow();
                    var profile = profiles.Where(m => m.ID == AllowLimit.ProfileID).FirstOrDefault();
                    if (profile != null)
                    {
                        row[Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.ProfileName] = profile.ProfileName;
                        row[Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                        Guid? orgId = profile.OrgStructureID;
                        var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                        var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                        var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                        var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                        var orgDivision = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                        var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                        var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);

                        row[Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                        row[Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                        row[Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                        row[Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                        row[Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                        row[Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                        row[Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                        row[Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                        row[Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.DivisionCode] = orgDivision != null ? orgDivision.Code : string.Empty;
                        row[Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.DivisionName] = orgDivision != null ? orgDivision.OrgStructureName : string.Empty;
                        row[Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.Position] = positon != null ? positon.PositionName : string.Empty;
                        row[Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.JobTitle] = jobtitle != null ? jobtitle.JobTitleName : string.Empty;
                        row[Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.DateFrom] = AllowLimit.DateStart;
                        row[Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.DateTo] = AllowLimit.DateEnd;
                        row[Att_ReportProfileAllowLimitOvertimeEntity.FieldNames.DateExport] = DateTime.Now;

                    }
                    table.Rows.Add(row);
                }
                return table;
            }
        }


        DataTable CreateReportDiligenceYearSchema()
        {

            DataTable tb = new DataTable("Att_ReportDiligenceYearEntity");
            tb.Columns.Add(Att_ReportDiligenceYearEntity.FieldNames.OrgStructureName);
            tb.Columns.Add(Att_ReportDiligenceYearEntity.FieldNames.DepartmentCode);
            tb.Columns.Add(Att_ReportDiligenceYearEntity.FieldNames.Date, typeof(DateTime));
            tb.Columns.Add(Att_ReportDiligenceYearEntity.FieldNames.ShiftName);
            tb.Columns.Add(Att_ReportDiligenceYearEntity.FieldNames.DepartmentName);
            tb.Columns.Add(Att_ReportDiligenceYearEntity.FieldNames.Sizes);
            tb.Columns.Add(Att_ReportDiligenceYearEntity.FieldNames.Absence);
            tb.Columns.Add(Att_ReportDiligenceYearEntity.FieldNames.DateFrom, typeof(DateTime));
            tb.Columns.Add(Att_ReportDiligenceYearEntity.FieldNames.DateTo, typeof(DateTime));
            tb.Columns.Add(Att_ReportDiligenceYearEntity.FieldNames.UserExport);
            tb.Columns.Add(Att_ReportDiligenceYearEntity.FieldNames.DateExport, typeof(DateTime));
            return tb;
        }

        public DataTable GetReportDiligenceYear(DateTime DateFrom, DateTime DateTo, string orgStructureID, List<Hre_ProfileEntity> profiles, string userExport)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                DataTable table = CreateReportDiligenceYearSchema();
                BaseService baseservice = new BaseService();
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var orgStructures = repoCat_OrgStructure.FindBy(s => s.IsDelete == null && s.Code != null).ToList();

                var shifts = unitOfWork.CreateQueryable<Cat_Shift>().ToList();
                DateTo = DateTo.AddDays(1).AddSeconds(-1);
                string status = string.Empty;
                var lstOrgIDs = new List<Guid>();
                List<string> lstOrgString = new List<string>();
                if (orgStructureID != null)
                {
                    lstOrgString = orgStructureID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                }
                foreach (var item in lstOrgString)
                {
                    Guid OrgID = Guid.Empty;
                    Guid.TryParse(item, out OrgID);
                    if (OrgID != Guid.Empty)
                    {
                        lstOrgIDs.Add(OrgID);
                    }
                }
                var RosterQuery = unitOfWork.CreateQueryable<Att_Roster>(m => m.DateStart <= DateTo && m.DateEnd >= DateFrom);
                if (lstOrgIDs.Count > 0)
                {
                    RosterQuery = RosterQuery.Where(m => m.OrgStructureID != null && lstOrgIDs.Contains(m.OrgStructureID.Value));
                }
                var rosters = RosterQuery.ToList();
                var lstProfileIDsByOrg = rosters.Select(m => m.ProfileID).Distinct().ToList();
                profiles = profiles.Where(m => lstProfileIDsByOrg.Contains(m.ID)).ToList();
                List<Guid> Profileguids = profiles.Select(s => s.ID).ToList();
                List<string> cardcodes = profiles.Select(s => s.CodeAttendance).Distinct().ToList();
                List<string> scan = unitOfWork.CreateQueryable<Att_TAMScanLog>(m => m.TimeLog >= DateFrom && m.TimeLog <= DateTo && cardcodes.Contains(m.CardCode)).Select(s => s.CardCode).ToList<string>();
                var repoAtt_Grade = new Att_GradeRepository(unitOfWork);
                var grades = repoAtt_Grade.FindBy(s => s.IsDelete == null && s.MonthStart <= DateTo && Profileguids.Contains(s.ProfileID.Value)).OrderByDescending(prop => prop.MonthStart).ToList();

                // var grades = unitOfWork.CreateQueryable<Att_Grade>(s => s.IsDelete == null && s.MonthStart <= DateTo && Profileguids.Contains(s.ProfileID.Value)).ToList().Translate<Att_Grade>();
                string E_ISDEFAULT = GradeRosterType.E_ISDEFAULT.ToString();
                string E_ISROSTER_ORG = GradeRosterType.E_ISROSTER_ORG.ToString();
                List<Att_Grade> gradesDefault = grades.Where(s => s.Cat_GradeCfg != null && s.Cat_GradeCfg.RosterType == E_ISDEFAULT).ToList();
                List<Att_Grade> gradesOrg = grades.Where(s => s.Cat_GradeCfg != null && s.Cat_GradeCfg.RosterType == E_ISROSTER_ORG).ToList();
                for (DateTime date = DateFrom.Date; date <= DateTo; date = date.AddDays(1))
                {
                    foreach (Cat_OrgStructure catOrgStructure in orgStructures)
                    {
                        List<Att_Roster> attRosters = rosters.Where(s => s.OrgStructureID == catOrgStructure.ID).ToList();
                        var profilesIID = attRosters.Select(m => m.ProfileID).Distinct().ToList();
                        List<Guid> guidsAll = new List<Guid>();
                        foreach (Cat_Shift catShift in shifts)
                        {
                            List<Guid> profileIDRoster = attRosters.Where(s => s.MonShiftID == catShift.ID || s.TueShiftID == catShift.ID || s.WedShiftID == catShift.ID || s.ThuShiftID == catShift.ID || s.FriShiftID == catShift.ID || s.SatShiftID == catShift.ID || s.SunShiftID == catShift.ID).Select(s => s.ProfileID).ToList();
                            List<Guid> profileIDDefault = gradesDefault
                                .Where(
                                        s => profilesIID.Contains(s.ProfileID.Value)
                                            && (s.Cat_GradeCfg.WorkOnMondayID == catShift.ID
                                            || s.Cat_GradeCfg.WorkOnTuesdayID == catShift.ID
                                            || s.Cat_GradeCfg.WorkOnWednesdayID == catShift.ID
                                            || s.Cat_GradeCfg.WorkOnThursdayID == catShift.ID
                                            || s.Cat_GradeCfg.WorkOnFridayID == catShift.ID
                                            || s.Cat_GradeCfg.WorkOnSaturdayID == catShift.ID
                                            || s.Cat_GradeCfg.WorkOnSundayID == catShift.ID))
                                .Select(s => s.ProfileID.Value).ToList();
                            List<Guid> profileIDgradesOrg = gradesOrg
                                .Where(
                                        s => (s.Cat_GradeCfg.WorkOnMondayID == catShift.ID
                                            || s.Cat_GradeCfg.WorkOnTuesdayID == catShift.ID
                                            || s.Cat_GradeCfg.WorkOnWednesdayID == catShift.ID
                                            || s.Cat_GradeCfg.WorkOnThursdayID == catShift.ID
                                            || s.Cat_GradeCfg.WorkOnFridayID == catShift.ID
                                            || s.Cat_GradeCfg.WorkOnSaturdayID == catShift.ID
                                            || s.Cat_GradeCfg.WorkOnSundayID == catShift.ID)
                                            && profilesIID.Contains(s.ProfileID.Value))
                                            .Select(s => s.ProfileID.Value).ToList();
                            guidsAll.AddRange(profileIDRoster);
                            guidsAll.AddRange(profileIDDefault);
                            guidsAll.AddRange(profileIDgradesOrg);
                        }
                        List<string> codeAtts = profiles.Where(s => guidsAll.Contains(s.ID)).Select(s => s.CodeAttendance).ToList();
                        DataRow row = table.NewRow();
                        row[Att_ReportDiligenceYearEntity.FieldNames.OrgStructureName] = catOrgStructure.OrgStructureName;
                        row[Att_ReportDiligenceYearEntity.FieldNames.DepartmentCode] = catOrgStructure.OrgStructureName;
                        row[Att_ReportDiligenceYearEntity.FieldNames.Date] = date;
                        row[Att_ReportDiligenceYearEntity.FieldNames.Sizes] = guidsAll.Count;
                        row[Att_ReportDiligenceYearEntity.FieldNames.DateFrom] = DateFrom;
                        row[Att_ReportDiligenceYearEntity.FieldNames.DateTo] = DateTo;
                        row[Att_ReportDiligenceYearEntity.FieldNames.UserExport] = userExport;
                        row[Att_ReportDiligenceYearEntity.FieldNames.DateExport] = DateTime.Today;
                        guidsAll = guidsAll.Distinct().ToList();
                        codeAtts = codeAtts.Where(s => !string.IsNullOrWhiteSpace(s) && !scan.Contains(s)).ToList();
                        double absence = codeAtts.Count();
                        row[Att_ReportDiligenceYearEntity.FieldNames.Absence] = absence;
                        if (guidsAll.Count > 0 || absence > 0)
                            table.Rows.Add(row);
                    }
                }

                return table;
            }

        }




        #region Báo cáo tổng số lần đi muộn về sớm trong tháng

        /// <summary>
        /// Lấy Dữ Liệu BC Báo cáo tổng số lần đi muộn về sớm trong tháng
        /// </summary>
        /// <param name="DateFrom">Ngày bắt đầu</param>
        /// <param name="DateTo">Ngày kết thúc (Cuối ngày 23:59:59)</param>
        /// <param name="lstProfileIDs">Ds ProfileIDs</param>
        /// <returns></returns>

        public List<Att_ReportSumaryLateInOutEntity> GetReportSumaryLateInOut(DateTime DateFrom, DateTime DateTo, List<Hre_ProfileEntity> profiles, Guid[] ShiftIDs, bool isIncludeQuitEmp, bool noDisplay0Data, string userExport)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Workday = new Att_WorkDayRepository(unitOfWork);
                List<Att_ReportSumaryLateInOutEntity> lstReportSumaryLateInOut = new List<Att_ReportSumaryLateInOutEntity>();
                var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                var shifts = repoCat_Shift.GetAll().ToList();
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var positions = repoCat_Position.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();
                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > DateTo).ToList();
                }
                var workDays = repoAtt_Workday.FindBy(s =>
                  DateFrom <= s.WorkDate && s.WorkDate <= DateTo
                  && s.LateEarlyDuration > 0 && s.IsDelete == null)
                  .Select(m => new { m.ID, m.ProfileID, m.LateDuration1, m.EarlyDuration1, m.LateEarlyDuration, m.ShiftID }).ToList();
                if (ShiftIDs != null)
                {
                    workDays = workDays.Where(s => s.ShiftID.HasValue && ShiftIDs.Contains(s.ShiftID.Value)).ToList();
                }
                //var lstWorkday = workDays.Select(m => new { m.ID, m.ProfileID, m.LateDuration1, m.EarlyDuration1, m.LateEarlyDuration }).ToList();
                if (noDisplay0Data)
                {
                    var profileIDs = workDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIDs.Contains(s.ID)).ToList();
                }
                foreach (var profile in profiles)
                {
                    Att_ReportSumaryLateInOutEntity rptSumaryLateInOut = new Att_ReportSumaryLateInOutEntity();

                    Guid? orgId = profile.OrgStructureID;
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    rptSumaryLateInOut.GroupCode = orgBranch != null ? orgBranch.Code : string.Empty;
                    rptSumaryLateInOut.DepartmentCode = orgOrg != null ? orgOrg.Code : string.Empty;
                    rptSumaryLateInOut.Division = orgTeam != null ? orgTeam.Code : string.Empty; ;
                    rptSumaryLateInOut.SectionCode = orgSection != null ? orgSection.Code : string.Empty;
                    rptSumaryLateInOut.CodeEmp = profile.CodeEmp;
                    rptSumaryLateInOut.ProfileName = profile.ProfileName;
                    var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                    rptSumaryLateInOut.PositionCode = positon != null ? positon.Code : string.Empty;
                    //  var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
                    rptSumaryLateInOut.DateFrom = DateFrom;
                    rptSumaryLateInOut.DateTo = DateTo;
                    rptSumaryLateInOut.DateExport = DateTime.Today;
                    rptSumaryLateInOut.UserExport = userExport;
                    var lstWorkday_ByProfile = workDays.Where(m => m.ProfileID == profile.ID).ToList();
                    if (lstWorkday_ByProfile != null && lstWorkday_ByProfile.Count > 0)
                    {
                        var sumEarly = lstWorkday_ByProfile.Sum(s => s.EarlyDuration1);
                        var SumLateMinute = lstWorkday_ByProfile.Sum(s => s.LateDuration1);
                        var CountLate = lstWorkday_ByProfile.Count(m => m.LateDuration1 > 0);
                        var CountEarly = lstWorkday_ByProfile.Count(m => m.EarlyDuration1 > 0);
                        rptSumaryLateInOut.EarlyMinutes = sumEarly > 0 ? sumEarly : null;
                        rptSumaryLateInOut.LateMinutes = SumLateMinute > 0 ? SumLateMinute : null;
                        rptSumaryLateInOut.NumTimeEarly = CountEarly > 0 ? (int?)CountEarly : null;
                        rptSumaryLateInOut.NumTimeLate = CountLate > 0 ? (int?)CountLate : null;
                    }
                    lstReportSumaryLateInOut.Add(rptSumaryLateInOut);
                }
                return lstReportSumaryLateInOut;

            }
        }
        #region Code Cũ
        //public List<Att_ReportSumaryLateInOutEntity> GetReportSumaryLateInOut(DateTime DateFrom, DateTime DateTo, List<Guid> lstProfileIDs)
        //{
        //    #region Logic Hiển thị Hiện nay
        //    //1. số lần trễ sớm được tính theo ngày. vd: nếu 1 ngày có 2 lần trễ thì result trễ =1 
        //    //2. Số phút trễ sớm được tính theo ngày vd: tổng các phút trễ trong ngày là 10 thì result = 10;
        //    //3. Phòng ban thì theo cái phòng ban loại DepartmentName và Division là của phòng ban hiện tại của nhân viên
        //    #endregion


        //    List<Att_ReportSumaryLateInOutEntity> lstReportSumaryLateInOut = new List<Att_ReportSumaryLateInOutEntity>();
        //    #region getData
        //    var lstWorkday = new List<Att_Workday>()
        //        .Select(m => new { m.ProfileID, m.WorkDate, m.LateDuration1, m.LateDuration2, m.LateDuration3, m.LateDuration4, m.EarlyDuration1, m.EarlyDuration2, m.EarlyDuration3, m.EarlyDuration4 }).ToList();
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_WorkDayRepository(unitOfWork);
        //        if (lstProfileIDs != null && lstProfileIDs.Count > 0)
        //        {
        //            lstWorkday = repo.FindBy(m => m.WorkDate >= DateFrom && m.WorkDate < DateTo
        //               && m.ProfileID != null && lstProfileIDs.Contains(m.ProfileID)
        //               && ((m.LateDuration1 != null && m.LateDuration1 > 0)
        //                   || (m.LateDuration2 != null && m.LateDuration2 > 0)
        //                   || (m.LateDuration3 != null && m.LateDuration3 > 0)
        //                   || (m.LateDuration4 != null && m.LateDuration4 > 0)
        //                   || (m.EarlyDuration1 != null && m.EarlyDuration1 > 0)
        //                   || (m.EarlyDuration2 != null && m.EarlyDuration2 > 0)
        //                   || (m.EarlyDuration3 != null && m.EarlyDuration3 > 0)
        //                   || (m.EarlyDuration4 != null && m.EarlyDuration4 > 0)))
        //                   .Select(m => new
        //                   {
        //                       m.ProfileID,
        //                       m.WorkDate,
        //                       m.LateDuration1,
        //                       m.LateDuration2,
        //                       m.LateDuration3,
        //                       m.LateDuration4,
        //                       m.EarlyDuration1,
        //                       m.EarlyDuration2,
        //                       m.EarlyDuration3,
        //                       m.EarlyDuration4
        //                   }).ToList();

        //        }
        //        else
        //        {
        //            lstWorkday = repo.FindBy(m => m.WorkDate >= DateFrom && m.WorkDate < DateTo
        //            && ((m.LateDuration1 != null && m.LateDuration1 > 0)
        //                || (m.LateDuration2 != null && m.LateDuration2 > 0)
        //                || (m.LateDuration3 != null && m.LateDuration3 > 0)
        //                || (m.LateDuration4 != null && m.LateDuration4 > 0)
        //                || (m.EarlyDuration1 != null && m.EarlyDuration1 > 0)
        //                || (m.EarlyDuration2 != null && m.EarlyDuration2 > 0)
        //                || (m.EarlyDuration3 != null && m.EarlyDuration3 > 0)
        //                || (m.EarlyDuration4 != null && m.EarlyDuration4 > 0)))
        //                .Select(m => new
        //                {
        //                    m.ProfileID,
        //                    m.WorkDate,
        //                    m.LateDuration1,
        //                    m.LateDuration2,
        //                    m.LateDuration3,
        //                    m.LateDuration4,
        //                    m.EarlyDuration1,
        //                    m.EarlyDuration2,
        //                    m.EarlyDuration3,
        //                    m.EarlyDuration4
        //                }).ToList();
        //        }

        //    }
        //    var lstProfile = new List<Hre_Profile>().Select(m => new { m.ID, m.OrgStructureID, m.CodeEmp, m.ProfileName }).ToList();
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Hre_ProfileRepository(unitOfWork);
        //        if (lstProfileIDs != null && lstProfileIDs.Count > 0)
        //        {
        //            lstProfile = repo.FindBy(m => lstProfileIDs.Contains(m.ID)).Select(m => new { m.ID, m.OrgStructureID, m.CodeEmp, m.ProfileName }).ToList();
        //        }
        //        else
        //        {
        //            lstProfile = repo.GetAll().Select(m => new { m.ID, m.OrgStructureID, m.CodeEmp, m.ProfileName }).ToList();
        //        }
        //    }
        //    List<Guid> lstProfileid = lstProfile.Select(m => m.ID).ToList();
        //    #endregion
        //    #region get org nhieu cap
        //    List<OrgTiny> lstOrgAll = new List<OrgTiny>();
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repoOrg = new Cat_OrgStructureRepository(unitOfWork);
        //        lstOrgAll = repoOrg
        //            .FindBy(s => s.IsDelete == null)
        //            .Select(m => new OrgTiny { ID = m.ID, OrgCode = m.Code, OrgName = m.OrgStructureName, ParentID = m.ParentID, TypeID = m.TypeID }).ToList();
        //    }
        //    List<Guid?> lstOrgIDs = lstProfile.Select(m => m.OrgStructureID).Distinct().ToList();
        //    Dictionary<Guid?, OrgLevelTypeName> OrgNameAllLevel = Cat_OrgStructureServices.GetFullLinkOrg(lstOrgIDs, lstOrgAll);
        //    #endregion
        //    foreach (var ProfileID in lstProfileid)
        //    {
        //        if (ProfileID == null)
        //            continue;
        //        var lstWorkdayByProfile = lstWorkday.Where(m => m.ProfileID == ProfileID).ToList();
        //        Att_ReportSumaryLateInOutEntity ReportSumaryLateInOutEntity = new Att_ReportSumaryLateInOutEntity();
        //        var profileInfomation = lstProfile.Where(m => m.ID == ProfileID).FirstOrDefault();
        //        #region thông tin chung
        //        ReportSumaryLateInOutEntity.DateFrom = DateFrom;
        //        ReportSumaryLateInOutEntity.DateTo = DateTo;
        //        #endregion
        //        #region thông tin Profile
        //        if (profileInfomation != null)
        //        {
        //            ReportSumaryLateInOutEntity.ProfileName = profileInfomation.ProfileName;
        //            ReportSumaryLateInOutEntity.CodeEmp = profileInfomation.CodeEmp;
        //        }
        //        #endregion
        //        #region thông tin Phòng ban
        //        Guid? orgID = null;
        //        if (profileInfomation != null)
        //        {
        //            orgID = profileInfomation.OrgStructureID;
        //        }
        //        if (orgID != null)
        //        {
        //            var orgByProfile = lstOrgAll.Where(m => m.ID == orgID).FirstOrDefault();
        //            if (orgByProfile != null)
        //            {
        //                ReportSumaryLateInOutEntity.DepartmentName = orgByProfile.OrgName;
        //                ReportSumaryLateInOutEntity.Division = orgByProfile.OrgCode;
        //            }

        //        }
        //        if (orgID != null && OrgNameAllLevel.ContainsKey(orgID))
        //        {
        //            var OrgNameFull = OrgNameAllLevel[orgID];
        //            if (OrgNameFull != null)
        //            {

        //                ReportSumaryLateInOutEntity.SectionCode = OrgNameFull.SectionCode;
        //                ReportSumaryLateInOutEntity.GroupCode = OrgNameFull.TeamCode;
        //            }
        //        }
        //        #endregion
        //        #region thông tin trễ sớm
        //        int LateCount = lstWorkdayByProfile.Where(m => (m.LateDuration1 != null && m.LateDuration1 > 0) || (m.LateDuration2 != null && m.LateDuration2 > 0) || (m.LateDuration3 != null && m.LateDuration3 > 0) || (m.LateDuration4 != null && m.LateDuration4 > 0)).Count();
        //        int EarlyCount = lstWorkdayByProfile.Where(m => (m.EarlyDuration1 != null && m.EarlyDuration1 > 0) || (m.EarlyDuration2 != null && m.EarlyDuration2 > 0) || (m.EarlyDuration3 != null && m.EarlyDuration3 > 0) || (m.EarlyDuration4 != null && m.EarlyDuration4 > 0)).Count();
        //        double? LateMintute = lstWorkdayByProfile.Sum(m => m.LateDuration1 ?? 0);
        //        LateMintute += lstWorkdayByProfile.Sum(m => m.LateDuration2 ?? 0);
        //        LateMintute += lstWorkdayByProfile.Sum(m => m.LateDuration3 ?? 0);
        //        LateMintute += lstWorkdayByProfile.Sum(m => m.LateDuration4 ?? 0);

        //        double? EarlyMintute = lstWorkdayByProfile.Sum(m => m.EarlyDuration1 ?? 0);
        //        LateMintute += lstWorkdayByProfile.Sum(m => m.EarlyDuration2 ?? 0);
        //        LateMintute += lstWorkdayByProfile.Sum(m => m.EarlyDuration3 ?? 0);
        //        LateMintute += lstWorkdayByProfile.Sum(m => m.EarlyDuration4 ?? 0);

        //        ReportSumaryLateInOutEntity.NumTimeLate = LateCount;
        //        ReportSumaryLateInOutEntity.LateMinutes = (int)LateMintute;
        //        ReportSumaryLateInOutEntity.NumTimeEarly = EarlyCount;
        //        ReportSumaryLateInOutEntity.EarlyMinutes = (int)EarlyMintute;

        //        #endregion
        //        lstReportSumaryLateInOut.Add(ReportSumaryLateInOutEntity);

        //    }
        //    return lstReportSumaryLateInOut;
        //}
        #endregion


        #endregion

        #region BC Chi Tiết Nghỉ Hàng Tháng của nhân viên

        /// <summary>
        /// Lấy Dữ Liệu BC Chi Tiết Nghỉ Hàng Tháng của nhân viên
        /// </summary>
        /// <returns></returns>
        public List<Att_ReportLeaveMonthEntity> GetReportLeaveMonth()
        {
            List<Att_ReportLeaveMonthEntity> lstLeaveMonthEntity = new List<Att_ReportLeaveMonthEntity>();
            return lstLeaveMonthEntity;
        }

        #endregion

        #region BC Chi Tiết Nghỉ Phép Ốm của nhân viên

        /// <summary>
        /// Lấy Dữ Liệu BC Chi Tiết Nghỉ Phép Ốm của nhân viên
        /// </summary>
        /// <returns></returns>
        public List<Att_ReportSickLeaveEntity> GetReportSickLeave()
        {
            List<Att_ReportSickLeaveEntity> lstSickLeaveEntity = new List<Att_ReportSickLeaveEntity>();
            return lstSickLeaveEntity;
        }

        #endregion

        #region báo cáo Sai ca

        /// <summary>
        /// [Hieu.Van] 01/06/2014
        /// Lấy ds tất cả báo cáo Sai ca
        /// </summary>
        /// <returns></returns>
        public List<Att_ReportWrongShiftEntity> GetReportWrongShift(DateTime dateStart, DateTime dateEnd, List<Hre_ProfileEntity> lstProfileAll, Guid[] ShiftIDs, bool isIncludeQuitEmp, string userExport)
        {
            using (var context = new VnrHrmDataContext())
            {

                List<Guid> lstProfileIDs = lstProfileAll.Select(s => s.ID).ToList();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Workday = new Att_WorkDayRepository(unitOfWork);
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                #region MyRegion
                //var workDays = repoAtt_Workday.FindBy(s => s.ShiftID != null && lstProfileIDs.Contains(s.ProfileID) && dateStart <= s.WorkDate && s.WorkDate <= dateEnd && s.IsDelete == null)
                //    .Select(s => new { s.ProfileID, s.WorkDate, s.ShiftID, s.InTime1, s.OutTime1, s.ShiftActual, s.ShiftApprove, s.Status }).ToList();
                #endregion
                var workDays = repoAtt_Workday.FindBy(s => s.ShiftID != null && dateStart <= s.WorkDate && s.WorkDate <= dateEnd && s.IsDelete == null)
                  .Select(s => new { s.ProfileID, s.WorkDate, s.ShiftID, s.InTime1, s.OutTime1, s.ShiftActual, s.ShiftApprove, s.Status }).ToList();
                //var profileIds = workDays.Where(s => lstProfileIDs.Contains(s.ProfileID)).Select(s => s.ProfileID).Distinct().ToList();
                var profileIds = workDays.Select(s => s.ProfileID).Distinct().ToList();
                var profiles = lstProfileAll.Where(s => profileIds.Contains(s.ID))
                    .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
                var orgs = repoCat_OrgStructure.FindBy(s => s.IsDelete == null && s.Code != null).ToList();
                #region
                // var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                //var positions = repoCat_Position.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
                //var jobtitles = repoCat_JobTitle.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();
                #endregion

                var shifts = repoCat_Shift.FindBy(s => s.IsDelete == null).ToList();
                if (ShiftIDs != null)
                {
                    workDays = workDays.Where(s => s.ShiftID.HasValue && ShiftIDs.Contains(s.ShiftID.Value)).ToList();
                    profileIds = workDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }
                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                }
                #region
                //if (!string.IsNullOrEmpty(CodeEmp))
                //{
                //    lstProfileIDs = repoHre_Profile.FindBy(s => s.CodeEmp == CodeEmp).Select(p => p.ID).ToList();
                //}
                #endregion
                List<Guid> guids = profiles.Select(s => s.ID).ToList();
                // workDays = workDays.Where(s => s.ProfileID != null && guids.Contains(s.ProfileID)).ToList();
                workDays = workDays.Where(s => guids.Contains(s.ProfileID)).ToList();
                profiles = profiles.Where(s => guids.Contains(s.ID)).ToList();
                List<Att_ReportWrongShiftEntity> lstReportWrongShiftEntity = new List<Att_ReportWrongShiftEntity>();
                Att_ReportWrongShiftEntity reportWrongShiftEntity = null;
                #region lay danh sach phong ban theo profiles
                ////  var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                //var lstDepartments = new List<Cat_OrgStructure>();
                ////   var orgTeam = new Cat_OrgStructure();
                ////   var orgSection = new Cat_OrgStructure();
                //var lstDivisions = new List<Cat_OrgStructure>();
                //var orgIds = new List<Guid>();
                //if (profiles.Any())
                //{
                //    orgIds = profiles.Select(p => p.OrgStructureID ?? Guid.Empty).ToList();
                //}

                //foreach (var orgId in orgIds)
                //{
                //    var department = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                //    var division = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);

                //    if (department != null)
                //    {
                //        lstDepartments.Add(department);
                //    }
                //    if (division != null)
                //    {
                //        lstDivisions.Add(division);
                //    }
                //}

                #endregion
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                var positions = repoCat_Position.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
                var jobtitles = repoCat_JobTitle.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();

                foreach (var profile in profiles)
                {
                    for (DateTime date = dateStart.Date; date <= dateEnd.Date; date = date.AddDays(1))
                    {
                        var workDay = workDays.FirstOrDefault(s => s.WorkDate.Date == date.Date && s.ProfileID == profile.ID);
                        if (workDay != null && workDay.ShiftActual != workDay.ShiftID)
                        {
                            reportWrongShiftEntity = new Att_ReportWrongShiftEntity();
                            //DataRow row = table.NewRow();
                            Guid? orgId = profile.OrgStructureID;
                            var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                            var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                            var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                            var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                            var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                            var orGroup = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);
                            var orgDivision = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);

                            reportWrongShiftEntity.BranchCode = orgBranch != null ? orgBranch.Code : string.Empty;
                            reportWrongShiftEntity.BranchName = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                            reportWrongShiftEntity.SectionCode = orgSection != null ? orgSection.Code : string.Empty;
                            reportWrongShiftEntity.SectionName = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                            reportWrongShiftEntity.GroupCode = orGroup != null ? orGroup.Code : string.Empty;
                            reportWrongShiftEntity.GroupName = orGroup != null ? orGroup.OrgStructureName : string.Empty;
                            reportWrongShiftEntity.Division = orgDivision != null ? orgDivision.Code : string.Empty;
                            reportWrongShiftEntity.DivisionName = orgDivision != null ? orgDivision.OrgStructureName : string.Empty;
                            reportWrongShiftEntity.TeamCode = orgTeam != null ? orgTeam.Code : string.Empty;
                            reportWrongShiftEntity.TeamName = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;

                            var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                            var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
                            var shift = shifts.FirstOrDefault(s => workDay != null && s.ID == workDay.ShiftID);
                            var shiftActual = shifts.FirstOrDefault(s => s.ID == workDay.ShiftActual);
                            var shiftApprove = shifts.FirstOrDefault(s => s.ID == workDay.ShiftApprove);
                            reportWrongShiftEntity.ProfileName = profile.ProfileName;
                            reportWrongShiftEntity.CodeEmp = profile.CodeEmp;

                            reportWrongShiftEntity.DepartmentCode = orgOrg != null ? orgOrg.Code : string.Empty;
                            reportWrongShiftEntity.DepartmentName = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                            reportWrongShiftEntity.PositionName = positon != null ? positon.PositionName : string.Empty;
                            reportWrongShiftEntity.JobtitleName = jobtitle != null ? jobtitle.JobTitleName : string.Empty;

                            reportWrongShiftEntity.OrgCode = org != null ? org.Code : string.Empty;
                            reportWrongShiftEntity.Date = date;
                            reportWrongShiftEntity.DateFrom = dateStart;
                            reportWrongShiftEntity.DateTo = dateEnd;
                            reportWrongShiftEntity.DateExport = DateTime.Now;
                            reportWrongShiftEntity.UserExport = userExport;
                            //reportWrongShiftEntity.UserExport = Session[SessionObjects.UserLogin];

                            reportWrongShiftEntity.ShiftName = shift != null ? shift.ShiftName : string.Empty;
                            reportWrongShiftEntity.ScheduleShift = shift != null ? shift.ShiftName : string.Empty;
                            reportWrongShiftEntity.ActualShift = shiftActual != null ? shiftActual.ShiftName : string.Empty;
                            reportWrongShiftEntity.ApprovedShift = shiftApprove != null ? shiftApprove.ShiftName : string.Empty;

                            reportWrongShiftEntity.Status = workDay.Status;
                            reportWrongShiftEntity.TimeIn = workDay.InTime1 != null ? workDay.InTime1 : null; ;
                            reportWrongShiftEntity.TimeOut = workDay.OutTime1 != null ? workDay.OutTime1 : null;
                            lstReportWrongShiftEntity.Add(reportWrongShiftEntity);
                        }
                    }

                }

                #region Code Cũ
                //foreach (var profile in profiles)
                //{

                //    for (DateTime date = dateStart.Date; date <= dateEnd.Date; date = date.AddDays(1))
                //    {
                //        var workDay = workDays.FirstOrDefault(s => s.WorkDate.Date == date.Date && s.ProfileID == profile.ID);
                //        if (workDay != null && workDay.ShiftActual != workDay.ShiftID)
                //        {
                //            reportWrongShiftEntity = new Att_ReportWrongShiftEntity();
                //            Guid? orgId = profile.OrgStructureID;
                //            #region
                //            // var workDay = workDays.FirstOrDefault(s => s.ProfileID == profile.ID && s.WorkDate.Date == date.Date);
                //           // var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                //            // var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                //            #endregion
                //            var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                //            var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                //            var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                //            #region
                //            var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                //            var orGroup = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);
                //            var orgDivision = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);

                //            reportWrongShiftEntity.BranchCode = orgBranch != null ? orgBranch.Code : string.Empty;
                //            reportWrongShiftEntity.BranchName = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                //            reportWrongShiftEntity.SectionCode = orgSection != null ? orgSection.Code : string.Empty;
                //            reportWrongShiftEntity.SectionName = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                //            reportWrongShiftEntity.GroupCode = orGroup != null ? orGroup.Code : string.Empty;
                //             reportWrongShiftEntity.GroupName = orGroup != null ? orGroup.OrgStructureName : string.Empty;
                //            reportWrongShiftEntity.Division = orgDivision != null ? orgDivision.Code : string.Empty;
                //            reportWrongShiftEntity.DivisionName = orgDivision != null ? orgDivision.OrgStructureName : string.Empty;
                //             reportWrongShiftEntity.TeamCode = orgTeam != null ? orgTeam.Code : string.Empty;
                //            reportWrongShiftEntity.TeamName = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                //            #endregion

                //            reportWrongShiftEntity.DepartmentCode = orgOrg != null ? orgOrg.Code : string.Empty;
                //            reportWrongShiftEntity.DepartmentName = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                //            var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                //            var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);

                //             reportWrongShiftEntity.PositionName = positon != null ? positon.PositionName : string.Empty;
                //             reportWrongShiftEntity.JobtitleName = jobtitle != null ? jobtitle.JobTitleName : string.Empty;
                //            var shift = shifts.FirstOrDefault(s => workDay != null && s.ID == workDay.ShiftID);
                //            var shiftActual = shifts.FirstOrDefault(s => s.ID == workDay.ShiftActual);
                //            var shiftApprove = shifts.FirstOrDefault(s => s.ID == workDay.ShiftApprove);


                //            reportWrongShiftEntity.ProfileName = profile.ProfileName;
                //            reportWrongShiftEntity.CodeEmp = profile.CodeEmp;
                //            reportWrongShiftEntity.Date = date;
                //            reportWrongShiftEntity.DateFrom = dateStart;
                //            reportWrongShiftEntity.DateTo = dateEnd;
                //            reportWrongShiftEntity.DateExport = DateTime.Now;

                //            reportWrongShiftEntity.ShiftName = shift != null ? shift.ShiftName : string.Empty;
                //            reportWrongShiftEntity.ScheduleShift = shift != null ? shift.ShiftName : string.Empty;
                //            reportWrongShiftEntity.ActualShift = shiftActual != null ? shiftActual.ShiftName : string.Empty;
                //            reportWrongShiftEntity.ApprovedShift = shiftApprove != null ? shiftApprove.ShiftName : string.Empty;

                //            reportWrongShiftEntity.Status = workDay.Status;
                //            reportWrongShiftEntity.TimeIn = workDay.InTime1 != null ? workDay.InTime1 : null; ;
                //            reportWrongShiftEntity.TimeOut = workDay.OutTime1 != null ? workDay.OutTime1 : null;
                //            lstReportWrongShiftEntity.Add(reportWrongShiftEntity);
                //        }
                //    }

                //}
                #endregion
                return lstReportWrongShiftEntity;
            }
        }


        #endregion


        public DataTable GetReportDailyAttendance(DateTime dateStart, DateTime dateEnd, string userExport, List<Hre_ProfileEntity> lstProfileAll, Guid[] payrollIDs, bool excludeNotInOut, bool isCreateTemplate, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable table = CreateReportDailyAttendanceSchema();
                if (isCreateTemplate)
                {
                    return table.ConfigDatatable();
                }

                List<Guid> lstProfileIDs = lstProfileAll.Select(s => s.ID).ToList();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Workday = new Att_WorkDayRepository(unitOfWork);
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                // var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                var repoWorkDay = new Att_WorkDayServices();
                var repoCat_Shift = new Cat_ShiftServices();
                var profiles = lstProfileAll.Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID, s.PayrollGroupID }).ToList();
                if (payrollIDs != null)
                {
                    profiles = profiles.Where(m => m.PayrollGroupID != null && payrollIDs.Contains(m.PayrollGroupID.Value)).ToList();
                }
                var profileIds = profiles.Select(s => s.ID).Distinct().ToList();
                string status = string.Empty;
                string E_APPROVED = WorkdayStatus.E_APPROVED.ToString();
                List<object> para_reportWorDay = new List<object>();
                para_reportWorDay.AddRange(new object[3]);
                para_reportWorDay[1] = dateStart;
                para_reportWorDay[2] = dateEnd;

                List<Att_WorkdayEntity> workDays = repoWorkDay.GetData<Att_WorkdayEntity>(para_reportWorDay, ConstantSql.hrm_att_sp_getdata_reportWorDay, UserLogin, ref status).ToList();
                workDays = workDays.Where(m => profileIds.Contains(m.ProfileID) && m.Status == E_APPROVED).ToList();
                var orgs = repoCat_OrgStructure.FindBy(s => s.IsDelete == null && s.Code != null).ToList();
                #region
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                var positions = repoCat_Position.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
                var jobtitles = repoCat_JobTitle.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();
                #endregion
                if (workDays == null || workDays.Count == 0)
                {
                    return table;
                }
                profileIds = workDays.Select(m => m.ProfileID).Distinct().ToList();
                profiles = profiles.Where(m => profileIds.Contains(m.ID)).ToList();
                List<object> para_repoCatShift = new List<object>();
                //para_repoCatShift.AddRange(new object[4]);
                //para_repoCatShift[0] = null;
                //para_repoCatShift[1] = null;
                //para_repoCatShift[2] = null;
                //para_repoCatShift[3] = null;
                List<Cat_ShiftEntity> lstShift = repoCat_Shift.GetData<Cat_ShiftEntity>(para_repoCatShift, ConstantSql.hrm_cat_sp_get_Shift, UserLogin, ref status).ToList();
                foreach (var item in workDays)
                {
                    DataRow row = table.NewRow();
                    row[Att_ReportDailyAttendanceEntity.FieldNames.DateFrom] = dateStart;
                    row[Att_ReportDailyAttendanceEntity.FieldNames.DateTo] = dateEnd;
                    var Profile = profiles.Where(m => m.ID == item.ProfileID).FirstOrDefault();
                    if (Profile != null)
                    {
                        row[Att_ReportDailyAttendanceEntity.FieldNames.CodeEmp] = Profile.CodeEmp != null ? Profile.CodeEmp : string.Empty;
                        row[Att_ReportDailyAttendanceEntity.FieldNames.ProfileName] = Profile.ProfileName != null ? Profile.ProfileName : string.Empty;
                        Guid? orgId = Profile.OrgStructureID;
                        if (orgId != null)
                        {

                            var org = orgs.FirstOrDefault(s => s.ID == Profile.OrgStructureID);
                            var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                            var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                            var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                            var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                            row[Att_ReportDailyAttendanceEntity.FieldNames.ProfileOrgCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                            row[Att_ReportDailyAttendanceEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                            row[Att_ReportDailyAttendanceEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                            row[Att_ReportDailyAttendanceEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                            row[Att_ReportDailyAttendanceEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                            row[Att_ReportDailyAttendanceEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                            row[Att_ReportDailyAttendanceEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                            row[Att_ReportDailyAttendanceEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                        }
                        var positon = positions.FirstOrDefault(s => s.ID == Profile.PositionID);
                        var jobtitle = jobtitles.FirstOrDefault(s => s.ID == Profile.JobTitleID);
                        row[Att_ReportDailyAttendanceEntity.FieldNames.PositionName] = positon != null ? positon.PositionName : string.Empty;
                        row[Att_ReportDailyAttendanceEntity.FieldNames.JobTitleName] = jobtitle != null ? jobtitle.JobTitleName : string.Empty;
                    }
                    var shift = lstShift.Where(m => m.ID == item.ShiftID).FirstOrDefault();
                    if (shift != null)
                    {
                        row[Att_ReportDailyAttendanceEntity.FieldNames.ShiftName] = shift.ShiftName;
                    }
                    var shiftActual = lstShift.Where(m => m.ID == item.ShiftActual).FirstOrDefault();
                    if (shiftActual != null)
                    {
                        row[Att_ReportDailyAttendanceEntity.FieldNames.ActualShift] = shiftActual.ShiftName;
                    }
                    if (item.WorkDate != null)
                    {
                        row[Att_ReportDailyAttendanceEntity.FieldNames.WorkDate] = item.WorkDate;
                    }
                    var shiftApprove = lstShift.Where(m => m.ID == item.ShiftApprove).FirstOrDefault();
                    if (shiftApprove != null)
                    {
                        row[Att_ReportDailyAttendanceEntity.FieldNames.ApprovedShift] = shiftApprove.ShiftName;
                    }
                    if (item.InTime1 != null)
                    {
                        row[Att_ReportDailyAttendanceEntity.FieldNames.TimeIn] = item.InTime1;
                    }
                    if (item.OutTime1 != null)
                    {
                        row[Att_ReportDailyAttendanceEntity.FieldNames.TimeOut] = item.OutTime1;
                    }
                    row[Att_ReportDailyAttendanceEntity.FieldNames.Type] = item.Type;
                    row[Att_ReportDailyAttendanceEntity.FieldNames.SrcType] = item.SrcType;
                    row[Att_ReportDailyAttendanceEntity.FieldNames.Status] = item.Status;
                    row[Att_ReportDailyAttendanceEntity.FieldNames.Note] = item.Note;
                    if (item.LateDuration1 != null)
                    {
                        row[Att_ReportDailyAttendanceEntity.FieldNames.LateDuration1] = item.LateDuration1;
                    }
                    if (item.EarlyDuration1 != null)
                    {
                        row[Att_ReportDailyAttendanceEntity.FieldNames.EarlyDuration1] = item.EarlyDuration1;
                    }
                    if (item.LateEarlyDuration != null)
                    {
                        row[Att_ReportDailyAttendanceEntity.FieldNames.LateEarlyDuration] = item.LateEarlyDuration;
                    }
                    if (item.LateEarlyRoot != null)
                    {
                        row[Att_ReportDailyAttendanceEntity.FieldNames.LateEarlyRoot] = item.LateEarlyRoot;
                    }

                    row[Att_ReportDailyAttendanceEntity.FieldNames.LateEarlyReason] = item.LateEarlyReason;
                    row[Att_ReportDailyAttendanceEntity.FieldNames.UserExport] = userExport;
                    row[Att_ReportDailyAttendanceEntity.FieldNames.DateExport] = DateTime.Today;
                    if (item.InTimeRoot != null)
                        row[Att_ReportDailyAttendanceEntity.FieldNames.InTimeRoot] = item.InTimeRoot.Value;
                    if (item.OutTimeRoot != null)
                        row[Att_ReportDailyAttendanceEntity.FieldNames.OutTimeRoot] = item.OutTimeRoot.Value;
                    if (item.LateEarlyDuration != null)
                    {
                        if (item.LateEarlyDuration.Value < 120)
                        {
                            row[Att_ReportDailyAttendanceEntity.FieldNames.EarlyLateLess2h] = "X";
                        }
                        else
                        {
                            row[Att_ReportDailyAttendanceEntity.FieldNames.EarlyLateOver2h] = "X";
                        }
                    }
                    else
                    {
                        row[Att_ReportDailyAttendanceEntity.FieldNames.EarlyLateLess2h] = "X";
                    }
                    table.Rows.Add(row);
                }
                return table;
            }
        }

        /// <summary>
        /// [Son.Vo] - 20140924
        /// Lấy ds tất cả báo cáo Chi Tiết quẹt thẻ
        /// </summary>
        /// <returns></returns>

        DataTable CreateReportDetailSwingCardSchema()
        {
            DataTable tb = new DataTable();
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.CardCode);
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.ProfileName);
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.WorkDate, typeof(DateTime));
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.TimeLog);
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.Status);
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.MachineNo);
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.ShiftName);
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.CodeBranch);
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.CodeDepartment);
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.CodeTeam);
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.CodeSection);
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.BranchName);
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.DepartmentName);
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.TeamName);
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.SectionName);
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.DateFrom, typeof(DateTime));
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.DateTo, typeof(DateTime));
            tb.Columns.Add(Att_ReportDetailSwingCardEntity.FieldNames.DateExport, typeof(DateTime));
            return tb;
        }
        public DataTable GetReportDetailSwingCard(DateTime dateStart, DateTime dateEnd, List<Hre_ProfileEntity> lstProfileAll, string CardCode, string[] sTatus, string strOrg, string userExport, string UserLogin)
        {
            dateEnd = dateEnd.Date.AddDays(1).AddMilliseconds(-1);
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_TAMScanLog = new CustomBaseRepository<Att_TAMScanLog>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_Position = new CustomBaseRepository<Cat_Position>(unitOfWork);
                var repoCat_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);
                var repoCat_OrgStructureType = new CustomBaseRepository<Cat_OrgStructureType>(unitOfWork);
                string status = string.Empty;
                List<object> lst3Param = new List<object>();
                lst3Param.Add(strOrg);
                lst3Param.Add(dateStart);
                lst3Param.Add(dateEnd);
                List<object> lst3ParamNon = new List<object>();
                lst3ParamNon.Add(null);
                lst3ParamNon.Add(dateStart);
                lst3ParamNon.Add(dateEnd);

                //string E_MANUAL = TAMScanStatus.E_MANUAL.ToString();
                //string E_LOADED = TAMScanStatus.E_LOADED.ToString();

                //var tamScanLogQuery = GetData<Att_TAMScanLogEntity>(lst3ParamNon, ConstantSql.hrm_att_getdata_TamScanLog, ref status)
                var tamScanLogQuery = repoAtt_TAMScanLog.FindBy(ts => ts.TimeLog != null && ts.TimeLog >= dateStart && ts.TimeLog <= dateEnd)
                                    .Select(s => new { s.CardCode, s.CodeEmp, s.TimeLog, s.MachineNo, s.Status, s.ProfileID, s.OrgStructureID }).ToList();
                if (sTatus != null && sTatus.Count() > 0)
                {
                    tamScanLogQuery = tamScanLogQuery.Where(m => sTatus.Contains(m.Status)).ToList();
                }
                if (!string.IsNullOrEmpty(CardCode))
                {
                    tamScanLogQuery = tamScanLogQuery.Where(s => s.CardCode == CardCode).ToList();
                }
                var tamScanlogs = tamScanLogQuery.Select(s => new { s.CardCode, s.CodeEmp, s.TimeLog, s.MachineNo, s.Status, s.ProfileID, s.OrgStructureID }).ToList();
                var workDays = GetData<Att_WorkdayEntity>(lst3Param, ConstantSql.hrm_att_getdata_Workday, UserLogin, ref status)
                                .Where(s => (s.InTimeRoot != null || s.OutTimeRoot != null)).Select(s => new { s.ProfileID, s.WorkDate, s.ShiftID }).ToList();

                var cardCodes = tamScanlogs.Select(s => s.CardCode).Distinct().ToList();
                var listCodeEmp = tamScanlogs.Select(s => s.CodeEmp).Distinct().ToList();

                var profiles = lstProfileAll.Where(s => (cardCodes.Contains(s.CodeAttendance) || listCodeEmp.Contains(s.CodeEmp)))
                                .Select(s => new { s.CodeAttendance, s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();

                var orgs = repoCat_OrgStructure.FindBy(s => s.IsDelete == null && s.Code != null).ToList();
                var positions = repoCat_Position.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => new { s.ID, s.Code }).ToList();
                var jobtitles = repoCat_JobTitle.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();
                var shifts = repoCat_Shift.FindBy(s => s.IsDelete == null).ToList();
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                DataTable table = CreateReportDetailSwingCardSchema();

                foreach (var tamScan in tamScanlogs)
                {
                    DataRow row = table.NewRow();
                    row[Att_ReportDetailSwingCardEntity.FieldNames.CardCode] = tamScan.CardCode;
                    row[Att_ReportDetailSwingCardEntity.FieldNames.CodeEmp] = tamScan.CodeEmp;

                    var profile = profiles.FirstOrDefault(s => s.ID == tamScan.ProfileID);

                    if (profile != null)
                    {
                        row[Att_ReportDetailSwingCardEntity.FieldNames.ProfileName] = profile.ProfileName;

                        var workDay = workDays.FirstOrDefault(s => s.ProfileID == profile.ID);
                        Guid? orgId = tamScan.OrgStructureID;
                        var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                        var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                        var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                        var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                        var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);

                        row[Att_ReportDetailSwingCardEntity.FieldNames.CodeBranch] = orgBranch != null ? orgBranch.Code : string.Empty;
                        row[Att_ReportDetailSwingCardEntity.FieldNames.CodeDepartment] = orgOrg != null ? orgOrg.Code : string.Empty;
                        row[Att_ReportDetailSwingCardEntity.FieldNames.CodeTeam] = orgTeam != null ? orgTeam.Code : string.Empty;
                        row[Att_ReportDetailSwingCardEntity.FieldNames.CodeSection] = orgSection != null ? orgSection.Code : string.Empty;
                        row[Att_ReportDetailSwingCardEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                        row[Att_ReportDetailSwingCardEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                        row[Att_ReportDetailSwingCardEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                        row[Att_ReportDetailSwingCardEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                        if (workDay != null)
                        {
                            row[Att_ReportDetailSwingCardEntity.FieldNames.ShiftName] = shifts.FirstOrDefault(s => s.ID == workDay.ShiftID);
                        }
                    }
                    row[Att_ReportDetailSwingCardEntity.FieldNames.DateFrom] = dateStart;
                    row[Att_ReportDetailSwingCardEntity.FieldNames.DateTo] = dateEnd;
                    row[Att_ReportDetailSwingCardEntity.FieldNames.DateExport] = DateTime.Now;
                    if (tamScan.TimeLog != null)
                    {
                        row[Att_ReportDetailSwingCardEntity.FieldNames.WorkDate] = tamScan.TimeLog;
                    }
                    if (tamScan.TimeLog != null)
                    {
                        row[Att_ReportDetailSwingCardEntity.FieldNames.TimeLog] = tamScan.TimeLog.Value.ToString("dd/MM/yyyy HH:mm:ss");
                    }
                    row[Att_ReportDetailSwingCardEntity.FieldNames.Status] = tamScan.Status;
                    row[Att_ReportDetailSwingCardEntity.FieldNames.MachineNo] = tamScan.MachineNo;
                    table.Rows.Add(row);
                }

                return table;
            }
        }

        /// <summary>
        /// Lấy Dữ Liệu BC BẢNG CÔNG HÀNG THÁNG
        /// </summary>
        /// <returns></returns>

        DataTable CreateReportMonthlyTimeSheetSchema(string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tb = new DataTable();

                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.ProfileName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.CodeOrg);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.DepartmentCode);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.BranchCode);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.TeamCode);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.SectionCode);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.DepartmentName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.BranchName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.TeamName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.SectionName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.PositionName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.JobTitleName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.CodeCenter);

                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.StdWorkDayCount);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.RealWorkDayCount);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.PaidWorkDayCount);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.AnlDayAvailable);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.NightShiftHours);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.LateEarlyDeductionHours);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.AnlDayTaken);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.Note);
                #region MyRegion
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.UnPaidLeave);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.Allowance1);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.Allowance2);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.COBeginPeriod);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.COEndPeriod);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.UsableLeave);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.CurrentMonth);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.YearToDate);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.SickLeave);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.SickYearToDate);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.CountLateLess2H);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.CountLateMore2H);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.SumMuteLateEarly);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.CountLateEarly);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.SumCODayOT);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.CountCOLeavday);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.SickCurrentMonth);
                #endregion

                #region MyRegion
                var leavedayTypeServices = new Cat_LeaveDayTypeServices();
                var lstObjLeavedayType = new List<object>();
                lstObjLeavedayType.Add(null);
                lstObjLeavedayType.Add(null);
                lstObjLeavedayType.Add(1);
                lstObjLeavedayType.Add(100000000);
                var lstLeavedayType = leavedayTypeServices.GetData<Cat_LeaveDayTypeEntity>(lstObjLeavedayType, ConstantSql.hrm_cat_sp_get_LeaveDayType, UserLogin, ref status).Where(s => s.Code != null).Select(m => m.Code).ToList();

                var overtimeTypeServices = new Cat_OvertimeTypeServices();
                var lstObjOvertimeType = new List<object>();
                lstObjOvertimeType.Add(null);
                lstObjOvertimeType.Add(null);
                lstObjOvertimeType.Add(null);
                lstObjOvertimeType.Add(1);
                lstObjOvertimeType.Add(10000000);
                var lstOvertimeType = overtimeTypeServices.GetData<Cat_OvertimeTypeEntity>(lstObjOvertimeType, ConstantSql.hrm_cat_sp_get_OvertimeType, UserLogin, ref status).Where(s => s.Code != null && s.OvertimeTypeName != null).Select(m => m.OvertimeTypeName).ToList();

                lstLeavedayType.AddRange(lstOvertimeType);

                foreach (var item in lstLeavedayType)
                {
                    if (!tb.Columns.Contains(item))
                    {
                        tb.Columns.Add(item);
                    }
                }
                #endregion


                return tb;
            }
        }

        DataTable CreateReportMonthlyTimeSheetV2Schema(string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                string status = string.Empty;
                DataTable tb = new DataTable("ReportMonthlyTimeSheetV2");

                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DateHire, typeof(DateTime));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DateEndProbation, typeof(DateTime));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.CodeEmp);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.ProfileName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DepartmentCode);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.PositionName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.JobTitleName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.StdWorkDayCount, typeof(Double));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.RealWorkDayCount, typeof(Double));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.PaidWorkDayCount, typeof(Double));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.HourPerDay, typeof(Double));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.AnlDayAvailable, typeof(Double));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.NightShiftHours, typeof(Double));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.LateEarlyDeductionHours, typeof(Double));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.AnlDayTaken, typeof(Double));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Note);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.ResignedDate, typeof(DateTime));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.StartingDate, typeof(DateTime));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DateFrom, typeof(DateTime));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DateTo, typeof(DateTime));

                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.UnPaidLeave, typeof(Double));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.SickDayTaken, typeof(Double));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.TotalPregnancy, typeof(Double));

                //tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Note);

                for (int i = 1; i <= 45; i++)
                {
                    DataColumn column = new DataColumn(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Data + i);
                    column.Caption = i.ToString();

                    tb.Columns.Add(column);
                }

                #region MyRegion

                var leavedayTypeServices = new Cat_LeaveDayTypeServices();
                //var lstObjLeavedayType = new List<object>();
                //lstObjLeavedayType.Add(null);
                //lstObjLeavedayType.Add(null);
                //lstObjLeavedayType.Add(1);
                //lstObjLeavedayType.Add(100000000);
                //var lstLeavedayType = leavedayTypeServices.GetData<Cat_LeaveDayTypeEntity>(lstObjLeavedayType, ConstantSql.hrm_cat_sp_get_LeaveDayType, ref status).Where(s => s.CodeStatistic != null).Select(m => m.CodeStatistic).ToList();
                var lstLeavedayType = unitOfWork.CreateQueryable<Cat_LeaveDayType>(Guid.Empty, s => s.CodeStatistic != null).Select(m => m.CodeStatistic).ToList();

                foreach (var item in lstLeavedayType)
                {
                    if (!tb.Columns.Contains(item))
                    {
                        tb.Columns.Add(item, typeof(double));
                    }
                }

                var lstOvertimeType = unitOfWork.CreateQueryable<Cat_OvertimeType>(Guid.Empty, s => s.CodeStatistic != null).Select(m => m.CodeStatistic).ToList();

                foreach (var item in lstOvertimeType)
                {
                    if (!tb.Columns.Contains(item))
                    {
                        tb.Columns.Add(item, typeof(double));
                    }
                }

                #endregion
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.BranchCode);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.TeamCode);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.SectionCode);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DepartmentName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.BranchName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.TeamName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.SectionName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Channel);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Region);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Area);

                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_COMPANY);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_BRANCH);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_UNIT);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_DIVISION);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_DEPARTMENT);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_TEAM);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_SECTION);


                var jobTypeServices = new Cat_JobTypeServices();
                var lstjobType = new List<object>();
                lstjobType.Add(null);
                lstjobType.Add(null);
                lstjobType.Add(1);
                lstjobType.Add(100000000);
                var listjobType = jobTypeServices.GetData<Cat_JobTypeEntity>(lstjobType, ConstantSql.hrm_cat_sp_get_JobType, UserLogin, ref status).Where(s => s.Code != null).Select(m => m.Code).ToList();
                foreach (var item in listjobType)
                {
                    if (!tb.Columns.Contains(item))
                    {
                        tb.Columns.Add(item, typeof(double));
                        tb.Columns.Add(item + "_Sector", typeof(double));
                    }
                }
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.UserExport);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DateExport, typeof(DateTime));
                return tb;
            }
        }
        DataTable CreateReportDailyAttendanceSchema()
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tb = new DataTable("Att_ReportDailyAttendanceEntity");
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.ProfileName);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.DepartmentCode);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.ShiftName);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.ActualShift);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.ApprovedShift);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.TimeIn, typeof(DateTime));
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.TimeOut, typeof(DateTime));
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.WorkDate, typeof(DateTime));
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.Type);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.SrcType);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.Status);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.Note);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.LateDuration1);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.EarlyDuration1);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.LateEarlyDuration);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.LateEarlyRoot);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.LateEarlyReason);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.InTimeRoot);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.OutTimeRoot);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.EarlyLateLess2h);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.EarlyLateOver2h);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.SectionCode);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.BranchCode);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.TeamCode);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.ProfileOrgCode);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.BranchName);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.TeamName);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.SectionName);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.JobTitleName);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.UserExport);
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.DateExport, typeof(DateTime));
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.DateFrom, typeof(DateTime));
                tb.Columns.Add(Att_ReportDailyAttendanceEntity.FieldNames.DateTo, typeof(DateTime));
                return tb;

            }
        }

        public DataTable GetReportMonthlyTimeSheet(DateTime? dateStart, DateTime DateTo, string strOrgStructure, List<Hre_ProfileEntity> profiles, Guid[] shiftIDs, Guid[] payrollIDs, bool isIncludeQuitEmp, bool isNotAllowZero, Guid exportid, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable table = CreateReportMonthlyTimeSheetSchema(UserLogin);

                dateStart = new DateTime(dateStart.Value.Year, dateStart.Value.Month, 1);
                DateTime monthEnd = new DateTime(dateStart.Value.Year, dateStart.Value.Month, DateTime.DaysInMonth(dateStart.Value.Year, dateStart.Value.Month));
                monthEnd = monthEnd.AddDays(1).AddMilliseconds(-1);
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var positions = repoCat_Position.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();
                var repoCat_LeaveDayType = new Cat_LeaveDayTypeRepository(unitOfWork);
                var leavedayTypes = repoCat_LeaveDayType.FindBy(s => s.Code != null && s.IsDelete == null).Select(s => new { s.Code, s.ID, s.PaidRate, s.LeaveDayTypeName }).ToList();
                var repoCat_OvertimeType = new Cat_OvertimeTypeRepository(unitOfWork);
                var overtimTypes = repoCat_OvertimeType.FindBy(s => s.Code != null && s.IsDelete == null).Select(s => new { s.ID, s.Code, s.OvertimeTypeName }).ToList();
                var codeCenters = unitOfWork.CreateQueryable<Cat_CostCentre>(Guid.Empty, s => s.Code != null).Select(s => new { s.ID, s.Code }).ToList();
                List<Guid> profileIds = profiles.Select(s => s.ID).ToList();
                string status = string.Empty;
                List<object> para_reportWorDay = new List<object>();
                para_reportWorDay.AddRange(new object[2]);
                para_reportWorDay[0] = dateStart;
                para_reportWorDay[0] = monthEnd;
                var repoWorkDay = new Att_WorkDayServices();
                List<Att_WorkdayEntity> workDays = repoWorkDay.GetData<Att_WorkdayEntity>(para_reportWorDay, ConstantSql.hrm_att_sp_getdata_reportWorDay, UserLogin, ref status).ToList();
                workDays = workDays.Where(s => profileIds.Contains(s.ProfileID)).ToList();
                if (payrollIDs != null)
                {
                    profiles = profiles.Where(s => s.PayrollGroupID != null && payrollIDs.Contains(s.PayrollGroupID.Value)).ToList();
                }
                if (shiftIDs != null)
                {
                    workDays = workDays.Where(s => s.ShiftID.HasValue && shiftIDs.Contains(s.ShiftID.Value)).ToList();
                    profileIds = workDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }
                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateStart).ToList();
                }
                profileIds = profiles.Select(s => s.ID).ToList();
                var repoAtt_AttendencaTable = new Att_AttendanceTableRepository(unitOfWork);
                List<object> para = new List<object>();
                para.AddRange(new object[7]);
                para[3] = dateStart;
                para[4] = monthEnd;
                para[5] = 1;
                para[6] = int.MaxValue;

                var attService = new Att_AttendanceServices();
                List<Att_AttendanceTableEntity> lstAttendanceTable = attService.GetData<Att_AttendanceTableEntity>(para, ConstantSql.hrm_att_sp_get_attdancetable, UserLogin, ref status).ToList();
                var attendanceTables = lstAttendanceTable.Where(s => profileIds.Contains(s.ProfileID)).ToList();

                profileIds = attendanceTables.Select(s => s.ProfileID).ToList();
                profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                foreach (var profile in profiles)
                {
                    var attendanceTableProfile = attendanceTables.FirstOrDefault(s => s.ProfileID == profile.ID);
                    if (attendanceTableProfile == null)
                    {
                        continue;
                    }
                    DataRow row = table.NewRow();
                    Guid? orgId = profile.OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);

                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);

                    var cost = codeCenters.FirstOrDefault(s => s.ID == profile.CostCentreID);
                    var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                    var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);

                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.ProfileName] = profile.ProfileName;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.CodeOrg] = orgOrg != null ? orgOrg.Code : string.Empty;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.PositionName] = positon != null ? positon.PositionName : string.Empty;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.JobTitleName] = jobtitle != null ? jobtitle.JobTitleName : string.Empty;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.CodeCenter] = cost != null ? cost.Code : string.Empty;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.StdWorkDayCount] = attendanceTableProfile.StdWorkDayCount > 0 ? (object)attendanceTableProfile.StdWorkDayCount : DBNull.Value;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.RealWorkDayCount] = attendanceTableProfile.RealWorkDayCount > 0 ? (object)attendanceTableProfile.RealWorkDayCount : DBNull.Value;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.PaidWorkDayCount] = attendanceTableProfile.PaidWorkDayCount > 0 ? (object)attendanceTableProfile.PaidWorkDayCount : DBNull.Value;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.AnlDayAvailable] = attendanceTableProfile.AnlDayAvailable > 0 ? (object)attendanceTableProfile.AnlDayAvailable : DBNull.Value;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.NightShiftHours] = attendanceTableProfile.NightShiftHours > 0 ? (object)attendanceTableProfile.NightShiftHours : DBNull.Value;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.LateEarlyDeductionHours] = attendanceTableProfile.LateEarlyDeductionHours > 0 ? (object)attendanceTableProfile.LateEarlyDeductionHours : DBNull.Value;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.AnlDayTaken] = attendanceTableProfile.AnlDayTaken > 0 ? (object)attendanceTableProfile.AnlDayTaken : DBNull.Value;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.Note] = attendanceTableProfile.Note != null ? attendanceTableProfile.Note : string.Empty;
                    #region Export theo cot dong
                    if (exportid != Guid.Empty)
                    {
                        var leaday = leavedayTypes.FirstOrDefault(s => s.ID == attendanceTableProfile.LeaveDay1Type);
                        if (leaday != null && !string.IsNullOrEmpty(leaday.Code) && table.Columns.Contains(leaday.Code))
                        {
                            row[leaday.Code] = attendanceTableProfile.LeaveDay1Hours > 0 ? (object)attendanceTableProfile.LeaveDay1Hours : DBNull.Value;
                        }
                        leaday = leavedayTypes.FirstOrDefault(s => s.ID == attendanceTableProfile.LeaveDay2Type);
                        if (leaday != null)
                        {
                            row[leaday.Code] = attendanceTableProfile.LeaveDay2Hours > 0 ? (object)attendanceTableProfile.LeaveDay2Hours : DBNull.Value;
                        }
                        leaday = leavedayTypes.FirstOrDefault(s => s.ID == attendanceTableProfile.LeaveDay3Type);
                        if (leaday != null)
                        {
                            row[leaday.Code] = attendanceTableProfile.LeaveDay3Hours > 0 ? (object)attendanceTableProfile.LeaveDay3Hours : DBNull.Value;
                        }
                        leaday = leavedayTypes.FirstOrDefault(s => s.ID == attendanceTableProfile.LeaveDay4Type);
                        if (leaday != null)
                        {
                            row[leaday.Code] = attendanceTableProfile.LeaveDay4Hours > 0 ? (object)attendanceTableProfile.LeaveDay4Hours : DBNull.Value;
                        }
                        var overtimeType = overtimTypes.FirstOrDefault(s => s.ID == attendanceTableProfile.Overtime1Type);
                        if (overtimeType != null)
                        {
                            // row[overtimeType.Code] = attendanceTableProfile.Overtime1Hours > 0 ? (object)attendanceTableProfile.Overtime1Hours : DBNull.Value;
                            row[overtimeType.OvertimeTypeName] = attendanceTableProfile.Overtime1Hours > 0 ? (object)attendanceTableProfile.Overtime1Hours : DBNull.Value;
                        }
                        overtimeType = overtimTypes.FirstOrDefault(s => s.ID == attendanceTableProfile.Overtime2Type);
                        if (overtimeType != null)
                        {
                            // row[overtimeType.Code] = attendanceTableProfile.Overtime2Hours > 0 ? (object)attendanceTableProfile.Overtime2Hours : DBNull.Value;
                            row[overtimeType.OvertimeTypeName] = attendanceTableProfile.Overtime2Hours > 0 ? (object)attendanceTableProfile.Overtime2Hours : DBNull.Value;
                        }
                        overtimeType = overtimTypes.FirstOrDefault(s => s.ID == attendanceTableProfile.Overtime3Type);
                        if (overtimeType != null)
                        {
                            //row[overtimeType.Code] = attendanceTableProfile.Overtime3Hours > 0 ? (object)attendanceTableProfile.Overtime3Hours : DBNull.Value;
                            row[overtimeType.OvertimeTypeName] = attendanceTableProfile.Overtime3Hours > 0 ? (object)attendanceTableProfile.Overtime3Hours : DBNull.Value;
                        }
                        overtimeType = overtimTypes.FirstOrDefault(s => s.ID == attendanceTableProfile.Overtime4Type);
                        if (overtimeType != null)
                        {
                            //row[overtimeType.Code] = attendanceTableProfile.Overtime4Hours > 0 ? (object)attendanceTableProfile.Overtime4Hours : DBNull.Value;
                            row[overtimeType.OvertimeTypeName] = attendanceTableProfile.Overtime4Hours > 0 ? (object)attendanceTableProfile.Overtime4Hours : DBNull.Value;
                        }
                        overtimeType = overtimTypes.FirstOrDefault(s => s.ID == attendanceTableProfile.Overtime5Type);
                        if (overtimeType != null)
                        {
                            //row[overtimeType.Code] = attendanceTableProfile.Overtime5Hours > 0 ? (object)attendanceTableProfile.Overtime5Hours : DBNull.Value;
                            row[overtimeType.OvertimeTypeName] = attendanceTableProfile.Overtime5Hours > 0 ? (object)attendanceTableProfile.Overtime5Hours : DBNull.Value;
                        }
                        overtimeType = overtimTypes.FirstOrDefault(s => s.ID == attendanceTableProfile.Overtime6Type);
                        if (overtimeType != null)
                        {
                            //row[overtimeType.Code] = attendanceTableProfile.Overtime6Hours > 0 ? (object)attendanceTableProfile.Overtime6Hours : DBNull.Value;
                            row[overtimeType.OvertimeTypeName] = attendanceTableProfile.Overtime6Hours > 0 ? (object)attendanceTableProfile.Overtime6Hours : DBNull.Value;
                        }



                        row[Att_ReportMonthlyTimeSheetEntity.FieldNames.CountLateEarly] = (object)attendanceTableProfile.CardMissingCount ?? DBNull.Value;
                        row[Att_ReportMonthlyTimeSheetEntity.FieldNames.CountLateLess2H] = (object)attendanceTableProfile.LateEarlyLeastCount ?? DBNull.Value;
                        row[Att_ReportMonthlyTimeSheetEntity.FieldNames.CountLateMore2H] = (object)attendanceTableProfile.LateEarlyGreaterCount ?? DBNull.Value;
                        row[Att_ReportMonthlyTimeSheetEntity.FieldNames.UsableLeave] = attendanceTableProfile.TotalAnlDayAvailable > 0 ? (object)attendanceTableProfile.TotalAnlDayAvailable : DBNull.Value;
                        row[Att_ReportMonthlyTimeSheetEntity.FieldNames.SickLeave] = attendanceTableProfile.TotalSickDayAvailable > 0 ? (object)attendanceTableProfile.TotalSickDayAvailable : DBNull.Value;
                        row[Att_ReportMonthlyTimeSheetEntity.FieldNames.CurrentMonth] = attendanceTableProfile.AnlDayTaken > 0 ? (object)attendanceTableProfile.AnlDayTaken : DBNull.Value;
                        //row["SickCurrentMonth"] = attendanceTableProfile.SickDayTaken > 0 ? (object)attendanceTableProfile.SickDayTaken : DBNull.Value;
                        row[Att_ReportMonthlyTimeSheetEntity.FieldNames.SickCurrentMonth] = attendanceTableProfile.SickDayTaken > 0 ? (object)attendanceTableProfile.SickDayTaken : DBNull.Value;
                        row[Att_ReportMonthlyTimeSheetEntity.FieldNames.YearToDate] = attendanceTableProfile.AnlDayAdjacent > 0 ? (object)attendanceTableProfile.AnlDayAdjacent : DBNull.Value;
                        row[Att_ReportMonthlyTimeSheetEntity.FieldNames.SickYearToDate] = attendanceTableProfile.SickDayAdjacent > 0 ? (object)attendanceTableProfile.SickDayAdjacent : DBNull.Value;
                        row[Att_ReportMonthlyTimeSheetEntity.FieldNames.SumMuteLateEarly] = (object)attendanceTableProfile.LateEarlyTotal ?? DBNull.Value;
                    }
                    #endregion
                    table.Rows.Add(row);
                }
                if (exportid != Guid.Empty)
                {
                    return table.ConfigDatatable();
                }
                return table;
            }
        }

        public DataTable GetReportMonthlyTimeSheetV2(string WorkHourType, Guid? GradeAttendanceID, Guid CutOffDurationID, 
            string strProfile, string strOrgStructure, string codeEmp, bool isIncludeQuitEmp, bool isNotAllowZero,
            bool isCreateTemplate, string userExport, string UserLogin, Guid? jobtitleID, Guid? positionID)
        {
            bool IsPaidHour = WorkHourType == EnumDropDown.WorkHourType.E_PAIDHOUR.ToString() ? true : false;
            using (var context = new VnrHrmDataContext())
            {
                DataTable table = CreateReportMonthlyTimeSheetV2Schema(UserLogin);
                if (isCreateTemplate)
                {
                    return table.ConfigDatatable();
                }
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var repoWorkDay = new Att_WorkDayRepository(unitOfWork);
                var repoTimeSheet = new Att_TimeSheetRepository(unitOfWork);
                var repoJobtype = new Cat_JobTypeRepository(unitOfWork);
                var repoAtt_AttendencaTable = new CustomBaseRepository<Att_AttendanceTable>(unitOfWork);
                var repoAtt_AttendanceTableItem = new CustomBaseRepository<Att_AttendanceTableItem>(unitOfWork);
                var repoAtt_CutOffDuration = new CustomBaseRepository<Att_CutOffDuration>(unitOfWork);

                var orgsService = new Cat_OrgStructureServices();
                List<object> objorgs = new List<object>();
                var lstallorgs = orgsService.GetDataNotParam<Cat_OrgStructure>(ConstantSql.hrm_cat_sp_get_AllOrg, UserLogin, ref status).ToList();

                var orgTypeService = new Cat_OrgStructureTypeServices();
                var lstObjOrgType = new List<object>();
                lstObjOrgType.Add(null);
                lstObjOrgType.Add(null);
                lstObjOrgType.Add(1);
                lstObjOrgType.Add(int.MaxValue - 1);
                var lstOrgType = orgTypeService.GetData<Cat_OrgStructureType>(lstObjOrgType, ConstantSql.hrm_cat_sp_get_OrgStructureType, UserLogin, ref status);

                var jobTypeServices = new Cat_JobTypeServices();
                var lstjobType = new List<object>();
                lstjobType.Add(null);
                lstjobType.Add(null);
                lstjobType.Add(1);
                lstjobType.Add(Int32.MaxValue - 1);
                var listjobType = jobTypeServices.GetData<Cat_JobTypeEntity>(lstjobType, ConstantSql.hrm_cat_sp_get_JobType, UserLogin, ref status).Where(s => s.Code != null).Select(m => new { m.ID, m.Code }).ToList();

                var lstObjOrgByOrderNumberCount = new List<object>();
                lstObjOrgByOrderNumberCount.Add(strOrgStructure);
                var lstOrgByOrderNumberCount = orgsService.GetData<Cat_OrgStructure>(lstObjOrgByOrderNumberCount, ConstantSql.hrm_cat_sp_get_OrgStructureByOrderNumber, UserLogin, ref status).ToList();

                List<Hre_ProfileEntity> profiles = new List<Hre_ProfileEntity>();
                List<object> lstParamHR = new List<object>();
                lstParamHR.Add(strOrgStructure);
                lstParamHR.Add(null);
                lstParamHR.Add(null);
                if (strProfile != null)
                {
                    List<Hre_ProfileEntity> _profileIDs = new List<Hre_ProfileEntity>();
                    var lst = strProfile.Split(',').Select(s => Guid.Parse(s)).ToList();

                    if (strOrgStructure != null)
                    {
                        profiles = GetData<Hre_ProfileEntity>(strOrgStructure, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status).ToList();
                        _profileIDs = profiles.Where(m => !lst.Contains(m.ID)).ToList();
                        profiles.AddRange(_profileIDs);
                    }
                    else
                    {
                        string selectedIds = Common.DotNetToOracle(strProfile);
                        profiles = GetData<Hre_ProfileEntity>(selectedIds, ConstantSql.hrm_hr_sp_get_ProfileByIds, UserLogin, ref status).ToList();
                    }
                }
                else
                {
                    profiles = GetData<Hre_ProfileEntity>(lstParamHR, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status).ToList();
                }

                if (GradeAttendanceID != null && GradeAttendanceID != Guid.Empty)
                {
                    var lstObjAttGrade = new List<object>();
                    lstObjAttGrade.AddRange(new object[6]);
                    lstObjAttGrade[1] = Common.DotNetToOracle(GradeAttendanceID.Value.ToString());
                    lstObjAttGrade[4] = 1;
                    lstObjAttGrade[5] = int.MaxValue - 1;
                    var lstAttGrade = GetData<Att_GradeEntity>(lstObjAttGrade, ConstantSql.hrm_att_sp_get_Att_Grade, UserLogin, ref status).ToList();

                    List<Guid> lstProOfGrade = lstAttGrade.Select(s => s.ProfileID.Value).Distinct().ToList();

                    profiles = profiles.Where(s => lstProOfGrade.Contains(s.ID)).ToList();
                }
                var cutoffInfo = repoAtt_CutOffDuration.FindBy(s => s.ID == CutOffDurationID).FirstOrDefault();
                var leavedayTypes = unitOfWork.CreateQueryable<Cat_LeaveDayType>(Guid.Empty, s => s.CodeStatistic != null).Select(s => new { s.Code, s.CodeStatistic, s.ID, s.PaidRate, s.LeaveDayTypeName }).ToList();
                var overtimeTypes = unitOfWork.CreateQueryable<Cat_OvertimeType>(Guid.Empty, s => s.CodeStatistic != null).ToList();
                DateTime dateStart = cutoffInfo.DateStart;
                DateTime dateEnd = cutoffInfo.DateEnd;

                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateStart).ToList();
                }

                if(jobtitleID != null)
                {
                    profiles = profiles.Where(s => s.JobTitleID == jobtitleID).ToList();
                }

                if (positionID != null)
                {
                    profiles = profiles.Where(s => s.PositionID == positionID).ToList();
                }

                List<Guid> profileIds = profiles.Select(s => s.ID).ToList();
                var timesheets = new List<Att_TimeSheet>().Select(s => new
                {
                    s.ProfileID,
                    s.JobTypeID,
                    s.RoleID,
                    s.Date,
                    s.NoHour,
                    s.ID,
                    s.Note,
                    s.Sector
                }).ToList();

                var workDays = new List<Att_Workday>().Select(s => new
                {
                    s.ProfileID,
                    s.ShiftID,
                    s.Status
                }).ToList();

                var attendanceTables = new List<Att_AttendanceTable>();
                var attendanceTableItems = new List<Att_AttendanceTableItem>();

                foreach (var item in profileIds.Chunk(1000))
                {
                    timesheets.AddRange(repoTimeSheet.FindBy(s => s.ProfileID != null && item.Contains(s.ProfileID.Value)
                        && s.Date != null && dateStart <= s.Date && s.Date <= dateEnd && s.IsDelete == null).Select(s => new
                        {
                            s.ProfileID,
                            s.JobTypeID,
                            s.RoleID,
                            s.Date,
                            s.NoHour,
                            s.ID,
                            s.Note,
                            s.Sector
                        }).ToList());

                    workDays.AddRange(repoWorkDay.FindBy(s => item.Contains(s.ProfileID) && dateStart <= s.WorkDate
                        && s.WorkDate <= dateEnd && s.IsDelete == null).Select(s => new
                        {
                            s.ProfileID,
                            s.ShiftID,
                            s.Status
                        }).ToList());

                    attendanceTables.AddRange(repoAtt_AttendencaTable.FindBy(s => s.IsDelete == null
                        && s.CutOffDurationID == CutOffDurationID && item.Contains(s.ProfileID)).ToList());
                }

                var jobtypes = repoJobtype.FindBy(s => s.IsDelete == null).Select(s => new { s.ID, s.Code }).ToList();
                List<Guid> attendanceTablesID = attendanceTables.Select(s => s.ID).ToList();

                foreach (var item in attendanceTablesID.Chunk(1000))
                {
                    attendanceTableItems.AddRange(unitOfWork.CreateQueryable<Att_AttendanceTableItem>(s =>
                        s.IsDelete == null && item.Contains(s.AttendanceTableID)).ToList());
                }

                if (isNotAllowZero)
                {
                    profileIds = attendanceTables.Where(x => x.StdWorkDayCount > 0).Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }

                profileIds = attendanceTables.Select(s => s.ProfileID).ToList();
                profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();

                int p = 0;
                foreach (var profile in profiles)
                {
                    p += 1;
                    var attendanceTableProfile = attendanceTables.FirstOrDefault(s => s.ProfileID == profile.ID);
                    var timeSheetByProfile = timesheets.Where(s => s.ProfileID == profile.ID).ToList();

                    //if (timeSheetByProfile == null)
                    //{
                    //    continue;
                    //}
                    var lstjobtypebytimesheetpro = timeSheetByProfile.Select(s => s.JobTypeID).ToList();
                    var lstJobTypeCodeByTimeSheet = jobtypes.Where(s => lstjobtypebytimesheetpro.Contains(s.ID)).Select(s => new { s.ID, s.Code }).ToList();
                    if (attendanceTableProfile == null)
                    {
                        continue;
                    }
                    var attendanceTableItemProfile = attendanceTableItems.Where(s => s.AttendanceTableID == attendanceTableProfile.ID).ToList();
                    DataRow row = table.NewRow();
                    //Guid? orgId = profile.OrgStructureID;
                    //var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);

                    //var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    //var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    //var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    //var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);

                    //var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                    //var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);

                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.ProfileName] = profile.ProfileName;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.CodeEmp] = profile.CodeEmp;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DepartmentName] = profile.OrgStructureName;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DepartmentCode] = profile.OrgStructureCode;

                    if (profile.DateHire.HasValue)
                        row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DateHire] = profile.DateHire.Value;
                    if (profile.DateEndProbation.HasValue)
                        row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DateEndProbation] = profile.DateEndProbation.Value;
                    if (profile.E_COMPANY != null)
                        row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_COMPANY] = profile.E_COMPANY;
                    if (profile.E_BRANCH != null)
                        row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_BRANCH] = profile.E_BRANCH;
                    if (profile.E_UNIT != null)
                        row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_UNIT] = profile.E_UNIT;
                    if (profile.E_DIVISION != null)
                        row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_DIVISION] = profile.E_DIVISION;
                    if (profile.E_DEPARTMENT != null)
                        row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_DEPARTMENT] = profile.E_DEPARTMENT;
                    if (profile.E_TEAM != null)
                        row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_TEAM] = profile.E_TEAM;
                    if (profile.E_SECTION != null)
                        row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_SECTION] = profile.E_SECTION;



                    //row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                    //row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                    //row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                    //row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                    //row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    //row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    //row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;



                    #region Lấy Tên Phòng ban cho BDF
                    var orgName = GetParentOrg(lstallorgs, lstOrgType, profile.OrgStructureID);
                    if (orgName.Count != 0)
                    {
                        if (orgName.Count < 3)
                        {
                            orgName.Insert(0, string.Empty);
                            if (orgName.Count < 3)
                            {
                                orgName.Insert(0, string.Empty);
                            }
                        }
                        row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Channel] = orgName[2];
                        row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Region] = orgName[1];
                        row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Area] = orgName[0];
                    }
                    #endregion

                    #region Lấy phòng ban cho VietNam EASport

                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_COMPANY] = profile.E_COMPANY;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_BRANCH] = profile.E_BRANCH;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_UNIT] = profile.E_UNIT;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_DIVISION] = profile.E_DIVISION;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_DEPARTMENT] = profile.E_DEPARTMENT;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_TEAM] = profile.E_TEAM;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.E_SECTION] = profile.E_SECTION;

                    #endregion

                    //row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.PositionName] = positon != null ? positon.PositionName : string.Empty;
                    //row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.JobTitleName] = jobtitle != null ? jobtitle.JobTitleName : string.Empty;
                    if (profile.DateHire != null)
                        row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.StartingDate] = profile.DateHire.Value;
                    if (profile.DateQuit != null)
                        row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.ResignedDate] = profile.DateQuit.Value;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DateFrom] = dateStart;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DateTo] = dateEnd;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DateExport] = DateTime.Today;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.UserExport] = userExport;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.StdWorkDayCount] = attendanceTableProfile.StdWorkDayCount > 0 ? attendanceTableProfile.StdWorkDayCount : 0.0;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.RealWorkDayCount] = attendanceTableProfile.RealWorkDayCount > 0 ? attendanceTableProfile.RealWorkDayCount : 0.0;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.PaidWorkDayCount] = attendanceTableProfile.PaidWorkDayCount > 0 ? attendanceTableProfile.PaidWorkDayCount : 0.0;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.HourPerDay] = attendanceTableProfile.HourPerDay > 0 ? attendanceTableProfile.HourPerDay : 0.0;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.AnlDayAvailable] = attendanceTableProfile.AnlDayAvailable > 0 ? attendanceTableProfile.AnlDayAvailable : 0.0;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.NightShiftHours] = attendanceTableProfile.NightShiftHours > 0 ? attendanceTableProfile.NightShiftHours : 0.0;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.LateEarlyDeductionHours] = attendanceTableProfile.LateEarlyDeductionHours > 0 ? attendanceTableProfile.LateEarlyDeductionHours : 0.0;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Note] = attendanceTableProfile.Note != null ? attendanceTableProfile.Note : string.Empty;

                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.UnPaidLeave] = attendanceTableProfile.UnPaidLeave > 0 ? attendanceTableProfile.UnPaidLeave : 0.0;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.AnlDayTaken] = attendanceTableProfile.AnlDayTaken > 0 ? attendanceTableProfile.AnlDayTaken : 0.0;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.SickDayTaken] = attendanceTableProfile.SickDayTaken > 0 ? attendanceTableProfile.SickDayTaken : 0.0;

                    double preg = 0.0;

                    #region code cũ xử lý đổ (Data + Day)
                    //foreach (var item in attendanceTableItemProfile)
                    //{
                    //    if (item.IsHavingPregTreatment)
                    //    {
                    //        preg += 1;
                    //    }

                    //    if (item.LeaveTypeID != null)
                    //    {
                    //        row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Data + item.WorkDate.Day.ToString()] = leavedayTypes.Where(s => s.ID == item.LeaveTypeID).FirstOrDefault().Code;
                    //    }
                    //    else
                    //    {
                    //        if (IsPaidHour)
                    //        {
                    //            row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Data + item.WorkDate.Day.ToString()] = item.WorkPaidHours;
                    //        }
                    //        else
                    //        {
                    //            row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Data + item.WorkDate.Day.ToString()] = item.WorkHours;
                    //        }
                    //    }
                    //} 
                    #endregion

                    attendanceTableItemProfile = attendanceTableItemProfile.OrderBy(s => s.WorkDate).ToList();
                    Dictionary<string, double> leave_Hour = new Dictionary<string, double>();
                    for (int i = 0; i < attendanceTableItemProfile.Count; i++)
                    {
                        int stt = i + 1;

                        if (attendanceTableItemProfile[i].IsHavingPregTreatment)
                        {
                            preg += 1;
                        }

                        if (attendanceTableItemProfile[i].LeaveTypeID != null)
                        {
                            var leave = leavedayTypes.Where(s => s.ID == attendanceTableItemProfile[i].LeaveTypeID).FirstOrDefault();
                            if (attendanceTableItemProfile[i].LeaveDays == 0.5)
                            {
                                row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Data + stt] = leave.CodeStatistic + "/2";
                            }
                            else
                            {
                                row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Data + stt] = leave.CodeStatistic;
                            }

                            if (leave.CodeStatistic == null)
                                continue;
                            if (leave_Hour.ContainsKey(leave.CodeStatistic))
                                leave_Hour[leave.CodeStatistic] += attendanceTableItemProfile[i].LeaveHours;
                            else
                                leave_Hour[leave.CodeStatistic] = attendanceTableItemProfile[i].LeaveHours;
                        }
                        else
                        {
                            if (IsPaidHour)
                            {
                                row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Data + stt] = attendanceTableItemProfile[i].WorkPaidHours;
                            }
                            else
                            {
                                row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Data + stt] = attendanceTableItemProfile[i].WorkHours;
                            }
                        }
                    }

                    foreach (var leave in leavedayTypes)
                    {
                        if (leave.CodeStatistic == null)
                            continue;
                        if (leave_Hour.ContainsKey(leave.CodeStatistic))
                        {
                            row[leave.CodeStatistic] = leave_Hour[leave.CodeStatistic];
                        }
                    }

                    #region Tăng Ca

                    if (attendanceTableProfile.Overtime1Type != null)
                    {
                        var overtime = overtimeTypes.Where(s => s.ID == attendanceTableProfile.Overtime1Type).FirstOrDefault();
                        if (overtime != null)
                            row[overtime.CodeStatistic] = attendanceTableProfile.Overtime1Hours;
                    }
                    if (attendanceTableProfile.Overtime2Type != null)
                    {
                        var overtime = overtimeTypes.Where(s => s.ID == attendanceTableProfile.Overtime2Type).FirstOrDefault();
                        if (overtime != null)
                            row[overtime.CodeStatistic] = attendanceTableProfile.Overtime2Hours;
                    }
                    if (attendanceTableProfile.Overtime3Type != null)
                    {
                        var overtime = overtimeTypes.Where(s => s.ID == attendanceTableProfile.Overtime3Type).FirstOrDefault();
                        if (overtime != null)
                            row[overtime.CodeStatistic] = attendanceTableProfile.Overtime3Hours;
                    }
                    if (attendanceTableProfile.Overtime4Type != null)
                    {
                        var overtime = overtimeTypes.Where(s => s.ID == attendanceTableProfile.Overtime4Type).FirstOrDefault();
                        if (overtime != null)
                            row[overtime.CodeStatistic] = attendanceTableProfile.Overtime4Hours;
                    }
                    if (attendanceTableProfile.Overtime5Type != null)
                    {
                        var overtime = overtimeTypes.Where(s => s.ID == attendanceTableProfile.Overtime5Type).FirstOrDefault();
                        if (overtime != null)
                            row[overtime.CodeStatistic] = attendanceTableProfile.Overtime5Hours;
                    }
                    if (attendanceTableProfile.Overtime6Type != null)
                    {
                        var overtime = overtimeTypes.Where(s => s.ID == attendanceTableProfile.Overtime6Type).FirstOrDefault();
                        if (overtime != null)
                            row[overtime.CodeStatistic] = attendanceTableProfile.Overtime6Hours;
                    }

                    #endregion


                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.TotalPregnancy] = preg;
                    foreach (var jobcode in listjobType)
                    {
                        if (table.Columns.Contains(jobcode.Code))
                        {
                            double? nohourByPro = timeSheetByProfile.Where(s => s.JobTypeID == jobcode.ID).Sum(s => s.NoHour);
                            double? sectorByPro = timeSheetByProfile.Where(s => s.JobTypeID == jobcode.ID).Sum(s => s.Sector);
                            row[jobcode.Code] = nohourByPro == null ? 0 : nohourByPro.Value;
                            row[jobcode.Code + "_Sector"] = sectorByPro == null ? 0 : sectorByPro.Value;
                        }
                    }
                    table.Rows.Add(row);
                }
                var configs = new Dictionary<string, Dictionary<string, object>>();
                var config = new Dictionary<string, object>();
                config.Add("format", "{0:dd/mm/yyyy}");
                configs.Add("DateFrom", config);
                return table.ConfigTable();
            }

        }


        DataTable CreateReportMonthlyHourFlightLocalchema(string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tb = new DataTable("ReportMonthlyHourFlightLocal");

                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.CodeEmp);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.ProfileName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DepartmentCode);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.PositionName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.JobTitleName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.StdWorkDayCount, typeof(Double));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.RealWorkDayCount, typeof(Double));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.PaidWorkDayCount, typeof(Double));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.AnlDayAvailable, typeof(Double));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.NightShiftHours, typeof(Double));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.LateEarlyDeductionHours, typeof(Double));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.AnlDayTaken, typeof(Double));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Note);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.ResignedDate, typeof(DateTime));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.StartingDate, typeof(DateTime));

                //tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Note);

                for (int i = 1; i <= 31; i++)
                {
                    DataColumn column = new DataColumn(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Data + i);
                    column.Caption = i.ToString();
                    tb.Columns.Add(column);
                }

                #region MyRegion
                var leavedayTypeServices = new Cat_LeaveDayTypeServices();
                var lstObjLeavedayType = new List<object>();
                lstObjLeavedayType.Add(null);
                lstObjLeavedayType.Add(null);
                lstObjLeavedayType.Add(1);
                lstObjLeavedayType.Add(100000000);
                var lstLeavedayType = leavedayTypeServices.GetData<Cat_LeaveDayTypeEntity>(lstObjLeavedayType, ConstantSql.hrm_cat_sp_get_LeaveDayType, UserLogin, ref status).Where(s => s.Code != null).Select(m => m.Code).ToList();


                foreach (var item in lstLeavedayType)
                {
                    if (!tb.Columns.Contains(item))
                    {
                        tb.Columns.Add(item);
                    }
                }

                #endregion
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.BranchCode);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.TeamCode);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.SectionCode);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DepartmentName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.BranchName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.TeamName);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.SectionName);

                var jobTypeServices = new Cat_JobTypeServices();
                var lstjobType = new List<object>();
                lstjobType.Add(null);
                lstjobType.Add(null);
                lstjobType.Add(1);
                lstjobType.Add(100000000);
                var listjobType = jobTypeServices.GetData<Cat_JobTypeEntity>(lstjobType, ConstantSql.hrm_cat_sp_get_JobType, UserLogin, ref status).Where(s => s.Code != null).Select(m => new { m.RoleID, m.Code }).ToList();


                var roleServices = new Cat_RoleServices();
                var lstRole = new List<object>();
                lstRole.Add(null);
                lstRole.Add(null);
                lstRole.Add(1);
                lstRole.Add(100000000);
                var listRole = roleServices.GetData<Cat_RoleEntity>(lstRole, ConstantSql.hrm_cat_sp_get_Role, UserLogin, ref status).Where(s => s.Code != null).Select(m => new { m.Code, m.ID }).ToList();

                foreach (var roleEntity in listRole)
                {
                    var lstJobTypeByRole = listjobType.Where(s => s.RoleID == roleEntity.ID).ToList();
                    foreach (var item in lstJobTypeByRole)
                    {
                        var CodeRoleAndJobType = roleEntity.Code + "_" + item.Code;
                        if (!tb.Columns.Contains(CodeRoleAndJobType))
                        {
                            tb.Columns.Add(CodeRoleAndJobType);
                        }
                    }
                }
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DateFrom, typeof(DateTime));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DateTo, typeof(DateTime));
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.UserExport);
                tb.Columns.Add(Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DateExport, typeof(DateTime));
                return tb;
            }
        }

        #region BC Dữ Liệu Không Hợp Lệ

        DataTable getSchema_ExceptionDataAdv()
        {
            DataTable tblData = new DataTable("Att_ReportExceptionDataAdvEntity");
            tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.ProfileName, typeof(String));
            tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.LeaveDayTypeName, typeof(String));
            tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.OvertimeTypeName, typeof(String));
            tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.CodeEmp, typeof(String));
            tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.PositionName, typeof(String));
            tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.PositionCode, typeof(String));
            tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.OrgStructureName, typeof(String));
            tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.OrgStructureCode, typeof(String));
            tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.JobTitleCode, typeof(String));
            tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.JobTitleName, typeof(String));
            tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.udDateOut, typeof(DateTime));
            //tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.DateStart, typeof(DateTime));
            //tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.DateEnd, typeof(DateTime));
            tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.WorkDate, typeof(DateTime));
            tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.InTime, typeof(DateTime));
            tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.OutTime, typeof(DateTime));
            tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.InHour, typeof(String));
            tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.OutHour, typeof(String));
            tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.UserExport);
            tblData.Columns.Add(Att_ReportExceptionDataAdvEntity.FieldNames.DateExport, typeof(DateTime));
            return tblData;
        }

        public DataTable GetReportExceptionDataAdvList(Att_ReportExceptionDataAdv_ConditionEntity condition, string userExport, string UserLogin)
        {
            DataTable viewData = new DataTable();
            viewData = getSchema_ExceptionDataAdv();

            using (var context = new VnrHrmDataContext())
            {

                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);
                var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);
                var repoCat_GradeAttendance = new CustomBaseRepository<Cat_GradeAttendance>(unitOfWork);
                var repoCat_Position = new CustomBaseRepository<Cat_Position>(unitOfWork);
                var repoCat_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);

                DateTime DateStart = condition.DateStart.Value;
                DateTime DateEnd = condition.DateEnd.Value;
                //DateTime start = Common.ConvertStringToDateTime(TextBox_DateStart.Text, ApplicationSettings.ShortDateTimeFormat);
                //DateTime end = Common.ConvertStringToDateTime(TextBox_DateEnd.Text, ApplicationSettings.ShortDateTimeFormat);

                var baseService = new BaseService();
                var status = string.Empty;
                List<Hre_ProfileEntity> lstPro = new List<Hre_ProfileEntity>();
                List<object> lstObj = new List<object>();
                lstObj.Add(condition.OrgStructureID);
                lstObj.Add(null);
                lstObj.Add(null);
                lstPro = baseService.GetData<Hre_ProfileEntity>(lstObj, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status);

                List<Att_Workday> listInOut = new List<Att_Workday>();
                List<Hre_Profile> listProfile = LoadProfileStatus(lstPro, condition.StatusEmployee, DateStart, DateEnd);
                //if (MultiCheckTreeControl1.SelectedValues.Count > 0)
                //    listProfile = listProfile.Where(s => s.OrgStructureID != null && MultiCheckTreeControl1.SelectedValues.Contains(s.OrgStructureID.Value)).ToList();


                listProfile = listProfile.OrderBy(p => p.OrgStructureID).ToList();
                List<Guid> lstProfileID = Hre_ProfileServices.GetProfileIdList(listProfile);

                DateTime dateStart_First = DateStart.Date;
                DateTime DateEnd_Last = DateEnd.Date.AddDays(1).AddMinutes(-1);

                #region Hàm getdata bằng Store

                var listPosition = repoCat_Position.FindBy(s => s.Code != null).ToList();
                var listJobTitle = repoCat_JobTitle.FindBy(s => s.Code != null).ToList();
                var listOrgStructure = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();

                List<object> lst2ParamNull = new List<object>();
                lst2ParamNull.Add(condition.OrgStructureID);
                lst2ParamNull.Add(null);

                List<object> lst3ParamNull = new List<object>();
                lst3ParamNull.Add(null);
                lst3ParamNull.Add(null);
                lst3ParamNull.Add(null);

                List<object> lst4Param = new List<object>();
                lst4Param.Add(condition.OrgStructureID);
                lst4Param.Add(DateStart);
                lst4Param.Add(DateEnd);
                lst4Param.Add(RosterStatus.E_APPROVED.ToString());

                List<object> lst3ParamSE = new List<object>();
                lst3ParamSE.Add(condition.OrgStructureID);
                lst3ParamSE.Add(dateStart_First);
                lst3ParamSE.Add(DateEnd_Last);

                var dataAtt_LeaveDay = GetData<Att_LeaveDay>(lst3ParamNull, ConstantSql.hrm_att_getdata_LeaveDay, UserLogin, ref status).ToList();
                var dataAtt_Roster_Inner = GetData<Att_Roster>(lst4Param, ConstantSql.hrm_att_getdata_Roster_Inner, UserLogin, ref status).ToList();
                var dataAtt_TAMScanLog = GetData<Att_TAMScanLog>(lst3ParamSE, ConstantSql.hrm_att_getdata_TamScanLog, UserLogin, ref status).ToList();
                var dataAtt_Workday = GetData<Att_Workday>(lst3ParamSE, ConstantSql.hrm_att_getdata_Workday, UserLogin, ref status).ToList();
                var dataHre_WorkHistory = GetData<Hre_WorkHistory>(lst2ParamNull, ConstantSql.hrm_hre_getdata_WorkHistory, UserLogin, ref status).ToList();

                #endregion

                if (condition.NoScan)
                {
                    //List<Att_LeaveDay> leaveDays = EntityService.GetEntityList<Att_LeaveDay>(s => DateStart.Date <= s.DateStart.Date && s.DateEnd.Date <= DateEnd.Date);
                    //triet fix câu trên
                    //List<Att_LeaveDay> leaveDays = EntityService.GetEntityList<Att_LeaveDay>(s => s.DateStart >= dateStart_First && s.DateEnd <= DateEnd_Last);
                    List<Att_LeaveDay> leaveDays = dataAtt_LeaveDay.Where(s => s.DateStart <= DateEnd_Last && s.DateEnd >= dateStart_First).ToList();

                    if (condition.NoShift)
                    {
                        #region NoShift

                        //trietmai 20121110 0015629
                        //Logic: trường hợp người ta đăng ký Roster là E_TIME_OFF thì loại bỏ ra khỏi list 
                        //vs dk là cùng người cùng khoảng thời gian đăng ký nghỉ v.v
                        string E_APPROVED = RosterStatus.E_APPROVED.ToString();
                        string E_TIME_OFF = RosterType.E_TIME_OFF.ToString();
                        //List<Att_Roster> RosterTimeOff = EntityService.GetEntityList<Att_Roster>(GuidContext, LoginUserID.Value,
                        //    m => lstProfileID.Contains(m.ProfileID)
                        //        && m.DateStart <= DateEnd
                        //        && m.DateEnd >= DateStart
                        //        && m.Status == E_APPROVED
                        //        && m.Type == E_TIME_OFF
                        //    );
                        List<Att_Roster> RosterTimeOff = dataAtt_Roster_Inner
                            .Where(m => lstProfileID.Contains(m.ProfileID) && m.Type == E_TIME_OFF).ToList();


                        //List<Att_TAMScanLog> lstTamAtt = EntityService.GetEntityList<Att_TAMScanLog>(prop => prop.TimeLog <= DateEnd_Last && prop.TimeLog >= DateStart).ToList();
                        List<Att_TAMScanLog> lstTamAtt = dataAtt_TAMScanLog.Where(prop => prop.TimeLog <= DateEnd_Last && prop.TimeLog >= DateStart).ToList();

                        foreach (Hre_Profile profile in listProfile)
                        {
                            for (DateTime idx = DateStart; idx <= DateEnd; idx = idx.AddDays(1))
                            {
                                var haveTimeOff = RosterTimeOff.Where(m =>
                                    m.ProfileID == profile.ID
                                    && m.DateStart <= idx
                                    && m.DateEnd >= idx
                                    ).ToList();
                                if (haveTimeOff.Count != 0)
                                {
                                    continue;
                                }
                                List<Att_TAMScanLog> lstRq = lstTamAtt.Where(prop => prop.CardCode == profile.CodeAttendance && idx.Day == prop.TimeLog.Value.Day).ToList();
                                if (lstRq.Count == 0)
                                {
                                    DataRow dr = viewData.NewRow();
                                    Att_LeaveDay leaveDay = leaveDays.Where(s => s.ProfileID == profile.ID && s.DateStart.Date <= idx.Date && idx.Date <= s.DateEnd.Date).FirstOrDefault();
                                    if (leaveDay != null)
                                    {
                                        dr[Att_ReportExceptionDataAdvEntity.FieldNames.OvertimeTypeName] = leaveDay.Comment;
                                        dr[Att_ReportExceptionDataAdvEntity.FieldNames.LeaveDayTypeName] = leaveDay.Cat_LeaveDayType;
                                    }
                                    dr[Att_ReportExceptionDataAdvEntity.FieldNames.ProfileName] = profile.ProfileName.ToString();
                                    dr[Att_ReportExceptionDataAdvEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                                    if (profile.PositionID != null && profile.PositionID != Guid.Empty)
                                    {
                                        var pos = listPosition.Where(s => s.ID == profile.PositionID).FirstOrDefault();
                                        dr[Att_ReportExceptionDataAdvEntity.FieldNames.PositionCode] = pos.Code;
                                        dr[Att_ReportExceptionDataAdvEntity.FieldNames.PositionName] = pos.PositionName;
                                    }
                                    if (profile.JobTitleID != null && profile.JobTitleID != Guid.Empty)
                                    {
                                        var jos = listJobTitle.Where(s => s.ID == profile.JobTitleID).FirstOrDefault();
                                        dr[Att_ReportExceptionDataAdvEntity.FieldNames.JobTitleCode] = jos.Code;
                                        dr[Att_ReportExceptionDataAdvEntity.FieldNames.JobTitleName] = jos.JobTitleName;
                                    }
                                    if (profile.OrgStructureID != null && profile.OrgStructureID != Guid.Empty)
                                    {
                                        var org = listOrgStructure.Where(s => s.ID == profile.OrgStructureID).FirstOrDefault();
                                        dr[Att_ReportExceptionDataAdvEntity.FieldNames.OrgStructureCode] = org.Code;
                                        dr[Att_ReportExceptionDataAdvEntity.FieldNames.OrgStructureName] = org.OrgStructureName;
                                    }
                                    dr[Att_ReportExceptionDataAdvEntity.FieldNames.udDateOut] = idx;
                                    viewData.Rows.Add(dr);
                                }
                            }

                        }


                        //ListControl1.DataSource = viewData;

                        return viewData;
                        #endregion
                    }
                    else
                    {
                        #region Noscan
                        List<Cat_DayOff> lstHoliday = repoCat_DayOff.FindBy(s => s.IsDelete == null).ToList();
                        //LamLe - 20120829 - tai sao lai loai truong hop "Nghỉ theo phân công"Neu nghi thi khong load du lieu len
                        //String RosterTypeAbsent = RosterType.E_TIME_OFF.ToString();
                        List<Att_Roster> lstRoster = dataAtt_Roster_Inner.Where(prop => prop.DateStart <= DateEnd && prop.DateEnd >= DateStart).ToList();
                        //&& prop.Type != RosterTypeAbsent);

                        List<Att_Workday> lstInOutAll = dataAtt_Workday.Where(wd => lstProfileID.Contains(wd.ProfileID)).OrderBy(wd => wd.ProfileID).ToList();
                        DateTime _dateStart = DateStart;
                        DateTime _dateEnd = DateEnd.AddDays(1).AddMinutes(-1);
                        List<Att_LeaveDay> lstLeaveDayAll = dataAtt_LeaveDay.Where(prop => prop.DateStart <= _dateEnd && prop.DateEnd >= _dateStart).ToList();

                        //GradeDAO gradeDAO = new GradeDAO();
                        List<Att_Grade> lstGradeAll = Att_GradeServices.getAllGrade(lstProfileID, DateEnd);

                        List<Hre_WorkHistory> lstWHistory = dataHre_WorkHistory.Where(s => lstProfileID.Contains(s.ProfileID)).ToList();

                        List<Guid> lstProfileIDs = listProfile.Select(m => m.ID).ToList();
                        List<Att_Roster> lstRosterTypeGroup = new List<Att_Roster>();
                        List<Att_RosterGroup> lstRosterGroup = new List<Att_RosterGroup>();
                        Att_RosterServices.GetRosterGroup(lstProfileIDs, DateStart, DateEnd, out lstRosterTypeGroup, out lstRosterGroup);

                        foreach (Hre_Profile profile in listProfile)
                        {
                            List<Att_LeaveDay> lstProfileLeaveDay = lstLeaveDayAll.Where(ld => ld.ProfileID == profile.ID).ToList();
                            List<Att_Workday> lstProfileInOut = lstInOutAll.Where(ld => ld.ProfileID == profile.ID).ToList();

                            Att_Grade grade = lstGradeAll.Where(grad => grad.ProfileID == profile.ID).FirstOrDefault();


                            List<Att_Roster> lstProfileRoster = lstRoster.Where(rt => rt.ProfileID == profile.ID).ToList();
                            List<Hre_WorkHistory> lstWHistoryPro = lstWHistory.Where(wh => wh.ProfileID == profile.ID).ToList();
                            Cat_GradeAttendance gradeCfg = grade == null ? null : grade.Cat_GradeAttendance;
                            Hashtable htRoster = Att_RosterServices.GetRosterTable(profile, lstProfileRoster, lstWHistoryPro, gradeCfg, DateStart, DateEnd, lstRosterGroup, lstRosterTypeGroup);

                            //string[] strDepartment = GradeCfgDAO.getLinkDepartment(ListCacheOrgStructure, profile.Cat_OrgStructure);
                            //profile.udLinkDepartmentName = strDepartment[0];

                            if (grade == null)
                                continue;

                            if (grade.Cat_GradeAttendance.AttendanceMethod != AttendanceMethod.E_TAM.ToString())
                                continue;

                            for (DateTime idx = DateStart; idx <= DateEnd
                                                         ; idx = idx.AddDays(1))
                            {
                                //In-Out
                                if (lstHoliday.Where(hol => hol.DateOff == idx).Count() > 0)
                                    continue;

                                if (!Att_AttendanceServices.IsWorkDay(grade.Cat_GradeAttendance, htRoster, lstHoliday, idx)
                                    || idx < profile.DateHire
                                    || idx >= profile.DateQuit)
                                    continue;

                                List<Att_Workday> lstDateProfileInOut = lstProfileInOut.Where(inout => inout.WorkDate == idx).ToList();
                                //ko di lam, ko quet the
                                if (lstDateProfileInOut.Count <= 0)
                                {

                                    //ko thong bao nghi doi voi nhung nhan vien dang ki nghi
                                    // neu chon vao checkbox danh sach nghi thi hien ra
                                    if (lstProfileLeaveDay.Where(ld => ld.ProfileID == profile.ID
                                                                && ld.DateStart.Date <= idx && ld.DateEnd.Date >= idx).Count() <= 0 || condition.ShowListLeaveDay == true)
                                    {

                                        //exceptionData = SetExceptionData(null, profile, null, idx);

                                        //exceptionData.DataType = LanguageManager.GetString(ClassNames.Att_Workday);
                                        //exceptionData.Exception = LanguageManager.GetString(Messages.MissingInOut);
                                        //exceptionData.Description = "? - ?";
                                        Att_Workday _inOut = new Att_Workday();
                                        _inOut.Hre_Profile = profile;
                                        _inOut.ID = Guid.NewGuid();
                                        _inOut.WorkDate = idx;
                                        //_inOut.udIsNew = true;
                                        _inOut.InTime1 = idx;
                                        _inOut.OutTime1 = idx;
                                        listInOut.Add(_inOut);
                                    }
                                }

                            }
                        }
                        #endregion
                    }
                }
                else if (condition.DifferenceMoreRoster)
                {
                    #region Difference
                    List<Att_Workday> lstInOutAll = dataAtt_Workday.Where(wd => lstProfileID.Contains(wd.ProfileID)).OrderBy(wd => wd.ProfileID).ToList();

                    foreach (Att_Workday InOut in lstInOutAll)
                    {
                        if (InOut.InTime1 != null && InOut.OutTime1 != null)
                        {
                            if (condition.More == null)
                                condition.More = 0.0;
                            if (condition.More < Att_AttendanceServices.GetShiftRosterShiftInOutHour(InOut, InOut.Cat_Shift))
                            {
                                //Att_ExceptionData exceptionData = SetExceptionData(InOut, InOut.Hre_Profile
                                //                                                    , InOut.Cat_Shift, InOut.WorkDate);
                                //exceptionData.DataType = LanguageManager.GetString(ClassNames.Att_Workday);
                                //exceptionData.Exception = LanguageManager.GetString(Messages.DifferenceShiftRosterAndInOut) + " " + txt_Difference.Text + " " + LanguageManager.GetString(Messages.Hour);

                                //String desc = "InOut: ";
                                //desc += InOut.InTime.Value.ToString(ApplicationSettings.TimeShortFormat);
                                //desc += " - ";
                                //desc += InOut.OutTime.Value.ToString(ApplicationSettings.TimeShortFormat);
                                //desc += " / Shift: ";
                                //desc += InOut.Cat_Shift.ShiftName;

                                //exceptionData.Description = desc;

                                listInOut.Add(InOut);
                            }
                        }
                    }
                    #endregion
                }
                else if (condition.LessHours)
                {
                    if (condition.Less == null)
                        condition.Less = 0.0;
                    #region LessHours
                    Double minHrs = 0;
                    //Get Hours
                    try
                    {
                        minHrs = condition.Less.Value;
                    }
                    catch (System.Exception ex)
                    {
                        //Common.MessageBoxs("Error", InstanceNames.InvalidParameter_Hour
                        //                    , Ext.Net.MessageBox.Icon.ERROR, txt_LessHours.ClientID);
                    }


                    List<Att_Workday> lstInOutAll = dataAtt_Workday.Where(wd => lstProfileID.Contains(wd.ProfileID)
                                                        && wd.InTime1.HasValue
                                                        && wd.OutTime1.HasValue)
                                                    .OrderBy(wd => wd.ProfileID).ToList();

                    foreach (Att_Workday InOut in lstInOutAll)
                    {
                        Double workHours = Att_AttendanceServices.GetInOutWorkingHour(InOut, InOut.Cat_Shift);
                        if (minHrs > workHours)
                        {
                            //Att_ExceptionData exceptionData = SetExceptionData(InOut, InOut.Hre_Profile
                            //                                                    , InOut.Cat_Shift, InOut.WorkDate);
                            //exceptionData.DataType = LanguageManager.GetString(ClassNames.Att_Workday);
                            //exceptionData.Exception = LanguageManager.GetString(Messages.WorkingHoursLess) + " " + txt_LessHours.Text + " " + LanguageManager.GetString(Messages.Hour);

                            //String desc = "";
                            //desc = InOut.InTime.Value.ToString(ApplicationSettings.TimeShortFormat);
                            //desc += " - ";
                            //desc += InOut.OutTime.Value.ToString(ApplicationSettings.TimeShortFormat);

                            //exceptionData.Description = desc;
                            listInOut.Add(InOut);
                        }
                    }
                    #endregion
                }
                else if (condition.MissInOut)
                {
                    List<Att_Workday> lstInOutAll = dataAtt_Workday.Where(wd => lstProfileID.Contains(wd.ProfileID)
                                                        && ((wd.InTime1.HasValue && !wd.OutTime2.HasValue)
                                                        || (!wd.InTime1.HasValue && wd.OutTime2.HasValue)))
                                                        .OrderBy(wd => wd.ProfileID).ToList();
                    // SonNgo
                    List<Att_Grade> lstGradeAll = Att_GradeServices.getAllGrade(lstProfileID, DateEnd);

                    foreach (Att_Workday InOut in lstInOutAll)
                    {
                        if (InOut.InTime1 == null && InOut.OutTime1 == null)
                        {
                            listInOut.Add(InOut);
                        }
                        else if (InOut.InTime1 == null || InOut.OutTime1 == null)
                        {
                            #region don't use
                            //Att_ExceptionData exceptionData = SetExceptionData(InOut, InOut.Hre_Profile, InOut.Cat_Shift, InOut.WorkDate);

                            //exceptionData.DataType = LanguageManager.GetString(ClassNames.Att_Workday);
                            //exceptionData.Exception = LanguageManager.GetString(Messages.MissingInOut);

                            //String desc = "";
                            //if (InOut.InTime != null)
                            //    desc = InOut.InTime.Value.ToString(ApplicationSettings.TimeShortFormat);
                            //else
                            //    desc = "?";
                            //desc += " - ";
                            //if (InOut.OutTime != null)
                            //    desc += InOut.OutTime.Value.ToString(ApplicationSettings.TimeShortFormat);
                            //else
                            //    desc += "?";
                            //exceptionData.Description = desc;
                            #endregion

                            // SonNgo - Neu che do luong khong tru tre som thi khong can liet ke
                            Att_Grade grade = lstGradeAll.Where(grad => grad.ProfileID == InOut.ProfileID).FirstOrDefault();

                            if (grade != null
                                && condition.ExcludeManagerStaff == true
                                && grade.Cat_GradeAttendance != null
                                && grade.Cat_GradeAttendance.IsDeductInLateOutEarly != true)
                            {
                                continue;
                            }
                            listInOut.Add(InOut);
                        }
                    }
                }
                // ListControl1.GridColumnNames = lstColumns.ToArray();

                //trietmai 20121110
                List<Guid> lstProfileIDT = listInOut.Select(m => m.ProfileID).Distinct().ToList();
                string E_APPROVEDT = RosterStatus.E_APPROVED.ToString();
                string E_TIME_OFFT = RosterType.E_TIME_OFF.ToString();
                List<Att_Roster> RosterTimeOffT = dataAtt_Roster_Inner
                    .Where(m => lstProfileIDT.Contains(m.ProfileID)
                        && m.DateStart <= DateEnd
                        && m.DateEnd >= DateStart
                        && m.Type == E_TIME_OFFT
                    ).ToList();

                var listRemoveTemp = listInOut.Where(m =>
                    RosterTimeOffT.Any(u => u.ProfileID == m.ProfileID
                    && u.DateStart <= m.WorkDate
                    && u.DateEnd >= m.WorkDate)
                    ).ToList();

                listInOut.RemoveRange(listRemoveTemp);

                //listInOut = listInOut.OrderBy(oi => oi.udCodeEmp).OrderBy(oi => oi.udDepartment).ToList();
                listInOut = listInOut.OrderBy(w => w.WorkDate).OrderBy(oi => oi.ProfileID).ToList();

                foreach (var item in listInOut)
                {
                    DataRow dr = viewData.NewRow();
                    var profile = lstPro.Where(s => s.ID == item.ProfileID).FirstOrDefault();

                    dr[Att_ReportExceptionDataAdvEntity.FieldNames.ProfileName] = profile.ProfileName.ToString();
                    dr[Att_ReportExceptionDataAdvEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    if (profile.PositionID != null && profile.PositionID != Guid.Empty)
                    {
                        var pos = listPosition.Where(s => s.ID == profile.PositionID).FirstOrDefault();
                        dr[Att_ReportExceptionDataAdvEntity.FieldNames.PositionCode] = pos.Code;
                        dr[Att_ReportExceptionDataAdvEntity.FieldNames.PositionName] = pos.PositionName;
                    }
                    if (profile.JobTitleID != null && profile.JobTitleID != Guid.Empty)
                    {
                        var jos = listJobTitle.Where(s => s.ID == profile.JobTitleID).FirstOrDefault();
                        dr[Att_ReportExceptionDataAdvEntity.FieldNames.JobTitleCode] = jos.Code;
                        dr[Att_ReportExceptionDataAdvEntity.FieldNames.JobTitleName] = jos.JobTitleName;
                    }
                    if (profile.OrgStructureID != null && profile.OrgStructureID != Guid.Empty)
                    {
                        var org = listOrgStructure.Where(s => s.ID == profile.OrgStructureID).FirstOrDefault();
                        dr[Att_ReportExceptionDataAdvEntity.FieldNames.OrgStructureCode] = org.Code;
                        dr[Att_ReportExceptionDataAdvEntity.FieldNames.OrgStructureName] = org.OrgStructureName;
                    }
                    dr[Att_ReportExceptionDataAdvEntity.FieldNames.WorkDate] = item.WorkDate;
                    if (item.InTime1 != null)
                    {
                        dr[Att_ReportExceptionDataAdvEntity.FieldNames.InTime] = item.InTime1;
                        dr[Att_ReportExceptionDataAdvEntity.FieldNames.InHour] = item.InTime1.Value.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                    }
                    if (item.OutTime1 != null)
                    {
                        dr[Att_ReportExceptionDataAdvEntity.FieldNames.OutTime] = item.OutTime1;
                        dr[Att_ReportExceptionDataAdvEntity.FieldNames.OutHour] = item.OutTime1.Value.ToString("HH:mm:ss", CultureInfo.InvariantCulture);
                    }
                    dr[Att_ReportExceptionDataAdvEntity.FieldNames.UserExport] = userExport;
                    dr[Att_ReportExceptionDataAdvEntity.FieldNames.DateExport] = DateTime.Today;
                    viewData.Rows.Add(dr);
                }

                //ListControl1.DataSource = listInOut;
                //    ViewState.Add("Data", tb);
                return viewData;
            }
        }
        #endregion

        public DataTable GetReportMonthlyHourFlightLocal(Guid CutOffDurationID, string strOrgStructure, Guid[] shiftIDs, Guid[] payrollIDs, Guid[] workPlaceIDs, string codeEmp, bool isIncludeQuitEmp, bool isNotAllowZero, bool isCreateTemplate, string userExport, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable table = CreateReportMonthlyHourFlightLocalchema(UserLogin);
                if (isCreateTemplate)
                {
                    return table.ConfigDatatable();
                }
                string status = string.Empty;
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var repoCat_LeaveDayType = new Cat_LeaveDayTypeRepository(unitOfWork);
                var repoWorkDay = new Att_WorkDayRepository(unitOfWork);
                var repoTimeSheet = new Att_TimeSheetRepository(unitOfWork);
                var repoJobtype = new Cat_JobTypeRepository(unitOfWork);
                var repoAtt_AttendencaTable = new CustomBaseRepository<Att_AttendanceTable>(unitOfWork);
                var repoAtt_AttendanceTableItem = new CustomBaseRepository<Att_AttendanceTableItem>(unitOfWork);
                var repoAtt_CutOffDuration = new CustomBaseRepository<Att_CutOffDuration>(unitOfWork);

                var roleServices = new Cat_RoleServices();
                var lstRole = new List<object>();
                lstRole.Add(null);
                lstRole.Add(null);
                lstRole.Add(1);
                lstRole.Add(100000000);
                var listRole = roleServices.GetData<Cat_RoleEntity>(lstRole, ConstantSql.hrm_cat_sp_get_Role, UserLogin, ref status).Where(s => s.Code != null).Select(m => new { m.ID, m.Code }).ToList();

                var jobTypeServices = new Cat_JobTypeServices();
                var lstjobType = new List<object>();
                lstjobType.Add(null);
                lstjobType.Add(null);
                lstjobType.Add(1);
                lstjobType.Add(100000000);
                var listjobType = jobTypeServices.GetData<Cat_JobTypeEntity>(lstjobType, ConstantSql.hrm_cat_sp_get_JobType, UserLogin, ref status).Where(s => s.Code != null).Select(m => m.Code).ToList();


                List<object> lstOrgIDs = new List<object>();
                lstOrgIDs.AddRange(new object[3]);
                lstOrgIDs[0] = (object)strOrgStructure;
                lstOrgIDs[1] = null;
                lstOrgIDs[2] = codeEmp;
                List<Hre_ProfileEntity> profiles = GetData<Hre_ProfileEntity>(lstOrgIDs, ConstantSql.hrm_hr_sp_get_ProfileIdsByOrg, UserLogin, ref status).ToList();
                var cutoffInfo = repoAtt_CutOffDuration.FindBy(s => s.ID == CutOffDurationID).FirstOrDefault();
                var leavedayTypes = repoCat_LeaveDayType.FindBy(s => s.Code != null && s.IsDelete == null).Select(s => new { s.Code, s.ID, s.PaidRate, s.LeaveDayTypeName }).ToList();


                DateTime dateStart = cutoffInfo.DateStart;
                DateTime dateEnd = cutoffInfo.DateEnd;


                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                var positions = repoCat_Position.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
                var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();

                List<Guid> profileIds = profiles.Select(s => s.ID).ToList();

                var timesheets = repoTimeSheet.FindBy(s => s.ProfileID != null && profileIds.Contains(s.ProfileID.Value) && s.Date != null && dateStart <= s.Date && s.Date <= dateEnd
                    && s.IsDelete == null).Select(s => new { s.ProfileID, s.JobTypeID, s.RoleID, s.Date, s.NoHour, s.ID, s.Note }).ToList();

                var jobtypes = repoJobtype.FindBy(s => s.IsDelete == null).Select(s => new { s.ID, s.Code, s.RoleID }).ToList();

                var workDays = repoWorkDay.FindBy(s => profileIds.Contains(s.ProfileID) && dateStart <= s.WorkDate && s.WorkDate <= dateEnd && s.IsDelete == null).Select(s => new { s.ProfileID, s.ShiftID, s.Status }).ToList();

                if (payrollIDs != null)
                {
                    profiles = profiles.Where(s => s.PayrollGroupID != null && payrollIDs.Contains(s.PayrollGroupID.Value)).ToList();
                }
                if (workPlaceIDs != null)
                {
                    profiles = profiles.Where(s => s.WorkPlaceID != null && workPlaceIDs.Contains(s.WorkPlaceID.Value)).ToList();
                }
                if (shiftIDs != null)
                {
                    workDays = workDays.Where(s => s.ShiftID.HasValue && shiftIDs.Contains(s.ShiftID.Value)).ToList();
                    profileIds = workDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }
                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateStart).ToList();
                }
                profileIds = profiles.Select(s => s.ID).ToList();

                var attendanceTables = repoAtt_AttendencaTable
                    .FindBy(s => s.IsDelete == null && s.CutOffDurationID == CutOffDurationID && profileIds.Contains(s.ProfileID))
                    .ToList();

                List<Guid> attendanceTablesID = attendanceTables.Select(s => s.ID).ToList();

                var attendanceTableItem = repoAtt_AttendanceTableItem
                    .FindBy(s => s.IsDelete == null && attendanceTablesID.Contains(s.AttendanceTableID))
                    .ToList();
                if (isNotAllowZero)
                {
                    profileIds = attendanceTables.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }
                profileIds = attendanceTables.Select(s => s.ProfileID).ToList();
                profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();

                foreach (var profile in profiles)
                {
                    var attendanceTableProfile = attendanceTables.FirstOrDefault(s => s.ProfileID == profile.ID);
                    var timeSheetByProfile = timesheets.Where(s => s.ProfileID == profile.ID).ToList();

                    if (timeSheetByProfile == null)
                    {
                        continue;
                    }
                    var lstjobtypebytimesheetpro = timeSheetByProfile.Select(s => s.JobTypeID).ToList();
                    var lstJobTypeCodeByTimeSheet = jobtypes.Where(s => lstjobtypebytimesheetpro.Contains(s.ID)).Select(s => new { s.ID, s.Code, s.RoleID }).ToList();
                    if (attendanceTableProfile == null)
                    {
                        continue;
                    }
                    var attendanceTableItemProfile = attendanceTableItem.Where(s => s.AttendanceTableID == attendanceTableProfile.ID).ToList();
                    DataRow row = table.NewRow();
                    Guid? orgId = profile.OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);

                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);

                    var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                    var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);

                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.ProfileName] = profile.ProfileName;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.CodeEmp] = profile.CodeEmp;

                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;

                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.PositionName] = positon != null ? positon.PositionName : string.Empty;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.JobTitleName] = jobtitle != null ? jobtitle.JobTitleName : string.Empty;
                    if (profile.DateHire != null)
                        row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.StartingDate] = profile.DateHire.Value;
                    if (profile.DateQuit != null)
                        row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.ResignedDate] = profile.DateQuit.Value;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DateFrom] = dateStart;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DateTo] = dateEnd;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.UserExport] = userExport;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.DateExport] = DateTime.Today;

                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.StdWorkDayCount] = attendanceTableProfile.StdWorkDayCount > 0 ? attendanceTableProfile.StdWorkDayCount : 0.0;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.RealWorkDayCount] = attendanceTableProfile.RealWorkDayCount > 0 ? attendanceTableProfile.RealWorkDayCount : 0.0;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.PaidWorkDayCount] = attendanceTableProfile.PaidWorkDayCount > 0 ? attendanceTableProfile.PaidWorkDayCount : 0.0;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.AnlDayAvailable] = attendanceTableProfile.AnlDayAvailable > 0 ? attendanceTableProfile.AnlDayAvailable : 0.0;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.NightShiftHours] = attendanceTableProfile.NightShiftHours > 0 ? attendanceTableProfile.NightShiftHours : 0.0;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.LateEarlyDeductionHours] = attendanceTableProfile.LateEarlyDeductionHours > 0 ? attendanceTableProfile.LateEarlyDeductionHours : 0.0;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.AnlDayTaken] = attendanceTableProfile.AnlDayTaken > 0 ? attendanceTableProfile.AnlDayTaken : 0.0;
                    row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Note] = attendanceTableProfile.Note != null ? attendanceTableProfile.Note : string.Empty;

                    foreach (var item in attendanceTableItemProfile)
                    {
                        if (item.LeaveTypeID != null)
                        {
                            row[Att_ReportMonthlyTimeSheetV2Entity.FieldNames.Data + item.WorkDate.Day.ToString()] = leavedayTypes.Where(s => s.ID == item.LeaveTypeID).FirstOrDefault().Code;
                        }
                    }



                    foreach (var roleEntity in listRole)
                    {
                        var lstJobTypeByRoleID = lstJobTypeCodeByTimeSheet.Where(s => s.RoleID == roleEntity.ID).ToList();
                        foreach (var jobcode in lstJobTypeByRoleID)
                        {
                            var CodeRoleAndJobType = roleEntity.Code + "_" + jobcode.Code;
                            if (table.Columns.Contains(CodeRoleAndJobType))
                            {
                                double? nohourByPro = timeSheetByProfile.Where(s => s.JobTypeID == jobcode.ID && s.RoleID == roleEntity.ID).Sum(s => s.NoHour);
                                row[CodeRoleAndJobType] = nohourByPro;
                            }
                        }
                    }

                    table.Rows.Add(row);
                }
                return table.ConfigTable();
            }

        }



        DataTable CreateReportGeneralMonthlyAttendanceSchema(string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tb = new DataTable("Att_ReportGeneralMonthlyAttendanceEntity");

                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.ProfileName);
                //tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.CodeOrg);
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.DepartmentCode);

                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.PositionName);
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.JobTitleName);

                //tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.Department);
                //tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.DeptCode);
                //    tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.OrderExcel);
                //tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.Cat_Section);
                //tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.SectCode);
                //tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.LaborType);
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.DateHire, typeof(DateTime));
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.DateQuit, typeof(DateTime));

                for (int i = 1; i <= 31; i++)
                {
                    //DataColumn column = new DataColumn(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.Data + i);
                    //column.Caption = i.ToString();
                    //tb.Columns.Add(column);
                    if (!tb.Columns.Contains(i.ToString()))
                        tb.Columns.Add(i.ToString());
                }
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.StandardWorkDays, typeof(double));
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.StandardWorkDaysFormula, typeof(double));
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.RealWorkingDays, typeof(double));
                //   tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.LateEarlyHours);
                var leavedayTypeServices = new Cat_LeaveDayTypeServices();
                var lstObjLeavedayType = new List<object>();
                lstObjLeavedayType.Add(null);
                lstObjLeavedayType.Add(null);
                lstObjLeavedayType.Add(1);
                lstObjLeavedayType.Add(100000000);
                var lstLeavedayType = leavedayTypeServices.GetData<Cat_LeaveDayTypeEntity>(lstObjLeavedayType, ConstantSql.hrm_cat_sp_get_LeaveDayType, UserLogin, ref status).Where(s => s.Code != null).Select(m => new { m.Code, m.CodeStatistic }).ToList();

                var overtimeTypeServices = new Cat_OvertimeTypeServices();
                var lstObjOvertimeType = new List<object>();
                lstObjOvertimeType.Add(null);
                lstObjOvertimeType.Add(null);
                lstObjOvertimeType.Add(null);
                lstObjOvertimeType.Add(1);
                lstObjOvertimeType.Add(10000000);


                foreach (var leaveDayType in lstLeavedayType)
                {
                    string fieldName = leaveDayType.Code;
                    string aliasName = leaveDayType.Code;

                    if (!string.IsNullOrWhiteSpace(leaveDayType.CodeStatistic))
                    {
                        fieldName = leaveDayType.CodeStatistic;
                        aliasName = leaveDayType.CodeStatistic;
                    }

                    fieldName = fieldName.ToString().Replace('.', '_');

                    if (!string.IsNullOrWhiteSpace(fieldName))
                    {
                        if (!tb.Columns.Contains(fieldName))
                        {
                            DataColumn column = new DataColumn(fieldName, typeof(double));
                            column.Caption = aliasName.ToString();
                            tb.Columns.Add(column);
                        }

                        if (!tb.Columns.Contains("h" + fieldName))
                        {
                            DataColumn column = new DataColumn("h" + fieldName, typeof(double));
                            column.Caption = "h" + aliasName.ToString();
                            tb.Columns.Add(column);
                        }
                    }
                }
                if (!tb.Columns.Contains(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.ANL))
                {
                    tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.ANL);
                }
                var lstOvertimeType = overtimeTypeServices.GetData<Cat_OvertimeTypeEntity>(lstObjOvertimeType, ConstantSql.hrm_cat_sp_get_OvertimeType, UserLogin, ref status).Where(s => s.Code != null && s.OvertimeTypeName != null).Select(m => m.Code).ToList();
                // lstLeavedayType.AddRange(lstOvertimeType);

                foreach (var overtimeTypeCode in lstOvertimeType)
                {
                    string typeCode = overtimeTypeCode.ToString().Replace('.', '_');

                    if (!string.IsNullOrWhiteSpace(typeCode))
                    {
                        if (!tb.Columns.Contains(typeCode))
                        {
                            DataColumn column = new DataColumn(typeCode, typeof(double));
                            column.Caption = overtimeTypeCode.ToString();
                            tb.Columns.Add(column);
                        }
                    }
                }

                // tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.Count);
                //tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.Cat_OrgStructure);
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.Comment);

                // ngay lam tra luong
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.PaidWorkDayCount, typeof(double));
                // tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.Hour);
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.LateEarlyDeductionHours, typeof(double));
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.LateEarlyDeductionDays, typeof(double));
                // tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.WorkingPlace);
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.AnlDayAvailable, typeof(double));
                //    tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.WorkHours);
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.WorkPaidHours, typeof(double));
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.LateEarlyMinutes, typeof(double));
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.LateInMinutes, typeof(double));
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.EarlyOutMinutes, typeof(double));
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.NightShiftHours, typeof(double));
                //    tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.Cat_WorkPlace);
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.TotalNightShiftHours, typeof(double));
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.WorkPaidHourOver8, typeof(double));
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.NightHourOver6, typeof(double));

                //  tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.UsableLeave);
                // tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.CurrentMonth);
                //  tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.YearToDate);
                //  tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.SickLeave);
                //   tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.SickYearToDate);
                //   tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.CountLateLess2H);
                // tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.CountLateMore2H);
                //  tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.SumMuteLateEarly);
                //   tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.CountLateEarly);
                //    tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.SumCODayOT);
                //  tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.CountCOLeavday);
                // tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.SickCurrentMonth);
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.BranchCode);
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.TeamCode);
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.SectionCode);
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.DepartmentName);
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.BranchName);
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.TeamName);
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.SectionName);
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.DateFrom, typeof(DateTime));
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.DateTo, typeof(DateTime));
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.UserExport);
                tb.Columns.Add(Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.DateExport, typeof(DateTime));
                return tb;
            }
        }

        public DataTable GetReportGeneralMonthlyAttendance(DateTime? dateStart, DateTime DateTo, string strOrgStructure, List<Hre_ProfileEntity> profiles, Guid[] shiftIDs, Guid[] payrollIDs, string employeeStatus, string groupByType, bool ExcludeIfWorkingDayIsZero, bool isShowOTHourRow, string userExport, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var basesevise = new BaseService();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoProfile = new Hre_ProfileRepository(unitOfWork);
                var repoAtt_Att_LeaveDay = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var repoCat_LeaveDayType = new Cat_LeaveDayTypeRepository(unitOfWork);
                var repoCat_OvertimeType = new Cat_OvertimeTypeRepository(unitOfWork);
                var repoWorkDay = new Att_WorkDayRepository(unitOfWork);
                var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);
                var repoAtt_AttendencaTable = new Att_AttendanceTableRepository(unitOfWork);
                var repoAtt_AttendanceItem = new Att_AttendanceTableItemRepository(unitOfWork);
                var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                var repoAtt_CutOffDuration = new Att_CutOffDurationRepository(unitOfWork);



                //  var monthDuration = dateStart.ToString();
                DataTable table = CreateReportGeneralMonthlyAttendanceSchema(UserLogin);
                dateStart = new DateTime(dateStart.Value.Year, dateStart.Value.Month, 1);
                // Guid? durationID = monthDuration != null ? (Guid?)(object)monthDuration : null;
                DateTime monthEnd = new DateTime(dateStart.Value.Year, dateStart.Value.Month, DateTime.DaysInMonth(dateStart.Value.Year, dateStart.Value.Month));


                // profiles = profiles.Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID, s.PayrollGroupID }).ToList();
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                var positions = repoCat_Position.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
                var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();
                var leavedayTypes = repoCat_LeaveDayType.FindBy(s => s.Code != null && s.IsDelete == null).Select(s => new { s.Code, s.ID, s.PaidRate, s.LeaveDayTypeName }).ToList();
                var overtimTypes = repoCat_OvertimeType.FindBy(s => s.Code != null && s.IsDelete == null).Select(s => new { s.ID, s.Code, s.OvertimeTypeName }).ToList();
                List<Guid> profileIds = profiles.Select(s => s.ID).ToList();
                DateTime dateFrom = dateStart.Value.Date.AddDays(1 - dateStart.Value.Day);
                DateTime dateTo = dateFrom.Date.AddDays(1 - dateFrom.Day).AddMonths(1).AddSeconds(-1);
                string status = string.Empty;
                List<object> para = new List<object>();
                para.AddRange(new object[3]);
                para[0] = strOrgStructure;
                para[1] = dateFrom;
                para[2] = dateTo;
                var workDays = basesevise.GetData<Att_WorkdayEntity>(para, ConstantSql.hrm_att_getdata_Workday, UserLogin, ref status).ToList();
                string groupBy = groupByType != null ? groupByType.ToString() : string.Empty;

                #region DataInfo
                List<object> paratables = new List<object>();
                paratables.AddRange(new object[7]);
                paratables[3] = dateFrom;
                paratables[4] = dateTo;
                paratables[5] = 1;
                paratables[6] = Int32.MaxValue - 1;
                var tableQueryable = basesevise.GetData<Att_AttendanceTableEntity>(paratables, ConstantSql.hrm_att_sp_get_AttendanceTable, UserLogin, ref status).ToList();
                List<object> paratable = new List<object>();
                paratable.AddRange(new object[4]);
                paratable[0] = dateFrom;
                paratable[1] = dateTo;
                paratable[2] = 1;
                paratable[3] = Int32.MaxValue - 1;
                var itemQueryable = basesevise.GetData<Att_AttendanceTableItemEntity>(paratable, ConstantSql.hrm_att_sp_getdata_AttendanceTableItem, UserLogin, ref status).ToList();

                #region durationID
                //if (durationID != null && durationID != Guid.Empty)
                //{
                //    var cutOffDuration = repoAtt_CutOffDuration.FindBy(
                //        d => d.ID == durationID).Select(d => new { d.DateStart, d.DateEnd }).FirstOrDefault();

                //    if (cutOffDuration != null)
                //    {
                //        dateFrom = cutOffDuration.DateStart;
                //        dateTo = cutOffDuration.DateEnd;
                //    }

                //    tableQueryable = tableQueryable.Where(d => d.CutOffDurationID == durationID).ToList();
                //}
                //else
                //{
                // tableQueryable = tableQueryable.Where(d => d.MonthYear == dateFrom).ToList();
                //  }
                #endregion

                tableQueryable = tableQueryable.Where(d => d.MonthYear == dateFrom).ToList();
                #endregion

                if (payrollIDs != null)
                {
                    profiles = profiles.Where(s => s.PayrollGroupID != null && payrollIDs.Contains(s.PayrollGroupID.Value)).ToList();
                }
                if (employeeStatus != null)
                {
                    if (employeeStatus == StatusEmployee.E_WORKING.ToString())
                    {
                        profiles = profiles.Where(pro => (pro.DateQuit == null || pro.DateQuit >= dateTo) && pro.DateHire < dateFrom).ToList();
                    }
                    else if (employeeStatus == StatusEmployee.E_NEWEMPLOYEE.ToString())
                    {
                        profiles = profiles.Where(pro => pro.DateHire <= dateTo && pro.DateHire >= dateFrom).ToList();
                    }
                    else if (employeeStatus == StatusEmployee.E_STOPWORKING.ToString())
                    {
                        profiles = profiles.Where(pro => pro.DateQuit != null && pro.DateQuit.Value <= dateTo && pro.DateQuit.Value >= dateFrom).ToList();
                    }
                    else if (employeeStatus == StatusEmployee.E_WORKINGANDNEW.ToString())
                    {
                        profiles = profiles.Where(pro => pro.DateQuit == null || pro.DateQuit >= dateTo).ToList();
                    }
                }

                profileIds = profiles.Select(d => d.ID).ToList<Guid>();
                tableQueryable = tableQueryable.Where(d => profileIds.Contains(d.ProfileID)).ToList();
                var listAttendanceTable = tableQueryable
                    .Select(d => new
                    {
                        d.ID,
                        d.ProfileID,
                        d.AnlDayAvailable,
                        d.StdWorkDayCount,
                        d.RealWorkDayCount,
                        d.LateEarlyDeductionHours,
                        d.PaidWorkDayCount,
                        d.AnlDayTaken,
                        d.Overtime1Hours,
                        d.Overtime2Hours,
                        d.Overtime3Hours,
                        d.Overtime4Hours,
                        d.Overtime5Hours,
                        d.Overtime6Hours,
                        d.Overtime1Type,
                        d.Overtime2Type,
                        d.Overtime3Type,
                        d.Overtime4Type,
                        d.Overtime5Type,
                        d.Overtime6Type,
                        d.LeaveDay1Hours,
                        d.LeaveDay2Hours,
                        d.LeaveDay3Hours,
                        d.LeaveDay4Hours,
                        d.NightShiftHours,
                        d.LeaveDay1Type,
                        d.LeaveDay2Type,
                        d.LeaveDay3Type,
                        d.LeaveDay4Type,
                    }).ToList();
                profileIds = listAttendanceTable.Select(s => s.ProfileID).ToList();
                List<Guid> listAttendanceTableID = listAttendanceTable.Select(d => d.ID).ToList();
                profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                var listProfileInfo = profiles.Select(d => new
                {
                    d.ID,
                    d.CodeEmp,
                    d.ProfileName,
                    d.DateHire,
                    d.DateQuit,
                    d.LaborType,
                    d.WorkingPlace,
                    d.OrgStructureID,
                    d.WorkPlaceName,
                    d.PositionName,
                    d.JobTitleName,
                    d.EmployeeTypeName,
                    d.OrgStructureName,
                }).ToList();

                itemQueryable = itemQueryable.Where(d => listAttendanceTableID.Contains(d.AttendanceTableID)
                   && d.WorkDate >= dateFrom && d.WorkDate <= dateTo).ToList();

                var listAttendanceTableItem = itemQueryable.Select(d => new
                {
                    d.AttendanceTableID,
                    d.WorkDate,
                    d.OvertimeHours,
                    d.ExtraOvertimeHours,
                    d.ExtraOvertimeHours2,
                    d.ExtraOvertimeHours3,
                    d.WorkPaidHours,
                    d.NightShiftHours,
                    d.ShiftID,
                    d.WorkHourFirstHaftShift,
                    d.WorkHourLastHaftShift,
                    d.LeaveTypeID,
                    d.AvailableHours,

                }).ToList();

                var listLeaveDay = repoAtt_Att_LeaveDay.FindBy(d => profileIds.Contains(d.ProfileID)
                  && dateFrom <= d.DateStart && d.DateEnd <= dateTo).Select(d => new { d.ProfileID, d.Comment }).ToList();
                var listGrade = repoAtt_Grade.FindBy(d => profileIds.Contains(d.ProfileID.Value) && d.IsDelete == null).OrderByDescending(d => d.MonthStart).Select(d =>
                        new { d.ProfileID, d.MonthStart, d.Cat_GradeAttendance.HourOnWorkDate }).ToList();
                var lstShift = repoCat_Shift.FindBy(s => s.IsDelete == null).Select(d => new { d.ID, d.IsDoubleShift, d.Code, d.FirstShiftID, d.LastShiftID }).ToList();
                DataTable table_lstResult = new DataTable();

                #region bang 7 dung store ben sql
                table_lstResult.Columns.Add("ProfileID");
                for (int i = 1; i <= 31; i++)
                {
                    table_lstResult.Columns.Add("Data" + i);
                }
                //lay du lieu bang cat_shift
                //  var lst_CatShift = repoCat_Shift.FindBy(s => s.IsDelete == null).Select(s => new { s.ID, s.IsDoubleShift, s.Code, s.FirstShiftID, s.LastShiftID }).ToList();
                var lst_CatShift = lstShift.ToList();
                //var lstAttendanceTable = repoAtt_AttendencaTable.FindBy(s => s.IsDelete == null && s.MonthYear == dateFrom && profileIds.Contains(s.ProfileID)).Select(s => new { s.ID, s.ProfileID }).ToList();
                var lstAttendanceTable = listAttendanceTable.ToList();
                var lstAttendanceTableID = lstAttendanceTable.Select(s => s.ID).Distinct().ToList();
                //var lstAttendanceTableItem = repoAtt_AttendanceItem.FindBy(s => s.IsDelete == null && lstAttendanceTableID.Contains(s.AttendanceTableID))
                //    .Select(s => new {s.ID,s.AttendanceTableID,s.LeaveTypeID,s.WorkPaidHours,s.AvailableHours,s.ShiftID,s.WorkHourFirstHaftShift,s.WorkHourLastHaftShift,s.WorkDate }).ToList();
                var lstAttendanceTableItem = listAttendanceTableItem.ToList();
                int countId = lstAttendanceTableID.Count;
                //  DataRow rowlstResult = table_lstResult.NewRow();
                var lstCat_LeaDayType = repoCat_LeaveDayType.FindBy(s => s.IsDelete == null).ToList();
                //while(countId >0)
                //{
                for (int i = countId; i > 0; i--)
                {

                    // }
                    var AttendanceID = lstAttendanceTable.Select(s => s.ID).FirstOrDefault();
                    var ProfileID = lstAttendanceTable.Select(s => s.ProfileID).FirstOrDefault();
                    lstAttendanceTable = lstAttendanceTable.Where(s => s.ID != AttendanceID).ToList();
                    DataRow rowlstResult = table_lstResult.NewRow();
                    rowlstResult["ProfileID"] = ProfileID;
                    //int Numday = 1;
                    //while(Numday <=31)
                    //{
                    for (int Numday = 1; Numday <= 31; Numday++)
                    {

                        // }
                        var tab_AttendanceTableItem = lstAttendanceTableItem.Where(s => s.AttendanceTableID == AttendanceID && s.WorkDate.Day == Numday)
                            .Select(s => new { s.LeaveTypeID, s.WorkPaidHours, s.AvailableHours, s.ShiftID, s.WorkHourFirstHaftShift, s.WorkHourLastHaftShift }).ToList();
                        var leaveTypeID = tab_AttendanceTableItem.Select(s => s.LeaveTypeID).FirstOrDefault();
                        double workPaidHour = tab_AttendanceTableItem.Select(s => s.WorkPaidHours).FirstOrDefault();
                        double available = tab_AttendanceTableItem.Select(s => s.AvailableHours).FirstOrDefault();
                        available = 8;
                        var shiftID = tab_AttendanceTableItem.Select(s => s.ShiftID).FirstOrDefault();
                        var isDoubleShift = lst_CatShift.Where(s => s.ID == shiftID).Select(s => s.IsDoubleShift).FirstOrDefault();
                        double? workHourFirstHaftShift = tab_AttendanceTableItem.Select(s => s.WorkHourFirstHaftShift).FirstOrDefault();
                        double? workHourLastHaftShift = tab_AttendanceTableItem.Select(s => s.WorkHourLastHaftShift).FirstOrDefault();
                        string result = string.Empty;
                        if (leaveTypeID != null)
                        {
                            result = lstCat_LeaDayType.Where(s => s.ID == leaveTypeID).Select(s => s.CodeStatistic).ToString();
                        }
                        else
                        {
                            if (isDoubleShift == true)
                            {
                                if (available == workHourFirstHaftShift)
                                {
                                    var firstShiftID = lst_CatShift.Where(s => s.ID == shiftID).Select(s => s.FirstShiftID).FirstOrDefault();
                                    result = lst_CatShift.Where(s => s.ID == firstShiftID).Select(s => s.Code).FirstOrDefault();
                                }
                                else
                                {
                                    result = workHourFirstHaftShift.ToString();
                                }
                                if (available == workHourLastHaftShift)
                                {
                                    var lastShiftID = lst_CatShift.Where(s => s.ID == shiftID).Select(s => s.LastShiftID).FirstOrDefault();
                                    result = result + "-" + lst_CatShift.Where(s => s.ID == lastShiftID).Select(s => s.Code).FirstOrDefault();
                                }
                                else
                                {
                                    result = result + "-" + workHourLastHaftShift.ToString();
                                }
                            }
                            else
                            {
                                if (available == workPaidHour)
                                {
                                    result = lst_CatShift.Where(s => s.ID == shiftID).Select(s => s.Code).FirstOrDefault();
                                }
                                else
                                {
                                    result = workPaidHour.ToString();
                                }
                            }
                        }
                        rowlstResult["Data" + Numday] = result;
                        //Numday++;
                    }
                    table_lstResult.Rows.Add(rowlstResult);
                    //countId--;
                }
                #endregion
                DateTime DateMin = DateTime.MinValue;
                DateTime DateMax = DateTime.MinValue;
                DateTime? DateS = tableQueryable.Select(m => m.DateStart).FirstOrDefault();
                DateTime? DateE = tableQueryable.Select(m => m.DateEnd).FirstOrDefault();
                if (DateS != null)
                    DateMin = DateS.Value;
                if (DateE != null)
                    DateMax = DateE.Value;
                foreach (var profile in profiles)
                {
                    DataRow row = table.NewRow();
                    var attendanceTable = listAttendanceTable.Where(s => s.ProfileID == profile.ID).FirstOrDefault();
                    Guid? orgId = profile.OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);

                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);

                    var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                    var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);

                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.ProfileName] = profile.ProfileName;
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.CodeEmp] = profile.CodeEmp;

                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                    //row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.CodeOrg] = orgOrg != null ? orgOrg.Code : string.Empty;
                    //    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.OrgName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;

                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.PositionName] = positon != null ? positon.PositionName : string.Empty;
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.JobTitleName] = jobtitle != null ? jobtitle.JobTitleName : string.Empty;


                    if (profile.DateHire != null)
                    {
                        row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.DateHire] = profile.DateHire.Value;
                    }

                    if (profile.DateQuit != null)
                    {
                        row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.DateQuit] = profile.DateQuit.Value;
                    }
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.UserExport] = userExport;
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.DateExport] = DateTime.Today;
                    var gradeByProfile = listGrade.Where(d => d.ProfileID == attendanceTable.ProfileID).OrderByDescending(d => d.MonthStart).FirstOrDefault();
                    var listAttendanceTableItemByTable = listAttendanceTableItem.Where(d => d.AttendanceTableID == attendanceTable.ID).ToList();
                    var listLeaveDayByProfile = listLeaveDay.Where(d => d.ProfileID == attendanceTable.ProfileID).ToList();
                    double hourOnWorkDate = gradeByProfile != null ? double.Parse(gradeByProfile.HourOnWorkDate.ToString()) : 1;
                    #region Att_AttendanceTable
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.PaidWorkDayCount] = attendanceTable.PaidWorkDayCount;
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.LateEarlyDeductionHours] = attendanceTable.LateEarlyDeductionHours;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.LateEarlyDeductionDays] = attendanceTable.LateEarlyDeductionHours / hourOnWorkDate;
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.NightShiftHours] = attendanceTable.NightShiftHours;
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.AnlDayAvailable] = attendanceTable.AnlDayAvailable;
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.StandardWorkDays] = attendanceTable.StdWorkDayCount;
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.RealWorkingDays] = attendanceTable.RealWorkDayCount;
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.StandardWorkDaysFormula] = attendanceTable.RealWorkDayCount;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.DateFrom] = DateMin;
                    row[Att_ReportMonthlyTimeSheetEntity.FieldNames.DateTo] = DateMax;
                    if (attendanceTable.AnlDayTaken > 0)
                    {
                        row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.ANL] = attendanceTable.AnlDayTaken;
                    }
                    #endregion
                    #region table_lstResult Att_AttendanceTableItem_Symbol
                    var dtRow = table_lstResult.Rows.OfType<DataRow>().Where(d => d["ProfileID"].GetString() == profile.ID.ToString()).FirstOrDefault();
                    if (dtRow != null)
                    {
                        for (DateTime dateCheck = DateMin; dateCheck < DateMax; dateCheck = dateCheck.AddDays(1))
                        {
                            string Result = string.Empty;
                            switch (dateCheck.Day)
                            {
                                case 1:
                                    Result = dtRow["Data1"].ToString();
                                    break;
                                case 2:
                                    Result = dtRow["Data2"].ToString();
                                    break;
                                case 3:
                                    Result = dtRow["Data3"].ToString();
                                    break;
                                case 4:
                                    Result = dtRow["Data4"].ToString();
                                    break;
                                case 5:
                                    Result = dtRow["Data5"].ToString();
                                    break;
                                case 6:
                                    Result = dtRow["Data6"].ToString();
                                    break;
                                case 7:
                                    Result = dtRow["Data7"].ToString();
                                    break;
                                case 8:
                                    Result = dtRow["Data8"].ToString();
                                    break;
                                case 9:
                                    Result = dtRow["Data9"].ToString();
                                    break;
                                case 10:
                                    Result = dtRow["Data10"].ToString();
                                    break;
                                case 11:
                                    Result = dtRow["Data11"].ToString();
                                    break;
                                case 12:
                                    Result = dtRow["Data12"].ToString();
                                    break;
                                case 13:
                                    Result = dtRow["Data13"].ToString();
                                    break;
                                case 14:
                                    Result = dtRow["Data14"].ToString();
                                    break;
                                case 15:
                                    Result = dtRow["Data15"].ToString();
                                    break;
                                case 16:
                                    Result = dtRow["Data16"].ToString();
                                    break;
                                case 17:
                                    Result = dtRow["Data17"].ToString();
                                    break;
                                case 18:
                                    Result = dtRow["Data18"].ToString();
                                    break;
                                case 19:
                                    Result = dtRow["Data19"].ToString();
                                    break;
                                case 20:
                                    Result = dtRow["Data20"].ToString();
                                    break;
                                case 21:
                                    Result = dtRow["Data21"].ToString();
                                    break;
                                case 22:
                                    Result = dtRow["Data22"].ToString();
                                    break;
                                case 23:
                                    Result = dtRow["Data23"].ToString();
                                    break;
                                case 24:
                                    Result = dtRow["Data24"].ToString();
                                    break;
                                case 25:
                                    Result = dtRow["Data25"].ToString();
                                    break;
                                case 26:
                                    Result = dtRow["Data26"].ToString();
                                    break;
                                case 27:
                                    Result = dtRow["Data27"].ToString();
                                    break;
                                case 28:
                                    Result = dtRow["Data28"].ToString();
                                    break;
                                case 29:
                                    Result = dtRow["Data29"].ToString();
                                    break;
                                case 30:
                                    Result = dtRow["Data30"].ToString();
                                    break;
                                case 31:
                                    Result = dtRow["Data31"].ToString();
                                    break;
                            }
                            //row["Data" + dateCheck.Day] = Result;
                            string day = dateCheck.Day.ToString();
                            row[day] = Result;
                        }
                    }
                    #endregion

                    #region att_AttendanceTableItem
                    int WorkPaidHourOver8 = 0;
                    int NightShiftOver6 = 0;
                    double WorkPaidHours = 0;
                    foreach (var attendanceTableItem in listAttendanceTableItemByTable)
                    {
                        var shift = lstShift.Where(m => m.ID == attendanceTableItem.ShiftID).FirstOrDefault();
                        if (shift != null && shift.IsDoubleShift == true)
                        {
                            if (attendanceTableItem.WorkHourFirstHaftShift >= 8)
                            {
                                WorkPaidHourOver8++;
                            }
                            if (attendanceTableItem.WorkHourLastHaftShift >= 8)
                            {
                                WorkPaidHourOver8++;
                            }
                        }
                        else if (attendanceTableItem.WorkPaidHours >= 8)
                        {
                            WorkPaidHourOver8++;
                        }
                        if (attendanceTableItem.NightShiftHours >= 6)
                        {
                            NightShiftOver6++;
                        }
                        WorkPaidHours += attendanceTableItem.WorkPaidHours;
                    }
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.WorkPaidHourOver8] = WorkPaidHourOver8;
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.NightHourOver6] = NightShiftOver6;
                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.WorkPaidHours] = WorkPaidHours;


                    #endregion
                    #region LeaveDayComment

                    string leaveDayComment = string.Empty;

                    foreach (var leaveDay in listLeaveDayByProfile)
                    {
                        if (!string.IsNullOrWhiteSpace(leaveDayComment))
                        {
                            leaveDayComment += ", ";
                        }

                        leaveDayComment += leaveDay.Comment;
                    }

                    row[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.Comment] = leaveDayComment;

                    #endregion
                    #region LeaveDay1Hours
                    //  string LeaveDayTypeCode1 = string.Empty;
                    var LeaveDayTypeCode1 = lstCat_LeaDayType.Where(s => s.ID == attendanceTable.LeaveDay1Type).Select(s => s.Code).FirstOrDefault();


                    string leaveDayTypeCode = LeaveDayTypeCode1;
                    var LeaveDayTypeCodeStatistic1 = lstCat_LeaDayType.Where(s => s.ID == attendanceTable.LeaveDay1Type).Select(s => s.CodeStatistic).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(LeaveDayTypeCodeStatistic1))
                    {
                        leaveDayTypeCode = LeaveDayTypeCodeStatistic1;
                    }

                    leaveDayTypeCode = leaveDayTypeCode.GetString().Replace('.', '_');
                    if (!string.IsNullOrWhiteSpace(leaveDayTypeCode))
                    {
                        if (table.Columns.Contains(leaveDayTypeCode))
                        {
                            row[leaveDayTypeCode] = attendanceTable.LeaveDay1Hours / hourOnWorkDate;
                        }

                        if (table.Columns.Contains("h" + leaveDayTypeCode))
                        {
                            row["h" + leaveDayTypeCode] = attendanceTable.LeaveDay1Hours;
                        }
                    }

                    #endregion
                    #region LeaveDay2Hours

                    leaveDayTypeCode = lstCat_LeaDayType.Where(s => s.ID == attendanceTable.LeaveDay2Type).Select(s => s.Code).FirstOrDefault();

                    var LeaveDayTypeCodeStatistic2 = lstCat_LeaDayType.Where(s => s.ID == attendanceTable.LeaveDay2Type).Select(s => s.CodeStatistic).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(LeaveDayTypeCodeStatistic2))
                    {
                        leaveDayTypeCode = LeaveDayTypeCodeStatistic2;
                    }

                    leaveDayTypeCode = leaveDayTypeCode.GetString().Replace('.', '_');
                    if (!string.IsNullOrWhiteSpace(leaveDayTypeCode))
                    {
                        if (table.Columns.Contains(leaveDayTypeCode))
                        {
                            row[leaveDayTypeCode] = attendanceTable.LeaveDay2Hours / hourOnWorkDate;
                        }

                        if (table.Columns.Contains("h" + leaveDayTypeCode))
                        {
                            row["h" + leaveDayTypeCode] = attendanceTable.LeaveDay2Hours;
                        }
                    }

                    #endregion
                    #region LeaveDay3Hours

                    leaveDayTypeCode = lstCat_LeaDayType.Where(s => s.ID == attendanceTable.LeaveDay3Type).Select(s => s.Code).FirstOrDefault();
                    var LeaveDayTypeCodeStatistic3 = lstCat_LeaDayType.Where(s => s.ID == attendanceTable.LeaveDay3Type).Select(s => s.CodeStatistic).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(LeaveDayTypeCodeStatistic3))
                    {
                        leaveDayTypeCode = LeaveDayTypeCodeStatistic3;
                    }

                    leaveDayTypeCode = leaveDayTypeCode.GetString().Replace('.', '_');
                    if (!string.IsNullOrWhiteSpace(leaveDayTypeCode))
                    {
                        if (table.Columns.Contains(leaveDayTypeCode))
                        {
                            row[leaveDayTypeCode] = attendanceTable.LeaveDay3Hours / hourOnWorkDate;
                        }

                        if (table.Columns.Contains("h" + leaveDayTypeCode))
                        {
                            row["h" + leaveDayTypeCode] = attendanceTable.LeaveDay3Hours;
                        }
                    }

                    #endregion
                    #region LeaveDay4Hours

                    leaveDayTypeCode = lstCat_LeaDayType.Where(s => s.ID == attendanceTable.LeaveDay4Type).Select(s => s.Code).FirstOrDefault();
                    var LeaveDayTypeCodeStatistic4 = lstCat_LeaDayType.Where(s => s.ID == attendanceTable.LeaveDay4Type).Select(s => s.CodeStatistic).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(LeaveDayTypeCodeStatistic4))
                    {
                        leaveDayTypeCode = LeaveDayTypeCodeStatistic4;
                    }

                    leaveDayTypeCode = leaveDayTypeCode.GetString().Replace('.', '_');
                    if (!string.IsNullOrWhiteSpace(leaveDayTypeCode))
                    {
                        if (table.Columns.Contains(leaveDayTypeCode))
                        {
                            row[leaveDayTypeCode] = attendanceTable.LeaveDay4Hours / hourOnWorkDate;
                        }

                        if (table.Columns.Contains("h" + leaveDayTypeCode))
                        {
                            row["h" + leaveDayTypeCode] = attendanceTable.LeaveDay4Hours;
                        }
                    }

                    #endregion
                    #region Cat_Overtime
                    var OvertimeTypeCode1 = overtimTypes.Where(s => s.ID == attendanceTable.Overtime1Type).Select(s => s.Code).FirstOrDefault();
                    string overtimeTypeCode = OvertimeTypeCode1;
                    overtimeTypeCode = overtimeTypeCode.GetString().Replace('.', '_');

                    if (!string.IsNullOrWhiteSpace(overtimeTypeCode))
                    {
                        if (table.Columns.Contains(overtimeTypeCode))
                        {
                            row[overtimeTypeCode] = attendanceTable.Overtime1Hours;
                        }
                    }
                    var OvertimeTypeCode2 = overtimTypes.Where(s => s.ID == attendanceTable.Overtime2Type).Select(s => s.Code).FirstOrDefault();
                    overtimeTypeCode = OvertimeTypeCode2;
                    overtimeTypeCode = overtimeTypeCode.GetString().Replace('.', '_');

                    if (!string.IsNullOrWhiteSpace(overtimeTypeCode))
                    {
                        if (table.Columns.Contains(overtimeTypeCode))
                        {
                            row[overtimeTypeCode] = attendanceTable.Overtime2Hours;
                        }
                    }
                    var OvertimeTypeCode3 = overtimTypes.Where(s => s.ID == attendanceTable.Overtime3Type).Select(s => s.Code).FirstOrDefault();
                    overtimeTypeCode = OvertimeTypeCode3;
                    overtimeTypeCode = overtimeTypeCode.GetString().Replace('.', '_');

                    if (!string.IsNullOrWhiteSpace(overtimeTypeCode))
                    {
                        if (table.Columns.Contains(overtimeTypeCode))
                        {
                            row[overtimeTypeCode] = attendanceTable.Overtime3Hours;
                        }
                    }
                    var OvertimeTypeCode4 = overtimTypes.Where(s => s.ID == attendanceTable.Overtime4Type).Select(s => s.Code).FirstOrDefault();
                    overtimeTypeCode = OvertimeTypeCode4;
                    overtimeTypeCode = overtimeTypeCode.GetString().Replace('.', '_');

                    if (!string.IsNullOrWhiteSpace(overtimeTypeCode))
                    {
                        if (table.Columns.Contains(overtimeTypeCode))
                        {
                            row[overtimeTypeCode] = attendanceTable.Overtime4Hours;
                        }
                    }
                    var OvertimeTypeCode5 = overtimTypes.Where(s => s.ID == attendanceTable.Overtime5Type).Select(s => s.Code).FirstOrDefault();
                    overtimeTypeCode = OvertimeTypeCode5;
                    overtimeTypeCode = overtimeTypeCode.GetString().Replace('.', '_');

                    if (!string.IsNullOrWhiteSpace(overtimeTypeCode))
                    {
                        if (table.Columns.Contains(overtimeTypeCode))
                        {
                            row[overtimeTypeCode] = attendanceTable.Overtime5Hours;
                        }
                    }
                    var OvertimeTypeCode6 = overtimTypes.Where(s => s.ID == attendanceTable.Overtime6Type).Select(s => s.Code).FirstOrDefault();
                    overtimeTypeCode = OvertimeTypeCode6;
                    overtimeTypeCode = overtimeTypeCode.GetString().Replace('.', '_');

                    if (!string.IsNullOrWhiteSpace(overtimeTypeCode))
                    {
                        if (table.Columns.Contains(overtimeTypeCode))
                        {
                            row[overtimeTypeCode] = attendanceTable.Overtime6Hours;
                        }
                    }

                    #endregion

                    table.Rows.Add(row);

                    #region second Row (OT HOUR)
                    if (isShowOTHourRow)
                    {
                        DataRow dr_OT_Hour = table.NewRow();

                        foreach (var attendanceTableItem in listAttendanceTableItemByTable)
                        {
                            int date = attendanceTableItem.WorkDate.Day;
                            double totalHourOT = attendanceTableItem.OvertimeHours;
                            totalHourOT += attendanceTableItem.ExtraOvertimeHours;
                            totalHourOT += attendanceTableItem.ExtraOvertimeHours2;
                            totalHourOT += attendanceTableItem.ExtraOvertimeHours3;
                            dr_OT_Hour[Att_ReportGeneralMonthlyAttendanceEntity.FieldNames.Data + date] = totalHourOT;
                        }
                        table.Rows.Add(dr_OT_Hour);
                    }

                    #endregion
                }
                #region Code cu
                #endregion
                var configs = new Dictionary<string, Dictionary<string, object>>();
                var config = new Dictionary<string, object>();
                var configdouble = new Dictionary<string, object>();
                var config90 = new Dictionary<string, object>();
                var config95 = new Dictionary<string, object>();
                var config110 = new Dictionary<string, object>();
                var config81 = new Dictionary<string, object>();
                config81.Add("width", 81);
                for (int i = 1; i <= 31; i++)
                {

                    if (!configs.ContainsKey("_" + i.ToString()))
                        configs.Add("_" + i.ToString(), config81);
                }
                config95.Add("width", 95);
                if (!configs.ContainsKey("DateQuit"))
                    configs.Add("DateQuit", config95);
                config90.Add("width", 90);
                if (!configs.ContainsKey("DateHire"))
                    configs.Add("DateHire", config90);
                if (!configs.ContainsKey("CodeEmp"))
                    configs.Add("CodeEmp", config90);
                if (!configs.ContainsKey("DepartmentCode"))
                    configs.Add("DepartmentCode", config90);
                if (!configs.ContainsKey("BranchCode"))
                    configs.Add("BranchCode", config90);
                if (!configs.ContainsKey("TeamCode"))
                    configs.Add("TeamCode", config90);
                if (!configs.ContainsKey("SectionCode"))
                    configs.Add("SectionCode", config90);
                config110.Add("width", 110);
                if (!configs.ContainsKey("BranchName"))
                    configs.Add("BranchName", config110);
                if (!configs.ContainsKey("TeamName"))
                    configs.Add("TeamName", config110);
                if (!configs.ContainsKey("SectionName"))
                    configs.Add("SectionName", config110);
                if (!configs.ContainsKey("PositionName"))
                    configs.Add("PositionName", config110);
                if (!configs.ContainsKey("JobTitleName"))
                    configs.Add("JobTitleName", config110);

                config.Add("hidden", true);
                if (!configs.ContainsKey("DateFrom"))
                    configs.Add("DateFrom", config);
                if (!configs.ContainsKey("DateTo"))
                    configs.Add("DateTo", config);
                if (!configs.ContainsKey("UserExport"))
                    configs.Add("UserExport", config);
                if (!configs.ContainsKey("DateExport"))
                    configs.Add("DateExport", config);

                configdouble.Add("format", "{0:n2}");
                if (!configs.ContainsKey("RealWorkingDays"))
                    configs.Add("RealWorkingDays", configdouble);
                if (!configs.ContainsKey("StandardWorkDaysFormula"))
                    configs.Add("StandardWorkDaysFormula", configdouble);
                return table.ConfigTable(configs);

            }

        }

        #region MonthlyTimeSheet Code Cũ
        //DataTable CreateReportMonthlyTimeSheetSchema()
        //{
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
        //        var repoCat_OvertimeType = new CustomBaseRepository<Cat_OvertimeType>(unitOfWork);


        //        DataTable tblData = new DataTable();

        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.CodeEmp, typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.ProfileName, typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.BranchCode, typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.DepartmentCode, typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.TeamCode, typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.SectionCode, typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.BranchName, typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.TeamName, typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.SectionName, typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.PositionCode, typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.JobtitleCode, typeof(String));
        //        //tblData.Columns.Add(Common.GetObjectName(ClassNames.Cat_EmployeeType), typeof(String));
        //        //tblData.Columns.Add(Common.GetObjectName(ClassNames.Cat_Position), typeof(String));
        //        //tblData.Columns.Add(Common.GetObjectName(ClassNames.Cat_JobTitle), typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.CurrentDept, typeof(string));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.CurrentDept1, typeof(string));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.CurrentDept2, typeof(string));

        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.DepartmentName, typeof(String));
        //        //    tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.DeptCode, typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.OrderExcel, typeof(String));
        //        //tblData.Columns.Add(Common.GetObjectName(ClassNames.Cat_Section), typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.SectCode, typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.LaborType, typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.DateHire, typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.DateQuit, typeof(String));

        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.EmployeeTypeName, typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.PositionName, typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.JobTitleName, typeof(String));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.WorkPlaceName, typeof(String));



        //        for (int i = 1; i <= 31; i++)
        //        {
        //            DataColumn column = new DataColumn("Data" + i);
        //            column.Caption = i.ToString();
        //            tblData.Columns.Add(column);
        //        }

        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.StandardWorkDays, typeof(Double));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.StandardWorkDaysFormula, typeof(Double));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.RealWorkingDays, typeof(Double));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.LateEarlyHours, typeof(Double));

        //        var listLeaveDayType = repoCat_LeaveDayType.FindBy(s => s.LeaveDayTypeName != null)
        //            .Select(d => new { d.Code, d.CodeStatistic })
        //            .ToList();

        //        foreach (var leaveDayType in listLeaveDayType)
        //        {
        //            string fieldName = leaveDayType.Code;
        //            string aliasName = leaveDayType.Code;

        //            if (!string.IsNullOrWhiteSpace(leaveDayType.CodeStatistic))
        //            {
        //                fieldName = leaveDayType.CodeStatistic;
        //                aliasName = leaveDayType.CodeStatistic;
        //            }

        //            fieldName = fieldName.ToString().Replace('.', '_');

        //            if (!string.IsNullOrWhiteSpace(fieldName))
        //            {
        //                if (!tblData.Columns.Contains(fieldName))
        //                {
        //                    DataColumn column = new DataColumn(fieldName, typeof(double));
        //                    column.Caption = aliasName.ToString();
        //                    tblData.Columns.Add(column);
        //                }

        //                if (!tblData.Columns.Contains("h" + fieldName))
        //                {
        //                    DataColumn column = new DataColumn("h" + fieldName, typeof(double));
        //                    column.Caption = "h" + aliasName.ToString();
        //                    tblData.Columns.Add(column);
        //                }
        //            }
        //        }

        //        // phep nam
        //        if (!tblData.Columns.Contains(Att_ReportMonthlyTimeSheetEntity.FieldNames.ANL))
        //        {
        //            tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.ANL);
        //        }

        //        var listOvertimeType = repoCat_OvertimeType
        //            .FindBy(s => s.IsDelete == null)
        //            .Select(d => d.Code)
        //            .ToList<string>();

        //        foreach (var overtimeTypeCode in listOvertimeType)
        //        {
        //            string typeCode = overtimeTypeCode.ToString().Replace('.', '_');

        //            if (!string.IsNullOrWhiteSpace(typeCode))
        //            {
        //                if (!tblData.Columns.Contains(typeCode))
        //                {
        //                    DataColumn column = new DataColumn(typeCode, typeof(double));
        //                    column.Caption = overtimeTypeCode.ToString();
        //                    tblData.Columns.Add(column);
        //                }
        //            }
        //        }

        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.DateFrom, typeof(DateTime));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.DateTo, typeof(DateTime));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.Count, typeof(string));
        //        //tblData.Columns.Add(Common.GetObjectName(ClassNames.Cat_OrgStructure), typeof(string));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.Comment, typeof(String));

        //        // ngay lam tra luong
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.PaidWorkDayCount, typeof(string));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.Hour, typeof(int));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.LateEarlyDeductionHours, typeof(double));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.LateEarlyDeductionDays, typeof(double));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.WorkingPlace, typeof(string));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.AnlDayAvailable, typeof(double));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.WorkHours, typeof(double));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.WorkPaidHours, typeof(double));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.LateEarlyMinutes, typeof(double));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.LateInMinutes, typeof(double));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.EarlyOutMinutes, typeof(double));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.NightShiftHours, typeof(double));
        //        //tblData.Columns.Add(Common.GetObjectName(ClassNames.Cat_WorkPlace), typeof(string));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.TotalNightShiftHours, typeof(double));

        //        //20140212 2 cot moi cua Thang.Nguyen
        //        // task: 0023498 
        //        // 1. Số ngày công làm đủ 8 tiếng
        //        // 2.  Số ngày làm ca đêm đủ 6 tiếng trở lên
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.WorkPaidHourOver8, typeof(double));
        //        tblData.Columns.Add(Att_ReportMonthlyTimeSheetEntity.FieldNames.NightHourOver6, typeof(double));

        //        return tblData;


        //    }
        //}

        //public DataTable GetReportMonthlyTimeSheet(DateTime? dateStart, DateTime DateTo, string strOrgStructure, List<Hre_ProfileEntity> lstProfileAll, Guid[] shiftIDs, Guid[] payrollIDs, bool isIncludeQuitEmp, bool isNotAllowZero)
        //{
        //    List<string> listStatus = new List<string>();
        //    Guid? durationID = Guid.Empty;
        //    string status = string.Empty;
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repoAtt_AttendanceTable = new CustomBaseRepository<Att_AttendanceTable>(unitOfWork);
        //        var repoAtt_AttendanceTableItem = new CustomBaseRepository<Att_AttendanceTableItem>(unitOfWork);
        //        var repoAtt_CutOffDuration = new CustomBaseRepository<Att_CutOffDuration>(unitOfWork);
        //        var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
        //        var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
        //        var repoCat_Position = new Cat_PositionRepository(unitOfWork);
        //        var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
        //        var repoAtt_Workday = new Att_WorkDayRepository(unitOfWork);
        //        var repoCat_TAMReasonmiss = new Cat_TAMScanReasonMissRepository(unitOfWork);
        //        var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);
        //        var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);
        //        DateTime dateFrom = dateStart.Value.Date.AddDays(1 - dateStart.Value.Day);
        //        DateTime dateTo = dateFrom.Date.AddDays(1 - dateFrom.Day).AddMonths(1).AddSeconds(-1);
        //        var profiles = lstProfileAll.Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID, s.PayrollGroupID }).ToList();
        //        var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null && s.IsDelete == null).ToList();
        //        var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
        //        var positions = repoCat_Position.FindBy(s => s.Code != null && s.IsDelete == null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
        //        var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null && s.IsDelete == null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();
        //        var repoCat_Leaveday = new Cat_LeaveDayTypeRepository(unitOfWork);
        //        var leavedayTypes = repoCat_Leaveday.FindBy(s => s.Code != null).Select(s => new { s.ID, s.PaidRate, s.Code }).ToList();
        //        var repoCat_OverTime = new Cat_OvertimeTypeRepository(unitOfWork);
        //        var overtimTypes = repoCat_OverTime.FindBy(s => s.Code != null && s.IsDelete != null).Select(s => new { s.ID, s.Code }).ToList();
        //        List<Guid> profileIds = profiles.Select(s => s.ID).ToList();
        //        DataTable tblData = CreateReportMonthlyTimeSheetSchema();
        //        if (payrollIDs != null)
        //        {
        //            profiles = profiles.Where(s => s.PayrollGroupID != null && payrollIDs.Contains(s.PayrollGroupID.Value)).ToList();
        //        }
        //        if (shiftIDs != null)
        //        {
        //            //var workDays = EntityService.CreateQueryable<Att_Workday>(GuidContext, LoginUserID, true, s => profileIds.Contains(s.ProfileID) && month <= s.WorkDate && s.WorkDate <= monthEnd)
        //            //     .Select(s => new { s.ProfileID, s.ShiftID }).Execute();
        //            var workDays = repoAtt_Workday.FindBy(s => profileIds.Contains(s.ProfileID) && dateFrom <= s.WorkDate && s.WorkDate <= DateTo && s.IsDelete == null)
        //         .Select(s => new { s.ProfileID, s.ShiftID }).ToList();
        //            workDays = workDays.Where(s => s.ShiftID.HasValue && shiftIDs.Contains(s.ShiftID.Value)).ToList();
        //            profileIds = workDays.Select(s => s.ProfileID).ToList();
        //            profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
        //        }
        //        if (!isIncludeQuitEmp)
        //        {
        //            profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateFrom).ToList();
        //        }
        //        profileIds = profiles.Select(s => s.ID).ToList();
        //        var attendanceTables = repoAtt_AttendanceTable.FindBy(s => s.MonthYear == dateFrom && profileIds.Contains(s.ProfileID) && s.IsDelete == null)
        //         .Select(s => new
        //         {
        //             s.TotalAnlDayAvailable,
        //             s.AnlDayTaken,
        //             s.AnlDayAdjacent,
        //             s.TotalSickDayAvailable,
        //             s.SickDayTaken,
        //             s.SickDayAdjacent,
        //             s.LeaveDay1Type,
        //             s.LeaveDay2Type,
        //             s.LeaveDay3Type,
        //             s.LeaveDay4Type,
        //             s.LeaveDay1Days,
        //             s.LeaveDay2Days,
        //             s.LeaveDay3Days,
        //             s.LeaveDay4Days,
        //             s.Overtime1Type,
        //             s.Overtime2Type,
        //             s.Overtime3Type,
        //             s.Overtime4Type,
        //             s.Overtime1Hours,
        //             s.Overtime2Hours,
        //             s.Overtime3Hours,
        //             s.Overtime4Hours,
        //             s.COBeginPeriod,
        //             s.COEndPeriod,
        //             s.ID,
        //             s.ProfileID
        //         }
        //         ).ToList();
        //        var attendanceTableIDs = attendanceTables.Select(s => s.ID).ToList();
        //        var attendanceTableItems = repoAtt_AttendanceTableItem.FindBy(s => attendanceTableIDs.Contains(s.AttendanceTableID) && s.IsDelete == null)
        //     .Select(s => new { s.LateEarlyMinutes, s.AttendanceTableID, s.RootInTime, s.RootOutTime, s.MissInOutReasonID }).ToList();
        //        if (isNotAllowZero)
        //        {
        //            profileIds = attendanceTables.Select(s => s.ProfileID).ToList();
        //            profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
        //        }
        //        //var reasons = repoCat_TAMReasonmiss.FindBy(s => s.IsDelete == null);
        //        var resionMissOutID = repoCat_TAMReasonmiss.FindBy(s => s.IsRemind != null && s.IsRemind.Value && s.IsDelete == null).Select(s => s.ID).FirstOrDefault();
        //        foreach (var profile in profiles)
        //        {
        //            DataRow dr = tblData.NewRow();
        //            Guid? orgId = profile.OrgStructureID;
        //            var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
        //            var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
        //            var orgGroup = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);
        //            var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
        //            var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
        //            var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
        //            dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
        //            dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
        //            dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
        //            dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.SectCode] = orgSection != null ? orgSection.Code : string.Empty;
        //            dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
        //            dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
        //            dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
        //            dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
        //            var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
        //            var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
        //            dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.CodeEmp] = profile.CodeEmp;
        //            dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.ProfileName] = profile.ProfileName;
        //            dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.PositionCode] = positon != null ? positon.Code : string.Empty;
        //            dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.JobtitleCode] = jobtitle != null ? jobtitle.Code : string.Empty;
        //            dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.DateFrom] = dateStart;
        //            dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.DateTo] = dateTo;
        //            #region Profile
        //            //  dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.WorkingPlace] = profile != null ? profile.WorkingPlace : string.Empty;
        //            // dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.ProfileName] = profile != null ? profile.ProfileName : string.Empty;
        //            // dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.CodeEmp] = profile != null ? profile.CodeEmp : string.Empty;
        //            // dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.LaborType] = profile != null ? profile.LaborType : string.Empty;

        //            //if (profile.DateHire != null)
        //            //{
        //            //    dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.DateHire] = profile.DateHire.Value;
        //            //}
        //            if (profile.DateQuit != null)
        //            {
        //                dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.DateQuit] = profile.DateQuit.Value;
        //            }
        //            //dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.EmployeeTypeName] = profile != null ? profile.EmployeeTypeName : string.Empty;
        //            //dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.WorkPlaceName] = profile != null ? profile.WorkPlaceName : string.Empty;
        //            //dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.PositionName] = profile != null ? profile.PositionName : string.Empty;
        //            //dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.JobTitleName] = profile != null ? profile.JobTitleName : string.Empty;
        //            //dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.OrderExcel] = profile != null ? profile.OrderExcel.ToString() : string.Empty;
        //            // dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.CurrentDept] = profile != null ? profile.OrgStructureName : string.Empty;
        //            dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.DepartmentName] = dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.CurrentDept];
        //            #endregion
        //            //if (isNotAllowZero)
        //            //{
        //            //    var attendanceTableProfile = attendanceTables.FirstOrDefault(s => s.ProfileID == profile.ID);
        //            //    if (attendanceTableProfile == null)
        //            //    {
        //            //        continue;
        //            //    }
        //            //    var attenceTableProfileID = attendanceTableProfile.ID;
        //            //    var attendanceTableItemProfiles = attendanceTableItems.Where(s => s.AttendanceTableID == attenceTableProfileID).ToList();
        //            //    var leaday = leavedayTypes.FirstOrDefault(s => s.ID == attendanceTableProfile.LeaveDay1Type);
        //            //    double totoalOT = 0;
        //            //    if (leaday != null)
        //            //    {
        //            //        row[leaday.Code] = (object)attendanceTableProfile.LeaveDay1Days ?? DBNull.Value;
        //            //    }
        //            //}
        //            tblData.Rows.Add(dr);
        //        }
        //        return tblData;
        //        //if (dateStart != null)
        //        //{
        //        //    #region DataInfo

        //        //    List<Hre_ProfileEntity> profileQueryable = lstProfileIDs;
        //        //    IQueryable<Att_AttendanceTable> tableQueryable = repoAtt_AttendanceTable.FindBy(s => s.IsDelete == null);
        //        //    IQueryable<Att_AttendanceTableItem> itemQueryable = repoAtt_AttendanceTableItem.FindBy(s => s.IsDelete == null);
        //        //    if (durationID != null && durationID != Guid.Empty)
        //        //    {
        //        //        var cutOffDuration = repoAtt_CutOffDuration
        //        //            .FindBy(d => d.IsDelete == null && d.ID == durationID)
        //        //            .Select(d => new { d.DateStart, d.DateEnd })
        //        //            .FirstOrDefault();
        //        //        if (cutOffDuration != null)
        //        //        {
        //        //            dateFrom = cutOffDuration.DateStart;
        //        //            dateTo = cutOffDuration.DateEnd;
        //        //        }

        //        //        tableQueryable = tableQueryable.Where(d => d.CutOffDurationID == durationID);
        //        //    }
        //        //    else
        //        //    {
        //        //        tableQueryable = tableQueryable.Where(d => d.MonthYear == dateFrom);
        //        //    }

        //        //    if (listStatus != null)
        //        //    {
        //        //        string statusT = listStatus.FirstOrDefault();

        //        //        if (statusT == StatusEmployee.E_WORKING.ToString())
        //        //        {
        //        //            profileQueryable = profileQueryable.Where(pro => (pro.DateQuit == null || pro.DateQuit >= dateTo) && pro.DateHire < dateFrom).ToList();
        //        //        }
        //        //        else if (statusT == StatusEmployee.E_NEWEMPLOYEE.ToString())
        //        //        {
        //        //            profileQueryable = profileQueryable.Where(pro => pro.DateHire <= dateTo && pro.DateHire >= dateFrom).ToList();
        //        //        }
        //        //        else if (statusT == StatusEmployee.E_STOPWORKING.ToString())
        //        //        {
        //        //            profileQueryable = profileQueryable.Where(pro => pro.DateQuit != null && pro.DateQuit.Value <= dateTo && pro.DateQuit.Value >= dateFrom).ToList();
        //        //        }
        //        //        else if (statusT == StatusEmployee.E_WORKINGANDNEW.ToString())
        //        //        {
        //        //            profileQueryable = profileQueryable.Where(pro => pro.DateQuit == null || pro.DateQuit >= dateTo).ToList();
        //        //        }
        //        //    }

        //        //    List<Guid> listProfileID = profileQueryable.Select(d => d.ID).ToList<Guid>();
        //        //    tableQueryable = tableQueryable.Where(d => listProfileID.Contains(d.ProfileID));


        //        //    var listAttendanceTable = tableQueryable.Select(d => new
        //        //    {
        //        //        d.ID,
        //        //        d.ProfileID,
        //        //        d.AnlDayAvailable,
        //        //        d.StdWorkDayCount,
        //        //        d.RealWorkDayCount,
        //        //        d.LateEarlyDeductionHours,
        //        //        d.PaidWorkDayCount,
        //        //        d.AnlDayTaken,
        //        //        d.Overtime1Hours,
        //        //        d.Overtime2Hours,
        //        //        d.Overtime3Hours,
        //        //        d.Overtime4Hours,
        //        //        d.Overtime5Hours,
        //        //        d.Overtime6Hours,
        //        //        OvertimeTypeCode1 = d.Cat_OvertimeType.Code,
        //        //        OvertimeTypeCode2 = d.Cat_OvertimeType1.Code,
        //        //        OvertimeTypeCode3 = d.Cat_OvertimeType2.Code,
        //        //        OvertimeTypeCode4 = d.Cat_OvertimeType3.Code,
        //        //        OvertimeTypeCode5 = d.Cat_OvertimeType4.Code,
        //        //        OvertimeTypeCode6 = d.Cat_OvertimeType5.Code,
        //        //        d.LeaveDay1Hours,
        //        //        d.LeaveDay2Hours,
        //        //        d.LeaveDay3Hours,
        //        //        d.LeaveDay4Hours,
        //        //        LeaveDayTypeCode1 = d.Cat_LeaveDayType.Code,
        //        //        LeaveDayTypeCode2 = d.Cat_LeaveDayType1.Code,
        //        //        LeaveDayTypeCode3 = d.Cat_LeaveDayType2.Code,
        //        //        LeaveDayTypeCode4 = d.Cat_LeaveDayType3.Code,
        //        //        LeaveDayTypeCodeStatistic1 = d.Cat_LeaveDayType.CodeStatistic,
        //        //        LeaveDayTypeCodeStatistic2 = d.Cat_LeaveDayType1.CodeStatistic,
        //        //        LeaveDayTypeCodeStatistic3 = d.Cat_LeaveDayType2.CodeStatistic,
        //        //        LeaveDayTypeCodeStatistic4 = d.Cat_LeaveDayType3.CodeStatistic,
        //        //        d.NightShiftHours,

        //        //    }).ToList();

        //        //    listProfileID = listAttendanceTable.Select(d => d.ProfileID).ToList();
        //        //    List<Guid> listAttendanceTableID = listAttendanceTable.Select(d => d.ID).ToList();
        //        //    profileQueryable = profileQueryable.Where(d => listProfileID.Contains(d.ID)).ToList();

        //        //    var lstOrgStructure = repoCat_OrgStructure.FindBy(s => s.OrgGroupName != null).Select(m => new { m.ID, m.ParentID, m.OrgStructureName }).ToList();

        //        //    var listProfileInfo = profileQueryable.Select(d => new
        //        //    {
        //        //        d.ID,
        //        //        d.CodeEmp,
        //        //        d.ProfileName,
        //        //        d.DateHire,
        //        //        d.DateQuit,
        //        //        d.LaborType,
        //        //        d.WorkingPlace,
        //        //        d.OrgStructureID,
        //        //        d.WorkPlaceName,
        //        //        d.PositionName,
        //        //        d.JobTitleName,
        //        //        d.EmployeeTypeName,
        //        //        d.OrgStructureName
        //        //        #region MyRegion
        //        //        //,
        //        //        //DeptCode = d.Cat_OrgStructure.Code,
        //        //        //d.Cat_OrgStructure.Cat_NameEntity.EnumType,
        //        //        //EnumType2 = d.Cat_OrgStructure.Cat_OrgStructure2.Cat_NameEntity.EnumType,
        //        //        //OrgStructureName2 = d.Cat_OrgStructure.Cat_OrgStructure2.OrgStructureName,
        //        //        //DeptCode2 = d.Cat_OrgStructure.Cat_OrgStructure2.Code,
        //        //        //d.Cat_OrgStructure.OrderExcel
        //        //        #endregion
        //        //    }).ToList();

        //        //    itemQueryable = itemQueryable.Where(d => listAttendanceTableID.Contains(d.AttendanceTableID)
        //        //        && d.WorkDate >= dateFrom && d.WorkDate <= dateTo);
        //        //    var listAttendanceTableItem = itemQueryable.Select(d => new
        //        //    {
        //        //        d.AttendanceTableID,
        //        //        d.WorkDate,
        //        //        d.OvertimeHours,
        //        //        d.ExtraOvertimeHours,
        //        //        d.ExtraOvertimeHours2,
        //        //        d.ExtraOvertimeHours3,
        //        //        d.WorkPaidHours,
        //        //        d.NightShiftHours,
        //        //        d.ShiftID,
        //        //        d.WorkHourFirstHaftShift,
        //        //        d.WorkHourLastHaftShift

        //        //    }).ToList();
        //        //    List<object> lst3Param = new List<object>();
        //        //    lst3Param.Add(strOrgStructure);
        //        //    lst3Param.Add(null);
        //        //    lst3Param.Add(null);
        //        //    var listLeaveDay = GetData<Att_LeaveDayEntity>(lst3Param, ConstantSql.hrm_att_getdata_LeaveDay, ref status)
        //        //        .Where(d => dateFrom <= d.DateStart && d.DateEnd <= dateTo)
        //        //        .Select(d => new { d.ProfileID, d.Comment }).ToList();
        //        //    var listGrade = repoAtt_Grade
        //        //        .FindBy(d => listProfileID.Contains(d.ProfileID.Value))
        //        //        .OrderByDescending(d => d.MonthStart).Select(d => new { d.ProfileID, d.MonthStart, d.Cat_GradeCfg.HourOnWorkDate })
        //        //        .ToList();
        //        //    var lstShift = repoCat_Shift
        //        //        .FindBy(s => s.ShiftName != null)
        //        //        .Select(d => new { d.ID, d.IsDoubleShift })
        //        //        .ToList();
        //        //    #endregion
        //        //    Dictionary<string, object> parameters = new Dictionary<string, object>();
        //        //    parameters.Add("MonthYear", dateFrom);
        //        //    #region MyRegion
        //        //         //DataTable SymbolAttentdance = AdoService.ExecuteCommand("vnr_GetSymbolAttendance",
        //        //    //   CommandType.StoredProcedure, parameters);
        //        //    #endregion
        //        //    DateTime DateMin = DateTime.MinValue;
        //        //    DateTime DateMax = DateTime.MinValue;
        //        //    DateTime? DateS = tableQueryable.Select(m => m.DateStart).FirstOrDefault();
        //        //    DateTime? DateE = tableQueryable.Select(m => m.DateEnd).FirstOrDefault();
        //        //    if (DateS != null)
        //        //        DateMin = DateS.Value;
        //        //    if (DateE != null)
        //        //        DateMax = DateE.Value;
        //        //    #region MyRegion
        //        //    // for test
        //        //    /*     string codeEmp = "BTMV-0249";
        //        //         Hre_Profile profile1 = EntityService.GetEntity<Hre_Profile>(GuidContext, LoginUserID, s => s.CodeEmp == codeEmp);
        //        //         listAttendanceTable = listAttendanceTable.Where(s => s.ProfileID == profile1.ID).ToList();*/
        //        //    #endregion
        //        //    foreach (var attendanceTable in listAttendanceTable)
        //        //    {
        //        //        DataRow dr = tblData.NewRow();

        //        //        #region Hre_Profile
        //        //        var profile = listProfileInfo.Where(d => d.ID == attendanceTable.ProfileID).FirstOrDefault();
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.WorkingPlace] = profile != null ? profile.WorkingPlace : string.Empty;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.ProfileName] = profile != null ? profile.ProfileName : string.Empty;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.CodeEmp] = profile != null ? profile.CodeEmp : string.Empty;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.LaborType] = profile != null ? profile.LaborType : string.Empty;

        //        //        if (profile.DateHire != null)
        //        //        {
        //        //            dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.DateHire] = profile.DateHire.Value;
        //        //        }
        //        //        if (profile.DateQuit != null)
        //        //        {
        //        //            dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.DateQuit] = profile.DateQuit.Value;
        //        //        }
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.EmployeeTypeName] = profile != null ? profile.EmployeeTypeName : string.Empty;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.WorkPlaceName] = profile != null ? profile.WorkPlaceName : string.Empty;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.PositionName] = profile != null ? profile.PositionName : string.Empty;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.JobTitleName] = profile != null ? profile.JobTitleName : string.Empty;
        //        //        //dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.OrderExcel] = profile != null ? profile.OrderExcel.ToString() : string.Empty;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.CurrentDept] = profile != null ? profile.OrgStructureName : string.Empty;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.Department] = dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.CurrentDept];
        //        //        if (profile.OrgStructureID != null)
        //        //        {
        //        //            var orgF0 = lstOrgStructure.Where(m => m.ID == profile.OrgStructureID).FirstOrDefault();
        //        //            if (orgF0 != null)
        //        //            {
        //        //                var orgF1 = lstOrgStructure.Where(m => m.ID == orgF0.ParentID).FirstOrDefault();
        //        //                if (orgF1 != null)
        //        //                {
        //        //                    dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.CurrentDept1] = orgF1.OrgStructureName;
        //        //                    var orgF2 = lstOrgStructure.Where(m => m.ID == orgF1.ParentID).FirstOrDefault();
        //        //                    if (orgF2 != null)
        //        //                    {
        //        //                        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.CurrentDept2] = orgF2.OrgStructureName;
        //        //                    }
        //        //                }
        //        //            }
        //        //        }
        //        //        if (profile != null)
        //        //        {
        //        //            #region MyRegion
        //        //            //if (profile.EnumType == OrgUnit.E_DEPARTMENT.ToString())
        //        //            //{
        //        //            //    dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.Department] = profile.OrgStructureName;
        //        //            //dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.DeptCode] = profile.DeptCode;
        //        //            //}
        //        //            //else if (profile.EnumType2 == OrgUnit.E_DEPARTMENT.ToString())
        //        //            //{
        //        //            //    dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.Department] = profile.OrgStructureName2;
        //        //            //    dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.DeptCode] = profile.DeptCode2;
        //        //            //}

        //        //            //if (profile.EnumType == OrgUnit.E_SECTION.ToString())
        //        //            //{
        //        //            //    dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.SectionName] = profile.OrgStructureName;
        //        //            //    dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.SectCode] = profile.DeptCode;
        //        //            //}
        //        //            //else if (profile.EnumType2 == OrgUnit.E_SECTION.ToString())
        //        //            //{
        //        //            //    dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.SectionName] = profile.OrgStructureName2;
        //        //            //    dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.SectCode] = profile.DeptCode2;
        //        //            //}
        //        //            #endregion
        //        //        }

        //        //        var gradeByProfile = listGrade.Where(d => d.ProfileID == attendanceTable.ProfileID).OrderByDescending(d => d.MonthStart).FirstOrDefault();
        //        //        var listAttendanceTableItemByTable = listAttendanceTableItem.Where(d => d.AttendanceTableID == attendanceTable.ID).ToList();
        //        //        var listLeaveDayByProfile = listLeaveDay.Where(d => d.ProfileID == attendanceTable.ProfileID).ToList();
        //        //        double hourOnWorkDate = gradeByProfile != null ? gradeByProfile.HourOnWorkDate.Get_Double() : 1;

        //        //        #endregion

        //        //        #region Att_AttendanceTable

        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.PaidWorkDayCount] = attendanceTable.PaidWorkDayCount;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.LateEarlyDeductionHours] = attendanceTable.LateEarlyDeductionHours;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.LateEarlyDeductionDays] = attendanceTable.LateEarlyDeductionHours / hourOnWorkDate;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.NightShiftHours] = attendanceTable.NightShiftHours;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.AnlDayAvailable] = attendanceTable.AnlDayAvailable;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.StandardWorkDays] = attendanceTable.StdWorkDayCount;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.RealWorkingDays] = attendanceTable.RealWorkDayCount;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.StandardWorkDaysFormula] = attendanceTable.RealWorkDayCount;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.DateFrom] = DateMin;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.DateTo] = DateMax;

        //        //        if (attendanceTable.AnlDayTaken > 0)
        //        //        {
        //        //            dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.ANL] = attendanceTable.AnlDayTaken;
        //        //        }

        //        //        #endregion

        //        //        #region Att_AttendanceTableItem_Symbol

        //        //        //var dtRow = SymbolAttentdance.Rows.OfType<DataRow>().Where(d => d["ProfileID"].GetString() == profile.ID.ToString()).FirstOrDefault();

        //        //        for (DateTime dateCheck = DateMin; dateCheck < DateMax; dateCheck = dateCheck.AddDays(1))
        //        //        {
        //        //            string Result = string.Empty;
        //        //            switch (dateCheck.Day)
        //        //            {
        //        //                #region MyRegion
        //        //                //case 1:
        //        //                //    Result = dtRow["Data1"].ToString();
        //        //                //    break;
        //        //                //case 2:
        //        //                //    Result = dtRow["Data2"].ToString();
        //        //                //    break;
        //        //                //case 3:
        //        //                //    Result = dtRow["Data3"].ToString();
        //        //                //    break;
        //        //                //case 4:
        //        //                //    Result = dtRow["Data4"].ToString();
        //        //                //    break;
        //        //                //case 5:
        //        //                //    Result = dtRow["Data5"].ToString();
        //        //                //    break;
        //        //                //case 6:
        //        //                //    Result = dtRow["Data6"].ToString();
        //        //                //    break;
        //        //                //case 7:
        //        //                //    Result = dtRow["Data7"].ToString();
        //        //                //    break;
        //        //                //case 8:
        //        //                //    Result = dtRow["Data8"].ToString();
        //        //                //    break;
        //        //                //case 9:
        //        //                //    Result = dtRow["Data9"].ToString();
        //        //                //    break;
        //        //                //case 10:
        //        //                //    Result = dtRow["Data10"].ToString();
        //        //                //    break;
        //        //                //case 11:
        //        //                //    Result = dtRow["Data11"].ToString();
        //        //                //    break;
        //        //                //case 12:
        //        //                //    Result = dtRow["Data12"].ToString();
        //        //                //    break;
        //        //                //case 13:
        //        //                //    Result = dtRow["Data13"].ToString();
        //        //                //    break;
        //        //                //case 14:
        //        //                //    Result = dtRow["Data14"].ToString();
        //        //                //    break;
        //        //                //case 15:
        //        //                //    Result = dtRow["Data15"].ToString();
        //        //                //    break;
        //        //                //case 16:
        //        //                //    Result = dtRow["Data16"].ToString();
        //        //                //    break;
        //        //                //case 17:
        //        //                //    Result = dtRow["Data17"].ToString();
        //        //                //    break;
        //        //                //case 18:
        //        //                //    Result = dtRow["Data18"].ToString();
        //        //                //    break;
        //        //                //case 19:
        //        //                //    Result = dtRow["Data19"].ToString();
        //        //                //    break;
        //        //                //case 20:
        //        //                //    Result = dtRow["Data20"].ToString();
        //        //                //    break;
        //        //                //case 21:
        //        //                //    Result = dtRow["Data21"].ToString();
        //        //                //    break;
        //        //                //case 22:
        //        //                //    Result = dtRow["Data22"].ToString();
        //        //                //    break;
        //        //                //case 23:
        //        //                //    Result = dtRow["Data23"].ToString();
        //        //                //    break;
        //        //                //case 24:
        //        //                //    Result = dtRow["Data24"].ToString();
        //        //                //    break;
        //        //                //case 25:
        //        //                //    Result = dtRow["Data25"].ToString();
        //        //                //    break;
        //        //                //case 26:
        //        //                //    Result = dtRow["Data26"].ToString();
        //        //                //    break;
        //        //                //case 27:
        //        //                //    Result = dtRow["Data27"].ToString();
        //        //                //    break;
        //        //                //case 28:
        //        //                //    Result = dtRow["Data28"].ToString();
        //        //                //    break;
        //        //                //case 29:
        //        //                //    Result = dtRow["Data29"].ToString();
        //        //                //    break;
        //        //                //case 30:
        //        //                //    Result = dtRow["Data30"].ToString();
        //        //                //    break;
        //        //                //case 31:
        //        //                //    Result = dtRow["Data31"].ToString();
        //        //                //    break;
        //        //                #endregion

        //        //            }
        //        //            dr["Data" + dateCheck.Day] = Result;

        //        //        }
        //        //        #endregion

        //        //        #region att_AttendanceTableItem
        //        //        int WorkPaidHourOver8 = 0;
        //        //        int NightShiftOver6 = 0;
        //        //        double WorkPaidHours = 0;
        //        //        foreach (var attendanceTableItem in listAttendanceTableItemByTable)
        //        //        {
        //        //            var shift = lstShift.Where(m => m.ID == attendanceTableItem.ShiftID).FirstOrDefault();
        //        //            if (shift != null && shift.IsDoubleShift == true)
        //        //            {
        //        //                if (attendanceTableItem.WorkHourFirstHaftShift >= 8)
        //        //                {
        //        //                    WorkPaidHourOver8++;
        //        //                }
        //        //                if (attendanceTableItem.WorkHourLastHaftShift >= 8)
        //        //                {
        //        //                    WorkPaidHourOver8++;
        //        //                }
        //        //            }
        //        //            else if (attendanceTableItem.WorkPaidHours >= 8)
        //        //            {
        //        //                WorkPaidHourOver8++;
        //        //            }
        //        //            if (attendanceTableItem.NightShiftHours >= 6)
        //        //            {
        //        //                NightShiftOver6++;
        //        //            }
        //        //            WorkPaidHours += attendanceTableItem.WorkPaidHours;
        //        //        }
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.WorkPaidHourOver8] = WorkPaidHourOver8;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.NightHourOver6] = NightShiftOver6;
        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.WorkPaidHours] = WorkPaidHours;


        //        //        #endregion

        //        //        #region LeaveDayComment

        //        //        string leaveDayComment = string.Empty;

        //        //        foreach (var leaveDay in listLeaveDayByProfile)
        //        //        {
        //        //            if (!string.IsNullOrWhiteSpace(leaveDayComment))
        //        //            {
        //        //                leaveDayComment += ", ";
        //        //            }

        //        //            leaveDayComment += leaveDay.Comment;
        //        //        }

        //        //        dr[Att_ReportMonthlyTimeSheetEntity.FieldNames.Comment] = leaveDayComment;

        //        //        #endregion

        //        //        #region LeaveDay1Hours

        //        //        string leaveDayTypeCode = attendanceTable.LeaveDayTypeCode1;

        //        //        if (!string.IsNullOrWhiteSpace(attendanceTable.LeaveDayTypeCodeStatistic1))
        //        //        {
        //        //            leaveDayTypeCode = attendanceTable.LeaveDayTypeCodeStatistic1;
        //        //        }

        //        //        if (!string.IsNullOrWhiteSpace(leaveDayTypeCode))
        //        //        {
        //        //            leaveDayTypeCode = leaveDayTypeCode.ToString().Replace('.', '_');
        //        //            if (tblData.Columns.Contains(leaveDayTypeCode))
        //        //            {
        //        //                dr[leaveDayTypeCode] = attendanceTable.LeaveDay1Hours / hourOnWorkDate;
        //        //            }

        //        //            if (tblData.Columns.Contains("h" + leaveDayTypeCode))
        //        //            {
        //        //                dr["h" + leaveDayTypeCode] = attendanceTable.LeaveDay1Hours;
        //        //            }
        //        //        }

        //        //        #endregion

        //        //        #region LeaveDay2Hours

        //        //        leaveDayTypeCode = attendanceTable.LeaveDayTypeCode2;

        //        //        if (!string.IsNullOrWhiteSpace(attendanceTable.LeaveDayTypeCodeStatistic2))
        //        //        {
        //        //            leaveDayTypeCode = attendanceTable.LeaveDayTypeCodeStatistic2;
        //        //        }

        //        //        if (!string.IsNullOrWhiteSpace(leaveDayTypeCode))
        //        //        {
        //        //            leaveDayTypeCode = leaveDayTypeCode.ToString().Replace('.', '_');
        //        //            if (tblData.Columns.Contains(leaveDayTypeCode))
        //        //            {
        //        //                dr[leaveDayTypeCode] = attendanceTable.LeaveDay2Hours / hourOnWorkDate;
        //        //            }

        //        //            if (tblData.Columns.Contains("h" + leaveDayTypeCode))
        //        //            {
        //        //                dr["h" + leaveDayTypeCode] = attendanceTable.LeaveDay2Hours;
        //        //            }
        //        //        }

        //        //        #endregion

        //        //        #region LeaveDay3Hours

        //        //        leaveDayTypeCode = attendanceTable.LeaveDayTypeCode3;

        //        //        if (!string.IsNullOrWhiteSpace(attendanceTable.LeaveDayTypeCodeStatistic3))
        //        //        {
        //        //            leaveDayTypeCode = attendanceTable.LeaveDayTypeCodeStatistic3;
        //        //        }

        //        //        if (!string.IsNullOrWhiteSpace(leaveDayTypeCode))
        //        //        {
        //        //            leaveDayTypeCode = leaveDayTypeCode.ToString().Replace('.', '_');
        //        //            if (tblData.Columns.Contains(leaveDayTypeCode))
        //        //            {
        //        //                dr[leaveDayTypeCode] = attendanceTable.LeaveDay3Hours / hourOnWorkDate;
        //        //            }

        //        //            if (tblData.Columns.Contains("h" + leaveDayTypeCode))
        //        //            {
        //        //                dr["h" + leaveDayTypeCode] = attendanceTable.LeaveDay3Hours;
        //        //            }
        //        //        }

        //        //        #endregion

        //        //        #region LeaveDay4Hours

        //        //        leaveDayTypeCode = attendanceTable.LeaveDayTypeCode4;

        //        //        if (!string.IsNullOrWhiteSpace(attendanceTable.LeaveDayTypeCodeStatistic4))
        //        //        {
        //        //            leaveDayTypeCode = attendanceTable.LeaveDayTypeCodeStatistic4;
        //        //        }

        //        //        if (!string.IsNullOrWhiteSpace(leaveDayTypeCode))
        //        //        {
        //        //            leaveDayTypeCode = leaveDayTypeCode.ToString().Replace('.', '_');
        //        //            if (tblData.Columns.Contains(leaveDayTypeCode))
        //        //            {
        //        //                dr[leaveDayTypeCode] = attendanceTable.LeaveDay4Hours / hourOnWorkDate;
        //        //            }

        //        //            if (tblData.Columns.Contains("h" + leaveDayTypeCode))
        //        //            {
        //        //                dr["h" + leaveDayTypeCode] = attendanceTable.LeaveDay4Hours;
        //        //            }
        //        //        }

        //        //        #endregion

        //        //        #region Cat_Overtime

        //        //        string overtimeTypeCode = attendanceTable.OvertimeTypeCode1;

        //        //        if (!string.IsNullOrWhiteSpace(overtimeTypeCode))
        //        //        {
        //        //            overtimeTypeCode = overtimeTypeCode.ToString().Replace('.', '_');
        //        //            if (tblData.Columns.Contains(overtimeTypeCode))
        //        //            {
        //        //                dr[overtimeTypeCode] = attendanceTable.Overtime1Hours;
        //        //            }
        //        //        }

        //        //        overtimeTypeCode = attendanceTable.OvertimeTypeCode2;

        //        //        if (!string.IsNullOrWhiteSpace(overtimeTypeCode))
        //        //        {
        //        //            overtimeTypeCode = overtimeTypeCode.ToString().Replace('.', '_');
        //        //            if (tblData.Columns.Contains(overtimeTypeCode))
        //        //            {
        //        //                dr[overtimeTypeCode] = attendanceTable.Overtime2Hours;
        //        //            }
        //        //        }

        //        //        overtimeTypeCode = attendanceTable.OvertimeTypeCode3;

        //        //        if (!string.IsNullOrWhiteSpace(overtimeTypeCode))
        //        //        {
        //        //            overtimeTypeCode = overtimeTypeCode.ToString().Replace('.', '_');
        //        //            if (tblData.Columns.Contains(overtimeTypeCode))
        //        //            {
        //        //                dr[overtimeTypeCode] = attendanceTable.Overtime3Hours;
        //        //            }
        //        //        }

        //        //        overtimeTypeCode = attendanceTable.OvertimeTypeCode4;

        //        //        if (!string.IsNullOrWhiteSpace(overtimeTypeCode))
        //        //        {
        //        //            overtimeTypeCode = overtimeTypeCode.ToString().Replace('.', '_');
        //        //            if (tblData.Columns.Contains(overtimeTypeCode))
        //        //            {
        //        //                dr[overtimeTypeCode] = attendanceTable.Overtime4Hours;
        //        //            }
        //        //        }

        //        //        overtimeTypeCode = attendanceTable.OvertimeTypeCode5;

        //        //        if (!string.IsNullOrWhiteSpace(overtimeTypeCode))
        //        //        {
        //        //            overtimeTypeCode = overtimeTypeCode.ToString().Replace('.', '_');
        //        //            if (tblData.Columns.Contains(overtimeTypeCode))
        //        //            {
        //        //                dr[overtimeTypeCode] = attendanceTable.Overtime5Hours;
        //        //            }
        //        //        }

        //        //        overtimeTypeCode = attendanceTable.OvertimeTypeCode6;

        //        //        if (!string.IsNullOrWhiteSpace(overtimeTypeCode))
        //        //        {
        //        //            overtimeTypeCode = overtimeTypeCode.ToString().Replace('.', '_');
        //        //            if (tblData.Columns.Contains(overtimeTypeCode))
        //        //            {
        //        //                dr[overtimeTypeCode] = attendanceTable.Overtime6Hours;
        //        //            }
        //        //        }

        //        //        #endregion

        //        //        tblData.Rows.Add(dr);

        //        //        #region second Row (OT HOUR)
        //        //        if (isNotAllowZero)
        //        //        {
        //        //            DataRow dr_OT_Hour = tblData.NewRow();

        //        //            foreach (var attendanceTableItem in listAttendanceTableItemByTable)
        //        //            {
        //        //                int date = attendanceTableItem.WorkDate.Day;
        //        //                double totalHourOT = attendanceTableItem.OvertimeHours;
        //        //                totalHourOT += attendanceTableItem.ExtraOvertimeHours;
        //        //                totalHourOT += attendanceTableItem.ExtraOvertimeHours2;
        //        //                totalHourOT += attendanceTableItem.ExtraOvertimeHours3;
        //        //                dr_OT_Hour["Data" + date] = totalHourOT;
        //        //            }
        //        //            tblData.Rows.Add(dr_OT_Hour);
        //        //        }

        //        //        #endregion

        //        //    }

        //        //    #region Convert DataTable

        //        //    //if (tblData.Rows.Count > 0)
        //        //    //{
        //        //    //    foreach (DataRow dataRow in tblData.Rows)
        //        //    //    {
        //        //    //        dataRow[TemporaryFieldNames.Count] = tblData.Rows.Count;

        //        //    //        if (listOrgStructureID != null && listOrgStructureID.Count() > 0)
        //        //    //        {
        //        //    //            dataRow[Common.GetObjectName(ClassNames.Cat_OrgStructure)] = listProfileInfo.Where(s => s.OrgStructureID
        //        //    //                == listOrgStructureID.FirstOrDefault()).Select(d => d.OrgStructureName).FirstOrDefault();
        //        //    //        }
        //        //    //        else
        //        //    //        {
        //        //    //            dataRow[Common.GetObjectName(ClassNames.Cat_OrgStructure)] = "TOÀN CÔNG TY";
        //        //    //        }
        //        //    //    }
        //        //    //}

        //        //    //DataTable table1 = tblData.Clone();

        //        //    //foreach (DataRow dataRow in tblData.Rows)
        //        //    //{
        //        //    //    DataRow row = table1.NewRow();
        //        //    //    bool isCheck = false;

        //        //    //    foreach (DataColumn dataColumn in tblData.Columns)
        //        //    //    {
        //        //    //        if (dataColumn.ColumnName.IndexOf(InstanceNames.Data) > -1 && !isCheck)
        //        //    //        {
        //        //    //            double i = 1;

        //        //    //            for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
        //        //    //            {
        //        //    //                row[InstanceNames.Data + i] = dataRow[InstanceNames.Data + date.Day];
        //        //    //                i++;
        //        //    //            }

        //        //    //            isCheck = true;
        //        //    //        }
        //        //    //        else if (dataColumn.ColumnName.IndexOf(InstanceNames.Data) == -1)
        //        //    //        {
        //        //    //            row[dataColumn.ColumnName] = dataRow[dataColumn.ColumnName];
        //        //    //        }
        //        //    //    }

        //        //    //    table1.Rows.Add(row);
        //        //    //}

        //        //    //tblData = table1.Sort(new string[] { Hre_Profile.FieldNames.CodeEmp });

        //        //    #endregion
        //        //}

        //        //return tblData;
        //    }

        //}
        #endregion



        #region Code Cũ
        //public List<Att_ReportMonthlyTimeSheetEntity> GetReportMonthlyTimeSheet(DateTime DateFrom, DateTime DateTo, List<Guid> lstProfileIDs)
        //{


        //    List<Att_ReportMonthlyTimeSheetEntity> lstReportMonthlyTimeSheetEntity = new List<Att_ReportMonthlyTimeSheetEntity>();
        //    DataTable table = CreateReportMonthlyTimeSheetSchema();
        //    #region getData
        //    var lstWorkday = new List<Att_Workday>().Select(m => new { m.ProfileID, m.WorkDate, m.ShiftID }).ToList();
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Att_WorkDayRepository(unitOfWork);
        //        if (lstProfileIDs != null && lstProfileIDs.Count > 0)
        //        {
        //            lstWorkday = lstWorkday.Where(m => m.WorkDate >= DateFrom && m.WorkDate < DateTo && m.ProfileID != null && lstProfileIDs.Contains(m.ProfileID))
        //                .Select(m => new { m.ProfileID, m.WorkDate, m.ShiftID }).ToList();
        //        }
        //        else
        //        {
        //            lstWorkday = repo.FindBy(m => m.WorkDate >= DateFrom && m.WorkDate < DateTo)
        //               .Select(m => new { m.ProfileID, m.WorkDate, m.ShiftID }).ToList();
        //        }
        //    }
        //    var lstProfile = new List<Hre_Profile>().Select(m => new { m.ID, m.OrgStructureID, m.CodeEmp, m.ProfileName }).ToList();
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Hre_ProfileRepository(unitOfWork);
        //        if (lstProfileIDs != null && lstProfileIDs.Count > 0)
        //        {
        //            lstProfile = lstProfile.Where(m => lstProfileIDs.Contains(m.ID)).Select(m => new { m.ID, m.OrgStructureID, m.CodeEmp, m.ProfileName }).ToList();
        //        }
        //        else
        //        {
        //            lstProfile = repo.GetAll().Select(m => new { m.ID, m.OrgStructureID, m.CodeEmp, m.ProfileName }).ToList();
        //        }
        //    }
        //    List<Guid> lstProfileID = lstProfile.Select(m => m.ID).ToList();
        //    var lstShift = new List<Cat_Shift>().Select(m => new { m.ID, m.ShiftName, m.InTime, m.CoOut }).ToList();
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repo = new Cat_ShiftRepository(unitOfWork);
        //        lstShift = repo.GetAll().Select(m => new { m.ID, m.ShiftName, m.InTime, m.CoOut }).ToList();
        //    }

        //    #endregion
        //    #region get org nhieu cap
        //    List<OrgTiny> lstOrgAll = new List<OrgTiny>();
        //    using (var context = new VnrHrmDataContext())
        //    {
        //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
        //        var repoOrg = new Cat_OrgStructureRepository(unitOfWork);
        //        lstOrgAll = repoOrg.FindBy(s => s.IsDelete == null).Select(m => new OrgTiny { ID = m.ID, OrgCode = m.Code, OrgName = m.OrgStructureName, ParentID = m.ParentID, TypeID = m.TypeID }).ToList();
        //    }
        //    List<Guid?> lstOrgIDs = lstProfile.Select(m => m.OrgStructureID).Distinct().ToList();
        //    Dictionary<Guid?, OrgLevelTypeName> OrgNameAllLevel = Cat_OrgStructureServices.GetFullLinkOrg(lstOrgIDs, lstOrgAll);
        //    #endregion
        //    foreach (var ProfileID in lstProfileID)
        //    {
        //        if (ProfileID == null)
        //            continue;


        //        var lstWorkdayByProfile = lstWorkday.Where(m => m.ProfileID == ProfileID).ToList();
        //        foreach (var WorkdayByProfile in lstWorkdayByProfile)
        //        {
        //            if (WorkdayByProfile.WorkDate == null)
        //            {
        //                continue;
        //            }

        //            Guid profileiD = WorkdayByProfile.ProfileID;
        //            DateTime Date = WorkdayByProfile.WorkDate;


        //            Att_ReportMonthlyTimeSheetEntity ReportMonthlyTimeSheetEntity = new Att_ReportMonthlyTimeSheetEntity();
        //            var profileInfomation = lstProfile.Where(m => m.ID == ProfileID).FirstOrDefault();
        //            #region thông tin chung
        //            ReportMonthlyTimeSheetEntity.DateFrom = DateFrom;
        //            ReportMonthlyTimeSheetEntity.DateTo = DateTo;
        //            #endregion
        //            #region thông tin Profile
        //            if (profileInfomation != null)
        //            {
        //                ReportMonthlyTimeSheetEntity.ProfileName = profileInfomation.ProfileName;
        //                ReportMonthlyTimeSheetEntity.CodeEmp = profileInfomation.CodeEmp;
        //            }
        //            #endregion
        //            #region thông tin Phòng ban
        //            Guid? orgID = null;
        //            if (profileInfomation != null)
        //            {
        //                orgID = profileInfomation.OrgStructureID;
        //            }
        //            if (orgID != null)
        //            {
        //                var orgByProfile = lstOrgAll.Where(m => m.ID == orgID).FirstOrDefault();
        //                if (orgByProfile != null)
        //                {
        //                    //      ReportMonthlyTimeSheetEntity.DepartmentName = orgByProfile.OrgName;
        //                    ReportMonthlyTimeSheetEntity.Division = orgByProfile.OrgCode;

        //                }

        //            }
        //            if (orgID != null && OrgNameAllLevel.ContainsKey(orgID))
        //            {
        //                var OrgNameFull = OrgNameAllLevel[orgID];
        //                if (OrgNameFull != null)
        //                {
        //                    ReportMonthlyTimeSheetEntity.SectionCode = OrgNameFull.SectionCode;
        //                    ReportMonthlyTimeSheetEntity.GroupCode = OrgNameFull.TeamCode;
        //                }
        //            }
        //            #endregion

        //            #region thông tin ngày nghỉ có lương
        //            #endregion
        //            #region thông tin ngày nghỉ bệnh
        //            #endregion

        //            lstReportMonthlyTimeSheetEntity.Add(ReportMonthlyTimeSheetEntity);

        //        }
        //    }


        //    return lstReportMonthlyTimeSheetEntity;
        //}
        #endregion




        /// <summary>
        /// Khởi Tạo DataTable cho BC Chi Tiết Quên Quẹt Thẻ
        /// </summary>
        /// <returns></returns>

        DataTable CreateReportDetailForgetCardSchema()
        {

            DataTable tb = new DataTable("Att_ReportDetailForgetCardEntity");
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.ProfileName);
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.PositionName);
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.JobtitleName);
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.GroupCode);
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.TeamCode);
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.BranchCode);
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.SectionCode);
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.DepartmentName);
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.DateFrom, typeof(DateTime));
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.DateTo, typeof(DateTime));
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.DateExport, typeof(DateTime));
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.Division);
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.Date, typeof(DateTime));
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.ShiftName);
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.InTime, typeof(DateTime));
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.OutTime, typeof(DateTime));
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.UserExport);
            tb.Columns.Add(Att_ReportDetailForgetCardEntity.FieldNames.ScanType);
            return tb;
        }

        /// <summary>
        /// Lấy Dữ Liệu BC GetReportDetailForgetCard
        /// </summary>
        /// <returns></returns>
        public DataTable GetReportDetailForgetCard(DateTime? DateFrom, DateTime? DateTo, List<Guid> lstProfileIDs, List<Guid?> ShiftIDs, string userExport)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Workday = new Att_WorkDayRepository(unitOfWork);
                DataTable table = CreateReportDetailForgetCardSchema();
                List<string> lstType = new List<string> { 
                WorkdayType.E_MISS_IN.ToString(),
                WorkdayType.E_MISS_IN_OUT.ToString(),
                WorkdayType.E_MISS_OUT.ToString()
                };

                var workDays = repoAtt_Workday.FindBy(s =>
                  DateFrom <= s.WorkDate && s.WorkDate <= DateTo
                  && lstType.Contains(s.Type))
                  .Select(s => new { s.ProfileID, s.ShiftID, s.WorkDate, s.FirstInTime, s.LastOutTime, s.Type, s.InTime1, s.OutTime1 }).ToList();
                if (lstProfileIDs != null)
                {
                    workDays = workDays.Where(s => lstProfileIDs.Contains(s.ProfileID)).ToList();
                }

                var profileIds = workDays.Select(s => s.ProfileID).Distinct().ToList();

                var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoHre_Profile.FindBy(s => (s.DateQuit == null || s.DateQuit > DateTo) && profileIds.Contains(s.ID))
                  .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();

                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();

                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var positions = repoCat_Position.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();

                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();

                var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                var shifts = repoCat_Shift.GetAll().ToList();

                if (ShiftIDs[0] != null)
                {
                    workDays = workDays.Where(s => s.ShiftID.HasValue && ShiftIDs.Contains(s.ShiftID.Value)).ToList();
                }

                List<Guid> guids = profiles.Select(s => s.ID).ToList();

                if (guids.Count > 0)
                {
                    workDays = workDays.Where(s => guids.Contains(s.ProfileID)).ToList();
                }

                foreach (var profile in profiles)
                {
                    for (DateTime date = DateFrom.Value.Date; date <= DateTo; date = date.AddDays(1))
                    {
                        var workDayProfiles = workDays.Where(s => s.WorkDate.Date == date && s.ProfileID == profile.ID).ToList();
                        if (workDayProfiles.Count > 0)
                        {
                            DataRow row = table.NewRow();

                            //var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                            //var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);

                            Guid? orgId = profile.OrgStructureID;
                            var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                            var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                            var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                            var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);

                            row[Att_ReportDetailForgetCardEntity.FieldNames.GroupCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                            row[Att_ReportDetailForgetCardEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.Code : string.Empty;
                            row[Att_ReportDetailForgetCardEntity.FieldNames.Division] = orgTeam != null ? orgTeam.Code : string.Empty;
                            row[Att_ReportDetailForgetCardEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;


                            row[Att_ReportDetailForgetCardEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                            row[Att_ReportDetailForgetCardEntity.FieldNames.ProfileName] = profile.ProfileName;
                            row[Att_ReportDetailForgetCardEntity.FieldNames.DateFrom] = DateFrom;
                            row[Att_ReportDetailForgetCardEntity.FieldNames.DateTo] = DateTo;
                            row[Att_ReportDetailForgetCardEntity.FieldNames.DateExport] = DateTime.Today;
                            row[Att_ReportDetailForgetCardEntity.FieldNames.UserExport] = userExport;

                            var workDay1 = workDayProfiles.FirstOrDefault();
                            row[Att_ReportDetailForgetCardEntity.FieldNames.Date] = date;

                            if (workDay1.ShiftID != null)
                            {
                                var shift = shifts.Where(m => m.ID == workDay1.ShiftID).FirstOrDefault();
                                row[Att_ReportDetailForgetCardEntity.FieldNames.ShiftName] = shift != null ? shift.ShiftName : string.Empty;
                            }
                            if (workDay1.Type == WorkdayType.E_MISS_IN.ToString())
                            {
                                row[Att_ReportDetailForgetCardEntity.FieldNames.OutTime] = workDay1.LastOutTime != null ? workDay1.LastOutTime.Value : DateTime.MinValue;

                            }
                            else if (workDay1.Type == WorkdayType.E_MISS_OUT.ToString())
                            {
                                row[Att_ReportDetailForgetCardEntity.FieldNames.InTime] = workDay1.FirstInTime != null ? workDay1.FirstInTime.Value : DateTime.MaxValue;
                            }


                            row[Att_ReportDetailForgetCardEntity.FieldNames.ScanType] = workDay1.Type;
                            table.Rows.Add(row);

                        }
                    }
                }


                return table;
            }

            #region Code Cũ
            //#region logic hien tai
            ////1. Chi tinh dc workday thieu in thieu out hoac thieu in out
            ////2. khong tinh dc nhưng quẹt thẻ da them bang tay boi vi khi them bang tay thì workday da du inout hok len dc bc
            //#endregion
            //List<Att_ReportDetailForgetCardEntity> lstReportDetailForgetCardEntity = new List<Att_ReportDetailForgetCardEntity>();
            //string E_MISS_IN = WorkdayType.E_MISS_IN.ToString();
            //string E_MISS_OUT = WorkdayType.E_MISS_OUT.ToString();
            //string E_MISS_IN_OUT = WorkdayType.E_MISS_IN_OUT.ToString();
            //#region getData
            //var lstWorkday = new List<Att_Workday>().Select(m => new { m.ProfileID, m.WorkDate, m.ShiftID, m.Type, m.FirstInTime, m.LastOutTime }).ToList();
            //using (var context = new VnrHrmDataContext())
            //{
            //    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
            //    var repo = new Att_WorkDayRepository(unitOfWork);
            //    if (lstProfileIDs != null && lstProfileIDs.Count > 0)
            //    {
            //        lstWorkday = repo.FindBy(m => m.WorkDate >= DateFrom && m.WorkDate < DateTo
            //        && m.ProfileID != null && lstProfileIDs.Contains(m.ProfileID)
            //        && (m.Type == E_MISS_IN || m.Type == E_MISS_OUT || m.Type == E_MISS_IN_OUT))
            //            .Select(m => new { m.ProfileID, m.WorkDate, m.ShiftID, m.Type, m.FirstInTime, m.LastOutTime }).ToList();
            //    }
            //    else
            //    {
            //        lstWorkday = repo.FindBy(m => (m.WorkDate >= DateFrom && m.WorkDate < DateTo)
            //       && (m.Type == E_MISS_IN || m.Type == E_MISS_OUT || m.Type == E_MISS_IN_OUT))
            //           .Select(m => new { m.ProfileID, m.WorkDate, m.ShiftID, m.Type, m.FirstInTime, m.LastOutTime }).ToList();
            //    }
            //}
            //List<Guid> lstprofilebyworkday = lstWorkday.Select(m => m.ProfileID).ToList();
            //var lstProfile = new List<Hre_Profile>().Select(m => new { m.ID, m.OrgStructureID, m.CodeEmp, m.ProfileName });
            //using (var context = new VnrHrmDataContext())
            //{
            //    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
            //    var repo = new Hre_ProfileRepository(unitOfWork);
            //    if (lstprofilebyworkday != null && lstprofilebyworkday.Count > 0)
            //    {
            //        lstProfile = repo.FindBy(m => lstprofilebyworkday.Contains(m.ID)).Select(m => new { m.ID, m.OrgStructureID, m.CodeEmp, m.ProfileName }).ToList();
            //    }
            //}

            //var lstShift = new List<Cat_Shift>().Select(m => new { m.ID, m.ShiftName, m.InTime, m.CoOut }).ToList();
            //using (var context = new VnrHrmDataContext())
            //{
            //    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
            //    var repo = new Cat_ShiftRepository(unitOfWork);
            //    lstShift = repo.GetAll().Select(m => new { m.ID, m.ShiftName, m.InTime, m.CoOut }).ToList();
            //}

            //#endregion
            //#region get org nhieu cap
            //List<OrgTiny> lstOrgAll = new List<OrgTiny>();
            //using (var context = new VnrHrmDataContext())
            //{
            //    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
            //    var repoOrg = new Cat_OrgStructureRepository(unitOfWork);
            //    lstOrgAll = repoOrg.FindBy(s => s.IsDelete == null).Select(m => new OrgTiny { ID = m.ID, OrgCode = m.Code, OrgName = m.OrgStructureName, ParentID = m.ParentID, TypeID = m.TypeID }).ToList();
            //}
            //List<Guid?> lstOrgIDs = lstProfile.Select(m => m.OrgStructureID).Distinct().ToList();
            //Dictionary<Guid?, OrgLevelTypeName> OrgNameAllLevel = Cat_OrgStructureServices.GetFullLinkOrg(lstOrgIDs, lstOrgAll);
            //#endregion
            //foreach (var ProfileID in lstprofilebyworkday)
            //{
            //    if (ProfileID == null)
            //        continue;
            //    var lstWorkdayByProfile = lstWorkday.Where(m => m.ProfileID == ProfileID).ToList();
            //    foreach (var WorkdayByProfile in lstWorkdayByProfile)
            //    {
            //        if (WorkdayByProfile.WorkDate == null)
            //        {
            //            continue;
            //        }
            //        Att_ReportDetailForgetCardEntity ReportDetailForgetCardEntity = new Att_ReportDetailForgetCardEntity();
            //        var profileInfomation = lstProfile.Where(m => m.ID == ProfileID).FirstOrDefault();
            //        #region thông tin chung
            //        ReportDetailForgetCardEntity.DateFrom = DateFrom;
            //        ReportDetailForgetCardEntity.DateTo = DateTo;
            //        #endregion
            //        #region thông tin Profile
            //        if (profileInfomation != null)
            //        {
            //            ReportDetailForgetCardEntity.ProfileName = profileInfomation.ProfileName;
            //            ReportDetailForgetCardEntity.CodeEmp = profileInfomation.CodeEmp;
            //        }
            //        #endregion
            //        #region thông tin Phòng ban
            //        Guid? orgID = null;
            //        if (profileInfomation != null)
            //        {
            //            orgID = profileInfomation.OrgStructureID;
            //        }
            //        if (orgID != null)
            //        {
            //            var orgByProfile = lstOrgAll.Where(m => m.ID == orgID).FirstOrDefault();
            //            if (orgByProfile != null)
            //            {
            //                ReportDetailForgetCardEntity.DepartmentName = orgByProfile.OrgName;
            //                ReportDetailForgetCardEntity.Division = orgByProfile.OrgCode;
            //            }

            //        }
            //        if (orgID != null && OrgNameAllLevel.ContainsKey(orgID))
            //        {
            //            var OrgNameFull = OrgNameAllLevel[orgID];
            //            if (OrgNameFull != null)
            //            {
            //                ReportDetailForgetCardEntity.SectionCode = OrgNameFull.SectionCode;
            //                ReportDetailForgetCardEntity.GroupCode = OrgNameFull.TeamCode;
            //            }
            //        }
            //        #endregion
            //        #region thông tin chấm công
            //        ReportDetailForgetCardEntity.Date = WorkdayByProfile.WorkDate;
            //        if (WorkdayByProfile.ShiftID != null)
            //        {
            //            var Shift = lstShift.Where(m => m.ID == WorkdayByProfile.ShiftID).FirstOrDefault();
            //            if (Shift != null)
            //            {
            //                ReportDetailForgetCardEntity.ShiftName = Shift.ShiftName;
            //            }
            //        }
            //        #endregion
            //        #region thoi gian vao ra
            //        if (WorkdayByProfile.Type == E_MISS_IN)
            //        {
            //            ReportDetailForgetCardEntity.OutTime = WorkdayByProfile.LastOutTime;
            //        }
            //        else if (WorkdayByProfile.Type == E_MISS_OUT)
            //        {
            //            ReportDetailForgetCardEntity.InTime = WorkdayByProfile.FirstInTime;
            //        }
            //        ReportDetailForgetCardEntity.ScanType = WorkdayByProfile.Type;
            //        #endregion

            //        lstReportDetailForgetCardEntity.Add(ReportDetailForgetCardEntity);
            //    }
            //}

            //return lstReportDetailForgetCardEntity;
            #endregion

        }
        /// <summary>
        /// Lấy Dữ Liệu BC tổng hợp nghỉ hàng năm
        /// </summary>
        /// <returns></returns>
        public List<Att_ReportLeaveYearEntity> GetReportLeaveYear()
        {
            List<Att_ReportLeaveYearEntity> lstReportLeaveYearEntity = new List<Att_ReportLeaveYearEntity>();
            return lstReportLeaveYearEntity;
        }
        /// <summary>
        /// Lấy Dữ Liệu BC tăng ca vượt giới hạn
        /// </summary>
        /// <returns></returns>
        public List<Att_ReportOvertimeOverLimitEntity> GetReportOvertimeOverLimit(List<Guid?> OrgStrutuceID, DateTime? Year)
        {
            List<Att_ReportOvertimeOverLimitEntity> lstReportOvertimeOverLimit = new List<Att_ReportOvertimeOverLimitEntity>();
            return lstReportOvertimeOverLimit;
        }

        /// <summary>
        /// Lấy Dữ Liệu phân tích ngày nghỉ và trễ sớm
        /// </summary>
        /// <returns></returns>
        public List<Att_AnalysysLeaveAndLateEarlyEntity> GetAnalysysLeaveAndLateEarlyData(DateTime DateFrom, DateTime DateTo, List<Guid> OrgStrutuceID)
        {
            List<Att_AnalysysLeaveAndLateEarlyEntity> AnalysysLeaveAndLateEarlyData = new List<Att_AnalysysLeaveAndLateEarlyEntity>();
            return AnalysysLeaveAndLateEarlyData;
        }
        /// <summary>
        /// Lấy Dữ Liệu vào ra 
        /// </summary>
        /// <returns></returns>
        public List<Att_ImportFromTAMEntity> GetImportFromTAMData()
        {
            List<Att_ImportFromTAMEntity> GetImportFromTAMData = new List<Att_ImportFromTAMEntity>();
            return GetImportFromTAMData;
        }
        /// <summary>
        /// Lấy Dữ Liệu BC vi phạm đăng ký
        /// </summary>
        /// <returns></returns>
        public List<Att_ReportWorkDateLimitEntity> GetReportWorkDateLimit(DateTime? DateFrom, DateTime? DateTo, List<Guid?> OrgStrutuceID, List<Guid?> PositionID, Guid? TemplateId)
        {
            List<Att_ReportWorkDateLimitEntity> ReportWorkDateLimit = new List<Att_ReportWorkDateLimitEntity>();
            return ReportWorkDateLimit;
        }
        /// <summary>
        /// Lấy Dữ Liệu BC Chi Tiết Nghỉ Hàng Ngày của nhân viên
        /// </summary>
        /// <returns></returns>
        public List<Att_ReportLeavedayEntity> GetReportLeaveday()
        {
            List<Att_ReportLeavedayEntity> lstLeavedayEntity = new List<Att_ReportLeavedayEntity>();
            return lstLeavedayEntity;
        }
        DataTable CreateReportLeavedaySchema()
        {

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                DataTable tb = new DataTable("Att_ReportLeavedayEntity");
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.ProfileName);
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.DepartmentCode);
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.JobTitleName);
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.PositionName);

                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.DateLeaday, typeof(DateTime));
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.ShiftName);

                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.Paid);
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.InTime, typeof(DateTime));
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.OutTime, typeof(DateTime));

                var repoLeadayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                var codes = repoLeadayType.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => s.Code).Distinct().ToList<string>();
                foreach (var code in codes)
                {
                    if (!tb.Columns.Contains(code))
                    {
                        tb.Columns.Add(code);
                    }
                }
                var repoTAMscanReasonMiss = new CustomBaseRepository<Cat_TAMScanReasonMiss>(unitOfWork);
                var TAMscanReasonMiss = repoTAMscanReasonMiss.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => s.Code).ToList<string>();
                foreach (var code in TAMscanReasonMiss)
                {
                    if (!tb.Columns.Contains("ReasonMiss_" + code))
                    {
                        tb.Columns.Add("ReasonMiss_" + code);
                    }
                }
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.BranchCode);
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.TeamCode);
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.SectionCode);
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.JobTitleCode);
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.PositionCode);
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.GroupCode);
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.DivisionCode);
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.BranchName);
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.DepartmentName);
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.TeamName);
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.SectionName);
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.GroupName);
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.DivisionName);

                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.FromDate, typeof(DateTime));
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.ToDate, typeof(DateTime));
                tb.Columns.Add(Att_ReportLeavedayEntity.FieldNames.DateExport, typeof(DateTime));

                return tb;
            }
        }

        public DataTable GetReportLeaveday(DateTime dateStart, DateTime dateEnd, string orgStructureID, List<Hre_ProfileEntity> lstProfileIDsModel, Guid[] shiftIDs, Guid[] leaveDayTypeIDs,
            bool? _isIncludeQuitEmp, bool? _isIncludeReasonMissFromWorkday, string userExport, string codeEmp, string UserLogin)
        {
            List<Guid> lstProfileIDs = lstProfileIDsModel.Select(s => s.ID).ToList();
            if (dateEnd != null)
                dateEnd = dateEnd.AddDays(1).AddMilliseconds(-1);

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                string status = OverTimeStatus.E_APPROVED.ToString();
                var repoAtt_Leaveday = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);
                var repoCat_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                var repoAtt_WorkDay = new CustomBaseRepository<Att_Workday>(unitOfWork);
                var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                var repoCat_Position = new CustomBaseRepository<Cat_Position>(unitOfWork);
                var repoCat_OrgStructureType = new CustomBaseRepository<Cat_OrgStructureType>(unitOfWork);
                var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);
                var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);
                var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);
                var repoCat_GradeAttendance = new CustomBaseRepository<Cat_GradeAttendance>(unitOfWork);
                var repoHre_WorkHistory = new CustomBaseRepository<Hre_WorkHistory>(unitOfWork);
                var repoCat_TAMScanReasonMiss = new CustomBaseRepository<Cat_TAMScanReasonMiss>(unitOfWork);
                var repoAtt_RosterGroup = new CustomBaseRepository<Att_RosterGroup>(unitOfWork);

                string key = OverTimeStatus.E_APPROVED.ToString();
                var baseservice = new BaseService();
                List<object> para = new List<object>();
                para.AddRange(new object[3]);
                para[0] = (object)orgStructureID;
                para[1] = dateStart;
                para[2] = dateEnd;

                List<Guid> lstOrgstructID = new List<Guid>();
                //if (!string.IsNullOrEmpty(orgStructureID))
                //{
                //    List<string> lstOrgStructureStr = orgStructureID.Split(',').ToList();
                //    foreach (var strOrg in lstOrgStructureStr)
                //    {
                //        if (!string.IsNullOrEmpty(strOrg))
                //        {
                //            var guidOrg = Guid.Parse(strOrg);
                //            if (!guidOrg.IsNullOrEmpty())
                //            {
                //                lstOrgstructID.Add(guidOrg);
                //            }
                //        }
                //    }
                //}
                var lstOrg = repoCat_OrgStructure.FindBy(s => s.IsDelete == null).ToList();
                if (!string.IsNullOrEmpty(orgStructureID))
                {
                    int[] orderNumber = null;
                    orderNumber = orgStructureID.Split(',').Select(s => int.Parse(s)).ToArray();

                    if (orderNumber != null)
                    {
                        lstOrg = lstOrg.Where(s => orderNumber.Contains(s.OrderNumber)).ToList();
                    }
                }
                lstOrgstructID = lstOrg.Select(s => s.ID).ToList();

                //var leaveDays = baseservice.GetData<Att_LeaveDayEntity>(para, ConstantSql.hrm_att_getdata_LeaveDay, ref status)
                var leaveDays = repoAtt_Leaveday.FindBy(s => s.TotalDuration > 0 && s.Status == key && dateStart <= s.DateEnd && s.DateStart <= dateEnd) // Tin.Nguyen hiện tại cột OrgStructureID trong bảng Att_Leaveday dang null nên tạm thời xóa dk tìm kiếm s.OrgStructureID != null var lstOrgstructID.Contains(s.OrgStructureID.Value)
                                .Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.Status, s.TotalDuration, s.DateStart, s.DateEnd, s.LeaveDays, s.OrgStructureID }).ToList();

                //var workDays = baseservice.GetData<Att_WorkdayEntity>(para, ConstantSql.hrm_att_getdata_Workday, ref status)
                var workDays = repoAtt_WorkDay.FindBy(wd => wd.OrgStructureID.HasValue && lstOrgstructID.Contains(wd.OrgStructureID.Value) && dateStart <= wd.WorkDate && wd.WorkDate <= dateEnd)
                                .Select(s => new { s.ProfileID, s.ShiftID, s.WorkDate, s.InTime1, s.OutTime1, s.MissInOutReason, s.OrgStructureID }).ToList();

                var profileIds = leaveDays.Select(s => s.ProfileID).Distinct().ToList();
                if (_isIncludeReasonMissFromWorkday == true)
                {
                    profileIds.AddRange(workDays.Where(m => m.MissInOutReason != null).Select(m => m.ProfileID).Distinct().ToList<Guid>());
                }
                var profiles = lstProfileIDsModel.Where(s => profileIds.Contains(s.ID))
                                .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();

                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                var positions = repoCat_Position.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
                var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();
                var leavedayTypes = repoCat_LeaveDayType.FindBy(s => s.IsDelete == null && s.Code != null).Select(s => new { s.ID, s.Code, s.PaidRate }).Distinct().ToList();
                var ledvedayPaidIds = leavedayTypes.Where(s => s.PaidRate > 0).Select(s => s.ID).ToList();
                var shifts = repoCat_Shift.FindBy(s => s.IsDelete == null).ToList();
                if (shiftIDs != null)
                {
                    workDays = workDays.Where(s => s.ShiftID.HasValue && shiftIDs.Contains(s.ShiftID.Value)).ToList();
                    profileIds = workDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }
                if (leaveDayTypeIDs != null)
                {
                    leaveDays = leaveDays.Where(s => leaveDayTypeIDs.Contains(s.LeaveDayTypeID)).ToList();
                    profileIds = leaveDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }

                if (_isIncludeQuitEmp == false)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateStart).ToList();
                }

                if (!string.IsNullOrEmpty(codeEmp))
                {
                    char[] ext = new char[] { ';', ',' };
                    List<string> codeEmpSeachs = codeEmp.Split(ext, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                    if (codeEmpSeachs.Count == 1)
                    {
                        string codeEmpSearch = codeEmpSeachs[0];
                        profiles = profiles.Where(hr => hr.CodeEmp == codeEmpSearch).ToList();
                        profileIds = profiles.Select(hr => hr.ID).ToList();
                    }
                    else
                    {
                        profiles = profiles.Where(hr => codeEmpSeachs.Contains(hr.CodeEmp)).ToList();
                        profileIds = profiles.Select(hr => hr.ID).ToList();
                    }
                }

                var rosterStatus = RosterStatus.E_APPROVED.ToString();
                List<object> para_Roster = new List<object>();
                para_Roster.AddRange(new object[4]);
                para_Roster[0] = (object)orgStructureID;
                para_Roster[1] = dateStart;
                para_Roster[2] = dateEnd;
                para_Roster[3] = rosterStatus;
                var listRoster = baseservice.GetData<Att_RosterEntity>(para_Roster, ConstantSql.hrm_att_getdata_Roster_Inner, UserLogin, ref status).ToList();

                var listHoliday = repoCat_DayOff.FindBy(s => s.IsDelete == null && s.DateOff >= dateStart && s.DateOff <= dateEnd).ToList();
                var lstHolidayDates = listHoliday.Select(hl => hl.DateOff).ToList();

                var listGrade = repoAtt_Grade.FindBy(s => s.IsDelete == null && s.MonthStart != null && s.MonthStart < dateEnd && profileIds.Contains(s.ProfileID.Value))
                                .Select(s => new { s.ProfileID, s.MonthStart, s.ID, s.GradeAttendanceID }).ToList();

                List<Guid> listGradeID = listGrade.Select(d => d.GradeAttendanceID.Value).ToList();
                var listGradeCfg = repoCat_GradeAttendance.FindBy(s => s.IsDelete == null && listGradeID.Contains(s.ID)).ToList();

                List<Att_Roster> lstRosterTypeGroup = new List<Att_Roster>();
                List<Att_RosterGroup> lstRosterGroup = new List<Att_RosterGroup>();
                string E_ROSTERGROUP = RosterType.E_ROSTERGROUP.ToString();
                if (dateStart == null || dateEnd == null)
                {
                    lstRosterTypeGroup = listRoster.Where(s => s.Type == E_ROSTERGROUP).ToList().Translate<Att_Roster>();
                    lstRosterGroup = repoAtt_RosterGroup.FindBy(s => s.IsDelete == null && s.DateStart != null && s.DateEnd != null).ToList();
                }
                else
                {
                    lstRosterTypeGroup = listRoster.Where(s => s.Type == E_ROSTERGROUP).ToList().Translate<Att_Roster>();
                    lstRosterGroup = repoAtt_RosterGroup.FindBy(s => s.IsDelete == null && s.DateStart <= dateStart && s.DateEnd >= dateEnd).ToList();
                }

                var listWorkHistory = repoHre_WorkHistory.FindBy(s => s.IsDelete == null && profileIds.Contains(s.ProfileID) && s.DateEffective <= dateEnd).ToList();
                var listCat_TAMScanReasonMiss = repoCat_TAMScanReasonMiss.FindBy(s => s.IsDelete == null).ToList();

                DataTable table = CreateReportLeavedaySchema();

                List<string> codeRejects = new List<string>();
                codeRejects.Add("SICK");
                codeRejects.Add("SU");
                codeRejects.Add("SD");
                codeRejects.Add("D");
                codeRejects.Add("DP");
                codeRejects.Add("PSN");
                codeRejects.Add("DSP");
                codeRejects.Add("M");
                List<Guid> leaveDayTypeRejectIDs = leavedayTypes.Where(s => codeRejects.Contains(s.Code)).Select(s => s.ID).ToList();

                #region foreach
                foreach (var profile in profiles)
                {
                    var rosterProfiles = listRoster.Where(s => s.ProfileID == profile.ID).ToList();
                    var leaveDayProfiles = leaveDays.Where(s => s.ProfileID == profile.ID).ToList();
                    List<DateTime> workDayDates = new List<DateTime>();
                    foreach (var roster in rosterProfiles)
                    {
                        for (DateTime date = roster.DateStart; date <= roster.DateEnd; date = date.AddDays(1))
                        {
                            workDayDates.Add(date.Date);
                        }
                    }
                    workDayDates = workDayDates.Where(s => !lstHolidayDates.Contains(s.Date)).ToList();
                    List<DateTime> leaveDates = new List<DateTime>();

                    foreach (var leaveday in leaveDayProfiles)
                    {
                        var DateEnd = leaveday.DateEnd;
                        var DateStart = leaveday.DateStart;
                        if (DateEnd > dateEnd)
                        {
                            DateEnd = dateEnd;
                        }
                        if (DateStart < dateStart)
                        {
                            DateStart = dateStart;
                        }
                        for (DateTime date = DateStart; date <= DateEnd; date = date.AddDays(1))
                        {
                            leaveDates.Add(date);
                            bool isWorkDay = workDayDates.Contains(date);
                            if (!isWorkDay)// neu ngay do ko phai ngay di lam
                            {
                                if (leaveDayTypeRejectIDs.Contains(leaveday.LeaveDayTypeID))// neu ngay do la ngay nghi dc xem la ko xem ca or ngay nghi
                                {
                                    isWorkDay = true;
                                }
                            }
                            if (isWorkDay)
                            {
                                DataRow row = table.NewRow();
                                Guid? orgId = leaveday.OrgStructureID;
                                var org = orgs.Where(og => og.ID == orgId).FirstOrDefault();
                                var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                                var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                                var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                                var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);

                                row[Att_ReportLeavedayEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                                row[Att_ReportLeavedayEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                                row[Att_ReportLeavedayEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                                row[Att_ReportLeavedayEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                                row[Att_ReportLeavedayEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                                row[Att_ReportLeavedayEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                                row[Att_ReportLeavedayEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                                row[Att_ReportLeavedayEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                                var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                                var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
                                row[Att_ReportLeavedayEntity.FieldNames.ProfileName] = profile.ProfileName;
                                row[Att_ReportLeavedayEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                                row[Att_ReportLeavedayEntity.FieldNames.PositionCode] = positon != null ? positon.Code : string.Empty;
                                row[Att_ReportLeavedayEntity.FieldNames.JobTitleCode] = jobtitle != null ? jobtitle.Code : string.Empty;
                                row[Att_ReportLeavedayEntity.FieldNames.DateLeaday] = date;
                                row[Att_ReportLeavedayEntity.FieldNames.FromDate] = dateStart;
                                row[Att_ReportLeavedayEntity.FieldNames.ToDate] = dateEnd;
                                row[Att_ReportLeavedayEntity.FieldNames.DateExport] = DateTime.Now;
                                var workDay = workDays.FirstOrDefault(s => s.WorkDate.Date == date && s.ProfileID == profile.ID);
                                if (workDay != null)
                                {
                                    var shift = shifts.FirstOrDefault(s => s.ID == workDay.ShiftID);
                                    row[Att_ReportLeavedayEntity.FieldNames.ShiftName] = shift != null ? shift.ShiftName : string.Empty;
                                    if (workDay.InTime1 != null)
                                    {
                                        row[Att_ReportLeavedayEntity.FieldNames.InTime] = workDay.InTime1.Value.ToString("HH:mm:ss");
                                    }
                                    if (workDay.OutTime1 != null)
                                    {
                                        row[Att_ReportLeavedayEntity.FieldNames.OutTime] = workDay.OutTime1.Value.ToString("HH:mm:ss");
                                    }
                                }
                                if (leaveday.TotalDuration > 0)
                                {
                                    var typeLeave = leavedayTypes.FirstOrDefault(m => m.ID == leaveday.LeaveDayTypeID);
                                    if (typeLeave != null && typeLeave.PaidRate > 0)
                                        row[Att_ReportLeavedayEntity.FieldNames.Paid] = "X";
                                }
                                var leadayeType = leavedayTypes.FirstOrDefault(s => s.ID == leaveday.LeaveDayTypeID);
                                if (leadayeType != null)
                                {
                                    row[leadayeType.Code] = "X";
                                }
                                table.Rows.Add(row);
                            }
                        }
                    }

                    #region if (_isIncludeReasonMissFromWorkday)
                    if (_isIncludeReasonMissFromWorkday == true)
                    {
                        var workDayProfiles = workDays.Where(s => s.ProfileID == profile.ID && !leaveDates.Contains(s.WorkDate.Date)
                                                && workDayDates.Contains(s.WorkDate.Date) && s.MissInOutReason != null).ToList();
                        foreach (var workDayProfile in workDayProfiles)
                        {
                            DataRow row = table.NewRow();
                            Guid? orgId = workDayProfile.OrgStructureID;
                            var org = orgs.Where(s => s.ID == orgId).FirstOrDefault();
                            var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_DIVISION, orgs, orgTypes);
                            var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                            var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                            var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);

                            row[Att_ReportLeavedayEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                            row[Att_ReportLeavedayEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                            row[Att_ReportLeavedayEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                            row[Att_ReportLeavedayEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;
                            row[Att_ReportLeavedayEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                            row[Att_ReportLeavedayEntity.FieldNames.DepartmentName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                            row[Att_ReportLeavedayEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                            row[Att_ReportLeavedayEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                            var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                            var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
                            row[Att_ReportLeavedayEntity.FieldNames.ProfileName] = profile.ProfileName;
                            row[Att_ReportLeavedayEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                            row[Att_ReportLeavedayEntity.FieldNames.PositionCode] = positon != null ? positon.Code : string.Empty;
                            row[Att_ReportLeavedayEntity.FieldNames.JobTitleCode] = jobtitle != null ? jobtitle.Code : string.Empty;
                            var shift = shifts.FirstOrDefault(s => s.ID == workDayProfile.ShiftID);
                            row[Att_ReportLeavedayEntity.FieldNames.DateLeaday] = workDayProfile.WorkDate;
                            row[Att_ReportLeavedayEntity.FieldNames.FromDate] = dateStart;
                            row[Att_ReportLeavedayEntity.FieldNames.ToDate] = dateEnd;
                            row[Att_ReportLeavedayEntity.FieldNames.DateExport] = DateTime.Now;
                            row[Att_ReportLeavedayEntity.FieldNames.ShiftName] = shift != null ? shift.ShiftName : string.Empty;
                            if (workDayProfile.InTime1 != null)
                            {
                                row[Att_ReportLeavedayEntity.FieldNames.InTime] = workDayProfile.InTime1.Value.ToString("HH:mm:ss");
                            }
                            if (workDayProfile.OutTime1 != null)
                            {
                                row[Att_ReportLeavedayEntity.FieldNames.OutTime] = workDayProfile.OutTime1.Value.ToString("HH:mm:ss");
                            }
                            var missReason = listCat_TAMScanReasonMiss.FirstOrDefault(m => m.ID == workDayProfile.MissInOutReason);
                            if (missReason != null && missReason.Code != null)
                            {
                                row["ReasonMiss_" + missReason.Code] = "X";
                            }
                            table.Rows.Add(row);
                        }
                    }
                    #endregion

                }
                #endregion

                var configs = new Dictionary<string, Dictionary<string, object>>();
                var config = new Dictionary<string, object>();
                var config80 = new Dictionary<string, object>();
                var config85 = new Dictionary<string, object>();
                config80.Add("width", 80);
                if (!configs.ContainsKey("Paid"))
                    configs.Add(Att_ReportLeavedayEntity.FieldNames.Paid, config80);
                foreach (var leaday in leavedayTypes)
                {
                    if (!configs.ContainsKey(leaday.Code))
                        configs.Add(leaday.Code, config80);
                    // leavedayTypes = leavedayTypes.Where(s => s.Code != leaday.Code).ToList();
                }
                config85.Add("width", 85);
                if (!configs.ContainsKey("CodeEmp"))
                    configs.Add(Att_ReportLeavedayEntity.FieldNames.CodeEmp, config85);

                var config100 = new Dictionary<string, object>();
                config100.Add("width", 100);
                if (!configs.ContainsKey("ProfileName"))
                    configs.Add(Att_ReportLeavedayEntity.FieldNames.ProfileName, config100);
                if (!configs.ContainsKey("DepartmentCode"))
                    configs.Add(Att_ReportLeavedayEntity.FieldNames.DepartmentCode, config100);
                if (!configs.ContainsKey("JobTitleName"))
                    configs.Add(Att_ReportLeavedayEntity.FieldNames.JobTitleName, config100);
                if (!configs.ContainsKey("PositionName"))
                    configs.Add(Att_ReportLeavedayEntity.FieldNames.PositionName, config100);
                if (!configs.ContainsKey("ShiftName"))
                    configs.Add(Att_ReportLeavedayEntity.FieldNames.ShiftName, config100);
                if (!configs.ContainsKey("BranchCode"))
                    configs.Add(Att_ReportLeavedayEntity.FieldNames.BranchCode, config100);
                if (!configs.ContainsKey("TeamCode"))
                    configs.Add(Att_ReportLeavedayEntity.FieldNames.TeamCode, config100);
                if (!configs.ContainsKey("SectionCode"))
                    configs.Add(Att_ReportLeavedayEntity.FieldNames.SectionCode, config100);
                if (!configs.ContainsKey("JobTitleCode"))
                    configs.Add(Att_ReportLeavedayEntity.FieldNames.JobTitleCode, config100);
                if (!configs.ContainsKey("PositionCode"))
                    configs.Add(Att_ReportLeavedayEntity.FieldNames.PositionCode, config100);
                config = new Dictionary<string, object>();
                config.Add("width", 80);
                config.Add("format", "{0:dd/MM/yyyy}");
                if (!configs.ContainsKey("DateLeaday"))
                    configs.Add(Att_ReportLeavedayEntity.FieldNames.DateLeaday, config);
                config = new Dictionary<string, object>();
                config.Add("width", 80);
                config.Add("format", "{0:HH:mm:ss}");
                if (!configs.ContainsKey("InTime"))
                    configs.Add(Att_ReportLeavedayEntity.FieldNames.InTime, config);
                if (!configs.ContainsKey("OutTime"))
                    configs.Add(Att_ReportLeavedayEntity.FieldNames.OutTime, config);
                return table.ConfigTable(configs);
            }

        }

        public List<Hre_Profile> GetWorkingProfile(DateTime from, DateTime to, bool isIncludeQuitEmp)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                string waitStatus = ProfileStatusSyn.E_WAITING.ToString();
                var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                List<Hre_Profile> listProfile = repoHre_Profile.FindBy(pro => pro.StatusSyn != waitStatus).ToList();
                if (!isIncludeQuitEmp)
                {
                    listProfile = listProfile.Where(pro => (pro.DateQuit == null || pro.DateQuit.Value > from) && pro.DateHire <= to).ToList();
                }
                return listProfile;
            }
        }
        public List<Hre_Profile> GetWorkingProfile(DateTime from, DateTime to)
        {
            List<Hre_Profile> listProfile = GetWorkingProfile(from, to, false);
            return listProfile;
        }
        private List<Hre_Profile> LoadProfileStatus(String status, DateTime From, DateTime To)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                List<Hre_Profile> lstProfile = GetWorkingProfile(From, To);
                if (status == StatusEmployee.E_WORKING.ToString())
                {
                    lstProfile = lstProfile.Where(pro => (pro.DateQuit == null || pro.DateQuit > To) && pro.DateHire < From).ToList();
                }
                else if (status == StatusEmployee.E_NEWEMPLOYEE.ToString())
                {
                    lstProfile = lstProfile.Where(pro => pro.DateHire < To && pro.DateHire > From).ToList();
                }
                else if (status == StatusEmployee.E_STOPWORKING.ToString())
                {
                    lstProfile = lstProfile.Where(pro => pro.DateQuit < To && pro.DateQuit > From).ToList();
                }
                else if (status == StatusEmployee.E_WORKINGANDNEW.ToString())
                {
                    lstProfile = lstProfile.Where(pro => pro.DateQuit == null || pro.DateQuit > To).ToList();
                }
                else
                {
                    return lstProfile;
                }
                return lstProfile;
            }
        }
        public static List<Guid> GetProfileIdList(List<Hre_Profile> lstProfile)
        {
            List<Guid> res = new List<Guid>();
            if (lstProfile == null)
                throw new Exception("List Profile Null");
            res = lstProfile.Select(p => p.ID).ToList();
            return res;
        }
        public static List<Att_Grade> GetAllGrade(List<Guid> listProfileId, DateTime monthYear)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                List<Att_Grade> lst = new List<Att_Grade>();
                var repoAtt_Grade = new Att_GradeRepository(unitOfWork);
                lst = repoAtt_Grade.FindBy(gd => listProfileId.Contains(gd.ProfileID.Value)
                    && gd.MonthStart <= monthYear).OrderByDescending(prop => prop.MonthStart).ToList();

                List<Att_Grade> lstGrade = new List<Att_Grade>();
                foreach (Att_Grade grade in lst)
                {
                    Att_Grade grade1 = lstGrade.FirstOrDefault(prop => prop.ProfileID == grade.ProfileID);
                    if (grade1 == null)
                    {
                        lstGrade.Add(grade);
                    }
                }
                return lstGrade;
            }
        }
        public static void GetRosterGroup(List<Guid> lstProfileID, DateTime? DateFrom, DateTime? DateTo,
            out List<Att_Roster> lstRosterTypeGroup, out List<Att_RosterGroup> lstRosterGroup)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                lstRosterGroup = new List<Att_RosterGroup>();
                lstRosterTypeGroup = new List<Att_Roster>();

                string E_APPROVED = RosterStatus.E_APPROVED.ToString();
                string E_ROSTERGROUP = RosterType.E_ROSTERGROUP.ToString();
                var repoAtt_Roster = new Att_RosterRepository(unitOfWork);
                var repoAtt_RosterGroup = new Att_RosterGroupRepository(unitOfWork);
                if (DateFrom == null || DateTo == null)
                {
                    lstRosterTypeGroup = repoAtt_Roster.FindBy(m => lstProfileID.Contains(m.ProfileID) && m.Status == E_APPROVED && m.Type == E_ROSTERGROUP).ToList<Att_Roster>();
                    lstRosterGroup = repoAtt_RosterGroup.FindBy(m => m.DateStart != null && m.DateEnd != null).ToList<Att_RosterGroup>();
                }
                else
                {
                    lstRosterTypeGroup = repoAtt_Roster.FindBy(m => lstProfileID.Contains(m.ProfileID) && m.Status == E_APPROVED && m.Type == E_ROSTERGROUP && m.DateStart <= DateTo).ToList<Att_Roster>();
                    lstRosterGroup = repoAtt_RosterGroup.FindBy(m => m.DateStart != null && m.DateEnd != null && m.DateStart <= DateTo && m.DateEnd >= DateFrom).ToList<Att_RosterGroup>();
                }
            }
        }
        /// <summary>
        /// LamLe - 20120803 - Lay ca lam viec dua vao phong ban.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="lstHistory"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static Hashtable GetRosterOrgTable(Hre_Profile profile
                                , List<Hre_WorkHistory> lstHistory
                                , DateTime from, DateTime to)
        {
            Hashtable res = new Hashtable();
            for (DateTime idx = from; idx <= to; idx = idx.AddDays(1))
            {
                List<Hre_WorkHistory> listWorkdate = lstHistory.Where(wh => wh.DateEffective <= idx).OrderByDescending(wh => wh.DateEffective).ToList();
                Cat_OrgStructure org = null;
                if (listWorkdate != null && listWorkdate.Count > 0)
                {
                    Hre_WorkHistory whDate = listWorkdate[0];
                    org = whDate.Cat_OrgStructure;
                }
                else if (profile.Cat_OrgStructure != null)
                {
                    org = profile.Cat_OrgStructure;
                }
                if (org == null)
                    continue;

                ArrayList arr = new ArrayList();
                if (idx.DayOfWeek == DayOfWeek.Monday)
                    arr.Add(org.Cat_Shift);

                else if (idx.DayOfWeek == DayOfWeek.Tuesday)
                    arr.Add(org.Cat_Shift1);

                else if (idx.DayOfWeek == DayOfWeek.Wednesday)
                    arr.Add(org.Cat_Shift2);

                else if (idx.DayOfWeek == DayOfWeek.Thursday)
                    arr.Add(org.Cat_Shift3);

                else if (idx.DayOfWeek == DayOfWeek.Friday)
                    arr.Add(org.Cat_Shift4);

                else if (idx.DayOfWeek == DayOfWeek.Saturday)
                    arr.Add(org.Cat_Shift5);

                else if (idx.DayOfWeek == DayOfWeek.Sunday)
                    arr.Add(org.Cat_Shift6);

                if (!res.ContainsKey(idx))
                    res.Add(idx, arr);
            }
            return res;
        }
        /// <summary>
        /// Lay table roster Key: Date, Value : Shift
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="rosterCfgPro"></param>
        /// <param name="lstHistoryPro"></param>
        /// <param name="gradeCfg"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="lstRosterGroup">Ds RosterGroup</param>
        /// <param name="lstRosterTypeGroup">Ds Roster loai E_ROSTERGROUP</param>
        /// <returns></returns>
        public static Hashtable GetRosterTable(Hre_Profile profile
                                , List<Att_Roster> rosterCfgPro
                                , List<Hre_WorkHistory> lstHistoryPro
                                , Cat_GradeAttendance gradeCfg
                                , DateTime from, DateTime to, List<Att_RosterGroup> lstRosterGroup, List<Att_Roster> lstRosterTypeGroup)
        {

            rosterCfgPro = rosterCfgPro.Where(s => s.ProfileID == profile.ID).ToList();
            Hashtable res = new Hashtable();
            from = from.Date;
            to = to.Date;
            //LamLe : 20120809 - #0014556 - Chi lay nhung roster o trang thai Approved
            String statusApproved = RosterStatus.E_APPROVED.ToString();
            List<Att_Roster> rosterApproved = rosterCfgPro.Where(ros => ros.Status == statusApproved || String.IsNullOrEmpty(ros.Status)).ToList();
            List<Att_Roster> lstRosterTypeGroup_ByProfile = lstRosterTypeGroup.Where(m => m.ProfileID == profile.ID).ToList();


            //LamLe - 20120803 - Lay ca lam viec trong phong ban.
            if (lstHistoryPro != null && gradeCfg != null && gradeCfg.RosterType == GradeRosterType.E_ISROSTER_ORG.ToString())
            {
                res = GetRosterOrgTable(profile, lstHistoryPro, from, to);
            }
            if (rosterApproved != null) //LamLe - 20120803 - Lay ca lam viec dua vao roster
            {
                foreach (Att_Roster roster in rosterApproved)
                {
                    try
                    {
                        if (RosterType.E_DEFAULT.ToString() != roster.Type)
                            continue;

                        DateTime start = from;
                        if (roster.DateStart != null && roster.DateStart > start)
                            start = roster.DateStart;

                        DateTime end = to;
                        if (roster.DateEnd != null && roster.DateEnd < end)
                            end = roster.DateEnd;

                        for (DateTime idx = start; idx <= end; idx = idx.AddDays(1))
                        {
                            ArrayList arr = new ArrayList();
                            if (idx.DayOfWeek == DayOfWeek.Monday)
                                arr.Add(roster.Cat_Shift);

                            else if (idx.DayOfWeek == DayOfWeek.Tuesday)
                                arr.Add(roster.Cat_Shift1);

                            else if (idx.DayOfWeek == DayOfWeek.Wednesday)
                                arr.Add(roster.Cat_Shift2);

                            else if (idx.DayOfWeek == DayOfWeek.Thursday)
                                arr.Add(roster.Cat_Shift3);

                            else if (idx.DayOfWeek == DayOfWeek.Friday)
                                arr.Add(roster.Cat_Shift4);

                            else if (idx.DayOfWeek == DayOfWeek.Saturday)
                                arr.Add(roster.Cat_Shift5);

                            else if (idx.DayOfWeek == DayOfWeek.Sunday)
                                arr.Add(roster.Cat_Shift6);
                            if (!res.ContainsKey(idx))
                                res.Add(idx, arr);

                        }
                    }
                    catch (System.Exception e)
                    {

                    }
                }

                #region triet.mai Loai E_ROSTERGROUP loại Đăng ký tăng ca theo nhóm
                //Logic khá phức tạp 
                //1. Độ ưu tiên thì đứng sau Att_Roster (loại khác Vd: dateOff, ChangeShift)
                //2. tìm cái roster của từng ngày và kiêm tra cai tên của RosterGroupName >> Chạy qua bảng Att_RosterGroup để tìm Shift

                string E_ROSTERGROUP = RosterType.E_ROSTERGROUP.ToString();
                List<Att_Roster> lstRoster_Type_RosterGroup = lstRosterTypeGroup_ByProfile.Where(m => m.Type == E_ROSTERGROUP).OrderByDescending(m => m.DateStart).ToList();
                bool isContinue = true; //Dung de chay nguoc cac roster lay cai moi nhat va ko chay nua => tang toc;
                foreach (Att_Roster rosterGroup in lstRoster_Type_RosterGroup)
                {
                    if (isContinue == false)
                        continue;

                    if (rosterGroup.DateStart <= from)
                    {
                        isContinue = false;
                    }

                    DateTime start = from;
                    DateTime end = to;
                    RosterType type = (RosterType)Common.GetEnumValue(typeof(RosterType), rosterGroup.Type);
                    switch (type)
                    {
                        case RosterType.E_ROSTERGROUP:
                            for (DateTime idx = start; idx <= end; idx = idx.AddDays(1))
                            {
                                bool isExist = res.ContainsKey(idx);
                                ArrayList arr = new ArrayList();
                                if (isExist)
                                    res.Remove(idx);

                                if (string.IsNullOrEmpty(rosterGroup.RosterGroupName))
                                    continue;
                                Att_RosterGroup RosterGroup_Current = lstRosterGroup.Where(m => m.RosterGroupName == rosterGroup.RosterGroupName && m.DateStart <= idx && m.DateEnd >= idx).FirstOrDefault();
                                if (RosterGroup_Current == null)
                                    continue;

                                if (idx.DayOfWeek == DayOfWeek.Monday && RosterGroup_Current.Cat_Shift != null)
                                    arr.Add(RosterGroup_Current.Cat_Shift);

                                else if (idx.DayOfWeek == DayOfWeek.Tuesday && RosterGroup_Current.Cat_Shift1 != null)
                                    arr.Add(RosterGroup_Current.Cat_Shift1);

                                else if (idx.DayOfWeek == DayOfWeek.Wednesday && RosterGroup_Current.Cat_Shift2 != null)
                                    arr.Add(RosterGroup_Current.Cat_Shift2);

                                else if (idx.DayOfWeek == DayOfWeek.Thursday && RosterGroup_Current.Cat_Shift3 != null)
                                    arr.Add(RosterGroup_Current.Cat_Shift3);

                                else if (idx.DayOfWeek == DayOfWeek.Friday && RosterGroup_Current.Cat_Shift4 != null)
                                    arr.Add(RosterGroup_Current.Cat_Shift4);

                                else if (idx.DayOfWeek == DayOfWeek.Saturday && RosterGroup_Current.Cat_Shift5 != null)
                                    arr.Add(RosterGroup_Current.Cat_Shift5);

                                else if (idx.DayOfWeek == DayOfWeek.Sunday && RosterGroup_Current.Cat_Shift6 != null)
                                    arr.Add(RosterGroup_Current.Cat_Shift6);

                                if (!res.ContainsKey(idx))
                                    res.Add(idx, arr);
                            }
                            break;

                        default:
                            break;
                    }
                }
                #endregion



                //LamLe - 20121101 - Order theo loai de E_TIME_OFF uu tien sau cung
                rosterApproved = rosterApproved.OrderBy(rs => rs.Type).ToList();

                string E_TIME_OFF = RosterType.E_TIME_OFF.ToString();
                string E_CHANGE_SHIFT = RosterType.E_CHANGE_SHIFT.ToString();
                List<Att_Roster> lstRoster_Type_TimeOff_ChangeShift = rosterApproved.Where(m => m.Type == E_TIME_OFF || m.Type == E_CHANGE_SHIFT).ToList();
                foreach (Att_Roster roster in lstRoster_Type_TimeOff_ChangeShift)
                {
                    DateTime start = from;
                    if (roster.DateStart != null)
                        start = roster.DateStart;

                    DateTime end = to;
                    if (roster.DateEnd != null)
                        end = roster.DateEnd;

                    RosterType type = (RosterType)Common.GetEnumValue(typeof(RosterType), roster.Type);
                    switch (type)
                    {
                        case RosterType.E_TIME_OFF:
                            for (DateTime idx = start; idx <= end; idx = idx.AddDays(1))
                            {
                                if (res.ContainsKey(idx))
                                    res.Remove(idx);
                            }
                            break;

                        case RosterType.E_CHANGE_SHIFT:
                            for (DateTime idx = start; idx <= end; idx = idx.AddDays(1))
                            {
                                bool isExist = res.ContainsKey(idx);
                                ArrayList arr = new ArrayList();

                                if (isExist)
                                    res.Remove(idx);
                                arr.Add(roster.Cat_Shift);

                                res.Add(idx, arr);
                            }
                            break;


                        //case RosterType.E_UNUSUAL:
                        //    for (DateTime idx = start; idx <= end; idx = idx.AddDays(1))
                        //    {
                        //        bool isExist = res.ContainsKey(idx);
                        //        if (!isExist)
                        //            res.Add(idx, new ArrayList());

                        //        ((ArrayList)res[idx]).Add(roster.Cat_Shift);
                        //    }
                        //    break;
                        default:
                            break;
                    }
                }
            }
            return res;
        }
        public List<Att_ReportShiftAdjustmentEntity> GetReportShiftAdjustmentList(DateTime dateStart, DateTime dateEnd, List<Guid?> lstOrgIds)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                //List<Hre_Profile> listProfile = GetWorkingProfile(dateStart, dateEnd);
                //listProfile = listProfile.Where(s => s.OrgStructureID != null && lstOrgIds.Contains(s.OrgStructureID.Value)).ToList();
                //List<int> lstProfileID = GetProfileIdList(listProfile);
                //List<Att_Grade> lstGrade = GetAllGrade(lstProfileID, dateEnd);
                //var repoCat_ShiftItem = new Cat_ShiftItemRepository(unitOfWork);
                //List<Cat_ShiftItem> lstShiftItem = repoCat_ShiftItem.GetAll().ToList();
                //var repoCat_DayOff = new Cat_DayOffRepository(unitOfWork);
                //List<Cat_DayOff> lstDayoff = repoCat_DayOff.GetAll().ToList();
                //DateTime dtStartTemp = dateStart.AddDays(-7);
                //listProfile = listProfile.OrderBy(p => p.Cat_OrgStructure.OrgStructureName).ThenBy(p => p.ProfileName).ToList();
                //var repoAtt_WorkDay = new Att_WorkDayRepository(unitOfWork);
                //List<Att_Workday> listInOutInMonthtemp = repoAtt_WorkDay.FindBy(ls => ls.WorkDate >= dtStartTemp && ls.WorkDate <= dateEnd
                //    && lstProfileID.Contains(ls.ProfileID)).ToList();
                //List<Att_Workday> listInOutInMonth = listInOutInMonthtemp.Where(ls => ls.WorkDate >= dateStart && ls.WorkDate <= dateEnd).ToList();
                //String typePre = PregnancyStatus.E_LEAVE_EARLY.ToString();
                //var repoAtt_Pregnancy = new Att_PregnancyRepository(unitOfWork);
                //List<Att_Pregnancy> lstPregnacyAll = repoAtt_Pregnancy.FindBy(prop => prop.DateStart <= dateEnd
                //                               && prop.DateEnd >= dateStart && prop.Type == typePre).ToList();
                //var repoHre_WorkHistory = new Hre_WorkHistoryRepository(unitOfWork);
                //List<Hre_WorkHistory> whProfile = repoHre_WorkHistory.GetAll().ToList();
                //var repoAtt_Roster = new Att_RosterRepository(unitOfWork);
                //List<Att_Roster> lstRoster = repoAtt_Roster.FindBy(prop => prop.DateStart <= dateEnd && prop.DateEnd >= dateStart).ToList();
                //List<int> lstProfileIDs = listProfile.Select(m => m.ID).ToList();
                //List<Att_Roster> lstRosterTypeGroup = new List<Att_Roster>();
                //List<Att_RosterGroup> lstRosterGroup = new List<Att_RosterGroup>();
                //GetRosterGroup(lstProfileIDs, dateStart, dateEnd, out lstRosterTypeGroup, out lstRosterGroup);
                //foreach (Hre_Profile profile in listProfile)
                //{

                //    Att_Grade grade = lstGrade.Where(gd => gd.ProfileID == profile.Id).FirstOrDefault();
                //    if (grade == null) continue;
                //    Cat_Grade gradeCfg = grade != null ? grade.Cat_Grade : null;
                //    List<Att_Workday> listInOut = listInOutInMonth.Where(ls => ls.ProfileID == profile.Id).ToList();
                //    List<Att_Roster> lstProfileRoster = lstRoster.Where(prop => prop.ProfileID == profile.Id).ToList();
                //    //List<Att_LeaveDay> listLeaveDay = listLeaveDayInMonth.Where(ls => ls.ProfileID == profile.ID).ToList();

                //    Hashtable rosterTable = GetRosterTable(profile, lstProfileRoster, whProfile, gradeCfg, dateStart.Date, dateEnd.Date, lstRosterGroup, lstRosterTypeGroup);
                //    List<Att_Pregnancy> lstPregnacyPro = lstPregnacyAll.Where(p => p.ProfileID == profile.Id).ToList();
                //    List<WorkDay> lstWorkDay = WorkDayHelper.ComputeWorkDays(profile, listInOut, lstShiftItem, lstProfileRoster, lstPregnacyPro, grade.Cat_Grade);

                //    DateTime dtStart = profile.DateHire == null ? dateStart : (profile.DateHire.Value > dateStart ? profile.DateHire.Value : dateStart);


                //    for (DateTime dx = dtStart; dx <= dateEnd; dx = dx.AddDays(1))
                //    {
                //        bool isWorkDate = AttendanceService.IsWorkDay(grade.Cat_GradeAttendance, rosterTable, lstDayoff, dx);
                //        if (lstDayoff.Exists(ls => ls.DateOff == dx))
                //        {
                //            isWorkDate = false;
                //        }
                //        if (isWorkDate)
                //        {

                //            //if (lstInOut.Count() == 0 && lstlv.Count() == 0)
                //            //{
                //            //    LeaveUnpaid += 1;
                //            //    LeaveNotWork += 1;
                //            //    if (dx.Day >= 21) LeaveUnpaid21To1 += 1;
                //            //    else LeaveUnpaid1To20 += 1;
                //            //}
                //            //double realworkinghours = 0;
                //            double hoursOnWorkDate = grade.Cat_GradeAttendance.HourOnWorkDate.Value;
                //            //Double WorkHours = 0;
                //            //Double leaveHour = 0;
                //            //Double LateEarlyHours = 0;
                //            Double LateHours = 0;
                //            Double EarlyHours = 0;
                //            WorkDay objWorkday = lstWorkDay.Where(wd => wd.WorkDate == dx).FirstOrDefault();

                //            bool attendanceMethod_E_FULL = false;
                //            if (grade.Cat_GradeAttendance.AttendanceMethod == AttendanceMethod.E_FULL.ToString())
                //                attendanceMethod_E_FULL = true;
                //            if (attendanceMethod_E_FULL
                //            && dx >= profile.DateHire.Value
                //            && (!profile.DateQuit.HasValue || profile.DateQuit.Value > dx))
                //            {
                //                WorkDay wd = new WorkDay();
                //                wd.WorkDuration = hoursOnWorkDate;
                //                wd.WorkDate = dx;
                //                wd.LateEarlyDuration = 0;
                //                objWorkday = wd;
                //            }
                //            if (objWorkday != null)
                //            {
                //                //WorkHours = objWorkday.WorkDuration;
                //                //double remainHrs = Math.Round(hoursOnWorkDate - (WorkHours + leaveHour), 2);//
                //                //remainHrs = remainHrs < 0 ? 0 : remainHrs;
                //                //remainHrs = remainHrs > hoursOnWorkDate ? hoursOnWorkDate : remainHrs;
                //                EarlyHours = objWorkday.EarlyDuration;
                //                LateHours = objWorkday.LateDuration;
                //                //if (grade.Cat_GradeAttendance.IsLateEarlyRounding == true)
                //                //{
                //                //    List<Cat_LateEarlyRule> lstRule = grade.Cat_GradeAttendance.Cat_LateEarlyRule.Where(rl => rl.IsDelete != true).ToList();
                //                //    EarlyHours = AttendanceService.RoundLateEarlyMinutes(lstRule, EarlyHours);
                //                //    LateHours = AttendanceService.RoundLateEarlyMinutes(lstRule, LateHours);
                //                //    //remainHrs = AttendanceService.RoundLateEarlyMinutes(lstRule, remainHrs);
                //                //}
                //                //LateEarlyHours = LateHours + EarlyHours;

                //                //WorkHours = Math.Round(hoursOnWorkDate - (leaveHour + remainHrs), 2);
                //                //if (grade.Cat_GradeAttendance.IsDeductInLateOutEarly == false)
                //                //    WorkHours = hoursOnWorkDate;

                //                //List<Att_LeaveDay> lstlv = listLeaveDay.Where(ld => ld.dateStart.Date <= dx && ld.dateEnd.Date >= dx).ToList();
                //                //foreach (Att_LeaveDay lv in lstlv)
                //                //{
                //                //    lv.t
                //                //}

                //                if ((LateHours + EarlyHours) > iMinLate)//|| EarlyHours > MinEarly
                //                {
                //                    DataRow dr = tb.NewRow();
                //                    dr[colDate] = dx;
                //                    dr[Att_WorkDay.FieldNames.udDepartment] = profile.Cat_OrgStructure.OrgStructureName;
                //                    dr[Att_WorkDay.FieldNames.udCodeEmp] = profile.CodeEmp;
                //                    dr[Hre_Profile.FieldNames.ProfileName] = profile.ProfileName;
                //                    dr[Cat_Shift.FieldNames.ShiftName] = objWorkday.Cat_Shift.ShiftName;
                //                    if (objWorkday.FirstInTime != null)
                //                        dr[Att_WorkDay.FieldNames.udTimeIn] = objWorkday.FirstInTime;
                //                    if (objWorkday.FirstOutTime != null)
                //                        dr[Att_WorkDay.FieldNames.udTimeOut] = objWorkday.FirstOutTime;
                //                    dr["LATE_EARLY_MIN"] = LateHours + EarlyHours;
                //                    //dr["EARLY_MIN"] = EarlyHours;

                //                    dr["ProfileID"] = profile.ID;
                //                    dr["ShiftID"] = objWorkday.Cat_Shift.ID;

                //                    tb.Rows.Add(dr);
                //                }
                //            }
                //        }
                //    }
                //}

                ////DataTable tb = //Common.ConvertIListToDataTable(listInOut, ListControl1.GridColumnNames);
                //ListControl1.DataSource = tb;
                //return tb;
                List<Att_ReportShiftAdjustmentEntity> lstReportShiftAdjustmentEntity = new List<Att_ReportShiftAdjustmentEntity>();
                return lstReportShiftAdjustmentEntity;
            }
        }

        /// <summary>
        /// Lấy Dữ Liệu BC Chi Tiết Nghỉ Hàng Tháng của nhân viên
        /// </summary>
        /// <returns></returns>

        DataTable CreateReportLeaveMonthSchema(Guid[] leaveDayTypeIDs, DateTime? _dateStart, DateTime? _dateend)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                string status = string.Empty;
                DataTable tb = new DataTable("Att_ReportLeaveMonthEntity");

                var leaveDayTypeService = new Cat_LeaveDayTypeServices();
                var lstObjLeaveDayType = new List<object>();
                lstObjLeaveDayType.Add(null);
                lstObjLeaveDayType.Add(null);
                lstObjLeaveDayType.Add(1);
                lstObjLeaveDayType.Add(100000000);

                tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.ProfileName);
                tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.PositionName);
                tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.JobTitleName);
                //tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.DateQuit);
                //tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.LeaveDayTypeName);
                tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.Paid);
                //var lstLeavedayType = leaveDayTypeService.GetData<Cat_LeaveDayTypeEntity>(lstObjLeaveDayType, ConstantSql.hrm_cat_sp_get_LeaveDayType, ref status).ToList();
                var lstLeavedayType = unitOfWork.CreateQueryable<Cat_LeaveDayType>(s => s.Code != null).ToList();
                if (_dateStart != null && _dateend != null)
                {
                    for (DateTime date = _dateStart.Value; date <= _dateend.Value; date = date.AddDays(1))
                    {
                        if (!tb.Columns.Contains("Day" + date.Day.ToString()))
                        {
                            tb.Columns.Add("Day" + date.Day.ToString(), typeof(double));
                        }
                    }
                }
                if (leaveDayTypeIDs != null && leaveDayTypeIDs[0] != null)
                {
                    lstLeavedayType = lstLeavedayType.Where(s => leaveDayTypeIDs.Contains(s.ID)).ToList();
                }

                foreach (var item in lstLeavedayType)
                {
                    if (!string.IsNullOrEmpty(item.Code.Trim()) && !tb.Columns.Contains(item.Code.Trim()))
                    {
                        tb.Columns.Add(item.Code.Trim());
                    }
                }
                tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.E_COMPANY);
                tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.E_BRANCH);
                tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.E_UNIT);
                tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.E_DIVISION);
                tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.E_DEPARTMENT);
                tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.E_TEAM);
                tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.E_SECTION);


                tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.DateFrom, typeof(DateTime));
                tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.DateTo, typeof(DateTime));
                tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.UserExport);
                tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.DateExport, typeof(DateTime));
                return tb;
            }
        }




        DataTable CreateReportDetailedMonthlyStaySchema(Guid[] leaveDayTypeIDs, DateTime? _dateStart, DateTime? _dateend)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                string status = string.Empty;
                DataTable tb = new DataTable("Att_ReportDetailedMonthlyStayEntity");

                var leaveDayTypeService = new Cat_LeaveDayTypeServices();
                var lstObjLeaveDayType = new List<object>();
                lstObjLeaveDayType.Add(null);
                lstObjLeaveDayType.Add(null);
                lstObjLeaveDayType.Add(1);
                lstObjLeaveDayType.Add(100000000);

                tb.Columns.Add(Att_ReportDetailedMonthlyStayEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Att_ReportDetailedMonthlyStayEntity.FieldNames.ProfileName);
                tb.Columns.Add(Att_ReportDetailedMonthlyStayEntity.FieldNames.PositionName);
                tb.Columns.Add(Att_ReportDetailedMonthlyStayEntity.FieldNames.JobTitleName);
                //tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.DateQuit);
                //tb.Columns.Add(Att_ReportLeaveMonthEntity.FieldNames.LeaveDayTypeName);
                tb.Columns.Add(Att_ReportDetailedMonthlyStayEntity.FieldNames.Paid);
                //var lstLeavedayType = leaveDayTypeService.GetData<Cat_LeaveDayTypeEntity>(lstObjLeaveDayType, ConstantSql.hrm_cat_sp_get_LeaveDayType, ref status).ToList();
                var lstLeavedayType = unitOfWork.CreateQueryable<Cat_LeaveDayType>(s => s.Code != null).ToList();
                if (_dateStart != null && _dateend != null)
                {
                    for (DateTime date = _dateStart.Value; date <= _dateend.Value; date = date.AddDays(1))
                    {
                        if (!tb.Columns.Contains("Day" + date.Day.ToString()))
                        {
                            tb.Columns.Add("Day" + date.Day.ToString(), typeof(double));
                        }
                    }
                }
                if (leaveDayTypeIDs != null && leaveDayTypeIDs[0] != null)
                {
                    lstLeavedayType = lstLeavedayType.Where(s => leaveDayTypeIDs.Contains(s.ID)).ToList();
                }

                foreach (var item in lstLeavedayType)
                {
                    if (!string.IsNullOrEmpty(item.Code.Trim()) && !tb.Columns.Contains(item.Code.Trim()))
                    {
                        tb.Columns.Add(item.Code.Trim());
                    }
                }
                tb.Columns.Add(Att_ReportDetailedMonthlyStayEntity.FieldNames.E_COMPANY);
                tb.Columns.Add(Att_ReportDetailedMonthlyStayEntity.FieldNames.E_BRANCH);
                tb.Columns.Add(Att_ReportDetailedMonthlyStayEntity.FieldNames.E_UNIT);
                tb.Columns.Add(Att_ReportDetailedMonthlyStayEntity.FieldNames.E_DIVISION);
                tb.Columns.Add(Att_ReportDetailedMonthlyStayEntity.FieldNames.E_DEPARTMENT);
                tb.Columns.Add(Att_ReportDetailedMonthlyStayEntity.FieldNames.E_TEAM);
                tb.Columns.Add(Att_ReportDetailedMonthlyStayEntity.FieldNames.E_SECTION);


                tb.Columns.Add(Att_ReportDetailedMonthlyStayEntity.FieldNames.DateFrom, typeof(DateTime));
                tb.Columns.Add(Att_ReportDetailedMonthlyStayEntity.FieldNames.DateTo, typeof(DateTime));
                tb.Columns.Add(Att_ReportDetailedMonthlyStayEntity.FieldNames.UserExport);
                tb.Columns.Add(Att_ReportDetailedMonthlyStayEntity.FieldNames.DateExport, typeof(DateTime));
                return tb;
            }
        }

        /// <summary>
        /// HieuVan
        /// BC Thống Kế Ngày Nghỉ Hàng Tháng
        /// Bản cập nhật bản 7 Oracle ngày 21/01/2015
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="profiles"></param>
        /// <param name="orgStructureID"></param>
        /// <param name="isIncludeQuitEmp"></param>
        /// <param name="leaveDayTypeIDs"></param>
        /// <param name="codeEmp"></param>
        /// <param name="isNotAllowZero"></param>
        /// <param name="userExport"></param>
        /// <returns></returns>
        public DataTable GetReportLeaveMonth(DateTime dateStart, DateTime dateEnd, List<Hre_ProfileEntity> profiles, string orgStructureID, bool isIncludeQuitEmp, Guid[] leaveDayTypeIDs, string codeEmp, bool isNotAllowZero, string userExport, bool IsCreateTemplate, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var baseservice = new BaseService();
                string key = OverTimeStatus.E_APPROVED.ToString();
                DataTable table = CreateReportLeaveMonthSchema(leaveDayTypeIDs, dateStart, dateEnd);
                if (IsCreateTemplate)
                {
                    return table.ConfigTable();
                }
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Leaveday = new Att_LeavedayRepository(unitOfWork);
                var repoAtt_Roster = new Att_RosterRepository(unitOfWork);
                var repoCat_LeaveDayTypeRepository = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                var repoCat_DayOff = new Cat_DayOffRepository(unitOfWork);
                var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                var repoCat_GradeAtt = new Cat_GradeAttendanceRepository(unitOfWork);
                var repoHre_WorkHistory = new Hre_WorkHistoryRepository(unitOfWork);
                var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);


                List<Guid> lstProfileIDs = profiles.Select(s => s.ID).ToList();
                List<Att_LeaveDayEntity> leaveDays = new List<Att_LeaveDayEntity>();
                List<Cat_LeaveDayType> leavedaytypes = new List<Cat_LeaveDayType>();
                string status = string.Empty;
                List<object> para = new List<object>();
                para.AddRange(new object[3]);
                para[0] = (object)orgStructureID;
                para[1] = dateStart;
                para[2] = dateEnd;
                if (lstProfileIDs != null && lstProfileIDs.Count > 0)
                {
                    leaveDays = baseservice.GetData<Att_LeaveDayEntity>(para, ConstantSql.hrm_att_getdata_LeaveDay, UserLogin, ref status).Where(s => s.Status == key).ToList();
                }
                else
                {
                    return table.ConfigTable();

                }

                //[Tho.Bui] Get LeavedayR=type
                if (leaveDays != null && leaveDays.Count > 0)
                {
                    List<Guid> lstLDTinLD = leaveDays.Select(s => s.LeaveDayTypeID).Distinct().ToList();
                    leavedaytypes = repoCat_LeaveDayTypeRepository.FindBy(s => s.IsDelete == null && lstLDTinLD.Contains(s.ID)).ToList();
                    List<Guid> leavedayguid = leaveDays.Select(s => s.LeaveDayTypeID).ToList();
                    leavedaytypes = leavedaytypes.Where(s => leavedayguid.Contains(s.ID)).ToList();
                }
                var profileIds = leaveDays.Select(s => s.ProfileID).Distinct().ToList();

                var rosterStatus = RosterStatus.E_APPROVED.ToString();
                var rosters = unitOfWork.CreateQueryable<Att_Roster>(Guid.Empty, d => d.Status == rosterStatus && d.DateStart <= dateEnd && d.DateEnd >= dateStart).ToList().Translate<Att_RosterEntity>();


                var repoCat_LeaveDayType = new Cat_LeaveDayTypeRepository(unitOfWork);
                var leavedayTypes = repoCat_LeaveDayType.FindBy(s => s.Code != null && s.IsDelete == null).Select(s => new { s.Code, s.ID, s.PaidRate, s.LeaveDayTypeName }).ToList();
                var leavdayPadidIDs = leavedayTypes.Where(s => s.PaidRate > 0).Select(s => s.ID).ToList();
                List<Guid> guids = profiles.Select(s => s.ID).ToList();
                leaveDays = leaveDays.Where(s => guids.Contains(s.ProfileID)).ToList();

                if (leaveDayTypeIDs != null)
                {
                    leaveDays = leaveDays.Where(s => leaveDayTypeIDs.Contains(s.LeaveDayTypeID)).ToList();
                    profileIds = leaveDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }
                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                }
                //var shifts = repoCat_Shift.FindBy(s => s.IsDelete == null).ToList();
                var shifts = unitOfWork.CreateQueryable<Cat_Shift>(Guid.Empty).ToList().Translate<Cat_ShiftEntity>();

                List<string> codeRejects = new List<string>();
                codeRejects.Add("SICK");
                codeRejects.Add("SU");
                codeRejects.Add("SD");
                codeRejects.Add("D");
                codeRejects.Add("DP");
                codeRejects.Add("PSN");
                codeRejects.Add("DSP");
                codeRejects.Add("M");
                List<Guid> leaveDayTypeRejectIDs = leavedayTypes.Where(s => codeRejects.Contains(s.Code)).Select(s => s.ID).ToList();

                if (isNotAllowZero)
                {
                    var profileIDs = leaveDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIDs.Contains(s.ID)).ToList();
                }
                #region sai xong xóa
                string CodeEmpTest = string.Empty;
                if (CodeEmpTest != string.Empty)
                {
                    char[] ext = new char[] { ';', ',' };
                    List<string> lstCodeEmpTest = CodeEmpTest.Split(ext, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                    profiles = profiles.Where(m => lstCodeEmpTest.Contains(m.CodeEmp)).ToList();
                }
                #endregion

                var leveDayIDs = leaveDays.Select(s => s.LeaveDayTypeID).ToList();
                leavedayTypes = leavedayTypes.Where(s => leveDayIDs.Contains(s.ID)).ToList();
                var holidayDates = repoCat_DayOff.FindBy(hol => hol.DateOff >= dateStart && hol.DateOff <= dateEnd).Select(s => s.DateOff).ToList();
                holidayDates = holidayDates.Select(s => s.Date).ToList();

                var lstRosterGroup = unitOfWork.CreateQueryable<Att_RosterGroup>(Guid.Empty, s => s.DateStart != null && s.DateStart >= dateStart && s.DateEnd != null && s.DateEnd <= dateEnd).ToList().Translate<Att_RosterGroupEntity>();

                foreach (var profile in profiles)
                {
                    var rosterProfiles = rosters.Where(s => s.ProfileID == profile.ID).ToList();
                    List<DateTime> workDayDates = new List<DateTime>();
                    foreach (var roster in rosterProfiles)
                    {
                        for (DateTime date = roster.DateStart; date <= roster.DateEnd; date = date.AddDays(1))
                        {
                            workDayDates.Add(date.Date);
                        }
                    }
                    workDayDates = workDayDates.Where(s => !holidayDates.Contains(s.Date)).ToList();

                    DataRow row = table.NewRow();
                    var leaveDay = leaveDays.FirstOrDefault(s => s.ProfileID == profile.ID);
                    var LeaveDayType = leavedaytypes.FirstOrDefault(s => s.ID != Guid.Empty && leaveDay != null && s.ID == leaveDay.LeaveDayTypeID);

                    row[Att_ReportLeaveMonthEntity.FieldNames.ProfileName] = profile.ProfileName;
                    row[Att_ReportLeaveMonthEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    if (profile.E_COMPANY != null)
                        row[Att_ReportLeaveMonthEntity.FieldNames.E_COMPANY] = profile.E_COMPANY;
                    if (profile.E_BRANCH != null)
                        row[Att_ReportLeaveMonthEntity.FieldNames.E_BRANCH] = profile.E_BRANCH;
                    if (profile.E_UNIT != null)
                        row[Att_ReportLeaveMonthEntity.FieldNames.E_UNIT] = profile.E_UNIT;
                    if (profile.E_DIVISION != null)
                        row[Att_ReportLeaveMonthEntity.FieldNames.E_DIVISION] = profile.E_DIVISION;
                    if (profile.E_DEPARTMENT != null)
                        row[Att_ReportLeaveMonthEntity.FieldNames.E_DEPARTMENT] = profile.E_DEPARTMENT;
                    if (profile.E_TEAM != null)
                        row[Att_ReportLeaveMonthEntity.FieldNames.E_TEAM] = profile.E_TEAM;
                    if (profile.E_SECTION != null)
                        row[Att_ReportLeaveMonthEntity.FieldNames.E_SECTION] = profile.E_SECTION;

                    if (profile.JobTitleName != null)
                        row[Att_ReportLeaveMonthEntity.FieldNames.JobTitleName] = profile.JobTitleName;
                    if (profile.PositionName != null)
                        row[Att_ReportLeaveMonthEntity.FieldNames.PositionName] = profile.PositionName;

                    row[Att_ReportLeaveMonthEntity.FieldNames.DateFrom] = dateStart;
                    row[Att_ReportLeaveMonthEntity.FieldNames.DateTo] = dateEnd;
                    row[Att_ReportLeaveMonthEntity.FieldNames.UserExport] = userExport;
                    row[Att_ReportLeaveMonthEntity.FieldNames.DateExport] = DateTime.Today;
                    var leadayProfiles = leaveDays.Where(s => s.ProfileID == profile.ID).ToList();
                    var ledayeTyepIDs = leadayProfiles.Select(s => s.LeaveDayTypeID).ToList();
                    double sumPaid = 0;
                    foreach (var leadayType in leavedayTypes.Where(s => ledayeTyepIDs.Contains(s.ID)).ToList())
                    {
                        double sum = 0;
                        var leavdayProfiles = leadayProfiles.Where(s => s.LeaveDayTypeID == leadayType.ID).ToList();
                        foreach (var leavdayProfile in leavdayProfiles)
                        {
                            var DateEnd = leavdayProfile.DateEnd;
                            if (DateEnd > dateEnd)
                            {
                                DateEnd = dateEnd;
                            }
                            for (DateTime date = leavdayProfile.DateStart; date <= DateEnd; date = date.AddDays(1))
                            {
                                bool isWorkDay = workDayDates.Contains(date);
                                if (!isWorkDay)// neu ngay do ko phai ngay di lam
                                {
                                    if (leaveDayTypeRejectIDs.Contains(leavdayProfile.LeaveDayTypeID))// neu ngay do la ngay nghi dc xem la ko xem ca or ngay nghi
                                    {
                                        isWorkDay = true;
                                    }
                                }
                                if (isWorkDay)
                                {
                                    if (leavdayProfile.Duration > 0)
                                    {
                                        //sum++; 
                                        sum += leavdayProfile.Duration;
                                    }
                                    if (leavdayPadidIDs.Contains(leavdayProfile.LeaveDayTypeID))
                                    {
                                        sumPaid += leadayType.PaidRate;
                                        //sumPaid++;
                                    }
                                }
                            }
                        }
                        row[leadayType.Code] = sum > 0 ? (object)sum : DBNull.Value;
                        row[Att_ReportLeaveMonthEntity.FieldNames.Paid] = sumPaid > 0 ? (object)sumPaid : DBNull.Value;
                    }
                    #region tinh so gio nghi tung ngay cua nhan vien
                    var leavdayByProfile = leadayProfiles.Where(s => s.ProfileID == profile.ID).ToList();
                    Dictionary<DateTime, List<Guid?>> tempShiftByDate = new Dictionary<DateTime, List<Guid?>>();
                    tempShiftByDate = Att_AttendanceLib.GetDailyShifts(dateStart, dateEnd, profile.ID, rosterProfiles, lstRosterGroup);
                    foreach (var leavdayProfile in leavdayByProfile)
                    {
                        for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                        {
                            bool isWorkDay = workDayDates.Contains(date);
                            if (isWorkDay)
                            {
                                //lay ca lam viec cua nhan vien
                                if (tempShiftByDate != null)
                                {
                                    var lstShiftID = tempShiftByDate.Where(s => s.Key == date).SelectMany(s => s.Value).ToList();
                                    if (lstShiftID.Count > 0)
                                    {
                                        var lstShiftProfileByDate = shifts.Where(s => s.WorkHours != null && lstShiftID.Contains(s.ID)).ToList();
                                        double? _tempWorkHour = 0;
                                        _tempWorkHour = lstShiftProfileByDate.Sum(s => s.WorkHours);
                                        if (_tempWorkHour > 0)
                                        {
                                            row["Day" + date.Day.ToString()] = _tempWorkHour;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    table.Rows.Add(row);
                }
                var configs = new Dictionary<string, Dictionary<string, object>>();
                var config = new Dictionary<string, object>();
                var confighidden = new Dictionary<string, object>();
                confighidden.Add("hidden", true);
                //var configdate = new Dictionary<string, object>();
                //configdate.Add("format", "{0:dd/MM/yyyy}");
                config.Add("width", 100);
                //config.Add("locked", true);
                if (!configs.ContainsKey("CodeEmp"))
                    configs.Add("CodeEmp", config);
                if (!configs.ContainsKey("DepartmentCode"))
                    configs.Add("DepartmentCode", config);
                if (!configs.ContainsKey("PositionName"))
                    configs.Add("PositionName", config);
                if (!configs.ContainsKey("JobTitleName"))
                    configs.Add("JobTitleName", config);
                if (!configs.ContainsKey("Paid"))
                    configs.Add("Paid", config);
                if (!configs.ContainsKey("BranchCode"))
                    configs.Add("BranchCode", config);
                if (!configs.ContainsKey("TeamCode"))
                    configs.Add("TeamCode", config);
                if (!configs.ContainsKey("SectionCode"))
                    configs.Add("SectionCode", config);
                if (!configs.ContainsKey("OrgName"))
                    configs.Add("OrgName", config);
                if (!configs.ContainsKey("TeamName"))
                    configs.Add("TeamName", config);
                if (!configs.ContainsKey("SectionName"))
                    configs.Add("SectionName", config);
                if (!configs.ContainsKey("BranchName"))
                    configs.Add("BranchName", config);
                if (!configs.ContainsKey("DateFrom"))
                    configs.Add("DateFrom", confighidden);
                if (!configs.ContainsKey("DateTo"))
                    configs.Add("DateTo", confighidden);
                if (!configs.ContainsKey("UserExport"))
                    configs.Add("UserExport", confighidden);
                if (!configs.ContainsKey("DateExport"))
                    configs.Add("DateExport", confighidden);

                foreach (var leavdayType in leavedayTypes)
                {
                    if (!configs.ContainsKey(leavdayType.Code))
                        configs.Add(leavdayType.Code, config);
                }

                return table.ConfigTable(configs);
            }

        }




        public DataTable GetReportDetailedMonthlyStay(DateTime dateStart, DateTime dateEnd, List<Hre_ProfileEntity> profiles, string orgStructureID, bool isIncludeQuitEmp, Guid[] leaveDayTypeIDs, string codeEmp, bool isNotAllowZero, string userExport, bool IsCreateTemplate, string UserLogin)
        {
            using (var context = new VnrHrmDataContext())
            {
                var baseservice = new BaseService();
                string key = OverTimeStatus.E_APPROVED.ToString();
                DataTable table = CreateReportDetailedMonthlyStaySchema(leaveDayTypeIDs, dateStart, dateEnd);
                if (IsCreateTemplate)
                {
                    return table.ConfigTable();
                }
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Leaveday = new Att_LeavedayRepository(unitOfWork);
                var repoAtt_Roster = new Att_RosterRepository(unitOfWork);
                var repoCat_LeaveDayTypeRepository = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                var repoCat_DayOff = new Cat_DayOffRepository(unitOfWork);
                var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                var repoCat_GradeAtt = new Cat_GradeAttendanceRepository(unitOfWork);
                var repoHre_WorkHistory = new Hre_WorkHistoryRepository(unitOfWork);
                var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);


                List<Guid> lstProfileIDs = profiles.Select(s => s.ID).ToList();
                List<Att_LeaveDayEntity> leaveDays = new List<Att_LeaveDayEntity>();
                List<Cat_LeaveDayType> leavedaytypes = new List<Cat_LeaveDayType>();
                string status = string.Empty;
                List<object> para = new List<object>();
                para.AddRange(new object[3]);
                para[0] = (object)orgStructureID;
                para[1] = dateStart;
                para[2] = dateEnd;
                if (lstProfileIDs != null && lstProfileIDs.Count > 0)
                {
                    leaveDays = baseservice.GetData<Att_LeaveDayEntity>(para, ConstantSql.hrm_att_getdata_LeaveDay, UserLogin, ref status).Where(s => s.Status == key).ToList();
                }
                else
                {
                    return table.ConfigTable();

                }

                //[Tho.Bui] Get LeavedayR=type
                if (leaveDays != null && leaveDays.Count > 0)
                {
                    List<Guid> lstLDTinLD = leaveDays.Select(s => s.LeaveDayTypeID).Distinct().ToList();
                    leavedaytypes = repoCat_LeaveDayTypeRepository.FindBy(s => s.IsDelete == null && lstLDTinLD.Contains(s.ID)).ToList();
                    List<Guid> leavedayguid = leaveDays.Select(s => s.LeaveDayTypeID).ToList();
                    leavedaytypes = leavedaytypes.Where(s => leavedayguid.Contains(s.ID)).ToList();
                }
                var profileIds = leaveDays.Select(s => s.ProfileID).Distinct().ToList();

                var rosterStatus = RosterStatus.E_APPROVED.ToString();
                var _rosters = new List<Att_Roster>();
                foreach (var tempProfileIds in lstProfileIDs.Chunk(1000))
                {
                    _rosters.AddRange(unitOfWork.CreateQueryable<Att_Roster>(Guid.Empty, d => tempProfileIds.Contains(d.ProfileID) && d.Status == rosterStatus && d.DateStart <= dateEnd && d.DateEnd >= dateStart).ToList());
                }
                var rosters = new List<Att_RosterEntity>();
                if (_rosters.Count > 0)
                    rosters = _rosters.Translate<Att_RosterEntity>();
                var repoCat_LeaveDayType = new Cat_LeaveDayTypeRepository(unitOfWork);
                var leavedayTypes = repoCat_LeaveDayType.FindBy(s => s.Code != null && s.IsDelete == null).Select(s => new { s.Code, s.ID, s.PaidRate, s.LeaveDayTypeName }).ToList();
                var leavdayPadidIDs = leavedayTypes.Where(s => s.PaidRate > 0).Select(s => s.ID).ToList();
                List<Guid> guids = profiles.Select(s => s.ID).ToList();
                leaveDays = leaveDays.Where(s => guids.Contains(s.ProfileID)).ToList();

                if (leaveDayTypeIDs != null)
                {
                    leaveDays = leaveDays.Where(s => leaveDayTypeIDs.Contains(s.LeaveDayTypeID)).ToList();
                    profileIds = leaveDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }
                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateEnd).ToList();
                }
                //var shifts = repoCat_Shift.FindBy(s => s.IsDelete == null).ToList();
                var shifts = unitOfWork.CreateQueryable<Cat_Shift>(Guid.Empty).ToList().Translate<Cat_ShiftEntity>();

                List<string> codeRejects = new List<string>();
                codeRejects.Add("SICK");
                codeRejects.Add("SU");
                codeRejects.Add("SD");
                codeRejects.Add("D");
                codeRejects.Add("DP");
                codeRejects.Add("PSN");
                codeRejects.Add("DSP");
                codeRejects.Add("M");
                List<Guid> leaveDayTypeRejectIDs = leavedayTypes.Where(s => codeRejects.Contains(s.Code)).Select(s => s.ID).ToList();

                if (isNotAllowZero)
                {
                    var profileIDs = leaveDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIDs.Contains(s.ID)).ToList();
                }
                #region sai xong xóa
                string CodeEmpTest = string.Empty;
                if (CodeEmpTest != string.Empty)
                {
                    char[] ext = new char[] { ';', ',' };
                    List<string> lstCodeEmpTest = CodeEmpTest.Split(ext, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                    profiles = profiles.Where(m => lstCodeEmpTest.Contains(m.CodeEmp)).ToList();
                }
                #endregion

                var leveDayIDs = leaveDays.Select(s => s.LeaveDayTypeID).ToList();
                leavedayTypes = leavedayTypes.Where(s => leveDayIDs.Contains(s.ID)).ToList();
                var holidayDates = repoCat_DayOff.FindBy(hol => hol.DateOff >= dateStart && hol.DateOff <= dateEnd).Select(s => s.DateOff).ToList();
                holidayDates = holidayDates.Select(s => s.Date).ToList();

                var lstRosterGroup = unitOfWork.CreateQueryable<Att_RosterGroup>(Guid.Empty, s => s.DateStart != null && s.DateStart >= dateStart && s.DateEnd != null && s.DateEnd <= dateEnd).ToList().Translate<Att_RosterGroupEntity>();

                foreach (var profile in profiles)
                {
                    var rosterProfiles = rosters.Where(s => s.ProfileID == profile.ID).ToList();
                    List<DateTime> workDayDates = new List<DateTime>();
                    foreach (var roster in rosterProfiles)
                    {
                        for (DateTime date = roster.DateStart; date <= roster.DateEnd; date = date.AddDays(1))
                        {
                            workDayDates.Add(date.Date);
                        }
                    }
                    workDayDates = workDayDates.Where(s => !holidayDates.Contains(s.Date)).ToList();

                    DataRow row = table.NewRow();
                    var leaveDay = leaveDays.FirstOrDefault(s => s.ProfileID == profile.ID);
                    var LeaveDayType = leavedaytypes.FirstOrDefault(s => s.ID != Guid.Empty && leaveDay != null && s.ID == leaveDay.LeaveDayTypeID);

                    row[Att_ReportDetailedMonthlyStayEntity.FieldNames.ProfileName] = profile.ProfileName;
                    row[Att_ReportDetailedMonthlyStayEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    if (profile.E_COMPANY != null)
                        row[Att_ReportDetailedMonthlyStayEntity.FieldNames.E_COMPANY] = profile.E_COMPANY;
                    if (profile.E_BRANCH != null)
                        row[Att_ReportDetailedMonthlyStayEntity.FieldNames.E_BRANCH] = profile.E_BRANCH;
                    if (profile.E_UNIT != null)
                        row[Att_ReportDetailedMonthlyStayEntity.FieldNames.E_UNIT] = profile.E_UNIT;
                    if (profile.E_DIVISION != null)
                        row[Att_ReportDetailedMonthlyStayEntity.FieldNames.E_DIVISION] = profile.E_DIVISION;
                    if (profile.E_DEPARTMENT != null)
                        row[Att_ReportDetailedMonthlyStayEntity.FieldNames.E_DEPARTMENT] = profile.E_DEPARTMENT;
                    if (profile.E_TEAM != null)
                        row[Att_ReportDetailedMonthlyStayEntity.FieldNames.E_TEAM] = profile.E_TEAM;
                    if (profile.E_SECTION != null)
                        row[Att_ReportDetailedMonthlyStayEntity.FieldNames.E_SECTION] = profile.E_SECTION;

                    if (profile.JobTitleName != null)
                        row[Att_ReportDetailedMonthlyStayEntity.FieldNames.JobTitleName] = profile.JobTitleName;
                    if (profile.PositionName != null)
                        row[Att_ReportDetailedMonthlyStayEntity.FieldNames.PositionName] = profile.PositionName;

                    row[Att_ReportDetailedMonthlyStayEntity.FieldNames.DateFrom] = dateStart;
                    row[Att_ReportDetailedMonthlyStayEntity.FieldNames.DateTo] = dateEnd;
                    row[Att_ReportDetailedMonthlyStayEntity.FieldNames.UserExport] = userExport;
                    row[Att_ReportDetailedMonthlyStayEntity.FieldNames.DateExport] = DateTime.Today;
                    var leadayProfiles = leaveDays.Where(s => s.ProfileID == profile.ID).ToList();
                    var ledayeTyepIDs = leadayProfiles.Select(s => s.LeaveDayTypeID).ToList();
                    double sumPaid = 0;
                    foreach (var leadayType in leavedayTypes.Where(s => ledayeTyepIDs.Contains(s.ID)).ToList())
                    {
                        double sum = 0;
                        var leavdayProfiles = leadayProfiles.Where(s => s.LeaveDayTypeID == leadayType.ID).ToList();
                        foreach (var leavdayProfile in leavdayProfiles)
                        {
                            var DateEnd = leavdayProfile.DateEnd;
                            if (DateEnd > dateEnd)
                            {
                                DateEnd = dateEnd;
                            }
                            for (DateTime date = leavdayProfile.DateStart; date <= DateEnd; date = date.AddDays(1))
                            {
                                bool isWorkDay = workDayDates.Contains(date);
                                if (!isWorkDay)// neu ngay do ko phai ngay di lam
                                {
                                    if (leaveDayTypeRejectIDs.Contains(leavdayProfile.LeaveDayTypeID))// neu ngay do la ngay nghi dc xem la ko xem ca or ngay nghi
                                    {
                                        isWorkDay = true;
                                    }
                                }
                                if (isWorkDay)
                                {
                                    if (leavdayProfile.Duration > 0)
                                    {
                                        //sum++; 
                                        sum += leavdayProfile.Duration;
                                    }
                                    if (leavdayPadidIDs.Contains(leavdayProfile.LeaveDayTypeID))
                                    {
                                        sumPaid += leadayType.PaidRate;
                                        //sumPaid++;
                                    }
                                }
                            }
                        }
                        row[leadayType.Code] = sum > 0 ? (object)sum : DBNull.Value;
                        row[Att_ReportDetailedMonthlyStayEntity.FieldNames.Paid] = sumPaid > 0 ? (object)sumPaid : DBNull.Value;
                    }
                    #region tinh so gio nghi tung ngay cua nhan vien
                    var leavdayByProfile = leadayProfiles.Where(s => s.ProfileID == profile.ID).ToList();
                    Dictionary<DateTime, List<Guid?>> tempShiftByDate = new Dictionary<DateTime, List<Guid?>>();
                    tempShiftByDate = Att_AttendanceLib.GetDailyShifts(dateStart, dateEnd, profile.ID, rosterProfiles, lstRosterGroup);
                    foreach (var leavdayProfile in leavdayByProfile)
                    {
                        for (DateTime date = dateStart; date < dateEnd; date = date.AddDays(1))
                        {
                            bool isWorkDay = workDayDates.Contains(date);
                            if (isWorkDay)
                            {
                                //lay ca lam viec cua nhan vien
                                if (tempShiftByDate != null)
                                {
                                    var lstShiftID = tempShiftByDate.Where(s => s.Key == date).SelectMany(s => s.Value).ToList();
                                    if (lstShiftID.Count > 0)
                                    {
                                        var lstShiftProfileByDate = shifts.Where(s => s.WorkHours != null && lstShiftID.Contains(s.ID)).ToList();
                                        double? _tempWorkHour = 0;
                                        _tempWorkHour = lstShiftProfileByDate.Sum(s => s.WorkHours);
                                        if (_tempWorkHour > 0)
                                        {
                                            row["Day" + date.Day.ToString()] = _tempWorkHour;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    table.Rows.Add(row);
                }
                var configs = new Dictionary<string, Dictionary<string, object>>();
                var config = new Dictionary<string, object>();
                var confighidden = new Dictionary<string, object>();
                confighidden.Add("hidden", true);
                //var configdate = new Dictionary<string, object>();
                //configdate.Add("format", "{0:dd/MM/yyyy}");
                config.Add("width", 100);
                //config.Add("locked", true);
                if (!configs.ContainsKey("CodeEmp"))
                    configs.Add("CodeEmp", config);
                if (!configs.ContainsKey("DepartmentCode"))
                    configs.Add("DepartmentCode", config);
                if (!configs.ContainsKey("PositionName"))
                    configs.Add("PositionName", config);
                if (!configs.ContainsKey("JobTitleName"))
                    configs.Add("JobTitleName", config);
                if (!configs.ContainsKey("Paid"))
                    configs.Add("Paid", config);
                if (!configs.ContainsKey("BranchCode"))
                    configs.Add("BranchCode", config);
                if (!configs.ContainsKey("TeamCode"))
                    configs.Add("TeamCode", config);
                if (!configs.ContainsKey("SectionCode"))
                    configs.Add("SectionCode", config);
                if (!configs.ContainsKey("OrgName"))
                    configs.Add("OrgName", config);
                if (!configs.ContainsKey("TeamName"))
                    configs.Add("TeamName", config);
                if (!configs.ContainsKey("SectionName"))
                    configs.Add("SectionName", config);
                if (!configs.ContainsKey("BranchName"))
                    configs.Add("BranchName", config);
                if (!configs.ContainsKey("DateFrom"))
                    configs.Add("DateFrom", confighidden);
                if (!configs.ContainsKey("DateTo"))
                    configs.Add("DateTo", confighidden);
                if (!configs.ContainsKey("UserExport"))
                    configs.Add("UserExport", confighidden);
                if (!configs.ContainsKey("DateExport"))
                    configs.Add("DateExport", confighidden);

                foreach (var leavdayType in leavedayTypes)
                {
                    if (!configs.ContainsKey(leavdayType.Code))
                        configs.Add(leavdayType.Code, config);
                }

                return table.ConfigTable(configs);
            }

        }

        /// <summary>
        /// Tho.Bui: Khởi tạo datatable cho báo cáo thống kê ngày nghĩ năm
        /// </summary>
        /// <param name="MonthStart"></param>
        /// <param name="MonthEnd"></param>
        /// <param name="dicSchemma"></param>
        /// <returns></returns>\
        /// 
        DataTable GetSchema(DateTime MonthStart, DateTime MonthEnd, out Dictionary<string, string> dicSchemma)
        {
            using (var context = new VnrHrmDataContext())
            {
                var configs = new Dictionary<string, Dictionary<string, object>>();
                var confighiden = new Dictionary<string, object>();

                var config60 = new Dictionary<string, object>();
                config60.Add("width", 60);
                var config100 = new Dictionary<string, object>();
                config100.Add("width", 100);

                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);

                string status = string.Empty;
                dicSchemma = new Dictionary<string, string>();
                var lstLeavedayType = repoCat_LeaveDayType.FindBy(s => s.Code != null).Select(s => s.Code).Distinct().ToList();
                DataTable tb = new DataTable("Att_ReportSickLeaveEntity");
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.ProfileName);
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.DepartmentCode);
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.BranchCode);
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.TeamCode);
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.CodeSection);
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.OrgName);
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.SectionName);
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.CodePosition);
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.CodeJobtitle);
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.BranchName);
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.TotalP);
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.TotalSC);
                Dictionary<string, string> dicDataLocation = new Dictionary<string, string>();
                int Stt = 0;
                for (DateTime MonthCheck = MonthStart; MonthCheck <= MonthEnd; MonthCheck = MonthCheck.AddMonths(1))
                {
                    foreach (string code in lstLeavedayType)
                    {
                        Stt++;
                        string nametempt = code + MonthCheck.Month;
                        if (!dicDataLocation.ContainsKey(nametempt))
                        {
                            dicDataLocation.Add(code + MonthCheck.Month, Att_ReportSickLeaveEntity.FieldNames.Data + Stt);
                            tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.Data + Stt, typeof(double));
                            tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.DataHeader + Stt, typeof(string));
                        }
                        if (!configs.ContainsKey(Att_ReportSickLeaveEntity.FieldNames.Data + Stt))
                            configs.Add(Att_ReportSickLeaveEntity.FieldNames.Data + Stt, config60);
                        if (!configs.ContainsKey(Att_ReportSickLeaveEntity.FieldNames.DataHeader + Stt))
                            configs.Add(Att_ReportSickLeaveEntity.FieldNames.DataHeader + Stt, config100);
                    }
                }
                Stt++;
                dicDataLocation.Add(Att_ReportSickLeaveEntity.FieldNames.SumANL, Att_ReportSickLeaveEntity.FieldNames.Data + Stt);
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.Data + Stt, typeof(double));
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.DataHeader + Stt, typeof(string));
                Stt++;
                dicDataLocation.Add(Att_ReportSickLeaveEntity.FieldNames.SumSICK, Att_ReportSickLeaveEntity.FieldNames.Data + Stt);
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.Data + Stt, typeof(double));
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.DataHeader + Stt, typeof(string));

                if (!configs.ContainsKey(Att_ReportSickLeaveEntity.FieldNames.Data + Stt))
                    configs.Add(Att_ReportSickLeaveEntity.FieldNames.Data + Stt, config60);
                if (!configs.ContainsKey(Att_ReportSickLeaveEntity.FieldNames.DataHeader + Stt))
                    configs.Add(Att_ReportSickLeaveEntity.FieldNames.DataHeader + Stt, config100);

                dicSchemma = dicDataLocation;
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.DateFrom, typeof(DateTime));
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.DateTo, typeof(DateTime));
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.UserExport);
                tb.Columns.Add(Att_ReportSickLeaveEntity.FieldNames.DateExport, typeof(DateTime));



                var config45 = new Dictionary<string, object>();
                config45.Add("width", 45);
                var config50 = new Dictionary<string, object>();
                config50.Add("width", 50);
                var config55 = new Dictionary<string, object>();
                config55.Add("width", 55);
                //var config60 = new Dictionary<string, object>();
                //config60.Add("width", 60);
                var config65 = new Dictionary<string, object>();
                config65.Add("width", 65);
                var config70 = new Dictionary<string, object>();
                config70.Add("width", 70);
                var config75 = new Dictionary<string, object>();
                config75.Add("width", 75);
                var config80 = new Dictionary<string, object>();
                config80.Add("width", 80);
                var config85 = new Dictionary<string, object>();
                config85.Add("width", 85);
                var config90 = new Dictionary<string, object>();
                config90.Add("width", 90);
                var config95 = new Dictionary<string, object>();
                config95.Add("width", 95);
                var config110 = new Dictionary<string, object>();
                config110.Add("width", 110);
                var config160 = new Dictionary<string, object>();
                config160.Add("width", 160);
                if (!configs.ContainsKey("CodeEmp"))
                    configs.Add("CodeEmp", config85);
                if (!configs.ContainsKey("BranchCode"))
                    configs.Add("BranchCode", config90);
                if (!configs.ContainsKey("DepartmentCode"))
                    configs.Add("DepartmentCode", config90);
                if (!configs.ContainsKey("TeamCode"))
                    configs.Add("TeamCode", config65);
                if (!configs.ContainsKey("CodeSection"))
                    configs.Add("CodeSection", config75);
                if (!configs.ContainsKey("CodePosition"))
                    configs.Add("CodePosition", config80);
                if (!configs.ContainsKey("CodeJobtitle"))
                    configs.Add("CodeJobtitle", config90);
                if (!configs.ContainsKey("BranchName"))
                    configs.Add("BranchName", config75);
                if (!configs.ContainsKey("OrgName"))
                    configs.Add("OrgName", config70);
                if (!configs.ContainsKey("SectionName"))
                    configs.Add("SectionName", config45);
                if (!configs.ContainsKey("TotalP"))
                    configs.Add("TotalP", config160);
                if (!configs.ContainsKey("TotalSC"))
                    configs.Add("TotalSC", config160);



                confighiden.Add("hidden", true);
                if (!configs.ContainsKey("DateFrom"))
                    configs.Add("DateFrom", confighiden);
                if (!configs.ContainsKey("DateTo"))
                    configs.Add("DateTo", confighiden);
                if (!configs.ContainsKey("UserExport"))
                    configs.Add("UserExport", confighiden);
                if (!configs.ContainsKey("DateExport"))
                    configs.Add("DateExport", confighiden);

                return tb.ConfigTable(configs);
            }

        }
        /// <summary>
        /// Lấy Dữ Liệu BC Chi Tiết Nghỉ Phép Ốm của nhân viên
        /// </summary>
        /// <returns></returns>
        /// 
        public DataTable GetReportLeaveYear(int dateYear, List<Guid?> leaveDayTypeIDs, List<Guid> lstProfileIDs, string userExport, string codeEmp)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                int year = dateYear;
                DateTime dateStart = new DateTime(year, 1, 1);
                DateTime dateEnd = new DateTime(year, 12, DateTime.DaysInMonth(year, 12));
                string key = OverTimeStatus.E_APPROVED.ToString();
                var repoCat_Leaveday = new Cat_LeaveDayTypeRepository(unitOfWork);
                var leavedayTypes = repoCat_Leaveday.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PaidRate }).Distinct().ToList();
                var leadayTypeIDs = leavedayTypes.Select(s => s.ID).ToList();
                var repoAtt_LeaveDay = new Att_LeavedayRepository(unitOfWork);
                var leaveDays = new List<Att_LeaveDay>().Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd, s.LeaveDays }).ToList();
                leaveDays = repoAtt_LeaveDay.FindBy(s => leadayTypeIDs.Contains(s.LeaveDayTypeID) && s.LeaveHours > 0 && s.Status == key && dateStart <= s.DateEnd && s.DateStart <= dateEnd)
                        .Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd, s.LeaveDays }).ToList();
                leaveDays = leaveDays.Where(s => leaveDayTypeIDs.Contains(s.ProfileID)).ToList();

                if (leaveDayTypeIDs[0] != null)
                {
                    leaveDays = leaveDays.Where(s => leaveDayTypeIDs.Contains(s.LeaveDayTypeID)).ToList();
                }
                var profileIds = leaveDays.Select(s => s.ProfileID).Distinct().ToList();
                lstProfileIDs.AddRange(profileIds);
                var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoHre_Profile.FindBy(s => profileIds.Contains(s.ID)).Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID, }).ToList();

                profiles = profiles.Where(s => lstProfileIDs.Contains(s.ID)).ToList();
                if (!string.IsNullOrEmpty(codeEmp))
                {
                    char[] ext = new char[] { ';', ',' };
                    List<string> codeEmpSeachs = codeEmp.Split(ext, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                    if (codeEmpSeachs.Count == 1)
                    {
                        string codeEmpSearch = codeEmpSeachs[0];
                        profiles = profiles.Where(hr => hr.CodeEmp == codeEmpSearch).ToList();
                    }
                    else
                    {
                        profiles = profiles.Where(hr => codeEmpSeachs.Contains(hr.CodeEmp)).ToList();
                    }
                }

                string E_ANNUAL_LEAVE = HRM.Infrastructure.Utilities.EnumDropDown.AnnualLeaveDetailType.E_ANNUAL_LEAVE.ToString();
                string E_SICK_LEAVE = HRM.Infrastructure.Utilities.EnumDropDown.AnnualLeaveDetailType.E_SICK_LEAVE.ToString();
                var repoAtt_AnnualLeaveDetail = new Att_AnnualLeaveDetailRepository(unitOfWork);

                var anualLeaves = repoAtt_AnnualLeaveDetail.FindBy(s => s.ProfileID != null && profileIds.Contains(s.ProfileID.Value) && (s.Type == E_ANNUAL_LEAVE || s.Type == E_SICK_LEAVE) && s.Year == year)
                   .Select(s => new { s.ProfileID, s.Month3, s.Type }).ToList();

                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();

                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var positions = repoCat_Position.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();

                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();

                var repoAtt_Pregnancy = new Att_PregnancyRepository(unitOfWork);
                var pregnancys = repoAtt_Pregnancy.FindBy(s => s.DateStart != null && s.DateEnd != null && dateStart <= s.DateEnd && s.DateStart <= dateEnd && profileIds.Contains(s.ProfileID))
                                .Select(s => new { s.ProfileID, s.DateStart, s.DateEnd }).ToList();

                Dictionary<string, string> dicSchemma = new Dictionary<string, string>();
                DataTable tb = GetSchema(dateStart, dateEnd, out dicSchemma);
                int stt = 0;
                foreach (var profile in profiles)
                {
                    stt++;
                    DataRow dr = tb.NewRow();
                    Guid? orgId = profile.OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    dr[Att_ReportSickLeaveEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                    dr[Att_ReportSickLeaveEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                    dr[Att_ReportSickLeaveEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                    dr[Att_ReportSickLeaveEntity.FieldNames.CodeSection] = orgSection != null ? orgSection.Code : string.Empty;
                    dr[Att_ReportSickLeaveEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    dr[Att_ReportSickLeaveEntity.FieldNames.OrgName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    dr[Att_ReportSickLeaveEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                    var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                    var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
                    dr[Att_ReportSickLeaveEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    dr[Att_ReportSickLeaveEntity.FieldNames.ProfileName] = profile.ProfileName;
                    dr[Att_ReportSickLeaveEntity.FieldNames.CodePosition] = positon != null ? positon.Code : string.Empty;
                    dr[Att_ReportSickLeaveEntity.FieldNames.CodeJobtitle] = jobtitle != null ? jobtitle.Code : string.Empty;
                    var anual = anualLeaves.FirstOrDefault(s => s.ProfileID == profile.ID && s.Type == E_ANNUAL_LEAVE);
                    var anualSick = anualLeaves.FirstOrDefault(s => s.ProfileID == profile.ID && s.Type == E_SICK_LEAVE);
                    if (dicSchemma.ContainsKey(Att_ReportSickLeaveEntity.FieldNames.SumANL))
                    {
                        dr[dicSchemma[Att_ReportSickLeaveEntity.FieldNames.SumANL]] = anual != null ? (object)anual.Month3 : DBNull.Value;
                        string SttOfData = dicSchemma[Att_ReportSickLeaveEntity.FieldNames.SumANL].Substring(4, dicSchemma[Att_ReportSickLeaveEntity.FieldNames.SumANL].Length - 4);
                        if (stt == 1)
                        {
                            dr[Att_ReportSickLeaveEntity.FieldNames.DataHeader + SttOfData] = "Total P";
                        }
                        else if (stt == 2)
                        {
                            dr[Att_ReportSickLeaveEntity.FieldNames.DataHeader + SttOfData] = "Total SC";
                        }
                    }
                    if (dicSchemma.ContainsKey(Att_ReportSickLeaveEntity.FieldNames.SumSICK))
                    {
                        dr[dicSchemma[Att_ReportSickLeaveEntity.FieldNames.SumSICK]] = anualSick != null ? (object)anualSick.Month3 : DBNull.Value;

                        string SttOfData = dicSchemma[Att_ReportSickLeaveEntity.FieldNames.SumSICK].Substring(4, dicSchemma[Att_ReportSickLeaveEntity.FieldNames.SumSICK].Length - 4);
                        if (stt == 1)
                        {
                            dr[Att_ReportSickLeaveEntity.FieldNames.DataHeader + SttOfData] = "Total P";
                        }
                        else if (stt == 2)
                        {
                            dr[Att_ReportSickLeaveEntity.FieldNames.DataHeader + SttOfData] = "Total SC";
                        }
                    }
                    //var pregnancyProfiles = pregnancys.Where(s => s.ProfileID == profile.ID).ToList();
                    //for (DateTime DateCheck = dateStart; DateCheck <= dateEnd; DateCheck = DateCheck.AddMonths(1))
                    //{
                    //    int dayPregancy = 0;
                    //    foreach (var pregnancyProfile in pregnancyProfiles)
                    //    {
                    //        for (DateTime date = pregnancyProfile.DateStart.Value; date < pregnancyProfile.DateEnd.Value; date = date.AddDays(1))
                    //        {
                    //            dayPregancy += 1;
                    //        }
                    //    }

                    //    foreach (var leaday in leavedayTypes)
                    //    {
                    //        if (dicSchemma.ContainsKey(leaday.Code + DateCheck.Month))
                    //        {
                    //            var sum = leaveDays.Where(s => dateStart <= s.DateEnd && s.DateStart <= dateEnd && s.ProfileID == profile.ID && s.LeaveDayTypeID == leaday.ID).Sum(s => s.TotalDuration);
                    //            dr[dicSchemma[leaday.CodeStatistic + DateCheck.Month]] = sum > 0 ? (object)(sum - dayPregancy) : DBNull.Value;
                    //            string SttOfData = dicSchemma[leaday.CodeStatistic + DateCheck.Month].Substring(4, dicSchemma[leaday.CodeStatistic + DateCheck.Month].Length - 4);
                    //            if (stt == 1)
                    //            {
                    //                dr[Att_ReportSickLeaveEntity.FieldNames.DataHeader + SttOfData] = DateCheck.ToString("MMM-yyyy");
                    //            }
                    //            else if (stt == 2)
                    //            {
                    //                dr[Att_ReportSickLeaveEntity.FieldNames.DataHeader + SttOfData] = leaday.CodeStatistic;
                    //            }
                    //        }
                    //    }
                    //}
                    //dr[Att_ReportSickLeaveEntity.FieldNames.DateFrom] = dateStart;
                    //dr[Att_ReportSickLeaveEntity.FieldNames.DateTo] = dateEnd;
                    //tb.Rows.Add(dr);
                    var pregnancyProfiles = pregnancys.Where(s => s.ProfileID == profile.ID).ToList();

                    for (DateTime DateCheck = dateStart; DateCheck <= dateEnd; DateCheck = DateCheck.AddMonths(1))
                    {
                        int dayPregancy = 0;
                        foreach (var pregnancyProfile in pregnancyProfiles)
                        {
                            for (DateTime date = pregnancyProfile.DateStart.Value; date < pregnancyProfile.DateEnd.Value; date = date.AddDays(1))
                            {
                                dayPregancy += 1;
                            }
                        }

                        foreach (var leaday in leavedayTypes)
                        {
                            if (dicSchemma.ContainsKey(leaday.Code + DateCheck.Month))
                            {
                                var sum = leaveDays.Where(s => dateStart <= s.DateEnd && s.DateStart <= dateEnd && s.ProfileID == profile.ID && s.LeaveDayTypeID == leaday.ID).Sum(s => s.LeaveDays);
                                dr[dicSchemma[leaday.Code + DateCheck.Month]] = sum > 0 ? (object)(sum - dayPregancy) : DBNull.Value;
                                string SttOfData = dicSchemma[leaday.Code + DateCheck.Month].Substring(4, dicSchemma[leaday.Code + DateCheck.Month].Length - 4);
                                if (stt == 1)
                                {
                                    dr[Att_ReportSickLeaveEntity.FieldNames.DataHeader + SttOfData] = DateCheck.ToString("MMM-yyyy");
                                }
                                else if (stt == 2)
                                {
                                    dr[Att_ReportSickLeaveEntity.FieldNames.DataHeader + SttOfData] = leaday.Code;
                                }
                            }
                        }
                    }
                    dr[Att_ReportSickLeaveEntity.FieldNames.DateFrom] = dateStart;
                    dr[Att_ReportSickLeaveEntity.FieldNames.DateTo] = dateEnd;
                    dr[Att_ReportSickLeaveEntity.FieldNames.UserExport] = userExport;
                    dr[Att_ReportSickLeaveEntity.FieldNames.DateExport] = DateTime.Today;
                    tb.Rows.Add(dr);
                }


                return tb;
            }
        }
        /// <summary>
        /// Lấy Dữ Liệu BC tổng hợp nghỉ hàng năm
        /// </summary>
        /// <returns></returns>
        public List<Att_ReportLeaveYearEntity> GetReportLeaveYearold(int year, List<Guid?> leaveDayTypeIDs, List<Guid> lstProfileIDs)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                DateTime dateStart = new DateTime(year, 1, 1);
                DateTime dateEnd = new DateTime(year, 12, DateTime.DaysInMonth(year, 12));
                string key = OverTimeStatus.E_APPROVED.ToString();
                var repoAtt_Leaveday = new Att_LeavedayRepository(unitOfWork);
                var leaveDays = new List<Att_LeaveDay>().Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd }).ToList();
                //if (lstProfileIDs != null && lstProfileIDs.Count > 0)
                //{
                leaveDays = repoAtt_Leaveday
                    .FindBy(s => s.LeaveHours > 0 && s.Status == key && dateStart <= s.DateEnd && s.DateStart <= dateEnd && lstProfileIDs.Contains(s.ProfileID))
                    .Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd }).ToList();
                //}
                //else
                //{
                //    leaveDays = repoAtt_Leaveday.FindBy(s => s.LeaveHours > 0 && s.Status == key && dateStart <= s.DateEnd && s.DateStart <= dateEnd)
                //        .Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd }).ToList();
                //}
                if (leaveDayTypeIDs[0] != null)
                {
                    leaveDays = leaveDays.Where(s => leaveDayTypeIDs.Contains(s.LeaveDayTypeID)).ToList();
                }
                var profileIds = leaveDays.Select(s => s.ProfileID).Distinct().ToList();
                var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoHre_Profile.FindBy(s => profileIds.Contains(s.ID))
                 .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
                string E_ANNUAL_LEAVE = HRM.Infrastructure.Utilities.EnumDropDown.AnnualLeaveDetailType.E_ANNUAL_LEAVE.ToString();
                string E_SICK_LEAVE = HRM.Infrastructure.Utilities.EnumDropDown.AnnualLeaveDetailType.E_SICK_LEAVE.ToString();
                var repoAtt_AnnualLeaveDetail = new Att_AnnualLeaveDetailRepository(unitOfWork);
                var anualLeaves = repoAtt_AnnualLeaveDetail.FindBy(s => s.ProfileID != null && profileIds.Contains(s.ProfileID.Value) && (s.Type == E_ANNUAL_LEAVE || s.Type == E_SICK_LEAVE) && s.Year == year)
                .Select(s => new { s.ProfileID, s.Month3, s.Type }).ToList();
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();

                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();

                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var positions = repoCat_Position.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();
                var repoCat_LeaveDayType = new Cat_LeaveDayTypeRepository(unitOfWork);
                var leavedayTypes = repoCat_LeaveDayType.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PaidRate }).Distinct().ToList();
                List<Att_ReportLeaveYearEntity> lstReportLeaveYearEntity = new List<Att_ReportLeaveYearEntity>();
                Att_ReportLeaveYearEntity reportLeaveYearEntity = null;
                foreach (var profile in profiles)
                {
                    Guid? orgId = profile.OrgStructureID;
                    reportLeaveYearEntity = new Att_ReportLeaveYearEntity();
                    var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);

                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);

                    reportLeaveYearEntity.BranchCode = orgBranch != null ? orgBranch.Code : string.Empty;
                    reportLeaveYearEntity.DepartmentCode = orgOrg != null ? orgOrg.Code : string.Empty;
                    reportLeaveYearEntity.TeamCode = orgTeam != null ? orgTeam.Code : string.Empty;
                    reportLeaveYearEntity.SectionCode = orgSection != null ? orgSection.Code : string.Empty;
                    var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                    var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
                    reportLeaveYearEntity.ProfileName = profile.ProfileName;
                    reportLeaveYearEntity.CodeEmp = profile.CodeEmp;
                    reportLeaveYearEntity.PositionName = positon != null ? positon.PositionName : string.Empty;
                    reportLeaveYearEntity.JobtitleName = jobtitle != null ? jobtitle.JobTitleName : string.Empty;
                    reportLeaveYearEntity.DateExport = DateTime.Now;
                    var anual = anualLeaves.FirstOrDefault(s => s.ProfileID == profile.ID && s.Type == E_ANNUAL_LEAVE);
                    var anualSick = anualLeaves.FirstOrDefault(s => s.ProfileID == profile.ID && s.Type == E_SICK_LEAVE);
                    reportLeaveYearEntity.TotalP = anual != null ? anual.Month3 : 0;
                    reportLeaveYearEntity.TotalSC = anualSick != null ? anualSick.Month3 : 0;
                    var leaveDay = leaveDays.FirstOrDefault(x => x.ProfileID == profile.ID);
                    //var Leadaytypename = leavedayTypes.FirstOrDefault(s=>s.ID == leaveDay.LeaveDayTypeID);
                    //reportLeaveYearEntity.LeaveDayTypeName = Leadaytypename.LeaveDayTypeName;
                    for (int i = 1; i <= 12; i++)
                    {
                        dateStart = new DateTime(year, i, 1);
                        dateEnd = new DateTime(year, i, DateTime.DaysInMonth(year, i));
                        reportLeaveYearEntity.FromDate = dateStart;
                        reportLeaveYearEntity.ToDate = dateEnd;
                        Dictionary<DateTime, Dictionary<string, double>> MonthLeave = new Dictionary<DateTime, Dictionary<string, double>>();
                        foreach (var leavedayType in leavedayTypes)
                        {
                            Dictionary<string, double> leavedayKeyValue = new Dictionary<string, double>();
                            double leaveDayHours = leaveDays.Where(s => dateStart <= s.DateEnd && s.DateStart <= dateEnd && s.ProfileID == profile.ID
                                && s.LeaveDayTypeID == leavedayType.ID && s.LeaveHours.HasValue).Sum(s => s.LeaveHours.Value / 8);

                            leavedayKeyValue.Add(leavedayType.Code, leaveDayHours);

                        }
                    }
                    lstReportLeaveYearEntity.Add(reportLeaveYearEntity);
                }

                return lstReportLeaveYearEntity;
            }
            //return lstReportLeaveYearEntity;
        }
        /// <summary>
        /// Lay du lieu bao cao tong hop nghi om
        /// </summary>
        /// <param name="dateYear"></param>
        /// <param name="lstProfileIDs"></param>
        /// <returns></returns>
        public List<Att_ReportSickLeaveEntity> GetReportSickLeave(DateTime dateYear, List<Guid> lstProfileIDs)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                int year = dateYear.Year;
                DateTime dateStart = new DateTime(year, 1, 1);
                DateTime dateEnd = new DateTime(year, 12, DateTime.DaysInMonth(year, 12));
                string key = OverTimeStatus.E_APPROVED.ToString();
                var repoCat_Leaveday = new Cat_LeaveDayTypeRepository(unitOfWork);
                var leavedayTypes = repoCat_Leaveday.FindBy(s => s.CodeStatistic == "P" || s.CodeStatistic == "SC").Select(s => new { s.ID, s.CodeStatistic, s.PaidRate }).Distinct().ToList();
                var leadayTypeIDs = leavedayTypes.Select(s => s.ID).ToList();
                var repoAtt_LeaveDay = new Att_LeavedayRepository(unitOfWork);
                var leaveDays = new List<Att_LeaveDay>().Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd }).ToList();
                if (lstProfileIDs != null && lstProfileIDs.Count > 0)
                {
                    leaveDays = repoAtt_LeaveDay.FindBy(s => leadayTypeIDs.Contains(s.LeaveDayTypeID) && s.LeaveHours > 0 && s.Status == key && dateStart <= s.DateEnd && s.DateStart <= dateEnd && lstProfileIDs.Contains(s.ProfileID))
                        .Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd }).ToList();
                }
                else
                {
                    leaveDays = repoAtt_LeaveDay.FindBy(s => leadayTypeIDs.Contains(s.LeaveDayTypeID) && s.LeaveHours > 0 && s.Status == key && dateStart <= s.DateEnd && s.DateStart <= dateEnd)
                        .Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd }).ToList();
                }
                var profileIds = leaveDays.Select(s => s.ProfileID).Distinct().ToList();
                var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);

                var profiles = repoHre_Profile.FindBy(s => profileIds.Contains(s.ID) && lstProfileIDs.Contains(s.ID)).Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID, }).ToList();

                string E_ANNUAL_LEAVE = HRM.Infrastructure.Utilities.EnumDropDown.AnnualLeaveDetailType.E_ANNUAL_LEAVE.ToString();
                string E_SICK_LEAVE = HRM.Infrastructure.Utilities.EnumDropDown.AnnualLeaveDetailType.E_SICK_LEAVE.ToString();
                var repoAtt_AnnualLeaveDetail = new Att_AnnualLeaveDetailRepository(unitOfWork);
                var anualLeaves = repoAtt_AnnualLeaveDetail.FindBy(s => s.ProfileID != null && profileIds.Contains(s.ProfileID.Value) && (s.Type == E_ANNUAL_LEAVE || s.Type == E_SICK_LEAVE) && s.Year == year)
                .Select(s => new { s.ProfileID, s.Month3, s.Type }).ToList();
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var positions = repoCat_Position.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();
                List<Att_ReportSickLeaveEntity> lstReportSickLeaveEntity = new List<Att_ReportSickLeaveEntity>();
                Att_ReportSickLeaveEntity reportSickLeaveEntity = null;
                foreach (var profile in profiles)
                {
                    reportSickLeaveEntity = new Att_ReportSickLeaveEntity();
                    Guid? orgId = profile.OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);

                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    reportSickLeaveEntity.BranchCode = orgBranch != null ? orgBranch.Code : string.Empty;
                    reportSickLeaveEntity.DepartmentCode = orgOrg != null ? orgOrg.Code : string.Empty;
                    reportSickLeaveEntity.TeamCode = orgTeam != null ? orgTeam.Code : string.Empty;
                    reportSickLeaveEntity.SectionCode = orgSection != null ? orgSection.Code : string.Empty;
                    var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                    var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
                    reportSickLeaveEntity.ProfileName = profile.ProfileName;
                    reportSickLeaveEntity.CodeEmp = profile.CodeEmp;
                    reportSickLeaveEntity.PositionName = positon != null ? positon.PositionName : string.Empty;
                    reportSickLeaveEntity.JobtitleName = jobtitle != null ? jobtitle.JobTitleName : string.Empty;
                    reportSickLeaveEntity.DateExport = DateTime.Now;
                    var anual = anualLeaves.FirstOrDefault(s => s.ProfileID == profile.ID && s.Type == E_ANNUAL_LEAVE);
                    var anualSick = anualLeaves.FirstOrDefault(s => s.ProfileID == profile.ID && s.Type == E_SICK_LEAVE);
                    reportSickLeaveEntity.TotalP = anual != null ? anual.Month3 : 0;
                    reportSickLeaveEntity.TotalSC = anualSick != null ? anualSick.Month3 : 0;
                    //reportSickLeaveEntity.BugetYearP = anualSick != null ? anualSick.Month3 : 0;
                    //reportSickLeaveEntity.BalanceSC = anualSick != null ? anualSick.Month3 : 0;

                    for (int i = 1; i <= 12; i++)
                    {
                        dateStart = new DateTime(year, i, 1);
                        dateEnd = new DateTime(year, i, DateTime.DaysInMonth(year, i));
                        reportSickLeaveEntity.DateFrom = dateStart;
                        reportSickLeaveEntity.DateTo = dateEnd;

                        foreach (var leaday in leavedayTypes)
                        {
                            Dictionary<string, double> leavedayKeyValue = new Dictionary<string, double>();
                            double leaveDayHours = leaveDays
                                .Where(s => dateStart <= s.DateEnd && s.DateStart <= dateEnd && s.ProfileID == profile.ID && s.LeaveDayTypeID == leaday.ID)
                                .Sum(s => s.LeaveHours.Value / 8);
                        }
                    }
                    lstReportSickLeaveEntity.Add(reportSickLeaveEntity);
                }
                return lstReportSickLeaveEntity;
            }

        }

        /// <summary>
        ///[Tam.Le] - Lấy Dữ Liệu BC chi tiết ngày nghỉ ốm
        /// </summary>
        /// <returns></returns>
        public List<Att_ReportDetailLeaveSickEntity> GetReportDetailLeaveSick(DateTime dateStart, DateTime dateEnd, string OrgStructureIDs, List<Hre_ProfileEntity> profiles, List<Guid?> shiftIDs, List<Guid?> leaveDayTypeIDs, Boolean? _isIncludeQuitEmp, string userExport)
        {
            using (var context = new VnrHrmDataContext())
            {
                List<Guid> lstProfileIDs = profiles.Select(s => s.ID).ToList();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                string key = OverTimeStatus.E_APPROVED.ToString();
                //    var repoCat_LeaveDayType = new Cat_LeaveDayTypeRepository(unitOfWork);
                var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                var leavedayTypes = repoCat_LeaveDayType
                    .FindBy(s => s.CodeStatistic != null && s.IsDelete == null).Select(s => new { s.ID, s.CodeStatistic, s.PaidRate })
                    .Distinct()
                    .ToList();
                var ledvedayPaidIds = leavedayTypes
                    .Where(s => s.PaidRate > 0 || s.CodeStatistic == "SU" || s.CodeStatistic == "SD" || s.CodeStatistic == "SC").Select(s => s.ID)
                    .ToList();
                var leaveDays = new List<Att_LeaveDay>().Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd }).ToList();
                //  var repoAtt_LeaveDay = new Att_LeavedayRepository(unitOfWork);
                var repoAtt_LeaveDay = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);

                leaveDays = repoAtt_LeaveDay
                        .FindBy(s => s.LeaveHours > 0 && s.IsDelete == null && ledvedayPaidIds.Contains(s.LeaveDayTypeID) && s.Status == key && dateStart <= s.DateEnd && s.DateStart <= dateEnd)
                        .Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd }).ToList();
                #region
                //if (lstProfileIDs != null && lstProfileIDs.Count > 0)
                //{
                //    leaveDays = repoAtt_LeaveDay
                //        .FindBy(s => s.LeaveHours > 0 && s.IsDelete==null && ledvedayPaidIds.Contains(s.LeaveDayTypeID) && s.Status == key && dateStart <= s.DateEnd && s.DateStart <= dateEnd && lstProfileIDs.Contains(s.ProfileID))
                //        .Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd }).ToList();
                //}
                //else
                //{
                //    leaveDays = repoAtt_LeaveDay
                //        .FindBy(s => s.LeaveHours > 0 && s.IsDelete==null && ledvedayPaidIds.Contains(s.LeaveDayTypeID) && s.Status == key && dateStart <= s.DateEnd && s.DateStart <= dateEnd)
                //        .Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd }).ToList();
                //}
                #endregion
                var profileIds = leaveDays.Select(s => s.ProfileID).Distinct().ToList();
                // var repoAtt_Workday = new Att_WorkDayRepository(unitOfWork);
                var repoAtt_Workday = new CustomBaseRepository<Att_Workday>(unitOfWork);
                var workDays = new List<Att_Workday>().Select(s => new { s.ProfileID, s.ShiftID, s.WorkDate, s.InTime1, s.OutTime1 }).ToList();
                workDays = repoAtt_Workday.FindBy(s => profileIds.Contains(s.ProfileID) && s.IsDelete == null && dateStart <= s.WorkDate && s.WorkDate <= dateEnd)
                       .Select(s => new { s.ProfileID, s.ShiftID, s.WorkDate, s.InTime1, s.OutTime1 }).ToList();
                profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                #region
                //if (lstProfileIDs != null && lstProfileIDs.Count > 0)
                //{
                //    workDays = repoAtt_Workday
                //        .FindBy(s => profileIds.Contains(s.ProfileID) && s.IsDelete == null && dateStart <= s.WorkDate && s.WorkDate <= dateEnd && (s.InTime1 != null || s.OutTime1 != null) && lstProfileIDs.Contains(s.ProfileID))
                //        .Select(s => new { s.ProfileID, s.ShiftID, s.WorkDate, s.InTime1, s.OutTime1 }).ToList();
                //}
                //else
                //{
                //    workDays = repoAtt_Workday.FindBy(s => profileIds.Contains(s.ProfileID) && s.IsDelete == null && dateStart <= s.WorkDate && s.WorkDate <= dateEnd && (s.InTime1 != null || s.OutTime1 != null))
                //        .Select(s => new { s.ProfileID, s.ShiftID, s.WorkDate, s.InTime1, s.OutTime1 }).ToList();
                //}
                //var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                //profiles = repoHre_Profile.FindBy(s => profileIds.Contains(s.ID))
                // .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
                #endregion
                if (OrgStructureIDs != null)
                {
                    profiles = profiles.Where(s => s.OrgStructureID != null).ToList();
                }
                if (leaveDayTypeIDs != null)
                {
                    leaveDays = leaveDays.Where(s => leaveDayTypeIDs.Contains(s.LeaveDayTypeID)).ToList();
                    profileIds = leaveDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }
                if (shiftIDs != null)
                {
                    workDays = workDays.Where(s => s.ShiftID.HasValue && shiftIDs.Contains(s.ShiftID.Value)).ToList();
                    profileIds = workDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }
                if (_isIncludeQuitEmp == false)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateStart).ToList();
                }
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var positions = repoCat_Position.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();
                //var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);
                var shifts = repoCat_Shift.GetAll().ToList();

                List<Att_ReportDetailLeaveSickEntity> lstReportDetailLeaveSickEntity = new List<Att_ReportDetailLeaveSickEntity>();
                Att_ReportDetailLeaveSickEntity reportDetailLeaveSickEntity = null;
                foreach (var profile in profiles)
                {
                    for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                    {
                        var leavdayProfiles = leaveDays.Where(s => s.DateStart.Date <= date.Date && date.Date <= s.DateEnd.Date && s.ProfileID == profile.ID).ToList();
                        if (leavdayProfiles.Count > 0)
                        {
                            reportDetailLeaveSickEntity = new Att_ReportDetailLeaveSickEntity();
                            Guid? orgId = profile.OrgStructureID;
                            var workDay = workDays.FirstOrDefault(s => s.ProfileID == profile.ID && s.WorkDate.Date == date.Date);
                            var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                            var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                            var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                            var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                            var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                            reportDetailLeaveSickEntity.BranchCode = orgBranch != null ? orgBranch.Code : string.Empty;
                            reportDetailLeaveSickEntity.DepartmentCode = orgOrg != null ? orgOrg.Code : string.Empty;
                            reportDetailLeaveSickEntity.TeamCode = orgTeam != null ? orgTeam.Code : string.Empty;
                            reportDetailLeaveSickEntity.SectionCode = orgSection != null ? orgSection.Code : string.Empty;
                            var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                            var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
                            reportDetailLeaveSickEntity.ProfileName = profile.ProfileName;
                            reportDetailLeaveSickEntity.CodeEmp = profile.CodeEmp;
                            reportDetailLeaveSickEntity.PositionName = positon != null ? positon.PositionName : string.Empty;
                            reportDetailLeaveSickEntity.JobtitleName = jobtitle != null ? jobtitle.JobTitleName : string.Empty;
                            var shift = shifts.FirstOrDefault(s => workDay != null && s.ID == workDay.ShiftID);
                            var leaveday = leavdayProfiles.FirstOrDefault(s => ledvedayPaidIds.Contains(s.LeaveDayTypeID));
                            reportDetailLeaveSickEntity.Date = date;
                            reportDetailLeaveSickEntity.DateFrom = dateStart;
                            reportDetailLeaveSickEntity.DateTo = dateEnd;
                            reportDetailLeaveSickEntity.DateExport = DateTime.Now;
                            reportDetailLeaveSickEntity.UserExport = userExport;
                            if (leaveday != null)
                            {
                                //reportDetailLeaveSickEntity.Paid = leaveday.LeaveHours / 8;
                                reportDetailLeaveSickEntity.IsPaid = "X";
                            }
                            if (workDay != null)
                            {
                                if (workDay.ShiftID != null)
                                    reportDetailLeaveSickEntity.ShiftName = shift.ShiftName;
                                if (workDay.InTime1 != null)
                                    reportDetailLeaveSickEntity.InTime = workDay.InTime1.Value;
                                if (workDay.OutTime1 != null)
                                    reportDetailLeaveSickEntity.OutTime = workDay.OutTime1.Value;
                            }
                            foreach (var leaday in leavedayTypes.Where(s => s.CodeStatistic == "SU" || s.CodeStatistic == "SD" || s.CodeStatistic == "SC"))
                            {
                                var leaday1 = leavdayProfiles.FirstOrDefault(s => s.LeaveDayTypeID == leaday.ID);
                                if (leaday1 != null)
                                {
                                    reportDetailLeaveSickEntity.CodeStatistic = (leaday1.LeaveHours.HasValue ? leaday1.LeaveHours.Value : 0) / 8;
                                    if (leaday.CodeStatistic == "SU")
                                        reportDetailLeaveSickEntity.SU = "X";
                                    else if (leaday.CodeStatistic == "SD")
                                        reportDetailLeaveSickEntity.SD = "X";
                                    else if (leaday.CodeStatistic == "SC")
                                        reportDetailLeaveSickEntity.SC = "X";
                                }
                            }
                            lstReportDetailLeaveSickEntity.Add(reportDetailLeaveSickEntity);
                        }
                    }


                }
                return lstReportDetailLeaveSickEntity;
            }
        }

        /// <summary>
        /// [SonVo] 05/06/2014
        /// Lấy ds tất cả ReportDetailOvertimeVer
        /// </summary>
        /// <returns></returns>
        public List<Att_ReportDetailOvertimeVer2Entity> GetReportDetailOvertimeVer2(DateTime dateStart, DateTime dateEnd, List<Guid> lstProfileIDs)
        {
            List<Att_ReportDetailOvertimeVer2Entity> ReportDetailOvertimeVer2 = new List<Att_ReportDetailOvertimeVer2Entity>();
            return ReportDetailOvertimeVer2;
        }

        /// <summary>
        /// [SonVo] 05/06/2014
        /// Lấy ds tất cả ReportProfileByShift
        /// </summary>
        /// <returns></returns>
        /// 

        public DataTable CreateReportProfileByShiftSchema(DateTime dateStart, DateTime dateEnd)
        {
            using (var context = new VnrHrmDataContext())
            {
                string status = string.Empty;
                DataTable tb = new DataTable("Att_ReportProfileByShift");
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoShift = new CustomBaseRepository<Cat_Shift>(unitOfWork);
                var shifts = repoShift.GetAll().Where(s => s.IsDelete == null && s.Code != null).Select(sh => new { sh.ShiftName }).Distinct().ToList();
                tb.Columns.Add(Att_ReportProfileByShiftEntity.FieldNames.WorkDay, typeof(DateTime));
                tb.Columns.Add(Att_ReportProfileByShiftEntity.FieldNames.UserExport);
                tb.Columns.Add(Att_ReportProfileByShiftEntity.FieldNames.DateExport, typeof(DateTime));
                //DataRow row = tb.NewRow();
                foreach (var item in shifts)
                {
                    if (!string.IsNullOrEmpty(item.ShiftName))
                    {

                        var strName = item.ShiftName.Replace(" ", "_");
                        tb.Columns.Add(strName, typeof(int));
                        //row[strName] = item.ShiftName;

                    }


                }
                //tb.Rows.Add(row);

                return tb;
            }

        }


        public DataTable GetReportProfileByShift(DateTime dateStart, DateTime dateEnd, List<Guid> lstProfileIDs, bool isCreateTable, string userExport)
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable table = CreateReportProfileByShiftSchema(dateStart, dateEnd);

                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoShift = new CustomBaseRepository<Cat_Shift>(unitOfWork);
                var shifts = repoShift.GetAll().Select(sh => new { sh.ID, sh.ShiftName }).ToList();
                if (isCreateTable == false)
                {
                    string status = string.Empty;
                    //   var profieIDs = LoadProfileStatus(string.Empty, departments, payrollGroups).Select(prf => prf.ID).ToList();
                    var repoAttGrade = new Att_GradeRepository(unitOfWork);

                    var grades = repoAttGrade.GetAll().ToList();
                    string key = RosterStatus.E_APPROVED.ToString();
                    var repoRoster = new CustomBaseRepository<Att_Roster>(unitOfWork);
                    var rosters = repoRoster.FindBy(s => s.DateStart <= dateEnd && s.DateEnd >= dateStart && lstProfileIDs.Contains(s.ProfileID) && s.Status == key)
                        .Select(ros => new
                        {
                            ros.ID,
                            ros.ProfileID,
                            ros.DateStart,
                            ros.DateEnd,
                            ros.MonShiftID,
                            ros.TueShiftID,
                            ros.WedShiftID,
                            ros.ThuShiftID,
                            ros.FriShiftID,
                            ros.SatShiftID,
                            ros.SunShiftID
                        }).ToList();


                    var repoGradeCfg = new CustomBaseRepository<Cat_GradeAttendance>(unitOfWork);
                    var gradeCfgs = repoGradeCfg.GetAll()
                        .Select(
                        grd => new
                        {
                            grd.ID,
                            grd.WorkOnMondayID,
                            grd.WorkOnTuesdayID,
                            grd.WorkOnWednesdayID,
                            grd.WorkOnThursdayID,
                            grd.WorkOnFridayID,
                            grd.WorkOnSaturdayID,
                            grd.WorkOnSundayID
                        }).ToList();

                    string key1 = GradeRosterType.E_ISROSTER.ToString();
                    string key2 = GradeRosterType.E_ISDEFAULT.ToString();
                    for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                    {
                        List<Guid?> guids1 = grades.Where(s => s.MonthStart <= date && date <= s.MonthEnd && s.Cat_GradeAttendance != null &&
                                s.Cat_GradeAttendance.RosterType == key1 && s.ProfileID.HasValue && lstProfileIDs.Contains(s.ProfileID.Value))
                                  .Select(s => s.ProfileID)
                                  .Distinct()
                                  .ToList();

                        List<Guid?> guids2 = grades.Where(s => s.MonthStart <= date && date <= s.MonthEnd && s.Cat_GradeAttendance != null &&
                                s.Cat_GradeAttendance.RosterType == key2 && s.ProfileID.HasValue && lstProfileIDs.Contains(s.ProfileID.Value))
                                  .Select(s => s.ProfileID)
                                  .Distinct()
                                  .ToList();
                        DataRow row = table.NewRow();
                        row[Att_ReportProfileByShiftEntity.FieldNames.WorkDay] = date.Date;
                        row[Att_ReportProfileByShiftEntity.FieldNames.UserExport] = userExport;
                        row[Att_ReportProfileByShiftEntity.FieldNames.DateExport] = DateTime.Today;
                        var attRoster = rosters.Where(s => s.DateStart <= date && date <= s.DateEnd).ToList();
                        foreach (var catShift in shifts)
                        {

                            int countRoster = attRoster.Where(s => s.MonShiftID == catShift.ID
                                                                   || s.TueShiftID == catShift.ID
                                                                   || s.WedShiftID == catShift.ID
                                                                   || s.ThuShiftID == catShift.ID
                                                                   || s.FriShiftID == catShift.ID
                                                                   || s.SatShiftID == catShift.ID
                                                                   ||
                                                                   s.SunShiftID == catShift.ID && guids1.Contains(s.ProfileID))
                                                       .Select(s => s.ProfileID)
                                                       .Distinct()
                                                       .Count();
                            List<Guid> guids3 = gradeCfgs.Where(s => s.WorkOnMondayID == catShift.ID
                                                           || s.WorkOnTuesdayID == catShift.ID
                                                           || s.WorkOnWednesdayID == catShift.ID
                                                           || s.WorkOnThursdayID == catShift.ID
                                                           || s.WorkOnFridayID == catShift.ID
                                                           || s.WorkOnSaturdayID == catShift.ID
                                                           || s.WorkOnSundayID == catShift.ID).Select(s => s.ID).ToList();
                            int countGrade = grades.Where(s => guids3.Contains(s.GradeAttendanceID.Value) && guids2.Contains(s.ID))
                                .Select(s => s.ProfileID).Distinct().Count();

                            var strName = catShift.ShiftName.Replace(" ", "");

                            if (catShift != null && !string.IsNullOrEmpty(catShift.ShiftName) && table.Columns.Contains(strName))
                            {
                                row[strName] = countRoster + countGrade;
                            }
                            //    row["ID"] = Guid.NewGuid();
                        }

                        table.Rows.Add(row);
                    }
                }
                var configs = new Dictionary<string, Dictionary<string, object>>();
                var config = new Dictionary<string, object>();
                var confighidden = new Dictionary<string, object>();
                confighidden.Add("hidden", true);
                configs.Add("UserExport", confighidden);
                configs.Add("DateExport", confighidden);
                config.Add("width", 100);

                foreach (var catShift in shifts)
                {
                    if (catShift.ShiftName != null)
                    {
                        var strName = catShift.ShiftName.Replace(" ", "");
                        if (catShift != null && !string.IsNullOrEmpty(catShift.ShiftName) && table.Columns.Contains(strName))
                        {
                            configs.Add(strName, config);
                        }
                    }
                    //    row["ID"] = Guid.NewGuid();
                }
                return table.ConfigTable(configs);
                //return table.ConfigTable();
            }

            #region Code Cũ
            //public List<Att_ReportProfileByShiftEntity> GetReportProfileByShift(DateTime dateStart, DateTime dateEnd, List<Guid> lstProfileIDs)
            //{
            //    using (var context = new VnrHrmDataContext())
            //    {

            //        var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

            //        DataTable datatable = CreateReportProfileByShiftSchema(dateStart, dateEnd);




            //        //var repoAtt_Workday = new Att_WorkDayRepository(unitOfWork);
            //        //var workDaysQuery = new List<Att_Workday>().Select(s => new { s.ProfileID, s.ShiftID, s.WorkDate, s.InTime1, s.OutTime1 });
            //        //if (lstProfileIDs != null && lstProfileIDs.Count > 0)
            //        //{
            //        //    workDaysQuery = repoAtt_Workday.FindBy(s => lstProfileIDs.Contains(s.ProfileID) && dateStart <= s.WorkDate && s.WorkDate <= dateEnd 
            //        //        && (s.InTime1 != null || s.OutTime1 != null) && lstProfileIDs.Contains(s.ProfileID))
            //        //        .Select(s => new { s.ProfileID, s.ShiftID, s.WorkDate, s.InTime1, s.OutTime1 });
            //        //}
            //        //else
            //        //{
            //        //    workDaysQuery = repoAtt_Workday.FindBy(s => lstProfileIDs.Contains(s.ProfileID) && dateStart <= s.WorkDate && s.WorkDate <= dateEnd && (s.InTime1 != null || s.OutTime1 != null))
            //        //        .Select(s => new { s.ProfileID, s.ShiftID, s.WorkDate, s.InTime1, s.OutTime1 });
            //        //}
            //        //var workDays = workDaysQuery.ToList();
            //        //var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);

            //        //List<Guid> lstProfileInWorkday = workDays.Select(m => m.ProfileID).Distinct().ToList<Guid>();

            //        //var profiles = new List<Hre_Profile>().Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
            //        //if (lstProfileInWorkday != null && lstProfileInWorkday.Count > 0)
            //        //{
            //        //    profiles = repoHre_Profile.FindBy(s => lstProfileInWorkday.Contains(s.ID))
            //        // .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
            //        //}


            //        //var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
            //        //var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
            //        //var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
            //        //var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
            //        //var repoCat_Position = new Cat_PositionRepository(unitOfWork);
            //        //var positions = repoCat_Position.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
            //        //var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
            //        //var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();
            //        //var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
            //        //var shifts = repoCat_Shift.GetAll().ToList();

            //       // List<Att_ReportProfileByShiftEntity> lstReportProfileByShiftEntity = new List<Att_ReportProfileByShiftEntity>();
            //        //Att_ReportProfileByShiftEntity ReportProfileByShiftEntity = null;
            //        //foreach (var profile in profiles)
            //        //{
            //        //    ReportProfileByShiftEntity = new Att_ReportProfileByShiftEntity();

            //        //        Guid? orgId = profile.OrgStructureID;
            //        //        var workDay = workDays.FirstOrDefault(s => s.ProfileID == profile.ID);
            //        //        var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
            //        //        //var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
            //        //        //var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
            //        //        //var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
            //        //        //var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
            //        //        //ReportProfileByShiftEntity.BranchCode = orgBranch != null ? orgBranch.Code : string.Empty;
            //        //        ReportProfileByShiftEntity.DepartmentCode = org != null ? org.Code : string.Empty;
            //        //        //ReportProfileByShiftEntity.TeamCode = orgTeam != null ? orgTeam.Code : string.Empty;
            //        //        //ReportProfileByShiftEntity.SectionCode = orgSection != null ? orgSection.Code : string.Empty;
            //        //        var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
            //        //        var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
            //        //        ReportProfileByShiftEntity.ProfileName = profile.ProfileName;
            //        //        ReportProfileByShiftEntity.CodeEmp = profile.CodeEmp;
            //        //        var shift = shifts.FirstOrDefault(s => workDay != null && s.ID == workDay.ShiftID);
            //        //        ReportProfileByShiftEntity.DateFrom = dateStart;
            //        //        ReportProfileByShiftEntity.DateTo = dateEnd;
            //        //        ReportProfileByShiftEntity.DateExport = DateTime.Now;
            //        //        if (workDay != null)
            //        //            ReportProfileByShiftEntity.WorkDay = workDay.WorkDate;
            //        //        if(shift!=null)
            //        //            ReportProfileByShiftEntity.ShiftName = shift.ShiftName;

            //        //        lstReportProfileByShiftEntity.Add(ReportProfileByShiftEntity);
            //        //}

            //        return lstReportProfileByShiftEntity;
            //    }
            //}
            #endregion


        }


        /// <summary>
        /// Lấy Dữ Liệu BC Thống Kê Đi Muộn Về Sớm
        /// </summary>
        /// <returns></returns>
        public List<Att_ReportSummaryLateInOutEntity> GetReportSummaryLateInOut(DateTime dateStart, DateTime dateEnd, List<Hre_ProfileEntity> profiles, Guid[] shiftIDs, bool isIncludeQuitEmp, string codeEmp, string userExport)
        {
            using (var context = new VnrHrmDataContext())
            {
                List<Guid> lstProfileIDs = profiles.Select(s => s.ID).ToList();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var repoAtt_WorkDay = new Att_WorkDayRepository(unitOfWork);
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var repoShift = new Cat_ShiftRepository(unitOfWork);
                var repoCat_TAMReasonmiss = new Cat_TAMScanReasonMissRepository(unitOfWork);

                string E_LATE_EARLY = WorkdayType.E_LATE_EARLY.ToString();
                string E_APPROVED = WorkdayStatus.E_APPROVED.ToString();
                var workDays = new List<Att_Workday>().Select(s => new { s.ProfileID, s.WorkDate, s.LateEarlyDuration, s.ShiftID, s.InTime1, s.OutTime1, s.MissInOutReason, s.LateDuration1, s.EarlyDuration1 }).ToList();

                workDays = repoAtt_WorkDay.FindBy(s => s.IsDelete == null && dateStart <= s.WorkDate && s.WorkDate <= dateEnd && s.Type == E_LATE_EARLY && !(s.Status == E_APPROVED && s.LateEarlyDuration == 0))
                    .Select(s => new { s.ProfileID, s.WorkDate, s.LateEarlyDuration, s.ShiftID, s.InTime1, s.OutTime1, s.MissInOutReason, s.LateDuration1, s.EarlyDuration1 }).ToList();

                var profileIds = workDays.Select(s => s.ProfileID).Distinct().ToList();
                profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                #region MyRegion
                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var positions = repoCat_Position.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();
                #endregion
                var reasons = repoCat_TAMReasonmiss.FindBy(s => s.IsDelete == null);
                var shifts = repoShift.FindBy(s => s.IsDelete == null);
                if (shiftIDs != null)
                {
                    workDays = workDays.Where(s => s.ShiftID.HasValue && shiftIDs.Contains(s.ShiftID.Value)).ToList();
                    profileIds = workDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }

                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateStart).ToList();
                }
                if (!string.IsNullOrEmpty(codeEmp))
                {
                    profiles = profiles.Where(s => s.CodeEmp == codeEmp).ToList();
                }

                var key = LeaveDayStatus.E_APPROVED.ToString();
                var repoLeaveDay = new Att_LeavedayRepository(unitOfWork);
                var leaveDays = repoLeaveDay.FindBy(s => s.Status == key && dateStart <= s.DateEnd && s.DateStart <= dateEnd);
                List<Guid> guids = profiles.Select(s => s.ID).ToList();
                workDays = workDays.Where(s => guids.Contains(s.ProfileID)).ToList();
                profiles = profiles.Where(s => guids.Contains(s.ID)).ToList();
                List<Att_ReportSummaryLateInOutEntity> lstReportSummaryLateInOutEntity = new List<Att_ReportSummaryLateInOutEntity>();
                Att_ReportSummaryLateInOutEntity reportSummaryLateInOutEntityEntity = null;
                foreach (var profile in profiles)
                {
                    for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                    {
                        var workDayProfiles = workDays.Where(s => s.ProfileID == profile.ID && s.WorkDate.Date == date).ToList();
                        var leveDay = leaveDays.FirstOrDefault(s => s.ProfileID == profile.ID && s.DateStart <= date && date <= s.DateEnd);
                        if (workDayProfiles.Count > 0 && leveDay == null)
                        {
                            var workDay = workDayProfiles.FirstOrDefault(s => s.WorkDate.Date == date);
                            if (workDay != null)
                            {
                                reportSummaryLateInOutEntityEntity = new Att_ReportSummaryLateInOutEntity();
                                Guid? orgId = profile.OrgStructureID;
                                var reason = reasons.FirstOrDefault(s => s.ID == workDay.MissInOutReason);
                                var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                                var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                                var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                                var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);

                                reportSummaryLateInOutEntityEntity.BranchCode = orgBranch != null ? orgBranch.Code : string.Empty;
                                reportSummaryLateInOutEntityEntity.DepartmentCode = orgOrg != null ? orgOrg.Code : string.Empty;
                                reportSummaryLateInOutEntityEntity.TeamCode = orgTeam != null ? orgTeam.Code : string.Empty;
                                reportSummaryLateInOutEntityEntity.SectionCode = orgSection != null ? orgSection.Code : string.Empty;

                                reportSummaryLateInOutEntityEntity.BranchName = orgBranch != null ? orgOrg.OrgStructureName : string.Empty;
                                reportSummaryLateInOutEntityEntity.DepartmentName = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                                reportSummaryLateInOutEntityEntity.TeamName = orgTeam != null ? orgOrg.OrgStructureName : string.Empty;
                                reportSummaryLateInOutEntityEntity.SectionName = orgSection != null ? orgOrg.OrgStructureName : string.Empty;

                                #region MyRegion
                                var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                                var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
                                #endregion
                                var shift = shifts.FirstOrDefault(s => s.ID == workDay.ShiftID);
                                reportSummaryLateInOutEntityEntity.ProfileName = profile.ProfileName;
                                reportSummaryLateInOutEntityEntity.CodeEmp = profile.CodeEmp;
                                #region MyRegion
                                reportSummaryLateInOutEntityEntity.PositionCode = positon != null ? positon.Code : string.Empty;
                                reportSummaryLateInOutEntityEntity.JobtitleCode = jobtitle != null ? jobtitle.Code : string.Empty;
                                reportSummaryLateInOutEntityEntity.PositionName = positon != null ? positon.PositionName : string.Empty;
                                reportSummaryLateInOutEntityEntity.JobtitleName = jobtitle != null ? jobtitle.JobTitleName : string.Empty;
                                #endregion
                                reportSummaryLateInOutEntityEntity.DateFrom = dateStart;
                                reportSummaryLateInOutEntityEntity.Date = date;
                                reportSummaryLateInOutEntityEntity.DateTo = dateEnd;
                                reportSummaryLateInOutEntityEntity.DateExport = DateTime.Now;
                                reportSummaryLateInOutEntityEntity.UserExport = userExport;
                                reportSummaryLateInOutEntityEntity.udTAMScanReasonMiss = reason == null ? string.Empty : reason.TAMScanReasonMissName;
                                reportSummaryLateInOutEntityEntity.ShiftName = shift == null ? string.Empty : shift.ShiftName;
                                reportSummaryLateInOutEntityEntity.udInTime = workDay.InTime1 != null ? workDay.InTime1.Value : DateTime.MinValue;
                                reportSummaryLateInOutEntityEntity.udOutTime = workDay.OutTime1 != null ? workDay.OutTime1.Value : DateTime.MinValue;
                                if (workDay.LateEarlyDuration >= 120)
                                {
                                    reportSummaryLateInOutEntityEntity.EarlyDurationMore2 = "X";
                                }
                                else
                                {
                                    reportSummaryLateInOutEntityEntity.EarlyDurationLess2 = "X";
                                }

                                if (workDay.LateDuration1 != null && workDay.LateDuration1 > 0)
                                {
                                    reportSummaryLateInOutEntityEntity.LateForWork = "X";
                                }
                                if (workDay.EarlyDuration1 != null && workDay.EarlyDuration1 > 0)
                                {
                                    reportSummaryLateInOutEntityEntity.EarlyGoHome = "X";
                                }

                                lstReportSummaryLateInOutEntity.Add(reportSummaryLateInOutEntityEntity);
                            }
                        }
                    }
                    #region MyRegion
                    //Guid? orgId = profile.OrgStructureID;
                    //var attendanceProfiles = attendanceTables.Where(s => s.ProfileID == profile.ID).ToList();
                    //var attendaceProfileIDs = attendanceProfiles.Select(s => s.ID).ToList();
                    //var attendanceItemProfiles = attendanceItems.Where(s => attendaceProfileIDs.Contains(s.AttendanceTableID)).ToList();                    
                    //var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                    //var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    //var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    //var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    //var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                    //reportSummaryLateInOutEntityEntity.LateMinutes = attendanceItemProfiles.Sum(s => s.LateInMinutes);
                    //reportSummaryLateInOutEntityEntity.EarlyMinutes = attendanceItemProfiles.Sum(s => s.EarlyOutMinutes);
                    //reportSummaryLateInOutEntityEntity.NumTimeLate = attendanceItemProfiles.Count(s => s.LateInMinutes > 0);
                    //reportSummaryLateInOutEntityEntity.NumTimeEarly = attendanceItemProfiles.Count(s => s.EarlyOutMinutes > 0);
                    //lstReportSummaryLateInOutEntity.Add(reportSummaryLateInOutEntityEntity);
                    #endregion
                }
                return lstReportSummaryLateInOutEntity;
            }
        }





        DataTable CreateReportSummaryLeaveYearSick()
        {
            DataTable tb = new DataTable("Att_ReportSummaryLeaveYearSickEntity");

            tb.Columns.Add(Att_ReportSummaryLeaveYearSickEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Att_ReportSummaryLeaveYearSickEntity.FieldNames.ProfileName);
            tb.Columns.Add(Att_ReportSummaryLeaveYearSickEntity.FieldNames.DepartmentCode);
            tb.Columns.Add(Att_ReportSummaryLeaveYearSickEntity.FieldNames.PositionName);
            tb.Columns.Add(Att_ReportSummaryLeaveYearSickEntity.FieldNames.JobTitleName);
            tb.Columns.Add(Att_ReportSummaryLeaveYearSickEntity.FieldNames.TotalAllowSick);
            tb.Columns.Add(Att_ReportSummaryLeaveYearSickEntity.FieldNames.TotalAllowYear);
            for (int i = 1; i <= 12; i++)
            {
                tb.Columns.Add("P" + i);
                tb.Columns.Add("SC" + i);
            }
            tb.Columns.Add(Att_ReportSummaryLeaveYearSickEntity.FieldNames.BranchCode);
            tb.Columns.Add(Att_ReportSummaryLeaveYearSickEntity.FieldNames.TeamCode);
            tb.Columns.Add(Att_ReportSummaryLeaveYearSickEntity.FieldNames.SectionCode);
            tb.Columns.Add(Att_ReportSummaryLeaveYearSickEntity.FieldNames.OrgName);
            tb.Columns.Add(Att_ReportSummaryLeaveYearSickEntity.FieldNames.TeamName);
            tb.Columns.Add(Att_ReportSummaryLeaveYearSickEntity.FieldNames.SectionName);
            tb.Columns.Add(Att_ReportSummaryLeaveYearSickEntity.FieldNames.BranchName);
            tb.Columns.Add(Att_ReportSummaryLeaveYearSickEntity.FieldNames.UserExport);
            tb.Columns.Add(Att_ReportSummaryLeaveYearSickEntity.FieldNames.DateExport);
            return tb;
        }






        /// <summary>
        /// Lấy Dữ Liệu BC Thống Kê Ngày Nghỉ Ốm Năm
        /// </summary>
        /// <returns></returns>
        //public List<Att_ReportSummaryLeaveYearSickEntity> GetReportSummaryLeaveYearSick(DateTime dateYear, List<Guid> lstProfileIDs)
        public DataTable GetReportSummaryLeaveYearSick(int year, List<Hre_ProfileEntity> profiles, Guid[] leavedayTypeIds, bool isNotAllowZero, string userExport)
        {
            using (var context = new VnrHrmDataContext())
            {
                DataTable table = CreateReportSummaryLeaveYearSick();

                bool isIncludeQuitEmp = false;
                // leavedayTypeIds = leavedayTypeIds.Where(p => p != null).ToArray();
                List<Att_ReportSummaryLeaveYearSickEntity> listReportSummaryLeaveYearSick = new List<Att_ReportSummaryLeaveYearSickEntity>();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                DateTime dateStart = new DateTime(year, 1, 1);
                DateTime dateEnd = new DateTime(year, 12, DateTime.DaysInMonth(year, 12));
                string key = OverTimeStatus.E_APPROVED.ToString();
                var repoCat_LeaveDayType = new Cat_LeaveDayTypeRepository(unitOfWork);
                var repoAtt_LeaveDay = new Att_LeavedayRepository(unitOfWork);

                var leavedayTypes = repoCat_LeaveDayType
                    .FindBy(s => s.CodeStatistic == "P" || s.CodeStatistic == "SC" && s.IsDelete == null)
                    .Select(s => new { s.ID, s.CodeStatistic, s.PaidRate })
                    .Distinct()
                    .ToList();
                var leadayTypeIDs = leavedayTypes.Select(s => s.ID).ToList();
                var leaveDays = new List<Att_LeaveDay>().Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd }).ToList();

                leaveDays = repoAtt_LeaveDay.FindBy(s => s.IsDelete == null && leadayTypeIDs.Contains(s.LeaveDayTypeID)
                    && s.LeaveHours > 0 && dateStart <= s.DateEnd && s.DateStart <= dateEnd)
                    .Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd }).ToList();
                var profileIds = leaveDays.Select(s => s.ProfileID).Distinct().ToList();
                //if (lstProfileIDs != null && lstProfileIDs.Count > 0)
                //{
                //    leaveDays = repoAtt_LeaveDay.FindBy(s => leadayTypeIDs.Contains(s.LeaveDayTypeID) && s.LeaveHours > 0 && s.Status == key && dateStart <= s.DateEnd && s.DateStart <= dateEnd && lstProfileIDs.Contains(s.ProfileID))
                //        .Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd }).ToList();
                //}
                //else
                //{
                //    leaveDays = repoAtt_LeaveDay.FindBy(s => leadayTypeIDs.Contains(s.LeaveDayTypeID) && s.LeaveHours > 0 && s.Status == key && dateStart <= s.DateEnd && s.DateStart <= dateEnd)
                //        .Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd }).ToList();
                //}

                //var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                // profiles = repoHre_Profile.FindBy(s => s.IsDelete == null)
                // .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();

                string E_ANNUAL_LEAVE = HRM.Infrastructure.Utilities.EnumDropDown.AnnualLeaveDetailType.E_ANNUAL_LEAVE.ToString();
                string E_SICK_LEAVE = HRM.Infrastructure.Utilities.EnumDropDown.AnnualLeaveDetailType.E_SICK_LEAVE.ToString();
                var repoAtt_AnnualLeaveDetail = new Att_AnnualLeaveDetailRepository(unitOfWork);
                var anualLeaves = repoAtt_AnnualLeaveDetail.FindBy(s => s.ProfileID != null && profileIds.Contains(s.ProfileID.Value)
                    && (s.Type == E_ANNUAL_LEAVE || s.Type == E_SICK_LEAVE) && s.Year == year)
                    .Select(s => new { s.ProfileID, s.Month3, s.Type }).ToList();
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var positions = repoCat_Position.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();
                List<Att_ReportSummaryLeaveYearSickEntity> lstReportSummaryLeaveYearSickEntity = new List<Att_ReportSummaryLeaveYearSickEntity>();
                Att_ReportSummaryLeaveYearSickEntity reportSummaryLeaveYearSickEntity = null;


                if (leavedayTypeIds != null)
                {
                    leaveDays = leaveDays.Where(s => leavedayTypeIds.Contains(s.LeaveDayTypeID)).ToList();
                    profileIds = leaveDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }
                if (!isIncludeQuitEmp)
                {
                    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateStart).ToList();
                }
                if (isNotAllowZero)
                {
                    var profileIDs = leaveDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIDs.Contains(s.ID)).ToList();
                }

                foreach (var profile in profiles)
                {
                    DataRow row = table.NewRow();
                    reportSummaryLeaveYearSickEntity = new Att_ReportSummaryLeaveYearSickEntity();

                    Guid? orgId = profile.OrgStructureID;
                    var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);

                    row[Att_ReportSummaryLeaveYearSickEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    row[Att_ReportSummaryLeaveYearSickEntity.FieldNames.ProfileName] = profile.ProfileName;

                    row[Att_ReportSummaryLeaveYearSickEntity.FieldNames.BranchCode] = orgBranch != null ? orgBranch.Code : string.Empty;
                    row[Att_ReportSummaryLeaveYearSickEntity.FieldNames.DepartmentCode] = orgOrg != null ? orgOrg.Code : string.Empty;
                    row[Att_ReportSummaryLeaveYearSickEntity.FieldNames.TeamCode] = orgTeam != null ? orgTeam.Code : string.Empty;
                    row[Att_ReportSummaryLeaveYearSickEntity.FieldNames.SectionCode] = orgSection != null ? orgSection.Code : string.Empty;

                    row[Att_ReportSummaryLeaveYearSickEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                    row[Att_ReportSummaryLeaveYearSickEntity.FieldNames.OrgName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                    row[Att_ReportSummaryLeaveYearSickEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                    row[Att_ReportSummaryLeaveYearSickEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;

                    //reportSummaryLeaveYearSickEntity.BranchCode = orgBranch != null ? orgBranch.Code : string.Empty;
                    //reportSummaryLeaveYearSickEntity.DepartmentCode = orgOrg != null ? orgOrg.Code : string.Empty;
                    //reportSummaryLeaveYearSickEntity.TeamCode = orgTeam != null ? orgTeam.Code : string.Empty;
                    //reportSummaryLeaveYearSickEntity.SectionCode = orgSection != null ? orgSection.Code : string.Empty;
                    //reportSummaryLeaveYearSickEntity.ProfileName = profile.ProfileName;
                    //reportSummaryLeaveYearSickEntity.CodeEmp = profile.CodeEmp;
                    var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                    var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);

                    row[Att_ReportSummaryLeaveYearSickEntity.FieldNames.PositionName] = positon != null ? positon.PositionName : string.Empty;
                    row[Att_ReportSummaryLeaveYearSickEntity.FieldNames.JobTitleName] = jobtitle != null ? jobtitle.JobTitleName : string.Empty;
                    row[Att_ReportSummaryLeaveYearSickEntity.FieldNames.UserExport] = userExport;
                    row[Att_ReportSummaryLeaveYearSickEntity.FieldNames.DateExport] = DateTime.Today;
                    //reportSummaryLeaveYearSickEntity.PositionName = positon != null ? positon.PositionName : string.Empty;
                    //reportSummaryLeaveYearSickEntity.JobtitleName = jobtitle != null ? jobtitle.JobTitleName : string.Empty;
                    //reportSummaryLeaveYearSickEntity.DateExport = DateTime.Now;

                    var anual = anualLeaves.FirstOrDefault(s => s.ProfileID == profile.ID && s.Type == E_ANNUAL_LEAVE);
                    var anualSick = anualLeaves.FirstOrDefault(s => s.ProfileID == profile.ID && s.Type == E_SICK_LEAVE);
                    row[Att_ReportSummaryLeaveYearSickEntity.FieldNames.TotalAllowYear] = anual != null ? anual.Month3 : 0;
                    row[Att_ReportSummaryLeaveYearSickEntity.FieldNames.TotalAllowSick] = anualSick != null ? anualSick.Month3 : 0;

                    //reportSummaryLeaveYearSickEntity.TotalAllowYear = anual != null ? anual.Month3 : 0;
                    //reportSummaryLeaveYearSickEntity.TotalAllowSick = anualSick != null ? anualSick.Month3 : 0;
                    for (int i = 1; i <= 12; i++)
                    {
                        dateStart = new DateTime(year, i, 1);
                        dateEnd = new DateTime(year, i, DateTime.DaysInMonth(year, i));
                        reportSummaryLeaveYearSickEntity.DateFrom = dateStart;
                        reportSummaryLeaveYearSickEntity.DateTo = dateEnd;
                        foreach (var leaday in leavedayTypes)
                        {
                            // Dictionary<string, double> leavedayKeyValue = new Dictionary<string, double>();
                            // var strCodeStatistic = leaday.CodeStatistic + i;
                            //  double leaveDayHours = leaveDays.Where(s => dateStart <= s.DateEnd && s.DateStart <= dateEnd && s.ProfileID == profile.ID && s.LeaveDayTypeID == leaday.ID).Sum(s => s.LeaveHours / 8);
                            var sum = leaveDays.Where(s => dateStart <= s.DateEnd && s.DateStart <= dateEnd && s.ProfileID == profile.ID && s.LeaveDayTypeID == leaday.ID && s.LeaveHours.HasValue).Sum(s => s.LeaveHours.Value / 8);
                            //if (leaday != null && !string.IsNullOrEmpty(leaday.CodeStatistic) && table.Columns.Contains(strCodeStatistic))
                            //{
                            //row[leaday.CodeStatistic + i] = leaveDayHours > 0 ? leaveDayHours : (double?)null;
                            row[leaday.CodeStatistic + i] = sum > 0 ? (object)sum : (double?)null;
                            //}
                            //    reportSummaryLeaveYearSickEntity.P  = leaveDayHours > 0 ? leaveDayHours : 0;

                        }
                    }
                    table.Rows.Add(row);
                    //listReportSummaryLeaveYearSick.Add(reportSummaryLeaveYearSickEntity);
                }
                var configs = new Dictionary<string, Dictionary<string, object>>();
                var config = new Dictionary<string, object>();
                var confighidden = new Dictionary<string, object>();
                var config90 = new Dictionary<string, object>();
                config90.Add("width", 90);

                var config110 = new Dictionary<string, object>();
                config110.Add("width", 110);
                configs.Add("CodeEmp", config90);
                configs.Add("BranchCode", config90);
                configs.Add("DepartmentCode", config90);
                configs.Add("TeamCode", config90);
                configs.Add("SectionCode", config90);
                configs.Add("BranchName", config110);
                configs.Add("OrgName", config110);
                configs.Add("TeamName", config110);
                configs.Add("SectionName", config110);
                configs.Add("PositionName", config110);
                configs.Add("JobTitleName", config110);

                confighidden.Add("hidden", true);
                configs.Add("UserExport", confighidden);
                configs.Add("DateExport", confighidden);
                config.Add("Width", 200);
                //for (int i = 1; i <= 12; i++)
                //{
                //    config = new Dictionary<string, object>();
                //    configs.Add("P" + i, confighidden);
                //    configs.Add("SC" + i, confighidden);
                //    config.Add("Width", 30);
                //}
                config.Add("locked", true);
                configs.Add("WorkDay", config);
                return table.ConfigTable(configs);
            }



            #region code cũ
            //using (var context = new VnrHrmDataContext())
            //{
            //    List<Att_ReportSummaryLeaveYearSickEntity> listReportSummaryLeaveYearSick = new List<Att_ReportSummaryLeaveYearSickEntity>();
            //    var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
            //    //int year = dateYear.Year;
            //    DateTime dateStart = new DateTime(year, 1, 1);
            //    DateTime dateEnd = new DateTime(year, 12, DateTime.DaysInMonth(year, 12));
            //    string key = OverTimeStatus.E_APPROVED.ToString();
            //    var repoCat_LeaveDayType = new Cat_LeaveDayTypeRepository(unitOfWork);
            //    var leavedayTypes = repoCat_LeaveDayType
            //        .FindBy(s => s.CodeStatistic == "P" || s.CodeStatistic == "SC")
            //        .Select(s => new { s.ID, s.CodeStatistic, s.PaidRate })
            //        .Distinct()
            //        .ToList();
            //    var leadayTypeIDs = leavedayTypes.Select(s => s.ID).ToList();
            //    var repoAtt_LeaveDay = new Att_LeavedayRepository(unitOfWork);
            //    var leaveDays = new List<Att_LeaveDay>().Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd }).ToList();
            //    if (lstProfileIDs != null && lstProfileIDs.Count > 0)
            //    {
            //        leaveDays = repoAtt_LeaveDay.FindBy(s => leadayTypeIDs.Contains(s.LeaveDayTypeID) && s.LeaveHours > 0 && s.Status == key && dateStart <= s.DateEnd && s.DateStart <= dateEnd && lstProfileIDs.Contains(s.ProfileID))
            //            .Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd }).ToList();
            //    }
            //    else
            //    {
            //        leaveDays = repoAtt_LeaveDay.FindBy(s => leadayTypeIDs.Contains(s.LeaveDayTypeID) && s.LeaveHours > 0 && s.Status == key && dateStart <= s.DateEnd && s.DateStart <= dateEnd)
            //            .Select(s => new { s.ProfileID, s.LeaveDayTypeID, s.LeaveHours, s.DateStart, s.DateEnd }).ToList();
            //    }
            //    var profileIds = leaveDays.Select(s => s.ProfileID).Distinct().ToList();
            //    var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
            //    var profiles = repoHre_Profile.FindBy(s => profileIds.Contains(s.ID))
            //     .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
            //    string E_ANNUAL_LEAVE = HRM.Infrastructure.Utilities.EnumDropDown.AnnualLeaveDetailType.E_ANNUAL_LEAVE.ToString();
            //    string E_SICK_LEAVE = HRM.Infrastructure.Utilities.EnumDropDown.AnnualLeaveDetailType.E_SICK_LEAVE.ToString();
            //    var repoAtt_AnnualLeaveDetail = new Att_AnnualLeaveDetailRepository(unitOfWork);
            //    var anualLeaves = repoAtt_AnnualLeaveDetail.FindBy(s => s.ProfileID != null && profileIds.Contains(s.ProfileID.Value) && (s.Type == E_ANNUAL_LEAVE || s.Type == E_SICK_LEAVE) && s.Year == year)
            //                    .Select(s => new { s.ProfileID, s.Month3, s.Type }).ToList();
            //    var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
            //    var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
            //    var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
            //    var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
            //    var repoCat_Position = new Cat_PositionRepository(unitOfWork);
            //    var positions = repoCat_Position.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PositionName }).ToList();
            //    var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
            //    var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.JobTitleName }).ToList();
            //    List<Att_ReportSummaryLeaveYearSickEntity> lstReportSummaryLeaveYearSickEntity = new List<Att_ReportSummaryLeaveYearSickEntity>();
            //    Att_ReportSummaryLeaveYearSickEntity reportSummaryLeaveYearSickEntity = null;

            //    foreach (var profile in profiles)
            //    {
            //        reportSummaryLeaveYearSickEntity = new Att_ReportSummaryLeaveYearSickEntity();
            //        Guid? orgId = profile.OrgStructureID;
            //        var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
            //        var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
            //        var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
            //        var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
            //        var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
            //        reportSummaryLeaveYearSickEntity.BranchCode = orgBranch != null ? orgBranch.Code : string.Empty;
            //        reportSummaryLeaveYearSickEntity.DepartmentCode = orgOrg != null ? orgOrg.Code : string.Empty;
            //        reportSummaryLeaveYearSickEntity.TeamCode = orgTeam != null ? orgTeam.Code : string.Empty;
            //        reportSummaryLeaveYearSickEntity.SectionCode = orgSection != null ? orgSection.Code : string.Empty;
            //        var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
            //        var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
            //        reportSummaryLeaveYearSickEntity.ProfileName = profile.ProfileName;
            //        reportSummaryLeaveYearSickEntity.CodeEmp = profile.CodeEmp;
            //        reportSummaryLeaveYearSickEntity.PositionName = positon != null ? positon.PositionName : string.Empty;
            //        reportSummaryLeaveYearSickEntity.JobtitleName = jobtitle != null ? jobtitle.JobTitleName : string.Empty;
            //        reportSummaryLeaveYearSickEntity.DateExport = DateTime.Now;
            //        var anual = anualLeaves.FirstOrDefault(s => s.ProfileID == profile.ID && s.Type == E_ANNUAL_LEAVE);
            //        var anualSick = anualLeaves.FirstOrDefault(s => s.ProfileID == profile.ID && s.Type == E_SICK_LEAVE);
            //        reportSummaryLeaveYearSickEntity.TotalAllowYear = anual != null ? anual.Month3 : 0;
            //        reportSummaryLeaveYearSickEntity.TotalAllowSick = anualSick != null ? anualSick.Month3 : 0;
            //        for (int i = 1; i <= 12; i++)
            //        {
            //            dateStart = new DateTime(year, i, 1);
            //            dateEnd = new DateTime(year, i, DateTime.DaysInMonth(year, i));
            //            reportSummaryLeaveYearSickEntity.DateFrom = dateStart;
            //            reportSummaryLeaveYearSickEntity.DateTo = dateEnd;
            //            foreach (var leaday in leavedayTypes)
            //            {
            //                Dictionary<string, double> leavedayKeyValue = new Dictionary<string, double>();
            //                double leaveDayHours = leaveDays.Where(s => dateStart <= s.DateEnd && s.DateStart <= dateEnd && s.ProfileID == profile.ID && s.LeaveDayTypeID == leaday.ID).Sum(s => s.LeaveHours / 8);

            //            }
            //        }
            //        listReportSummaryLeaveYearSick.Add(reportSummaryLeaveYearSickEntity);
            //    }

            //    return listReportSummaryLeaveYearSick;
            //}
            #endregion
        }

        /// <summary>
        /// [Kiet.Chung] 4/11/2014
        /// BC In Out Đã Bị Điều Chỉnh
        /// </summary>
        public List<Att_ReportInOutAdjustmentEntity> GetReportInOutAdjustment(DateTime dateStart, DateTime dateEnd, String TAMScanReasonMissIDs, String OrgStructureIDs, String CodeEmp, String PayrollGroupIDs, string userPrint)
        {
            using (var context = new VnrHrmDataContext())
            {
                var baseService = new BaseService();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Workday = new Att_WorkDayRepository(unitOfWork);
                var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var repoCat_OrgStructureType = new Cat_OrgStructureTypeRepository(unitOfWork);
                var repoCat_TAMScanReasonMiss = new CustomBaseRepository<Cat_TAMScanReasonMiss>(unitOfWork);
                var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);

                List<Cat_OrgStructure> lstOrgAll = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();

                //var LstProfileQuery = repoHre_Profile.FindBy(s => s.IsDelete == null);


                //List<Guid> lstprofileIDs = LstProfileQuery.Select(m => m.ID).ToList<Guid>();
                string E_APPROVED = WorkdayStatus.E_APPROVED.ToString();

                var lstWorkdayQuery = repoAtt_Workday.FindBy(m => m.Status == E_APPROVED
                    && m.WorkDate >= dateStart
                    && m.WorkDate <= dateEnd
                    && (m.InTime1 != m.InTimeRoot
                        || m.OutTime1 != m.OutTimeRoot
                        || (m.InTime1 != null && m.InTimeRoot == null)
                        || (m.OutTime1 != null && m.OutTimeRoot == null)));
                //int? NumLimitContains = Common.NumLimitContains;
                //if (NumLimitContains == null || lstprofileIDs.Count < NumLimitContains.Value)
                //{
                if (!string.IsNullOrEmpty(OrgStructureIDs))
                {
                    var lstOrderNumber = OrgStructureIDs.Split(',').Select(s => int.Parse(s)).Distinct().ToList();
                    List<Guid> lstOrgS = lstOrgAll.Where(s => lstOrderNumber.Contains(s.OrderNumber)).Select(s => s.ID).ToList();
                    lstWorkdayQuery = lstWorkdayQuery.Where(m => m.OrgStructureID != null && lstOrgS.Contains(m.OrgStructureID.Value));
                }
                if (!string.IsNullOrEmpty(PayrollGroupIDs))
                {
                    var lstPayroll = PayrollGroupIDs.Split(',').Select(s => Guid.Parse(s)).ToList();
                    lstWorkdayQuery = lstWorkdayQuery.Where(m => m.Hre_Profile.PayrollGroupID != null && lstPayroll.Contains(m.Hre_Profile.PayrollGroupID.Value));
                }

                if (!string.IsNullOrEmpty(CodeEmp))
                {
                    lstWorkdayQuery = lstWorkdayQuery.Where(s => s.Hre_Profile.CodeEmp.Contains(CodeEmp));
                }
                //lstWorkdayQuery = lstWorkdayQuery.Where(m => lstprofileIDs.Contains(m.ProfileID));
                List<Att_Workday> lstWorkday = lstWorkdayQuery.ToList<Att_Workday>();
                if (!string.IsNullOrEmpty(TAMScanReasonMissIDs))
                {
                    var lstReasonMiss = TAMScanReasonMissIDs.Split(',').Select(s => Guid.Parse(s)).ToList();
                    lstWorkday = lstWorkday.Where(s => s.MissInOutReason.HasValue && lstReasonMiss.Contains(s.MissInOutReason.Value)).ToList();
                }


                #region ConvertToExportData

                #endregion

                List<Att_ReportInOutAdjustmentEntity> dt = new List<Att_ReportInOutAdjustmentEntity>();
                if (lstWorkday == null || lstWorkday.Count == 0)
                {
                    return dt;
                }

                List<Guid> lstProfileId = lstWorkday.Select(m => m.ProfileID).Distinct().ToList();
                var lstProfile = repoHre_Profile.FindBy(m => lstProfileId.Contains(m.ID)).Select(m => new { m.ID, m.ProfileName, m.CodeEmp, m.OrgStructureID, m.Cat_Position.Code, m.Cat_Position.PositionName }).ToList();
                //string UserLogin = EntityService.CreateQueryable<Sys_UserInfo>(false, GuidContext, Guid.Empty, m => m.ID == LoginUserID).Select(m => m.UserLogin).FirstOrDefault();
                var lstShift = repoCat_Shift.FindBy(s => s.Code != null).Select(m => new { m.ID, m.ShiftName, m.Code }).ToList();

                List<Guid> lstOrgID = lstWorkday.Where(m => m.OrgStructureID != null).Select(m => m.OrgStructureID.Value).Distinct().ToList<Guid>();
                Dictionary<Guid, OrgNameClass> DicOrgFull = (new Hre_ProfileServices()).GetOrgFullLink(lstOrgID, lstOrgAll);
                var lstReasonMissInout = repoCat_TAMScanReasonMiss.FindBy(s => s.Code != null).Select(m => new { m.ID, m.TAMScanReasonMissName }).ToList();
                foreach (var item in lstWorkday)
                {
                    Att_ReportInOutAdjustmentEntity dr = new Att_ReportInOutAdjustmentEntity();
                    var Profile = lstProfile.Where(m => m.ID == item.ProfileID).FirstOrDefault();
                    if (Profile != null)
                    {
                        dr.ProfileName = Profile.ProfileName;
                        dr.CodeEmp = Profile.CodeEmp;

                        if (item.OrgStructureID != null)
                        {
                            var Org = lstOrgAll.Where(m => m.ID == item.OrgStructureID).FirstOrDefault();
                            if (Org != null)
                            {
                                dr.ProfileOrgCode = Org.Code;
                                dr.ProfileOrgName = Org.OrgStructureName;
                            }

                            if (DicOrgFull.ContainsKey(item.OrgStructureID.Value))
                            {
                                OrgNameClass OrgName = DicOrgFull[item.OrgStructureID.Value];
                                if (OrgName != null)
                                {
                                    dr.DepartmentCode = OrgName.DepartmentCode;
                                    dr.SectionCode = OrgName.SectionCode;
                                    dr.BrandCode = OrgName.BrandCode;
                                    dr.TeamCode = OrgName.TeamCode;
                                    dr.DepartmentName = OrgName.DepartmentName;
                                    dr.SectionName = OrgName.SectionName;
                                    dr.BrandName = OrgName.BrandName;
                                    dr.TeamName = OrgName.TeamName;
                                }
                            }
                        }
                        dr.PositionCode = Profile.Code;
                        dr.PositionName = Profile.PositionName;
                    }

                    var shift = lstShift.Where(m => m.ID == item.ShiftID).FirstOrDefault();
                    if (shift != null)
                    {
                        dr.ShiftName = shift.ShiftName;
                    }
                    var shiftActual = lstShift.Where(m => m.ID == item.ShiftActual).FirstOrDefault();
                    if (shiftActual != null)
                    {
                        dr.ShiftNameActual = shiftActual.ShiftName;
                    }
                    if (item.WorkDate != null)
                    {
                        dr.WorkDate = item.WorkDate;
                    }
                    var shiftApprove = lstShift.Where(m => m.ID == item.ShiftApprove).FirstOrDefault();
                    if (shiftApprove != null)
                    {
                        dr.ShiftNameApprove = shiftApprove.ShiftName;
                    }
                    if (item.InTime1 != null)
                    {
                        dr.InTime1 = item.InTime1;
                    }
                    if (item.OutTime1 != null)
                    {
                        dr.OutTime1 = item.OutTime1;
                    }
                    dr.Type = item.Type;
                    dr.SrcType = item.SrcType;
                    dr.Status = item.Status;
                    dr.Note = item.Note;
                    if (item.LateDuration1 != null)
                    {
                        dr.LateDuration1 = item.LateDuration1;
                    }
                    if (item.EarlyDuration1 != null)
                    {
                        dr.EarlyDuration1 = item.EarlyDuration1;
                    }
                    if (item.LateEarlyDuration != null)
                    {
                        dr.LateEarlyDuration = item.LateEarlyDuration;
                    }
                    if (item.LateEarlyRoot != null)
                    {
                        dr.LateEarlyRoot = item.LateEarlyRoot;
                    }
                    if (item.MissInOutReason != null)
                    {
                        var reason = lstReasonMissInout.Where(m => m.ID == item.MissInOutReason.Value).FirstOrDefault();
                        if (reason != null)
                        {
                            dr.MissInOutReason = reason.TAMScanReasonMissName;
                        }
                    }

                    dr.LateEarlyReason = item.LateEarlyReason;
                    if (userPrint != null)
                    {
                        dr.UserExport = userPrint;
                    }
                    dr.DateExport = DateTime.Today;
                    if (item.InTimeRoot != null)
                        dr.InTimeRoot = item.InTimeRoot.Value;
                    if (item.OutTimeRoot != null)
                        dr.OutTimeRoot = item.OutTimeRoot.Value;
                    if (item.LateEarlyDuration != null)
                    {
                        if (item.LateEarlyDuration.Value < 120)
                        {
                            dr.EarlyLateLess2h = "X";
                        }
                        else
                        {
                            dr.EarlyLateOver2h = "X";
                        }
                    }
                    else
                    {
                        dr.EarlyLateLess2h = "X";
                    }

                    dr.DateFrom = dateStart;
                    dr.DateTo = dateEnd;
                    dt.Add(dr);
                }
                return dt;

                //var lstWorkdayQuery = repoAtt_Workday.FindBy(x => x.Status == WorkdayStatus.E_APPROVED.ToString() && x.WorkDate >= DateFrom && x.WorkDate <= DateTo &&
                //    (x.InTime1 != x.InTimeRoot || x.OutTime1 != x.OutTimeRoot || (x.InTime1 != null && x.InTimeRoot == null) || (x.OutTime1 != null && x.OutTimeRoot == null)));
                //List<Guid> lstprofileIDs = new List<Guid>();
                //if (!string.IsNullOrEmpty(OrgStructureIDs) || !string.IsNullOrEmpty(PayrollGroupIDs) || !string.IsNullOrEmpty(CodeEmp))
                //{
                //    if (PayrollGroupIDs == null)
                //        PayrollGroupIDs = "";
                //    var LstProfileQuery = repoHre_Profile.FindBy(x => x.PayrollGroupID.HasValue && PayrollGroupIDs.Contains(x.PayrollGroupID.Value.ToString()));
                //    if (!string.IsNullOrEmpty(OrgStructureIDs))
                //        LstProfileQuery = repoHre_Profile.FindBy(x => x.OrgStructureID.HasValue && OrgStructureIDs.Contains(x.OrgStructureID.Value.ToString()));
                //    if (!string.IsNullOrEmpty(CodeEmp))
                //        LstProfileQuery = repoHre_Profile.FindBy(x => x.CodeEmp == CodeEmp);
                //    lstprofileIDs = LstProfileQuery.Select(m => m.ID).ToList<Guid>();
                //}
                //if (lstWorkdayQuery != null && lstprofileIDs.Count > 0)
                //    lstWorkdayQuery = repoAtt_Workday.FindBy(x => lstprofileIDs.Contains(x.ProfileID));
                //List<Att_Workday> lstWorkday = lstWorkdayQuery.ToList();
                ////if (lstWorkday == null)
                ////    return null;
                ////return lstWorkday.Translate<Att_WorkdayEntity>();

                //if (lstWorkday == null)
                //    return null;

                //var profileIds = lstWorkday.Select(s => s.ProfileID).Distinct().ToList();
                //string strIDs = string.Join(",", profileIds.ToArray());
                //List<object> lstObj = new List<object>();
                //lstObj.Add(strIDs);
                //string status = "";
                //var lstProfile = baseService.GetData<Hre_ProfileEntity>(Common.DotNetToOracle(String.Join(",", profileIds)), ConstantSql.hrm_hr_sp_get_ProfileByIds, ref status);
                //lstObj = new List<object>(4);
                //List<object> paracatshift = new List<object>();
                //paracatshift.AddRange(new object[4]);
                //paracatshift[2] = 1;
                //paracatshift[3] = 10000000;
                ////var lstShift = baseService.GetData<Cat_ShiftEntity>(lstObj, ConstantSql.hrm_cat_sp_get_Shift, ref status);
                //var lstShift = baseService.GetData<Cat_ShiftEntity>(paracatshift, ConstantSql.hrm_cat_sp_get_Shift, ref status);
                //var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                //var orgTypes = repoCat_OrgStructureType.FindBy(s => s.IsDelete == null).ToList();
                //List<Att_ReportInOutAdjustmentEntity> lstResult = new List<Att_ReportInOutAdjustmentEntity>();
                //var lstReasonMissInout = repoCat_TAMScanReasonMiss.FindBy(s => s.IsDelete == null).Select(s => new { s.ID, s.TAMScanReasonMissName }).ToList();
                //foreach (var item in lstWorkday)
                //{
                //    Att_ReportInOutAdjustmentEntity objResult = new Att_ReportInOutAdjustmentEntity();
                //    var Profile = lstProfile.Where(m => m.ID == item.ProfileID).FirstOrDefault();
                //    Guid? orgId = Profile.OrgStructureID;
                //    var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                //    var orgGroup = LibraryService.GetNearestParent(orgId, OrgUnit.E_GROUP, orgs, orgTypes);
                //    var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                //    var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                //    var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);

                //    objResult.CodeEmp = Profile.CodeEmp;
                //    objResult.ProfileName = Profile.ProfileName;
                //    if (lstShift != null)
                //    {
                //        var shift = lstShift.Where(m => m.ID == item.ShiftID).FirstOrDefault();
                //        if (shift != null)
                //            objResult.ShiftName = shift.ShiftName;
                //        var shiftActual = lstShift.Where(m => m.ID == item.ShiftActual).FirstOrDefault();
                //        if (shiftActual != null)
                //            objResult.ShiftNameActual = shiftActual.ShiftName;
                //        if (item.WorkDate != null)
                //            objResult.WorkDate = item.WorkDate;
                //        var shiftApprove = lstShift.Where(m => m.ID == item.ShiftApprove).FirstOrDefault();
                //        if (shiftApprove != null)
                //            objResult.ShiftNameApprove = shiftApprove.ShiftName;
                //    }
                //    if (item.InTime1 != null)
                //        objResult.InTime1 = item.InTime1;
                //    if (item.OutTime1 != null)
                //        objResult.OutTime1 = item.OutTime1;
                //    objResult.InTimeRoot = item.InTimeRoot;
                //    objResult.OutTimeRoot = item.OutTimeRoot;
                //    objResult.WorkDate = item.WorkDate;
                //    objResult.Type = item.Type;
                //    objResult.SrcType = item.SrcType;
                //    objResult.Status = item.Status;
                //    objResult.Note = item.Note;
                //    objResult.LateDuration1 = item.LateDuration1;
                //    objResult.EarlyDuration1 = item.EarlyDuration1;
                //    objResult.LateEarlyDuration = item.LateEarlyDuration;
                //    objResult.LateEarlyRoot = item.LateEarlyRoot;
                //    objResult.LateEarlyReason = item.LateEarlyReason;

                //    objResult.BrandCode = orgBranch != null ? orgBranch.Code : string.Empty;
                //    objResult.DepartmentCode = orgOrg != null ? orgOrg.Code : string.Empty;
                //    objResult.TeamCode = orgTeam != null ? orgTeam.Code : string.Empty;
                //    objResult.SectionCode = orgSection != null ? orgSection.Code : string.Empty;
                //    var Org = orgs.Where(d => d.ID == Profile.OrgStructureID).FirstOrDefault();
                //    if (Org != null)
                //    {
                //        objResult.ProfileOrgCode = Org.Code;
                //    }
                //    if (item.LateEarlyDuration != null)
                //    {
                //        if (item.LateEarlyDuration.Value < 2)
                //        {
                //            objResult.EarlyLateLess2h = "X";
                //        }
                //        else
                //        {
                //            objResult.EarlyLateOver2h = "X";
                //        }
                //    }
                //    else
                //    {
                //        objResult.EarlyLateLess2h = "X";
                //    }
                //    if (item.MissInOutReason != null)
                //    {
                //        var reason = lstReasonMissInout.Where(m => m.ID == item.MissInOutReason.Value).FirstOrDefault();
                //        if (reason != null)
                //        {
                //            objResult.MissInOutReason = reason.TAMScanReasonMissName;
                //        }
                //    }
                //    objResult.DateFrom = DateFrom;
                //    objResult.DateTo = DateTo;
                //    objResult.DatePrint = DateTime.Today;
                //    objResult.UserPrint = userPrint;
                //    lstResult.Add(objResult);
                //}
                //return lstResult;
            }
        }

        #region Hàm Lấy Phòng Ban theo đệ quy
        //Biến để get orderNumber của phòng ban
        string orderNumber = string.Empty;
        //Hàm đệ quy để lấy phòng ban
        public void getChildOrgStructure(List<Cat_OrgStructure> source, Guid idParent, Guid? orgTypeID)
        {
            var child = source.Where(m => m.ParentID == idParent && m.OrgStructureTypeID == orgTypeID).ToList();
            if (child.Count <= 0)
                return;
            else
            {
                for (int i = 0; i < child.Count; i++)
                {
                    orderNumber += child[i].OrderNumber.ToString() + ",";
                    getChildOrgStructure(source, child[i].ID, orgTypeID);
                }
            }
        }

        public void getChildOrgStructure(List<Cat_OrgStructure> source, Guid idParent)
        {
            var child = source.Where(m => m.ParentID == idParent).ToList();
            if (child.Count <= 0)
                return;
            else
            {
                for (int i = 0; i < child.Count; i++)
                {
                    orderNumber += child[i].OrderNumber.ToString() + ",";
                    getChildOrgStructure(source, child[i].ID);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listOrg"></param>
        /// <param name="listOrgType"></param>
        /// <param name="CurrentID"></param>
        /// <returns></returns>
        public List<string> GetParentOrg(List<Cat_OrgStructure> listOrg, List<Cat_OrgStructureType> listOrgType, Guid? CurrentID)
        {
            List<string> result = new List<string>();
            while (true)
            {
                if (CurrentID == null)
                {
                    return result;
                }
                var item = listOrg.FirstOrDefault(m => m.ID == CurrentID);
                if (item != null)
                {
                    result.Add(listOrg.FirstOrDefault(m => m.ID == CurrentID).OrgStructureName);
                    if (item.OrgStructureTypeID != null && listOrgType.Where(m => m.ID == (Guid)item.OrgStructureTypeID).FirstOrDefault().OrgStructureTypeCode != "DEPARTMENT")
                    {
                        if (item.ParentID == null)
                        {
                            return result;
                        }
                        CurrentID = (Guid)item.ParentID;
                    }
                    else
                    {
                        return result;
                    }
                }

            }
        }
        #endregion
        #region BC Phép Năm

        public DataTable CreateSchema_ReportAnnualDetail(DateTime dateStart, DateTime dateEnd, string ReportName)
        {
            DataTable tblData = new DataTable(ReportName);

            tblData.Columns.Add("CodeEmp", typeof(String));
            tblData.Columns.Add("ProfileName", typeof(String));
            tblData.Columns.Add("DateHire", typeof(DateTime));
            tblData.Columns.Add("PositionName", typeof(String));
            tblData.Columns.Add("JobTitleName", typeof(String));
            tblData.Columns.Add("OrgStructureName", typeof(String));
            tblData.Columns.Add("Year", typeof(string));

            for (DateTime date = dateStart.AddMonths(1 - dateStart.Month); date <= dateEnd; date = date.AddMonths(1))
            {
                tblData.Columns.Add(date.ToString("MMM-yyyy"), typeof(String));
            }

            tblData.Columns.Add("AvaliableLeave", typeof(double));
            tblData.Columns.Add("Total_ANL", typeof(double));
            tblData.Columns.Add("TakenLeave", typeof(double));
            tblData.Columns.Add("Remain", typeof(double));
            tblData.Columns.Add("Notes", typeof(string));
            tblData.Columns.Add("WorkingPlace", typeof(string));
            tblData.Columns.Add("InitAvailable", typeof(double));

            return tblData;
        }

        public DataTable ReportAnnualDetail(string strOrgStructure, List<Hre_ProfileEntity> listProfile, int Year, bool IsCreateTemplate, string ReportName, string userLogin)
        {
            DataTable tblData = null;
            DateTime dateStart = new DateTime(Year, 1, 1);
            DateTime dateEnd = dateStart.AddYears(1).AddMinutes(-1);
            if (dateStart != null && dateEnd != null)
            {
                tblData = CreateSchema_ReportAnnualDetail(dateStart, dateEnd, ReportName);
                if (IsCreateTemplate)
                {
                    return tblData.ConfigTable();
                }

                //string rangeSalary = AppConfig.E_RANGE_SALARY_MONTH.ToString();
                string waitStatus = ProfileStatusSyn.E_WAITING.ToString();
                string approvedStatus = RosterStatus.E_APPROVED.ToString();
                DateTime dateFrom = dateStart.AddMonths(-1);
                DateTime dateTo = dateEnd;

                using (var context = new VnrHrmDataContext())
                {
                    string status = string.Empty;

                    var unitOfWork = (IUnitOfWork)new UnitOfWork(context);
                    var repoCat_JobTitle = new CustomBaseRepository<Cat_JobTitle>(unitOfWork);
                    var repoCat_Position = new CustomBaseRepository<Cat_Position>(unitOfWork);
                    var repoCat_WorkPlace = new CustomBaseRepository<Cat_WorkPlace>(unitOfWork);
                    var repoCat_OrgStructure = new CustomBaseRepository<Cat_OrgStructure>(unitOfWork);
                    var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);
                    var repoCat_GradeAttendance = new CustomBaseRepository<Cat_GradeAttendance>(unitOfWork);
                    var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);
                    var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                    var repoAtt_AnnualDetail = new CustomBaseRepository<Att_AnnualDetail>(unitOfWork);

                    #region LoadData

                    //Sys_AppConfig rangeSalaryConfig = EntityService.GetEntityList<Sys_AppConfig>(false,
                    //    GuidContext, LoginUserID, d => d.Info == rangeSalary).FirstOrDefault();

                    var listIsAnnualLeave = repoCat_LeaveDayType.FindBy(d => d.IsAnnualLeave == true).ToList();

                    List<Guid> listIsAnnualLeaveID = listIsAnnualLeave.Select(d => d.ID).ToList();
                    List<Guid> listProfileID = listProfile.Select(d => d.ID).ToList();
                    List<Guid?> listWorkPlaceID = listProfile.Select(d => d.WorkPlaceID).Distinct().ToList();
                    List<Guid> listOrgStructureID = listProfile.Where(d => d.OrgStructureID != null).Select(d => d.OrgStructureID.Value).Distinct().ToList();
                    List<Guid?> listJobTitleID = listProfile.Select(d => d.JobTitleID).Distinct().ToList();
                    List<Guid?> listPositionID = listProfile.Select(d => d.PositionID).Distinct().ToList();


                    var listJobTitle = repoCat_JobTitle.FindBy(d => listJobTitleID.Contains(d.ID)).Select(d => new { d.ID, d.JobTitleName }).ToList();

                    var listPosition = repoCat_Position.FindBy(d => listPositionID.Contains(d.ID)).Select(d => new { d.ID, d.PositionName }).ToList();

                    var listWorkPlace = repoCat_WorkPlace.FindBy(d => listWorkPlaceID.Contains(d.ID)).Select(d => new { d.ID, d.WorkPlaceName }).ToList();

                    var listOrgStructure = repoCat_OrgStructure.FindBy(d => listOrgStructureID.Contains(d.ID)).Select(d => new { d.ID, d.OrgStructureName }).ToList();

                    List<Guid> lstProfileID = listProfile.Select(m => m.ID).Distinct().ToList();

                    var listPara_AnnualDetail = new List<object>();
                    listPara_AnnualDetail.AddRange(new object[3]);
                    listPara_AnnualDetail[0] = strOrgStructure;
                    listPara_AnnualDetail[1] = dateStart;
                    listPara_AnnualDetail[2] = dateEnd;

                    var lstAnnualDetail = GetData<Att_AnnualDetail>(listPara_AnnualDetail, ConstantSql.hrm_att_getdata_AnnualDetail, userLogin, ref status).ToList();

                    if (listProfile.Count > 1000)
                    {
                        lstAnnualDetail = repoAtt_AnnualDetail.FindBy(d => d.IsDelete == null && d.Year == Year && d.ProfileID != null).ToList();
                        lstAnnualDetail = lstAnnualDetail.Where(m => lstProfileID.Contains(m.ProfileID.Value)).ToList();
                    }
                    else
                    {
                        lstAnnualDetail = repoAtt_AnnualDetail.FindBy(d => d.IsDelete == null && d.Year == Year && d.ProfileID != null && lstProfileID.Contains(d.ProfileID.Value)).ToList();
                    }


                    #endregion

                    #region Process
                    foreach (var profile in listProfile)
                    {
                        DataRow dr = tblData.NewRow();
                        dr["CodeEmp"] = profile.CodeEmp;
                        dr["ProfileName"] = profile.ProfileName;
                        dr["WorkingPlace"] = profile.WorkingPlace;
                        dr["OrgStructureName"] = listOrgStructure.Where(d => d.ID == profile.OrgStructureID).Select(d => d.OrgStructureName).FirstOrDefault();
                        //dr[Common.GetObjectName(ClassNames.Cat_WorkPlace)] = listWorkPlace.Where(d => d.ID == profile.WorkPlaceID).Select(d => d.WorkPlaceName).FirstOrDefault();
                        dr["PositionName"] = listPosition.Where(d => d.ID == profile.PositionID).Select(d => d.PositionName).FirstOrDefault();
                        dr["JobTitleName"] = listJobTitle.Where(d => d.ID == profile.JobTitleID).Select(d => d.JobTitleName).FirstOrDefault();
                        dr["Year"] = Year;

                        if (profile.DateHire != null)
                        {
                            dr["DateHire"] = profile.DateHire.Value;
                        }


                        List<Att_AnnualDetail> annualLeaveByProfile = lstAnnualDetail.Where(d => d.ProfileID == profile.ID).ToList();
                        double takenLeaveTotal = 0;
                        if (annualLeaveByProfile != null && annualLeaveByProfile.Count > 0)
                        {

                            for (DateTime month = dateStart; month <= dateEnd.Date; month = month.AddMonths(1))
                            {
                                double leaveInMonth = 0;
                                var annualLeaveByProfile_ByMonth = annualLeaveByProfile.Where(m => m.MonthYear == month).FirstOrDefault();
                                if (annualLeaveByProfile_ByMonth != null)
                                {
                                    leaveInMonth = (annualLeaveByProfile_ByMonth.LeaveInMonth ?? 0) + (annualLeaveByProfile_ByMonth.LeaveInMonthFromInitAvailable ?? 0);
                                }
                                if (tblData.Columns.Contains(month.ToString("MMM-yyyy")))
                                {
                                    dr[month.ToString("MMM-yyyy")] = leaveInMonth;
                                }
                                takenLeaveTotal += leaveInMonth;
                            }

                            var annualLeaveByProfile_Default = annualLeaveByProfile.FirstOrDefault();
                            int MonthBegin = annualLeaveByProfile_Default.MonthBeginInYear ?? 1;
                            int MonthLast = (MonthBegin - 1) < 1 ? 12 : (MonthBegin - 1);

                            DateTime DateMonthLast = new DateTime(dateEnd.Year, MonthLast, 1);

                            //Tinh tong phep nam dc huong trong tung thang. Lay cong thuc trong grade chu khong + theo tung thang.

                            double available = 0;
                            double InitAvailable = 0;
                            double totalAnl = 0;

                            var annualLeaveByProfile_Endyear = annualLeaveByProfile.Where(m => m.MonthYear == DateMonthLast).FirstOrDefault();
                            if (annualLeaveByProfile_Endyear != null)
                            {
                                available = annualLeaveByProfile_Endyear.Available ?? 0;
                                InitAvailable = (annualLeaveByProfile_Endyear != null ? annualLeaveByProfile_Endyear.InitAvailable.Get_Double() : 0);
                            }
                            totalAnl = available + InitAvailable;

                            dr["InitAvailable"] = InitAvailable; //Phép Năm Cũ chuyển qua
                            dr["Total_ANL"] = totalAnl; // Tổng số phép năm được cấp (tính luôn phép năm cũ chuyển qua)
                            dr["AvaliableLeave"] = available; // tổng số phép năm được cấp (không tính phép năm cũ chuyển qua)
                            dr["TakenLeave"] = takenLeaveTotal; //Tổng số phép đã nghĩ
                            dr["Remain"] = totalAnl - takenLeaveTotal; //Số phép còn lại
                        }
                        tblData.Rows.Add(dr);
                    }


                    #endregion

                }
            }
            else
            {
                //UIController.ShowNotify(Messages.MustChooseReportDuration);
            }

            return tblData.ConfigTable();
        }

        #endregion

        private DataTable GetSchemaReportAttendanceByMonthV2(List<string> lstLeaveCode, List<string> lstShiftCode)
        {
            DataTable tb = new DataTable("Att_ReportMonthlyEntity");
            tb.Columns.Add(Att_ReportMonthlyEntity.FieldNames.CodeEmp);
            tb.Columns.Add(Att_ReportMonthlyEntity.FieldNames.ProfileName);
            tb.Columns.Add(Att_ReportMonthlyEntity.FieldNames.DepartmentName);
            tb.Columns.Add(Att_ReportMonthlyEntity.FieldNames.SectionName);
            tb.Columns.Add(Att_ReportMonthlyEntity.FieldNames.PositionName);
            tb.Columns.Add(Att_ReportMonthlyEntity.FieldNames.DateHire, typeof(DateTime));
            tb.Columns.Add(Att_ReportMonthlyEntity.FieldNames.DateQuit, typeof(DateTime));
            for (int i = 0; i <= 31; i++)
            {
                if(!tb.Columns.Contains("Data"+i))
                    tb.Columns.Add("Data" + i, typeof(double));
            }
            tb.Columns.Add(Att_ReportMonthlyEntity.FieldNames.TotalDayHavePaid, typeof(double));
            tb.Columns.Add(Att_ReportMonthlyEntity.FieldNames.RealdayWorking, typeof(double));
            foreach (var code in lstLeaveCode)
            {
                if(!tb.Columns.Contains(code.Trim()))
                    tb.Columns.Add(code.Trim(), typeof(double));
            }
            tb.Columns.Add(Att_ReportMonthlyEntity.FieldNames.LateEarlyTotal, typeof(double));
            tb.Columns.Add(Att_ReportMonthlyEntity.FieldNames.TotalNightShiftHourOver8, typeof(double));
            tb.Columns.Add(Att_ReportMonthlyEntity.FieldNames.TotalNightShiftHourUnder8, typeof(double));
            foreach (var shiftCode in lstShiftCode)
            {
                if (!tb.Columns.Contains("TotalShiftHourOver8" + "_" + shiftCode.Trim()))
                    tb.Columns.Add("TotalShiftHourOver8" + "_" + shiftCode.Trim(), typeof(double));
            }
            tb.Columns.Add(Att_ReportMonthlyEntity.FieldNames.HourOvertimeOver8PerDay, typeof(double));

            return tb;
        }
        public DataTable GetReportAttendanceByMonthV2(List<Guid> lstProfileId, DateTime? Month, bool? _IsCreateTemplate)
        {
            DataTable dt = new DataTable();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var lstAttendance = unitOfWork.CreateQueryable<Att_AttendanceTable>(m => m.MonthYear == Month && lstProfileId.Contains(m.ProfileID))
                    .Select(m => new
                    {
                        m.ID,
                        m.ProfileID,
                        m.StdWorkDayCount,
                        m.LeaveDay1Type,
                        m.LeaveDay1Hours,
                        m.LeaveDay2Type,
                        m.LeaveDay2Hours,
                        m.LeaveDay3Type,
                        m.LeaveDay3Hours,
                        m.LeaveDay4Type,
                        m.LeaveDay4Hours,
                        m.LateEarlyTotal
                    }).ToList();
                var lstAttendanceID = lstAttendance.Select(m => m.ID).ToList();
                var lstAttendanceItem = unitOfWork.CreateQueryable<Att_AttendanceTableItem>(m => lstAttendanceID.Contains(m.AttendanceTableID))
                    .Select(m => new
                    {
                        m.WorkDate,
                        m.ShiftID,
                        m.AttendanceTableID,
                        m.WorkPaidHours,
                        m.OvertimeHours,
                        m.OvertimeTypeID,
                        m.LeaveHours,
                        m.ExtraOvertimeHours,
                        m.ExtraOvertimeHours2,
                        m.ExtraOvertimeHours3,
                        m.ExtraOvertimeTypeID,
                        m.ExtraOvertimeType2ID,
                        m.ExtraOvertimeType3ID
                    }).ToList();

                var lstProfile = unitOfWork.CreateQueryable<Hre_Profile>(m => lstProfileId.Contains(m.ID))
                    .Select(m => new
                    {
                        m.ID,
                        m.ProfileName,
                        m.CodeEmp,
                        m.OrgStructureID,
                        m.DateHire,
                        m.PositionID,
                        m.DateQuit
                    }).ToList();
                var lstOrgID = lstProfile.Where(m => m.OrgStructureID != null).Select(m => m.OrgStructureID.Value).Distinct().ToList();
                var lstOrgAll = unitOfWork.CreateQueryable<Cat_OrgStructure>().ToList();
                var lstPosition = unitOfWork.CreateQueryable<Cat_Position>().Select(m => new { m.ID, m.PositionName }).ToList();
                var lstLeaveType = unitOfWork.CreateQueryable<Cat_LeaveDayType>().Select(m => new { m.ID, m.Code, m.CodeStatistic }).ToList();
                var lstShift = unitOfWork.CreateQueryable<Cat_Shift>().Select(m => new { m.ID, m.IsNightShift, m.Code }).ToList();
                var lstNightShiftID = lstShift.Where(m => m.IsNightShift == true).Select(m => m.ID).ToList();
                dt = GetSchemaReportAttendanceByMonthV2(lstLeaveType.Select(m => m.Code).ToList(), lstShift.Select(m=>m.Code).ToList());
                if (_IsCreateTemplate==true)
                    return dt;
                Dictionary<Guid, OrgNameClass> DicOrgFull = (new Hre_ProfileServices()).GetOrgFullLink(lstOrgID, lstOrgAll);
                foreach (var profileID in lstProfileId)
                {
                    DataRow dr = dt.NewRow();
                    var profile = lstProfile.Where(m => m.ID == profileID).FirstOrDefault();
                    if (profile == null)
                        continue;
                    dr[Att_ReportMonthlyEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                    dr[Att_ReportMonthlyEntity.FieldNames.ProfileName] = profile.ProfileName;
                    Guid? OrgID = profile.OrgStructureID;
                    if (OrgID != null && DicOrgFull.ContainsKey(OrgID.Value))
                    {
                        OrgNameClass OrgName = DicOrgFull[OrgID.Value];
                        if (OrgName != null )
                        {
                            if(OrgName.DepartmentName!=null)
                                dr[Att_ReportMonthlyEntity.FieldNames.DepartmentName] = OrgName.DepartmentName;
                            if (OrgName.SectionName != null)
                                dr[Att_ReportMonthlyEntity.FieldNames.SectionName] = OrgName.SectionName;
                        }
                    }
                    var position = lstPosition.Where(m => m.ID == profile.PositionID).FirstOrDefault();
                    if (position != null && position.PositionName!=null)
                    {
                        dr[Att_ReportMonthlyEntity.FieldNames.PositionName] = position.PositionName;
                    }
                    if (profile.DateHire != null)
                    {
                        dr[Att_ReportMonthlyEntity.FieldNames.DateHire] = profile.DateHire;
                    }
                    if (profile.DateQuit != null)
                    {
                        dr[Att_ReportMonthlyEntity.FieldNames.DateQuit] = profile.DateQuit;
                    }
                    var AttendanceTableByProfile = lstAttendance.Where(m => m.ProfileID == profileID).FirstOrDefault();
                    if (AttendanceTableByProfile != null)
                    {
                        var lstAttendanceTableItemByProfile = lstAttendanceItem.Where(m => m.AttendanceTableID == AttendanceTableByProfile.ID).OrderBy(m => m.WorkDate).ToList();
                        int i = 0;
                        foreach (var item in lstAttendanceTableItemByProfile)
                        {
                            i++;
                            dr["Data" + i] = item.WorkPaidHours;
                        }
                        //Trừ đi ngày nghỉ không lương
                        dr[Att_ReportMonthlyEntity.FieldNames.TotalDayHavePaid] = lstAttendanceTableItemByProfile.Count(m => m.WorkPaidHours > 0);
                        //Công chuẩn trừ đi nghỉ ko luong
                        dr[Att_ReportMonthlyEntity.FieldNames.RealdayWorking] = AttendanceTableByProfile.StdWorkDayCount - lstAttendanceTableItemByProfile.Count(m => m.WorkPaidHours == 0 && m.LeaveHours != null && m.LeaveHours > 0);
                        foreach (var leave in lstLeaveType)
                        {
                            double HourLeave = 0;
                            HourLeave += (AttendanceTableByProfile.LeaveDay1Type != null && AttendanceTableByProfile.LeaveDay1Type == leave.ID) ? AttendanceTableByProfile.LeaveDay1Hours : 0;
                            HourLeave += (AttendanceTableByProfile.LeaveDay2Type != null && AttendanceTableByProfile.LeaveDay2Type == leave.ID) ? AttendanceTableByProfile.LeaveDay2Hours : 0;
                            HourLeave += (AttendanceTableByProfile.LeaveDay3Type != null && AttendanceTableByProfile.LeaveDay3Type == leave.ID) ? AttendanceTableByProfile.LeaveDay3Hours : 0;
                            HourLeave += (AttendanceTableByProfile.LeaveDay4Type != null && AttendanceTableByProfile.LeaveDay4Type == leave.ID) ? AttendanceTableByProfile.LeaveDay4Hours : 0;
                            dr[leave.Code.Trim()] = HourLeave;
                        }
                        if (AttendanceTableByProfile.LateEarlyTotal!=null)
                            dr[Att_ReportMonthlyEntity.FieldNames.LateEarlyTotal] = AttendanceTableByProfile.LateEarlyTotal;
                        dr[Att_ReportMonthlyEntity.FieldNames.TotalNightShiftHourOver8] = lstAttendanceTableItemByProfile.Where(m => m.ShiftID != null && lstNightShiftID.Contains(m.ShiftID.Value) && m.WorkPaidHours >= 8).Sum(m => m.WorkPaidHours);
                        dr[Att_ReportMonthlyEntity.FieldNames.TotalNightShiftHourUnder8] = lstAttendanceTableItemByProfile.Where(m => m.ShiftID != null && lstNightShiftID.Contains(m.ShiftID.Value) && m.WorkPaidHours < 8).Sum(m => m.WorkPaidHours);
                        foreach (var shift in lstShift)
                        {
                            dr["TotalShiftHourOver8" + "_" + shift.Code.Trim()] = lstAttendanceTableItemByProfile.Where(m => m.ShiftID != null && m.ShiftID == shift.ID && m.WorkPaidHours >= 8).Sum(m => m.WorkPaidHours);
                        }
                        dr[Att_ReportMonthlyEntity.FieldNames.HourOvertimeOver8PerDay] = lstAttendanceTableItemByProfile.Where(m => (m.OvertimeHours + m.ExtraOvertimeHours + m.ExtraOvertimeHours2 + m.ExtraOvertimeHours3) > 8).Select(m => (m.OvertimeHours + m.ExtraOvertimeHours + m.ExtraOvertimeHours2 + m.ExtraOvertimeHours3)).FirstOrDefault();
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt.ConfigTable();
        }

        public Att_GradeEntity GetGradeByProfileId(Guid ProfileID,DateTime WorkDate)
        {
            List<Guid> lstProfileID = new List<Guid>();
            lstProfileID.Add(ProfileID);
            Att_GradeServices _grateService = new Att_GradeServices();
         
            var templstGrade = Att_GradeServices.getAllGrade(lstProfileID, WorkDate);
            Att_GradeEntity objGrade = new Att_GradeEntity();
            if (templstGrade != null)
            {
                string status = string.Empty;
                List<Att_GradeEntity> lstGrade = templstGrade.Translate<Att_GradeEntity>();
                 objGrade = lstGrade.FirstOrDefault();
            }
            return objGrade;
        }
    }
}
