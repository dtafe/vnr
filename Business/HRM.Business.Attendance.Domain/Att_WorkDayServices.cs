using HRM.Data.BaseRepository;
using System.Linq;
using HRM.Business.Attendance.Models;
using HRM.Data.Entity.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HRM.Infrastructure.Utilities;
using HRM.Business.Hr.Models;
using VnResource.Helper.Data;
using HRM.Data.Attendance.Data.Sql.Repositories;
using HRM.Business.Main.Domain;
using HRM.Business.Category.Models;
using HRM.Business.HrmSystem.Models;
using HRM.Data.Entity;
using System.Threading;
using System.Text.RegularExpressions;
using VnResource.Helper.Linq;
using HRM.Business.Hr.Domain;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Domain
{
    public class Att_WorkDayServices : BaseService
    {
        #region Methods

        /// <summary>
        /// Lấy danh sách tất cả WorkDay
        /// </summary>
        /// <returns></returns>
        public void SubmitWorkDay(List<Guid> ids)
        {
            using (var context = new VnrHrmDataContext())
            {
                var lstWorkday = new List<Att_Workday>();
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Att_WorkDayRepository(unitOfWork);
                lstWorkday = repo.FindBy(m => ids.Contains(m.ID)).ToList();
                foreach (var workday in lstWorkday)
                {
                    workday.Status = AttendanceDataStatus.E_SUBMIT.ToString();
                    repo.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Lấy toàn bộ data
        /// </summary>
        /// <returns></returns>
        public List<WorkdayCustom> GetWorkDaysByInOut(DateTime inTime, DateTime outTime)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Att_WorkDayRepository(unitOfWork);
                return repo.GetWorkDaysByInOut(inTime, outTime).Translate<WorkdayCustom>();
            }
        }

        public List<Att_WorkdayEntity> GetWorkDaysByDate(DateTime DateFrom, DateTime DateTo)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = (IUnitOfWork)(new UnitOfWork(context));
                var repo = new Att_WorkDayRepository(unitOfWork);
                var repoAtt_Workday = new CustomBaseRepository<Att_Workday>(unitOfWork);
                return repoAtt_Workday.FindBy(m => m.IsDelete == null && m.WorkDate >= DateFrom && m.WorkDate <= DateTo).Select(s => new Att_WorkdayEntity { ID = s.ID, ProfileID = s.ProfileID, InTimeRoot = s.InTimeRoot, OutTimeRoot = s.OutTimeRoot, LateEarlyDuration = s.LateEarlyDuration, ShiftID = s.ShiftID, MissInOutReason = s.MissInOutReason }).ToList<Att_WorkdayEntity>();
            }
        }

        #endregion

        #region CreateComputingTask

        public Guid CreateComputingTask(Guid userID, DateTime dateFrom, DateTime dateTo)
        {
            #region Khởi tạo Sys_AsynTask cho mỗi lần xử lý

            using (var context = new VnrHrmDataContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                Sys_AsynTask task = new Sys_AsynTask();
                task.ID = Guid.NewGuid();
                task.PercentComplete = 0.01D;
                task.TimeStart = DateTime.Now;
                task.Status = AsynTaskStatus.Doing.ToString();
                task.Type = AsynTask.Attendance_Compute_Workday.ToString();
                task.Summary = "Workday: " + dateFrom.ToString("dd/MM/yyyy") + " - " + dateTo.ToString("dd/MM/yyyy");
                unitOfWork.AddObject(typeof(Sys_AsynTask), task);
                unitOfWork.SaveChanges(userID);
                return task.ID;
            }

            #endregion
        }

        private void CompleteComputingTask(Guid asynTaskID, Guid userID,
            int totalComputed, int totalProfile, DataErrorCode dataErrorCode)
        {
            #region Lưu Sys_AsynTask khi xử lý xong

            if (asynTaskID != Guid.Empty)
            {
                using (var taskContext = new VnrHrmDataContext())
                {
                    IUnitOfWork taskunitOfWork = new UnitOfWork(taskContext);
                    var asynTask = taskunitOfWork.CreateQueryable<Sys_AsynTask>(s =>
                        s.ID == asynTaskID).FirstOrDefault();

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
                            asynTask.Description = "Dữ Liệu Ngày Công Đã Bị Khóa";
                        }

                        dataErrorCode = taskunitOfWork.SaveChanges();
                    }
                }
            }

            #endregion
        }

        #endregion

        #region ComputeWorkday

        public int ComputeWorkday(Guid asynTaskID, Guid userID,
            DateTime dateFrom, DateTime dateTo, List<Guid> listProfileID)
        {
            int result = 0;
            Hre_ProfileEntity[] listProfile = null;

            if (listProfileID != null && listProfileID.Count() > 0)
            {
                using (var context = new VnrHrmDataContext())
                {
                    IUnitOfWork unitOfWork = new UnitOfWork(context);

                    var profileQueryable = unitOfWork.CreateQueryable<Hre_Profile>(userID);
                    profileQueryable = profileQueryable.Where(d => listProfileID.Contains(d.ID));

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

                result = ComputeWorkday(asynTaskID, userID,
                    dateFrom, dateTo, listProfile);
            }

            return result;
        }

        public int ComputeWorkday(Guid asynTaskID, Guid userID, DateTime dateFrom,
            DateTime dateTo, List<Guid> listOrgStructureID, List<Guid> listWorkplaceID)
        {
            int result = 0;
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

                if (listWorkplaceID != null && listWorkplaceID.Count() > 0)
                {
                    profileQueryable = profileQueryable.Where(d => d.WorkPlaceID.HasValue
                        && listWorkplaceID.Contains(d.WorkPlaceID.Value));
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

            result = ComputeWorkday(asynTaskID, userID,
                dateFrom, dateTo, listProfile);

            return result;
        }

        private int ComputeWorkday(Guid asynTaskID, Guid userID, DateTime dateFrom,
            DateTime dateTo, params Hre_ProfileEntity[] listProfile)
        {
            try
            {
                int resultCount = 0;
                int pageSize = 500;

                var dataErrorCode = DataErrorCode.Success;
                int totalProfile = listProfile.Count();
                double timeoutMinutes = 10;

                #region Xử lý cho những nhân viên được chọn

                //Vinhtran - 20140625: Chia theo pagesize để không bị quá tải ram
                foreach (var listProfileSplit in listProfile.Chunk(pageSize))
                {
                    if (dataErrorCode == DataErrorCode.Success)
                    {
                        resultCount += ComputeWorkday(asynTaskID, userID, totalProfile, resultCount,
                            timeoutMinutes, dateFrom, dateTo, out dataErrorCode, listProfileSplit.ToArray());
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

                return resultCount;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private int ComputeWorkday(Guid asynTaskID, Guid userID, int totalProfile, int totalComputed, double timeoutMinutes,
            DateTime dateFrom, DateTime dateTo, out DataErrorCode dataErrorCode, params Hre_ProfileEntity[] listProfile)
        {
            List<Att_Workday> listWorkday = new List<Att_Workday>();
            List<Att_Workday> listWorkdayChecked = new List<Att_Workday>();
            List<Att_Workday> listWorkdayAnalyze = new List<Att_Workday>();

            dateFrom = dateFrom <= DateTime.MinValue ? DateTime.Now : dateFrom;
            dateTo = dateTo <= DateTime.MinValue ? DateTime.Now : dateTo;
            DateTime dateStart = dateFrom.AddDays(-1);
            DateTime dateEnd = dateTo.AddDays(1);

            using (var context = new VnrHrmDataContext())
            {
                var workHistoryServices = new Hre_WorkHistoryServices();
                var leavedayServices = new Att_LeavedayServices();
                var unitOfWork = new UnitOfWork(context);

                string waitStatus = ProfileStatusSyn.E_WAITING.ToString();
                string rosterStatus = RosterStatus.E_APPROVED.ToString();
                string leaveDayStatus = LeaveDayStatus.E_APPROVED.ToString();
                String appConfigInfo = AppConfig.E_SERVER_TAM.ToString();
                string E_LEAVE_EARLY = PregnancyType.E_LEAVE_EARLY.ToString();

                string workdaySrcType = WorkdaySrcType.E_MANUAL.ToString();
                string workdayStatus1 = WorkdayStatus.E_APPROVED.ToString();
                string workdayStatus2 = WorkdayStatus.E_WAIT_APPROVED.ToString();
                var listProfileID = listProfile.Select(d => d.ID).ToArray();

                #region Delete Workday đã tổng hợp trước đó

                if (unitOfWork.CheckLock(typeof(Att_Workday), dateFrom, dateTo))
                {
                    dataErrorCode = DataErrorCode.Locked;

                    CompleteComputingTask(asynTaskID, userID,
                        totalComputed, totalProfile, dataErrorCode);

                    return listWorkday.Count();
                }
                else
                {
                    Task task = Task.Run(() => DeleteWorkday(userID, dateFrom, dateTo,
                        workdaySrcType, workdayStatus1, workdayStatus2, listProfileID));
                }

                #endregion

                #region Khởi tạo dữ liệu cho lần tổng hợp

                var tamScanLogQueryable = unitOfWork.CreateQueryable<Att_TAMScanLog>(Guid.Empty, d => d.ProfileID.HasValue
                    && listProfileID.Contains(d.ProfileID.Value) && d.TimeLog.HasValue && d.TimeLog >= dateStart && d.TimeLog <= dateEnd);

                //Danh sách quẹt thẻ theo điều kiện được chọn
                var listAllTamScanLog = tamScanLogQueryable.Select(d => new Att_TAMScanLogEntity
                {
                    ID = d.ID,
                    ProfileID = d.ProfileID,
                    CardCode = d.CardCode,
                    CodeEmp = d.CodeEmp,
                    TimeLog = d.TimeLog,
                    SrcType = d.SrcType,
                    Type = d.Type
                }).ToList();

                var listRoster = unitOfWork.CreateQueryable<Att_Roster>(Guid.Empty, d => d.Status == rosterStatus && d.DateStart <= dateEnd
                    && d.DateEnd >= dateStart.Date && listProfileID.Contains(d.ProfileID)).Select(d => new Att_RosterEntity
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

                var listRosterGroup = unitOfWork.CreateQueryable<Att_RosterGroup>(Guid.Empty, m => m.DateStart != null && m.DateEnd != null
                    && m.DateStart <= dateEnd && m.DateEnd >= dateStart.Date).Select(d => new Att_RosterGroupEntity
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

                var listShift = unitOfWork.CreateQueryable<Cat_Shift>(Guid.Empty).ToList();

                var listHoliday = unitOfWork.CreateQueryable<Cat_DayOff>(Guid.Empty,
                    d => d.DateOff >= dateFrom.Date && d.DateOff <= dateTo).Select(d =>
                        new Cat_DayOffEntity
                        {
                            ID = d.ID,
                            DateOff = d.DateOff,
                            Type = d.Type
                        }).ToList();

                var listWorkdayExisting = unitOfWork.CreateQueryable<Att_Workday>(Guid.Empty, d => d.WorkDate >= dateStart
                    && d.WorkDate <= dateEnd && (d.SrcType == workdaySrcType || d.Status == workdayStatus1
                    || d.Status == workdayStatus2) && listProfileID.Contains(d.ProfileID)).ToList();

                var listLeaveDay = unitOfWork.CreateQueryable<Att_LeaveDay>(Guid.Empty, d => d.Status == leaveDayStatus
                    && d.DateStart <= dateTo && d.DateEnd >= dateFrom.Date && listProfileID.Contains(d.ID)).Select(d =>
                        new
                        {
                            d.ID,
                            d.ProfileID,
                            d.LeaveDayTypeID,
                            d.DateOvertimeOff,
                            d.DateStart,
                            d.DateEnd
                        }).ToList();

                var listLeaveDayType = unitOfWork.CreateQueryable<Cat_LeaveDayType>(Guid.Empty).Select(d =>
                    new
                    {
                        d.ID,
                        d.Code,
                        d.MissInOutReasonID
                    }).ToList();

                var listAllSetting = unitOfWork.CreateQueryable<Sys_AllSetting>(Guid.Empty, i => i.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_MAXHOURSONESHIFT.ToString()
                    || i.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_MAXHOURSNEXTINOUT.ToString() || i.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_MINMINUTESSAMEATT.ToString()
                    || i.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_SYMBOL.ToString() || i.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_DETECTSHIFT.ToString()
                    || i.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_TYPELOADDATA.ToString()).Select(d => new { d.Name, d.Value1, d.Value2 }).ToList();

                var inOutConfigMaxHoursOneShift = listAllSetting.Where(i => i.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_MAXHOURSONESHIFT.ToString()).FirstOrDefault();
                var inOutConfigMaxHoursNextInOut = listAllSetting.Where(i => i.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_MAXHOURSNEXTINOUT.ToString()).FirstOrDefault();
                var inOutConfigMinMinutesSameAtt = listAllSetting.Where(i => i.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_MINMINUTESSAMEATT.ToString()).FirstOrDefault();
                var inOutConfigSymbol = listAllSetting.Where(i => i.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_SYMBOL.ToString()).FirstOrDefault();
                var inOutConfigDetectShift = listAllSetting.Where(i => i.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_DETECTSHIFT.ToString()).FirstOrDefault();
                var inOutConfigTypeLoadData = listAllSetting.Where(i => i.Name == AppConfig.HRM_ATT_WORKDAY_SUMMARY_TYPELOADDATA.ToString()).FirstOrDefault();

                var inOutConfig = new string[] { string.Empty }.Select(d => new WorkdayConfig
                {
                    MaxHoursOneShift = inOutConfigMaxHoursOneShift != null ? inOutConfigMaxHoursOneShift.Value1 : string.Empty,
                    MaxHoursNextInOut = inOutConfigMaxHoursNextInOut != null ? inOutConfigMaxHoursNextInOut.Value1 : string.Empty,
                    MinMinutesSameAtt = inOutConfigMinMinutesSameAtt != null ? inOutConfigMinMinutesSameAtt.Value1 : string.Empty,
                    TypeLoadData = inOutConfigTypeLoadData != null ? inOutConfigTypeLoadData.Value1 : string.Empty,
                    SymbolIn = inOutConfigSymbol != null ? inOutConfigTypeLoadData.Value1 : string.Empty,
                    SymbolOut = inOutConfigSymbol != null ? inOutConfigTypeLoadData.Value2 : string.Empty,
                    DetectShift = inOutConfigDetectShift != null ? inOutConfigTypeLoadData.Value1 : string.Empty,
                    DetectWrongShift = inOutConfigDetectShift != null ? inOutConfigTypeLoadData.Value2 : string.Empty,
                }).FirstOrDefault();

                List<Att_Pregnancy> lstPrenancy = unitOfWork.CreateQueryable<Att_Pregnancy>(Guid.Empty, m => m.Type == E_LEAVE_EARLY
                    && m.DateEnd >= dateFrom && m.DateStart < dateTo && listProfileID.Contains(m.ProfileID)).ToList();

                List<Cat_LateEarlyRule> lstLateEarlyRule = unitOfWork.CreateQueryable<Cat_LateEarlyRule>(Guid.Empty).ToList();
                List<Cat_GradeAttendance> lstGradeConfig = unitOfWork.CreateQueryable<Cat_GradeAttendance>(Guid.Empty).ToList();

                List<Att_Grade> lstGrade = unitOfWork.CreateQueryable<Att_Grade>(Guid.Empty, m => m.ProfileID.HasValue && listProfileID.Contains(m.ProfileID.Value)).ToList();
                List<DateTime> lstDayOff = unitOfWork.CreateQueryable<Cat_DayOff>(Guid.Empty, m => m.DateOff >= dateStart && m.DateOff <= dateEnd).Select(m => m.DateOff).ToList<DateTime>();

                #endregion

                #region Bắt đầu xử lý tổng hợp in out

                string sameAttConfig = inOutConfig != null ? inOutConfig.MinMinutesSameAtt : string.Empty;
                var minMinutesSameAtt = VnResource.Helper.Data.DataHelper.TryGetValue<double>(sameAttConfig);

                //Loại bỏ những dòng quẹt thẻ liền kề nhau trong khoảng thời gian như cấu hình
                var listTamScanLog = RemoveSameTimeLog(listAllTamScanLog, minMinutesSameAtt);

                //Giữ danh sách những quẹt thẻ tạm thời được cho là trùng với những quẹt thẻ khác để xử lý sau
                var listTamScanLogRemove = listAllTamScanLog.Where(d => !listTamScanLog.Contains(d)).ToList();

                using (var taskContext = new VnrHrmDataContext())
                {
                    var taskUnitOfWork = new UnitOfWork(taskContext);
                    Sys_AsynTask asynTask = null;

                    if (asynTaskID != Guid.Empty)
                    {
                        asynTask = taskUnitOfWork.CreateQueryable<Sys_AsynTask>(s =>
                           s.ID == asynTaskID).FirstOrDefault();
                    }

                    int totalComputedProfileMustSubmitTask = 50;
                    int totalComputedProfileMustSubmit = 200;
                    int totalComputedProfileForSubmit = 0;
                    int totalComputedProfileForTask = 0;

                    totalComputedProfileMustSubmitTask = totalProfile * 5 / 100;

                    if (totalComputedProfileMustSubmitTask > listProfile.Count())
                    {
                        totalComputedProfileMustSubmitTask = listProfile.Count();
                    }

                    foreach (var profile in listProfile)
                    {
                        #region Cập nhật thời gian tính timeout cho task

                        if (asynTask != null)
                        {
                            bool mustSaveTask = false;

                            if (asynTask.TimeEnd.HasValue)
                            {
                                var timeoutDate = DateTime.Now.AddMinutes(-timeoutMinutes);

                                if (timeoutDate.AddMinutes(1) >= asynTask.TimeEnd.Value)
                                {
                                    asynTask.TimeEnd = DateTime.Now;
                                    mustSaveTask = true;
                                }
                            }

                            totalComputedProfileForTask++;
                            double percent = totalComputedProfileForTask / (double)totalProfile;

                            if (totalComputedProfileForTask >= totalComputedProfileMustSubmitTask)
                            {
                                var totalPercent = asynTask.PercentComplete + percent;
                                asynTask.PercentComplete = totalPercent;
                                asynTask.TimeEnd = DateTime.Now;
                                mustSaveTask = true;
                            }

                            if (mustSaveTask)
                            {
                                taskUnitOfWork.SaveChanges(userID);
                                totalComputedProfileForTask = 0;
                            }
                        }

                        #endregion

                        #region Những dữ liệu liên quan theo từng nhân viên

                        var listRosterByProfile = listRoster.Where(d => d.ProfileID == profile.ID).ToList();
                        var listMonthShifts = Att_AttendanceLib.GetDailyShifts(dateStart, dateEnd, profile.ID, listRosterByProfile, listRosterGroup);

                        #endregion

                        #region Xử lý tổng hợp in out theo từng ngày

                        for (DateTime date = dateFrom.Date; date <= dateTo; date = date.AddDays(1))
                        {
                            Att_Workday workday = listWorkdayExisting.Where(d => d.ProfileID == profile.ID && d.WorkDate == date).FirstOrDefault();
                            var shiftByDate = listMonthShifts.Where(d => d.Key == date).Select(d => d.Value).FirstOrDefault();

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

                            var shiftInfo1 = listShift.Where(d => d.ID == shiftID1).FirstOrDefault();
                            var shiftInfo2 = listShift.Where(d => d.ID == shiftID2).FirstOrDefault();

                            if ((profile.DateQuit.HasValue && profile.DateQuit.Value.Date <= date)
                                || (profile.DateHire.HasValue && profile.DateHire.Value.Date > date))
                            {
                                if (workday != null)
                                {
                                    workday.IsDelete = true;
                                }

                                continue;
                            }

                            if (workday != null)
                            {
                                if (shiftByDate == null)
                                {
                                    shiftByDate = new List<Guid?>();
                                }

                                if ((!shiftID1.HasValue || shiftID1 == Guid.Empty))
                                {
                                    if (workday.ShiftActual.HasValue)
                                    {
                                        shiftByDate.Insert(0, workday.ShiftActual);
                                    }
                                }

                                if ((!shiftID2.HasValue || shiftID2 == Guid.Empty))
                                {
                                    if (workday.ShiftActual2.HasValue)
                                    {
                                        if (shiftByDate.Count() == 0)
                                        {
                                            shiftByDate.Add(Guid.Empty);
                                        }

                                        shiftByDate.Add(workday.ShiftActual2);
                                    }
                                }

                                if (listMonthShifts.ContainsKey(date))
                                {
                                    listMonthShifts[date] = shiftByDate;
                                }
                                else
                                {
                                    listMonthShifts.Add(date, shiftByDate);
                                }

                                if ((workday.SrcType == workdaySrcType || workday.Status == workdayStatus1 || workday.Status == workdayStatus2))
                                {
                                    listWorkday.Add(workday);
                                    continue;
                                }
                            }

                            Guid? actualShiftID1 = null;
                            Guid? actualShiftID2 = null;
                            bool isPreWorkday = false;

                            var listTamScanLogByCardCode = listTamScanLog.Where(d => d.ProfileID == profile.ID).OrderBy(d => d.TimeLog).ToList();
                            var listTamScanLogByCardCodeUnchecked = listTamScanLogByCardCode.Where(d => !d.Checked && d.TimeLog.HasValue).ToList();

                            if (shiftID1.HasValue && shiftID1 != Guid.Empty)
                            {
                                #region Trường hợp có lịch làm việc - ca thứ nhất

                                workday = CreateWorkday(unitOfWork, date, profile, inOutConfig, listHoliday, listWorkday, workday,
                                    false, shiftID1, shiftInfo1, listShift, listMonthShifts, out actualShiftID1,
                                    listTamScanLogByCardCode, listTamScanLogByCardCodeUnchecked);

                                #endregion
                            }
                            else if (shiftID2.HasValue && shiftID2 != Guid.Empty)
                            {
                                #region Trường hợp có lịch làm việc - ca thứ hai

                                workday = CreateWorkday(unitOfWork, date, profile, inOutConfig, listHoliday, listWorkday, workday,
                                    true, shiftID2, shiftInfo2, listShift, listMonthShifts, out actualShiftID2,
                                    listTamScanLogByCardCode, listTamScanLogByCardCodeUnchecked);

                                #endregion
                            }
                            else if (inOutConfig != null && inOutConfig.DetectShift == Boolean.TrueString)
                            {
                                #region Trường hợp không có lịch làm việc

                                var listTamScanLogByDate = listTamScanLogByCardCodeUnchecked.Where(d =>
                                    d.TimeLog.Value.Date == date).ToList();

                                if (listTamScanLogByDate.Count() > 0)
                                {
                                    var shiftInTime = date.Date.AddHours(12);
                                    var listTimeLog = new List<Att_TAMScanLogEntity>();
                                    bool isWrongShiftDetected = false;

                                    foreach (var item in listTamScanLogByDate)
                                    {
                                        if (item.TimeLog < shiftInTime)
                                        {
                                            //Quẹt thẻ gần với thời gian vào của ca -> miss-out hoặc làm sai ca
                                            //Tìm ca làm việc trước gần nhất và dòng quẹt thẻ trước gần nhất của nhân viên đang xét
                                            var preShift = listMonthShifts.Where(d => d.Key >= date.AddDays(-1) && d.Key < date).OrderByDescending(d => d.Key).FirstOrDefault();
                                            var preShiftValue = preShift.Value != null && preShift.Value.Count() > 0 ? preShift.Value.LastOrDefault() : null;

                                            //Lấy dòng quẹt thẻ hiện tại và 2 dòng quẹt thẻ phía trước gần nhất của nhân viên đang xét để kiểm tra ca trước
                                            var listPreTamScanLog = listTamScanLogByCardCode.Where(d => d.TimeLog <= item.TimeLog).OrderByDescending(d => d.TimeLog).Take(3).ToList();
                                            var listTamScanLogByPreShift = GetTamScanLogByShift(preShift.Key, listShift, preShiftValue, listMonthShifts, listPreTamScanLog.ToArray());

                                            //Nếu 2 dòng quẹt thẻ phía trước không thuộc một ca khác trong lịch của nhân viên
                                            if (listTamScanLogByPreShift == null || listTamScanLogByPreShift.Count() < 2)
                                            {
                                                var preTamScanLog = listPreTamScanLog.Where(d => d.TimeLog < item.TimeLog).FirstOrDefault();
                                                var currentShiftDuration = 12;//cho ca tối đa được 12 tiếng đồng hồ

                                                if (preTamScanLog != null && listWorkday.Any(d => d.ProfileID == profile.ID && (d.InTime1 == preTamScanLog.TimeLog
                                                    || d.InTime2 == preTamScanLog.TimeLog || d.InTime3 == preTamScanLog.TimeLog || d.InTime4 == preTamScanLog.TimeLog
                                                    || d.OutTime1 == preTamScanLog.TimeLog || d.OutTime2 == preTamScanLog.TimeLog || d.OutTime3 == preTamScanLog.TimeLog
                                                    || d.OutTime4 == preTamScanLog.TimeLog)))
                                                {
                                                    //Trường hợp quẹt thẻ này đã sử dụng
                                                    preTamScanLog = null;
                                                }

                                                //Nếu quẹt thẻ hiện tại kết với quẹt thẻ trước đó mà phù hợp duration thì ghép sai ca
                                                if (preTamScanLog != null && preTamScanLog.TimeLog.HasValue && item.TimeLog.Value
                                                    .Subtract(preTamScanLog.TimeLog.Value).TotalHours <= currentShiftDuration)
                                                {
                                                    if (preTamScanLog.TimeLog < date)
                                                    {
                                                        if (preShiftValue.HasValue)
                                                        {
                                                            workday = listWorkday.Where(d => d.ProfileID == profile.ID
                                                                && d.WorkDate == date.AddDays(-1)).FirstOrDefault();

                                                            if (workday == null)
                                                            {
                                                                if (preTamScanLog.TimeLog.Value.Date >= dateFrom.Date
                                                                    && preTamScanLog.TimeLog.Value.Date <= dateTo.Date)
                                                                {
                                                                    workday = new Att_Workday();
                                                                    unitOfWork.AddObject(typeof(Att_Workday), workday);
                                                                    workday.WorkDate = preTamScanLog.TimeLog.Value.Date;
                                                                    workday.FirstInTime = workday.InTime1 = preTamScanLog.TimeLog;
                                                                    workday.LastOutTime = workday.OutTime1 = item.TimeLog;
                                                                    workday.Type = WorkdayType.E_DETECTED_SHIFT.ToString();

                                                                    isPreWorkday = true;
                                                                    preTamScanLog.Checked = true;
                                                                    isWrongShiftDetected = true;
                                                                    break;//chỉ hỗ trợ 1 ca
                                                                }
                                                            }
                                                            else
                                                            {
                                                                listTimeLog.Add(item);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            listTimeLog.Add(item);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (workday == null)
                                                        {
                                                            workday = new Att_Workday();
                                                            unitOfWork.AddObject(typeof(Att_Workday), workday);
                                                        }

                                                        workday.FirstInTime = workday.InTime1 = preTamScanLog.TimeLog;
                                                        workday.LastOutTime = workday.OutTime1 = item.TimeLog;
                                                        workday.Type = WorkdayType.E_DETECTED_SHIFT.ToString();

                                                        preTamScanLog.Checked = true;
                                                        isWrongShiftDetected = true;
                                                        break;//chỉ hỗ trợ 1 ca
                                                    }
                                                }
                                                else
                                                {
                                                    listTimeLog.Add(item);
                                                }
                                            }
                                            else
                                            {
                                                if (!listTamScanLogByPreShift.Any(d => d.TimeLog == item.TimeLog))
                                                {
                                                    listTimeLog.Add(item);
                                                }

                                                listTimeLog = listTimeLog.Where(d => !listTamScanLogByPreShift.Contains(d)).ToList();
                                            }
                                        }
                                        else if (item.TimeLog > shiftInTime)
                                        {
                                            //Quẹt thẻ gần với thời gian ra của ca -> miss-in hoặc làm sai ca
                                            //Tìm ca làm việc tiếp theo gần nhất và dòng quẹt thẻ tiếp theo gần nhất của nhân viên đang xét
                                            var nextShift = listMonthShifts.Where(d => d.Key > date && d.Key <= date.AddDays(1)).OrderBy(d => d.Key).FirstOrDefault();
                                            var nextShiftValue = nextShift.Value != null && nextShift.Value.Count() > 0 ? nextShift.Value.FirstOrDefault() : null;

                                            //Lấy dòng quẹt thẻ hiện tại và 2 dòng quẹt thẻ tiếp theo gần nhất của nhân viên đang xét để kiểm tra ca tiếp theo
                                            var listNextTamScanLog = listTamScanLogByCardCodeUnchecked.Where(d => d.TimeLog >= item.TimeLog).OrderBy(d => d.TimeLog).Take(3).ToList();
                                            var listTamScanLogByNextShift = GetTamScanLogByShift(nextShift.Key, listShift, nextShiftValue, listMonthShifts, listNextTamScanLog.ToArray());

                                            //Nếu 2 dòng quẹt thẻ tiếp theo không thuộc một ca khác trong lịch của nhân viên
                                            if (listTamScanLogByNextShift == null || listTamScanLogByNextShift.Count() < 2)
                                            {
                                                listTimeLog.Add(item);

                                                foreach (var nextTamScanLog in listNextTamScanLog.Where(d => d.TimeLog > item.TimeLog))
                                                {
                                                    var currentShiftDuration = 12;//cho ca tối đa được 12 tiếng đồng hồ

                                                    //Nếu quẹt thẻ hiện tại kết với quẹt thẻ tiếp theo mà phù hợp duration thì ghép sai ca
                                                    if (nextTamScanLog != null && nextTamScanLog.TimeLog.HasValue && nextTamScanLog
                                                        .TimeLog.Value.Subtract(item.TimeLog.Value).TotalHours <= currentShiftDuration)
                                                    {
                                                        listTimeLog.Add(nextTamScanLog);
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                if (!listTamScanLogByNextShift.Any(d => d.TimeLog == item.TimeLog))
                                                {
                                                    listTimeLog.Add(item);
                                                }

                                                listTimeLog = listTimeLog.Where(d => !listTamScanLogByNextShift.Contains(d)).ToList();
                                            }
                                        }
                                    }

                                    if (!isWrongShiftDetected)
                                    {
                                        if (listTimeLog.Count() > 0)
                                        {
                                            var listRemove = new List<Att_TAMScanLogEntity>();

                                            if (workday == null)
                                            {
                                                workday = new Att_Workday();
                                                unitOfWork.AddObject(typeof(Att_Workday), workday);
                                            }

                                            if (listTimeLog.Count() >= 2)
                                            {
                                                workday.FirstInTime = workday.InTime1 = listTimeLog.OrderBy(d => d.TimeLog).Select(d => d.TimeLog).FirstOrDefault();
                                                workday.LastOutTime = workday.OutTime1 = listTimeLog.OrderBy(d => d.TimeLog).Select(d => d.TimeLog).LastOrDefault();
                                                var currentShiftDuration = 16;//cho ca tối đa được 12 tiếng đồng hồ
                                                isWrongShiftDetected = true;

                                                if (workday.LastOutTime.HasValue && workday.FirstInTime.HasValue && workday.LastOutTime.Value
                                                    .Subtract(workday.FirstInTime.Value).TotalHours <= currentShiftDuration)
                                                {
                                                    workday.Type = WorkdayType.E_DETECTED_SHIFT.ToString();
                                                }
                                                else
                                                {
                                                    foreach (var item in listTimeLog.Where(d => d.TimeLog > workday.FirstInTime).OrderByDescending(d => d.TimeLog))
                                                    {
                                                        workday.LastOutTime = workday.OutTime1 = item.TimeLog;

                                                        if (item.TimeLog > workday.FirstInTime && item.TimeLog.HasValue && workday.FirstInTime.HasValue
                                                            && item.TimeLog.Value.Subtract(workday.FirstInTime.Value).TotalHours <= currentShiftDuration)
                                                        {
                                                            workday.Type = WorkdayType.E_DETECTED_SHIFT.ToString();
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            workday.Type = WorkdayType.E_LONGIN_SHIFT.ToString();
                                                        }
                                                    }

                                                    listRemove = listTimeLog.Where(d => d.TimeLog > workday.LastOutTime).ToList();
                                                }
                                            }
                                            else if (listTimeLog.Count() == 1)
                                            {
                                                if (listTimeLog.Any(d => d.TimeLog < shiftInTime))
                                                {
                                                    workday.FirstInTime = workday.InTime1 = listTimeLog.Select(d => d.TimeLog).FirstOrDefault();
                                                    workday.Type = WorkdayType.E_MISS_OUT.ToString();//miss-in hay miss-out cũng như nhau
                                                }
                                                else
                                                {
                                                    workday.LastOutTime = workday.OutTime1 = listTimeLog.Select(d => d.TimeLog).FirstOrDefault();
                                                    workday.Type = WorkdayType.E_MISS_IN.ToString();//miss-in hay miss-out cũng như nhau
                                                }

                                                isWrongShiftDetected = true;
                                            }

                                            listTimeLog.Where(d => !listRemove.Contains(d)).ToList().ForEach(d => d.Checked = true);
                                        }
                                    }

                                    if (workday != null)
                                    {
                                        //Trường hợp wrong-shift và detected-shift thì phải check lại actualShiftID
                                        actualShiftID1 = GetDetectedShiftID(workday.InTime1, workday.OutTime1, listShift, null);
                                    }
                                }

                                #endregion
                            }

                            if (workday != null)
                            {
                                if (string.IsNullOrWhiteSpace(workday.Type))
                                {
                                    workday.Type = WorkdayType.E_NORMAL.ToString();
                                }

                                workday.SrcType = WorkdaySrcType.E_NORMAL.ToString();
                                workday.ProfileID = profile.ID;

                                listTamScanLogByCardCode = listTamScanLogRemove.Where(d => d.ProfileID == profile.ID).OrderBy(d => d.TimeLog).ToList();
                                listTamScanLogByCardCodeUnchecked = listTamScanLogByCardCode.Where(d => !d.Checked && d.TimeLog.HasValue).ToList();

                                workday.FirstInTime = workday.InTime1 = FindTimeLogAvailable(listTamScanLogByCardCode, workday.InTime1, minMinutesSameAtt, false);
                                workday.LastOutTime = workday.OutTime1 = FindTimeLogAvailable(listTamScanLogByCardCode, workday.OutTime1, minMinutesSameAtt, true);

                                if (!isPreWorkday)
                                {
                                    workday.WorkDate = date;
                                    workday.InTimeRoot = workday.InTime1;
                                    workday.OutTimeRoot = workday.OutTime1;
                                }

                                if (shiftID1 != null && shiftID1 != Guid.Empty)
                                {
                                    workday.ShiftID = shiftID1.Value;
                                    workday.ShiftActual = shiftID1.Value;
                                    workday.ShiftApprove = shiftID1.Value;
                                }

                                if (shiftID2 != null && shiftID2 != Guid.Empty)
                                {
                                    workday.Shift2ID = shiftID2.Value;
                                    workday.ShiftActual2 = shiftID2.Value;
                                    workday.ShiftApprove2 = shiftID2.Value;
                                }

                                if (actualShiftID1.HasValue && actualShiftID1 != Guid.Empty)
                                {
                                    workday.ShiftActual = actualShiftID1;
                                    workday.ShiftApprove = actualShiftID1;
                                }

                                if (actualShiftID2.HasValue && actualShiftID2 != Guid.Empty)
                                {
                                    workday.ShiftActual2 = actualShiftID2;
                                    workday.ShiftApprove2 = actualShiftID2;
                                }

                                var listLeaveDayByProfile = listLeaveDay.Where(d => d.ProfileID == profile.ID).ToList();

                                var leaveDayID1 = listLeaveDayByProfile.Where(d => workday.WorkDate >= d.DateStart
                                    && workday.WorkDate <= d.DateEnd).Select(d => d.ID).FirstOrDefault();

                                var leaveDayID2 = listLeaveDayByProfile.Where(d => d.ID != workday.LeaveDayID1 && workday.WorkDate
                                    >= d.DateStart && workday.WorkDate <= d.DateEnd).Select(d => d.ID).FirstOrDefault();

                                workday.LeaveDayID1 = leaveDayID1 != Guid.Empty ? (Guid?)leaveDayID1 : null;
                                workday.LeaveDayID2 = leaveDayID2 != Guid.Empty ? (Guid?)leaveDayID2 : null;

                                workday.MissInOutReason = listLeaveDayType.Where(d => listLeaveDayByProfile.Any(p => p.LeaveDayTypeID == d.ID)
                                    && d.MissInOutReasonID != null).Select(d => d.MissInOutReasonID).FirstOrDefault();

                                listWorkday.Add(workday);
                            }
                        }

                        #endregion

                        #region Chia thành nhiều giai đoạn lưu ngày công

                        //Nên submit lần đầu tiên khi đã tính được 5 profile
                        bool firstSubmit = listProfile.ToList().IndexOf(profile) == 5;
                        firstSubmit = totalComputed >= 5 ? false : firstSubmit;//đã chạy 1 lần
                        totalComputedProfileForSubmit++;
                        totalComputed++;

                        if (firstSubmit || totalComputedProfileForSubmit >= totalComputedProfileMustSubmit)
                        {
                            listWorkdayAnalyze = listWorkday.Where(d => !listWorkdayChecked.Contains(d)).ToList();
                            listWorkdayChecked.AddRange(listWorkdayAnalyze);

                            leavedayServices.AnalyzeLeaveLate(listWorkdayAnalyze, lstPrenancy, lstLateEarlyRule,
                                listShift, lstGrade, lstGradeConfig, lstDayOff);

                            totalComputedProfileForSubmit = firstSubmit ? totalComputedProfileForSubmit : 0;
                            dataErrorCode = unitOfWork.SaveChanges(userID);

                            if (dataErrorCode == DataErrorCode.Locked)
                            {
                                break;
                            }
                        }

                        #endregion
                    }
                }

                #endregion

                #region Lưu kết quả tổng hợp in out

                listWorkdayAnalyze = listWorkday.Where(d => !listWorkdayChecked.Contains(d)).ToList();
                listWorkdayChecked.AddRange(listWorkdayAnalyze);

                leavedayServices.AnalyzeLeaveLate(listWorkdayAnalyze, lstPrenancy, lstLateEarlyRule,
                    listShift, lstGrade, lstGradeConfig, lstDayOff);

                unitOfWork.SetCorrectOrgStructureID(listWorkday);
                dataErrorCode = unitOfWork.SaveChanges(userID);

                #endregion
            }

            return listWorkday.Count();
        }

        private Att_Workday CreateWorkday(IUnitOfWork unitOfWork, DateTime date, Hre_ProfileEntity profile, WorkdayConfig inOutConfig,
            List<Cat_DayOffEntity> listHoliday, List<Att_Workday> listWorkday, Att_Workday workday, bool isFromShift2, Guid? shiftID,
            Cat_Shift shiftInfo, List<Cat_Shift> listShift, Dictionary<DateTime, List<Guid?>> listMonthShifts, out Guid? actualShiftID,
            List<Att_TAMScanLogEntity> listTamScanLogByCardCode, List<Att_TAMScanLogEntity> listTamScanLogByCardCodeUnchecked)
        {
            #region Trường hợp có lịch làm việc

            #region Kiểm tra dữ liệu quẹt theo theo ngày (ca)

            var listTamScanLogByShift = GetTamScanLogByShift(date, listShift, shiftID,
              listMonthShifts, listTamScanLogByCardCodeUnchecked.ToArray());

            //Checked = true để không tự detect shift cho nó
            listTamScanLogByShift.ForEach(d => d.Checked = true);
            actualShiftID = null;

            if (workday == null)
            {
                workday = new Att_Workday();
                unitOfWork.AddObject(typeof(Att_Workday), workday);
            }

            #endregion

            if (inOutConfig == null || inOutConfig.TypeLoadData.IsNullOrEmpty()
                || inOutConfig.TypeLoadData == TypeLoadData.E_DEFAULT.ToString()
                || inOutConfig.TypeLoadData == TypeLoadData.E_MAXMIN.ToString())
            {
                if (listTamScanLogByShift.Count() >= 2)
                {
                    #region Trường hợp có ít nhất 2 dòng quẹt thẻ thuộc ca đang xét

                    var inTime = listTamScanLogByShift.Select(d => d.TimeLog).FirstOrDefault();
                    var outTime = listTamScanLogByShift.Select(d => d.TimeLog).LastOrDefault();

                    //Khi phát hiện 2 quẹt thẻ hợp lệ với ca đăng ký thì nhân luôn - không detect shift
                    workday.FirstInTime = !workday.FirstInTime.HasValue || workday.FirstInTime > inTime ? inTime : workday.FirstInTime;
                    workday.LastOutTime = !workday.LastOutTime.HasValue || workday.LastOutTime < outTime ? outTime : workday.LastOutTime;

                    if (isFromShift2)
                    {
                        workday.InTime2 = inTime;
                        workday.OutTime2 = outTime;
                    }
                    else
                    {
                        workday.InTime1 = inTime;
                        workday.OutTime1 = outTime;
                    }

                    if (inOutConfig.DetectWrongShift == Boolean.TrueString)
                    {
                        //Trường hợp wrong-shift và detected-shift thì phải check lại actualShiftID
                        actualShiftID = GetDetectedShiftID(inTime, outTime, listShift, null);

                        if (actualShiftID != Guid.Empty && actualShiftID != shiftID)
                        {
                            var listTest = GetTamScanLogByShift(inTime.Value.Date, listShift,
                                actualShiftID, listMonthShifts, listTamScanLogByShift.ToArray());

                            if (listTest != null && listTest.Count() >= 2 && listTest.Any(d =>
                                d.TimeLog.HasValue && d.TimeLog.Value.Date == date))
                            {
                                workday.Type = WorkdayType.E_WRONG_SHIFT.ToString();
                            }
                            else
                            {
                                actualShiftID = null;
                            }
                        }
                    }

                    #endregion
                }
                else if (listTamScanLogByShift.Count() == 1)
                {
                    #region Trường hợp có 1 dòng quẹt thẻ thuộc ca đang xét

                    if (listTamScanLogByShift.Any(d => d.SrcType == TAMScanType.E_OUT.ToString()))
                    {
                        //Dữ liệu quẹt thẻ được lấy từ máy chấm công ra - có option máy vào máy ra
                        workday.Type = WorkdayType.E_MISS_IN.ToString();

                        if (isFromShift2)
                        {
                            workday.OutTime2 = listTamScanLogByShift.Select(d => d.TimeLog).LastOrDefault();
                        }
                        else
                        {
                            workday.OutTime1 = listTamScanLogByShift.Select(d => d.TimeLog).LastOrDefault();
                        }
                    }
                    else
                    {
                        //So sanh độ lệch với thời gian của ca -> gần bên nào thì tính theo bên đó
                        var timeLog = listTamScanLogByShift.Select(d => d.TimeLog).FirstOrDefault();
                        var shiftInTime = date.Add(shiftInfo.InTime.TimeOfDay);

                        if (timeLog.Value <= shiftInTime || timeLog.Value.Subtract(shiftInTime)
                            < shiftInTime.AddHours(shiftInfo.CoOut).Subtract(timeLog.Value))
                        {
                            //1 dòng quẹt thẻ rơi vào nửa đầu của ca
                            workday.Type = WorkdayType.E_MISS_OUT.ToString();

                            if (isFromShift2)
                            {
                                workday.InTime2 = timeLog;
                            }
                            else
                            {
                                workday.InTime1 = timeLog;
                            }
                        }

                        if (inOutConfig.DetectWrongShift == Boolean.TrueString)
                        {
                            #region Tìm quẹt thẻ phía trước

                            //Quẹt thẻ gần với thời gian vào của ca -> miss-out hoặc làm sai ca
                            //Tìm ca làm việc trước gần nhất và dòng quẹt thẻ trước gần nhất của nhân viên đang xét
                            var preShift = listMonthShifts.Where(d => d.Key >= date.AddDays(-1) && d.Key < date).OrderByDescending(d => d.Key).FirstOrDefault();
                            var preShiftValue = preShift.Value != null && preShift.Value.Count() > 0 ? preShift.Value.LastOrDefault() : null;

                            //Lấy dòng quẹt thẻ hiện tại và 2 dòng quẹt thẻ phía trước gần nhất của nhân viên đang xét để kiểm tra ca trước
                            var listPreTamScanLog = listTamScanLogByCardCode.Where(d => d.TimeLog <= timeLog).OrderByDescending(d => d.TimeLog).Take(3).ToList();
                            var listTamScanLogByPreShift = GetTamScanLogByShift(preShift.Key, listShift, preShiftValue, listMonthShifts, listPreTamScanLog.ToArray());

                            //Nếu 2 dòng quẹt thẻ phía trước không thuộc một ca khác trong lịch của nhân viên
                            if (listTamScanLogByPreShift == null || listTamScanLogByPreShift.Count() < 2)
                            {
                                var preTamScanLog = listPreTamScanLog.Where(d => d.TimeLog < timeLog).FirstOrDefault();
                                var currentShiftDuration = shiftInfo.MinIn + shiftInfo.CoOut + shiftInfo.MaxOut;

                                if (preTamScanLog != null && listWorkday.Any(d => d.ProfileID == profile.ID && (d.InTime1 == preTamScanLog.TimeLog
                                    || d.InTime2 == preTamScanLog.TimeLog || d.InTime3 == preTamScanLog.TimeLog || d.InTime4 == preTamScanLog.TimeLog
                                    || d.OutTime1 == preTamScanLog.TimeLog || d.OutTime2 == preTamScanLog.TimeLog || d.OutTime3 == preTamScanLog.TimeLog
                                    || d.OutTime4 == preTamScanLog.TimeLog)))
                                {
                                    //Trường hợp quẹt thẻ này đã sử dụng
                                    preTamScanLog = null;
                                }

                                //Nếu quẹt thẻ hiện tại kết với quẹt thẻ tiếp theo mà phù hợp duration thì ghép sai ca
                                if (preTamScanLog != null && preTamScanLog.TimeLog.HasValue && timeLog.Value
                                    .Subtract(preTamScanLog.TimeLog.Value).TotalHours <= currentShiftDuration)
                                {
                                    //Trường hợp wrong-shift và detected-shift thì phải check lại actualShiftID
                                    actualShiftID = GetDetectedShiftID(preTamScanLog.TimeLog, timeLog, listShift, null);

                                    var listTest = GetTamScanLogByShift(preTamScanLog.TimeLog.Value.Date, listShift, actualShiftID,
                                        listMonthShifts, new Att_TAMScanLogEntity[] { preTamScanLog, listTamScanLogByShift.FirstOrDefault() });

                                    if (preTamScanLog.TimeLog.Value.Date == date.Date && listTest != null && listTest.Count() >= 2
                                        && !listTest.Any(d => d.TimeLog.HasValue && d.TimeLog.Value.Date < date.Date))
                                    {
                                        if (isFromShift2)
                                        {
                                            workday.InTime2 = preTamScanLog.TimeLog;
                                            workday.OutTime2 = timeLog;
                                        }
                                        else
                                        {
                                            workday.InTime1 = preTamScanLog.TimeLog;
                                            workday.OutTime1 = timeLog;
                                        }

                                        workday.FirstInTime = !workday.FirstInTime.HasValue || workday.FirstInTime > preTamScanLog.TimeLog ? preTamScanLog.TimeLog : workday.FirstInTime;
                                        workday.LastOutTime = !workday.LastOutTime.HasValue || workday.LastOutTime < timeLog ? timeLog : workday.LastOutTime;

                                        workday.Type = WorkdayType.E_WRONG_SHIFT.ToString();
                                        preTamScanLog.Checked = true;
                                    }
                                    else
                                    {
                                        actualShiftID = null;
                                    }
                                }
                            }

                            #endregion

                            #region Tìm quẹt thẻ phía sau

                            if (workday.Type == WorkdayType.E_MISS_OUT.ToString()
                                || string.IsNullOrWhiteSpace(workday.Type))
                            {
                                if (string.IsNullOrWhiteSpace(workday.Type))
                                {
                                    //1 dòng quẹt thẻ rơi vào nửa sau của ca
                                    workday.Type = WorkdayType.E_MISS_IN.ToString();

                                    if (isFromShift2)
                                    {
                                        workday.OutTime2 = timeLog;
                                    }
                                    else
                                    {
                                        workday.OutTime1 = timeLog;
                                    }
                                }

                                //Quẹt thẻ gần với thời gian ra của ca -> miss-in hoặc làm sai ca
                                //Tìm ca làm việc tiếp theo gần nhất và dòng quẹt thẻ tiếp theo gần nhất của nhân viên đang xét
                                var nextShift = listMonthShifts.Where(d => d.Key > date && d.Key <= date.AddDays(1)).OrderBy(d => d.Key).FirstOrDefault();
                                var nextShiftValue = nextShift.Value != null && nextShift.Value.Count() > 0 ? nextShift.Value.FirstOrDefault() : null;

                                //Lấy dòng quẹt thẻ hiện tại và 2 dòng quẹt thẻ tiếp theo gần nhất của nhân viên đang xét để kiểm tra ca tiếp theo
                                var listNextTamScanLog = listTamScanLogByCardCodeUnchecked.Where(d => d.TimeLog >= timeLog).OrderBy(d => d.TimeLog).Take(3).ToList();
                                var listTamScanLogByNextShift = GetTamScanLogByShift(nextShift.Key, listShift, nextShiftValue, listMonthShifts, listNextTamScanLog.ToArray());

                                //Nếu 2 dòng quẹt thẻ tiếp theo không thuộc một ca khác trong lịch của nhân viên
                                if (listTamScanLogByNextShift == null || listTamScanLogByNextShift.Count() < 2)
                                {
                                    var nextTamScanLog = listNextTamScanLog.Where(d => d.TimeLog > timeLog).FirstOrDefault();
                                    var currentShiftDuration = shiftInfo.MinIn + shiftInfo.CoOut + shiftInfo.MaxOut;

                                    //Nếu quẹt thẻ hiện tại kết với quẹt thẻ tiếp theo mà phù hợp duration thì ghép sai ca
                                    if (nextTamScanLog != null && nextTamScanLog.TimeLog.HasValue && nextTamScanLog
                                        .TimeLog.Value.Subtract(timeLog.Value).TotalHours <= currentShiftDuration)
                                    {
                                        //Trường hợp wrong-shift và detected-shift thì phải check lại actualShiftID
                                        actualShiftID = GetDetectedShiftID(timeLog, nextTamScanLog.TimeLog, listShift, null);

                                        var listTest = GetTamScanLogByShift(timeLog.Value.Date, listShift, actualShiftID, listMonthShifts,
                                            new Att_TAMScanLogEntity[] { listTamScanLogByShift.FirstOrDefault(), nextTamScanLog });

                                        if (timeLog.Value.Date == date.Date && listTest != null && listTest.Count() >= 2
                                            && !listTest.Any(d => d.TimeLog.HasValue && d.TimeLog.Value.Date < date.Date))
                                        {
                                            if (isFromShift2)
                                            {
                                                workday.InTime1 = timeLog;
                                                workday.OutTime1 = nextTamScanLog.TimeLog;
                                            }
                                            else
                                            {
                                                workday.InTime1 = timeLog;
                                                workday.OutTime1 = nextTamScanLog.TimeLog;
                                            }

                                            workday.FirstInTime = !workday.FirstInTime.HasValue || workday.FirstInTime > timeLog ? timeLog : workday.FirstInTime;
                                            workday.LastOutTime = !workday.LastOutTime.HasValue || workday.LastOutTime < nextTamScanLog.TimeLog ? nextTamScanLog.TimeLog : workday.LastOutTime;

                                            workday.Type = WorkdayType.E_WRONG_SHIFT.ToString();
                                            nextTamScanLog.Checked = true;
                                        }
                                        else
                                        {
                                            actualShiftID = null;
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            if (IsInTime(date, timeLog, shiftInfo))
                            {
                                if (isFromShift2)
                                {
                                    workday.InTime2 = timeLog;
                                }
                                else
                                {
                                    workday.InTime1 = timeLog;
                                }

                                workday.Type = WorkdayType.E_MISS_OUT.ToString();
                            }
                            else
                            {
                                if (isFromShift2)
                                {
                                    workday.OutTime2 = timeLog;
                                }
                                else
                                {
                                    workday.OutTime1 = timeLog;
                                }

                                workday.Type = WorkdayType.E_MISS_IN.ToString();
                            }
                        }
                    }

                    #endregion
                }
                else
                {
                    #region Trường hợp không có dòng quẹt thẻ thuộc ca đang xét

                    var listTamScanLogByDate = listTamScanLogByCardCodeUnchecked.Where(d =>
                        d.TimeLog.Value.Date == date).ToList();

                    var shiftInTime = date.Add(shiftInfo.InTime.TimeOfDay);
                    var listTimeLog = new List<Att_TAMScanLogEntity>();
                    bool isWrongShiftDetected = false;

                    foreach (var item in listTamScanLogByDate)
                    {
                        if (item.TimeLog < shiftInTime)
                        {
                            //Quẹt thẻ gần với thời gian vào của ca -> miss-out hoặc làm sai ca
                            //Tìm ca làm việc trước gần nhất và dòng quẹt thẻ trước gần nhất của nhân viên đang xét
                            var preShift = listMonthShifts.Where(d => d.Key >= date.AddDays(-1) && d.Key < date).OrderByDescending(d => d.Key).FirstOrDefault();
                            var preShiftValue = preShift.Value != null && preShift.Value.Count() > 0 ? preShift.Value.LastOrDefault() : null;

                            //Lấy dòng quẹt thẻ hiện tại và 2 dòng quẹt thẻ phía trước gần nhất của nhân viên đang xét để kiểm tra ca trước
                            var listPreTamScanLog = listTamScanLogByCardCode.Where(d => d.TimeLog <= item.TimeLog).OrderByDescending(d => d.TimeLog).Take(3).ToList();
                            var listTamScanLogByPreShift = GetTamScanLogByShift(preShift.Key, listShift, preShiftValue, listMonthShifts, listPreTamScanLog.ToArray());

                            //Nếu 2 dòng quẹt thẻ phía trước không thuộc một ca khác trong lịch của nhân viên
                            if (listTamScanLogByPreShift == null || listTamScanLogByPreShift.Count() < 2)
                            {
                                var preTamScanLog = listPreTamScanLog.Where(d => d.TimeLog < item.TimeLog).FirstOrDefault();
                                var currentShiftDuration = shiftInfo.MinIn + shiftInfo.CoOut + shiftInfo.MaxOut;

                                if (preTamScanLog != null && listWorkday.Any(d => d.ProfileID == profile.ID && (d.InTime1 == preTamScanLog.TimeLog
                                    || d.InTime2 == preTamScanLog.TimeLog || d.InTime3 == preTamScanLog.TimeLog || d.InTime4 == preTamScanLog.TimeLog
                                    || d.OutTime1 == preTamScanLog.TimeLog || d.OutTime2 == preTamScanLog.TimeLog || d.OutTime3 == preTamScanLog.TimeLog
                                    || d.OutTime4 == preTamScanLog.TimeLog)))
                                {
                                    //Trường hợp quẹt thẻ này đã sử dụng
                                    preTamScanLog = null;
                                }

                                //Nếu quẹt thẻ hiện tại kết với quẹt thẻ trước đó mà phù hợp duration thì ghép sai ca
                                if (preTamScanLog != null && preTamScanLog.TimeLog.HasValue && item.TimeLog.Value
                                    .Subtract(preTamScanLog.TimeLog.Value).TotalHours <= currentShiftDuration)
                                {
                                    //Trường hợp wrong-shift và detected-shift thì phải check lại actualShiftID
                                    actualShiftID = GetDetectedShiftID(preTamScanLog.TimeLog, item.TimeLog, listShift, null);

                                    var listTest = GetTamScanLogByShift(preTamScanLog.TimeLog.Value.Date, listShift, actualShiftID,
                                        listMonthShifts, new Att_TAMScanLogEntity[] { preTamScanLog, item });

                                    if (preTamScanLog.TimeLog.Value.Date == date.Date && listTest != null && listTest.Count() >= 2
                                        && listTest.Any(d => d.TimeLog.HasValue && d.TimeLog.Value.Date < date.Date))
                                    {
                                        if (isFromShift2)
                                        {
                                            workday.InTime2 = preTamScanLog.TimeLog;
                                            workday.OutTime2 = item.TimeLog;
                                        }
                                        else
                                        {
                                            workday.InTime1 = preTamScanLog.TimeLog;
                                            workday.OutTime1 = item.TimeLog;
                                        }

                                        workday.FirstInTime = !workday.FirstInTime.HasValue || workday.FirstInTime > preTamScanLog.TimeLog ? preTamScanLog.TimeLog : workday.FirstInTime;
                                        workday.LastOutTime = !workday.LastOutTime.HasValue || workday.LastOutTime < item.TimeLog ? item.TimeLog : workday.LastOutTime;

                                        workday.Type = WorkdayType.E_WRONG_SHIFT.ToString();
                                        preTamScanLog.Checked = true;
                                        isWrongShiftDetected = true;
                                        break;//chỉ hỗ trợ 1 ca
                                    }
                                    else
                                    {
                                        actualShiftID = null;
                                    }
                                }
                                else
                                {
                                    listTimeLog.Add(item);
                                }
                            }
                            else
                            {
                                if (!listTamScanLogByPreShift.Any(d => d.TimeLog == item.TimeLog))
                                {
                                    listTimeLog.Add(item);
                                }

                                listTimeLog = listTimeLog.Where(d => !listTamScanLogByPreShift.Contains(d)).ToList();
                            }
                        }
                        else if (item.TimeLog > shiftInTime)
                        {
                            //Quẹt thẻ gần với thời gian ra của ca -> miss-in hoặc làm sai ca
                            //Tìm ca làm việc tiếp theo gần nhất và dòng quẹt thẻ tiếp theo gần nhất của nhân viên đang xét
                            var nextShift = listMonthShifts.Where(d => d.Key > date && d.Key <= date.AddDays(1)).OrderBy(d => d.Key).FirstOrDefault();
                            var nextShiftValue = nextShift.Value != null && nextShift.Value.Count() > 0 ? nextShift.Value.FirstOrDefault() : null;

                            //Lấy dòng quẹt thẻ hiện tại và 2 dòng quẹt thẻ tiếp theo gần nhất của nhân viên đang xét để kiểm tra ca tiếp theo
                            var listNextTamScanLog = listTamScanLogByCardCodeUnchecked.Where(d => d.TimeLog >= item.TimeLog).OrderBy(d => d.TimeLog).Take(3).ToList();
                            var listTamScanLogByNextShift = GetTamScanLogByShift(nextShift.Key, listShift, nextShiftValue, listMonthShifts, listNextTamScanLog.ToArray());

                            //Nếu 2 dòng quẹt thẻ tiếp theo không thuộc một ca khác trong lịch của nhân viên
                            if (listTamScanLogByNextShift == null || listTamScanLogByNextShift.Count() < 2)
                            {
                                var nextTamScanLog = listNextTamScanLog.Where(d => d.TimeLog > item.TimeLog).FirstOrDefault();
                                var currentShiftDuration = shiftInfo.MinIn + shiftInfo.CoOut + shiftInfo.MaxOut;

                                //Nếu quẹt thẻ hiện tại kết với quẹt thẻ tiếp theo mà phù hợp duration thì ghép sai ca
                                if (nextTamScanLog != null && nextTamScanLog.TimeLog.HasValue && nextTamScanLog
                                    .TimeLog.Value.Subtract(item.TimeLog.Value).TotalHours <= currentShiftDuration)
                                {
                                    //Trường hợp wrong-shift và detected-shift thì phải check lại actualShiftID
                                    actualShiftID = GetDetectedShiftID(item.TimeLog, nextTamScanLog.TimeLog, listShift, null);

                                    var listTest = GetTamScanLogByShift(item.TimeLog.Value.Date, listShift, actualShiftID,
                                        listMonthShifts, new Att_TAMScanLogEntity[] { item, nextTamScanLog });

                                    if (item.TimeLog.Value.Date == date.Date && listTest != null && listTest.Count() >= 2
                                        && listTest.Any(d => d.TimeLog.HasValue && d.TimeLog.Value.Date < date.Date))
                                    {
                                        if (isFromShift2)
                                        {
                                            workday.InTime2 = item.TimeLog;
                                            workday.OutTime2 = nextTamScanLog.TimeLog;
                                        }
                                        else
                                        {
                                            workday.InTime1 = item.TimeLog;
                                            workday.OutTime1 = nextTamScanLog.TimeLog;
                                        }

                                        workday.FirstInTime = !workday.FirstInTime.HasValue || workday.FirstInTime > item.TimeLog ? item.TimeLog : workday.FirstInTime;
                                        workday.LastOutTime = !workday.LastOutTime.HasValue || workday.LastOutTime < nextTamScanLog.TimeLog ? nextTamScanLog.TimeLog : workday.LastOutTime;

                                        workday.Type = WorkdayType.E_WRONG_SHIFT.ToString();
                                        nextTamScanLog.Checked = true;
                                        isWrongShiftDetected = true;
                                        break;//chỉ hỗ trợ 1 ca
                                    }
                                    else
                                    {
                                        actualShiftID = null;
                                    }
                                }
                                else
                                {
                                    listTimeLog.Add(item);
                                }
                            }
                            else
                            {
                                if (!listTamScanLogByNextShift.Any(d => d.TimeLog == item.TimeLog))
                                {
                                    listTimeLog.Add(item);
                                }

                                listTimeLog = listTimeLog.Where(d => !listTamScanLogByNextShift.Contains(d)).ToList();
                            }
                        }
                    }

                    if (!isWrongShiftDetected)
                    {
                        var listRemove = new List<Att_TAMScanLogEntity>();

                        if (listTimeLog.Count() >= 2)
                        {
                            var currentShiftDuration = shiftInfo.MinIn + shiftInfo.CoOut + shiftInfo.MaxOut;
                            var timeLog1 = listTimeLog.OrderBy(d => d.TimeLog).FirstOrDefault();
                            var timeLog2 = listTimeLog.OrderBy(d => d.TimeLog).LastOrDefault();

                            if (isFromShift2)
                            {
                                workday.InTime2 = timeLog1.TimeLog;
                                workday.OutTime2 = timeLog2.TimeLog;
                            }
                            else
                            {
                                workday.InTime1 = timeLog1.TimeLog;
                                workday.OutTime1 = timeLog2.TimeLog;
                            }

                            workday.FirstInTime = !workday.FirstInTime.HasValue || workday.FirstInTime > timeLog1.TimeLog ? timeLog1.TimeLog : workday.FirstInTime;
                            workday.LastOutTime = !workday.LastOutTime.HasValue || workday.LastOutTime < timeLog2.TimeLog ? timeLog2.TimeLog : workday.LastOutTime;
                            isWrongShiftDetected = true;

                            if (timeLog2.TimeLog.HasValue && timeLog1.TimeLog.HasValue && timeLog2.TimeLog.Value
                                .Subtract(timeLog1.TimeLog.Value).TotalHours <= currentShiftDuration)
                            {
                                //Trường hợp wrong-shift và detected-shift thì phải check lại actualShiftID
                                actualShiftID = GetDetectedShiftID(timeLog1.TimeLog, timeLog2.TimeLog, listShift, null);

                                var listTest = GetTamScanLogByShift(timeLog1.TimeLog.Value.Date, listShift, actualShiftID,
                                    listMonthShifts, new Att_TAMScanLogEntity[] { timeLog1, timeLog2 });

                                if (listTest != null && listTest.Count() >= 2 && listTest.Any(d =>
                                    d.TimeLog.HasValue && d.TimeLog.Value.Date < date.Date))
                                {
                                    workday.Type = WorkdayType.E_WRONG_SHIFT.ToString();
                                }
                                else
                                {
                                    workday.Type = WorkdayType.E_LONGIN_SHIFT.ToString();
                                }
                            }
                            else
                            {
                                foreach (var item in listTimeLog.Where(d => d.TimeLog > timeLog1.TimeLog).OrderByDescending(d => d.TimeLog))
                                {
                                    if (isFromShift2)
                                    {
                                        workday.OutTime2 = item.TimeLog;
                                    }
                                    else
                                    {
                                        workday.OutTime1 = item.TimeLog;
                                    }

                                    workday.LastOutTime = !workday.LastOutTime.HasValue || workday.LastOutTime < item.TimeLog ? item.TimeLog : workday.LastOutTime;

                                    if (item.TimeLog > timeLog1.TimeLog && item.TimeLog.HasValue && timeLog1.TimeLog.HasValue
                                        && item.TimeLog.Value.Subtract(timeLog1.TimeLog.Value).TotalHours <= currentShiftDuration)
                                    {
                                        workday.Type = WorkdayType.E_DETECTED_SHIFT.ToString();
                                        break;
                                    }
                                    else
                                    {
                                        workday.Type = WorkdayType.E_LONGIN_SHIFT.ToString();
                                    }
                                }

                                listRemove = listTimeLog.Where(d => d.TimeLog > workday.LastOutTime).ToList();
                            }
                        }
                        else if (listTimeLog.Count() == 1)
                        {
                            if (listTimeLog.Any(d => d.TimeLog < shiftInTime))
                            {
                                var inTimeValue = listTimeLog.Select(d => d.TimeLog).FirstOrDefault();

                                if (isFromShift2)
                                {
                                    workday.InTime2 = inTimeValue;
                                }
                                else
                                {
                                    workday.InTime1 = inTimeValue;
                                }

                                workday.FirstInTime = !workday.FirstInTime.HasValue || workday.FirstInTime > inTimeValue ? inTimeValue : workday.FirstInTime;
                                workday.Type = WorkdayType.E_MISS_OUT.ToString();//miss-in hay miss-out cũng như nhau
                            }
                            else
                            {
                                var outTimeValue = listTimeLog.Select(d => d.TimeLog).FirstOrDefault();

                                if (isFromShift2)
                                {
                                    workday.OutTime2 = outTimeValue;
                                }
                                else
                                {
                                    workday.OutTime1 = outTimeValue;
                                }

                                workday.LastOutTime = !workday.LastOutTime.HasValue || workday.LastOutTime < outTimeValue ? outTimeValue : workday.LastOutTime;
                                workday.Type = WorkdayType.E_MISS_IN.ToString();//miss-in hay miss-out cũng như nhau
                            }

                            isWrongShiftDetected = true;
                        }

                        listTimeLog.Where(d => !listRemove.Contains(d)).ToList().ForEach(d => d.Checked = true);
                    }

                    if (!isWrongShiftDetected)
                    {
                        if (listHoliday.Any(d => d.DateOff.Date == date.Date))
                        {
                            //Có ca mà không inout thì xem có phải holiday
                            workday.Type = WorkdayType.E_HOLIDAY.ToString();
                        }
                        else
                        {
                            workday.Type = WorkdayType.E_MISS_IN_OUT.ToString();
                        }
                    }

                    #endregion
                }
            }
            else if (inOutConfig.TypeLoadData == TypeLoadData.E_TYPEINOUT.ToString())
            {
                //Xử lý cho các loại khác
            }
            return workday;
            #endregion
        }

        private void DeleteWorkday(Guid userID, DateTime dateFrom, DateTime dateTo,
            string workdaySrcType, string workdayStatus1, string workdayStatus2, params Guid[] listProfileID)
        {
            using (var context = new VnrHrmDataContext())
            {
                var unitOfWork = new UnitOfWork(context);
                int pageSize = 2000;//tối đa là 2100 parameter

                foreach (var listProfileIDBySize in listProfileID.Chunk(pageSize))
                {
                    var workdayQueryable = unitOfWork.CreateQueryable<Att_Workday>(userID, d => d.WorkDate >= dateFrom.Date && d.WorkDate <= dateTo
                        && (d.SrcType == null || d.SrcType != workdaySrcType) && (d.Status == null || (d.Status != workdayStatus1 && d.Status != workdayStatus2))
                        && (listProfileID.Contains(d.ProfileID) || (d.Hre_Profile.IsDelete.HasValue && d.Hre_Profile.IsDelete.Value)));

                    var result = unitOfWork.Delete(workdayQueryable);
                }
            }
        }

        private bool IsInTime(DateTime date, DateTime? timeLog, Cat_Shift shiftInfo)
        {
            bool result = true;

            if (timeLog.HasValue && shiftInfo != null)
            {
                DateTime inTime = date.Date.Add(shiftInfo.InTime.TimeOfDay);
                DateTime outTime = inTime.AddHours(shiftInfo.CoOut);

                double inMinutes = timeLog.Value.Subtract(inTime).TotalMinutes;
                double outMinutes = timeLog.Value.Subtract(outTime).TotalMinutes;

                result = VnResource.Helper.Data.DataHelper.GetPositiveDouble(inMinutes)
                    <= VnResource.Helper.Data.DataHelper.GetPositiveDouble(outMinutes);
            }

            return result;
        }

        private DateTime? FindTimeLogAvailable(List<Att_TAMScanLogEntity> listTamScanLog,
            DateTime? timeLog, double minMinutesSameAtt, bool isOutTime)
        {
            DateTime? timeLogAvailable = null;

            if (timeLog.HasValue && minMinutesSameAtt > 0)
            {
                if (isOutTime)
                {
                    timeLogAvailable = listTamScanLog.Where(d => d.TimeLog.HasValue && d.TimeLog > timeLog && d.TimeLog
                        <= timeLog.Value.AddMinutes(minMinutesSameAtt)).Select(d => d.TimeLog).OrderByDescending(d => d).FirstOrDefault();
                }
                else
                {
                    timeLogAvailable = listTamScanLog.Where(d => d.TimeLog.HasValue && d.TimeLog < timeLog && d.TimeLog
                        >= timeLog.Value.AddMinutes(-minMinutesSameAtt)).Select(d => d.TimeLog).OrderBy(d => d).FirstOrDefault();
                }
            }

            return timeLogAvailable.HasValue ? timeLogAvailable : timeLog;
        }

        private List<Att_TAMScanLogEntity> RemoveSameTimeLog(List<Att_TAMScanLogEntity> listTamScanLog, double minMinutesSameAtt)
        {
            List<Att_TAMScanLogEntity> result = listTamScanLog;

            if (listTamScanLog != null && minMinutesSameAtt > 0)
            {
                var listTimeLogGroup = listTamScanLog.GroupBy(d => d.ProfileID).ToList();
                result = new List<Att_TAMScanLogEntity>();//khởi tạo lại danh sách này

                foreach (var groupCard in listTimeLogGroup)
                {
                    List<Att_TAMScanLogEntity> listTamScanLogRemove = new List<Att_TAMScanLogEntity>();

                    foreach (var timeLog in groupCard.OrderBy(d => d.TimeLog))
                    {
                        if (!listTamScanLogRemove.Any(s => s.ID == timeLog.ID))
                        {
                            listTamScanLogRemove.AddRange(groupCard.Where(d => d.ID != timeLog.ID && d.TimeLog >= timeLog.TimeLog
                                && d.TimeLog.Value.Subtract(timeLog.TimeLog.Value).TotalMinutes <= minMinutesSameAtt).ToList());
                        }
                    }

                    //Loại bỏ những dòng quẹt thẻ liền kề nhau trong khoảng thời gian (minMinutesSameAtt) như cấu hình
                    result.AddRange(groupCard.Where(d => !listTamScanLogRemove.Any(s => s.ID == d.ID)).ToList());
                }
            }

            return result;
        }

        /// <summary>
        /// Kiểm tra dữ liệu quẹt theo theo ngày - shift
        /// </summary>
        /// <param name="date"></param>
        /// <param name="shiftID"></param>
        /// <param name="listShift"></param>
        /// <param name="listTamScanLog"></param>
        /// <returns></returns>
        private List<Att_TAMScanLogEntity> GetTamScanLogByShift(DateTime date, List<Cat_Shift> listShift, Guid? shiftID,
            Dictionary<DateTime, List<Guid?>> listMonthShifts, params Att_TAMScanLogEntity[] listTamScanLog)
        {
            List<Att_TAMScanLogEntity> listTamScanLogByShift = null;

            if (listShift != null && shiftID.HasValue && shiftID != Guid.Empty)
            {
                var shiftInfo = listShift.Where(d => d.ID == shiftID).FirstOrDefault();

                if (shiftInfo != null)
                {
                    TimeSpan firstInTime = shiftInfo.InTime.TimeOfDay;
                    TimeSpan lastInTime = shiftInfo.InTime.TimeOfDay;
                    double firstMinIn = shiftInfo.MinIn;
                    double lastMaxOut = shiftInfo.MaxOut;
                    double lastCoOut = shiftInfo.CoOut;

                    if (shiftInfo.IsDoubleShift.GetBoolean())
                    {
                        var firstShift = listShift.Where(d => d.ID == shiftInfo.FirstShiftID).FirstOrDefault();
                        var lastShift = listShift.Where(d => d.ID == shiftInfo.LastShiftID).FirstOrDefault();

                        firstInTime = firstShift != null ? firstShift.InTime.TimeOfDay : lastInTime;
                        lastInTime = lastShift != null ? lastShift.InTime.TimeOfDay : lastInTime;
                        firstMinIn = firstShift != null ? firstShift.MinIn : firstMinIn;
                        lastMaxOut = lastShift != null ? lastShift.MaxOut : lastMaxOut;
                        lastCoOut = lastShift != null ? lastShift.CoOut : lastCoOut;
                    }

                    DateTime maxDateExtent = date.AddDays(1).AddSeconds(-1);
                    DateTime maxShift = date.Add(lastInTime).AddHours(lastCoOut + lastMaxOut);
                    maxDateExtent = maxDateExtent > maxShift ? maxDateExtent : maxShift;

                    if (listMonthShifts != null && listMonthShifts.ContainsKey(date.AddDays(1)))
                    {
                        var nextShiftID = listMonthShifts[date.AddDays(1)].FirstOrDefault();
                        var nextShiftInfo = listShift.Where(d => d.ID == nextShiftID).FirstOrDefault();

                        if (nextShiftInfo != null)
                        {
                            TimeSpan nextShiftInTime = nextShiftInfo.InTime.TimeOfDay;
                            DateTime nextShiftInTimeMin = date.AddDays(1).Add(nextShiftInTime);
                            nextShiftInTimeMin = nextShiftInTimeMin.AddHours(-nextShiftInfo.MinIn);
                            maxDateExtent = maxDateExtent > nextShiftInTimeMin ? nextShiftInTimeMin : maxDateExtent;
                        }
                    }

                    var listTamScanLogByMaxDateExtent = listTamScanLog.Where(d =>
                        d.TimeLog >= date && d.TimeLog <= maxDateExtent).ToList();

                    listTamScanLogByShift = listTamScanLogByMaxDateExtent.Where(d => d.TimeLog >= date.Add(firstInTime).AddHours(-firstMinIn)
                        && d.TimeLog <= date.Add(lastInTime).AddHours(lastCoOut + lastMaxOut)).OrderBy(d => d.TimeLog).ToList();
                }
            }

            return listTamScanLogByShift;
        }

        private Guid? GetDetectedShiftID(DateTime? inTime, DateTime? outTime,
            List<Cat_Shift> listShift, Dictionary<DateTime, List<Guid?>> listMonthShifts)
        {
            Guid? result = null;
            double validMinutes = 30;

            inTime = inTime ?? outTime;
            outTime = outTime ?? inTime;

            if (inTime.HasValue && outTime.HasValue)
            {
                List<Cat_Shift> listShiftDetected = new List<Cat_Shift>();

                foreach (var shiftInfo in listShift)
                {
                    List<Att_TAMScanLogEntity> listTamScanLogByDate = new List<Att_TAMScanLogEntity>();
                    listTamScanLogByDate.Add(new Att_TAMScanLogEntity { TimeLog = inTime });
                    listTamScanLogByDate.Add(new Att_TAMScanLogEntity { TimeLog = outTime });

                    var listTamScanLogByShift = GetTamScanLogByShift(inTime.Value.Date, listShift,
                        shiftInfo.ID, listMonthShifts, listTamScanLogByDate.ToArray());

                    if (listTamScanLogByShift.Count() >= 2)
                    {
                        listShiftDetected.Add(shiftInfo);
                    }
                }

                if (listShiftDetected.Count() == 1)
                {
                    result = listShiftDetected.Select(d => d.ID).FirstOrDefault();
                }
                else if (listShiftDetected.Count() >= 2)
                {
                    var listShiftInfo = listShiftDetected.Select(d => new
                    {
                        d.ID,
                        d.InTime,
                        OutTime = d.InTime.AddHours(d.CoOut),
                        EarlyInMinutes = d.InTime.TimeOfDay.Subtract(inTime.Value.TimeOfDay).TotalMinutes,
                        LateOutMinutes = outTime.Value.TimeOfDay.Subtract(d.InTime.AddHours(d.CoOut).TimeOfDay).TotalMinutes
                    }).ToList();

                    var listEarlyInTime = listShiftInfo.Where(d => d.EarlyInMinutes >= 0
                        && d.EarlyInMinutes <= validMinutes).OrderBy(d => d.InTime).ToList();

                    if (listEarlyInTime.Count() == 1)
                    {
                        //Ưu tiên đi sớm trong khoảng thời gian cho phép so với ca (sát ca)
                        result = listEarlyInTime.Select(d => d.ID).FirstOrDefault();
                    }
                    else if (listEarlyInTime.Count() >= 2)
                    {
                        //Ưu tiên về muộn và muộn ít nhất để sát với ca nhất
                        result = listEarlyInTime.Where(d => d.LateOutMinutes >= 0).OrderBy(d =>
                            d.LateOutMinutes).Select(d => d.ID).FirstOrDefault();

                        if (result == null || result == Guid.Empty)
                        {
                            //Tiếp theo thì ưu tiên về sớm (LateOutMinutes < 0) trong khoảng thời gian cho phép so với ca
                            result = listEarlyInTime.Where(d => VnResource.Helper.Data.DataHelper.GetPositiveDouble(d.LateOutMinutes)
                                <= VnResource.Helper.Data.DataHelper.GetPositiveDouble(validMinutes)).OrderBy(d =>
                                    d.LateOutMinutes).Select(d => d.ID).LastOrDefault();
                        }
                    }
                    else
                    {
                        var listLateOutTime = listShiftInfo.Where(d => d.LateOutMinutes >= 0
                            && d.LateOutMinutes <= validMinutes).OrderBy(d => d.InTime).ToList();

                        if (listLateOutTime.Count() == 1)
                        {
                            //Ưu tiên về muộn trong khoảng thời gian cho phép so với ca (sát ca)
                            result = listLateOutTime.Select(d => d.ID).FirstOrDefault();
                        }
                        else if (listLateOutTime.Count() >= 2)
                        {
                            //Ưu tiên về muộn và muộn ít nhất để sát với ca nhất
                            result = listEarlyInTime.Where(d => d.EarlyInMinutes >= 0).OrderBy(d =>
                                d.EarlyInMinutes).Select(d => d.ID).FirstOrDefault();

                            if (result == null || result == Guid.Empty)
                            {
                                //Tiếp theo thì ưu tiên đi trễ (EarlyInMinutes < 0) trong khoảng thời gian cho phép so với ca
                                result = listLateOutTime.Where(d => VnResource.Helper.Data.DataHelper.GetPositiveDouble(d.EarlyInMinutes)
                                    <= VnResource.Helper.Data.DataHelper.GetPositiveDouble(validMinutes)).OrderBy(d =>
                                        d.EarlyInMinutes).Select(d => d.ID).LastOrDefault();
                            }
                        }
                    }

                    if (result == null || result == Guid.Empty)
                    {
                        //shift1 là đi trễ it nhất (EarlyInMinutes < 0), shift2 là đi sớm ít nhất (EarlyInMinutes > 0)
                        var shift1 = listShiftInfo.Where(d => d.EarlyInMinutes < 0).OrderBy(d => d.EarlyInMinutes).LastOrDefault();
                        var shift2 = listShiftInfo.Where(d => d.EarlyInMinutes > 0).OrderBy(d => d.EarlyInMinutes).FirstOrDefault();

                        if (shift1 != null && shift2 != null)
                        {
                            if (VnResource.Helper.Data.DataHelper.GetPositiveDouble(shift1.EarlyInMinutes)
                                < VnResource.Helper.Data.DataHelper.GetPositiveDouble(shift2.EarlyInMinutes))
                            {
                                result = shift1.ID;
                            }
                            else
                            {
                                result = shift2.ID;
                            }
                        }
                        else if (shift1 != null)
                        {
                            result = shift1.ID;
                        }
                        else if (shift2 != null)
                        {
                            result = shift2.ID;
                        }
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
