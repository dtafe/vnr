using HRM.Data.Attendance.Data;
using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
using System.Linq;
using HRM.Business.Attendance.Models;
using System;
using HRM.Business.Category.Models;
using System.Collections.Generic;
using HRM.Infrastructure.Utilities;
using System.Linq.Expressions;
using VnResource.Helper.Linq;
using HRM.Data.Attendance.Data.Sql.Repositories;
using System.Reflection;
using System.Collections;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using HRM.Business.Hr.Models;
using VnResource.Helper.Data;
using System.Threading;
using HRM.Business.Hr.Domain;
using HRM.Business.HrmSystem.Models;


namespace HRM.Business.Attendance.Domain
{
    public class Att_AttendanceServices : BaseService
    {
        #region CreateComputingTask

        public Guid CreateComputingTask(Guid userID, DateTime monthYear)
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                Sys_AsynTask task = new Sys_AsynTask();
                task.ID = Guid.NewGuid();
                task.PercentComplete = 0.01D;
                task.TimeStart = DateTime.Now;
                task.Status = AsynTaskStatus.Doing.ToString();
                task.Type = AsynTask.Attendance_Computing.ToString();
                task.Summary = "Attendance: " + monthYear.ToString("MM/yyyy");
                unitOfWork.AddObject(typeof(Sys_AsynTask), task);
                unitOfWork.SaveChanges(userID);
                return task.ID;
            }
        }

        public void CompleteComputingTask(Guid asynTaskID, Guid userID,
            int totalComputed, int totalProfile, DataErrorCode dataErrorCode)
        {
            #region Lưu Sys_AsynTask khi xử lý xong

            if (asynTaskID != Guid.Empty)
            {
                using (var taskContext = new VnrHrmDataContext())
                {
                    IUnitOfWork taskunitOfWork = new UnitOfWork(taskContext);
                    var asynTask = taskunitOfWork.CreateQueryable<Sys_AsynTask>(s => s.ID == asynTaskID).FirstOrDefault();

                    if (asynTask != null)
                    {
                        asynTask.PercentComplete = 1D;
                        asynTask.TimeEnd = DateTime.Now;
                        asynTask.Status = AsynTaskStatus.Done.ToString();

                        var time = asynTask.TimeEnd.Value.Subtract(asynTask.TimeStart).TotalMinutes;
                        asynTask.Description += " - Result: " + totalComputed + "/" + totalProfile;
                        asynTask.Description += " - Time: " + time.ToString("N2");

                        if (dataErrorCode == DataErrorCode.Locked)
                        {
                            asynTask.PercentComplete = 1D;//Không cần nhân với 100
                            asynTask.Description = "Dữ Liệu Tính Công Đã Bị Khóa";
                        }

                        dataErrorCode = taskunitOfWork.SaveChanges();
                    }
                }
            }

            #endregion
        }

        #endregion

        #region ComputeAttendance

        public DataErrorCode ComputeAttendance(Guid asynTaskID, Guid userID,
            DateTime monthYear, Guid cutOffDurationID, Guid profileID)
        {
            using (var context = new VnrHrmDataContext())
            {
                DataErrorCode dataErrorCode = DataErrorCode.Success;
                IUnitOfWork unitOfWork = new UnitOfWork(context);

                var listProfile = unitOfWork.CreateQueryable<Hre_Profile>(userID,
                    d => d.ID == profileID).Select(d => new Hre_ProfileEntity
                {
                    ID = d.ID,
                    CodeEmp = d.CodeEmp,
                    CodeAttendance = d.CodeAttendance,
                    OrgStructureID = d.OrgStructureID,
                    DateHire = d.DateHire,
                    DateQuit = d.DateQuit
                }).ToArray<Hre_ProfileEntity>();

                ComputeAttendance(asynTaskID, userID, monthYear,
                    cutOffDurationID, out dataErrorCode, listProfile);

                return dataErrorCode;
            }
        }

        public DataErrorCode ComputeAttendance(Guid asynTaskID, Guid userID,
            DateTime monthYear, Guid cutOffDurationID, List<Guid> listOrgStructureID,
            List<Guid> listPayrollGroupID, List<Guid> listWorkplaceID, bool onlyQuitEmp)
        {
            DataErrorCode dataErrorCode = DataErrorCode.Success;
            Hre_ProfileEntity[] listProfile = null;

            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);

                var profileQueryable = unitOfWork.CreateQueryable<Hre_Profile>(userID);

                if (listOrgStructureID != null && listOrgStructureID.Count() > 0)
                {
                    profileQueryable = profileQueryable.Where(d => d.OrgStructureID.HasValue
                        && listOrgStructureID.Contains(d.OrgStructureID.Value));
                }

                if (listPayrollGroupID != null && listPayrollGroupID.Count() > 0)
                {
                    profileQueryable = profileQueryable.Where(d => d.PayrollGroupID.HasValue
                        && listPayrollGroupID.Contains(d.PayrollGroupID.Value));
                }

                if (listWorkplaceID != null && listWorkplaceID.Count() > 0)
                {
                    profileQueryable = profileQueryable.Where(d => d.WorkPlaceID.HasValue
                        && listWorkplaceID.Contains(d.WorkPlaceID.Value));
                }

                if (onlyQuitEmp)
                {
                    DateTime monthEnd = monthYear.AddMonths(1).AddSeconds(-1);
                    profileQueryable = profileQueryable.Where(d => d.DateQuit.HasValue
                        && d.DateQuit <= monthEnd);
                }

                listProfile = profileQueryable.Select(d => new Hre_ProfileEntity
                {
                    ID = d.ID,
                    CodeEmp = d.CodeEmp,
                    CodeAttendance = d.CodeAttendance,
                    OrgStructureID = d.OrgStructureID,
                    DateHire = d.DateHire,
                    DateQuit = d.DateQuit
                }).ToArray<Hre_ProfileEntity>();
            }

            ComputeAttendance(asynTaskID, userID, monthYear,
                cutOffDurationID, out dataErrorCode, listProfile);

            return dataErrorCode;
        }

        private void ComputeAttendance(Guid asynTaskID, Guid userID, DateTime monthYear,
            Guid cutOffDurationID, out DataErrorCode dataErrorCode, params Hre_ProfileEntity[] listProfile)
        {
            try
            {
                int resultCount = 0;
                int pageSize = 500;

                dataErrorCode = DataErrorCode.Success;
                int totalProfile = listProfile.Count();

                #region Xử lý cho những nhân viên được chọn

                //Vinhtran - 20140625: Chia theo pagesize để không bị quá tải ram
                foreach (var listProfileSplit in listProfile.Chunk(pageSize))
                {
                    if (dataErrorCode == DataErrorCode.Success)
                    {
                        resultCount += ComputeAttendance(userID, asynTaskID, monthYear, cutOffDurationID,
                            out dataErrorCode, totalProfile, resultCount, listProfileSplit.ToArray());
                    }
                    else
                    {
                        break;
                    }
                }

                #endregion

                #region Lưu Sys_AsynTask khi xử lý xong

                CompleteComputingTask(asynTaskID, userID,
                    resultCount, totalProfile, dataErrorCode);

                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        private int ComputeAttendance(Guid userID, Guid asynTaskID, DateTime monthYear, Guid cutOffDurationID,
            out DataErrorCode dataErrorCode, int totalProfile, int totalComputed, params Hre_ProfileEntity[] listProfile)
        {
            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                int attendanceTableCount = 0;

                List<Guid> listProfileID = listProfile.Select(s => s.ID).ToList();
                totalProfile = totalProfile <= 0 ? listProfileID.Count() : totalProfile;

                #region Xóa bảng công cũ đã tính trước đó

                dataErrorCode = DeleteAttendance(monthYear,
                    cutOffDurationID, listProfileID);

                if (dataErrorCode == DataErrorCode.Locked
                    || dataErrorCode == DataErrorCode.Error)
                {
                    return attendanceTableCount;
                }

                #endregion

                #region Tính khoảng thời gian theo cutOffDurationID

                DateTime attendanceFrom = monthYear.AddDays(1 - monthYear.Day).Date;
                DateTime attendanceTo = attendanceFrom.AddMonths(1).AddSeconds(-1);

                var cutOffDuration = unitOfWork.CreateQueryable<Att_CutOffDuration>(Guid.Empty,
                    d => d.ID == cutOffDurationID).Select(d => new
                    {
                        d.DateStart,
                        d.DateEnd,
                        d.OvertimeStart,
                        d.OvertimeEnd,
                        d.LeavedayStart,
                        d.LeavedayEnd,
                    }).FirstOrDefault();

                if (cutOffDuration != null)
                {
                    attendanceFrom = cutOffDuration.DateStart;
                    attendanceTo = cutOffDuration.DateEnd;
                }

                #endregion

                #region Tải các thông tin danh mục có liên quan

                string overTimeMethod = MethodOption.E_CASHOUT.ToString();
                string overTimeStatus = OverTimeStatus.E_APPROVED.ToString();
                string pregnancyType = PregnancyType.E_LEAVE_EARLY.ToString();
                string leaveStatus = LeaveDayStatus.E_APPROVED.ToString();
                string hdtJobStatus = HDTJobStatus.E_APPROVE.ToString();
                string leavedayTypeNOPAY = LeavedayTypeCode.NOPAY.ToString();
                string leavedayTypeABS = LeavedayTypeCode.ABS.ToString();
                string leavedayTypeHLD = LeavedayTypeCode.HLD.ToString();
                string rosterStatus = RosterStatus.E_APPROVED.ToString();

                string NOTAUTOREGISTERHOLIDAYLEAVE = AppConfig.HRM_ATT_NOTAUTOREGISTERHOLIDAYLEAVE.ToString();
                string OT_HOLIDAYSCOMPUTE400 = AppConfig.HRM_ATT_OT_HOLIDAYSCOMPUTE400.ToString();
                string MISSTAM_LEAVETYPE = AppConfig.HRM_ATT_MISSTAM_LEAVETYPE.ToString();
                string HRM_ATT_OT_OVERTIMESTATUS = AppConfig.HRM_ATT_OT_OVERTIMESTATUS.ToString();

                var standardWorkdayConfig = unitOfWork.CreateQueryable<Sys_AllSetting>(d => d.Name == NOTAUTOREGISTERHOLIDAYLEAVE
                    || d.Name == OT_HOLIDAYSCOMPUTE400 || d.Name == MISSTAM_LEAVETYPE || d.Name == HRM_ATT_OT_OVERTIMESTATUS).ToList();

                var notAutoRegHolidayLeave = standardWorkdayConfig.Where(s => s.Name == NOTAUTOREGISTERHOLIDAYLEAVE).FirstOrDefault();
                var otholidayscompute400 = standardWorkdayConfig.Where(s => s.Name == OT_HOLIDAYSCOMPUTE400).FirstOrDefault();
                var missTAM_LeaveType = standardWorkdayConfig.Where(s => s.Name == MISSTAM_LEAVETYPE).FirstOrDefault();
                var statusOT = standardWorkdayConfig.Where(s => s.Name == HRM_ATT_OT_OVERTIMESTATUS).FirstOrDefault();

                if (statusOT != null)
                {
                    overTimeStatus = statusOT.Value1;
                }

                DateTime startYear = new DateTime(monthYear.Year, 1, 1).AddMonths(-1);//đầu năm
                DateTime endYear = new DateTime(monthYear.Year, 12, 31).Date.AddDays(1).AddSeconds(-1);
                DateTime preMonthYear = monthYear.AddMonths(-1);

                List<Cat_DayOff> listHoliday = unitOfWork.CreateQueryable<Cat_DayOff>(d =>
                    d.DateOff >= startYear && d.DateOff <= endYear).ToList();

                //Tất cả roster từ đầu năm đến thời điểm tính công - cần cho mục tính tính phép năm còn lại cho từng nhân viên.
                var listRoster = unitOfWork.CreateQueryable<Att_Roster>(d => d.DateStart <= attendanceTo && d.DateEnd >= startYear
                    && d.Status == rosterStatus && listProfileID.Contains(d.ProfileID)).Select(d => new Att_RosterEntity
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

                var listRosterGroup = unitOfWork.CreateQueryable<Att_RosterGroup>(m => m.DateStart != null && m.DateEnd != null
                    && m.DateStart <= attendanceTo && m.DateEnd >= attendanceFrom).Select(d => new Att_RosterGroupEntity
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

                var listWorkHistory = unitOfWork.CreateQueryable<Hre_WorkHistory>(d => d.DateEffective <= attendanceTo
                    && listProfileID.Contains(d.ProfileID)).ToList();

                List<Att_Roster> lstRosterTypeGroup = new List<Att_Roster>();
                List<Att_RosterGroup> lstRosterGroup = new List<Att_RosterGroup>();

                GetRosterGroup(listProfileID, startYear, attendanceTo, out lstRosterTypeGroup, out lstRosterGroup);
                var listLateEarlyRule = unitOfWork.CreateQueryable<Cat_LateEarlyRule>().ToList<Cat_LateEarlyRule>();
                var listShift = unitOfWork.CreateQueryable<Cat_Shift>().ToList<Cat_Shift>();

                var currentYear = unitOfWork.CreateQueryable<Att_AnnualDetail>(d => monthYear == d.MonthYear).Select(d => d.Year).FirstOrDefault();
                var monthStart = unitOfWork.CreateQueryable<Att_AnnualDetail>(d => d.Year == currentYear).OrderBy(d => d.MonthYear).Select(d => d.MonthYear).FirstOrDefault();

                var listAnnualDetail = unitOfWork.CreateQueryable<Att_AnnualDetail>(d => d.MonthYear == monthStart
                    && d.ProfileID.HasValue && listProfileID.Contains(d.ProfileID.Value));

                var listPreAttendanceTable = unitOfWork.CreateQueryable<Att_AttendanceTable>(d => d.MonthYear.HasValue
                    && d.MonthYear == preMonthYear && listProfileID.Contains(d.ProfileID)).Select(d =>
                        new Att_AttendanceTableEntity
                        {
                            ID = d.ID,
                            AnlDayTaken = d.AnlDayTaken,
                            AnlDayAdjacent = d.AnlDayAdjacent,
                            SickDayTaken = d.SickDayTaken,
                            SickDayAdjacent = d.SickDayAdjacent,
                            AnlDayAvailable = d.AnlDayAvailable,
                            SickDayAvailable = d.SickDayAvailable,
                            ProfileID = d.ProfileID
                        }).ToList<Att_AttendanceTableEntity>();

                var listPregnancy = unitOfWork.CreateQueryable<Att_Pregnancy>(d => d.DateEnd >= attendanceFrom && d.DateStart <= attendanceTo
                    && d.Type == pregnancyType && listProfileID.Contains(d.ProfileID)).Select(d => new Att_PregnancyEntity
                    {
                        ID = d.ID,
                        ProfileID = d.ProfileID,
                        DateStart = d.DateStart,
                        DateEnd = d.DateEnd,
                        TypePregnancyEarly = d.TypePregnancyEarly
                    }).ToList<Att_PregnancyEntity>();

                var listOvertime = unitOfWork.CreateQueryable<Att_Overtime>(d => d.Status == overTimeStatus
                    && (d.MethodPayment == null || d.MethodPayment == overTimeMethod) && d.WorkDateRoot >= attendanceFrom
                    && d.WorkDateRoot <= attendanceTo && listProfileID.Contains(d.ProfileID)).Select(d => new Att_OvertimeEntity
                    {
                        ID = d.ID,
                        ProfileID = d.ProfileID,
                        OvertimeTypeID = d.OvertimeTypeID,
                        DurationType = d.DurationType,
                        WorkDateRoot = d.WorkDateRoot,
                        WorkDate = d.WorkDate,
                        ApproveHours = d.ApproveHours,
                        ConfirmHours = d.ConfirmHours,
                        RegisterHours = d.RegisterHours
                    }).ToList<Att_OvertimeEntity>();

                List<Guid> listOvertimeTypeID = listOvertime.Select(o => o.OvertimeTypeID).Distinct().ToList();
                var listOvertimeType = unitOfWork.CreateQueryable<Cat_OvertimeType>(d => listOvertimeTypeID.Contains(d.ID)).ToList();

                var listLeaveDay = unitOfWork.CreateQueryable<Att_LeaveDay>(d => d.Status == leaveStatus && ((d.DateStart <= attendanceTo
                    && d.DateEnd >= attendanceFrom) || d.DateOvertimeOff >= attendanceFrom && d.DateOvertimeOff <= attendanceTo)
                    && listProfileID.Contains(d.ProfileID)).Select(d => new Att_LeaveDayEntity
                    {
                        ID = d.ID,
                        ProfileID = d.ProfileID,
                        DateStart = d.DateStart,
                        DateEnd = d.DateEnd,
                        DateOvertimeOff = d.DateOvertimeOff,
                        LeaveDayTypeID = d.LeaveDayTypeID,
                        DurationType = d.DurationType,
                        LeaveHours = d.LeaveHours,
                        LeaveDays = d.LeaveDays
                    }).ToList<Att_LeaveDayEntity>();

                List<Guid> listLeaveDayTypeID = listLeaveDay.Select(o => o.LeaveDayTypeID).Distinct().ToList();
                var listLeaveDayType = unitOfWork.CreateQueryable<Cat_LeaveDayType>(d => listLeaveDayTypeID.Contains(d.ID)
                    || d.Code == leavedayTypeABS || d.Code == leavedayTypeNOPAY || d.Code == leavedayTypeHLD).ToList();

                List<Hre_HDTJob> listHDTJob = new List<Hre_HDTJob>();
                foreach (var templstProfileIds in listProfileID.Chunk(1000))
                {
                    listHDTJob.AddRange(unitOfWork.CreateQueryable<Hre_HDTJob>(d => d.Status == hdtJobStatus && ((d.DateFrom <= attendanceTo && d.DateTo == null)
                    || (d.DateTo != null && d.DateFrom <= attendanceTo && d.DateTo >= attendanceFrom)) && templstProfileIds.Contains(d.ProfileID.Value)).ToList());
                }
                //var listHDTJob = unitOfWork.CreateQueryable<Hre_HDTJob>(d => d.Status == hdtJobStatus && ((d.DateFrom <= attendanceTo && d.DateTo == null)
                //    || (d.DateTo != null && d.DateFrom <= attendanceTo && d.DateTo >= attendanceFrom)) && listProfileID.Contains(d.ProfileID.Value)).ToList();

                //Danh sách cấu hình grade theo danh sách nhân viên có trong danh sách mã thẻ
                var listGrade = unitOfWork.CreateQueryable<Att_Grade>(d => d.MonthStart.HasValue && d.MonthStart <= attendanceTo
                    && listProfileID.Contains(d.ProfileID.Value)).Select(d => new
                    {
                        d.ProfileID,
                        d.MonthStart,
                        d.GradeAttendanceID
                    }).ToList();

                listGrade = listGrade.GroupBy(d => d.ProfileID).Select(d =>
                    d.OrderByDescending(p => p.MonthStart).FirstOrDefault()).ToList();

                //Danh sách cấu hình gradeCfg theo danh sách grade tìm được ở trên
                var listGradeCfgID = listGrade.Select(d => d.GradeAttendanceID).Distinct().ToList();
                var listGradeCfg = unitOfWork.CreateQueryable<Cat_GradeAttendance>(d => listGradeCfgID.Contains(d.ID)).ToList();

                var lstTimeOffInLieu = unitOfWork.CreateQueryable<Att_TimeOffInLieu>(d => d.Date != null
                    && d.Date >= attendanceFrom && d.Date <= attendanceTo).Select(m => new
                    {
                        m.ProfileID,
                        m.UnusualLeaves,
                        m.TakenLeaves
                    }).ToList();

                DateTime month3Ago = new DateTime(attendanceTo.Year, attendanceTo.Month, 1);
                month3Ago = month3Ago.AddMonths(-3);

                List<Att_TimeOffInLieuMonth> lstTimeOffInLieu_ByMonth_3Month = unitOfWork.CreateQueryable<Att_TimeOffInLieuMonth>(d =>
                    d.Month != null && d.Month >= month3Ago && d.Month <= attendanceTo && listProfileID.Contains(d.ProfileID)).ToList();

                List<Att_TimeOffInLieuMonth> lstTimeOffInLieuMonth = lstTimeOffInLieu_ByMonth_3Month.Where(d =>
                    d.Month != null && d.Month >= attendanceFrom && d.Month <= attendanceTo).ToList<Att_TimeOffInLieuMonth>();

                List<Att_TimeOffInLieuMonth> lstTimeOffInLieuMonthInsert = new List<Att_TimeOffInLieuMonth>();
                DateTime beginYear = new DateTime(attendanceTo.Year, 1, 1);

                #endregion

                #region Bắt đầu tính công cho từng người

                var listWorkDay = unitOfWork.CreateQueryable<Att_Workday>(d => d.WorkDate >= attendanceFrom
                    && d.WorkDate <= attendanceTo && listProfileID.Contains(d.ProfileID)).ToList();

                List<Att_TimeOffInLieu> lstTimeOffInLieu_3Month = unitOfWork.CreateQueryable<Att_TimeOffInLieu>(d =>
                    d.Date >= month3Ago && d.Date <= attendanceTo && listProfileID.Contains(d.ProfileID)).ToList();

                var listTimeOffInLieu = lstTimeOffInLieu_3Month.Where(s => listProfileID.Contains(s.ProfileID)
                    && attendanceFrom <= s.Date && s.Date <= attendanceTo).Select(s => new Att_TimeOffInLieuEntity
                    {
                        ID = s.ID,
                        Date = s.Date,
                        ProfileID = s.ProfileID,
                        OvertimeID = s.OvertimeID,
                        LeaveDayID = s.LeaveDayID,
                        UnusualLeaves = s.UnusualLeaves,
                        LeaveHours = s.LeaveHours,
                        TakenLeaves = s.TakenLeaves
                    }).ToList<Att_TimeOffInLieuEntity>();

                using (var taskContext = new VnrHrmDataContext())
                {
                    IUnitOfWork taskUnitOfWork = new UnitOfWork(taskContext);
                    Sys_AsynTask asynTask = null;

                    if (asynTaskID != Guid.Empty)
                    {
                        asynTask = taskUnitOfWork.CreateQueryable<Sys_AsynTask>(s =>
                           s.ID == asynTaskID).FirstOrDefault();
                    }

                    int totalComputedProfileMustSubmitTask = 50;
                    int totalComputedProfileMustSubmit = 50;
                    int totalComputedProfileForSubmit = 0;
                    int totalComputedProfileForTask = 0;

                    totalComputedProfileMustSubmitTask = totalProfile * 5 / 100;

                    if (totalComputedProfileMustSubmitTask > listProfile.Count())
                    {
                        totalComputedProfileMustSubmitTask = listProfile.Count();
                    }

                    foreach (var profileId in listProfileID)
                    {
                        try
                        {
                            #region Những dữ liệu liên quan tính công theo từng nhân viên

                            var profile = listProfile.Where(d => d.ID == profileId).FirstOrDefault();
                            var listAnnualDetailByProfile = listAnnualDetail.Where(d => d.ProfileID == profileId).ToList();
                            var listPreAttendanceTableByProfile = listPreAttendanceTable.Where(d => d.ProfileID == profileId).ToList();
                            var listWorkDayByProfile = listWorkDay.Where(d => d.ProfileID == profileId).OrderBy(d => d.WorkDate).ToList();
                            var listTimeOffInLieuByProfile = listTimeOffInLieu.Where(d => d.ProfileID == profileId).ToList();

                            var listHDTJobByProfile = listHDTJob.Where(d => d.ProfileID == profileId).ToList();
                            var listPregnancyByProfile = listPregnancy.Where(prg => prg.ProfileID == profileId && prg.DateEnd >= attendanceFrom && prg.DateStart <= attendanceTo).ToList();
                            var listLeaveDayByProfile = listLeaveDay.Where(d => d.ProfileID == profileId && ((d.DateStart <= attendanceTo && d.DateEnd >= attendanceFrom)
                                || (d.DateOvertimeOff.HasValue && d.DateOvertimeOff >= attendanceFrom && d.DateOvertimeOff <= attendanceTo))).ToList();

                            var listOvertimeByProfile = listOvertime.Where(d => d.ProfileID == profileId && d.WorkDate >= attendanceFrom && d.WorkDate <= attendanceTo).ToList();
                            var gradeCfgIDByProfile = listGrade.Where(d => d.ProfileID == profileId && d.MonthStart <= attendanceTo).Select(d => d.GradeAttendanceID).FirstOrDefault();
                            var gradeCfgByProfile = listGradeCfg.Where(d => d.ID == gradeCfgIDByProfile).FirstOrDefault();

                            List<Hre_WorkHistory> listWorkHistoryByProfile = listWorkHistory.Where(d => d.ProfileID == profileId).ToList();
                            var listRosterByProfile = listRoster.Where(d => d.ProfileID == profileId && d.DateStart <= attendanceTo && d.DateEnd >= attendanceFrom).ToList();
                            var listMonthShifts = Att_AttendanceLib.GetDailyShifts(attendanceFrom, attendanceTo, profile.ID, listRosterByProfile, listRosterGroup);

                            #endregion

                            attendanceTableCount += ComputeAttendance(unitOfWork, userID, monthYear, cutOffDurationID, attendanceFrom, attendanceTo, profile,
                                gradeCfgByProfile, listShift, listAnnualDetailByProfile, listHDTJobByProfile, listLeaveDayByProfile, listLeaveDayType,
                                listOvertimeByProfile, listOvertimeType, listPregnancyByProfile, listLateEarlyRule, listWorkDayByProfile,
                                listPreAttendanceTableByProfile, listTimeOffInLieuByProfile, notAutoRegHolidayLeave, otholidayscompute400,
                                missTAM_LeaveType, listMonthShifts, listHoliday);

                            #region Cập Nhật Dữ Liệu Cho Việc Tính TimeOffInLieuMonth and Year

                            double? balanceLeaves = (new Att_LeavedayServices()).CalculateTotalHourTimeOff(profileId, lstTimeOffInLieu_3Month.Where(m =>
                                m.ProfileID == profileId).ToList(), lstTimeOffInLieu_ByMonth_3Month.Where(m => m.ProfileID == profileId).ToList(), monthYear, 1);

                            double UnusualLeaves = lstTimeOffInLieu.Where(m => m.ProfileID == profileId).Sum(m => m.UnusualLeaves ?? 0);
                            double TakenLeaves = lstTimeOffInLieu.Where(m => m.ProfileID == profileId).Sum(m => m.TakenLeaves ?? 0);
                            double RemainLeaves = UnusualLeaves - TakenLeaves + (balanceLeaves ?? 0);

                            var timeOffInLieuMonth_ByProfile = lstTimeOffInLieuMonth.Where(m =>
                                m.ProfileID == profileId).FirstOrDefault();

                            if (timeOffInLieuMonth_ByProfile == null)
                            {
                                //tạo mới
                                timeOffInLieuMonth_ByProfile = new Att_TimeOffInLieuMonth();
                                timeOffInLieuMonth_ByProfile.ID = Guid.NewGuid();
                                timeOffInLieuMonth_ByProfile.ProfileID = profileId;
                                timeOffInLieuMonth_ByProfile.BalanceLeaves = balanceLeaves ?? 0;
                                timeOffInLieuMonth_ByProfile.UnusualLeaves = UnusualLeaves;
                                timeOffInLieuMonth_ByProfile.TakenLeaves = TakenLeaves;
                                timeOffInLieuMonth_ByProfile.RemainLeaves = RemainLeaves;
                                timeOffInLieuMonth_ByProfile.Month = new DateTime(attendanceTo.Year, attendanceTo.Month, 1);
                                unitOfWork.AddObject(typeof(Att_TimeOffInLieuMonth), timeOffInLieuMonth_ByProfile);
                            }
                            else
                            {
                                //chinh sua
                                timeOffInLieuMonth_ByProfile.BalanceLeaves = balanceLeaves ?? 0;
                                timeOffInLieuMonth_ByProfile.UnusualLeaves = UnusualLeaves;
                                timeOffInLieuMonth_ByProfile.TakenLeaves = TakenLeaves;
                                timeOffInLieuMonth_ByProfile.RemainLeaves = RemainLeaves;
                            }

                            #endregion

                            #region Chia thành nhiều giai đoạn lưu bảng công

                            if (asynTask != null)
                            {
                                totalComputedProfileForTask++;
                                double percent = (double)totalComputedProfileForTask / (double)totalProfile;

                                if (totalComputedProfileForTask >= totalComputedProfileMustSubmitTask)
                                {
                                    asynTask.PercentComplete = asynTask.PercentComplete + percent;
                                    dataErrorCode = taskUnitOfWork.SaveChanges(userID);
                                    totalComputedProfileForTask = 0;

                                    if (dataErrorCode == DataErrorCode.Locked)
                                    {
                                        break;
                                    }
                                }
                            }

                            //Nên submit lần đầu tiên khi đã tính được 5 profile
                            bool firstSubmit = listProfileID.ToList().IndexOf(profileId) == 5;
                            firstSubmit = totalComputed >= 5 ? false : firstSubmit;//đã chạy 1 lần
                            totalComputedProfileForSubmit++;

                            if (firstSubmit || totalComputedProfileForSubmit >= totalComputedProfileMustSubmit)
                            {
                                totalComputedProfileForSubmit = firstSubmit ? totalComputedProfileForSubmit : 0;
                                dataErrorCode = unitOfWork.SaveChanges(userID);

                                if (dataErrorCode == DataErrorCode.Locked)
                                {
                                    break;
                                }
                            }

                            #endregion
                        }
                        catch
                        {
                            throw new Exception(profileId.ToString());
                        }
                    }
                }

                #endregion

                //Lưu tất cả kết quả tính công
                dataErrorCode = unitOfWork.SaveChanges(userID);
                return attendanceTableCount;
            }
        }

        private int ComputeAttendance(IUnitOfWork unitOfWork, Guid userID, DateTime monthYear, Guid cutOffDurationID, DateTime attendanceFrom,
            DateTime attendanceTo, Hre_ProfileEntity profile, Cat_GradeAttendance gradeCfg, List<Cat_Shift> listShift, List<Att_AnnualDetail> listAnnualDetailByProfile,
            List<Hre_HDTJob> listHDTJobByProfile, List<Att_LeaveDayEntity> listLeaveDayByProfile, List<Cat_LeaveDayType> listLeaveDayType,
            List<Att_OvertimeEntity> listOvertimeByProfile, List<Cat_OvertimeType> listOvertimeType, List<Att_PregnancyEntity> listPregnancyByProfile,
            List<Cat_LateEarlyRule> listLateEarlyRule, List<Att_Workday> listWorkDayByProfile, List<Att_AttendanceTableEntity> listPreAttendanceTableByProfile,
            List<Att_TimeOffInLieuEntity> listTimeOffInLieuByProfile, Sys_AllSetting notAutoRegHolidayLeave, Sys_AllSetting otholidayscompute400,
            Sys_AllSetting missTAM_LeaveType, Dictionary<DateTime, List<Guid?>> listMonthShiftID, List<Cat_DayOff> listHoliday)
        {
            Guid profileID = profile.ID;

            if (gradeCfg == null || profile == null)
            {
                return 0;
            }

            DateTime midCutOffDate = attendanceTo;
            bool isMidCutOffDay = false;

            #region Tạo Att_AttendanceTable theo khoảng thời gian

            Att_AttendanceTable attendanceTable = new Att_AttendanceTable();
            unitOfWork.AddObject(typeof(Att_AttendanceTable), attendanceTable);
            attendanceTable.Status = AttendanceTableStatus.E_WAITING.ToString();
            attendanceTable.ID = Guid.NewGuid();
            attendanceTable.ProfileID = profileID;
            attendanceTable.MonthYear = monthYear;
            attendanceTable.DateStart = attendanceFrom;
            attendanceTable.DateEnd = attendanceTo;

            double anlDayTaken = 0D;
            double sickDayTaken = 0D;
            double coBeginPeriod = 0D;
            double coEndPeriod = 0D;

            if (cutOffDurationID != Guid.Empty)
            {
                attendanceTable.CutOffDurationID = cutOffDurationID;
            }

            //Tính công đến ngày cấu hình trong grade, những ngày sau mặc định full công - Khách hàng CPG
            if (gradeCfg != null && gradeCfg.IsMonthlyMidCutOff == true && gradeCfg.MidCutOffDay > 0)
            {
                if (!profile.DateQuit.HasValue || profile.DateQuit > attendanceTo)
                {
                    midCutOffDate = new DateTime(monthYear.Year,
                        monthYear.Month, gradeCfg.MidCutOffDay.Value);

                    //Luôn tạo một dòng Ứng công lúc tính công, kô cần biết có nghỉ không
                    Att_AdvanceTableItem advanceTableItem = new Att_AdvanceTableItem();
                    advanceTableItem.Att_AttendanceTable = attendanceTable;
                    advanceTableItem.DateFrom = midCutOffDate.AddDays(1);
                    advanceTableItem.DateTo = attendanceTo;
                    isMidCutOffDay = true;
                }
            }

            #endregion

            #region Tạo Att_AttendanceTableItem theo khoảng thời gian

            List<Att_AttendanceTableItem> listAttendanceTableItemByProfile = new List<Att_AttendanceTableItem>();

            for (DateTime date = attendanceFrom.Date; date <= attendanceTo; date = date.AddDays(1))
            {
                var workdayByProfile = listWorkDayByProfile.Where(d =>
                    d.WorkDate.Date == date).FirstOrDefault();

                Guid? shiftID1 = Guid.Empty;
                Guid? shiftID2 = Guid.Empty;

                var shiftByDate = listMonthShiftID.Where(d => d.Key.Date == date).Select(d => d.Value).FirstOrDefault();

                if (shiftByDate != null)
                {
                    if (shiftByDate.Count() > 0)
                    {
                        shiftID1 = shiftByDate[0];
                    }

                    if (shiftByDate.Count() > 1)
                    {
                        shiftID2 = shiftByDate[1];
                    }
                }

                if (workdayByProfile == null && gradeCfg.EDType == PayrollComputeMethod.E_SUBTRACT.ToString())
                {
                    if (shiftID1.HasValue && shiftID1 != Guid.Empty)
                    {
                        workdayByProfile = new Att_Workday
                        {
                            ShiftID = shiftID1,
                            ShiftApprove = shiftID1,
                            ShiftActual = shiftID1,
                            WorkDate = date
                        };

                        if (shiftID2.HasValue && shiftID2 != Guid.Empty)
                        {
                            workdayByProfile.Shift2ID = shiftID2;
                            workdayByProfile.ShiftApprove2 = shiftID2;
                            workdayByProfile.ShiftActual2 = shiftID2;
                        }

                        if (listHoliday.Any(d => d.DateOff.Date == date))
                        {
                            workdayByProfile.Type = WorkdayType.E_HOLIDAY.ToString();
                        }
                    }
                }

                Att_AttendanceTableItem attendanceTableItem = new Att_AttendanceTableItem();
                listAttendanceTableItemByProfile.Add(attendanceTableItem);
                attendanceTableItem.ID = Guid.NewGuid();
                attendanceTableItem.Att_AttendanceTable = attendanceTable;
                attendanceTableItem.DutyCode = DutyCode.E_OFF.ToString();
                attendanceTableItem.WorkDate = date;

                double nightDuration = 0F;
                double workDuration = 0F;

                attendanceTableItem.WorkPaidHours = 0;
                attendanceTableItem.PaidLeaveHours = 0;
                attendanceTableItem.UnpaidLeaveHours = 0;
                attendanceTableItem.LateEarlyMinutes = 0;
                attendanceTableItem.LateInMinutes = 0;
                attendanceTableItem.EarlyOutMinutes = 0;
                attendanceTableItem.LeaveHours = 0;
                attendanceTableItem.LeaveDays = 0;
                attendanceTableItem.WorkHourFirstHaftShift = 0;
                attendanceTableItem.WorkHourLastHaftShift = 0;

                if (workdayByProfile != null)
                {
                    attendanceTableItem.MissInOutReasonID = workdayByProfile.MissInOutReason;
                    attendanceTableItem.FirstInTime = workdayByProfile.FirstInTime;
                    attendanceTableItem.LastOutTime = workdayByProfile.LastOutTime;
                    attendanceTableItem.RootInTime = workdayByProfile.FirstInTime;
                    attendanceTableItem.RootOutTime = workdayByProfile.LastOutTime;
                    attendanceTableItem.ShiftID = workdayByProfile.ShiftID;
                    attendanceTableItem.Shift2ID = workdayByProfile.Shift2ID;

                    List<DateTime?> listInTime = new List<DateTime?>();
                    List<DateTime?> listOutTime = new List<DateTime?>();

                    listInTime.Add(workdayByProfile.InTime1);
                    listInTime.Add(workdayByProfile.InTime2);
                    listInTime.Add(workdayByProfile.InTime3);
                    listInTime.Add(workdayByProfile.InTime4);

                    listOutTime.Add(workdayByProfile.OutTime1);
                    listOutTime.Add(workdayByProfile.OutTime2);
                    listOutTime.Add(workdayByProfile.OutTime3);
                    listOutTime.Add(workdayByProfile.OutTime4);

                    attendanceTableItem.FirstInTime = listInTime.Where(d => d.HasValue).OrderBy(d => d).FirstOrDefault();
                    attendanceTableItem.LastOutTime = listOutTime.Where(d => d.HasValue).OrderBy(d => d).LastOrDefault();

                    if (workdayByProfile.ShiftApprove.HasValue)
                    {
                        attendanceTableItem.ShiftID = workdayByProfile.ShiftApprove.Value;
                    }

                    if (workdayByProfile.ShiftApprove2.HasValue)
                    {
                        attendanceTableItem.Shift2ID = workdayByProfile.ShiftApprove2.Value;
                    }

                    if (gradeCfg.AttendanceMethod == AttendanceMethod.E_FULL.ToString())
                    {
                        if (listMonthShiftID.Any(d => d.Key.Date == date))
                        {
                            //Bao.Tran: khi không kiểm tra vào ra thì ưu tiên ca đã tạo trong lịch làm việc
                            if (shiftID1 != Guid.Empty)
                            {
                                attendanceTableItem.ShiftID = shiftID1;
                            }

                            if (shiftID2 != Guid.Empty)
                            {
                                attendanceTableItem.Shift2ID = shiftID2;
                            }
                        }

                        //Bao.Tran: yêu cầu nếu công ko kiểm tra in/out thì không lấy in/out 
                        attendanceTableItem.FirstInTime = null;
                        attendanceTableItem.LastOutTime = null;

                        //comment code này do ảnh hưởng task 0046036 : ko tính được ngày nghỉ lễ
                        ////Bao.Tran: do khi THNC cũ, Nếu ko bỏ type thì nó sẽ tính ngày công trên là holiday  (attendanceTableItem.IsHoliday = true;)
                        //workdayByProfile.Type = WorkdayType.E_NORMAL.ToString();
                    }

                    //kiểm tra ngày làm việc có Lịch làm việc hay ko?
                    if (attendanceTableItem.ShiftID != null && workdayByProfile.FirstInTime != null && workdayByProfile.LastOutTime != null)
                    {
                        #region Kiểm tra HDTJob

                        var listHDTJobByProfileByDate = listHDTJobByProfile.Where(d => (d.DateFrom.Value.Date <= date && d.DateTo == null)
                            || (d.DateTo != null && d.DateFrom.Value.Date <= date && d.DateTo.Value.Date >= date)).ToList();

                        foreach (var HDTJobByProfile in listHDTJobByProfileByDate)
                        {
                            if (HDTJobByProfile != null)
                            {
                                attendanceTableItem.HDTJobType = HDTJobByProfile.Type;
                            }
                        }

                        #endregion
                    }
                }

                var shiftByProfile1 = listShift.Where(d => d.ID == attendanceTableItem.ShiftID).FirstOrDefault();
                var shiftByProfile2 = listShift.Where(d => d.ID == attendanceTableItem.Shift2ID).FirstOrDefault();

                if (shiftByProfile1 != null)
                {
                    attendanceTableItem.Cat_Shift = shiftByProfile1;
                    attendanceTableItem.AvailableHours = shiftByProfile1.udAvailableHours;
                }

                if (shiftByProfile2 != null)
                {
                    attendanceTableItem.Cat_Shift1 = shiftByProfile2;
                }

                if ((workdayByProfile != null && workdayByProfile.Type == WorkdayType.E_HOLIDAY.ToString())
                    || (attendanceTableItem.ShiftID.HasValue && attendanceTableItem.ShiftID != Guid.Empty))
                {
                    //Ngày đang xét là ngày làm việc bình thường
                    attendanceTableItem.DutyCode = DutyCode.E_ON.ToString();

                    if (workdayByProfile.Type == WorkdayType.E_HOLIDAY.ToString())
                    {
                        attendanceTableItem.IsHoliday = true;
                    }
                }
                else
                {
                    //Ngày đang xét không phải là ngày làm việc bình thường
                    attendanceTableItem.DutyCode = DutyCode.E_OFF.ToString();
                }

                #region Kiểm tra ngày nghỉ - leaveday

                if (attendanceTableItem.DutyCode != DutyCode.E_OFF.ToString())
                {
                    var listLeaveDayByProfileByDate = listLeaveDayByProfile.Where(d =>
                        d.DateStart.Date <= date && d.DateEnd.Date >= date).ToList();

                    foreach (var leaveDayByProfile in listLeaveDayByProfileByDate)
                    {
                        if (leaveDayByProfile != null)
                        {
                            double leaveHours = 0D;
                            double leaveDays = 0D;

                            var leaveDayType = listLeaveDayType.Where(d => d.ID ==
                                leaveDayByProfile.LeaveDayTypeID).FirstOrDefault();

                            var standardWorkHours = attendanceTableItem.AvailableHours;

                            if (leaveDayByProfile.DurationType == LeaveDayDurationType.E_FULLSHIFT.ToString())
                            {
                                //Lấy giờ nghỉ của ngày đang xét dựa vào ca làm việc
                                leaveHours = standardWorkHours;
                                leaveDays = 1;//Nghỉ full shift
                            }
                            else
                            {
                                leaveHours = leaveDayByProfile.LeaveHours.GetDouble();
                                leaveDays = leaveDayByProfile.LeaveDays.GetDouble();
                            }

                            leaveHours = leaveHours > standardWorkHours ? standardWorkHours : leaveHours;
                            leaveDays = leaveDays > 1 ? 1 : leaveDays;//Tối đa là nghỉ 1 ngày

                            if (!attendanceTableItem.LeaveTypeID.HasValue)
                            {
                                attendanceTableItem.LeaveTypeID = leaveDayByProfile.LeaveDayTypeID;
                                attendanceTableItem.Cat_LeaveDayType = leaveDayType;

                                attendanceTableItem.LeaveHours = leaveHours;
                                attendanceTableItem.LeaveDays = leaveDays;
                            }
                            else if (!attendanceTableItem.ExtraLeaveTypeID.HasValue)
                            {
                                attendanceTableItem.ExtraLeaveTypeID = leaveDayByProfile.LeaveDayTypeID;
                                attendanceTableItem.Cat_LeaveDayType1 = leaveDayType;

                                attendanceTableItem.ExtraLeaveHours = leaveHours;
                                attendanceTableItem.ExtraLeaveDays = leaveDays;
                            }

                            if (leaveDayType != null && leaveDays > 0)
                            {
                                if (leaveDayType.Code == LeavedayTypeCode.ANL.ToString())
                                {
                                    anlDayTaken += leaveDays;
                                }
                                if (leaveDayType.Code == LeavedayTypeCode.SICK.ToString())
                                {
                                    sickDayTaken += leaveDays;
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Kiểm tra làm thêm - overtime

                if (gradeCfg != null && gradeCfg.IsReceiveOvertimeBonus.Get_Boolean())
                {
                    var listOvertimeByProfileByDate = listOvertimeByProfile.Where(d =>
                        d.WorkDate.Date == date).ToList();

                    foreach (var overtime in listOvertimeByProfileByDate)
                    {
                        if (overtime != null)
                        {
                            double overtimeHours = overtime.ConfirmHours;
                            string overTimeStatus = OverTimeStatus.E_APPROVED.ToString();

                            if (overTimeStatus == OverTimeStatus.E_APPROVED.ToString())
                            {
                                overtimeHours = overtime.ApproveHours.GetDouble();
                            }
                            else if (overTimeStatus == OverTimeStatus.E_CONFIRM.ToString())
                            {
                                overtimeHours = overtime.ConfirmHours;
                            }

                            if (!attendanceTableItem.OvertimeTypeID.HasValue)
                            {
                                attendanceTableItem.OvertimeTypeID = overtime.OvertimeTypeID;
                                attendanceTableItem.Cat_OvertimeType = listOvertimeType.Where(s => s.ID == overtime.OvertimeTypeID).FirstOrDefault();
                                attendanceTableItem.OvertimeDurationType = overtime.DurationType;
                                attendanceTableItem.OvertimeRegisterHours = overtime.RegisterHours;
                                attendanceTableItem.OvertimeHours = overtimeHours;
                            }
                            else if (!attendanceTableItem.ExtraOvertimeTypeID.HasValue)
                            {
                                attendanceTableItem.ExtraOvertimeTypeID = overtime.OvertimeTypeID;
                                attendanceTableItem.Cat_OvertimeType1 = listOvertimeType.Where(s => s.ID == overtime.OvertimeTypeID).FirstOrDefault();
                                attendanceTableItem.ExtraOvertimeDurationType = overtime.DurationType;
                                attendanceTableItem.ExtraOvertimeRegisterHours = overtime.RegisterHours;
                                attendanceTableItem.ExtraOvertimeHours = overtimeHours;
                            }
                            else if (!attendanceTableItem.ExtraOvertimeType2ID.HasValue)
                            {
                                attendanceTableItem.ExtraOvertimeType2ID = overtime.OvertimeTypeID;
                                attendanceTableItem.Cat_OvertimeType2 = listOvertimeType.Where(s => s.ID == overtime.OvertimeTypeID).FirstOrDefault();
                                attendanceTableItem.ExtraOvertimeDurationType2 = overtime.DurationType;
                                attendanceTableItem.ExtraOvertimeRegisterHours2 = overtime.RegisterHours;
                                attendanceTableItem.ExtraOvertimeHours2 = overtimeHours;
                            }
                            else if (!attendanceTableItem.ExtraOvertimeType3ID.HasValue)
                            {
                                attendanceTableItem.ExtraOvertimeType3ID = overtime.OvertimeTypeID;
                                attendanceTableItem.Cat_OvertimeType3 = listOvertimeType.Where(s => s.ID == overtime.OvertimeTypeID).FirstOrDefault();
                                attendanceTableItem.ExtraOvertimeDurationType3 = overtime.DurationType;
                                attendanceTableItem.ExtraOvertimeRegisterHours3 = overtime.RegisterHours;
                                attendanceTableItem.ExtraOvertimeHours3 = overtimeHours;
                            }
                        }
                    }
                }

                #endregion

                #region Kiểm tra có thai sản không

                var pregnancyByProfile = listPregnancyByProfile.Where(d =>
                    date >= d.DateStart && date <= d.DateEnd).FirstOrDefault();

                if (pregnancyByProfile != null)
                {
                    attendanceTableItem.IsHavingPregTreatment = true;
                }

                #endregion

                #region Nhân viên đã nghỉ hoặc chưa đi làm

                if ((profile.DateQuit.HasValue && profile.DateQuit.Value.Date <= date.Date)
                   || (profile.DateHire.HasValue && profile.DateHire.Value.Date > date.Date))
                {
                    if (attendanceTableItem.DutyCode == DutyCode.E_ON.ToString())
                    {
                        if (profile.DateHire.HasValue && profile.DateHire.Value.Date > date.Date)
                        {
                            if (shiftByProfile1 != null)
                            {
                                attendanceTableItem.udNotHiredHours = shiftByProfile1.WorkHours.GetDouble();
                            }
                            else
                            {
                                attendanceTableItem.udNotHiredHours = attendanceTableItem.udStandardWorkHours;
                            }

                            if (shiftByProfile2 != null)
                            {
                                attendanceTableItem.udNotHiredHours += shiftByProfile2.WorkHours.GetDouble();
                            }
                        }

                        if (profile.DateQuit.HasValue && profile.DateQuit.Value.Date <= date.Date)
                        {
                            if (shiftByProfile1 != null)
                            {
                                attendanceTableItem.udTerminatedHours = shiftByProfile1.WorkHours.GetDouble();
                            }
                            else
                            {
                                attendanceTableItem.udTerminatedHours = attendanceTableItem.udStandardWorkHours;
                            }

                            if (shiftByProfile2 != null)
                            {
                                attendanceTableItem.udTerminatedHours += shiftByProfile2.WorkHours.GetDouble();
                            }
                        }
                    }

                    attendanceTableItem.Cat_LeaveDayType = null;
                    continue;
                }

                #endregion

                #region Loại ngày nghỉ là ngày làm việc

                if (attendanceTableItem.Cat_LeaveDayType != null)
                {
                    //Trường hợp ngày nghỉ tính như ngày đi làm => không tính nghỉ
                    if (attendanceTableItem.Cat_LeaveDayType.IsWorkDay)
                    {
                        attendanceTableItem.WorkHours += attendanceTableItem.LeaveHours;
                        attendanceTableItem.WorkPaidHours += attendanceTableItem.LeaveHours;

                        attendanceTableItem.LeaveWorkDayHour = attendanceTableItem.LeaveHours;
                        attendanceTableItem.LeaveWorkDayDays = attendanceTableItem.LeaveHours / attendanceTableItem.udStandardWorkHours;
                        attendanceTableItem.Cat_LeaveDayType2 = attendanceTableItem.Cat_LeaveDayType;

                        attendanceTableItem.Cat_LeaveDayType = null;
                        attendanceTableItem.LeaveHours = 0;
                        attendanceTableItem.LeaveDays = 0;
                    }
                    else
                    {
                        attendanceTableItem.PaidLeaveHours += attendanceTableItem.LeaveHours * attendanceTableItem.Cat_LeaveDayType.PaidRate;
                        attendanceTableItem.UnpaidLeaveHours += attendanceTableItem.LeaveHours * (1 - attendanceTableItem.Cat_LeaveDayType.PaidRate);

                        attendanceTableItem.PaidLeaveDays = attendanceTableItem.PaidLeaveHours / attendanceTableItem.udStandardWorkHours;
                        attendanceTableItem.UnpaidLeaveDays = attendanceTableItem.UnpaidLeaveHours / attendanceTableItem.udStandardWorkHours;
                    }
                }

                if (attendanceTableItem.Cat_LeaveDayType1 != null)
                {
                    if (attendanceTableItem.Cat_LeaveDayType1.IsWorkDay)
                    {
                        attendanceTableItem.WorkHours += attendanceTableItem.ExtraLeaveHours;
                        attendanceTableItem.WorkPaidHours += attendanceTableItem.ExtraLeaveHours;
                        attendanceTableItem.Cat_LeaveDayType1 = null;
                        attendanceTableItem.ExtraLeaveHours = 0;
                    }
                    else
                    {
                        attendanceTableItem.PaidLeaveHours += attendanceTableItem.ExtraLeaveHours * attendanceTableItem.Cat_LeaveDayType1.PaidRate;
                        attendanceTableItem.UnpaidLeaveHours += attendanceTableItem.ExtraLeaveHours * (1 - attendanceTableItem.Cat_LeaveDayType1.PaidRate);
                    }
                }

                #endregion

                if (attendanceTableItem.DutyCode == DutyCode.E_ON.ToString())
                {
                    #region IsHavingPregTreatment

                    if (attendanceTableItem.IsHavingPregTreatment)
                    {
                        if ((attendanceTableItem.WorkPaidHours + attendanceTableItem.PaidLeaveHours) > attendanceTableItem.AvailableHours)
                        {
                            attendanceTableItem.WorkPaidHours = attendanceTableItem.AvailableHours - attendanceTableItem.PaidLeaveHours;
                        }
                        else
                        {
                            attendanceTableItem.WorkPaidHours = attendanceTableItem.WorkPaidHours;
                        }
                    }

                    #endregion

                    #region LateEarlyMinutes

                    attendanceTableItem.LateEarlyMinutes = (int)workdayByProfile.LateEarlyDuration.GetDouble();
                    attendanceTableItem.RealLateEarlyMinutes = attendanceTableItem.LateEarlyMinutes;

                    #endregion

                    #region Tính số giờ đi làm

                    if (workdayByProfile != null && shiftByProfile1 != null)
                    {
                        DateTime timeShiftStart1 = workdayByProfile.WorkDate.Date.Add(shiftByProfile1.InTime.TimeOfDay);
                        DateTime timeShiftEnd1 = timeShiftStart1.AddHours(shiftByProfile1.CoOut);

                        //Thời gian bắt đầu và kết thúc nghỉ giữa ca - dùng cho tính toán
                        DateTime timeShiftBreakIn1 = timeShiftStart1.AddHours(shiftByProfile1.CoBreakIn);
                        DateTime timeShiftBreakOut1 = timeShiftStart1.AddHours(shiftByProfile1.CoBreakOut);

                        workDuration = GetDuration(workdayByProfile, timeShiftStart1, timeShiftEnd1,
                            timeShiftBreakIn1, timeShiftBreakOut1, shiftByProfile1.InOutDynamic);

                        #region Tính số giờ làm ca đêm

                        if (shiftByProfile1.IsNightShift)
                        {
                            DateTime nightTimeStart = workdayByProfile.WorkDate.Date.AddHours(22);
                            DateTime nightTimeEnd = workdayByProfile.WorkDate.Date.AddDays(1).AddHours(6);

                            nightTimeStart = workdayByProfile.WorkDate.Date.Add(shiftByProfile1.NightTimeStart.Value.TimeOfDay);
                            nightTimeEnd = workdayByProfile.WorkDate.Date.Add(shiftByProfile1.NightTimeEnd.Value.TimeOfDay);
                            nightTimeEnd = nightTimeStart > nightTimeEnd ? nightTimeEnd.AddDays(1) : nightTimeEnd;

                            //Truong hop nghi giua ca nam trong khoang bat dau ca dem
                            nightDuration = GetDuration(workdayByProfile, nightTimeStart, nightTimeEnd,
                                timeShiftBreakIn1, timeShiftBreakOut1, shiftByProfile1.InOutDynamic);
                        }

                        #endregion
                    }

                    if (workdayByProfile != null && shiftByProfile2 != null)
                    {
                        DateTime timeShiftStart2 = workdayByProfile.WorkDate.Date.Add(shiftByProfile2.InTime.TimeOfDay);
                        DateTime timeShiftEnd2 = timeShiftStart2.AddHours(shiftByProfile2.CoOut);

                        //Thời gian bắt đầu và kết thúc nghỉ giữa ca - dùng cho tính toán
                        DateTime timeShiftBreakIn2 = timeShiftStart2.AddHours(shiftByProfile2.CoBreakIn);
                        DateTime timeShiftBreakOut2 = timeShiftStart2.AddHours(shiftByProfile2.CoBreakOut);

                        workDuration += GetDuration(workdayByProfile, timeShiftStart2, timeShiftEnd2,
                            timeShiftBreakIn2, timeShiftBreakOut2, shiftByProfile2.InOutDynamic);

                        #region Tính số giờ làm ca đêm

                        if (shiftByProfile2.IsNightShift)
                        {
                            DateTime nightTimeStart = workdayByProfile.WorkDate.Date.AddHours(22);
                            DateTime nightTimeEnd = workdayByProfile.WorkDate.Date.AddDays(1).AddHours(6);

                            nightTimeStart = workdayByProfile.WorkDate.Date.Add(shiftByProfile2.NightTimeStart.Value.TimeOfDay);
                            nightTimeEnd = workdayByProfile.WorkDate.Date.Add(shiftByProfile2.NightTimeEnd.Value.TimeOfDay);
                            nightTimeEnd = nightTimeStart > nightTimeEnd ? nightTimeEnd.AddDays(1) : nightTimeEnd;

                            //Truong hop nghi giua ca nam trong khoang bat dau ca dem
                            nightDuration = GetDuration(workdayByProfile, nightTimeStart, nightTimeEnd,
                                timeShiftBreakIn2, timeShiftBreakOut2, shiftByProfile2.InOutDynamic);
                        }

                        #endregion
                    }

                    attendanceTableItem.NightShiftHours = (nightDuration / 60);//lưu số giờ 
                    attendanceTableItem.WorkPaidHours = (workDuration / 60);//lưu số giờ 
                    attendanceTableItem.WorkHours = (workDuration / 60);//lưu số giờ 

                    attendanceTableItem.WorkPaidHours = attendanceTableItem.WorkPaidHours < 0 ? 0 : attendanceTableItem.WorkPaidHours;
                    attendanceTableItem.WorkPaidHours = attendanceTableItem.WorkPaidHours > attendanceTableItem.AvailableHours
                        ? attendanceTableItem.AvailableHours : attendanceTableItem.WorkPaidHours;

                    #endregion

                    if (gradeCfg.AttendanceMethod == AttendanceMethod.E_FULL.ToString())
                    {
                        #region gradeCfg.AttendanceMethod = E_FULL

                        if (shiftByProfile1 != null && shiftByProfile1.IsNightShift == true)
                        {
                            if (shiftByProfile1.NightTimeEnd != null && shiftByProfile1.NightTimeStart != null)
                            {
                                DateTime dateStartNightShift = shiftByProfile1.NightTimeStart.Value;
                                DateTime dateEndNightShift = shiftByProfile1.NightTimeEnd.Value;

                                if (dateStartNightShift > dateEndNightShift)
                                {
                                    dateEndNightShift = dateEndNightShift.AddDays(1);
                                }

                                attendanceTableItem.NightShiftHours = dateEndNightShift.Subtract(dateStartNightShift).TotalHours;
                            }
                            else
                            {
                                attendanceTableItem.NightShiftHours = shiftByProfile1.WorkHours ?? 0;
                            }
                        }
                        else if (shiftByProfile2 != null && shiftByProfile2.IsNightShift == true)
                        {
                            if (shiftByProfile2.NightTimeEnd != null && shiftByProfile2.NightTimeStart != null)
                            {
                                DateTime dateStartNightShift = shiftByProfile2.NightTimeStart.Value;
                                DateTime dateEndNightShift = shiftByProfile2.NightTimeEnd.Value;

                                if (dateStartNightShift > dateEndNightShift)
                                {
                                    dateEndNightShift = dateEndNightShift.AddDays(1);
                                }

                                attendanceTableItem.NightShiftHours = dateEndNightShift.Subtract(dateStartNightShift).TotalHours;
                            }
                            else
                            {
                                attendanceTableItem.NightShiftHours = shiftByProfile2.WorkHours ?? 0;
                            }
                        }

                        if ((shiftByProfile1 != null && shiftByProfile1.IsNightShift == true)
                            || shiftByProfile2 != null && shiftByProfile2.IsNightShift == true)
                        {
                            if (attendanceTableItem.Cat_LeaveDayType != null)
                            {
                                double hourLeave = attendanceTableItem.LeaveHours + attendanceTableItem.PaidLeaveHours + attendanceTableItem.ExtraLeaveHours;
                                attendanceTableItem.NightShiftHours = attendanceTableItem.NightShiftHours - hourLeave;

                                if (attendanceTableItem.NightShiftHours < 0)
                                {
                                    attendanceTableItem.NightShiftHours = 0;
                                }
                            }
                        }

                        if (attendanceTableItem.IsHoliday)
                        {
                            attendanceTableItem.LeaveHours = attendanceTableItem.PaidLeaveHours = 0;
                            attendanceTableItem.WorkPaidHours = attendanceTableItem.LateEarlyMinutes = 0;
                            attendanceTableItem.UnpaidLeaveHours = attendanceTableItem.AvailableHours;

                            bool isNoLongerWorking = false;

                            if ((profile.DateHire.HasValue && attendanceTableItem.WorkDate < profile.DateHire.Value)
                                || (profile.DateQuit.HasValue && attendanceTableItem.WorkDate >= profile.DateQuit.Value))
                            {
                                isNoLongerWorking = true;
                            }

                            if (!attendanceTableItem.IsHavingPregTreatment && !isNoLongerWorking && (notAutoRegHolidayLeave == null
                                || notAutoRegHolidayLeave.Value1 != bool.TrueString))
                            {
                                attendanceTableItem.WorkPaidHours = 0;
                                attendanceTableItem.LateEarlyMinutes = 0;
                                attendanceTableItem.UnpaidLeaveHours = 0;

                                attendanceTableItem.LeaveHours = attendanceTableItem.AvailableHours;
                                attendanceTableItem.PaidLeaveHours = attendanceTableItem.AvailableHours;

                                attendanceTableItem.Cat_LeaveDayType = listLeaveDayType.Where(d =>
                                    d.Code == LeavedayTypeCode.HLD.ToString()).FirstOrDefault();
                            }
                        }

                        attendanceTableItem.WorkHours = 0;//Thời gian làm việc = 0
                        attendanceTableItem.WorkPaidHours = attendanceTableItem.AvailableHours
                            - attendanceTableItem.UnpaidLeaveHours - attendanceTableItem.PaidLeaveHours;

                        #endregion
                    }

                    #region Trường hợp tăng ca trong ngày lễ

                    if (attendanceTableItem.Cat_LeaveDayType != null && attendanceTableItem.Cat_LeaveDayType.Code == LeavedayTypeCode.HLD.ToString() &&
                        (otholidayscompute400 == null || otholidayscompute400.Value1 != bool.TrueString) && attendanceTableItem.Cat_OvertimeType != null)
                    {
                        var totalOvertimeHours = attendanceTableItem.OvertimeHours + attendanceTableItem.ExtraOvertimeHours
                            + attendanceTableItem.ExtraOvertimeHours2 + attendanceTableItem.ExtraOvertimeHours3;

                        if (gradeCfg.EDType == PayrollComputeMethod.E_SUBTRACT.ToString())
                        {
                            if (attendanceTableItem.AvailableHours > totalOvertimeHours)
                            {
                                attendanceTableItem.LeaveHours = attendanceTableItem.AvailableHours - totalOvertimeHours;
                                attendanceTableItem.ExtraLeaveHours = totalOvertimeHours;
                                attendanceTableItem.PaidLeaveHours = attendanceTableItem.LeaveHours;
                                attendanceTableItem.Cat_LeaveDayType1 = null;
                            }
                            else
                            {
                                attendanceTableItem.Cat_LeaveDayType = null;
                                attendanceTableItem.LeaveHours = attendanceTableItem.AvailableHours;
                                attendanceTableItem.PaidLeaveHours = 0;
                            }
                        }
                        else
                        {
                            if (attendanceTableItem.AvailableHours >= totalOvertimeHours)
                            {
                                attendanceTableItem.LeaveHours = attendanceTableItem.PaidLeaveHours =
                                    attendanceTableItem.AvailableHours - totalOvertimeHours;
                            }
                            else
                            {
                                attendanceTableItem.LeaveHours = attendanceTableItem.PaidLeaveHours = 0;
                            }
                        }

                        attendanceTableItem.WorkPaidHours = 0;
                    }

                    #endregion
                }

                unitOfWork.AddObject(typeof(Att_AttendanceTableItem), attendanceTableItem);
            }

            #endregion

            #region Tính toán lại phép năm và phép ốm trong kỳ

            var annualLeaveDetail = listAnnualDetailByProfile.Where(d => d.Type == AnnualLeaveDetailType.E_ANNUAL_LEAVE.ToString()).FirstOrDefault();
            var sickLeaveDetail = listAnnualDetailByProfile.Where(d => d.Type == AnnualLeaveDetailType.E_SICK_LEAVE.ToString()).FirstOrDefault();
            var preAttendanceTable = listPreAttendanceTableByProfile.Where(d => d.ProfileID == profileID).FirstOrDefault();

            attendanceTable.AnlDayTaken = anlDayTaken;
            attendanceTable.SickDayTaken = sickDayTaken;

            if (preAttendanceTable != null)
            {
                if (annualLeaveDetail != null && annualLeaveDetail.MonthYear.HasValue
                    && annualLeaveDetail.MonthYear.Value.Date == monthYear.Date)
                {
                    attendanceTable.AnlDayAdjacent = anlDayTaken;
                }
                else
                {
                    //VinhTran - 0030581 - AnlDayAdjacent là số ngày đã nghỉ tính tới đầu tháng đang tính công
                    attendanceTable.AnlDayAdjacent = preAttendanceTable.AnlDayAdjacent.GetDouble() + anlDayTaken;
                }

                if (sickLeaveDetail != null && sickLeaveDetail.MonthYear.HasValue
                    && sickLeaveDetail.MonthYear.Value.Date == monthYear.Date)
                {
                    attendanceTable.SickDayAdjacent = sickDayTaken;
                }
                else
                {
                    //VinhTran - 0030581 - AnlDayAdjacent là số ngày đã nghỉ tính tới đầu tháng đang tính công
                    attendanceTable.SickDayAdjacent = preAttendanceTable.SickDayAdjacent.GetDouble() + sickDayTaken;
                }
            }
            else
            {
                attendanceTable.AnlDayAdjacent = anlDayTaken;
                attendanceTable.SickDayAdjacent = sickDayTaken;
            }

            if (annualLeaveDetail != null && annualLeaveDetail.MonthYear.HasValue)
            {
                attendanceTable.TotalAnlDayAvailable = annualLeaveDetail.Available.Get_Double();
                attendanceTable.AnlDayAvailable = attendanceTable.TotalAnlDayAvailable.GetDouble() - attendanceTable.AnlDayAdjacent.GetDouble();
            }
            else if (preAttendanceTable != null)
            {
                attendanceTable.AnlDayAvailable = preAttendanceTable.AnlDayAvailable - attendanceTable.AnlDayTaken;
            }

            if (sickLeaveDetail != null && sickLeaveDetail.MonthYear.HasValue)
            {
                attendanceTable.TotalSickDayAvailable = sickLeaveDetail.Available.Get_Double();
                attendanceTable.SickDayAvailable = attendanceTable.TotalSickDayAvailable.GetDouble() - attendanceTable.SickDayAdjacent.GetDouble();
            }
            else if (preAttendanceTable != null)
            {
                //tháng 7: còn 13, liền kề 1, hiện tại 1 => tháng 8: liền kề 2, hiện tại 1, còn 12
                attendanceTable.SickDayAvailable = preAttendanceTable.SickDayAvailable.GetDouble() - attendanceTable.SickDayTaken.GetDouble();
            }

            #endregion

            #region Tính toán lại phép bù trong kỳ

            foreach (var leaveDayByProfile in listLeaveDayByProfile)
            {
                if (leaveDayByProfile != null)
                {
                    var dateStart = leaveDayByProfile.DateStart;
                    var dateEnd = leaveDayByProfile.DateEnd;

                    if (leaveDayByProfile.DateOvertimeOff.HasValue)
                    {
                        dateStart = leaveDayByProfile.DateOvertimeOff.Value;
                        dateEnd = leaveDayByProfile.DateOvertimeOff.Value;
                    }

                    for (DateTime date = dateStart.Date; date <= dateEnd; date = date.AddDays(1))
                    {
                        double leaveHours = 0D;
                        double leaveDays = 0D;
                        double standardWorkHours = 0;

                        var leaveDayType = listLeaveDayType.Where(d => d.ID == leaveDayByProfile.LeaveDayTypeID).FirstOrDefault();
                        var attendanceTableItem = attendanceTable.Att_AttendanceTableItem.Where(d => d.WorkDate == date).FirstOrDefault();
                        if (attendanceTableItem!=null)
                            standardWorkHours = attendanceTableItem.udStandardWorkHours;

                        if (leaveDayByProfile.DurationType == LeaveDayDurationType.E_FULLSHIFT.ToString())
                        {   
                            //Lấy giờ nghỉ của ngày đang xét dựa vào ca làm việc
                            leaveHours = standardWorkHours;
                            leaveDays = 1;//Nghỉ full shift
                        }
                        else
                        {
                            leaveHours = leaveDayByProfile.LeaveHours.GetDouble();
                            leaveDays = leaveDayByProfile.LeaveDays.GetDouble();
                        }

                        leaveHours = leaveHours > standardWorkHours ? standardWorkHours : leaveHours;
                        leaveDays = leaveDays > 1 ? 1 : leaveDays;//Tối đa là nghỉ 1 ngày

                        if (leaveDayType != null && leaveDays > 0)
                        {
                            if (leaveDayType.Code == LeavedayTypeCode.CPS.ToString()
                                || leaveDayType.Code == LeavedayTypeCode.OCPS.ToString())
                            {
                                if ((leaveDayByProfile.DateOvertimeOff.HasValue
                                    && leaveDayByProfile.DateStart > attendanceTo)
                                    || date > attendanceTo)
                                {
                                    //Nghỉ CO tháng sau => coEndPeriod
                                    coEndPeriod += leaveDays;
                                }
                                else if (date >= attendanceFrom && date <= attendanceTo)
                                {
                                    //Nghỉ CO trong tháng => coBeginPeriod
                                    coBeginPeriod += leaveDays;
                                }
                            }
                        }
                    }
                }
            }

            attendanceTable.COBeginPeriod = coBeginPeriod;
            attendanceTable.COEndPeriod = coEndPeriod;

            #endregion

            #region Cập nhật Att_AttendanceTable sau khi tính xong

            #region Sum type HDTJob
            var _type4 = attendanceTable.Att_AttendanceTableItem.Where(d => d.HDTJobType == EnumDropDown.HDTJobType.E_TYPE4.ToString());
            var _type5 = attendanceTable.Att_AttendanceTableItem.Where(d => d.HDTJobType == EnumDropDown.HDTJobType.E_TYPE5.ToString());
            attendanceTable.HDTJobType1 = EnumDropDown.HDTJobType.E_TYPE4.ToString();
            attendanceTable.HDTJobType2 = EnumDropDown.HDTJobType.E_TYPE5.ToString();
            attendanceTable.HDTJobDayCount1 = _type4 != null ? _type4.Count() : 0;
            attendanceTable.HDTJobDayCount2 = _type5 != null ? _type5.Count() : 0;

            #endregion

            attendanceTable.RealWorkDayCount = attendanceTable.Att_AttendanceTableItem.Where(d => d.udWorkHours > 0).Sum(d => (d.WorkPaidHours + d.LateEarlyMinutes / 60.0) / (d.udStandardWorkHours * d.udStandardWorkHours / d.udWorkHours));
            attendanceTable.NightShiftHours = attendanceTable.Att_AttendanceTableItem.Where(d => d.DutyCode == DutyCode.E_ON.ToString()).Sum(d => d.NightShiftHours);
            attendanceTable.OTNightShiftHours = attendanceTable.Att_AttendanceTableItem.Where(d => d.DutyCode == DutyCode.E_ON.ToString()).Sum(d => d.OTNightShiftHours.GetDouble());
            attendanceTable.LateEarlyDeductionHours += attendanceTable.Att_AttendanceTableItem.Sum(d => d.LateEarlyMinutes) / 60.0;
            attendanceTable.UnPaidLeave = attendanceTable.Att_AttendanceTableItem.Sum(d => d.UnpaidLeaveDays.GetDouble());
            attendanceTable.PaidLeaveDays = attendanceTable.Att_AttendanceTableItem.Sum(d => d.PaidLeaveDays.GetDouble());

            //Tính nghỉ bù
            attendanceTable.COBeginPeriod = coBeginPeriod;
            attendanceTable.COEndPeriod = coEndPeriod;

            //Tính số ngày công chuẩn
            int weekendDaysCount = attendanceTable.Att_AttendanceTableItem.Where(d => d.IsHoliday).Count();
            attendanceTable.StdWorkDayCount = GetStandardWorkDays(gradeCfg, listHoliday, listShift, listMonthShiftID, monthYear, attendanceFrom, attendanceTo);
            attendanceTable.StdWorkDayOTCount = GetOTStandardWorkDays(weekendDaysCount, gradeCfg, listHoliday, listShift, listMonthShiftID, monthYear, attendanceFrom, attendanceTo);

            //Tổng số ngày được trả lương của nhân viên đang xét => ngày thực tế
            attendanceTable.PaidWorkDayCount = attendanceTable.RealWorkDayCount;

            if (gradeCfg.EDType == PayrollComputeMethod.E_SUBTRACT.ToString())
            {
                //Số ngày công thực tế + số ngày nghỉ hưởng lương
                Double workAndPaidLeaveDays = attendanceTable.RealWorkDayCount;
                double unHiredDays = 0; double terminatedDays = 0;

                if (attendanceTable.Att_AttendanceTableItem.Count() > 0)
                {
                    workAndPaidLeaveDays += attendanceTable.Att_AttendanceTableItem.Sum(d =>
                        d.PaidLeaveHours / d.udStandardWorkHours);
                }

                //Nếu nhân viên không vào làm hoặc nghỉ việc trong tháng và số ngày công thực tế + số ngày nghỉ hưởng lương > số ngày công tối thiểu
                if (((profile.DateHire.HasValue && profile.DateHire.Value.Date < attendanceFrom) || workAndPaidLeaveDays > attendanceTable.StdWorkDayCount)
                    && ((!profile.DateQuit.HasValue || profile.DateQuit.Value.Date > attendanceTo) || workAndPaidLeaveDays > attendanceTable.StdWorkDayCount)
                    && workAndPaidLeaveDays >= gradeCfg.MinWorkDay.Value)
                {
                    //Tổng số ngày nghỉ loại 1 và loại 2 khi có trường hợp 1 ngày mà nghỉ 2 loại
                    Double totalLeaveDays = attendanceTable.Att_AttendanceTableItem.Sum(d =>
                       (d.LeaveHours + d.ExtraLeaveHours) / d.udStandardWorkHours);

                    if (isMidCutOffDay)
                    {
                        totalLeaveDays = attendanceTable.Att_AttendanceTableItem.Where(d => d.WorkDate <= midCutOffDate)
                            .Sum(d => d.LeaveHours + d.ExtraLeaveHours / d.udStandardWorkHours);//Số ngày nghỉ không ứng công
                    }

                    //Cộng thêm số ngày chưa làm việc - trường hợp làm nửa ca
                    totalLeaveDays += attendanceTable.Att_AttendanceTableItem.Sum(d =>
                        d.udNotHiredHours / d.udStandardWorkHours);

                    if (profile.DateQuit.HasValue)
                    {
                        //Cộng thêm số ngày đã nghỉ việc
                        totalLeaveDays += attendanceTable.Att_AttendanceTableItem.Sum(d =>
                            d.udTerminatedHours / d.udStandardWorkHours);
                    }

                    //Tính số ngày công tính lương = số ngày công chuẩn - tổng số ngày nghỉ
                    attendanceTable.PaidWorkDayCount = attendanceTable.StdWorkDayCount - totalLeaveDays;

                    if (attendanceTable.PaidWorkDayCount < 0)
                    {
                        attendanceTable.PaidWorkDayCount = 0;
                    }
                }
                else if ((gradeCfg.IsApplySubSalaryNewQuitEmp.GetBoolean() && workAndPaidLeaveDays >= gradeCfg.MinWorkDayNewQuitEmp.GetDouble()) &&
                    ((profile.DateHire.HasValue && profile.DateHire.Value.Date >= attendanceFrom) || (profile.DateQuit.HasValue && profile.DateQuit.Value.Date <= attendanceTo)))
                {
                    Double totalLeaveDays = attendanceTable.Att_AttendanceTableItem.Sum(it => (it.LeaveHours + it.ExtraLeaveHours) / it.udStandardWorkHours);
                    Double DayNonShiftInUnEmployeeTime = 0;//Thời gian của nhân viên không có lịch làm việc trước khi vào công ty hoặc là sau khi ra khỏi công ty trong tháng

                    if (profile.DateHire.HasValue && profile.DateHire.Value > attendanceFrom)
                    {
                        unHiredDays = profile.DateHire.Value.Subtract(attendanceFrom).Days;

                        for (DateTime DateCheck = attendanceFrom; DateCheck < profile.DateHire.Value; DateCheck = DateCheck.AddDays(1))
                        {
                            if (listMonthShiftID == null || !listMonthShiftID.Any(d =>
                               d.Key.Date == DateCheck.Date && d.Value != null))
                            {
                                DayNonShiftInUnEmployeeTime++;
                            }
                        }
                    }

                    if (profile.DateQuit.HasValue && profile.DateQuit.Value < attendanceTo)
                    {
                        terminatedDays = attendanceTo.Subtract(profile.DateQuit.Value).Days + 1;
                        for (DateTime DateCheck = profile.DateQuit.Value; DateCheck < attendanceTo; DateCheck = DateCheck.AddDays(1))
                        {
                            if (listMonthShiftID == null || !listMonthShiftID.Any(d =>
                                d.Key.Date == DateCheck.Date && d.Value != null))
                            {
                                DayNonShiftInUnEmployeeTime++;
                            }
                        }
                    }

                    Double paidWorkDay = attendanceTable.StdWorkDayCount - totalLeaveDays;
                    paidWorkDay = paidWorkDay - (unHiredDays + terminatedDays - DayNonShiftInUnEmployeeTime) * gradeCfg.RateUneven.GetDouble();
                    attendanceTable.PaidWorkDayCount = paidWorkDay;
                }
            }

            #region Tính tổng thời gian theo từng loại ngày nghỉ và loại làm thêm

            TotalGroupByLeaveDayType(attendanceTable, attendanceTo);
            TotalGroupByOvertimeType(attendanceTable, attendanceTo);

            #endregion

            #region Tính tổng thời gian và làm tròn

            attendanceTable.TotalPaidWorkDayCount = attendanceTable.PaidWorkDayCount;
            attendanceTable.TotalRealWorkDayCount = attendanceTable.RealWorkDayCount;

            //Cấu hình làm tròng trễ sớm theo tháng
            if (gradeCfg != null && gradeCfg.IsLateEarlyFirstLastShiftRound == true)
            {
                attendanceTable.LateEarlyDeductionHours = Att_AttendanceLib.RoundLateEarlyMinutes(listLateEarlyRule.Where(d =>
                    d.GradeCfgID == gradeCfg.ID).ToList(), Convert.ToInt32(attendanceTable.LateEarlyDeductionHours * 60)) / 60;
            }

            #endregion

            #endregion

            return attendanceTable != null ? 1 : 0;
        }

        private DataErrorCode DeleteAttendance(DateTime monthYear, Guid cutOffDurationID, List<Guid> listProfileID)
        {
            #region Delete Attendance

            try
            {
                using (var context = new VnrHrmDataContext())
                {
                    var unitOfWork = new UnitOfWork(context);
                    int pageSize = 2000;//tối đa là 2100 parameter
                    int result = 0;

                    foreach (var listProfileIDBySize in listProfileID.Chunk(pageSize))
                    {
                        result += unitOfWork.SetIsDelete(context.Att_AttendanceTable.Where(d => !d.IsDelete.HasValue && d.MonthYear == monthYear
                            && d.CutOffDurationID == cutOffDurationID && listProfileIDBySize.Contains(d.ProfileID)));
                    }

                    return DataErrorCode.Success;
                }
            }
            catch (Exception)
            {
                return DataErrorCode.Error;
            }

            #endregion
        }

        #endregion

        public static TimeSpan GetIntersectAmount(DateTime In1, DateTime Out1, DateTime In2, DateTime Out2)
        {
            DateTime From = In2.CompareTo(In1) > 0 ? In2 : In1;
            DateTime To = Out2.CompareTo(Out1) < 0 ? Out2 : Out1;
            return (TimeSpan)To.Subtract(From);
        }

        public static Double GetInOutWorkingHour(Att_Workday inout, Cat_Shift shift)
        {
            DateTime workdate = new DateTime(inout.WorkDate.Year, inout.WorkDate.Month, inout.WorkDate.Day
                                            , shift.InTime.Hour, shift.InTime.Minute, shift.InTime.Second);
            DateTime shiftStart = workdate;
            DateTime shiftEnd = workdate.AddHours(shift.CoOut);

            return GetIntersectAmount(inout.InTime1.Value, inout.OutTime1.Value, shiftStart, shiftEnd).TotalHours;
        }

        public static Double GetShiftRosterShiftInOutHour(Att_Workday inout, Cat_Shift shift)
        {
            DateTime workdate = new DateTime(inout.WorkDate.Year, inout.WorkDate.Month, inout.WorkDate.Day
                                            , shift.InTime.Hour, shift.InTime.Minute, shift.InTime.Second);
            DateTime shiftStart = workdate;
            DateTime shiftEnd = workdate.AddHours(shift.CoOut);
            DateTime dateIn = inout.InTime1.Value;
            DateTime dateOut = inout.OutTime1.Value;

            Double _hrs = 0;
            if (dateIn.CompareTo(shiftStart) > 0)
                _hrs += dateIn.Subtract(shiftStart).TotalHours;
            else
                _hrs += shiftStart.Subtract(dateIn).TotalHours;

            if (dateOut.CompareTo(shiftEnd) > 0)
                _hrs += dateOut.Subtract(shiftEnd).TotalHours;
            else
                _hrs += shiftEnd.Subtract(dateOut).TotalHours;

            return _hrs;
        }

        public static void GetRosterGroup(List<Guid> lstProfileID, DateTime? DateFrom, DateTime? DateTo, out List<Att_Roster> lstRosterTypeGroup, out List<Att_RosterGroup> lstRosterGroup)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Roster = new CustomBaseRepository<Att_Roster>(unitOfWork);
                var repoAtt_RosterGroup = new CustomBaseRepository<Att_RosterGroup>(unitOfWork);

                lstRosterGroup = new List<Att_RosterGroup>();
                lstRosterTypeGroup = new List<Att_Roster>();
                string E_APPROVED = RosterStatus.E_APPROVED.ToString();
                string E_ROSTERGROUP = RosterType.E_ROSTERGROUP.ToString();
                if (DateFrom == null || DateTo == null)
                {
                    lstRosterTypeGroup = repoAtt_Roster.FindBy(m => lstProfileID.Contains(m.ProfileID) && m.Status == E_APPROVED && m.Type == E_ROSTERGROUP).ToList();
                    lstRosterGroup = repoAtt_RosterGroup.FindBy(m => m.DateStart != null && m.DateEnd != null).ToList();
                }
                else
                {
                    lstRosterTypeGroup = repoAtt_Roster.FindBy(m => lstProfileID.Contains(m.ProfileID) && m.Status == E_APPROVED && m.Type == E_ROSTERGROUP && m.DateStart <= DateTo).ToList();
                    lstRosterGroup = repoAtt_RosterGroup.FindBy(m => m.DateStart != null && m.DateEnd != null && m.DateStart <= DateTo && m.DateEnd >= DateFrom).ToList();
                }
            }
        }

        private static int GetDuration(Att_Workday workdayByProfile, DateTime timeShiftStart,
          DateTime timeShiftEnd, DateTime timeShiftBreakIn, DateTime timeShiftBreakOut, double? DynamicHour)
        {
            int duration = 0;
            #region Xử lý vấn đề Ca có giờ linh động cho VE VietNam Esports (Thái phụ trách) - Tính giờ làm việc linh động theo Ca Làm Việc
            DateTime? In1 = workdayByProfile.InTime1;
            if (In1 != null && In1 > timeShiftStart && DynamicHour != null && DynamicHour > 0)
            {
                double HourLate = (In1.Value - timeShiftStart).TotalHours;
                if (HourLate > DynamicHour)
                {
                    HourLate = DynamicHour.Value;
                }
                timeShiftEnd.AddHours(HourLate);
            }
            #endregion


            if (timeShiftStart >= timeShiftBreakIn && timeShiftStart <= timeShiftBreakOut)
            {
                duration += GetIntersectMinutes(workdayByProfile.InTime1, workdayByProfile.OutTime1, timeShiftBreakOut, timeShiftEnd);
                duration += GetIntersectMinutes(workdayByProfile.InTime2, workdayByProfile.OutTime2, timeShiftBreakOut, timeShiftEnd);
                duration += GetIntersectMinutes(workdayByProfile.InTime3, workdayByProfile.OutTime3, timeShiftBreakOut, timeShiftEnd);
                duration += GetIntersectMinutes(workdayByProfile.InTime4, workdayByProfile.OutTime4, timeShiftBreakOut, timeShiftEnd);
            }
            else if (timeShiftEnd >= timeShiftBreakIn && timeShiftEnd <= timeShiftBreakOut)
            {
                duration += GetIntersectMinutes(workdayByProfile.InTime1, workdayByProfile.OutTime1, timeShiftStart, timeShiftBreakIn);
                duration += GetIntersectMinutes(workdayByProfile.InTime2, workdayByProfile.OutTime2, timeShiftStart, timeShiftBreakIn);
                duration += GetIntersectMinutes(workdayByProfile.InTime3, workdayByProfile.OutTime3, timeShiftStart, timeShiftBreakIn);
                duration += GetIntersectMinutes(workdayByProfile.InTime4, workdayByProfile.OutTime4, timeShiftStart, timeShiftBreakIn);
            }
            else if (timeShiftEnd > timeShiftBreakOut && timeShiftStart < timeShiftBreakIn)
            {
                duration += GetIntersectMinutes(workdayByProfile.InTime1, workdayByProfile.OutTime1, timeShiftStart, timeShiftBreakIn);
                duration += GetIntersectMinutes(workdayByProfile.InTime1, workdayByProfile.OutTime1, timeShiftBreakOut, timeShiftEnd);

                duration += GetIntersectMinutes(workdayByProfile.InTime2, workdayByProfile.OutTime2, timeShiftStart, timeShiftBreakIn);
                duration += GetIntersectMinutes(workdayByProfile.InTime2, workdayByProfile.OutTime2, timeShiftBreakOut, timeShiftEnd);

                duration += GetIntersectMinutes(workdayByProfile.InTime3, workdayByProfile.OutTime3, timeShiftStart, timeShiftBreakIn);
                duration += GetIntersectMinutes(workdayByProfile.InTime3, workdayByProfile.OutTime3, timeShiftBreakOut, timeShiftEnd);

                duration += GetIntersectMinutes(workdayByProfile.InTime4, workdayByProfile.OutTime4, timeShiftStart, timeShiftBreakIn);
                duration += GetIntersectMinutes(workdayByProfile.InTime4, workdayByProfile.OutTime4, timeShiftBreakOut, timeShiftEnd);
            }
            else
            {
                duration += GetIntersectMinutes(workdayByProfile.InTime1, workdayByProfile.OutTime1, timeShiftStart, timeShiftEnd);
                duration += GetIntersectMinutes(workdayByProfile.InTime2, workdayByProfile.OutTime2, timeShiftStart, timeShiftEnd);
                duration += GetIntersectMinutes(workdayByProfile.InTime3, workdayByProfile.OutTime3, timeShiftStart, timeShiftEnd);
                duration += GetIntersectMinutes(workdayByProfile.InTime4, workdayByProfile.OutTime4, timeShiftStart, timeShiftEnd);
            }

            return duration;
        }

        #region GetIntersectMinutes

        /// <summary>
        /// Lay mien giao giua hai khoang thoi gian
        /// </summary>
        /// <param name="In1"></param>
        /// <param name="Out1"></param>
        /// <param name="In2"></param>
        /// <param name="Out2"></param>
        /// <returns>don vi giao nhau la phut</returns>
        public static Int32 GetIntersectMinutes(DateTime? In1,
            DateTime? Out1, DateTime? In2, DateTime? Out2)
        {
            Double result = 0;

            if (In1 != null && Out1 != null && In2 != null && Out2 != null)
            {
                DateTime From = In2.Value.CompareTo(In1.Value) > 0 ? In2.Value : In1.Value;
                DateTime To = Out2.Value.CompareTo(Out1.Value) < 0 ? Out2.Value : Out1.Value;

                From = From.AddSeconds(-From.Second);
                To = To.AddSeconds(-To.Second);

                result = To.Subtract(From).TotalMinutes;
                result = result > 0 ? result : 0;
            }

            return (Int32)Math.Round(result, 0);
        }

        #endregion

        #region TotalGroupByOvertimeType

        /// <summary>
        /// Tính tổng thời gian làm thêm theo từng loại trên chi tiết gán vào Att_AttendanceTable 
        /// </summary>
        /// <param name="attendanceTable"></param>
        /// <param name="midCutOffDate"></param>
        private static void TotalGroupByOvertimeType(Att_AttendanceTable attendanceTable, DateTime midCutOffDate)
        {
            //Nếu làm thêm nhiều loại làm thêm khác nhau thì ưu tiên cột Cat_OvertimeType trước...
            List<Att_AttendanceTableItem> listAttendanceTableItem = attendanceTable.Att_AttendanceTableItem.Where(d => d.WorkDate <= midCutOffDate
                && (d.Cat_OvertimeType != null || d.Cat_OvertimeType1 != null || d.Cat_OvertimeType2 != null || d.Cat_OvertimeType3 != null)).ToList();

            foreach (Att_AttendanceTableItem item in listAttendanceTableItem)
            {
                #region item.Cat_OvertimeType != null

                if (item.Cat_OvertimeType != null)
                {
                    if (attendanceTable.Cat_OvertimeType == null || attendanceTable.Cat_OvertimeType == item.Cat_OvertimeType)
                    {
                        attendanceTable.Cat_OvertimeType = item.Cat_OvertimeType;
                        attendanceTable.Overtime1Hours += item.OvertimeHours;
                    }
                    else if (attendanceTable.Cat_OvertimeType1 == null || attendanceTable.Cat_OvertimeType1 == item.Cat_OvertimeType)
                    {
                        attendanceTable.Cat_OvertimeType1 = item.Cat_OvertimeType;
                        attendanceTable.Overtime2Hours += item.OvertimeHours;
                    }
                    else if (attendanceTable.Cat_OvertimeType2 == null || attendanceTable.Cat_OvertimeType2 == item.Cat_OvertimeType)
                    {
                        attendanceTable.Cat_OvertimeType2 = item.Cat_OvertimeType;
                        attendanceTable.Overtime3Hours += item.OvertimeHours;
                    }
                    else if (attendanceTable.Cat_OvertimeType3 == null || attendanceTable.Cat_OvertimeType3 == item.Cat_OvertimeType)
                    {
                        attendanceTable.Cat_OvertimeType3 = item.Cat_OvertimeType;
                        attendanceTable.Overtime4Hours += item.OvertimeHours;
                    }
                    else if (attendanceTable.Cat_OvertimeType4 == null || attendanceTable.Cat_OvertimeType4 == item.Cat_OvertimeType)
                    {
                        attendanceTable.Cat_OvertimeType4 = item.Cat_OvertimeType;
                        attendanceTable.Overtime4Hours += item.OvertimeHours;
                    }
                    else if (attendanceTable.Cat_OvertimeType5 == null || attendanceTable.Cat_OvertimeType5 == item.Cat_OvertimeType)
                    {
                        attendanceTable.Cat_OvertimeType5 = item.Cat_OvertimeType;
                        attendanceTable.Overtime4Hours += item.OvertimeHours;
                    }
                }

                #endregion

                #region item.Cat_OvertimeType1 != null

                if (item.Cat_OvertimeType1 != null)
                {
                    if (attendanceTable.Cat_OvertimeType == null || attendanceTable.Cat_OvertimeType == item.Cat_OvertimeType1)
                    {
                        attendanceTable.Cat_OvertimeType = item.Cat_OvertimeType1;
                        attendanceTable.Overtime1Hours += item.ExtraOvertimeHours;
                    }
                    else if (attendanceTable.Cat_OvertimeType1 == null || attendanceTable.Cat_OvertimeType1 == item.Cat_OvertimeType1)
                    {
                        attendanceTable.Cat_OvertimeType1 = item.Cat_OvertimeType1;
                        attendanceTable.Overtime2Hours += item.ExtraOvertimeHours;
                    }
                    else if (attendanceTable.Cat_OvertimeType2 == null || attendanceTable.Cat_OvertimeType2 == item.Cat_OvertimeType1)
                    {
                        attendanceTable.Cat_OvertimeType2 = item.Cat_OvertimeType1;
                        attendanceTable.Overtime3Hours += item.ExtraOvertimeHours;
                    }
                    else if (attendanceTable.Cat_OvertimeType3 == null || attendanceTable.Cat_OvertimeType3 == item.Cat_OvertimeType1)
                    {
                        attendanceTable.Cat_OvertimeType3 = item.Cat_OvertimeType1;
                        attendanceTable.Overtime4Hours += item.ExtraOvertimeHours;
                    }
                    else if (attendanceTable.Cat_OvertimeType4 == null || attendanceTable.Cat_OvertimeType4 == item.Cat_OvertimeType1)
                    {
                        attendanceTable.Cat_OvertimeType4 = item.Cat_OvertimeType1;
                        attendanceTable.Overtime4Hours += item.ExtraOvertimeHours;
                    }
                    else if (attendanceTable.Cat_OvertimeType5 == null || attendanceTable.Cat_OvertimeType5 == item.Cat_OvertimeType1)
                    {
                        attendanceTable.Cat_OvertimeType5 = item.Cat_OvertimeType1;
                        attendanceTable.Overtime4Hours += item.ExtraOvertimeHours;
                    }
                }

                #endregion

                #region item.Cat_OvertimeType2 != null

                if (item.Cat_OvertimeType2 != null)
                {
                    if (attendanceTable.Cat_OvertimeType == null || attendanceTable.Cat_OvertimeType == item.Cat_OvertimeType2)
                    {
                        attendanceTable.Cat_OvertimeType = item.Cat_OvertimeType2;
                        attendanceTable.Overtime1Hours += item.ExtraOvertimeHours2;
                    }
                    else if (attendanceTable.Cat_OvertimeType1 == null || attendanceTable.Cat_OvertimeType1 == item.Cat_OvertimeType2)
                    {
                        attendanceTable.Cat_OvertimeType1 = item.Cat_OvertimeType2;
                        attendanceTable.Overtime2Hours += item.ExtraOvertimeHours2;
                    }
                    else if (attendanceTable.Cat_OvertimeType2 == null || attendanceTable.Cat_OvertimeType2 == item.Cat_OvertimeType2)
                    {
                        attendanceTable.Cat_OvertimeType2 = item.Cat_OvertimeType2;
                        attendanceTable.Overtime3Hours += item.ExtraOvertimeHours2;
                    }
                    else if (attendanceTable.Cat_OvertimeType3 == null || attendanceTable.Cat_OvertimeType3 == item.Cat_OvertimeType2)
                    {
                        attendanceTable.Cat_OvertimeType3 = item.Cat_OvertimeType2;
                        attendanceTable.Overtime4Hours += item.ExtraOvertimeHours2;
                    }
                    else if (attendanceTable.Cat_OvertimeType4 == null || attendanceTable.Cat_OvertimeType4 == item.Cat_OvertimeType2)
                    {
                        attendanceTable.Cat_OvertimeType4 = item.Cat_OvertimeType2;
                        attendanceTable.Overtime4Hours += item.ExtraOvertimeHours2;
                    }
                    else if (attendanceTable.Cat_OvertimeType5 == null || attendanceTable.Cat_OvertimeType5 == item.Cat_OvertimeType2)
                    {
                        attendanceTable.Cat_OvertimeType5 = item.Cat_OvertimeType2;
                        attendanceTable.Overtime4Hours += item.ExtraOvertimeHours2;
                    }
                }

                #endregion

                #region item.Cat_OvertimeType3 != null

                if (item.Cat_OvertimeType3 != null)
                {
                    if (attendanceTable.Cat_OvertimeType == null || attendanceTable.Cat_OvertimeType == item.Cat_OvertimeType3)
                    {
                        attendanceTable.Cat_OvertimeType = item.Cat_OvertimeType3;
                        attendanceTable.Overtime1Hours += item.ExtraOvertimeHours3;
                    }
                    else if (attendanceTable.Cat_OvertimeType1 == null || attendanceTable.Cat_OvertimeType1 == item.Cat_OvertimeType3)
                    {
                        attendanceTable.Cat_OvertimeType1 = item.Cat_OvertimeType3;
                        attendanceTable.Overtime2Hours += item.ExtraOvertimeHours3;
                    }
                    else if (attendanceTable.Cat_OvertimeType2 == null || attendanceTable.Cat_OvertimeType2 == item.Cat_OvertimeType3)
                    {
                        attendanceTable.Cat_OvertimeType2 = item.Cat_OvertimeType3;
                        attendanceTable.Overtime3Hours += item.ExtraOvertimeHours3;
                    }
                    else if (attendanceTable.Cat_OvertimeType3 == null || attendanceTable.Cat_OvertimeType3 == item.Cat_OvertimeType3)
                    {
                        attendanceTable.Cat_OvertimeType3 = item.Cat_OvertimeType3;
                        attendanceTable.Overtime4Hours += item.ExtraOvertimeHours3;
                    }
                    else if (attendanceTable.Cat_OvertimeType4 == null || attendanceTable.Cat_OvertimeType4 == item.Cat_OvertimeType3)
                    {
                        attendanceTable.Cat_OvertimeType4 = item.Cat_OvertimeType3;
                        attendanceTable.Overtime4Hours += item.ExtraOvertimeHours3;
                    }
                    else if (attendanceTable.Cat_OvertimeType5 == null || attendanceTable.Cat_OvertimeType5 == item.Cat_OvertimeType3)
                    {
                        attendanceTable.Cat_OvertimeType5 = item.Cat_OvertimeType3;
                        attendanceTable.Overtime4Hours += item.ExtraOvertimeHours3;
                    }
                }

                #endregion
            }
        }

        #endregion

        #region TotalGroupByLeaveDayType

        /// <summary>
        /// Tính tổng thời gian nghỉ theo từng loại trên chi tiết gán vào Att_AttendanceTable
        /// </summary>
        /// <param name="attendanceTable"></param>
        /// <param name="midCutOffDate"></param>
        private static void TotalGroupByLeaveDayType(Att_AttendanceTable attendanceTable, DateTime midCutOffDate)
        {
            #region Nghỉ mà xem như đi làm - Loại IsWorkDay

            List<Att_AttendanceTableItem> listAttendanceTableItemCutOff = attendanceTable
                .Att_AttendanceTableItem.Where(d => d.WorkDate <= midCutOffDate).ToList();

            //Nếu nghỉ 2 loại ngày nghỉ khác nhau và cùng là IsWorkDay thì ưu tiên cột Cat_LeaveDayType4 trước...
            List<Att_AttendanceTableItem> listAttendanceTableItemIsWorkDay = listAttendanceTableItemCutOff.Where(d =>
                d.Cat_LeaveDayType2 != null && !d.Cat_LeaveDayType2.IsAnnualLeave && d.Cat_LeaveDayType2.IsWorkDay).ToList();

            foreach (Att_AttendanceTableItem itemIsWorkDay in listAttendanceTableItemIsWorkDay)
            {
                if (attendanceTable.Cat_LeaveDayType4 == null || attendanceTable.Cat_LeaveDayType4 == itemIsWorkDay.Cat_LeaveDayType2)
                {
                    attendanceTable.Cat_LeaveDayType4 = itemIsWorkDay.Cat_LeaveDayType2;
                    attendanceTable.LeaveDay4Hours = attendanceTable.LeaveDay4Hours + itemIsWorkDay.LeaveWorkDayHour.Get_Double();
                    attendanceTable.LeaveDay4Days = attendanceTable.LeaveDay4Days.Get_Double() + itemIsWorkDay.LeaveWorkDayDays.Get_Double();
                }
                //else if (attendanceTable.Cat_LeaveDayType5 == null || attendanceTable.Cat_LeaveDayType5 == itemIsWorkDay.Cat_LeaveDayType2)
                //{
                //    attendanceTable.Cat_LeaveDayType5 = itemIsWorkDay.Cat_LeaveDayType2;
                //    attendanceTable.LeaveWorkDay2Hour = attendanceTable.LeaveWorkDay2Hour.Get_Double() + itemIsWorkDay.LeaveWorkDayHour.Get_Double();
                //    attendanceTable.LeaveWorkDay2Days = attendanceTable.LeaveWorkDay2Days.Get_Double() + itemIsWorkDay.LeaveWorkDayDays.Get_Double();
                //}
            }

            #endregion

            #region Những ngày nghỉ bình thường khác

            //Nếu nghỉ nhiều loại ngày nghỉ khác nhau thì ưu tiên cột Cat_LeaveDayType trước...
            List<Att_AttendanceTableItem> listAttendanceTableItemOther = listAttendanceTableItemCutOff.Where(d => (d.Cat_LeaveDayType != null
                && !d.Cat_LeaveDayType.IsAnnualLeave) || (d.Cat_LeaveDayType1 != null && !d.Cat_LeaveDayType1.IsAnnualLeave)).ToList();

            attendanceTable.PaidWorkDayCount -= listAttendanceTableItemOther.Where(d => d.Cat_LeaveDayType != null && !d.Cat_LeaveDayType.IsAnnualLeave
                && d.Cat_LeaveDayType.Penalty > 0).Sum(d => (d.LeaveHours * d.Cat_LeaveDayType.Penalty) / d.udStandardWorkHours);

            attendanceTable.PaidWorkDayCount -= listAttendanceTableItemOther.Where(d => d.Cat_LeaveDayType1 != null && !d.Cat_LeaveDayType1.IsAnnualLeave
                && d.Cat_LeaveDayType1.Penalty > 0).Sum(d => (d.ExtraLeaveHours * d.Cat_LeaveDayType1.Penalty) / d.udStandardWorkHours);

            foreach (Att_AttendanceTableItem itemOther in listAttendanceTableItemOther)
            {
                if (itemOther.Cat_LeaveDayType != null && !itemOther.Cat_LeaveDayType.IsAnnualLeave)
                {
                    if (attendanceTable.Cat_LeaveDayType == null || attendanceTable.Cat_LeaveDayType == itemOther.Cat_LeaveDayType)
                    {
                        attendanceTable.Cat_LeaveDayType = itemOther.Cat_LeaveDayType;
                        attendanceTable.LeaveDay1Hours = attendanceTable.LeaveDay1Hours + itemOther.LeaveHours;
                        attendanceTable.LeaveDay1Days = attendanceTable.LeaveDay1Days.Get_Double() + itemOther.LeaveDays.Get_Double();
                    }
                    else if (attendanceTable.Cat_LeaveDayType1 == null || attendanceTable.Cat_LeaveDayType1 == itemOther.Cat_LeaveDayType)
                    {
                        attendanceTable.Cat_LeaveDayType1 = itemOther.Cat_LeaveDayType;
                        attendanceTable.LeaveDay2Hours = attendanceTable.LeaveDay2Hours + itemOther.LeaveHours;
                        attendanceTable.LeaveDay2Days = attendanceTable.LeaveDay2Days.Get_Double() + itemOther.LeaveDays.Get_Double();
                    }
                    else if (attendanceTable.Cat_LeaveDayType2 == null || attendanceTable.Cat_LeaveDayType2 == itemOther.Cat_LeaveDayType)
                    {
                        attendanceTable.Cat_LeaveDayType2 = itemOther.Cat_LeaveDayType;
                        attendanceTable.LeaveDay3Hours = attendanceTable.LeaveDay3Hours + itemOther.LeaveHours;
                        attendanceTable.LeaveDay3Days = attendanceTable.LeaveDay3Days.Get_Double() + itemOther.LeaveDays.Get_Double();
                    }
                    else if (attendanceTable.Cat_LeaveDayType3 == null || attendanceTable.Cat_LeaveDayType3 == itemOther.Cat_LeaveDayType)
                    {
                        attendanceTable.Cat_LeaveDayType3 = itemOther.Cat_LeaveDayType;
                        attendanceTable.LeaveDay4Hours = attendanceTable.LeaveDay4Hours + itemOther.LeaveHours;
                        attendanceTable.LeaveDay4Days = attendanceTable.LeaveDay4Days.Get_Double() + itemOther.LeaveDays.Get_Double();
                    }
                }

                if (itemOther.Cat_LeaveDayType1 != null && !itemOther.Cat_LeaveDayType1.IsAnnualLeave)
                {
                    if (attendanceTable.Cat_LeaveDayType == null || attendanceTable.Cat_LeaveDayType == itemOther.Cat_LeaveDayType1)
                    {
                        attendanceTable.Cat_LeaveDayType = itemOther.Cat_LeaveDayType1;
                        attendanceTable.LeaveDay1Hours = attendanceTable.LeaveDay1Hours + itemOther.ExtraLeaveHours;
                        attendanceTable.LeaveDay1Days = attendanceTable.LeaveDay1Days.Get_Double() + itemOther.ExtraLeaveDays.Get_Double();
                    }
                    else if (attendanceTable.Cat_LeaveDayType1 == null || attendanceTable.Cat_LeaveDayType1 == itemOther.Cat_LeaveDayType1)
                    {
                        attendanceTable.Cat_LeaveDayType1 = itemOther.Cat_LeaveDayType1;
                        attendanceTable.LeaveDay2Hours = attendanceTable.LeaveDay2Hours + itemOther.ExtraLeaveHours;
                        attendanceTable.LeaveDay2Days = attendanceTable.LeaveDay2Days.Get_Double() + itemOther.ExtraLeaveDays.Get_Double();
                    }
                    else if (attendanceTable.Cat_LeaveDayType2 == null || attendanceTable.Cat_LeaveDayType2 == itemOther.Cat_LeaveDayType1)
                    {
                        attendanceTable.Cat_LeaveDayType2 = itemOther.Cat_LeaveDayType1;
                        attendanceTable.LeaveDay3Hours = attendanceTable.LeaveDay3Hours + itemOther.ExtraLeaveHours;
                        attendanceTable.LeaveDay3Days = attendanceTable.LeaveDay3Days.Get_Double() + itemOther.ExtraLeaveDays.Get_Double();
                    }
                    else if (attendanceTable.Cat_LeaveDayType3 == null || attendanceTable.Cat_LeaveDayType3 == itemOther.Cat_LeaveDayType1)
                    {
                        attendanceTable.Cat_LeaveDayType3 = itemOther.Cat_LeaveDayType1;
                        attendanceTable.LeaveDay4Hours = attendanceTable.LeaveDay4Hours + itemOther.ExtraLeaveHours;
                        attendanceTable.LeaveDay4Days = attendanceTable.LeaveDay4Days.Get_Double() + itemOther.ExtraLeaveDays.Get_Double();
                    }
                }
            }

            #endregion
        }

        #endregion

        #region GetStandardWorkDays

        public static Double GetStandardWorkDays(Cat_GradeAttendance gradeCfg, List<Cat_DayOff> listDayOff, List<Cat_Shift> listShift,
            Dictionary<DateTime, List<Guid?>> listMonthShifts, DateTime monthYear, DateTime dateFrom, DateTime dateTo)
        {
            Double result = 0;

            int holidayCount = listDayOff.Where(d => d.DateOff >= dateFrom
                && d.DateOff <= dateTo).Count();

            if (gradeCfg.IsFixedLeave.GetBoolean())
            {
                result = gradeCfg.LeaveWorkDayFix.GetDouble();
            }
            else if (gradeCfg.IsActualLeave.GetBoolean())
            {
                result = dateTo.Subtract(dateFrom).Days + 1;

                if (gradeCfg.LeaveWorkdayActual.HasValue)
                {
                    result += gradeCfg.LeaveWorkdayActual.Value;
                }
            }
            else if (gradeCfg.IsRosterLeave.HasValue && gradeCfg.IsRosterLeave.Value)
            {
                if (gradeCfg.RosterType == GradeRosterType.E_ISROSTER.ToString())
                {
                    result = GetRosterStandardWorkDays(gradeCfg, listShift, listMonthShifts, dateFrom, dateTo);
                }
                else if (gradeCfg.RosterType == GradeRosterType.E_ISDEFAULT.ToString())
                {
                    result = GetGradeStandardWorkDays(gradeCfg, dateFrom, dateTo);
                }
                else if (gradeCfg.RosterType == GradeRosterType.E_ISHOLIDAY.ToString())
                {
                    int monthDayCount = dateTo.Subtract(dateFrom).Days + 1;
                    result = monthDayCount - holidayCount;
                }
            }
            //else if (gradeCfg.IsFormulaLeave.HasValue && gradeCfg.IsFormulaLeave.Value)
            else if (gradeCfg.LeaveWorkdayFormula != null)
            {
                Formula formula = new Formula(gradeCfg.LeaveWorkdayFormula);
                if (gradeCfg.LeaveWorkdayFormula.Contains(Formula.FormulaConstant.DAYS_IN_MONTH_EXCLUDE_SUNDAY.ToString()))
                {
                    formula.Parameters.Add(Formula.FormulaConstant.DAYS_IN_MONTH_EXCLUDE_SUNDAY.ToString(),
                        GetDaysInMonthExcludeSunday(gradeCfg, monthYear));
                }

                if (gradeCfg.LeaveWorkdayFormula.Contains(Formula.FormulaConstant.DAYS_IN_MONTH_EXCLUDE_SUNDAY_HOLIDAY.ToString()))
                {
                    formula.Parameters.Add(Formula.FormulaConstant.DAYS_IN_MONTH_EXCLUDE_SUNDAY_HOLIDAY.ToString(),
                        GetDaysInMonthExcludeSunday(gradeCfg, monthYear) - holidayCount);
                }

                if (gradeCfg.LeaveWorkdayFormula.Contains(Formula.FormulaConstant.WORKDAY_STANDARD_PER_MONTH.ToString()))
                {
                    formula.Parameters.Add(Formula.FormulaConstant.WORKDAY_STANDARD_PER_MONTH.ToString(),
                        GetWorkdayStandardPerMonth(listDayOff, dateFrom, dateTo));
                }

                if (gradeCfg.LeaveWorkdayFormula.Contains(Formula.FormulaConstant.D.ToString()))
                {
                    formula.Parameters.Add(Formula.FormulaConstant.D.ToString(), dateTo.Subtract(dateFrom).Days + 1);
                }

                if (gradeCfg.LeaveWorkdayFormula.Contains(Formula.FormulaConstant.H.ToString()))
                {
                    formula.Parameters.Add(Formula.FormulaConstant.H.ToString(), holidayCount);
                }

                if (gradeCfg.LeaveWorkdayFormula.Contains(Formula.FormulaConstant.S.ToString()))
                {
                    int sundayCount = 0;

                    for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
                    {
                        if (date.DayOfWeek == DayOfWeek.Sunday)
                        {
                            sundayCount++;
                        }
                    }

                    formula.Parameters.Add(Formula.FormulaConstant.S.ToString(), sundayCount);
                }

                if (gradeCfg.LeaveWorkdayFormula.Contains(Formula.FormulaConstant.R.ToString()))
                {
                    Double count = GetRosterStandardWorkDays(gradeCfg, listShift, listMonthShifts, dateFrom, dateTo);
                    formula.Parameters.Add(Formula.FormulaConstant.R.ToString(), count);
                }

                if (gradeCfg.LeaveWorkdayFormula.Contains(Formula.FormulaConstant.R_NOT_H.ToString()))
                {
                    Double count = GetRosterSdNotHoliDays(listMonthShifts, listDayOff, dateFrom, dateTo);
                    formula.Parameters.Add(Formula.FormulaConstant.R_NOT_H.ToString(), count);
                }

                if (gradeCfg.LeaveWorkdayFormula.Contains(Formula.FormulaConstant.SAT.ToString()))
                {
                    int satdayCount = 0;

                    for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
                    {
                        if (date.DayOfWeek == DayOfWeek.Saturday)
                        {
                            satdayCount++;
                        }
                    }

                    formula.Parameters.Add(Formula.FormulaConstant.SAT.ToString(), satdayCount);
                }

                if (gradeCfg.LeaveWorkdayFormula.Contains(Formula.FormulaConstant.SHIFT_COUNT.ToString()))
                {
                    int ShiftCount = 0;

                    for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
                    {
                        if (listMonthShifts.ContainsKey(date) && listMonthShifts[date] != null)
                        {
                            ShiftCount++;
                        }
                    }

                    formula.Parameters.Add(Formula.FormulaConstant.SHIFT_COUNT.ToString(), ShiftCount);
                }

                result = Convert.ToDouble(formula.Evaluate());
            }

            return result;
        }

        private static double GetRosterStandardWorkDays(Cat_GradeAttendance gradeCfg, List<Cat_Shift> listShift,
            Dictionary<DateTime, List<Guid?>> listMonthShifts, DateTime dateFrom, DateTime dateTo)
        {
            Double result = 0;

            for (DateTime date = dateFrom.Date; date <= dateTo.Date; date = date.AddDays(1))
            {
                if (listMonthShifts.ContainsKey(date))
                {
                    if (listMonthShifts != null && listMonthShifts.Any(d =>
                        d.Key.Date == date.Date && d.Value != null))
                    {
                        var shiftByDate = listMonthShifts.Where(d => d.Key.Date == date.Date
                              && d.Value != null).Select(d => d.Value).FirstOrDefault();

                        Guid? shiftID1 = Guid.Empty;
                        Guid? shiftID2 = Guid.Empty;

                        if (shiftByDate != null)
                        {
                            if (shiftByDate.Count() > 0)
                            {
                                shiftID1 = shiftByDate[0];
                            }

                            if (shiftByDate.Count() > 1)
                            {
                                shiftID2 = shiftByDate[1];
                            }
                        }

                        var shift1 = listShift.Where(p => p.ID == shiftID1).FirstOrDefault();
                        var shift2 = listShift.Where(p => p.ID == shiftID2).FirstOrDefault();

                        if (shift1 != null && shift1.WorkHours.HasValue)
                        {
                            result += shift1.WorkHours.Value / shift1.udStandardWorkHours;
                        }

                        if (shift2 != null && shift2.WorkHours.HasValue)
                        {
                            result += shift2.WorkHours.Value / shift2.udStandardWorkHours;
                        }
                    }
                }
            }

            return result;
        }

        private static double GetRosterSdNotHoliDays(Dictionary<DateTime, List<Guid?>> listMonthShifts,
            List<Cat_DayOff> listDayOff, DateTime dateFrom, DateTime dateTo)
        {
            int result = 0;

            listDayOff = listDayOff.Where(d => d.Type == HolidayType.E_HOLIDAY_HLD.ToString()).ToList();
            List<DateTime> listDate = listDayOff.Select(d => d.DateOff).ToList();

            for (DateTime date = dateFrom.Date; date <= dateTo.Date; date = date.AddDays(1))
            {
                if (listMonthShifts != null && listMonthShifts.Any(d =>
                    d.Key.Date == date.Date && d.Value != null))
                {
                    if (!listDate.Contains(date))
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        private static double GetWorkdayStandardPerMonth(List<Cat_DayOff> listDayoff,
            DateTime dateFrom, DateTime dateTo)
        {
            double result = 0;

            double countdayOfMonth = dateTo.Subtract(dateFrom).TotalDays;
            double countSaturdayDayOffInMonth = Common.getAllSundayInMonth(dateFrom, dateTo);
            listDayoff = listDayoff.Where(d => d.DateOff >= dateFrom && d.DateOff <= dateTo).ToList();

            foreach (Cat_DayOff item in listDayoff)
            {
                if (item.Type == HolidayType.E_WEEKEND_HLD.ToString()
                    || (item.IsLeaveDay != null && item.IsLeaveDay.Value))
                {
                    countSaturdayDayOffInMonth++;
                }
            }

            result = countdayOfMonth - (countSaturdayDayOffInMonth);
            return Math.Round(result, 2);
        }

        private static int GetDaysInMonthExcludeSunday(Cat_GradeAttendance gradeCfg, DateTime monthYear)
        {
            int result = 0;
            DateTime dateFrom, dateTo;

            gradeCfg.GetSalaryDateRange(monthYear, out dateFrom, out dateTo);
            for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Sunday)
                {
                    result++;
                }
            }

            return result;
        }

        private static double GetGradeStandardWorkDays(Cat_GradeAttendance gradeCfg, DateTime dateFrom, DateTime dateTo)
        {
            Double result = 0;

            for (DateTime date = dateFrom.Date; date <= dateTo.Date; date = date.AddDays(1))
            {
                if (gradeCfg.RosterType == GradeRosterType.E_ISDEFAULT.ToString())
                {
                    if (Att_WorkDayHelper.IsWorkDay(date, gradeCfg, null))
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        #endregion

        #region GetOTStandardWorkDays

        public static Double GetOTStandardWorkDays(int holidayCount, Cat_GradeAttendance gradeCfg, List<Cat_DayOff> listDayOff,
            List<Cat_Shift> listShift, Dictionary<DateTime, List<Guid?>> listMonthShifts, DateTime monthYear, DateTime dateFrom, DateTime dateTo)
        {
            Double result = 0;

            if (gradeCfg.IsFixedOT.HasValue && gradeCfg.IsFixedOT.Value)
            {
                result = gradeCfg.OTWorkDayFix.GetDouble();
            }
            else if (gradeCfg.IsActualOT.HasValue && gradeCfg.IsActualOT.Value)
            {
                result = dateTo.Subtract(dateFrom).Days + 1;

                if (gradeCfg.IsActualOT.GetBoolean())
                {
                    result += gradeCfg.OTWorkdayActual.GetInteger();
                }
            }
            else if (gradeCfg.IsRosterOT.HasValue && gradeCfg.IsRosterOT.Value)
            {
                if (gradeCfg.RosterType == GradeRosterType.E_ISROSTER.ToString())
                {
                    result = GetRosterStandardWorkDays(gradeCfg, listShift, listMonthShifts, dateFrom, dateTo);
                }
                else if (gradeCfg.RosterType == GradeRosterType.E_ISDEFAULT.ToString())
                {
                    result = GetGradeStandardWorkDays(gradeCfg, dateFrom, dateTo);
                }
                else if (gradeCfg.RosterType == GradeRosterType.E_ISHOLIDAY.ToString())
                {
                    int monthDayCount = dateTo.Subtract(dateFrom).Days + 1;
                    result = monthDayCount - holidayCount;
                }
            }
            else if (gradeCfg.IsFormulaOT.HasValue && gradeCfg.IsFormulaOT.Value)
            {
                Formula formula = new Formula(gradeCfg.OTWorkdayFormula);
                if (gradeCfg.OTWorkdayFormula.Contains(Formula.FormulaConstant.DAYS_IN_MONTH_EXCLUDE_SUNDAY.ToString()))
                {
                    formula.Parameters.Add(Formula.FormulaConstant.DAYS_IN_MONTH_EXCLUDE_SUNDAY.ToString(),
                        GetDaysInMonthExcludeSunday(gradeCfg, monthYear));
                }
                if (gradeCfg.OTWorkdayFormula.Contains(Formula.FormulaConstant.DAYS_IN_MONTH_EXCLUDE_SUNDAY_HOLIDAY.ToString()))
                {
                    formula.Parameters.Add(Formula.FormulaConstant.DAYS_IN_MONTH_EXCLUDE_SUNDAY_HOLIDAY.ToString(),
                        GetDaysInMonthExcludeSunday(gradeCfg, monthYear) - holidayCount);
                }
                if (gradeCfg.OTWorkdayFormula.Contains(Formula.FormulaConstant.WORKDAY_AVERAGE_PER_MONTH.ToString()))
                {
                    formula.Parameters.Add(Formula.FormulaConstant.WORKDAY_AVERAGE_PER_MONTH.ToString(),
                        GetStandardWorkDays(monthYear.Year, listDayOff));
                }
                if (gradeCfg.OTWorkdayFormula.Contains(Formula.FormulaConstant.D.ToString()))
                {
                    formula.Parameters.Add(Formula.FormulaConstant.D.ToString(), dateTo.Subtract(dateFrom).Days + 1);
                }
                if (gradeCfg.OTWorkdayFormula.Contains(Formula.FormulaConstant.H.ToString()))
                {
                    formula.Parameters.Add(Formula.FormulaConstant.H.ToString(), holidayCount);
                }
                if (gradeCfg.OTWorkdayFormula.Contains(Formula.FormulaConstant.S.ToString()))
                {
                    int sundayCount = 0;

                    for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
                    {
                        if (date.DayOfWeek == DayOfWeek.Sunday)
                        {
                            sundayCount++;
                        }
                    }

                    formula.Parameters.Add(Formula.FormulaConstant.S.ToString(), sundayCount);
                }
                if (gradeCfg.OTWorkdayFormula.Contains(Formula.FormulaConstant.R.ToString()))
                {
                    Double count = GetRosterStandardWorkDays(gradeCfg, listShift, listMonthShifts, dateFrom, dateTo);
                    formula.Parameters.Add(Formula.FormulaConstant.R.ToString(), count);
                }
                if (gradeCfg.OTWorkdayFormula.Contains(Formula.FormulaConstant.R_NOT_H.ToString()))
                {
                    Double count = GetRosterSdNotHoliDays(listMonthShifts, listDayOff, dateFrom, dateTo);
                    formula.Parameters.Add(Formula.FormulaConstant.R_NOT_H.ToString(), count);
                }
                if (gradeCfg.OTWorkdayFormula.Contains(Formula.FormulaConstant.SAT.ToString()))
                {
                    int satdayCount = 0;

                    for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
                    {
                        if (date.DayOfWeek == DayOfWeek.Saturday)
                        {
                            satdayCount++;
                        }
                    }

                    formula.Parameters.Add(Formula.FormulaConstant.SAT.ToString(), satdayCount);
                }

                result = Convert.ToDouble(formula.Evaluate());
            }

            return result;
        }

        public static double GetStandardWorkDays(int year, List<Cat_DayOff> listDayoff)
        {
            double result = 0;

            listDayoff = listDayoff.Where(df => df.DateOff != null && df.DateOff.Year == year).ToList();
            double countSaturdayDayOffInYear = Common.getAllSundayInYear(DateTime.Now.Year);
            double countdayOfYear = Common.GetDaysInAYear(DateTime.Now.Year);

            foreach (Cat_DayOff item in listDayoff)
            {
                if (item.Type == HolidayType.E_WEEKEND_HLD.ToString()
                    || (item.IsLeaveDay != null && item.IsLeaveDay.Value))
                {
                    countSaturdayDayOffInYear++;
                }
            }

            result = countdayOfYear - countSaturdayDayOffInYear;
            return Math.Round(result / 12, 1);
        }

        #endregion


        public List<Att_OvertimeEntity> GetExceptionOvertimeList(DateTime dateStart, DateTime dateEnd, List<Hre_ProfileEntity> lstProfileIDsAll, string UserLogin)
        {
            List<Guid> lstProfileIDs = lstProfileIDsAll.Select(s => s.ID).ToList();

            var lstAtt_Overtime = new List<Att_OvertimeEntity>();
            using (var context = new VnrHrmDataContext())
            {
                var baseService = new BaseService();
                var _status = string.Empty;
                ListQueryModel lqm = new ListQueryModel();

                #region [tại thời  chưa có giải pháp tốt nên để code củ chuối]
                List<object> lstObj = new List<object>();
                lstObj.Add(1);
                lstObj.Add(1000000);
                #endregion

                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                if (lstProfileIDs != null)
                {
                    string waitStatus = ProfileStatusSyn.E_WAITING.ToString();
                }
                string status = OverTimeStatus.E_REJECTED.ToString();
                var repoAtt_Overtime = new Att_OvertimeRepository(unitOfWork);

                List<Att_OvertimeEntity> lstOvertimeAll = baseService.GetData<Att_OvertimeEntity>(lstObj, ConstantSql.hrm_att_sp_get_OT, UserLogin, ref _status).Where(s => dateStart <= s.WorkDate
                    && s.WorkDate <= dateEnd && s.Status != status).Select(d => new Att_OvertimeEntity
                    {
                        ID = d.ID,
                        WorkDate = d.WorkDate,
                        RegisterHours = d.RegisterHours,
                        ApproveHours = d.ApproveHours,
                        ProfileID = d.ProfileID,
                        Status = d.Status,
                        MethodPayment = d.MethodPayment,
                        ReasonOT = d.ReasonOT,
                        ShiftName = d.ShiftName,
                        ProfileName = d.ProfileName,
                        CodeEmp = d.CodeEmp,
                        OvertimeTypeName = d.OvertimeTypeName,
                        UserApproveName1 = d.UserApproveName1,
                        UserApproveName2 = d.UserApproveName2,
                        OrgStructureName = d.OrgStructureName
                    }).ToList<Att_OvertimeEntity>();
                if (lstProfileIDs.Count > 0)
                    lstOvertimeAll = lstOvertimeAll.Where(s => lstProfileIDs.Contains(s.ProfileID)).ToList();
                List<Guid> listDuple = new List<Guid>();
                IList<IGrouping<Guid, Att_OvertimeEntity>> ilGroupProfileID = lstOvertimeAll.GroupBy(d => d.ProfileID).ToList();
                foreach (Att_OvertimeEntity attOvertime in lstOvertimeAll)
                {
                    Guid profileID = attOvertime.ProfileID;
                    DateTime workDate = attOvertime.WorkDate.AddHours(attOvertime.RegisterHours);

                    if (lstOvertimeAll.Where(s => s.ProfileID == profileID && s.WorkDate < workDate && s.WorkDate.AddHours(s.RegisterHours) > attOvertime.WorkDate).Count() > 1)
                        listDuple.Add(attOvertime.ID);
                }
                lstAtt_Overtime = lstOvertimeAll.Where(s => listDuple.Contains(s.ID)).OrderBy(ls => ls.ProfileID)
                    .ThenBy(ls => ls.WorkDate).ToList();
            }
            return lstAtt_Overtime;
        }


        #region Hỗ trợ tính phép năm phép bệnh

        public void GetConfigANL(string fomularConfig, out ParamGetConfigANL paramConfig)
        {
            #region set default value
            paramConfig = new ParamGetConfigANL();
            paramConfig.monthBeginYear = 1;
            paramConfig.dayBeginFullMonth = 1;
            paramConfig.seniorMonth = 12 * 5;
            paramConfig.dayPerMonth = 30;
            paramConfig.anlRoundUp = 0.5;
            paramConfig.typeProfileBegin = AnlProfileTypeBegin.E_DATE_HIRE.ToString();
            paramConfig.maxInMonthToGetAct = 15; //Trươc ngày 15 thì tính cho tháng đó
            paramConfig.anlFullYear = (double)12 / (double)12;
            paramConfig.anlSeniorMoreThanNormal = (double)1 / (double)12;
            paramConfig.anlHDT4MoreThanNormal = (double)2 / (double)12;
            paramConfig.anlHDT5MoreThanNormal = (double)4 / (double)12;
            paramConfig.lstCodeLeaveNonANL = new List<string>();
            paramConfig.monthInYearSenior = 0;
            paramConfig.monthRoundUp = 0;
            #endregion

            #region set value by Config

            List<string> lstConfig = fomularConfig.Split(';').ToList();
            Dictionary<string, string> dicConfigANL = new Dictionary<string, string>();
            foreach (var item in lstConfig)
            {
                string Val = string.Empty;
                Val = item.Replace("[", "");
                Val = Val.Replace("]", "");

                List<string> config = Val.Split(':').ToList();
                if (config.Count > 1)
                {
                    dicConfigANL.Add(config[0], config[1]);
                }
            }

            if (dicConfigANL.ContainsKey(Formula.FormulaConstant.CONFIG_ANL_MONTHBEGINYEAR.ToString()))
            {
                int value = 0;
                int.TryParse(dicConfigANL[Formula.FormulaConstant.CONFIG_ANL_MONTHBEGINYEAR.ToString()], out value);
                value = value == 0 ? 1 : value;
                paramConfig.monthBeginYear = value;
            }
            if (dicConfigANL.ContainsKey(Formula.FormulaConstant.CONFIG_ANL_DAYBEGIN_FULLMONTH.ToString()))
            {
                int value = 0;
                int.TryParse(dicConfigANL[Formula.FormulaConstant.CONFIG_ANL_DAYBEGIN_FULLMONTH.ToString()], out value);
                value = value == 0 ? 1 : value;
                paramConfig.dayBeginFullMonth = value;
            }
            if (dicConfigANL.ContainsKey(Formula.FormulaConstant.CONFIG_ANL_SENIOR_MONTH.ToString()))
            {
                int value = 0;
                int.TryParse(dicConfigANL[Formula.FormulaConstant.CONFIG_ANL_SENIOR_MONTH.ToString()], out value);
                paramConfig.seniorMonth = value;
            }
            if (dicConfigANL.ContainsKey(Formula.FormulaConstant.CONFIG_ANL_DAY_PER_MONTH.ToString()))
            {
                int value = 0;
                int.TryParse(dicConfigANL[Formula.FormulaConstant.CONFIG_ANL_DAY_PER_MONTH.ToString()], out value);
                paramConfig.dayPerMonth = value;
            }
            if (dicConfigANL.ContainsKey(Formula.FormulaConstant.CONFIG_ANL_ROUND_UP.ToString()))
            {
                double value = 0;
                double.TryParse(dicConfigANL[Formula.FormulaConstant.CONFIG_ANL_ROUND_UP.ToString()], out value);
                paramConfig.anlRoundUp = value;
            }
            if (dicConfigANL.ContainsKey(Formula.FormulaConstant.CONFIG_ANL_TYPE_PROFILE_BEGIN.ToString()))
            {
                paramConfig.typeProfileBegin = dicConfigANL[Formula.FormulaConstant.CONFIG_ANL_TYPE_PROFILE_BEGIN.ToString()];
            }
            if (dicConfigANL.ContainsKey(Formula.FormulaConstant.CONFIG_ANL_DAY_MAX_IN_MONTH_GET_ACTUAL.ToString()))
            {
                int value = 0;
                int.TryParse(dicConfigANL[Formula.FormulaConstant.CONFIG_ANL_DAY_MAX_IN_MONTH_GET_ACTUAL.ToString()], out value);
                paramConfig.maxInMonthToGetAct = value;
            }

            if (dicConfigANL.ContainsKey(Formula.FormulaConstant.CONFIG_ANL_NORMAL_CAN_GET_FULLYEAR.ToString()))
            {
                double value = 0;
                double.TryParse(dicConfigANL[Formula.FormulaConstant.CONFIG_ANL_NORMAL_CAN_GET_FULLYEAR.ToString()], out value);
                paramConfig.anlFullYear = (double)value / (double)12;
            }
            if (dicConfigANL.ContainsKey(Formula.FormulaConstant.CONFIG_ANL_SENIOR_CAN_GET_EACH_LEVEL.ToString()))
            {
                double value = 0;
                double.TryParse(dicConfigANL[Formula.FormulaConstant.CONFIG_ANL_SENIOR_CAN_GET_EACH_LEVEL.ToString()], out value);
                paramConfig.anlSeniorMoreThanNormal = (double)value / (double)12;
            }
            if (dicConfigANL.ContainsKey(Formula.FormulaConstant.CONFIG_ANL_HDT4_CAN_GET_MORE_NORMAL.ToString()))
            {
                double value = 0;
                double.TryParse(dicConfigANL[Formula.FormulaConstant.CONFIG_ANL_HDT4_CAN_GET_MORE_NORMAL.ToString()], out value);
                paramConfig.anlHDT4MoreThanNormal = (double)value / (double)12;
            }
            if (dicConfigANL.ContainsKey(Formula.FormulaConstant.CONFIG_ANL_HDT5_CAN_GET_MORE_NORMAL.ToString()))
            {
                double value = 0;
                double.TryParse(dicConfigANL[Formula.FormulaConstant.CONFIG_ANL_HDT5_CAN_GET_MORE_NORMAL.ToString()], out value);
                paramConfig.anlHDT5MoreThanNormal = (double)value / (double)12;
            }
            if (dicConfigANL.ContainsKey(Formula.FormulaConstant.CONFIG_LEAVE_NON_ALN_CODES.ToString()))
            {
                string CodeLeaveNonANL = dicConfigANL[Formula.FormulaConstant.CONFIG_LEAVE_NON_ALN_CODES.ToString()];
                char[] ext = new char[] { ',' };
                paramConfig.lstCodeLeaveNonANL = CodeLeaveNonANL.Split(ext, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            if (dicConfigANL.ContainsKey(Formula.FormulaConstant.CONFIG_MONTH_INYEAR_TO_COMPUTE_SENIOR.ToString()))
            {
                string CodeLeaveNonANL = dicConfigANL[Formula.FormulaConstant.CONFIG_MONTH_INYEAR_TO_COMPUTE_SENIOR.ToString()];
                int value = 0;
                int.TryParse(dicConfigANL[Formula.FormulaConstant.CONFIG_MONTH_INYEAR_TO_COMPUTE_SENIOR.ToString()], out value);
                paramConfig.monthInYearSenior = value;
            }

            if (dicConfigANL.ContainsKey(Formula.FormulaConstant.CONFIG_MONTH_ROUND_UP.ToString()))
            {
                string roundUp = dicConfigANL[Formula.FormulaConstant.CONFIG_MONTH_ROUND_UP.ToString()];
                int value = 0;
                int.TryParse(roundUp, out value);
                paramConfig.monthRoundUp = value;
            }


            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guidContext"></param>
        /// <param name="Year"></param>
        /// <param name="Month">nếu như null ->> Tính nguyên năm , nếu !Null -> tính cho từng tháng</param>
        /// <param name="dateHire"></param>
        /// <param name="dateEndProbation"></param>
        /// <param name="dateQuit"></param>
        /// <param name="fomularConfig"></param>
        /// <param name="formulaLeave"></param>
        /// <param name="lstLeaveDay"></param>
        /// <param name="lstJobtitle"></param>
        /// <param name="lstDayOff"></param>
        /// <param name="lstHDTJob_ByProfile"></param>
        /// <returns></returns>
        public Double GetAnnualLeaveAvailableAllYear(int Year, int? Month, DateTime? dateHire,
           DateTime? dateEndProbation, DateTime? dateQuit, string fomularConfig,
            string formulaLeave, List<Att_LeaveDay> lstLeaveDay, List<Cat_JobTitle> lstJobtitle,
            List<DateTime> lstDayOff, List<Hre_HDTJob> lstHDTJob_ByProfile, List<Att_LeaveDay> lstLeaveDayAllYear)
        {
            #region Param
            lstHDTJob_ByProfile = lstHDTJob_ByProfile.Where(m => m.DateFrom != null && m.Type != null).OrderBy(m => m.DateFrom).ToList();
            ParamGetConfigANL paramConfig = new ParamGetConfigANL();

            //set du lieu
            GetConfigANL(fomularConfig, out paramConfig);

            int monthBeginYear = paramConfig.monthBeginYear; //Tháng bắt đầu tính phép năm
            int dayBeginFullMonth = paramConfig.dayBeginFullMonth; //Ngày bắt đầu tính tròn ANL cho tháng
            int seniorMonth = paramConfig.seniorMonth; // Số tháng để có 1 level cho thâm niên
            int dayPerMonth = paramConfig.dayPerMonth; // Số ngày cho 1 tháng
            double anlRoundUp = paramConfig.anlRoundUp; //Số làm tròn Lên xuống
            string typeProfileBegin = paramConfig.typeProfileBegin; //Loại lấy theo DateHire hay DateQuit
            int maxInMonthToGetAct = paramConfig.maxInMonthToGetAct; //Ngày chuẩn để xét là DT4 và DT5 đc tính cho tháng àno
            double anlFullYear = paramConfig.anlFullYear; // Số ngày phép bình thường cho 1 năm (tính theo tháng)
            double anlSeniorMoreThanNormal = paramConfig.anlSeniorMoreThanNormal; // Số ngày phép Được cộng thêm do thâm niên so với bình thường (tính theo tháng)
            double anlHDT4MoreThanNormal = paramConfig.anlHDT4MoreThanNormal; // Số ngày phép được cộng thêm do HDT4 so với bình thường (tính theo tháng)
            double anlHDT5MoreThanNormal = paramConfig.anlHDT5MoreThanNormal; // Số ngày phép được cộng thêm do HDT5 so với bình thường (tính theo tháng)
            List<string> lstCodeLeaveNonANL = paramConfig.lstCodeLeaveNonANL;
            int monthInYearSenior = paramConfig.monthInYearSenior;
            int monthRoundUp = paramConfig.monthRoundUp;
            #endregion
            #region Data
            //gan du lieu can thiet

            DateTime BeginYear = new DateTime(Year, monthBeginYear, 1);
            DateTime EndYear = BeginYear.AddYears(1).AddMinutes(-1);

            DateTime? DateCheckByMonth = null;

            if (Month != null)
            {
                DateCheckByMonth = new DateTime(Year, Month.Value, 1);
                if (Month < monthBeginYear)
                {
                    DateCheckByMonth = new DateTime(Year + 1, Month.Value, 1);
                }
            }


            DateTime? DateStartProfile = null;
            DateTime DateEndProfile = EndYear;

            if (typeProfileBegin == AnlProfileTypeBegin.E_DATE_ENDPROBATION.ToString())
            {
                DateStartProfile = dateEndProbation;
            }
            else
            {
                DateStartProfile = dateHire;
            }
            if (DateStartProfile == null)
                return 0;

            if (dateQuit != null && dateQuit < EndYear)
            {
                DateEndProfile = dateQuit.Value.Date.AddDays(1).AddMinutes(-1);
            }

            if (DateStartProfile.Value.Day > dayBeginFullMonth)
            {
                DateStartProfile = new DateTime(DateStartProfile.Value.AddMonths(1).Year, DateStartProfile.Value.AddMonths(1).Month, 1);
            }

            DateTime DateStartInYear = BeginYear > DateStartProfile.Value ? BeginYear : DateStartProfile.Value;
            DateTime DateEndInYear = EndYear < DateEndProfile ? EndYear : DateEndProfile;


            List<HDTJobTypeRange> lstHDTJobContaint = new List<HDTJobTypeRange>();

            foreach (var item in lstHDTJob_ByProfile)
            {
                HDTJobTypeRange hdtJob = new HDTJobTypeRange();
                hdtJob.Type = item.Type;
                hdtJob.DateStart = item.DateFrom.Value;
                hdtJob.DateEnd = item.DateTo;
                lstHDTJobContaint.Add(hdtJob);
            }
            lstHDTJobContaint = lstHDTJobContaint.OrderByDescending(m => m.DateStart).ToList();
            DateTime DateBeFore = DateTime.MaxValue;
            foreach (var item in lstHDTJobContaint)
            {
                if (item.DateEnd == null)
                {
                    item.DateEnd = DateBeFore;
                }
                DateBeFore = item.DateStart;
            }

            #endregion
            //ANL_WORK_HDT4,
            //ANL_WORK_HDT5,
            Formula formula = new Formula(formulaLeave);
            if (DateCheckByMonth == null)
            {
                #region ANL and Leave
                if (formulaLeave.Contains(Formula.FormulaConstant.ANL_NORMAL.ToString()))
                {
                    double value = 0;
                    double monthWorkingNormalInYear = 0;

                    for (int i = 0; i < 12; i++)
                    {
                        if (DateStartInYear.AddMonths(i) < DateEndInYear)
                        {
                            monthWorkingNormalInYear++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    value = anlFullYear * monthWorkingNormalInYear;
                    formula.Parameters.Add(Formula.FormulaConstant.ANL_NORMAL.ToString(), value);
                }
                if (formulaLeave.Contains(Formula.FormulaConstant.ANL_SENIOR.ToString()))
                {
                    double value = 0;
                    int MonBegin = monthInYearSenior == 0 ? 1 : monthInYearSenior;
                    //--DateStartProfile
                    DateTime Monthyear = new DateTime(Year, MonBegin, 1);

                    double dateLeave = lstLeaveDayAllYear.Where(m => m.LeaveDays != null).Sum(m => m.LeaveDays.Value);
                    dateLeave += (lstLeaveDayAllYear.Where(m => m.LeaveHours == null).Sum(m => m.LeaveHours.Value) / 8);

                    DateTime dateRoundProfileStart = DateStartProfile.Value.AddMonths(-monthRoundUp);

                    double Days = (Monthyear - dateRoundProfileStart).TotalDays - dateLeave;
                    int level = (int)(Days / (365 * (seniorMonth / 12)));
                    value = level * anlSeniorMoreThanNormal * 12;
                    formula.Parameters.Add(Formula.FormulaConstant.ANL_SENIOR.ToString(), value);
                }
                if (formulaLeave.Contains(Formula.FormulaConstant.ANL_LEAVE_NON_HAVEANL.ToString()))
                {
                    double value = 0;
                    //Logic: Vướng cai đầu năm cuối năm nên phải cắt cái ngày đó ra cho chính xác
                    double numLeave = 0;
                    foreach (var item in lstLeaveDay)
                    {
                        if (item.DateStart < DateStartInYear && item.DateEnd > DateStartInYear)
                        {
                            DateTime FirstSunday = DateTime.MinValue;
                            for (DateTime DateCheck = DateStartInYear; DateCheck < item.DateEnd; DateCheck = DateCheck.AddDays(1))
                            {
                                if (DateCheck.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    FirstSunday = DateCheck;
                                    break;
                                }
                            }
                            int sundayCount = 0;
                            if (FirstSunday != DateTime.MinValue)
                            {
                                sundayCount = (int)((item.DateEnd - FirstSunday).TotalDays / 7) + 1;
                            }
                            int dayOffCount = lstDayOff.Select(m => m.Date >= DateStartInYear && m.Date < item.DateEnd).Count();
                            numLeave += (item.DateEnd - DateStartInYear).TotalDays - sundayCount - dayOffCount;

                        }
                        else if (item.DateStart < DateEndInYear && item.DateEnd > DateEndInYear)
                        {

                            DateTime FirstSunday = DateTime.MinValue;
                            for (DateTime DateCheck = item.DateStart; DateCheck < DateEndInYear; DateCheck = DateCheck.AddDays(1))
                            {
                                if (DateCheck.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    FirstSunday = DateCheck;
                                    break;
                                }
                            }
                            int sundayCount = 0;
                            if (FirstSunday != DateTime.MinValue)
                            {
                                sundayCount = (int)((DateEndInYear - FirstSunday).TotalDays / 7) + 1;
                            }
                            int dayOffCount = lstDayOff.Select(m => m.Date >= item.DateStart && m.Date < DateEndInYear).Count();
                            numLeave += (item.DateEnd - DateStartInYear).TotalDays - sundayCount - dayOffCount;
                        }
                        else
                        {
                            numLeave += item.TotalDuration ?? 0;
                        }

                    }
                    value = ((int)(numLeave / dayPerMonth)) + ((numLeave % dayPerMonth) >= anlRoundUp ? 1 : 0);
                    formula.Parameters.Add(Formula.FormulaConstant.ANL_LEAVE_NON_HAVEANL.ToString(), value);
                }
                if (formulaLeave.Contains(Formula.FormulaConstant.ANL_WORK_HDT4.ToString()))
                {
                    double value = 0;
                    double monthCount = 0;
                    double dayCount = 0;
                    //thuat: 1. lây cái mới nhất so với ngày bắt đầu năm
                    //Lấy cái moi nhất nhỏ hơn ngày bắt đầu năm
                    string Lv4 = HDTJobType.E_Four.ToString();

                    List<HDTJobTypeRange> lstHDTJobContaintLV4 = lstHDTJobContaint.Where(m => m.Type == Lv4).ToList();
                    //Lấy ra 3 khoảng
                    //Đầu Năm
                    HDTJobTypeRange lstHDTJobContaintLV4_BeginYear = lstHDTJobContaintLV4.Where(m => m.DateStart < DateStartInYear && m.DateEnd != null && m.DateEnd.Value > DateStartInYear).FirstOrDefault();
                    //Trong Năm
                    List<HDTJobTypeRange> lstHDTJobContaintLV4_InYear = lstHDTJobContaintLV4.Where(m => m.DateStart >= DateStartInYear && m.DateEnd != null && m.DateEnd.Value <= DateEndInYear).ToList();
                    //Cuối Năm
                    HDTJobTypeRange lstHDTJobContaintLV4_EndYear = lstHDTJobContaintLV4.Where(m => m.DateStart < DateEndInYear && m.DateEnd != null && m.DateEnd.Value > DateEndInYear).FirstOrDefault();
                    bool isFullYear = false;
                    if (lstHDTJobContaintLV4_BeginYear != null && !isFullYear)
                    {
                        if (lstHDTJobContaintLV4_BeginYear.DateEnd.Value > DateEndInYear)
                        {
                            dayCount = (DateEndInYear - DateStartInYear).TotalDays;
                            isFullYear = true;
                        }
                        else
                        {
                            dayCount += (lstHDTJobContaintLV4_BeginYear.DateEnd.Value - DateStartInYear).TotalDays;
                        }
                    }
                    if (lstHDTJobContaintLV4_EndYear != null && !isFullYear)
                    {
                        if (lstHDTJobContaintLV4_EndYear.DateStart < DateStartInYear)
                        {
                            dayCount = (DateEndInYear - DateStartInYear).TotalDays;
                            isFullYear = true;
                        }
                        else
                        {
                            dayCount += (DateEndInYear - lstHDTJobContaintLV4_EndYear.DateStart).TotalDays;
                        }
                    }
                    if (!isFullYear)
                    {
                        foreach (var item in lstHDTJobContaintLV4_InYear)
                        {
                            dayCount += (item.DateEnd.Value - item.DateStart).TotalDays;
                        }
                    }

                    monthCount = ((int)(dayCount / dayPerMonth)) + ((dayCount % dayPerMonth) >= anlRoundUp ? 1 : 0);
                    value = monthCount * anlHDT4MoreThanNormal;
                    formula.Parameters.Add(Formula.FormulaConstant.ANL_WORK_HDT4.ToString(), value);
                }
                if (formulaLeave.Contains(Formula.FormulaConstant.ANL_WORK_HDT5.ToString()))
                {
                    double value = 0;
                    double monthCount = 0;
                    double dayCount = 0;
                    //thuat: 1. lây cái mới nhất so với ngày bắt đầu năm
                    //Lấy cái moi nhất nhỏ hơn ngày bắt đầu năm
                    string Lv5 = HDTJobType.E_Five.ToString();

                    List<HDTJobTypeRange> lstHDTJobContaintLV5 = lstHDTJobContaint.Where(m => m.Type == Lv5).ToList();
                    //Lấy ra 3 khoảng
                    //Đầu Năm
                    HDTJobTypeRange lstHDTJobContaintLV5_BeginYear = lstHDTJobContaintLV5.Where(m => m.DateStart < DateStartInYear && m.DateEnd != null && m.DateEnd.Value > DateStartInYear).FirstOrDefault();
                    //Trong Năm
                    List<HDTJobTypeRange> lstHDTJobContaintLV5_InYear = lstHDTJobContaintLV5.Where(m => m.DateStart >= DateStartInYear && m.DateEnd != null && m.DateEnd.Value <= DateEndInYear).ToList();
                    //Cuối Năm
                    HDTJobTypeRange lstHDTJobContaintLV5_EndYear = lstHDTJobContaintLV5.Where(m => m.DateStart < DateEndInYear && m.DateEnd != null && m.DateEnd.Value > DateEndInYear).FirstOrDefault();
                    bool isFullYear = false;
                    if (lstHDTJobContaintLV5_BeginYear != null && !isFullYear)
                    {
                        if (lstHDTJobContaintLV5_BeginYear.DateEnd.Value > DateEndInYear)
                        {
                            dayCount = (DateEndInYear - DateStartInYear).TotalDays;
                            isFullYear = true;
                        }
                        else
                        {
                            dayCount += (lstHDTJobContaintLV5_BeginYear.DateEnd.Value - DateStartInYear).TotalDays;
                        }
                    }
                    if (lstHDTJobContaintLV5_EndYear != null && !isFullYear)
                    {
                        if (lstHDTJobContaintLV5_EndYear.DateStart < DateStartInYear)
                        {
                            dayCount = (DateEndInYear - DateStartInYear).TotalDays;
                            isFullYear = true;
                        }
                        else
                        {
                            dayCount += (DateEndInYear - lstHDTJobContaintLV5_EndYear.DateStart).TotalDays;
                        }
                    }
                    if (!isFullYear)
                    {
                        foreach (var item in lstHDTJobContaintLV5_InYear)
                        {
                            dayCount += (item.DateEnd.Value - item.DateStart).TotalDays;
                        }
                    }

                    monthCount = ((int)(dayCount / dayPerMonth)) + ((dayCount % dayPerMonth) >= anlRoundUp ? 1 : 0);
                    value = monthCount * anlHDT5MoreThanNormal;
                    formula.Parameters.Add(Formula.FormulaConstant.ANL_WORK_HDT5.ToString(), value);
                }
                #endregion
            }
            else
            {
                #region BHXH
                if (formulaLeave.Contains(Formula.FormulaConstant.ANL_NORMAL.ToString()))
                {
                    double value = 0;
                    if (DateStartInYear <= DateCheckByMonth)
                    {
                        value = anlFullYear;
                    }
                    formula.Parameters.Add(Formula.FormulaConstant.ANL_NORMAL.ToString(), value);
                }
                if (formulaLeave.Contains(Formula.FormulaConstant.ANL_SENIOR.ToString()))
                {
                    double value = 0;
                    int level = 0;

                    DateTime DateSenior = DateTime.MinValue;
                    for (int i = 0; i < 20; i++)
                    {
                        DateSenior = DateStartProfile.Value.AddMonths(seniorMonth);
                        if (DateSenior <= DateCheckByMonth)
                        {
                            level++;
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    value = (level * anlSeniorMoreThanNormal);
                    formula.Parameters.Add(Formula.FormulaConstant.ANL_SENIOR.ToString(), value);
                }
                if (formulaLeave.Contains(Formula.FormulaConstant.ANL_WORK_HDT4.ToString()))
                {
                    double value = 0;
                    double monthCount = 0;
                    double dayCount = 0;
                    //thuat: 1. lây cái mới nhất so với ngày bắt đầu năm
                    //Lấy cái moi nhất nhỏ hơn ngày bắt đầu năm
                    string Lv4 = HDTJobType.E_Four.ToString();

                    DateTime DateStartMonth = DateCheckByMonth.Value;
                    DateTime DateEndMonth = DateStartMonth.AddMonths(1).AddMinutes(-1);

                    List<HDTJobTypeRange> lstHDTJobContaintLV4 = lstHDTJobContaint.Where(m => m.Type == Lv4).ToList();
                    //Lấy ra 3 khoảng
                    //Đầu Năm
                    HDTJobTypeRange lstHDTJobContaintLV4_BeginMonth = lstHDTJobContaintLV4.Where(m => m.DateStart < DateStartMonth && m.DateEnd != null && m.DateEnd.Value > DateStartMonth).FirstOrDefault();
                    //Trong Năm
                    List<HDTJobTypeRange> lstHDTJobContaintLV4_InMonth = lstHDTJobContaintLV4.Where(m => m.DateStart >= DateStartMonth && m.DateEnd != null && m.DateEnd.Value <= DateEndMonth).ToList();
                    //Cuối Năm
                    HDTJobTypeRange lstHDTJobContaintLV4_EndMonth = lstHDTJobContaintLV4.Where(m => m.DateStart < DateEndMonth && m.DateEnd != null && m.DateEnd.Value > DateEndMonth).FirstOrDefault();
                    bool isFullMonth = false;
                    if (lstHDTJobContaintLV4_BeginMonth != null && !isFullMonth)
                    {
                        if (lstHDTJobContaintLV4_BeginMonth.DateEnd.Value > DateEndMonth)
                        {
                            dayCount = (DateEndMonth - DateStartMonth).TotalDays;
                            isFullMonth = true;
                        }
                        else
                        {
                            dayCount += (lstHDTJobContaintLV4_BeginMonth.DateEnd.Value - DateStartMonth).TotalDays;
                        }
                    }
                    if (lstHDTJobContaintLV4_EndMonth != null && !isFullMonth)
                    {
                        if (lstHDTJobContaintLV4_EndMonth.DateStart < DateStartMonth)
                        {
                            dayCount = (DateEndMonth - DateStartMonth).TotalDays;
                            isFullMonth = true;
                        }
                        else
                        {
                            dayCount += (DateEndMonth - lstHDTJobContaintLV4_EndMonth.DateStart).TotalDays;
                        }
                    }
                    if (!isFullMonth)
                    {
                        foreach (var item in lstHDTJobContaintLV4_InMonth)
                        {
                            dayCount += (item.DateEnd.Value - item.DateStart).TotalDays;
                        }
                    }

                    monthCount = ((dayCount % dayPerMonth) >= anlRoundUp ? 1 : 0);
                    value = monthCount * anlHDT4MoreThanNormal;
                    formula.Parameters.Add(Formula.FormulaConstant.ANL_WORK_HDT4.ToString(), value);
                }
                if (formulaLeave.Contains(Formula.FormulaConstant.ANL_WORK_HDT5.ToString()))
                {
                    double value = 0;
                    double monthCount = 0;
                    double dayCount = 0;
                    //thuat: 1. lây cái mới nhất so với ngày bắt đầu năm
                    //Lấy cái moi nhất nhỏ hơn ngày bắt đầu năm
                    string Lv5 = HDTJobType.E_Five.ToString();

                    DateTime DateStartMonth = DateCheckByMonth.Value;
                    DateTime DateEndMonth = DateStartMonth.AddMonths(1).AddMinutes(-1);

                    List<HDTJobTypeRange> lstHDTJobContaintLV5 = lstHDTJobContaint.Where(m => m.Type == Lv5).ToList();
                    //Lấy ra 3 khoảng
                    //Đầu Năm
                    HDTJobTypeRange lstHDTJobContaintLV5_BeginMonth = lstHDTJobContaintLV5.Where(m => m.DateStart < DateStartMonth && m.DateEnd != null && m.DateEnd.Value > DateStartMonth).FirstOrDefault();
                    //Trong Năm
                    List<HDTJobTypeRange> lstHDTJobContaintLV5_InMonth = lstHDTJobContaintLV5.Where(m => m.DateStart >= DateStartMonth && m.DateEnd != null && m.DateEnd.Value <= DateEndMonth).ToList();
                    //Cuối Năm
                    HDTJobTypeRange lstHDTJobContaintLV5_EndMonth = lstHDTJobContaintLV5.Where(m => m.DateStart < DateEndMonth && m.DateEnd != null && m.DateEnd.Value > DateEndMonth).FirstOrDefault();
                    bool isFullMonth = false;
                    if (lstHDTJobContaintLV5_BeginMonth != null && !isFullMonth)
                    {
                        if (lstHDTJobContaintLV5_BeginMonth.DateEnd.Value > DateEndMonth)
                        {
                            dayCount = (DateEndMonth - DateStartMonth).TotalDays;
                            isFullMonth = true;
                        }
                        else
                        {
                            dayCount += (lstHDTJobContaintLV5_BeginMonth.DateEnd.Value - DateStartMonth).TotalDays;
                        }
                    }
                    if (lstHDTJobContaintLV5_EndMonth != null && !isFullMonth)
                    {
                        if (lstHDTJobContaintLV5_EndMonth.DateStart < DateStartMonth)
                        {
                            dayCount = (DateEndMonth - DateStartMonth).TotalDays;
                            isFullMonth = true;
                        }
                        else
                        {
                            dayCount += (DateEndMonth - lstHDTJobContaintLV5_EndMonth.DateStart).TotalDays;
                        }
                    }
                    if (!isFullMonth)
                    {
                        foreach (var item in lstHDTJobContaintLV5_InMonth)
                        {
                            dayCount += (item.DateEnd.Value - item.DateStart).TotalDays;
                        }
                    }

                    monthCount = ((dayCount % dayPerMonth) >= anlRoundUp ? 1 : 0);
                    value = monthCount * anlHDT5MoreThanNormal;
                    formula.Parameters.Add(Formula.FormulaConstant.ANL_WORK_HDT5.ToString(), value);
                }
                #endregion
            }

            double result = Convert.ToDouble(formula.Evaluate());
            if ((result - (int)result) > 0)
            {
                double dental = result - (int)result;
                result = dental >= 0.5 ? (int)result + 1 : (int)result;
            }
            return result;
        }

        #endregion

        #region Hỗ trợ phân tích tạo mới tăng ca
        /// <summary>
        /// Kiem tra ngay lam viec, dua tren cau hinh Grade
        /// Tru cac ngay nghi le
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="date"></param>
        /// <returns></returns>

        public static bool IsWorkDay(Cat_GradeAttendance grade, Hashtable rosterTable
                                    , List<Cat_DayOff> lstDayOff, DateTime date)
        {
            if (grade == null)
                return false;
            //throw new VNRException("Grade is not configured");

            List<Cat_DayOff> lstDayWeekend = new List<Cat_DayOff>();

            //if (lstDayOff != null)
            //    lstDayWeekend = lstDayOff.Where(off => off.Type == HolidayType.E_WEEKEND_HLD.ToString()
            //        || off.Type == HolidayType.E_HOLIDAY_HLD.ToString()).ToList();

            if (lstDayOff != null)
                lstDayWeekend = lstDayOff.Where(off => off.Type == HolidayType.E_WEEKEND_HLD.ToString()).ToList();
            if (grade.RosterType == GradeRosterType.E_ISROSTER.ToString() || grade.RosterType == GradeRosterType.E_ISROSTER_ORG.ToString())
            {
                return Att_RosterServices.IsWorkDay(date, rosterTable);
            }
            else if (grade.RosterType == GradeRosterType.E_ISHOLIDAY.ToString())
            {
                if (lstDayWeekend == null || lstDayWeekend.Count <= 0)
                    return true;
                if (!lstDayWeekend.Exists(dof => dof.DateOff.Date == date.Date))
                    return true;
            }
            else if (grade.RosterType == GradeRosterType.E_ISDEFAULT.ToString())// dua tren roster Default
            {
                //Kiem tra xem co cau hinh nghi cuoi tuan trong ngay nghi 

                if (lstDayWeekend != null && lstDayWeekend.Exists(dof => dof.DateOff.Date == date.Date))
                    return false;

                if ((date.DayOfWeek == DayOfWeek.Monday) && grade.WorkOnMondayID != null)
                {
                    return true;
                }
                else if ((date.DayOfWeek == DayOfWeek.Tuesday) && grade.WorkOnTuesdayID != null)
                {
                    return true;
                }
                else if ((date.DayOfWeek == DayOfWeek.Wednesday) && grade.WorkOnWednesdayID != null)
                {
                    return true;
                }
                else if ((date.DayOfWeek == DayOfWeek.Thursday) && grade.WorkOnThursdayID != null)
                {
                    return true;
                }
                else if ((date.DayOfWeek == DayOfWeek.Friday) && grade.WorkOnFridayID != null)
                {
                    return true;
                }
                else if ((date.DayOfWeek == DayOfWeek.Saturday) && grade.WorkOnSaturdayID != null)
                {
                    return true;
                }
                else if (date.DayOfWeek == DayOfWeek.Sunday && grade.WorkOnSundayID != null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="rosterTable"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static Cat_Shift GetShift(Cat_GradeAttendance grade, Hashtable rosterTable, DateTime date)
        {

            //Doc Shift tu Roster 
            Cat_Shift res = Att_RosterServices.GetShift(date, rosterTable);
            //Kiem tra co cau hinh Roster hay ko. neu co thi lay Shift theo Roster, ko thi lay theo Default
            //Neu ko co roster. 
            if (res == null)
            {
                //Kiem tra cau hinh grade
                if (grade != null)
                {
                    //Kiem tra cau hinh Shift trong grade neu thiet lap lich lam viec la 'Mac dinh-Default'
                    if (grade.RosterType == GradeRosterType.E_ISDEFAULT.ToString())// dua tren roster Default
                    {
                        //Thu tu shift ~ ngay trong tuan tham khao file 'UserDataObject.cs
                        if ((date.DayOfWeek == DayOfWeek.Monday) && grade.WorkOnMondayID != null)
                        {
                            res.ID = grade.WorkOnMondayID.Value;
                        }
                        else if ((date.DayOfWeek == DayOfWeek.Tuesday) && grade.WorkOnTuesdayID != null)
                        {
                            res.ID = grade.WorkOnTuesdayID.Value;
                        }
                        else if ((date.DayOfWeek == DayOfWeek.Wednesday) && grade.WorkOnWednesdayID != null)
                        {
                            res.ID = grade.WorkOnWednesdayID.Value;
                        }
                        else if ((date.DayOfWeek == DayOfWeek.Thursday) && grade.WorkOnThursdayID != null)
                        {
                            res.ID = grade.WorkOnThursdayID.Value;
                        }
                        else if ((date.DayOfWeek == DayOfWeek.Friday) && grade.WorkOnFridayID != null)
                        {
                            res.ID = grade.WorkOnFridayID.Value;
                        }
                        else if ((date.DayOfWeek == DayOfWeek.Saturday) && grade.WorkOnSaturdayID != null)
                        {
                            res.ID = grade.WorkOnSaturdayID.Value;
                        }
                        else if (date.DayOfWeek == DayOfWeek.Sunday && grade.WorkOnSundayID != null)
                        {
                            res.ID = grade.WorkOnSundayID.Value;
                        }
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inout"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        public static Double GetShiftRosterShiftInOutHour(Att_InOut inout, Cat_Shift shift)
        {
            DateTime workdate = new DateTime(inout.WorkDate.Year, inout.WorkDate.Month, inout.WorkDate.Day
                                            , shift.InTime.Hour, shift.InTime.Minute, shift.InTime.Second);
            DateTime shiftStart = workdate;
            DateTime shiftEnd = workdate.AddHours(shift.CoOut);
            DateTime dateIn = inout.InTime.Value;
            DateTime dateOut = inout.OutTime.Value;

            Double _hrs = 0;
            if (dateIn.CompareTo(shiftStart) > 0)
                _hrs += dateIn.Subtract(shiftStart).TotalHours;
            else
                _hrs += shiftStart.Subtract(dateIn).TotalHours;

            if (dateOut.CompareTo(shiftEnd) > 0)
                _hrs += dateOut.Subtract(shiftEnd).TotalHours;
            else
                _hrs += shiftEnd.Subtract(dateOut).TotalHours;

            return _hrs;
        }
        public static Double GetInOutWorkingHour(Att_InOut inout, Cat_Shift shift)
        {
            DateTime workdate = new DateTime(inout.WorkDate.Year, inout.WorkDate.Month, inout.WorkDate.Day
                                            , shift.InTime.Hour, shift.InTime.Minute, shift.InTime.Second);
            DateTime shiftStart = workdate;
            DateTime shiftEnd = workdate.AddHours(shift.CoOut);

            return GetIntersectAmount(inout.InTime.Value, inout.OutTime.Value, shiftStart, shiftEnd).TotalHours;

        }
        public static bool IsNightShiftByConfig(Sys_AllSettingEntity objAppConfig)
        {
            bool isAllow = false;
            string type = AppConfig.E_STANDARD_WORKDAY.ToString();
            //Sys_AppConfig objAppConfig = EntityService.Instance.GetEntity<Sys_AppConfig>(guidContext, userid, ui => ui.Info == type);

            if (objAppConfig != null && objAppConfig.Value1 == ConfigNightShift.E_CONFIG.ToString())
            {
                isAllow = true;
            }
            return isAllow;
        }

        #endregion

        #region Lay Line trong DS roster
        /// <summary>
        /// Lay Line trong DS roster trong khoang thoi gian. so sanh theo ngay End. Neu End ko co thi lay tru di tung ngay
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="lstRoster"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public static string GetLineInRoster(Hre_Profile profile, List<Att_Roster> lstRoster, DateTime dateStart, DateTime dateEnd)
        {
            String strLine = String.Empty;
            List<Att_Roster> lstRosterRange = lstRoster.Where(roster => roster.DateStart <= dateEnd && roster.DateEnd >= dateStart).ToList();
            List<Att_Roster> lstProfileRoster = lstRosterRange.Where(rt => rt.ProfileID == profile.ID).ToList();
            DateTime timeLineNow = dateEnd.Date;
            List<Att_Roster> lstRosterLine = lstProfileRoster.Where(p => p.DateStart <= timeLineNow && p.DateEnd >= timeLineNow).OrderByDescending(p => p.DateUpdate).ToList();

            //Neu ngay timeLineNow khong co roster thi tru di 1 ngay
            while (lstRosterLine.Count <= 0 && timeLineNow >= dateStart)
            {
                timeLineNow = timeLineNow.AddDays(-1);
                lstRosterLine = lstProfileRoster.Where(p => p.DateStart <= timeLineNow && p.DateEnd >= timeLineNow).OrderByDescending(p => p.DateUpdate).ToList();
            }

            if (lstRosterLine.Count > 0)
                strLine = lstRosterLine[0].Cat_OrgStructure == null ? string.Empty : lstRosterLine[0].Cat_OrgStructure.OrgStructureName;
            return strLine;
        }
        #endregion

        #region Lay ngay bat dau tinh luong nho nhat , ngay ket thuc tinh luong lon nhat trong tat ca cac Grade
        /// <summary>
        /// Lay ngay bat dau tinh luong nho nhat , ngay ket thuc tinh luong lon nhat trong tat ca cac Grade
        /// </summary>
        /// <returns></returns>
        public static void GetRangeMaxMinGrade(List<Cat_GradeAttendance> lstGra, DateTime monthYear, out DateTime dateStart, out DateTime dateEnd)
        {
            dateStart = monthYear;
            dateEnd = monthYear;
            foreach (Cat_GradeAttendance grade in lstGra)
            {
                DateTime start = new DateTime();
                DateTime end = new DateTime();
                int day = 1;
                if (grade.SalaryTimeDay != null && grade.SalaryTimeDay > 0 && grade.SalaryTimeDay <= 31)
                    day = Convert.ToInt32(grade.SalaryTimeDay);
                if (EnumDropDown.SalaryTimeType.E_LASTMONTH.ToString() == grade.SalaryTimeType)
                {
                    start = new DateTime(monthYear.AddMonths(-1).Year, monthYear.AddMonths(-1).Month, day);
                }
                else //same month
                {
                    start = new DateTime(monthYear.Year, monthYear.Month, day);
                }
                end = start.AddMonths(1).AddMinutes(-1);
                if (start < dateStart)
                {
                    dateStart = start;
                }
                if (end > dateEnd)
                {
                    dateEnd = end;
                }
            }

        }

        #endregion

        /// <summary>
        /// Xac dinh ngay bat dau, ket thuc thang luong dua vao  Cau hinh chung
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="monthYear"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private static void GetSalaryDateRange(Sys_AllSetting appConfig, DateTime monthYear
                                            , out DateTime from, out DateTime to)
        {
            if (appConfig != null)
            {
                int day = Convert.ToInt32(appConfig.Value1);
                if (appConfig.Value3 == EnumDropDown.SalaryTimeType.E_LASTMONTH.ToString())
                {
                    from = new DateTime(monthYear.AddMonths(-1).Year, monthYear.AddMonths(-1).Month, day);
                }
                else //same month
                {
                    from = new DateTime(monthYear.Year, monthYear.Month, day);
                }
                to = from.AddMonths(1).AddMinutes(-1);
            }
            else
            {
                GetSalaryMonthRange(monthYear, out from, out to);
            }
        }

        /// <summary>
        /// Lay gia tri dau thang den cuoi thang
        /// </summary>
        /// <param name="monthYear"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void GetSalaryMonthRange(DateTime monthYear, out DateTime from, out DateTime to)
        {
            from = new DateTime(monthYear.Year, monthYear.Month, 1);
            to = from.AddMonths(1).AddMinutes(-1);
        }

        /// <summary>
        /// Xac dinh ngay bat dau, ket thuc thang luong dua vao grade
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="monthYear"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private static void GetSalaryDateRange(Cat_GradeAttendance gradeCfg, List<Att_CutOffDuration> lstCutOff, DateTime monthYear
                                            , out DateTime from, out DateTime to)
        {
            int day = Convert.ToInt32(gradeCfg.SalaryTimeDay);
            //tan.do: check when day is 0 
            day = day == 0 ? 1 : day;
            DateTime _monthYear = new DateTime(monthYear.Year, monthYear.Month, 1);
            DateTime _from = _monthYear;
            DateTime _to = _monthYear;
            if (gradeCfg.IsDurationCutOff != null && gradeCfg.IsDurationCutOff.Value == true && lstCutOff != null && lstCutOff.Count > 0 && !String.IsNullOrEmpty(gradeCfg.DurationType))
            {
                string strDurationType = gradeCfg.DurationType;
                if (gradeCfg.DurationType == EnumDropDown.DurationType.E_MOTHLY.ToString())
                {
                    Att_CutOffDuration cutOffMonth = lstCutOff.Where(cut => cut.DurationType == strDurationType && cut.MonthYear == _monthYear).FirstOrDefault();
                    if (cutOffMonth!=null)
                    {
                        _from = cutOffMonth.DateStart;
                        _to = cutOffMonth.DateEnd;
                    }
                  
                }
                else if (gradeCfg.DurationType == EnumDropDown.DurationType.E_MOTHLY.ToString() || gradeCfg.DurationType == EnumDropDown.DurationType.E_WEEKLY.ToString())
                {
                    List<Att_CutOffDuration> lstCutOffMonth = lstCutOff.Where(cut => cut.DurationType == strDurationType && cut.MonthYear == _monthYear).ToList();
                    _from = DateTime.MaxValue;
                    _to = DateTime.MinValue;
                    foreach (Att_CutOffDuration cutOff in lstCutOffMonth)
                    {
                        if (_from > cutOff.DateStart)
                        {
                            _from = cutOff.DateStart;
                        }
                        if (_to < cutOff.DateEnd)
                        {
                            _to = cutOff.DateEnd;
                        }
                    }
                    if (_from == DateTime.MaxValue || _to == DateTime.MinValue)
                    {
                        _from = _monthYear;
                        _to = _monthYear;
                    }
                }
            }
            else
            {
                if (EnumDropDown.SalaryTimeType.E_LASTMONTH.ToString() == gradeCfg.SalaryTimeType)
                {
                    _from = new DateTime(monthYear.AddMonths(-1).Year, monthYear.AddMonths(-1).Month, day);
                }
                else //same month
                {
                    _from = new DateTime(monthYear.Year, monthYear.Month, day);
                }
                _to = _from.AddMonths(1).AddMinutes(-1);
            }
            from = _from;
            to = _to;
        }
        public static void GetSalaryDateRange(Cat_GradeAttendance gradeCfg, Sys_AllSetting objAppConfig, List<Att_CutOffDuration> lstCutOff, DateTime monthYear
                                            , out DateTime from, out DateTime to)
        {
            DateTime fromDate = new DateTime(monthYear.Year, monthYear.Month, 1);
            DateTime toDate = fromDate.AddMonths(1).AddMinutes(-1);
            if (gradeCfg != null)
                Att_AttendanceServices.GetSalaryDateRange(gradeCfg, lstCutOff, monthYear, out fromDate, out toDate);
            else
            {
                if (objAppConfig != null)
                    Att_AttendanceServices.GetSalaryDateRange(objAppConfig, monthYear, out fromDate, out toDate);
            }
            from = fromDate;
            to = toDate;
        }

        /// <summary>
        /// Xac dinh thang tinh luong cua thoi gian
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="monthYear"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void GetMonthSalary(Cat_GradeAttendance gradeCfg, DateTime dateTime
                                            , out DateTime monthYear)
        {
            int day = Convert.ToInt32(gradeCfg.SalaryTimeDay);
            //tan.do: check when day is 0 
            day = day == 0 ? 1 : day;
            DateTime from = dateTime;
            if (EnumDropDown.SalaryTimeType.E_LASTMONTH.ToString() == gradeCfg.SalaryTimeType)
            {
                from = new DateTime(dateTime.AddMonths(-1).Year, dateTime.AddMonths(-1).Month, day);
            }
            else //same month
            {
                from = new DateTime(dateTime.Year, dateTime.Month, day);
            }
            DateTime to = from.AddMonths(1).AddMinutes(-1);

            if (from <= dateTime && to >= dateTime)
            {
                monthYear = new DateTime(dateTime.Year, dateTime.Month, 1);
            }
            else
            {
                monthYear = new DateTime(dateTime.AddMonths(1).Year, dateTime.AddMonths(1).Month, 1);
            }
        }

        /// <summary>
        /// Hàm tính ra số phép Còn lại của tháng hiện tại và cuối năm  ps: chưa trừ ra cái leaveday đang check
        /// </summary>
        /// <param name="leaveDay"></param>
        /// <param name="lstLeaveDayType"></param>
        /// <param name="RemainInEndMonth"></param>
        /// <param name="RemainInEndYear"></param>
        public void GetRemainLeaveDay(Att_LeaveDay leaveDay, List<Cat_LeaveDayType> lstLeaveDayType, out double RemainInEndMonth, out double RemainInEndYear)
        {
            RemainInEndMonth = 0;
            RemainInEndYear = 0;
            var LeaveDayType = lstLeaveDayType.Where(m => m.ID == leaveDay.LeaveDayTypeID).FirstOrDefault();
            if (LeaveDayType == null)
                return;

            if (LeaveDayType.IsAnnualLeave)
            {
                List<Att_RemainLeaveEntity> lstRemainLeaveEntity = GetRemainLeave_Annual(leaveDay.DateEnd, new List<Guid>() { leaveDay.ProfileID });
                if (lstRemainLeaveEntity != null && lstRemainLeaveEntity.Count > 0)
                {
                    Att_RemainLeaveEntity RemainLeave = lstRemainLeaveEntity.FirstOrDefault();
                    if (RemainLeave != null)
                    {
                        RemainInEndMonth = RemainLeave.RemainEndMonth ?? 0;
                        RemainInEndYear = RemainLeave.RemainEndYear ?? 0;
                    }
                }
            }
            else if (LeaveDayType.IsTimeOffInLieu != null && LeaveDayType.IsTimeOffInLieu == true)
            {
                List<Att_RemainLeaveEntity> lstRemainLeaveEntity = GetRemainLeave_TimeOffInLieu(leaveDay.DateEnd, new List<Guid>() { leaveDay.ProfileID });
                if (lstRemainLeaveEntity != null && lstRemainLeaveEntity.Count > 0)
                {
                    Att_RemainLeaveEntity RemainLeave = lstRemainLeaveEntity.FirstOrDefault();
                    if (RemainLeave != null)
                    {
                        RemainInEndMonth = RemainLeave.RemainEndMonth ?? 0;
                        RemainInEndYear = RemainLeave.RemainEndYear ?? 0;
                    }
                }
            }
        }
        /// <summary>
        /// hàm tính ra số phép năm còn lại của tháng và cuối năm
        /// </summary>
        /// <param name="DateCheck"></param>
        /// <param name="ProfileIDs"></param>
        /// <returns></returns>
        public List<Att_RemainLeaveEntity> GetRemainLeave_Annual(DateTime DateCheck, List<Guid> ProfileIDs)
        {
            int Year = DateCheck.Year;
            List<Att_AnnualDetail> lstAnnualDetail = new List<Att_AnnualDetail>();
            if (ProfileIDs.Count > 1000)
            {
                lstAnnualDetail = new List<Att_AnnualDetail>().Where(m => m.Year == Year).ToList();
                lstAnnualDetail = lstAnnualDetail.Where(m => m.ProfileID != null && ProfileIDs.Contains(m.ProfileID.Value)).ToList();
            }
            else
            {
                lstAnnualDetail = new List<Att_AnnualDetail>().Where(m => m.Year == Year && m.ProfileID != null && ProfileIDs.Contains(m.ProfileID.Value)).ToList();
            }
            return GetRemainLeave_Annual(DateCheck, ProfileIDs, lstAnnualDetail);
        }
        /// <summary>
        /// hàm tính ra số phép năm còn lại của tháng và cuối năm
        /// </summary>
        /// <param name="DateCheck"></param>
        /// <param name="ProfileIDs"></param>
        /// <param name="lstAnnualDetailInput"></param>
        /// <returns></returns>
        public List<Att_RemainLeaveEntity> GetRemainLeave_Annual(DateTime DateCheck, List<Guid> ProfileIDs, List<Att_AnnualDetail> lstAnnualDetailInput)
        {
            DateCheck = new DateTime(DateCheck.Year, DateCheck.Month, 1);
            int year = DateCheck.Year;
            List<Att_RemainLeaveEntity> RemainLeaveResult = new List<Att_RemainLeaveEntity>();
            List<Att_AnnualDetail> lstAnnualDetail = lstAnnualDetailInput.Where(m => m.Year == year).ToList();

            foreach (var item in ProfileIDs)
            {
                Att_RemainLeaveEntity RemainLeave = new Att_RemainLeaveEntity();
                RemainLeave.ProfileID = item;
                var LstAnnualDetailByprofile = lstAnnualDetail.Where(m => m.ProfileID == item).ToList();
                if (LstAnnualDetailByprofile.Count > 0)
                {
                    var AnnualDetailByprofile_CurrentMonth = LstAnnualDetailByprofile.Where(m => m.MonthYear == DateCheck).FirstOrDefault();
                    if (AnnualDetailByprofile_CurrentMonth != null)
                    {
                        RemainLeave.RemainEndMonth = AnnualDetailByprofile_CurrentMonth.Remain;
                    }
                    int MonthBeginYearANL = LstAnnualDetailByprofile.FirstOrDefault().MonthBeginInYear ?? 1;
                    int EndYearANL = (MonthBeginYearANL - 1) == 0 ? 12 : (MonthBeginYearANL - 1);
                    DateTime MonthEndYear = new DateTime(DateCheck.Year, EndYearANL, 1);
                    var AnnualDetailByprofile_EndYear = LstAnnualDetailByprofile.Where(m => m.MonthYear == MonthEndYear).FirstOrDefault();
                    if (AnnualDetailByprofile_EndYear != null)
                    {
                        RemainLeave.RemainEndYear = AnnualDetailByprofile_EndYear.Remain;
                    }
                }
                RemainLeaveResult.Add(RemainLeave);
            }
            return RemainLeaveResult;
        }
        /// <summary>
        /// hàm tính ra số phép bù còn lại của tháng và cuối năm
        /// </summary>
        /// <param name="DateCheck"></param>
        /// <param name="ProfileIDs"></param>
        /// <returns></returns>
        public List<Att_RemainLeaveEntity> GetRemainLeave_TimeOffInLieu(DateTime DateCheck, List<Guid> ProfileIDs)
        {

            DateTime EndYear = (new DateTime(DateCheck.Year, 1, 1)).AddYears(1).AddMinutes(-1);
            List<Att_TimeOffInLieu> lstTimeOffInLieulInput = new List<Att_TimeOffInLieu>();
            if (ProfileIDs.Count > 1000)
            {
                lstTimeOffInLieulInput = new List<Att_TimeOffInLieu>().Where(m => m.Date <= EndYear).ToList();
                lstTimeOffInLieulInput = lstTimeOffInLieulInput.Where(m => ProfileIDs.Contains(m.ProfileID)).ToList();
            }
            else
            {
                lstTimeOffInLieulInput = new List<Att_TimeOffInLieu>().Where(m => m.Date <= EndYear && ProfileIDs.Contains(m.ProfileID)).ToList();
            }

            return GetRemainLeave_TimeOffInLieu(DateCheck, ProfileIDs, lstTimeOffInLieulInput);
        }
        /// <summary>
        /// hàm tính ra số phép bù còn lại của tháng và cuối năm
        /// </summary>
        /// <param name="DateCheck"></param>
        /// <param name="ProfileIDs"></param>
        /// <param name="lstTimeOffInLieulInput"></param>
        /// <returns></returns>
        public List<Att_RemainLeaveEntity> GetRemainLeave_TimeOffInLieu(DateTime DateCheck, List<Guid> ProfileIDs, List<Att_TimeOffInLieu> lstTimeOffInLieulInput)
        {
            DateTime DateEndYear = (new DateTime(DateCheck.Year, 1, 1)).AddYears(1).AddSeconds(-1);
            List<Att_RemainLeaveEntity> RemainLeaveResult = new List<Att_RemainLeaveEntity>();
            List<Att_TimeOffInLieu> lstTimeOffInLieu = lstTimeOffInLieulInput.Where(m => m.Date <= DateEndYear).ToList();

            foreach (var item in ProfileIDs)
            {
                Att_RemainLeaveEntity RemainLeave = new Att_RemainLeaveEntity();
                RemainLeave.ProfileID = item;
                var LstAnnualDetailByprofile = lstTimeOffInLieu.Where(m => m.ProfileID == item).ToList();
                if (LstAnnualDetailByprofile.Count > 0)
                {
                    var AnnualDetailByprofile_Present = LstAnnualDetailByprofile.Where(m => m.Date <= DateCheck).ToList();
                    if (AnnualDetailByprofile_Present != null)
                    {
                        RemainLeave.RemainEndMonth = AnnualDetailByprofile_Present.Sum(m => m.UnusualLeaves - m.TakenLeaves);
                    }
                    var AnnualDetailByprofile_EndYear = LstAnnualDetailByprofile.Where(m => m.Date <= DateEndYear).ToList();
                    if (AnnualDetailByprofile_EndYear != null)
                    {
                        RemainLeave.RemainEndYear = AnnualDetailByprofile_EndYear.Sum(m => m.UnusualLeaves - m.TakenLeaves);
                    }
                }
                RemainLeaveResult.Add(RemainLeave);
            }
            return RemainLeaveResult;
        }
    }
}
