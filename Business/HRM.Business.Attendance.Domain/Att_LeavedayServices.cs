using HRM.Data.BaseRepository;
using HRM.Data.Entity.Repositories;
//using HRM.Data.Attendance.Model;
using System.Linq;
//using HRM.Data.Main.Sql;
using HRM.Business.Attendance.Models;
using HRM.Business.Hr.Domain;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using System;
using HRM.Data.Attendance.Data.Sql.Repositories;
using HRM.Business.Main.Domain;
//using HRM.Data.Category.Model;
//using HRM.Data.Hr.Model;
using HRM.Business.Attendance.Domain;
using System.Data;
using HRM.Data.Entity;
using HRM.Data.Entity.Repositories;
using VnResource.Helper.Data;
using HRM.Business.Hr.Models;
using HRM.Business.Category.Models;
using HRM.Business.Category.Domain;

namespace HRM.Business.Attendance.Domain
{
    public class Att_LeavedayServices : BaseService
    {


        /// <summary>
        /// [Hieu.Van] 14/07/2014
        /// Thay Đổi trạng thái ngày nghỉ
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool UpdateStatusLeaveday(string selectedIds, string type)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Att_LeavedayRepository(unitOfWork);
                return repo.UpdateStatusLeaveday(selectedIds, type);
            }
        }

        #region ComputeLeaveLateEarly
        /// <summary>
        /// [Hieu.Van] 05/06/2014
        /// Phân Tích Nghỉ Phép Và Trễ Sớm
        /// </summary>
        /// <returns></returns>
        public List<Att_WorkdayEntity> ComputeLeaveLateEarly(DateTime? DateS, DateTime? DateE, List<Hre_ProfileEntity> lstProfileIDsFull, string udType)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                List<Att_WorkdayEntity> lstEntity = new List<Att_WorkdayEntity>();
                List<Guid> lstProfileIDs = lstProfileIDsFull.Select(s => s.ID).ToList();

                if (DateS == null || DateE == null)
                    return lstEntity;
                DateTime DateStart = DateS.Value;
                DateTime DateEnd = DateE.Value.Date.AddDays(1).AddMilliseconds(-1);
                string E_LATE_EARLY = WorkdayType.E_LATE_EARLY.ToString();
                string E_MISS_IN = WorkdayType.E_MISS_IN.ToString();
                string E_MISS_OUT = WorkdayType.E_MISS_OUT.ToString();
                string E_MISS_IN_OUT = WorkdayType.E_MISS_IN_OUT.ToString();
                var repoWorkday = new CustomBaseRepository<Att_Workday>(unitOfWork);

                var lstWorkday = repoWorkday
                    .FindBy(m => m.IsDelete == null
                        && ((m.Type == E_LATE_EARLY && m.LateEarlyDuration != null
                        && m.LateEarlyDuration > 0) || m.InTime1 == null || m.OutTime1 == null)
                        && m.WorkDate >= DateStart && m.WorkDate < DateEnd && lstProfileIDs.Contains(m.ProfileID))
                    .OrderBy(s => s.WorkDate)
                    .ToList().Translate<Att_WorkdayEntity>();

                SetStatusLeaveOnWorkday(lstWorkday);
                List<string> lstTypeData = null;
                if (udType != null)
                {
                    lstTypeData = udType.Split(',').ToList();
                }
                if (lstTypeData != null)
                {
                    if (!lstTypeData.Any(m => m == ComputeLeavedayType.E_DATA_LEAVE.ToString()))
                    {
                        lstWorkday = lstWorkday.Where(m => (m.udLeavedayCode1 == null || m.udLeavedayCode1 == string.Empty) && (m.udLeavedayCode2 == null || m.udLeavedayCode2 == string.Empty)).ToList();
                    }
                    if (!lstTypeData.Any(m => m == ComputeLeavedayType.E_DATA_NON_LEAVE.ToString()))
                    {
                        lstWorkday = lstWorkday.Where(m => m.udLeavedayCode1 != null || m.udLeavedayCode2 != null).ToList();
                    }
                }


                var repoShift = new Cat_ShiftRepository(unitOfWork);
                var lstShift = repoShift.FindBy(s => s.IsDelete == null).ToList();
                Att_WorkdayEntity entity = null;
                foreach (var workDay in lstWorkday)
                {
                    entity = new Att_WorkdayEntity();
                    if (workDay.ShiftID != null)
                    {
                        var shiftApprove = lstShift.FirstOrDefault(s => s.ID == workDay.ShiftID);
                        entity.ShiftApproveName = shiftApprove != null ? shiftApprove.ShiftName : string.Empty;
                        entity.ShiftName = shiftApprove != null ? shiftApprove.ShiftName : string.Empty;
                        entity.ShiftCode = shiftApprove != null ? shiftApprove.Code : string.Empty;
                    }
                    if (workDay.ProfileID != null)
                    {
                        var profile = lstProfileIDsFull.FirstOrDefault(s => s.ID == workDay.ProfileID);
                        entity.ProfileName = profile != null ? profile.ProfileName : string.Empty;
                        entity.CodeEmp = profile != null ? profile.CodeEmp : string.Empty;
                    }
                    entity.ID = workDay.ID;
                    entity.TotalRow = lstWorkday.Count;
                    entity.WorkDate = workDay.WorkDate;
                    entity.EarlyDuration1 = workDay.EarlyDuration1 ?? 0;
                    entity.EarlyDuration2 = workDay.EarlyDuration2 ?? 0;
                    entity.EarlyDuration3 = workDay.EarlyDuration3 ?? 0;
                    entity.EarlyDuration4 = workDay.EarlyDuration4 ?? 0;
                    entity.FirstInTime = workDay.FirstInTime;
                    entity.InTime1 = workDay.InTime1;
                    entity.InTime2 = workDay.InTime2;
                    entity.InTime3 = workDay.InTime3;
                    entity.InTime4 = workDay.InTime4;
                    entity.LastOutTime = workDay.LastOutTime;
                    entity.LateDuration1 = workDay.LateDuration1 ?? 0;
                    entity.LateDuration2 = workDay.LateDuration2 ?? 0;
                    entity.LateDuration3 = workDay.LateDuration3 ?? 0;
                    entity.LateDuration4 = workDay.LateDuration4 ?? 0;
                    entity.LateEarlyDuration = workDay.LateEarlyDuration ?? 0;
                    entity.OutTime1 = workDay.OutTime1;
                    entity.OutTime2 = workDay.OutTime2;
                    entity.OutTime3 = workDay.OutTime3;
                    entity.OutTime4 = workDay.OutTime4;
                    entity.ProfileID = workDay.ProfileID;
                    entity.ShiftActual = workDay.ShiftActual;
                    entity.ShiftApprove = workDay.ShiftApprove;
                    entity.ShiftID = workDay.ShiftID;
                    // entity.ShiftName = workDay.ShiftName;
                    entity.SrcType = workDay.SrcType;
                    entity.Status = workDay.Status;
                    entity.Type = workDay.Type;
                    entity.UserUpdate = workDay.UserUpdate;
                    entity.DateUpdate = workDay.DateUpdate;
                    entity.WorkDuration = workDay.WorkDuration;

                    entity.udLeavedayCode1 = workDay.udLeavedayCode1;
                    entity.udLeavedayCode2 = workDay.udLeavedayCode2;
                    entity.udLeavedayStatus1 = workDay.udLeavedayStatus1;
                    entity.udLeavedayStatus2 = workDay.udLeavedayStatus2;

                    lstEntity.Add(entity);
                }

                return lstEntity;
            }
        }

        /// <summary>
        /// Hàm cập nhật lại TotalDuration cho nhân viên
        /// </summary>
        /// <param name="lstLeaveIDs"></param>
        public string UpdateTotalDuration(List<Guid> lstLeaveIDs)
        {
            string message = "";

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Leaveday = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);
                var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);
                var repoCat_GradeAttendance = new CustomBaseRepository<Cat_GradeAttendance>(unitOfWork);
                var repoHre_WorkHistory = new CustomBaseRepository<Hre_WorkHistory>(unitOfWork);
                var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);
                var repoAtt_Roster = new CustomBaseRepository<Att_Roster>(unitOfWork);
                var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                var shifts = repoCat_Shift.FindBy(s => s.IsDelete == null).ToList();

                List<Att_LeaveDay> lstLeaveDay = repoAtt_Leaveday.FindBy(m => lstLeaveIDs.Contains(m.ID)).ToList<Att_LeaveDay>();
                if (lstLeaveDay == null || lstLeaveDay.Count == 0)
                {
                    message = ConstantMessages.LeavedayIsNotExist.TranslateString();
                    return message;
                }
                foreach (var item in lstLeaveDay)
                {
                    if (item.Status == AttendanceDataStatus.E_FIRST_APPROVED.ToString() || item.Status == AttendanceDataStatus.E_APPROVED.ToString())
                    {
                        message = ConstantMessages.StatusApproveCannotEdit.TranslateString();
                        return message;
                    }
                    if (item.Status == AttendanceDataStatus.E_REJECTED.ToString())
                    {
                        message = ConstantMessages.StatusRejectcannotEdit.TranslateString();
                        return message;
                    }
                }
                List<Cat_LeaveDayType> lstLeaveDayType = repoCat_LeaveDayType.GetAll().ToList<Cat_LeaveDayType>();
                List<Guid> lstProfileIDs = lstLeaveDay.Select(m => m.ProfileID).Distinct().ToList<Guid>();
                List<Hre_Profile> lstProfile = repoHre_Profile.FindBy(m => lstProfileIDs.Contains(m.ID)).ToList<Hre_Profile>();
                DateTime dateMin = lstLeaveDay.Min(m => m.DateStart).Date;
                DateTime dateMax = lstLeaveDay.Max(m => m.DateEnd);
                List<Att_Roster> lstRosterTypeGroup = new List<Att_Roster>();
                List<Att_RosterGroup> lstRosterGroup = new List<Att_RosterGroup>();
                var lstGrade = repoAtt_Grade.FindBy(m => lstProfileIDs.Contains((Guid)m.ProfileID))
                    .Select(m => new { m.ID, m.ProfileID, m.MonthStart, m.GradeAttendanceID })
                    .OrderByDescending(m => m.MonthStart)
                    .ToList();


                List<Cat_GradeAttendance> lstGradeCfg = repoCat_GradeAttendance.GetAll().ToList<Cat_GradeAttendance>();
                List<Hre_WorkHistory> listWorkHistory = repoHre_WorkHistory.FindBy(m => m.DateEffective <= dateMax && lstProfileIDs.Contains(m.ProfileID)).OrderByDescending(m => m.DateEffective).ToList<Hre_WorkHistory>();
                GetRosterGroup(lstProfileIDs, dateMin, dateMax, out lstRosterTypeGroup, out lstRosterGroup);
                List<Cat_DayOff> lstHoliday = repoCat_DayOff.GetAll().ToList<Cat_DayOff>();
                string E_APPROVED = RosterStatus.E_APPROVED.ToString();
                string E_ROSTERGROUP = RosterType.E_ROSTERGROUP.ToString();
                List<Att_Roster> lstRoster = repoAtt_Roster.FindBy(m => m.Status == E_APPROVED && m.DateStart <= dateMax && m.DateEnd >= dateMin && lstProfileIDs.Contains(m.ProfileID)).ToList<Att_Roster>();
                List<DateTime> lstHolidayType = lstHoliday.Select(m => m.DateOff).ToList<DateTime>();
                foreach (var item in lstLeaveDay)
                {
                    if (item.DurationType == null)
                        continue;
                    Cat_LeaveDayType leaveDayType = lstLeaveDayType.Where(m => m.ID == item.LeaveDayTypeID).FirstOrDefault();
                    if (leaveDayType == null)
                        continue;
                    Hre_Profile profile = lstProfile.Where(m => m.ID == item.ProfileID).FirstOrDefault();
                    if (profile == null)
                        continue;

                    DateTime dateFrom = item.DateStart;
                    DateTime dateTo = item.DateEnd;
                    //dateTo = dateTo.AddDays(1).AddMinutes(-1);



                    Guid? GradeCfgID = lstGrade.Where(m => m.ProfileID == profile.ID && m.MonthStart <= dateTo).Select(m => m.GradeAttendanceID).FirstOrDefault();
                    Cat_GradeAttendance gradeCfg = lstGradeCfg.Where(m => m.ID == GradeCfgID).FirstOrDefault();
                    List<Att_Roster> lstRosterByProfile = lstRoster.Where(m => m.ProfileID == profile.ID).ToList();
                    List<Hre_WorkHistory> listWorkHistoryByProfile = listWorkHistory.Where(m => m.ProfileID == profile.ID && m.DateEffective < dateTo).ToList();
                    List<Att_Roster> lstRosterByProfileTypeGroup = lstRosterByProfile.Where(m => m.Type == E_ROSTERGROUP).ToList();

                    AnalyseTotalLeaveDaysAndHours(item, leaveDayType, profile, gradeCfg, lstRosterByProfile, lstRosterGroup, listWorkHistoryByProfile, lstHoliday, shifts);
                    string LeaveDayTypeCode = leaveDayType.Code;

                    if (item.DurationType == null)
                    {
                        item.DurationType = LeaveDayDurationType.E_FULLSHIFT.ToString();
                    }
                    double Totalduration = 0;
                    bool isSetFullLeaveDay = false;
                    for (DateTime idx = dateFrom.Date; idx <= dateTo.Date; idx = idx.AddDays(1))
                    {
                        if (!string.IsNullOrEmpty(LeaveDayTypeCode) && (LeaveDayTypeCode == "SICK"
                                        || LeaveDayTypeCode == "PRG"
                                        || LeaveDayTypeCode == "SU"
                                        || LeaveDayTypeCode == "SD"
                                        || LeaveDayTypeCode == "D"
                                        || LeaveDayTypeCode == "DP"
                                        || LeaveDayTypeCode == "PSN"
                                        || LeaveDayTypeCode == "M"
                                        || LeaveDayTypeCode == "DSP"))
                        {
                            if (!lstHolidayType.Any(m => m == idx))
                            {
                                Totalduration += 1;
                            }
                            isSetFullLeaveDay = true;
                        }

                    }
                    if (isSetFullLeaveDay == false)
                    {
                        if (gradeCfg == null)
                        {
                            message = ConstantMessages.GradeAttendanceIsNotExist.TranslateString();
                            return message;
                        }

                        var listRosterEntity = lstRosterByProfile.Select(d => new Att_RosterEntity
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

                        Dictionary<DateTime, Cat_Shift> listMonthShifts = Att_AttendanceLib.GetDailyShifts(profile != null ? profile.ID : Guid.Empty, dateFrom, dateTo, listRosterEntity, listRosterGroupEntity, shifts);
                        for (DateTime idx = dateFrom.Date; idx <= dateTo.Date; idx = idx.AddDays(1))
                        {
                            if (gradeCfg != null && Att_WorkDayHelper.IsWorkDay(idx, gradeCfg, listMonthShifts, lstHoliday))
                            {
                                Totalduration += 1;
                            }
                        }
                    }

                    if (item.LeaveDays.HasChanged(Totalduration))
                    {
                        item.LeaveDays = Totalduration;
                        item.LeaveHours = 8;
                    }

                }

                unitOfWork.SaveChanges();
                message = ConstantMessages.Succeed.TranslateString();
                return message;
            }


















        }
        /// <summary>
        /// [Tam.Le] - 2014/08/08
        /// Hàm xử lý load dữ liệu
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        public DataTable LoadData(DateTime dateStart, DateTime dateEnd, List<Guid> orgIDs, string ProfileName, string CodeEmp, Guid LeavedayType, out string ErrorMessages)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                ErrorMessages = string.Empty;
                //bool isIncludeQuitEmp = true;
                List<Guid> leavedayTypeIds = new List<Guid>();
                if (LeavedayType != null)
                    leavedayTypeIds.Add(LeavedayType);

                List<Guid> shiftIDs = new List<Guid>();
                string E_WAIT_APPROVED = LeaveDayStatus.E_WAIT_APPROVED.ToString();
                string E_SUBMIT = LeaveDayStatus.E_SUBMIT.ToString();
                var repoAtt_Leaveday = new Att_LeavedayRepository(unitOfWork);
                List<Att_LeaveDay> leaveDays = repoAtt_Leaveday.FindBy(s => s.LeaveDays > 0
                    && (s.Status == E_WAIT_APPROVED || s.Status == E_SUBMIT) && dateStart <= s.DateEnd && s.DateStart <= dateEnd).ToList<Att_LeaveDay>();
                foreach (var item in leaveDays)
                {
                    item.Status = E_WAIT_APPROVED;
                }

                var profileIds = leaveDays.Select(s => s.ProfileID).Distinct().ToList();
                var repoAtt_Workday = new Att_WorkDayRepository(unitOfWork);
                var workDays = repoAtt_Workday.FindBy(s => profileIds.Contains(s.ProfileID) && dateStart <= s.WorkDate && s.WorkDate <= dateEnd)
                .Select(s => new { s.ProfileID, s.ShiftID, s.WorkDate, s.InTime1, s.OutTime1 }).ToList();
                var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoHre_Profile.FindBy(s => profileIds.Contains(s.ID))
                 .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
                var repoCat_OrgStructure = new Cat_OrgStructureRepository(unitOfWork);
                var orgs = repoCat_OrgStructure.FindBy(s => s.Code != null).ToList();
                var repoCat_Position = new Cat_PositionRepository(unitOfWork);
                var positions = repoCat_Position.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code }).ToList();
                var repoCat_JobTitle = new Cat_JobTitleRepository(unitOfWork);
                var jobtitles = repoCat_JobTitle.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code }).ToList();
                var repoCat_LeaveDayType = new Cat_LeaveDayTypeRepository(unitOfWork);
                var leavedayTypes = repoCat_LeaveDayType.FindBy(s => s.Code != null).Select(s => new { s.ID, s.Code, s.PaidRate }).Distinct().ToList();
                var ledvedayPaidIds = leavedayTypes.Where(s => s.PaidRate > 0).Select(s => s.ID).ToList();
                var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                var shifts = repoCat_Shift.GetAll().ToList();
                if (orgIDs != null)
                {
                    profiles = profiles.Where(s => s.OrgStructureID != null && orgIDs.Contains(s.OrgStructureID.Value)).ToList();
                }
                if (shiftIDs.Count > 0)
                {
                    workDays = workDays.Where(s => s.ShiftID.HasValue && shiftIDs.Contains(s.ShiftID.Value)).ToList();
                    profileIds = workDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }
                if (leavedayTypeIds.Count > 0)
                {
                    leaveDays = leaveDays.Where(s => leavedayTypeIds.Contains(s.LeaveDayTypeID)).ToList();
                    profileIds = leaveDays.Select(s => s.ProfileID).ToList();
                    profiles = profiles.Where(s => profileIds.Contains(s.ID)).ToList();
                }
                //if (!isIncludeQuitEmp)
                //{
                //    profiles = profiles.Where(s => s.DateQuit == null || s.DateQuit > dateStart).ToList();
                //}
                var startYear = new DateTime(dateStart.Year, 1, 1);
                var endYear = new DateTime(dateStart.Year, 12, DateTime.DaysInMonth(dateStart.Year, 12));
                var rosterStatus = RosterStatus.E_APPROVED.ToString();
                var repoAtt_Roster = new Att_RosterRepository(unitOfWork);
                List<Att_Roster> listRoster = repoAtt_Roster.FindBy(d =>
                        d.Status == rosterStatus && profileIds.Contains(d.ProfileID) && d.DateStart <= dateEnd && d.DateEnd >= startYear).ToList<Att_Roster>();
                var repoCat_DayOff = new Cat_DayOffRepository(unitOfWork);
                List<Cat_DayOff> listHoliday = repoCat_DayOff.FindBy(hol => hol.DateOff >= startYear && hol.DateOff <= endYear).ToList<Cat_DayOff>();
                var repoAtt_Grade = new Att_GradeRepository(unitOfWork);
                var listGrade = repoAtt_Grade.FindBy(d => d.MonthStart != null && d.MonthStart <= dateEnd
                && profileIds.Contains((Guid)d.ProfileID)).Select(d => new { d.ProfileID, d.MonthStart, d.GradeAttendanceID }).ToList();
                List<Guid?> listGradeID = listGrade.Select(d => d.GradeAttendanceID).ToList();
                var repoCat_Grade = new Cat_GradeAttendanceRepository(unitOfWork);
                List<Cat_GradeAttendance> listGradeCfg = repoCat_Grade.FindBy(d => listGradeID.Contains(d.ID)).ToList<Cat_GradeAttendance>();
                List<Att_Roster> lstRosterTypeGroup = new List<Att_Roster>();
                List<Att_RosterGroup> lstRosterGroup = new List<Att_RosterGroup>();
                GetRosterGroup(profileIds, startYear, dateEnd, out lstRosterTypeGroup, out lstRosterGroup);
                var repoHre_WorkHistory = new Hre_WorkHistoryRepository(unitOfWork);
                List<Hre_WorkHistory> listWorkHistory = repoHre_WorkHistory.FindBy(d => profileIds.Contains(d.ProfileID) && d.DateEffective <= dateEnd).ToList<Hre_WorkHistory>();
                var orgTypes = new List<Cat_OrgStructureType>();
                var repoorgType = new Cat_OrgStructureTypeRepository(unitOfWork);
                orgTypes = repoorgType.FindBy(s => s.IsDelete == null).ToList<Cat_OrgStructureType>();
                DataTable table = GetSchema();
                List<string> codeRejects = new List<string>();
                codeRejects.Add("SICK");
                codeRejects.Add("SU");
                codeRejects.Add("SD");
                codeRejects.Add("D");
                codeRejects.Add("D");
                codeRejects.Add("DP");
                codeRejects.Add("PSN");
                codeRejects.Add("DSP");
                codeRejects.Add("M");
                List<Guid> leaveDayTypeRejectIDs = leavedayTypes.Where(s => codeRejects.Contains(s.Code)).Select(s => s.ID).ToList();
                foreach (var profile in profiles)
                {
                    var grade = listGrade.Where(d => d.ProfileID == profile.ID && d.MonthStart <= dateEnd).OrderByDescending(d => d.MonthStart).FirstOrDefault();
                    Cat_GradeAttendance gradeCfg = listGradeCfg.FirstOrDefault(d => grade != null && d.ID == grade.GradeAttendanceID);
                    List<Hre_WorkHistory> listWorkHistoryByProfile = listWorkHistory.Where(d => d.ProfileID == profile.ID).ToList();
                    List<Att_Roster> listRosterByProfile = listRoster.Where(d => d.ProfileID == profile.ID && d.DateStart <= dateEnd && d.DateEnd >= dateStart).ToList();

                    var listRosterEntity = listRosterByProfile.Select(d => new Att_RosterEntity
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

                    Dictionary<DateTime, Cat_Shift> listMonthShifts = Att_AttendanceLib.GetDailyShifts(profile.ID, dateStart, dateEnd, listRosterEntity, listRosterGroupEntity, shifts);
                    for (DateTime date = dateStart; date <= dateEnd; date = date.AddDays(1))
                    {

                        var leavdayProfiles = leaveDays.Where(s => s.DateStart.Date <= date.Date && date.Date <= s.DateEnd.Date && s.ProfileID == profile.ID).ToList();
                        if (leavdayProfiles.Count > 0)
                        {
                            bool isWorkDay = gradeCfg != null && Att_WorkDayHelper.IsWorkDay(date, gradeCfg, listMonthShifts, listHoliday);
                            var leaveday = leavdayProfiles.FirstOrDefault(s => ledvedayPaidIds.Contains(s.LeaveDayTypeID));
                            if (!isWorkDay)// neu ngay do ko phai ngay di lam
                            {
                                if (leaveday != null && leaveDayTypeRejectIDs.Contains(leaveday.LeaveDayTypeID) || listHoliday.Exists(s => s.DateOff == date))// neu ngay do la ngay nghi dc xem la ko xem ca or ngay nghi
                                {
                                    isWorkDay = true;
                                }
                            }
                            if (isWorkDay)
                            {
                                var workDay = workDays.FirstOrDefault(s => s.ProfileID == profile.ID && s.WorkDate.Date == date.Date);
                                DataRow row = table.NewRow();
                                Guid? orgId = profile.OrgStructureID;
                                var org = orgs.FirstOrDefault(s => s.ID == profile.OrgStructureID);
                                var orgBranch = LibraryService.GetNearestParent(orgId, OrgUnit.E_BRANCH, orgs, orgTypes);
                                var orgOrg = LibraryService.GetNearestParent(orgId, OrgUnit.E_DEPARTMENT, orgs, orgTypes);
                                var orgTeam = LibraryService.GetNearestParent(orgId, OrgUnit.E_TEAM, orgs, orgTypes);
                                var orgSection = LibraryService.GetNearestParent(orgId, OrgUnit.E_SECTION, orgs, orgTypes);
                                row[Att_LeaveDayEntity.FieldNames.CodeBranch] = orgBranch != null ? orgBranch.Code : string.Empty;
                                row[Att_LeaveDayEntity.FieldNames.CodeOrg] = orgOrg != null ? orgOrg.Code : string.Empty;
                                row[Att_LeaveDayEntity.FieldNames.CodeTeam] = orgTeam != null ? orgTeam.Code : string.Empty;
                                row[Att_LeaveDayEntity.FieldNames.CodeSection] = orgSection != null ? orgSection.Code : string.Empty;
                                row[Att_LeaveDayEntity.FieldNames.BranchName] = orgBranch != null ? orgBranch.OrgStructureName : string.Empty;
                                row[Att_LeaveDayEntity.FieldNames.OrgName] = orgOrg != null ? orgOrg.OrgStructureName : string.Empty;
                                row[Att_LeaveDayEntity.FieldNames.TeamName] = orgTeam != null ? orgTeam.OrgStructureName : string.Empty;
                                row[Att_LeaveDayEntity.FieldNames.SectionName] = orgSection != null ? orgSection.OrgStructureName : string.Empty;
                                var positon = positions.FirstOrDefault(s => s.ID == profile.PositionID);
                                var jobtitle = jobtitles.FirstOrDefault(s => s.ID == profile.JobTitleID);
                                row[Att_LeaveDayEntity.FieldNames.ProfileName] = profile.ProfileName;
                                row[Att_LeaveDayEntity.FieldNames.CodeEmp] = profile.CodeEmp;
                                row[Att_LeaveDayEntity.FieldNames.CodePosition] = positon != null ? positon.Code : string.Empty;
                                row[Att_LeaveDayEntity.FieldNames.CodeJobtitle] = jobtitle != null ? jobtitle.Code : string.Empty;
                                var shift = shifts.FirstOrDefault(s => workDay != null && s.ID == workDay.ShiftID);
                                row[Att_LeaveDayEntity.FieldNames.Date] = date;
                                row[Att_LeaveDayEntity.FieldNames.DateFrom] = dateStart;
                                row[Att_LeaveDayEntity.FieldNames.DateTo] = dateEnd;
                                row[Att_LeaveDayEntity.FieldNames.DateExport] = DateTime.Now;
                                //row[Att_LeavedayEntity.FieldNames.UserExport] = Session[SessionObjects.UserLogin];
                                if (leaveday != null && leaveday.LeaveDays > 0)
                                {
                                    row["Paid"] = "X";
                                }
                                if (workDay != null)
                                {
                                    row[Att_LeaveDayEntity.FieldNames.Cat_Shift] = shift.ShiftName;
                                    row[Att_LeaveDayEntity.FieldNames.udInTime] = workDay.InTime1 != null ? workDay.InTime1.Value.ToString("HH:mm") : string.Empty;
                                    row[Att_LeaveDayEntity.FieldNames.udOutTime] = workDay.OutTime1 != null ? workDay.OutTime1.Value.ToString("HH:mm") : string.Empty;
                                }
                                foreach (var leaday in leavedayTypes)
                                {
                                    var leaday1 = leavdayProfiles.FirstOrDefault(s => s.LeaveDayTypeID == leaday.ID);
                                    if (leaday1 != null && leaday1.LeaveDays > 0)
                                    {
                                        row[leaday.Code] = "X";
                                    }
                                }

                                if (leaveday != null)
                                {
                                    row[Att_LeaveDayEntity.FieldNames.Status] = leaveday.Status;
                                }

                                table.Rows.Add(row);
                            }
                        }

                    }


                }
                //EntityService.SubmitChanges(GuidContext, LoginUserID.Value);
                context.SaveChanges();
                return table;
            }
        }
        /// <summary>
        /// [Tam.Le] - 2014/08/08
        /// Tạo cấu trúc bảng cho hàm Load Data của att_leaveday
        /// </summary>
        /// <param name="request"></param>
        /// <param name="otModel"></param>
        /// <returns></returns>
        DataTable GetSchema()
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                DataTable tb = new DataTable();
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.CodeEmp);
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.ProfileName);
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.CodeBranch);
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.CodeOrg);
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.CodeTeam);
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.CodeSection);
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.CodeJobtitle);
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.CodePosition);
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.BranchName);
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.OrgName);
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.TeamName);
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.SectionName);
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.Paid);
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.Date, typeof(DateTime));
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.DateFrom, typeof(DateTime));
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.DateTo, typeof(DateTime));
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.udInTime);
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.udOutTime);
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.Cat_Shift);
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.Status);
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.DateExport, typeof(DateTime));
                tb.Columns.Add(Att_LeaveDayEntity.FieldNames.UserExport);
                var repoCat_LeaveDayType = new Cat_LeaveDayTypeRepository(unitOfWork);
                var codes = repoCat_LeaveDayType.FindBy(s => s.Code != null).Select(s => s.Code).Distinct().ToList<string>();
                foreach (string code in codes)
                {
                    if (!tb.Columns.Contains(code))
                    {
                        tb.Columns.Add(code);
                    }
                }
                return tb;
            }
        }

        public static void GetRosterGroup(List<Guid> lstProfileID, DateTime? DateFrom, DateTime? DateTo, out List<Att_Roster> lstRosterTypeGroup, out List<Att_RosterGroup> lstRosterGroup)
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
        #endregion

        #region AnalysisLeaveAndLate
        /// <summary>
        /// [Son.Vo] 05/06/2014
        /// Phân Tích Nghỉ Phép Và Trễ Sớm
        /// </summary>
        /// <returns></returns>
        public List<Att_AnalysysLeaveAndLateEarlyEntity> AnalysisLeaveAndLate(DateTime DateS, DateTime DateE, List<Guid> lstProfileIDs)
        {
            using (var context = new VnrHrmDataContext())
            {
                if (DateS == null || DateE == null)
                    return null;
                DateTime DateStart = DateS;
                DateTime DateEnd = DateE;
                string E_LATE_EARLY = WorkdayType.E_LATE_EARLY.ToString();
                var lstWorkday = new List<Att_WorkdayEntity>();

                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var repo = new CustomBaseRepository<Att_Workday>(unitOfWork);
                lstWorkday = repo
                    .FindBy(m => m.IsDelete == null && ((m.Type == E_LATE_EARLY && m.LateEarlyDuration != null && m.LateEarlyDuration > 0) || m.InTime1 == null || m.OutTime1 == null)
                    && m.WorkDate >= DateStart && m.WorkDate < DateEnd && lstProfileIDs.Contains(m.ProfileID))
                    .ToList()
                    .Translate<Att_WorkdayEntity>();
                SetStatusLeaveOnWorkday(lstWorkday);
                var profileIds = lstWorkday.Select(s => s.ProfileID).Distinct().ToList();

                var repoHre_Profile = new Hre_ProfileRepository(unitOfWork);
                var profiles = repoHre_Profile.FindBy(s => profileIds.Contains(s.ID))
                 .Select(s => new { s.ID, s.DateQuit, s.OrgStructureID, s.ProfileName, s.CodeEmp, s.PositionID, s.JobTitleID }).ToList();
                List<Guid?> shiftids = lstWorkday.Select(m => m.ShiftID).ToList();

                var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                var shifts = repoCat_Shift.FindBy(s => shiftids.Contains(s.ID))
                 .Select(s => new { s.ID, s.ShiftName }).ToList();

                List<Att_AnalysysLeaveAndLateEarlyEntity> lstAnalysysLeaveAndLateEarly = new List<Att_AnalysysLeaveAndLateEarlyEntity>();
                foreach (var item in profiles)
                {
                    var lstWorkdayByProfile = lstWorkday.Where(m => m.ProfileID == item.ID).ToList();
                    foreach (var WorkdayByProfile in lstWorkdayByProfile)
                    {
                        Att_AnalysysLeaveAndLateEarlyEntity analysysLeaveAndLateEarly = new Att_AnalysysLeaveAndLateEarlyEntity();
                        var profileshifts = shifts.Where(m => m.ID == WorkdayByProfile.ShiftID).FirstOrDefault();
                        analysysLeaveAndLateEarly.ProfileName = item.ProfileName;
                        analysysLeaveAndLateEarly.CodeEmp = item.CodeEmp;
                        analysysLeaveAndLateEarly.WorkDate = WorkdayByProfile.WorkDate;
                        if (profileshifts != null)
                        {
                            analysysLeaveAndLateEarly.ShiftName = profileshifts.ShiftName;
                        }
                        analysysLeaveAndLateEarly.InTime = WorkdayByProfile.FirstInTime;
                        analysysLeaveAndLateEarly.OutTime = WorkdayByProfile.LastOutTime;
                        analysysLeaveAndLateEarly.udLeavedayCode1 = WorkdayByProfile.udLeavedayCode1;
                        analysysLeaveAndLateEarly.udLeavedayStatus1 = WorkdayByProfile.udLeavedayStatus1;
                        analysysLeaveAndLateEarly.Late = WorkdayByProfile.LateDuration1;
                        analysysLeaveAndLateEarly.Early = WorkdayByProfile.EarlyDuration1;
                        analysysLeaveAndLateEarly.LateEarly = WorkdayByProfile.LateEarlyDuration;

                        lstAnalysysLeaveAndLateEarly.Add(analysysLeaveAndLateEarly);
                    }
                }
                return lstAnalysysLeaveAndLateEarly;
            }
        }


        public void SetStatusLeaveOnWorkday(List<Att_WorkdayEntity> lstWorkday)
        {
            if (lstWorkday == null || lstWorkday.Count == 0)
                return;
            List<Guid> lstProfileId = lstWorkday.Select(m => m.ProfileID).ToList();
            DateTime DateMin = lstWorkday.Min(m => m.WorkDate);
            DateTime DateMax = lstWorkday.Max(m => m.WorkDate);
            DateMax = DateMax.Date.AddDays(1).AddMinutes(-1);
            string E_CANCEL = AttendanceDataStatus.E_CANCEL.ToString();
            string E_REJECTED = AttendanceDataStatus.E_REJECTED.ToString();
            var lstLeaveDay = new List<Att_LeaveDay>().Select(m => new { m.ID, m.ProfileID, m.DateStart, m.DateEnd, m.LeaveHours, m.LeaveDayTypeID, m.Status }).ToList();
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_Leaveday = new Att_LeavedayRepository(unitOfWork);
                var repoCat_DayOff = new Cat_DayOffRepository(unitOfWork);
                var repoCat_LeaveDayType = new Cat_LeaveDayTypeRepository(unitOfWork);

                lstLeaveDay = repoAtt_Leaveday
                    .FindBy(m => m.Status != E_CANCEL && m.Status != E_REJECTED && m.DateStart <= DateMax
                        && m.DateEnd >= DateMin && lstProfileId.Contains(m.ProfileID) && m.IsDelete == null)
                    .Select(m => new { m.ID, m.ProfileID, m.DateStart, m.DateEnd, m.LeaveHours, m.LeaveDayTypeID, m.Status }).ToList();
                var lstLeaveType = repoCat_LeaveDayType.FindBy(s => s.IsDelete == null).Select(m => new { m.ID, m.Code, m.CodeStatistic }).ToList();
                var lstDayOffHoliday = repoCat_DayOff.FindBy(m => m.DateOff != null && m.IsDelete == null).Select(m => new { m.DateOff, m.Type }).ToList();

                foreach (var item in lstWorkday)
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
                        item.udLeavedayCode1 = leavedayCode;
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
                                if (leavetype != null)
                                {
                                    string leavedayCode = !string.IsNullOrEmpty(leavetype.CodeStatistic) ? leavetype.CodeStatistic : leavetype.Code;
                                    item.udLeavedayCode1 = leavedayCode;
                                }
                                item.udLeavedayStatus1 = leaveDay.Status;
                            }
                            else if (Num == 2)
                            {
                                var leavetype = lstLeaveType.Where(m => m.ID == leaveDay.LeaveDayTypeID).FirstOrDefault();
                                if (leavetype != null)
                                {
                                    string leavedayCode = !string.IsNullOrEmpty(leavetype.CodeStatistic) ? leavetype.CodeStatistic : leavetype.Code;
                                    item.udLeavedayCode2 = leavedayCode;
                                }
                                item.udLeavedayStatus2 = leaveDay.Status;
                            }
                        }
                    }
                }
            }
            return;
        }
        #endregion

        #region [Hien.Nguyen]
        public void SaveLeaveData(List<Att_WorkdayEntity> lstWorkDate, Guid LeaveDayCode, Guid? UserApproved,string userLogin, string Comment)
        {
            string status = string.Empty;
            List<Att_LeaveDayEntity> lstLeaveDaySave = new List<Att_LeaveDayEntity>();
            var workDate = new Att_WorkDayServices();
            var hre_Profile = new Hre_ProfileServices();
            #region getData
            List<object> lstobject = new List<object>();
            lstobject.AddRange(new object[18]);
            lstobject[16] = 1;
            lstobject[17] = int.MaxValue - 1;

            List<Guid> lstProfileId = lstWorkDate.Select(m => m.ProfileID).ToList();

            var lstProfile = hre_Profile.GetData<Hre_ProfileEntity>(lstobject, ConstantSql.hrm_hr_sp_get_Profile, userLogin, ref status).Where(m => lstProfileId.Contains(m.ID)).Select(m => new { m.ID, m.CodeEmp, m.ProfileName }).ToList();

            #endregion
            string Duplicate = string.Empty;
            foreach (var item in lstWorkDate)
            {
                if (!string.IsNullOrEmpty(item.udLeavedayCode1))
                {
                    var profile = lstProfile.Where(m => m.ID == item.ProfileID).FirstOrDefault();
                    if (profile != null)
                    {
                        bool isHaveValue = false;
                        if (profile.ProfileName != string.Empty)
                        {
                            Duplicate += profile.ProfileName;
                            isHaveValue = true;
                        }
                        if (profile.CodeEmp != string.Empty)
                        {
                            Duplicate += "[" + profile.CodeEmp + "]";
                            isHaveValue = true;
                        }
                        if (isHaveValue)
                        {
                            Duplicate += "; ";
                        }
                    }
                    continue;
                }
                //Att_WorkdayEntity WorkdayModify = SaveLeaveDataItem(item.ID, LeaveDayCode, UserApproved, Comment, false);
                var message = SaveLeaveDataItem(item.ID, LeaveDayCode, UserApproved, Comment, false);
            }
            //DataErrorCode ErrorCode = EntityService.SubmitChanges(GuidContext, LoginUserID);
            return;
        }



        public String SaveLeaveDataItem(Guid workDayID, Guid LeaveTypeID, Guid? userApprove, string comment, bool IsAllowModify)
        {
            var message = "";
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_LeaveDay = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);
                var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                var repoCat_Shift = new CustomBaseRepository<Cat_Shift>(unitOfWork);
                var repoAtt_Workday = new CustomBaseRepository<Att_Workday>(unitOfWork);
                var repoSys_AllSeting = new CustomBaseRepository<Sys_AllSetting>(unitOfWork);
                var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);
                var repoCat_GradeAttendance = new CustomBaseRepository<Cat_GradeAttendance>(unitOfWork);
                var repoHre_WorkHistory = new CustomBaseRepository<Hre_WorkHistory>(unitOfWork);
                var repoCat_DayOff = new CustomBaseRepository<Cat_DayOff>(unitOfWork);
                var repoAtt_Roster = new CustomBaseRepository<Att_Roster>(unitOfWork);

                var shifts = repoCat_Shift.FindBy(s => s.IsDelete == null).ToList();
                Att_WorkdayEntity WorkDayInDB = repoAtt_Workday.FindBy(m => m.ID == workDayID).FirstOrDefault().CopyData<Att_WorkdayEntity>();
                if (WorkDayInDB == null || LeaveTypeID == Guid.Empty)
                {
                    //return WorkDayInDB.CopyData<Att_WorkdayEntity>();
                    message = ConstantMessages.PlsSelectLeaveType.TranslateString();
                    return message;
                }
                #region [Hien.Nguyen] - Nếu ngày đó đã có loại ngày nghỉ này rồi thì out function
                var tmp = repoAtt_LeaveDay.FindBy(m => m.IsDelete == null && m.ProfileID == WorkDayInDB.ProfileID && m.LeaveDayTypeID == LeaveTypeID && m.DateStart <= WorkDayInDB.WorkDate && m.DateEnd >= WorkDayInDB.WorkDate).FirstOrDefault();
                if (tmp != null)
                {
                    message = "Đã Có Ngày Nghỉ";
                    return message;
                    //return WorkDayInDB.CopyData<Att_WorkdayEntity>();
                }
                #endregion



                Cat_Shift shift = repoCat_Shift.FindBy(m => m.ID == WorkDayInDB.ShiftID).FirstOrDefault();
                if (shift == null)
                {
                    //return WorkDayInDB.CopyData<Att_WorkdayEntity>();
                    message = ConstantMessages.Error.TranslateString();
                    return message;
                }
                var leaveType = repoCat_LeaveDayType.FindBy(m => m.ID == LeaveTypeID).Select(m => new { m.Code, m.CodeStatistic }).FirstOrDefault();
                var leaveTypeCode = string.Empty;
                if (leaveType != null)
                {
                    leaveTypeCode = leaveType.CodeStatistic ?? leaveType.Code;
                }
                DateTime workday = WorkDayInDB.WorkDate;
                DateTime beginDate = workday.Date;
                DateTime endDate = beginDate.AddDays(1).AddMinutes(-1);

                DateTime beginShift = workday.AddHours(shift.InTime.Hour).AddMinutes(shift.InTime.Minute);
                DateTime endShift = beginShift.AddHours(shift.CoOut);
                List<Att_LeaveDay> lstLeaveDayInDbUpdate = repoAtt_LeaveDay.FindBy(m =>
                    m.DateStart <= endDate && m.DateEnd >= beginDate && m.ProfileID == WorkDayInDB.ProfileID).ToList<Att_LeaveDay>();

                List<Att_LeaveDay> lstLeaveDayInsert = new List<Att_LeaveDay>();
                if (WorkDayInDB.InTime1 == null || WorkDayInDB.OutTime1 == null)
                //Thiếu 1 trong 2 la tao nghi full ca
                {
                    if (IsAllowModify && lstLeaveDayInDbUpdate.Count > 0 && !lstLeaveDayInDbUpdate.Any(m => m.Status == LeaveDayStatus.E_APPROVED.ToString()))
                    {
                        lstLeaveDayInDbUpdate.ForEach(m => m.LeaveDayTypeID = LeaveTypeID);
                    }
                    else
                    {
                        Att_LeaveDay LeavedayInsert = new Att_LeaveDay();
                        LeavedayInsert.ID = Guid.NewGuid();
                        LeavedayInsert.ProfileID = WorkDayInDB.ProfileID;
                        LeavedayInsert.TotalDuration = 1;
                        LeavedayInsert.LeaveDays = 1;
                        LeavedayInsert.Duration = shift.WorkHours ?? 8;
                        LeavedayInsert.LeaveHours = shift.WorkHours ?? 8;
                        LeavedayInsert.DurationType = LeaveDayDurationType.E_FULLSHIFT.ToString();
                        LeavedayInsert.DateStart = workday;
                        LeavedayInsert.DateEnd = workday;
                        LeavedayInsert.LeaveDayTypeID = LeaveTypeID;
                        LeavedayInsert.Status = LeaveDayStatus.E_SUBMIT.ToString();
                        lstLeaveDayInsert.Add(LeavedayInsert);
                    }
                }
                else // thuộc loại trễ sớm
                {
                    if (IsAllowModify && lstLeaveDayInDbUpdate.Count > 0 && !lstLeaveDayInDbUpdate.Any(m => m.Status == LeaveDayStatus.E_APPROVED.ToString()))
                    {
                        lstLeaveDayInDbUpdate.ForEach(m => m.LeaveDayTypeID = LeaveTypeID);
                    }
                    else
                    {
                        if (WorkDayInDB.LateDuration1 != null && WorkDayInDB.LateDuration1 > 0) //đi trễ
                        {
                            double HourLeave = (int)(WorkDayInDB.LateDuration1 ?? 0) / 60;
                            if (((WorkDayInDB.LateDuration1 ?? 0) % 60) > 0)
                                HourLeave = HourLeave + 1;
                            //di trễ thì loại của no la nua ca dau 
                            Att_LeaveDay LeavedayInsert = new Att_LeaveDay();
                            LeavedayInsert.ID = Guid.NewGuid();
                            LeavedayInsert.ProfileID = WorkDayInDB.ProfileID;
                            LeavedayInsert.TotalDuration = 1;
                            LeavedayInsert.LeaveDays = 1;
                            LeavedayInsert.Duration = HourLeave;
                            LeavedayInsert.LeaveHours = HourLeave;
                            LeavedayInsert.DurationType = LeaveDayDurationType.E_FIRSTHALFSHIFT.ToString();
                            LeavedayInsert.DateStart = beginShift;
                            LeavedayInsert.DateEnd = beginShift.AddHours(HourLeave);
                            LeavedayInsert.LeaveDayTypeID = LeaveTypeID;
                            LeavedayInsert.Status = LeaveDayStatus.E_SUBMIT.ToString();
                            lstLeaveDayInsert.Add(LeavedayInsert);

                        }
                        else if (WorkDayInDB.EarlyDuration1 != null && WorkDayInDB.EarlyDuration1 > 0) //Về sớm
                        {
                            //về sơm thi loại cua no la nua ca sau
                            double HourLeave = (int)(WorkDayInDB.EarlyDuration1 ?? 0) / 60;
                            if (((WorkDayInDB.EarlyDuration1 ?? 0) % 60) > 0)
                                HourLeave = HourLeave + 1;
                            //di trễ thì loại của no la nua ca sai
                            Att_LeaveDay LeavedayInsert = new Att_LeaveDay();
                            LeavedayInsert.ID = Guid.NewGuid();
                            LeavedayInsert.ProfileID = WorkDayInDB.ProfileID;
                            LeavedayInsert.TotalDuration = 1;
                            LeavedayInsert.LeaveDays = 1;
                            LeavedayInsert.Duration = HourLeave;
                            LeavedayInsert.LeaveHours = HourLeave;
                            LeavedayInsert.DurationType = LeaveDayDurationType.E_FIRSTHALFSHIFT.ToString();
                            LeavedayInsert.DateStart = endShift.AddHours(-HourLeave);
                            LeavedayInsert.DateEnd = endShift;
                            LeavedayInsert.LeaveDayTypeID = LeaveTypeID;
                            LeavedayInsert.Status = LeaveDayStatus.E_SUBMIT.ToString();
                            lstLeaveDayInsert.Add(LeavedayInsert);

                        }
                    }
                }
                foreach (var item in lstLeaveDayInsert)
                {
                    if (userApprove != null)
                    {
                        item.UserApproveID = userApprove.Value;
                    }
                    item.Comment = comment;
                }
                #region Triet.Mai Validate Nghỉ Bù
                if (lstLeaveDayInsert.Count > 0)
                {
                    List<Guid> LeaveTypeDayOffID = repoCat_LeaveDayType.FindBy(m => m.IsTimeOffInLieu == true).Select(m => m.ID).ToList<Guid>();
                    if (lstLeaveDayInsert.FirstOrDefault().Status != OverTimeStatus.E_CANCEL.ToString() && LeaveTypeDayOffID.Any(m => m == lstLeaveDayInsert.FirstOrDefault().LeaveDayTypeID))
                    {
                        message = ValidateLeaveDayTimeOff(lstLeaveDayInsert.Select(m => m.ProfileID).Distinct().ToList(), lstLeaveDayInsert);
                        if (message != string.Empty)
                        {
                            //ErrorMessages = Error;
                            return message;
                        }
                    }
                }
                #endregion

                #region triet.mai validate vấn đề ngày nghỉ dành cho nhân viên thực tập
                string HRM_ATT_STAFF_PROBATION = AppConfig.HRM_ATT_STAFF_PROBATION.ToString();
                Sys_AllSetting config = repoSys_AllSeting.FindBy(m => m.Name == HRM_ATT_STAFF_PROBATION).FirstOrDefault();
                string validateEmpProbation = ValidateLeaveTypeByNewEmployee(config, lstLeaveDayInsert);
                if (validateEmpProbation != string.Empty)
                {
                    //ErrorMessages = validateEmpProbation;
                    return null;
                }
                #endregion

                //Cap nhat leavedayID cho workday o day
                if (lstLeaveDayInsert.Count > 0)
                {
                    if (lstLeaveDayInsert.Count > 1)
                    {
                        WorkDayInDB.LeaveDayID1 = lstLeaveDayInsert[0].ID;
                        WorkDayInDB.LeaveDayID2 = lstLeaveDayInsert[1].ID;
                        WorkDayInDB.udLeavedayCode1 = leaveTypeCode;
                        WorkDayInDB.udLeavedayCode2 = leaveTypeCode;
                        WorkDayInDB.udLeavedayStatus1 = lstLeaveDayInsert.FirstOrDefault().Status;
                        WorkDayInDB.udLeavedayStatus2 = lstLeaveDayInsert.FirstOrDefault().Status;
                    }
                    else
                    {
                        WorkDayInDB.LeaveDayID1 = lstLeaveDayInsert[0].ID;
                        WorkDayInDB.udLeavedayCode1 = leaveTypeCode;
                        WorkDayInDB.udLeavedayStatus1 = lstLeaveDayInsert.FirstOrDefault().Status;
                    }
                }
                else if (lstLeaveDayInDbUpdate.Count > 0)
                {
                    if (lstLeaveDayInDbUpdate.Count >= 2)
                    {
                        WorkDayInDB.udLeavedayCode1 = leaveTypeCode;
                        WorkDayInDB.udLeavedayCode2 = leaveTypeCode;
                        WorkDayInDB.udLeavedayStatus1 = lstLeaveDayInDbUpdate.FirstOrDefault().Status;
                        WorkDayInDB.udLeavedayStatus2 = lstLeaveDayInDbUpdate.FirstOrDefault().Status;
                    }
                    else
                    {
                        WorkDayInDB.udLeavedayCode1 = leaveTypeCode;
                        WorkDayInDB.udLeavedayStatus1 = lstLeaveDayInDbUpdate.FirstOrDefault().Status;
                        WorkDayInDB.udLeavedayStatus2 = lstLeaveDayInDbUpdate.FirstOrDefault().Status;
                    }
                }

                #region triet.mai cap nhat leaveDays va leaveHours
                List<Cat_LeaveDayType> lstLeaveDayType = repoCat_LeaveDayType.GetAll().ToList<Cat_LeaveDayType>();
                List<Guid> lstProfileID1 = lstLeaveDayInsert.Select(m => m.ProfileID).Distinct().ToList<Guid>();
                List<Hre_Profile> lstProfile = repoHre_Profile.FindBy(m => lstProfileID1.Contains(m.ID)).ToList<Hre_Profile>();
                DateTime dateMin1 = lstLeaveDayInsert.Min(m => m.DateStart).Date;
                DateTime dateMax1 = lstLeaveDayInsert.Max(m => m.DateEnd);
                List<Att_Roster> lstRosterTypeGroup = new List<Att_Roster>();
                List<Att_RosterGroup> lstRosterGroup = new List<Att_RosterGroup>();
                var lstGrade = repoAtt_Grade.FindBy(m => lstProfileID1.Contains((Guid)m.ProfileID))
                    .Select(m => new { m.ID, m.ProfileID, m.MonthStart, m.GradeAttendanceID })
                    .OrderByDescending(m => m.MonthStart)
                    .ToList();
                List<Cat_GradeAttendance> lstGradeCfg = repoCat_GradeAttendance.GetAll().ToList<Cat_GradeAttendance>();
                List<Hre_WorkHistory> listWorkHistory = repoHre_WorkHistory.FindBy(m => m.DateEffective <= dateMax1 && lstProfileID1.Contains(m.ProfileID)).OrderByDescending(m => m.DateEffective).ToList<Hre_WorkHistory>();
                GetRosterGroup(lstProfileID1, dateMin1, dateMax1, out lstRosterTypeGroup, out lstRosterGroup);
                List<Cat_DayOff> lstHoliday = repoCat_DayOff.GetAll().ToList();
                string E_APPROVED1 = RosterStatus.E_APPROVED.ToString();
                string E_ROSTERGROUP = RosterType.E_ROSTERGROUP.ToString();
                List<Att_Roster> lstRoster = repoAtt_Roster.FindBy(m => m.Status == E_APPROVED1 && m.DateStart <= dateMax1 && m.DateEnd >= dateMin1 && lstProfileID1.Contains(m.ProfileID)).ToList<Att_Roster>();
                List<DateTime> lstHolidayType = lstHoliday.Select(m => m.DateOff).ToList<DateTime>();
                //LeaveDayDAO ldDao = new LeaveDayDAO();
                foreach (var item in lstLeaveDayInsert)
                {
                    if (item.DurationType != null && item.DurationType != LeaveDayDurationType.E_FULLSHIFT.ToString())
                        continue;
                    Cat_LeaveDayType leaveDayType = lstLeaveDayType.Where(m => m.ID == item.LeaveDayTypeID).FirstOrDefault();
                    if (leaveDayType == null)
                        continue;
                    Hre_Profile profileInLeave = lstProfile.Where(m => m.ID == item.ProfileID).FirstOrDefault();
                    if (profileInLeave == null)
                        continue;

                    DateTime dateFrom = item.DateStart;
                    DateTime dateTo = item.DateEnd;
                    dateTo = dateTo.AddDays(1).AddMinutes(-1);

                    Guid GradeCfgID = lstGrade.Where(m => m.ProfileID.Value == profileInLeave.ID && m.MonthStart <= dateTo).Select(m => m.GradeAttendanceID.Value).FirstOrDefault();
                    Cat_GradeAttendance gradeCfg = lstGradeCfg.Where(m => m.ID == GradeCfgID).FirstOrDefault();
                    List<Att_Roster> lstRosterByProfile = lstRoster.Where(m => m.ProfileID == profileInLeave.ID).ToList();
                    List<Hre_WorkHistory> listWorkHistoryByProfile = listWorkHistory.Where(m => m.ProfileID == profileInLeave.ID && m.DateEffective < dateTo).ToList();
                    List<Att_Roster> lstRosterByProfileTypeGroup = lstRosterByProfile.Where(m => m.Type == E_ROSTERGROUP).ToList();
                    AnalyseTotalLeaveDaysAndHours(item, leaveDayType, profileInLeave, gradeCfg, lstRosterByProfile, lstRosterGroup, listWorkHistoryByProfile, lstHoliday, shifts);
                }
                #endregion

                //var listWorkDay=repoAtt_Workday.GetAll().ToList();
                //Att_Workday workdayOld = listWorkDay.Where(m => m.ProfileID == WorkDayInDB.ProfileID && m.WorkDate == WorkDayInDB.WorkDate).FirstOrDefault();
                //if (workdayOld != null)
                //{
                //    int index = listWorkDay.IndexOf(workdayOld);
                //    listWorkDay.Remove(workdayOld);
                //    listWorkDay.Insert(index, WorkDayInDB.CopyData<Att_Workday>());
                //}
                repoAtt_LeaveDay.Add(lstLeaveDayInsert);
                unitOfWork.SaveChanges();
                //EntityService.AddEntity<Att_LeaveDay>(GuidContext, lstLeaveDayInsert.ToArray());
                //message = "Susscess";
                return message;
                //return WorkDayInDB;
            }
        }
        public string ValidateLeaveDayTimeOff(List<Guid> lstProfileId, List<Att_LeaveDay> listleaveDayInsert)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoAtt_TimeOffInLieu = new Att_TimeOffInLieuRepository(unitOfWork);
                var repoAtt_Roster = new Att_RosterRepository(unitOfWork);
                var repoAtt_RosterGroup = new Att_RosterGroupRepository(unitOfWork);
                var repoHre_WorkHistory = new Hre_WorkHistoryRepository(unitOfWork);
                var repoCat_DayOff = new Cat_DayOffRepository(unitOfWork);
                var repoAtt_TimeOffInLieuMonth = new CustomBaseRepository<Att_TimeOffInLieuMonth>(unitOfWork);
                var repoCat_GradeAttendance = new CustomBaseRepository<Cat_GradeAttendance>(unitOfWork);
                var repoAtt_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);
                var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var repoCat_Shift = new Cat_ShiftRepository(unitOfWork);
                var shifts = repoCat_Shift.FindBy(s => s.IsDelete == null).ToList();
                string ErrorResult = string.Empty;
                Guid GuidContext = Guid.NewGuid();
                DateTime dateMin = listleaveDayInsert.Min(m => m.DateStart);
                DateTime Datemax = listleaveDayInsert.Min(m => m.DateEnd);
                DateTime BeginMonthOfMin = new DateTime(dateMin.Year, dateMin.Month, 1);
                DateTime EndMonthOfMax = new DateTime(Datemax.Year, Datemax.Month, 1);
                EndMonthOfMax = EndMonthOfMax.AddMonths(1).AddMinutes(-1);
                DateTime Month4Ago = BeginMonthOfMin.AddMonths(-4);
                List<Att_TimeOffInLieu> lstTimeOffInLieu = repoAtt_TimeOffInLieu.FindBy(m => m.Date >= Month4Ago && m.Date < EndMonthOfMax && lstProfileId.Contains(m.ProfileID)).ToList<Att_TimeOffInLieu>();
                List<Att_TimeOffInLieuMonth> lstTimeOffInLieu_Month = repoAtt_TimeOffInLieuMonth.FindBy(m => m.Month >= Month4Ago && m.Month < EndMonthOfMax && lstProfileId.Contains(m.ProfileID)).ToList<Att_TimeOffInLieuMonth>();

                List<Cat_GradeAttendance> lstGradeCfg = repoCat_GradeAttendance.GetAll().ToList<Cat_GradeAttendance>();
                var lstGrade = repoAtt_Grade.FindBy(m => lstProfileId.Contains((Guid)m.ProfileID) && m.MonthStart <= dateMin).Select(m => new { m.ProfileID, m.MonthStart, m.GradeAttendanceID }).OrderByDescending(m => m.MonthStart);
                string E_APPROVED_Roster = RosterStatus.E_APPROVED.ToString();
                List<Att_Roster> lstRoster = repoAtt_Roster.FindBy(m => m.Status == E_APPROVED_Roster && m.DateEnd >= dateMin && m.DateStart <= Datemax && lstProfileId.Contains(m.ProfileID)).ToList<Att_Roster>();
                List<Att_RosterGroup> lstRosterGroup = repoAtt_RosterGroup.FindBy(m => m.DateEnd >= dateMin && m.DateStart <= Datemax).ToList<Att_RosterGroup>();
                string E_ROSTERGROUP = RosterType.E_ROSTERGROUP.ToString();
                List<Att_Roster> lstRosterTypeGroup = lstRoster.Where(m => m.Type == E_ROSTERGROUP).ToList();
                List<Hre_WorkHistory> lstWorkHistory = repoHre_WorkHistory.FindBy(m => lstProfileId.Contains(m.ProfileID)).ToList<Hre_WorkHistory>();
                List<DateTime> lstDayOff = repoCat_DayOff.GetAll().Select(m => m.DateOff).ToList<DateTime>();


                //Tạo ra một list giả dữ liệu
                List<LeaveDayValidate> lstLeaveDayValidate = new List<LeaveDayValidate>();
                List<LeaveDayValidate> lstLeaveDayValidate1 = new List<LeaveDayValidate>();
                foreach (var item in listleaveDayInsert)
                {
                    LeaveDayValidate LeaveDayValid = new LeaveDayValidate();
                    LeaveDayValid.ProfileID = item.ProfileID;
                    LeaveDayValid.DateStart = item.DateStart;
                    LeaveDayValid.DateEnd = item.DateEnd;
                    LeaveDayValid.Duration = item.LeaveHours.Value;
                    LeaveDayValid.TotalDuration = item.LeaveDays ?? 1;
                    lstLeaveDayValidate1.Add(LeaveDayValid);
                }

                foreach (var item in lstLeaveDayValidate1)
                {
                    if (item.DateEnd.Date > item.DateStart.Date)
                    {
                        Dictionary<DateTime, DateTime> dicTime = new Dictionary<DateTime, DateTime>();
                        DateTime DateMinBeginMonth = new DateTime(item.DateStart.Year, item.DateStart.Month, 1);
                        DateTime DateMaxBeginMonth = new DateTime(item.DateEnd.Year, item.DateEnd.Month, 1);
                        if (DateMinBeginMonth < DateMaxBeginMonth)
                        {
                            Guid GradeID = Guid.Empty;
                            var gradeByProfile = lstGrade.Where(m => m.ProfileID == item.ProfileID).FirstOrDefault();
                            if (gradeByProfile != null)
                            {
                                GradeID = (Guid)gradeByProfile.GradeAttendanceID;
                            }
                            Cat_GradeAttendance gradeCfg = lstGradeCfg.Where(m => m.ID == GradeID).FirstOrDefault();
                            if (gradeCfg == null)
                                continue;
                            List<Att_Roster> listRosterByProfile = lstRoster.Where(m => m.ProfileID == item.ProfileID).ToList();
                            List<Hre_WorkHistory> listWorkHistoryByProfile = lstWorkHistory.Where(m => m.ProfileID == item.ProfileID).ToList();
                            List<Att_Roster> lstRosterTypeGroupByProfile = lstRosterTypeGroup.Where(m => m.ProfileID == item.ProfileID).ToList();

                            var listRosterEntity = listRosterByProfile.Select(d => new Att_RosterEntity
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

                            Dictionary<DateTime, Cat_Shift> dailyShifts = Att_AttendanceLib.GetDailyShifts(item.ProfileID,
                                item.DateStart, item.DateEnd, listRosterEntity, listRosterGroupEntity, shifts);

                            for (DateTime DateCheck = DateMinBeginMonth; DateCheck <= DateMaxBeginMonth; DateCheck = DateCheck.AddMonths(1))
                            {
                                DateTime beginMonth = DateCheck;
                                DateTime endMonth = beginMonth.AddMonths(1).AddMinutes(-1);
                                if (DateCheck == DateMinBeginMonth)
                                {
                                    dicTime.Add(item.DateStart, endMonth);
                                }
                                else if (DateCheck == DateMaxBeginMonth)
                                {
                                    dicTime.Add(beginMonth, item.DateEnd);
                                }
                                else
                                {
                                    dicTime.Add(beginMonth, endMonth);
                                }
                            }
                            foreach (var dicTimeKey in dicTime.Keys)
                            {
                                DateTime DateS = dicTimeKey;
                                DateTime DateE = dicTime[dicTimeKey];

                                double TotalDuration = 0;
                                for (DateTime dateC = DateS; dateC <= DateE; dateC = dateC.AddDays(1))
                                {
                                    if (!lstDayOff.Contains(dateC) && dailyShifts.ContainsKey(dateC))
                                    {
                                        if (dailyShifts[dateC] != null)
                                        {
                                            TotalDuration++;
                                        }
                                    }
                                }
                                LeaveDayValidate LeaveDayValid = new LeaveDayValidate();
                                LeaveDayValid.ProfileID = item.ProfileID;
                                LeaveDayValid.DateStart = DateS;
                                LeaveDayValid.DateEnd = DateE;
                                LeaveDayValid.Duration = item.Duration;
                                LeaveDayValid.TotalDuration = TotalDuration;
                                lstLeaveDayValidate1.Add(LeaveDayValid);
                            }
                        }
                        else
                        {
                            lstLeaveDayValidate.Add(item);
                        }
                    }
                    else
                    {
                        lstLeaveDayValidate.Add(item);
                    }
                }

                List<Guid> LstProfileIDs_Error_MissImport = new List<Guid>();
                List<Guid> LstProfileIDs_Error_NotValid = new List<Guid>();

                foreach (var item in lstLeaveDayValidate)
                {
                    DateTime monthYear = new DateTime(item.DateStart.Year, item.DateStart.Month, 1);
                    List<Att_TimeOffInLieu> lstTimeOffInlieu_ByProfile = lstTimeOffInLieu.Where(m => m.ProfileID == item.ProfileID).ToList();
                    List<Att_TimeOffInLieuMonth> lstTimeOffInlieuMonth_ByProfile = lstTimeOffInLieu_Month.Where(m => m.ProfileID == item.ProfileID).ToList();
                    double? NumAvailable = CalculateTotalHourTimeOff(item.ProfileID, lstTimeOffInlieu_ByProfile, lstTimeOffInlieuMonth_ByProfile, monthYear, 1);

                    if (NumAvailable == null)
                    {
                        LstProfileIDs_Error_MissImport.Add(item.ProfileID);
                    }
                    else if ((item.Duration * item.TotalDuration) > NumAvailable.Value)
                    {
                        LstProfileIDs_Error_NotValid.Add(item.ProfileID);
                    }
                }
                if (LstProfileIDs_Error_MissImport.Count > 0)
                {
                    var profile = repoHre_Profile.FindBy(m => LstProfileIDs_Error_MissImport.Contains(m.ID)).Select(m => new { m.CodeEmp, m.ProfileName });

                    foreach (var item in profile)
                    {
                        ErrorResult += item.ProfileName + "[" + item.CodeEmp + "]; ";
                    }
                    if (ErrorResult.Length > 0)
                    {
                        ErrorResult = ErrorResult.Substring(0, ErrorResult.Length - 2);
                    }
                    ErrorResult = ConstantMessages.EmpDoNotConfigTimeOffBegin.TranslateString();
                    //ErrorResult = "EmpDoNotConfigTimeOffBegin";

                }
                else if (LstProfileIDs_Error_NotValid.Count > 0)
                {
                    var profile = repoHre_Profile.FindBy(m => LstProfileIDs_Error_NotValid.Contains(m.ID)).Select(m => new { m.CodeEmp, m.ProfileName });

                    foreach (var item in profile)
                    {
                        ErrorResult += item.ProfileName + "[" + item.CodeEmp + "]; ";
                    }
                    if (ErrorResult.Length > 0)
                    {
                        ErrorResult = ErrorResult.Substring(0, ErrorResult.Length - 2);
                    }
                    ErrorResult = ConstantMessages.DataNotEnoughToMakeLeave.TranslateString();
                    //ErrorResult = "EmpDoNotEnoughTimeOff";
                }
                return ErrorResult;
            }
        }

        //Quy tắc phép bù của honda cập nhật trong file tài liệu 
        /// <summary>
        /// Hàm tính ra số phép bù của nhân viên hiện tại
        /// </summary>
        /// <param name="ProfileID">ID của nhân viên</param>
        /// <param name="lstTimeOff4Ago">Ds trong bảng Att_TimeOffInLieu của nhân viên từ 3 tháng trước cho đến hiện tại</param>
        /// <param name="lstTimeOffByMonth3Ago">Ds trong bảng Att_TimeOffInLieuMonth của nhân viên tính theo 3 tháng trước tháng hiện tại</param>
        /// <param name="monthYear">Tháng Hiện tại</param>
        /// <param name="lstTimeOffByMonthNEW">Ds sẽ phát sinh ra thi tính công thì sẽ lưu Ds này</param>
        /// <param name="isFromValidateLeaveDay">Nếu validate từ ngày nghỉ thì TRUE ngươc lại từ tính công thi FALSE</param>
        /// <param name="dateStartOfMonth">Ngày bắt đầu của Tháng, Honda là ngày 1</param>
        /// <returns></returns>
        public double? CalculateTotalHourTimeOff(Guid ProfileID, List<Att_TimeOffInLieu> lstTimeOff4Ago, List<Att_TimeOffInLieuMonth> lstTimeOffByMonth3Ago, DateTime monthYear, int dateStartOfMonth)
        {
            monthYear = new DateTime(monthYear.Year, monthYear.Month, 1);
            lstTimeOffByMonth3Ago = lstTimeOffByMonth3Ago.Where(m => m.Month != monthYear).ToList();
            if (lstTimeOffByMonth3Ago.Where(m => m.Month != null).ToList().Count == 0)
                return null;
            List<Att_TimeOffInLieuMonth> lstTimeOffByMonth_Auto_NEW = new List<Att_TimeOffInLieuMonth>();
            Att_TimeOffInLieuMonth TimeOffInLieuMonthNew = lstTimeOffByMonth3Ago.OrderByDescending(m => m.Month).FirstOrDefault();
            DateTime monthInLastTimeOff_inMonth = TimeOffInLieuMonthNew.Month ?? monthYear;
            for (DateTime MonthCheck = monthInLastTimeOff_inMonth.AddMonths(1); MonthCheck <= monthYear; MonthCheck = MonthCheck.AddMonths(1))
            {
                DateTime beginMonth = new DateTime(MonthCheck.Year, MonthCheck.Month, dateStartOfMonth);
                DateTime endMonth = beginMonth.AddMonths(1).AddMinutes(-1);
                //DK.m = IF(Giam(m-1)>=DK(m-1), CK(m-1), IF(Giam(m-1) < DK(m-1),Tang(m-1),CK(m-1))	
                DateTime monthTimeOff_Inmonth = MonthCheck.AddMonths(-1);
                Att_TimeOffInLieuMonth TimeOffInLieuLast = lstTimeOffByMonth3Ago.Where(m => m.Month == monthTimeOff_Inmonth).FirstOrDefault();
                if (TimeOffInLieuLast == null)
                {
                    TimeOffInLieuLast = lstTimeOffByMonth_Auto_NEW.Where(m => m.Month == monthTimeOff_Inmonth).FirstOrDefault();
                }
                if (TimeOffInLieuLast == null)
                    return null;

                double Balance = 0;
                if ((TimeOffInLieuLast.TakenLeaves ?? 0) >= (TimeOffInLieuLast.BalanceLeaves ?? 0))
                {
                    Balance = TimeOffInLieuLast.RemainLeaves ?? 0;
                }
                else
                {
                    Balance = TimeOffInLieuLast.UnusualLeaves ?? 0;
                }
                Att_TimeOffInLieuMonth LieuMonth_New = new Att_TimeOffInLieuMonth();
                LieuMonth_New.ProfileID = ProfileID;
                LieuMonth_New.Month = MonthCheck;
                LieuMonth_New.BalanceLeaves = Balance;
                LieuMonth_New.UnusualLeaves = lstTimeOff4Ago.Where(m => m.ProfileID == ProfileID && m.Date >= beginMonth && m.Date < endMonth).Sum(m => m.UnusualLeaves);
                LieuMonth_New.TakenLeaves = lstTimeOff4Ago.Where(m => m.ProfileID == ProfileID && m.Date >= beginMonth && m.Date < endMonth).Sum(m => m.TakenLeaves);
                LieuMonth_New.RemainLeaves = LieuMonth_New.BalanceLeaves + LieuMonth_New.UnusualLeaves - LieuMonth_New.TakenLeaves;
                lstTimeOffByMonth_Auto_NEW.Add(LieuMonth_New);
            }
            //if (!isFromValidateLeaveDay)
            //{
            //    lstTimeOffByMonthNEW = lstTimeOffByMonth_Auto_NEW;
            //}

            if (lstTimeOffByMonth_Auto_NEW.Where(m => m.Month == monthYear).ToList().Count > 0)
            {
                return lstTimeOffByMonth_Auto_NEW.Where(m => m.Month == monthYear).FirstOrDefault().RemainLeaves;
            }

            return null;
        }

        /// <summary>
        /// hàm Validate những nhân viên trong giai đoạn thử việc sẽ không được phép đăng ký một số loại ngày nghỉ
        /// </summary>
        /// <param name="gradeCfg"></param>
        /// <param name="lstLeaveDay"></param>
        /// <returns></returns>
        public string ValidateLeaveTypeByNewEmployee(Sys_AllSetting AppConfig, List<Att_LeaveDay> lstLeaveDay)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repoHre_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var repoCat_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);

                if (AppConfig == null || string.IsNullOrEmpty(AppConfig.Value1))
                    return string.Empty;

                Guid GuidContex = Guid.NewGuid();

                char[] ex = new char[] { ',' };
                List<string> leaveTypeCode = AppConfig.Value1.Split(ex, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                List<Guid> lstLeaveTypeIDs = repoCat_LeaveDayType.FindBy(m => leaveTypeCode.Contains(m.Code)).Select(m => m.ID).ToList<Guid>();

                List<Att_LeaveDay> lstLeaveDayValidate = lstLeaveDay.Where(m => lstLeaveTypeIDs.Contains(m.LeaveDayTypeID)).ToList();
                if (lstLeaveDayValidate.Count == 0)
                    return string.Empty;

                List<Guid> lstProfileID = lstLeaveDayValidate.Select(m => m.ProfileID).Distinct().ToList<Guid>();
                var lstProfile = repoHre_Profile.FindBy(m => lstProfileID.Contains(m.ID)).Select(m => new { m.ID, m.CodeEmp, m.ProfileName, m.DateEndProbation });
                string ErrMess = string.Empty;

                foreach (var item in lstLeaveDayValidate)
                {
                    var profile = lstProfile.Where(m => m.ID == item.ProfileID).FirstOrDefault();
                    DateTime dateStart = item.DateStart;
                    if (profile != null && profile.DateEndProbation != null && profile.DateEndProbation.Value > dateStart.Date)
                    {
                        ErrMess += profile.ProfileName + "[" + profile.CodeEmp + "]; ";
                    }
                }
                if (ErrMess != string.Empty)
                {
                    ErrMess = ErrMess.Substring(0, ErrMess.Length - 2);
                    //ErrMess = string.Format(Messages.EmpCantRegisterLeaveTypeWithProbation.TranslateString(), ErrMess);
                }
                return ErrMess;
            }
        }

        public static void GetRosterGroup(List<Guid> lstProfileID, DateTime? DateFrom, DateTime? DateTo, string userLogin, out List<Att_RosterEntity> lstRosterTypeGroup, out List<Att_RosterGroup> lstRosterGroup)
        {
            var _attRoster = new Att_RosterServices();
            var _attRosterGroup = new Att_RosterGroupServices();
            string status = string.Empty;

            lstRosterGroup = new List<Att_RosterGroup>();
            lstRosterTypeGroup = new List<Att_RosterEntity>();

            string E_APPROVED = RosterStatus.E_APPROVED.ToString();
            string E_ROSTERGROUP = RosterType.E_ROSTERGROUP.ToString();

            var lstobject = new List<object>();
            lstobject.AddRange(new object[10]);
            lstobject[8] = 1;
            lstobject[9] = int.MaxValue;

            var lstobject1 = new List<object>();
            lstobject1.AddRange(new object[6]);
            lstobject1[4] = 1;
            lstobject1[5] = int.MaxValue;

            if (DateFrom == null || DateTo == null)
            {
                lstRosterTypeGroup = _attRoster.GetData<Att_RosterEntity>(lstobject, ConstantSql.hrm_att_sp_get_Roster, userLogin, ref status).Where(m => lstProfileID.Contains(m.ProfileID) && m.Status == E_APPROVED && m.Type == E_ROSTERGROUP).ToList<Att_RosterEntity>();
                lstRosterGroup = _attRosterGroup.GetData<Att_RosterGroupEntity>(lstobject1, ConstantSql.hrm_att_sp_get_RosterGroup,userLogin, ref status).Where(m => m.DateStart != null && m.DateEnd != null).ToList<Att_RosterGroupEntity>().CopyData<List<Att_RosterGroup>>();
            }
            else
            {
                lstRosterTypeGroup = _attRoster.GetData<Att_RosterEntity>(lstobject, ConstantSql.hrm_att_sp_get_Roster, userLogin, ref status).Where(m => lstProfileID.Contains(m.ProfileID) && m.Status == E_APPROVED && m.Type == E_ROSTERGROUP && m.DateStart <= DateTo).ToList<Att_RosterEntity>();
                lstRosterGroup = _attRosterGroup.GetData<Att_RosterGroupEntity>(lstobject1, ConstantSql.hrm_att_sp_get_RosterGroup, userLogin, ref status).Where(m => m.DateStart != null && m.DateEnd != null && m.DateStart <= DateTo && m.DateEnd >= DateFrom).ToList<Att_RosterGroupEntity>().CopyData<List<Att_RosterGroup>>();
            }
        }


        #endregion

        #region Hàm LeaveDayDAO

        public void AnalyzeLeaveLate(List<Att_Workday> lstWorkday, List<Att_Pregnancy> lstPrenancy, List<Cat_LateEarlyRule> lstLateEarlyRule,
            List<Cat_Shift> lstShift, List<Att_Grade> lstGrade, List<Cat_GradeAttendance> lstGradeConfig, List<DateTime> lstDayOff)
        {
            foreach (var Workday in lstWorkday)
            {
                if (Workday.InTime1 == null || Workday.OutTime1 == null)
                    continue;
                Cat_Shift Shift = null;
                Guid? ShiftID = Workday.ShiftApprove ?? Workday.ShiftID;
                Shift = lstShift.Where(m => m.ID == ShiftID).FirstOrDefault();
                if (Shift == null)
                    continue;
                DateTime Date = Workday.WorkDate;
                if (lstDayOff.Any(m => m == Date.Date))
                    continue;

                DateTime? dateStartShift = Date.Date.AddHours(Shift.InTime.Hour).AddMinutes(Shift.InTime.Minute);
                DateTime? dateEndShift = dateStartShift.Value.AddHours(Shift.CoOut);
                string E_LEAVE_EARLY = PregnancyType.E_LEAVE_EARLY.ToString();
                Att_Pregnancy Prenancy = lstPrenancy
                    .Where(m => m.ProfileID == Workday.ProfileID && m.DateStart <= Workday.WorkDate && m.DateEnd > Workday.WorkDate && m.Type == E_LEAVE_EARLY)
                    .FirstOrDefault();

                Att_Grade GradeByProfile = lstGrade
                    .Where(m => m.ProfileID == Workday.ProfileID && m.MonthStart < Workday.WorkDate)
                    .OrderByDescending(m => m.MonthStart)
                    .FirstOrDefault();
                Cat_GradeAttendance GradeCfg = null;
                List<Cat_LateEarlyRule> lstLateEarlyRule_By_Config = new List<Cat_LateEarlyRule>();
                if (GradeByProfile != null)
                {
                    GradeCfg = lstGradeConfig.Where(m => m.ID == GradeByProfile.GradeAttendanceID).FirstOrDefault();
                }
                if (GradeCfg != null)
                {
                    lstLateEarlyRule_By_Config = lstLateEarlyRule.Where(m => m.GradeCfgID == GradeCfg.ID).ToList();
                }
                if (GradeCfg == null)
                    continue;
                #region tính có trễ sớm cho trường hơp đầu ca cuối ca
                //2. Kiểm tra đủ đk để không bị trễ sớm
                bool BeLate = true;
                bool BeEarly = true;
                double HourLate = 0;
                double HourEarly = 0;
                #region cặp thứ 1
                if (Workday.InTime1 != null && Workday.OutTime1 != null)
                {
                    if (!IsHaveLateEarly(dateStartShift.Value, dateEndShift.Value, Workday.InTime1.Value, Workday.OutTime1.Value, ref BeLate, ref BeEarly, ref HourLate, ref HourEarly, new Dictionary<DateTime, DateTime>(), Shift))
                    {
                        setNonLateEarly(Workday);
                        continue;
                    }
                }
                #endregion

                if ((Workday.Shift2ID != null || Workday.ShiftApprove2 != null) && Workday.InTime2 != null && Workday.OutTime2 != null)
                {
                    int MinuteLateShift2 = 0;
                    int MinuteEarlyShift2 = 0;
                    AnalyzeLeaveLateSecondShift(Workday, lstPrenancy, lstLateEarlyRule, lstShift, lstGrade, lstGradeConfig, lstDayOff, out MinuteLateShift2, out MinuteEarlyShift2);
                    HourLate += (MinuteLateShift2 / 60);
                    HourEarly += (MinuteEarlyShift2 / 60);
                }



                double Late = HourLate * 60; //Phut neen nhaan cho 60
                double EarLy = HourEarly * 60; //Phut neen nhaan cho 60
                if (Late > 480)
                {
                    Late = 480;
                    EarLy = 0;
                }
                if ((Late - (int)Late) > 0)
                {
                    Late = (int)Late + 1;
                }
                else
                {
                    Late = (int)Late;
                }

                if ((EarLy - (int)EarLy) > 0)
                {
                    EarLy = (int)EarLy + 1;
                }
                else
                {
                    EarLy = (int)EarLy;
                }

                Workday.LateDuration1 = (int)(Late);
                Workday.EarlyDuration1 = (int)(EarLy);
                bool isCheck = false;
                if (GradeCfg.AttendanceMethod == AttendanceMethod.E_TAM.ToString())
                    isCheck = true;
                bool isUseLateEarlyRule = false;
                if (GradeCfg.AttendanceMethod == AttendanceMethod.E_TAM.ToString() && GradeCfg.IsDeductInLateOutEarly == true && GradeCfg.IsLateEarlyRounding == true)
                    isUseLateEarlyRule = true;
                RoundLateEarly(Prenancy, lstLateEarlyRule_By_Config, ref HourLate, ref HourEarly, isCheck, isUseLateEarlyRule);

                double LateRound = HourLate * 60;
                double EarLyRound = HourEarly * 60;
                if (LateRound > 480)
                {
                    LateRound = 480;
                    EarLyRound = 0;
                }
                if ((LateRound - (int)LateRound) > 0)
                {
                    LateRound = (int)LateRound + 1;
                }
                else
                {
                    LateRound = (int)LateRound;
                }
                if ((EarLyRound - (int)EarLyRound) > 0)
                {
                    EarLyRound = (int)EarLyRound + 1;
                }
                else
                {
                    EarLyRound = (int)EarLyRound;
                }

                Workday.LateDuration4 = (int)LateRound;
                Workday.EarlyDuration4 = (int)EarLyRound;
                Workday.LateEarlyDuration = (int)(LateRound + EarLyRound);
                Workday.LateEarlyRoot = LateRound + EarLyRound;
                if (Workday.LateEarlyRoot != null && Workday.LateEarlyRoot > 0)
                {
                    Workday.Type = WorkdayType.E_LATE_EARLY.ToString();
                }
                #endregion
            }
        }


        public void AnalyzeLeaveLateSecondShift(Att_Workday Workday, List<Att_Pregnancy> lstPrenancy, List<Cat_LateEarlyRule> lstLateEarlyRule,
           List<Cat_Shift> lstShift, List<Att_Grade> lstGrade, List<Cat_GradeAttendance> lstGradeConfig, List<DateTime> lstDayOff, out int LateRound,out int EarLyRound)
        {
            LateRound = 0;
            EarLyRound = 0;                                                                                                        
                if (Workday.InTime2 == null || Workday.OutTime2 == null)
                    return;
                Cat_Shift Shift = null;
                Guid? ShiftID = Workday.ShiftApprove2 ?? Workday.Shift2ID;
                Shift = lstShift.Where(m => m.ID == ShiftID).FirstOrDefault();
                if (Shift == null)
                    return;
                DateTime Date = Workday.WorkDate;
                if (lstDayOff.Any(m => m == Date.Date))
                    return;

                DateTime? dateStartShift = Date.Date.AddHours(Shift.InTime.Hour).AddMinutes(Shift.InTime.Minute);
                DateTime? dateEndShift = dateStartShift.Value.AddHours(Shift.CoOut);
                string E_LEAVE_EARLY = PregnancyType.E_LEAVE_EARLY.ToString();
                Att_Pregnancy Prenancy = lstPrenancy
                    .Where(m => m.ProfileID == Workday.ProfileID && m.DateStart <= Workday.WorkDate && m.DateEnd > Workday.WorkDate && m.Type == E_LEAVE_EARLY)
                    .FirstOrDefault();

                Att_Grade GradeByProfile = lstGrade
                    .Where(m => m.ProfileID == Workday.ProfileID && m.MonthStart < Workday.WorkDate)
                    .OrderByDescending(m => m.MonthStart)
                    .FirstOrDefault();
                Cat_GradeAttendance GradeCfg = null;
                List<Cat_LateEarlyRule> lstLateEarlyRule_By_Config = new List<Cat_LateEarlyRule>();
                if (GradeByProfile != null)
                {
                    GradeCfg = lstGradeConfig.Where(m => m.ID == GradeByProfile.GradeAttendanceID).FirstOrDefault();
                }
                if (GradeCfg != null)
                {
                    lstLateEarlyRule_By_Config = lstLateEarlyRule.Where(m => m.GradeCfgID == GradeCfg.ID).ToList();
                }
                if (GradeCfg == null)
                    return;
                #region tính có trễ sớm cho trường hơp đầu ca cuối ca
                //2. Kiểm tra đủ đk để không bị trễ sớm
                bool BeLate = true;
                bool BeEarly = true;
                double HourLate = 0;
                double HourEarly = 0;
                #region cặp thứ 2
                if (Workday.InTime2 != null && Workday.OutTime2 != null)
                {
                    if (!IsHaveLateEarly(dateStartShift.Value, dateEndShift.Value, Workday.InTime2.Value, Workday.OutTime2.Value, ref BeLate, ref BeEarly, ref HourLate, ref HourEarly, new Dictionary<DateTime, DateTime>(), Shift))
                    {
                    }
                }
                #endregion
                double Late = HourLate * 60; //Phut neen nhaan cho 60
                double EarLy = HourEarly * 60; //Phut neen nhaan cho 60
                if (Late > 480)
                {
                    Late = 480;
                    EarLy = 0;
                }
                if ((Late - (int)Late) > 0)
                {
                    Late = (int)Late + 1;
                }
                else
                {
                    Late = (int)Late;
                }

                if ((EarLy - (int)EarLy) > 0)
                {
                    EarLy = (int)EarLy + 1;
                }
                else
                {
                    EarLy = (int)EarLy;
                }

               
                bool isCheck = false;
                if (GradeCfg.AttendanceMethod == AttendanceMethod.E_TAM.ToString())
                    isCheck = true;
                bool isUseLateEarlyRule = false;
                if (GradeCfg.AttendanceMethod == AttendanceMethod.E_TAM.ToString() && GradeCfg.IsDeductInLateOutEarly == true && GradeCfg.IsLateEarlyRounding == true)
                    isUseLateEarlyRule = true;
                RoundLateEarly(Prenancy, lstLateEarlyRule_By_Config, ref HourLate, ref HourEarly, isCheck, isUseLateEarlyRule);

                LateRound =(int) (HourLate * 60);
                EarLyRound =(int) (HourEarly * 60);
                if (LateRound > 480)
                {
                    LateRound = 480;
                    EarLyRound = 0;
                }
                if ((LateRound - (int)LateRound) > 0)
                {
                    LateRound = (int)LateRound + 1;
                }
                else
                {
                    LateRound = (int)LateRound;
                }
                if ((EarLyRound - (int)EarLyRound) > 0)
                {
                    EarLyRound = (int)EarLyRound + 1;
                }
                else
                {
                    EarLyRound = (int)EarLyRound;
                }
                #endregion
        }


        #region Phân tích tổng số ngày nghỉ và Tổng số Giờ nghỉ
        public void AnalyseTotalLeaveDaysAndHours(Att_LeaveDay LeaveDay, Cat_LeaveDayType LeaveDayType, Hre_Profile profile, Cat_GradeAttendance gradeCfg, List<Att_Roster> lstRoster, List<Att_RosterGroup> lstRosterGroup, List<Hre_WorkHistory> listWorkHistory, List<Cat_DayOff> lstHoliday, List<Cat_Shift> lstShift)
        {
            if (LeaveDay.DurationType == null)
                return;
            #region data
            string LeaveDayTypeCode = string.Empty;
            if (LeaveDayType != null)
                LeaveDayTypeCode = LeaveDayType.Code;

            if (gradeCfg == null)
                return;
            DateTime dateFrom = LeaveDay.DateStart.Date;
            DateTime dateTo = LeaveDay.DateEnd;
            dateTo = dateTo.AddDays(1).AddMinutes(-1);
            string E_ROSTERGROUP = RosterType.E_ROSTERGROUP.ToString();
            List<Att_Roster> lstRosterByProfile = lstRoster.Where(m => m.ProfileID == profile.ID && m.DateStart <= dateTo && m.DateEnd >= dateFrom).ToList();
            List<Att_Roster> lstRosterByProfileTypeGroup = lstRosterByProfile.Where(m => m.Type == E_ROSTERGROUP).ToList();
            List<Hre_WorkHistory> listWorkHistoryByProfile = listWorkHistory.Where(m => m.ProfileID == profile.ID && m.DateEffective < dateTo).ToList();

            var listRosterEntity = lstRosterByProfile.Select(d => new Att_RosterEntity
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

            Dictionary<DateTime, Cat_Shift> listMonthShifts = Att_AttendanceLib.GetDailyShifts(profile.ID,
                dateFrom, dateTo, listRosterEntity, listRosterGroupEntity, lstShift);

            double leaveDays = 0;
            double leaveHours = 0;

            #endregion
            if (LeaveDay.DurationType != LeaveDayDurationType.E_FULLSHIFT.ToString())
            {
                DateTime dateLeave = LeaveDay.DateStart.Date;
                //Check co ca ko
                if (gradeCfg != null && Att_WorkDayHelper.IsWorkDay(dateLeave, gradeCfg, listMonthShifts, lstHoliday))
                {
                    if (listMonthShifts.ContainsKey(dateLeave) && listMonthShifts[dateLeave] != null && listMonthShifts[dateLeave].WorkHours != null && listMonthShifts[dateLeave].WorkHours != 0)
                    {
                        Cat_Shift Shift = listMonthShifts[dateLeave];
                        //Có ca thi lây giờ nghỉ giua ca để mà tính thời gian thực nghỉ
                        //Nếu như trừ thời gian nghỉ ra khỏi giờ nghi trưa mà lơn hơn workhour của ca thì lấy max la workhour của ca
                        DateTime ShiftBreakIn = dateLeave.Add(Shift.InTime.TimeOfDay).AddHours(Shift.CoBreakIn);
                        DateTime ShiftBreakOut = dateLeave.Add(Shift.InTime.TimeOfDay).AddHours(Shift.CoBreakOut);
                        if (LeaveDay.DateStart < ShiftBreakOut && LeaveDay.DateEnd > ShiftBreakIn)
                        {
                            //neu co long nhau
                            if (ShiftBreakIn > LeaveDay.DateStart)
                                leaveHours = (ShiftBreakIn - LeaveDay.DateStart).TotalHours;
                            if (LeaveDay.DateEnd > ShiftBreakOut)
                                leaveHours += (LeaveDay.DateEnd - ShiftBreakOut).TotalHours;

                            leaveDays = leaveHours / Shift.WorkHours ?? 8;
                        }
                        else
                        {
                            leaveHours = (LeaveDay.DateEnd - LeaveDay.DateStart).TotalHours;
                            leaveDays = leaveHours / Shift.WorkHours ?? 8;
                            //neu ko long nhau
                        }
                    }
                    else //Ko có ca làm việc
                    {
                        //Nếu ko có ca thì lấy giờ out trừ giờ in
                        leaveHours = (LeaveDay.DateEnd - LeaveDay.DateStart).TotalHours;
                        leaveDays = leaveHours / 8;
                    }
                }



            }
            else //Loại FullShift
            {
                if (LeaveDay.DurationType == null)
                {
                    LeaveDay.DurationType = LeaveDayDurationType.E_FULLSHIFT.ToString();
                }
                if (profile == null)
                    return;


                bool isSetFullLeaveDay = false;

                if (!string.IsNullOrEmpty(LeaveDayTypeCode) && (LeaveDayTypeCode == "SICK"
                                    || LeaveDayTypeCode == "PRG"
                                    || LeaveDayTypeCode == "SU"
                                    || LeaveDayTypeCode == "SD"
                                    || LeaveDayTypeCode == "D"
                                    || LeaveDayTypeCode == "DP"
                                    || LeaveDayTypeCode == "PSN"
                                    || LeaveDayTypeCode == "M"
                                    || LeaveDayTypeCode == "DSP"))
                {


                    for (DateTime idx = dateFrom; idx <= dateTo; idx = idx.AddDays(1))
                    {
                        if (!lstHoliday.Any(m => m.DateOff == idx))
                        {
                            leaveDays += 1;
                            Cat_Shift ShiftByDay = null;
                            if (listMonthShifts.ContainsKey(idx))
                            {
                                ShiftByDay = listMonthShifts[idx];
                            }
                            if (ShiftByDay != null)
                            {
                                leaveHours += ShiftByDay.WorkHours ?? 8;
                            }
                            else
                            {
                                leaveHours += 8;
                            }
                        }
                        isSetFullLeaveDay = true;
                    }
                }
                if (isSetFullLeaveDay == false)
                {

                    for (DateTime idx = dateFrom; idx <= dateTo; idx = idx.AddDays(1))
                    {
                        if (gradeCfg != null && Att_WorkDayHelper.IsWorkDay(idx, gradeCfg, listMonthShifts, lstHoliday))
                        {
                            leaveDays += 1;
                            Cat_Shift ShiftByDay = null;
                            if (listMonthShifts.ContainsKey(idx))
                            {
                                ShiftByDay = listMonthShifts[idx];
                            }
                            if (ShiftByDay != null)
                            {
                                leaveHours += ShiftByDay.WorkHours ?? 8;
                            }
                        }
                    }
                }
            }
            if (LeaveDay.LeaveDays == null || LeaveDay.LeaveDays != leaveDays)
            {
                LeaveDay.LeaveDays = leaveDays;
            }
            if (LeaveDay.LeaveHours == null || LeaveDay.LeaveHours != leaveHours)
            {
                LeaveDay.LeaveHours = leaveHours;
            }

        }
        #endregion
        private bool IsHaveLateEarly(DateTime ShiftIn, DateTime ShiftOut, DateTime InTime, DateTime OutTime, ref bool BeLate, ref bool BeEarly, ref double HourLate, ref double HourEarly, Dictionary<DateTime, DateTime> TimeLeave, Cat_Shift Shift)
        {

            #region Xử lý vấn đề Ca có giờ linh động cho VE VietNam Esports (Thái phụ trách) - Tính giờ làm việc linh động theo Ca Làm Việc
            if (Shift != null && Shift.InOutDynamic != null && Shift.InOutDynamic > 0)
            {
                if (InTime > ShiftIn)
                {
                    double HourDynamicLate = (InTime - ShiftIn).TotalHours;
                    HourDynamicLate = HourDynamicLate > Shift.InOutDynamic.Value ? Shift.InOutDynamic.Value : HourDynamicLate;
                    InTime = InTime.AddHours(HourDynamicLate);
                    OutTime = OutTime.AddHours(HourDynamicLate);
                }
            }
            #endregion

            if (InTime <= ShiftIn)
            {
                BeLate = false;
                HourLate = 0;
            }
            else
            {
                double Late = (InTime - ShiftIn).TotalHours;
                foreach (var item in TimeLeave.Keys)
                {
                    DateTime Start = item;
                    DateTime End = TimeLeave[item];
                    if (InTime >= Start && ShiftIn >= End)//neu co giao nhau
                    {
                        //Tinh thoi gian giao nhau
                        if (ShiftIn > Start)
                        {
                            Start = ShiftIn;
                        }

                        if (InTime < End)
                        {
                            End = InTime;
                        }

                        Late -= (End - Start).TotalHours;
                    }
                }
                if (HourLate == 0 || Late < HourLate)
                {
                    HourLate = Late;
                }
            }
            if (OutTime >= ShiftOut)
            {
                BeEarly = false;
                HourEarly = 0;
            }
            else
            {
                double Early = (ShiftOut - OutTime).TotalHours;
                foreach (var item in TimeLeave.Keys)
                {
                    DateTime Start = item;
                    DateTime End = TimeLeave[item];
                    if (OutTime <= Start && ShiftOut >= Start)//neu co giao nhau
                    {
                        //Tinh thoi gian giao nhau
                        if (ShiftOut < End)
                        {
                            End = ShiftOut;
                        }

                        if (Start < OutTime)
                        {
                            Start = OutTime;
                        }

                        Early -= (End - Start).TotalHours;
                    }
                }
                if (HourEarly == 0 || Early < HourEarly)
                {
                    HourEarly = Early;
                }
            }
            if (BeLate == false && BeEarly == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        private void setNonLateEarly(Att_Workday workday)
        {
            workday.LateDuration1 = 0;
            workday.LateDuration2 = 0;
            workday.LateDuration3 = 0;
            workday.LateDuration4 = 0;
            workday.EarlyDuration1 = 0;
            workday.EarlyDuration2 = 0;
            workday.EarlyDuration3 = 0;
            workday.EarlyDuration4 = 0;
            workday.LateEarlyDuration = 0;
            workday.LateEarlyRoot = 0;
        }

        /// <summary>
        /// Hàm làm tròn trễ sớm
        /// </summary>
        /// <param name="Pregnacy">Thai phụ  thuộc dạng trễ sớm</param>
        /// <param name="lstLateEarlyRule">Ds Qui tắc làm tròn đã được order by cột ORDER</param>
        /// <param name="HourLate"></param>
        /// <param name="Early"></param>
        private void RoundLateEarly(Att_Pregnancy Pregnacy, List<Cat_LateEarlyRule> lstLateEarlyRule, ref double HourLate, ref double HourEarly, bool isCheck, bool isUseLateEarlyRule)
        {
            if (isCheck == false)
            {
                HourLate = 0;
                HourEarly = 0;
                return;
            }
            if (Pregnacy != null)
            {
                if (Pregnacy.TypePregnancyEarly == PregnancyLeaveEarlyType.E_FIRST.ToString())
                {
                    HourLate = HourLate - 1;
                    if (HourLate < 0)
                        HourLate = 0;
                }
                else if (Pregnacy.TypePregnancyEarly == PregnancyLeaveEarlyType.E_LAST.ToString())
                {
                    HourEarly = HourEarly - 1;
                    if (HourEarly < 0)
                        HourEarly = 0;
                }
            }

            if (isUseLateEarlyRule)
            {
                if (HourLate > 0 && lstLateEarlyRule != null && lstLateEarlyRule.Count > 0)
                {
                    double LateMinute = HourLate * 60;
                    var LateEarlyRule = lstLateEarlyRule.Where(m => m.MinValue <= LateMinute && m.MaxValue > LateMinute).FirstOrDefault();
                    if (LateEarlyRule != null)
                    {
                        HourLate = LateEarlyRule.RoundValue / 60;
                    }

                }
                if (HourEarly > 0 && lstLateEarlyRule != null && lstLateEarlyRule.Count > 0)
                {
                    double EarlyMinute = HourEarly * 60;
                    var LateEarlyRule = lstLateEarlyRule.Where(m => m.MinValue <= EarlyMinute && m.MaxValue > EarlyMinute).FirstOrDefault();
                    if (LateEarlyRule != null)
                    {
                        HourEarly = LateEarlyRule.RoundValue / 60;
                    }
                }
            }

        }


        public static double getAccumulateLeaves(Guid profileId)
        {

            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));

                var repoAtt_TimeofInLieu = new CustomBaseRepository<Att_TimeOffInLieu>(unitOfWork);
                List<Att_TimeOffInLieu> listTimeOnInLieu = repoAtt_TimeofInLieu.FindBy(toi => toi.AccumulateLeaves != null && toi.ProfileID == profileId).OrderByDescending(toi => toi.DateCreate).ToList();

                if (listTimeOnInLieu.Count == 0)
                    return 0;
                double? accumulateLeaves = listTimeOnInLieu[0].RemainLeaves;
                if (accumulateLeaves == null)
                    return 0;
                return accumulateLeaves.Value;
            }
        }
        #endregion

        #region Validate LeaveDay Register

        public string ValidateOverDayInMonth(Att_LeaveDayEntity LeaveDay, List<Guid> lstProfileID)
        {
            string Error = string.Empty;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo_LeaveDayType = new CustomBaseRepository<Cat_LeaveDayType>(unitOfWork);
                var repo_Grade = new CustomBaseRepository<Att_Grade>(unitOfWork);
                var repo_Profile = new CustomBaseRepository<Hre_Profile>(unitOfWork);
                var repo_Roster = new CustomBaseRepository<Att_Roster>(unitOfWork);
                var repo_RosterGroup = new CustomBaseRepository<Att_RosterGroup>(unitOfWork);
                var repo_GradeAttendance = new CustomBaseRepository<Cat_GradeAttendance>(unitOfWork);
                var repo_LeaveDay = new CustomBaseRepository<Att_LeaveDay>(unitOfWork);
                var LeaveDayType = repo_LeaveDayType
                    .FindBy(m => m.IsDelete == null && (m.ID == LeaveDay.LeaveDayTypeID))
                    .FirstOrDefault();
                if (LeaveDayType == null || LeaveDayType.MaxPerMonth == null || LeaveDayType.MaxPerMonth <= 0)
                {
                    return string.Empty;
                }

                DateTime DateEnd = LeaveDay.DateEnd;
                var lstGrade = repo_Grade.FindBy(m => m.IsDelete == null && m.MonthStart != null && m.MonthStart <= DateEnd && m.GradeAttendanceID != null && m.ProfileID != null && lstProfileID.Contains(m.ProfileID.Value))
                    .Select(m => new { m.ProfileID, m.GradeAttendanceID, m.MonthStart })
                    .ToList();

                List<Guid> lstGradeAttendanceID = lstGrade.Select(m => m.GradeAttendanceID.Value).Distinct().ToList();
                var lstGradeAttendance = repo_GradeAttendance.FindBy(m => lstGradeAttendanceID.Contains(m.ID)).Select(m => new { m.ID, m.SalaryLeaveTimeDay, m.SalaryLeaveTimeType }).ToList();
                if (!lstGradeAttendance.Any(m => m.SalaryLeaveTimeDay != null && m.SalaryLeaveTimeType != null))
                {
                    return string.Empty;
                }


                var lstProfile = repo_Profile.FindBy(m => m.IsDelete == null && lstProfileID.Contains(m.ID))
                    .Select(m => new { m.ID, m.CodeEmp, m.ProfileName })
                    .ToList();
                string E_APPROVED_Roster = RosterStatus.E_APPROVED.ToString();
                string E_CANCEL_LeaveDay = LeaveDayStatus.E_CANCEL.ToString();
                string E_REJECTED_LeaveDay = LeaveDayStatus.E_REJECTED.ToString();
                DateTime DateStart = LeaveDay.DateStart;
                DateTime DateStartBef1Month = DateStart.AddMonths(-1);
                DateTime DateEndAf1Month = DateEnd.AddMonths(1);
                Guid LeaveTypeID = LeaveDay.LeaveDayTypeID;
                var lstLeavedayInDb = repo_LeaveDay
                    .FindBy(m => m.Status != E_CANCEL_LeaveDay && m.Status != E_REJECTED_LeaveDay && m.DateStart <= DateEndAf1Month && m.DateEnd >= DateStartBef1Month && m.LeaveDayTypeID == LeaveTypeID && lstProfileID.Contains(m.ID))
                    .Select(m => new { m.ID, m.ProfileID, m.DateStart, m.DateEnd, m.DurationType, m.LeaveDays })
                    .ToList();
                var lstRoster = repo_Roster.FindBy(m => m.IsDelete == null && m.Status == E_APPROVED_Roster && m.DateStart <= DateEndAf1Month && m.DateEnd >= DateStartBef1Month && lstProfileID.Contains(m.ID))
                    .Select(m => m.CopyData<Att_RosterEntity>()).ToList();
                var lstRosterGroup = repo_RosterGroup.FindBy(m => m.IsDelete == null && m.Status == E_APPROVED_Roster && m.DateStart <= DateEndAf1Month && m.DateEnd >= DateStartBef1Month && lstProfileID.Contains(m.ID))
                    .Select(m => m.CopyData<Att_RosterGroupEntity>())
                    .ToList();

                string E_ROSTERGROUP = RosterType.E_ROSTERGROUP.ToString();


                foreach (var Profile in lstProfile)
                {
                    var GradeNewest = lstGrade.Where(m => m.ProfileID == Profile.ID).Select(m => m.GradeAttendanceID).FirstOrDefault();
                    if (GradeNewest != null)
                    {
                        var GradeConfig = lstGradeAttendance.Where(m => m.ID == GradeNewest).FirstOrDefault();
                        if (GradeConfig == null || GradeConfig.SalaryLeaveTimeDay == null || GradeConfig.SalaryLeaveTimeType == null)
                            continue;

                        DateTime DateBeginAtt = LeaveDay.DateStart;
                        DateTime DateEndAtt = LeaveDay.DateEnd;

                        if (GradeConfig.SalaryLeaveTimeType == SalaryTimeTypeClose.E_CURRENTMONTH.ToString())
                        {
                            DateTime DateBeginConfig = new DateTime(DateBeginAtt.Year, DateBeginAtt.Month, GradeConfig.SalaryLeaveTimeDay.Value);
                            if (DateBeginConfig > DateBeginAtt)
                            {
                                DateBeginConfig = DateBeginConfig.AddMonths(-1);
                                DateBeginAtt = DateBeginConfig;
                            }
                            DateTime DateEndConfig = new DateTime(DateEndAtt.Year, DateEndAtt.Month, GradeConfig.SalaryLeaveTimeDay.Value).AddDays(-1).AddMonths(1);
                            if (DateEndConfig < DateEndAtt)
                            {
                                DateEndConfig = DateEndConfig.AddMonths(1);
                                DateEndAtt = DateEndConfig;
                            }
                        }
                        else
                        {
                            DateTime DateBeginConfig = new DateTime(DateBeginAtt.Year, DateBeginAtt.Month, GradeConfig.SalaryLeaveTimeDay.Value).AddMonths(-1);
                            if (DateBeginConfig > DateBeginAtt)
                            {
                                DateBeginConfig = DateBeginConfig.AddMonths(-1);
                                DateBeginAtt = DateBeginConfig;
                            }
                            DateTime DateEndConfig = new DateTime(DateEndAtt.Year, DateEndAtt.Month, GradeConfig.SalaryLeaveTimeDay.Value).AddDays(-1);
                            if (DateEndConfig < DateEndAtt)
                            {
                                DateEndConfig = DateEndConfig.AddMonths(1);
                                DateEndAtt = DateEndConfig;
                            }
                        }

                        var DaiLyShift = Att_AttendanceLib.GetDailyShifts(Profile.ID, DateBeginAtt, DateEndAtt, lstRoster.Where(m => m.ProfileID == Profile.ID).ToList(), lstRosterGroup);

                        double NumDay = 0;
                        DateTime EndOneMonth = DateBeginAtt.AddMonths(1).AddDays(-1);
                        var lstLeaveDayByProfile = lstLeavedayInDb.Where(m => m.ProfileID == Profile.ID).ToList();
                        for (DateTime DateCheck = DateBeginAtt; DateCheck <= DateEndAtt; DateCheck = DateCheck.AddDays(1))
                        {
                            if (DateCheck > EndOneMonth)
                            {
                                NumDay = 0;
                                EndOneMonth = DateCheck.AddMonths(1).AddDays(-1);
                            }

                            var leaveInDb = lstLeaveDayByProfile.Where(m => m.DateStart <= DateCheck && m.DateEnd >= DateCheck).FirstOrDefault();
                            if (leaveInDb != null || (LeaveDay.DateStart <= DateCheck && LeaveDay.DateEnd >= DateCheck))
                            {
                                if (DaiLyShift.ContainsKey(DateCheck) && DaiLyShift[DateCheck] != null)
                                {
                                    if (leaveInDb != null)
                                    {
                                        if (leaveInDb.DurationType == LeaveDayDurationType.E_FULLSHIFT.ToString())
                                        {
                                            NumDay++;
                                        }
                                        else
                                        {
                                            NumDay += leaveInDb.LeaveDays ?? 0;
                                        }
                                    }
                                    else
                                    {
                                        if (LeaveDay.DurationType == LeaveDayDurationType.E_FULLSHIFT.ToString())
                                        {
                                            NumDay++;
                                        }
                                        else
                                        {
                                            NumDay += LeaveDay.LeaveDays ?? 0;
                                        }
                                    }


                                }
                            }
                            if (NumDay > LeaveDayType.MaxPerMonth)
                            {
                                Error += Profile.ProfileName + " [" + Profile.CodeEmp + "], ";
                            }
                        }



                    }
                }
                if (Error != string.Empty)
                {
                    Error = Error.Substring(0, Error.Length - 2);
                    Error += " " + ConstantMessages.OverMaxNumdayLeaveInMonth;
                }

            }
            return Error;
        }
        #endregion

        #region import Leaveday
        public void CheckData(Guid LoginUserID, List<ImportLeavedayModel> listLeave,
             out List<ImportLeavedayModel> listLeaveCorrect, out List<ImportLeavedayModel> ListLeaveError)
        {
            int pageSize = 200;
            int skipRows = 0;

            listLeaveCorrect = new List<ImportLeavedayModel>();
            ListLeaveError = new List<ImportLeavedayModel>();
            var lstProfileIDs = listLeave.Select(m => m.ProfileID).Distinct().ToList();

            var listShiftInfo = new List<Cat_Shift>();
            var listLeaveTypeInfo = new List<Cat_LeaveDayType>();
            bool allowApproveLeave = false;
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                listShiftInfo = unitOfWork.CreateQueryable<Cat_Shift>().ToList();
                listLeaveTypeInfo = unitOfWork.CreateQueryable<Cat_LeaveDayType>().ToList();
                var key = ApproveType.E_LEAVE_DAY.ToString();
                allowApproveLeave = unitOfWork.CreateQueryable<Sys_UserApprove>(d => d.Type == key && d.UserApproveID == LoginUserID).Any();
            }
            while (skipRows < lstProfileIDs.Count())
            {
                //Vinhtran - 20140718: Chia theo pagesize để không bị quá tải ram
                var listProfileIDSplit = lstProfileIDs.Skip(skipRows).Take(pageSize).ToList();
                var listLeaveSplit = listLeave.Where(d => listProfileIDSplit.Contains(d.ProfileID)).ToList();
                CheckData(listLeaveSplit, allowApproveLeave, listProfileIDSplit, listLeaveTypeInfo, listShiftInfo, ref listLeaveCorrect, ref ListLeaveError);
                skipRows += pageSize;
            }
        }

        private void CheckData(List<ImportLeavedayModel> listLeaveDay, bool allowApproveLeave,
            List<Guid> lstProfileIDs, List<Cat_LeaveDayType> listLeaveTypeInfo, List<Cat_Shift> listShiftInfo, ref List<ImportLeavedayModel> listLeaveCorrect, ref List<ImportLeavedayModel> ListLeaveError)
        {
            #region Kiểm tra dữ liệu lỗi
            var listProfileInfo = new List<Hre_Profile>().Select(m => new { m.ID, m.CodeEmp });
            var listOldLeaveDay = new List<Att_LeaveDay>().Select(d => new { d.ID, d.ProfileID, d.DateStart, d.DateEnd, d.DurationType, d.LeaveDayTypeID, d.Status });
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                listProfileInfo = unitOfWork.CreateQueryable<Hre_Profile>(d => lstProfileIDs.Contains(d.ID)).Select(d => new { d.ID, d.CodeEmp }).ToList();

                var dateStart = listLeaveDay.Select(d => d.DateStart).OrderBy(d => d).FirstOrDefault();
                var dateEnd = listLeaveDay.Select(d => d.DateEnd).OrderBy(d => d).LastOrDefault();
                var listProfileID = listProfileInfo.Select(d => d.ID).Distinct().ToList();

                string cancelStatus = LeaveDayStatus.E_CANCEL.ToString();
                string rejectStatus = LeaveDayStatus.E_REJECTED.ToString();

                listOldLeaveDay = unitOfWork.CreateQueryable<Att_LeaveDay>(d => d.Status != cancelStatus && d.Status != rejectStatus && listProfileID.Contains(d.ProfileID)
                   && d.DateStart <= dateEnd && d.DateEnd >= dateStart).Select(d => new
                   {
                       d.ID,
                       d.ProfileID,
                       d.DateStart,
                       d.DateEnd,
                       d.DurationType,
                       d.LeaveDayTypeID,
                       d.Status
                   }).ToList();
            }






            foreach (var leaveDay in listLeaveDay)
            {
                var profileInfo = listProfileInfo.Where(d => d.CodeEmp != null
                    && d.CodeEmp == leaveDay.CodeEmp).FirstOrDefault();

                if (profileInfo != null)
                {
                    leaveDay.ProfileID = profileInfo.ID;
                    string shiftNotFound = string.Empty;
                    string leaveTypeNotFound = string.Empty;
                    var listShiftNotFound = new List<string>();
                    var listTypeNotFound = new List<string>();

                    if (!string.IsNullOrWhiteSpace(leaveDay.LeaveDayType))
                    {
                        var leaveDayTypeSplits = leaveDay.LeaveDayType.Split('/');
                        var leaveType = leaveDayTypeSplits.FirstOrDefault();
                        var shiftCode = string.Empty;
                        var shiftType = string.Empty;

                        if (leaveDayTypeSplits.Count() == 3)
                        {
                            //Có chỉ định ca nghỉ nửa buổi
                            shiftCode = leaveDayTypeSplits[1];
                            shiftType = leaveDayTypeSplits[2];
                        }
                        else if (leaveDayTypeSplits.Count() == 2)
                        {
                            //buổi đầu hay sau - ca 8 tiếng
                            shiftType = leaveDayTypeSplits[1];
                        }

                        var leaveTypeInfo = listLeaveTypeInfo.Where(d =>
                            d.LeaveDayTypeName == leaveType).FirstOrDefault();

                        if (leaveTypeInfo != null)
                        {
                            double halfShiftHours = 4;

                            if (!string.IsNullOrWhiteSpace(shiftCode))
                            {
                                var shiftInfo = listShiftInfo.Where(d => d.Code == shiftCode).FirstOrDefault();
                                var workHours = shiftInfo != null ? (shiftInfo.WorkHours ?? 0) : 0;
                                halfShiftHours = workHours > 0 ? workHours / 2 : halfShiftHours;
                            }

                            //Nếu DateEnd và DateStart cùng ngày thì TotalDuration là 1 ngày
                            leaveDay.TotalDuration = leaveDay.DateEnd.Date.Subtract(leaveDay.DateStart.Date).TotalDays;
                            leaveDay.TotalDuration = leaveDay.TotalDuration <= 0 ? 1 : leaveDay.TotalDuration + 1;
                            leaveDay.LeaveDays = leaveDay.TotalDuration;

                            leaveDay.DurationType = LeaveDayDurationType.E_FULLSHIFT.ToString();
                            leaveDay.Duration = halfShiftHours * 2;//tổng số giờ nghỉ của 1 ca
                            leaveDay.LeaveHours = leaveDay.Duration * leaveDay.LeaveDays;

                            if (shiftType == "F")
                            {
                                leaveDay.DurationType = LeaveDayDurationType.E_FIRSTHALFSHIFT.ToString();
                                leaveDay.LeaveHours = leaveDay.Duration = halfShiftHours;//tổng số giờ nghỉ của 1/2 ca
                                leaveDay.TotalDuration = 1;
                                leaveDay.LeaveDays = 0.5D;
                            }
                            else if (shiftType == "S")
                            {
                                leaveDay.DurationType = LeaveDayDurationType.E_LASTHALFSHIFT.ToString();
                                leaveDay.LeaveHours = leaveDay.Duration = halfShiftHours;//tổng số giờ nghỉ của 1/2 ca
                                leaveDay.TotalDuration = 1;
                                leaveDay.LeaveDays = 0.5D;
                            }

                            if (string.IsNullOrWhiteSpace(shiftCode) || listShiftInfo.Any(d => d.Code == shiftCode))
                            {
                                leaveDay.LeaveDayTypeID = leaveTypeInfo.ID;
                            }
                            else
                            {
                                if (!listShiftNotFound.Contains(shiftCode))
                                {
                                    listShiftNotFound.Add(shiftCode);
                                    shiftNotFound += (string.IsNullOrWhiteSpace(shiftNotFound) ? string.Empty : ",") + shiftCode;
                                }
                            }
                        }
                        else
                        {
                            if (!listShiftNotFound.Contains(leaveType))
                            {
                                listTypeNotFound.Add(leaveType);
                                leaveTypeNotFound += (string.IsNullOrWhiteSpace(leaveTypeNotFound) ? string.Empty : ",") + leaveType;
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(leaveTypeNotFound))
                    {
                        leaveDay.Description = "Không tìm thấy loại ngày nghỉ [" + leaveTypeNotFound + "]";
                        ListLeaveError.Add(leaveDay);
                    }
                    else if (!string.IsNullOrWhiteSpace(shiftNotFound))
                    {
                        leaveDay.Description = "Không tìm thấy ca [" + shiftNotFound + "]";
                        ListLeaveError.Add(leaveDay);
                    }
                    else
                    {
                        var listOldLeaveByProfile = listOldLeaveDay.Where(d => d.ProfileID == profileInfo.ID
                            && d.DateStart <= leaveDay.DateEnd && d.DateEnd >= leaveDay.DateStart).ToList();

                        var listApprovedLeaveByProfile = listOldLeaveDay.Where(d => d.ProfileID == profileInfo.ID
                            && d.DateStart <= leaveDay.DateEnd && d.DateEnd >= leaveDay.DateStart
                            && d.Status == LeaveDayStatus.E_APPROVED.ToString()).ToList();

                        if (listApprovedLeaveByProfile.Count() > 0 && !allowApproveLeave)
                        {
                            leaveDay.Description = "Không được sửa ngày nghỉ đã duyệt [" + leaveDay.DateStart + " - " + leaveDay.DateEnd + "]";
                            listLeaveCorrect.Add(leaveDay);
                        }
                        else
                        {
                            if (listOldLeaveByProfile.Count() > 0)
                            {
                                leaveDay.Description = "Trùng ngày nghỉ [" + leaveDay.DateStart + " - " + leaveDay.DateEnd + "]";
                                ListLeaveError.Add(leaveDay);
                            }
                            else
                            {
                                listLeaveCorrect.Add(leaveDay);
                            }
                        }
                    }
                }
                else
                {
                    leaveDay.Description = "Không tìm thấy nhân viên [" + leaveDay.CodeEmp + "]";
                    ListLeaveError.Add(leaveDay);
                }
            }

            #endregion
        }

        public string SaveListLeave(List<ImportLeavedayModel> listLeaveImport)
        {
            var ImportLeave = listLeaveImport.CopyData<Att_LeaveDayEntity>();
            BaseService _ba = new BaseService();
            var result = _ba.Add<Att_LeaveDayEntity>(ImportLeave);
            return result;
        }
        #endregion
    }
}
