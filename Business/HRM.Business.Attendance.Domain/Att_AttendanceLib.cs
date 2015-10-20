using HRM.Business.Attendance.Models;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using HRM.Business.Hr.Domain;
using HRM.Business.Hr.Models;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Data.Entity;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Domain
{
    public class Att_AttendanceLib
    {
        private static Guid _ProfileCurrentID;
        private static Guid ProfileCurrentID
        {
            get
            {
                return _ProfileCurrentID;
            }
            set
            {
                _ProfileCurrentID = value;
            }
        }
        private static double _Num_TOTAL_LEAVE_BY_TYPE_IN_MONTH;
        private static double Num_TOTAL_LEAVE_BY_TYPE_IN_MONTH
        {
            get
            {
                return _Num_TOTAL_LEAVE_BY_TYPE_IN_MONTH;
            }
            set
            {
                _Num_TOTAL_LEAVE_BY_TYPE_IN_MONTH = value;
            }
        }
        private static List<string> _lstCodeLeaveNonAnl;
        private static List<string> lstCodeLeaveNonAnl
        {
            get
            {
                if (_lstCodeLeaveNonAnl != null)
                {
                    return _lstCodeLeaveNonAnl;
                }
                else
                {
                    BaseService _baseService = new BaseService();
                    string status = string.Empty;
                    var key = AppConfig.HRM_ATT_ANNUALDETAIL_LEAVECODENOTCOMPUTE.ToString();
                    var lstConfig = _baseService.GetData<Sys_AllSettingEntity>(key, ConstantSql.hrm_sys_sp_get_AllSettingByKey, string.Empty, ref status);
                    List<string> lstResult = new List<string>();

                    if (lstConfig.Count > 0)
                    {
                        string codeArr = lstConfig.FirstOrDefault().Value1;
                        if (!string.IsNullOrEmpty(codeArr))
                        {
                            char[] ext = new char[] { ',' };
                            lstResult = codeArr.Split(ext, StringSplitOptions.RemoveEmptyEntries).ToList();
                        }
                    }
                    _lstCodeLeaveNonAnl = lstResult;
                    return _lstCodeLeaveNonAnl;
                }
            }
        }

        private static List<Cat_JobTitle> _lstCatJobTitle;
        private static List<Cat_JobTitle> lstCatJobTitle
        {
            get
            {
                if (_lstCatJobTitle != null)
                {
                    return _lstCatJobTitle;
                }
                else
                {
                    string status = string.Empty;
                    var _baseService = new Cat_ShiftServices();
                    var lstConfig = _baseService.GetAllUseEntity<Cat_JobTitle>(ref status);
                    List<Cat_JobTitle> lstResult = new List<Cat_JobTitle>();

                    if (lstConfig.Count > 0)
                    {
                        lstResult = lstConfig;
                    }
                    _lstCatJobTitle = lstResult;
                    return _lstCatJobTitle;
                }
            }
        }

        private static List<Cat_Position> _lstCatPosition;
        private static List<Cat_Position> lstCatPosition
        {
            get
            {
                if (_lstCatPosition != null)
                {
                    return _lstCatPosition;
                }
                else
                {
                    string status = string.Empty;
                    var _baseService = new Cat_ShiftServices();
                    var lstConfig = _baseService.GetAllUseEntity<Cat_Position>(ref status);
                    List<Cat_Position> lstResult = new List<Cat_Position>();

                    if (lstConfig.Count > 0)
                    {
                        lstResult = lstConfig;
                    }
                    _lstCatPosition = lstResult;
                    return _lstCatPosition;
                }
            }
        }

        #region RoundLateEarly

        public static int RoundLateEarlyMinutes(WorkDay workday, List<Cat_LateEarlyRule> listLateEarlyRule,
            List<Att_LeaveDay> listLeaveDay, int minLate, int minEarly)
        {
            int total = 0;
            if (workday != null)
            {
                DateTime workDate = workday.WorkDate;
                string e_FULLSHIFT = LeaveDayDurationType.E_FULLSHIFT.ToString();
                string e_FIRSTHALFSHIFT = LeaveDayDurationType.E_FIRSTHALFSHIFT.ToString();
                string e_LASTHALFSHIFT = LeaveDayDurationType.E_LASTHALFSHIFT.ToString();
                string e_MIDDLEOFSHIFT = LeaveDayDurationType.E_MIDDLEOFSHIFT.ToString();

                List<Att_LeaveDay> listLeaveDayFull = listLeaveDay.Where(d => (d.DurationType == null || d.DurationType == e_FULLSHIFT)
                    && d.DateStart.Date <= workDate && d.DateEnd.Date >= workDate).ToList();

                if (listLeaveDayFull.Count > 0)
                {
                    return total;
                }

                List<Att_LeaveDay> listLeaveDayFirst = listLeaveDay.Where(d => (d.DurationType == e_FIRSTHALFSHIFT)
                    && d.DateStart.Date <= workDate && d.DateEnd.Date >= workDate).ToList();

                if (listLeaveDayFirst.Count > 0)
                {
                    minLate = 0;
                }

                List<Att_LeaveDay> listLeaveDayLast = listLeaveDay.Where(d => (d.DurationType == e_LASTHALFSHIFT)
                    && d.DateStart.Date <= workDate && d.DateEnd.Date >= workDate).ToList();

                if (listLeaveDayLast.Count > 0)
                {
                    minEarly = 0;
                }

                Double minutesDuration = 0;
                List<Att_LeaveDay> listLeaveDayMidd = listLeaveDay.Where(d => d.DurationType == e_MIDDLEOFSHIFT
                    && d.DateStart.Date <= workDate && d.DateEnd.Date >= workDate).ToList();

                if (listLeaveDayMidd.Count > 0)
                {
                    for (int i = 0; i < listLeaveDayMidd.Count; i++)
                    {
                        Att_LeaveDay leaveDayTemp = listLeaveDayMidd[i];
                        minutesDuration += leaveDayTemp.LeaveHours.Value * 60;
                        int minDur = Convert.ToInt32(minutesDuration);

                        if (leaveDayTemp.DateStart <= workday.ShiftBreakInTime && leaveDayTemp.DateEnd >= workday.ShiftInTime)
                        {
                            DateTime timeStart = leaveDayTemp.DateStart <= workday.ShiftInTime ? workday.ShiftInTime.Value : leaveDayTemp.DateStart;
                            DateTime timeEnd = leaveDayTemp.DateEnd >= workday.ShiftBreakInTime ? workday.ShiftBreakInTime.Value : leaveDayTemp.DateEnd;
                            Double time = timeEnd.Subtract(timeStart).TotalMinutes;

                            if (Convert.ToInt32(time) > minLate)
                            {
                                minLate = 0;
                            }
                            else
                            {
                                minLate -= Convert.ToInt32(time);
                            }
                        }

                        if (leaveDayTemp.DateStart <= workday.ShiftOutTime && leaveDayTemp.DateEnd >= workday.ShiftBreakOutTime)
                        {
                            DateTime timeStart = leaveDayTemp.DateStart <= workday.ShiftBreakOutTime ? workday.ShiftBreakOutTime.Value : leaveDayTemp.DateStart;
                            DateTime timeEnd = leaveDayTemp.DateEnd >= workday.ShiftOutTime ? workday.ShiftOutTime.Value : leaveDayTemp.DateEnd;
                            Double time = timeEnd.Subtract(timeStart).TotalMinutes;

                            if (Convert.ToInt32(time) > minEarly)
                            {
                                minEarly = 0;
                            }
                            else
                            {
                                minEarly -= Convert.ToInt32(time);
                            }
                        }
                    }
                }
            }

            int minRoundLate = 0;
            int minRoundEarly = 0;

            Cat_LateEarlyRule roundRuleLate = listLateEarlyRule.Where(d => d.MinValue <= minLate
                && (!d.MaxValue.HasValue || d.MaxValue >= minLate)).FirstOrDefault();

            if (roundRuleLate != null)
            {
                minRoundLate = Convert.ToInt32(roundRuleLate.RoundValue);
            }

            Cat_LateEarlyRule roundRuleEarly = listLateEarlyRule.Where(d => d.MinValue <= minEarly
                && (!d.MaxValue.HasValue || d.MaxValue >= minEarly)).FirstOrDefault();

            if (roundRuleEarly != null)
            {
                minRoundEarly = Convert.ToInt32(roundRuleEarly.RoundValue);
            }

            total = (minRoundEarly + minRoundLate);
            return total;
        }

        public static int RoundLateEarlyMinutes(List<Cat_LateEarlyRule> listLateEarlyRule, int minutes)
        {
            var lateEarlyRule = listLateEarlyRule.Where(d => d.MinValue <= minutes
                && (!d.MaxValue.HasValue || d.MaxValue >= minutes)).FirstOrDefault();

            return lateEarlyRule != null ? (int)lateEarlyRule.RoundValue : minutes;
        }

        #endregion

        #region GetDailyLines

        /// <summary>
        /// Kiểm tra chuyền làm việc hàng ngày của nhân viên
        /// </summary>
        /// <param name="profile">Nhân viên cần kiểm tra</param>
        /// <param name="listRoster">Lịch làm việc của nhân viên</param>
        /// <param name="dateFrom">Khoảng thời gian cần kiểm tra</param>
        /// <param name="dateTo">Khoảng thời gian cần kiểm tra</param>
        /// <returns></returns>
        public static Dictionary<DateTime, Cat_OrgStructure> GetDailyLines(Hre_Profile profile,
            List<Att_Roster> listRoster, DateTime dateFrom, DateTime dateTo)
        {
            Dictionary<DateTime, Cat_OrgStructure> listOrgStructure = new Dictionary<DateTime, Cat_OrgStructure>();

            if (listRoster != null && listRoster.Count() > 0 && profile != null)
            {
                listRoster = listRoster.Where(d => d != null && d.ProfileID == profile.ID
                    && d.Status == RosterStatus.E_APPROVED.ToString()).ToList();
            }

            if (listRoster != null)
            {
                #region Data roster

                List<Att_Roster> listRosterDefault = listRoster.Where(d => d.Type == RosterType.E_DEFAULT.ToString()).ToList();
                List<Att_Roster> listRosterChanged = listRoster.Where(d => d.Type == RosterType.E_CHANGE_SHIFT.ToString()).ToList();
                List<Att_Roster> listRosterOff = listRoster.Where(d => d.Type == RosterType.E_TIME_OFF.ToString()).ToList();

                #endregion

                #region Default roster

                foreach (Att_Roster roster in listRosterDefault)
                {
                    //Đối với những roster là lịch làm việc mặc định => Ưu tiên sau cùng
                    if (roster.Type == RosterType.E_DEFAULT.ToString())
                    {
                        DateTime dateStart = dateFrom;
                        DateTime dateEnd = dateTo;

                        if (roster.DateStart != null && roster.DateStart > dateFrom)
                        {
                            dateStart = roster.DateStart;
                        }

                        if (roster.DateEnd != null && roster.DateEnd < dateTo)
                        {
                            dateEnd = roster.DateEnd;
                        }

                        for (DateTime date = dateStart.Date; date <= dateEnd; date = date.AddDays(1))
                        {
                            if (roster.Cat_OrgStructure != null)
                            {
                                if (!listOrgStructure.ContainsKey(date))
                                {
                                    listOrgStructure.Add(date, roster.Cat_OrgStructure);
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Changed roster

                foreach (Att_Roster roster in listRosterChanged)
                {
                    if (roster.Type == RosterType.E_CHANGE_SHIFT.ToString())
                    {
                        DateTime dateStart = dateFrom;
                        DateTime dateEnd = dateTo;

                        if (roster.DateStart != null && roster.DateStart > dateFrom)
                        {
                            dateStart = roster.DateStart;
                        }

                        if (roster.DateEnd != null && roster.DateEnd < dateTo)
                        {
                            dateEnd = roster.DateEnd;
                        }

                        //Đối với những roster là lịch làm việc sửa đổi => Ưu tiên hơn loại default
                        for (DateTime date = dateStart.Date; date <= dateEnd; date = date.AddDays(1))
                        {
                            if (roster.Cat_OrgStructure != null)
                            {
                                if (listOrgStructure.ContainsKey(date))
                                {
                                    listOrgStructure[date] = roster.Cat_OrgStructure;
                                }
                                else
                                {
                                    listOrgStructure.Add(date, roster.Cat_OrgStructure);
                                }
                            }
                        }
                    }
                }

                #endregion

                #region TimeOff roster

                foreach (Att_Roster roster in listRosterOff)
                {
                    if (roster.Type == RosterType.E_TIME_OFF.ToString())
                    {
                        DateTime dateStart = dateFrom;
                        DateTime dateEnd = dateTo;

                        if (roster.DateStart != null && roster.DateStart > dateFrom)
                        {
                            dateStart = roster.DateStart;
                        }

                        if (roster.DateEnd != null && roster.DateEnd < dateTo)
                        {
                            dateEnd = roster.DateEnd;
                        }

                        //Đối với những roster là lịch được nghỉ làm => không sử dụng, ưu tiên nhất
                        for (DateTime date = dateStart.Date; date <= dateEnd; date = date.AddDays(1))
                        {
                            if (listOrgStructure.ContainsKey(date))
                            {
                                listOrgStructure.Remove(date);
                            }
                        }
                    }
                }

                #endregion
            }

            return listOrgStructure;
        }

        #endregion

        #region GetDailyShifts

        /// <summary>
        /// Kiểm tra ca làm việc hàng ngày của nhân viên theo roster.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="listRoster"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="listDailyShift"></param>
        public static Dictionary<DateTime, Guid?> GetDailyShifts(Guid profileID, DateTime dateFrom,
            DateTime dateTo, List<Att_RosterEntity> listRoster, List<Att_RosterGroupEntity> listRosterGroup)
        {
            Dictionary<DateTime, Guid?> listDailyShift = new Dictionary<DateTime, Guid?>();

            if (profileID != Guid.Empty && listRoster != null && listRoster.Count() > 0)
            {
                #region Shift theo roster

                listRoster = listRoster.Where(d => d != null && d.ProfileID == profileID
                    && d.Status == RosterStatus.E_APPROVED.ToString()).ToList();

                string typeRosterGroup = RosterType.E_ROSTERGROUP.ToString();
                var listRosterTypeGroup = listRoster.Where(d => d.Type == typeRosterGroup).ToList();

                foreach (var roster in listRoster)
                {
                    DateTime dateStart = dateFrom;
                    DateTime dateEnd = dateTo;

                    if (roster.DateStart != null && roster.DateStart > dateFrom)
                    {
                        dateStart = roster.DateStart;
                    }

                    if (roster.DateEnd != null && roster.DateEnd < dateTo)
                    {
                        dateEnd = roster.DateEnd;
                    }

                    for (DateTime date = dateStart.Date; date <= dateEnd; date = date.AddDays(1))
                    {
                        if (roster.Type == RosterType.E_TIME_OFF.ToString())
                        {
                            if (listDailyShift.ContainsKey(date))
                            {
                                //Ngày nghỉ không dùng
                                listDailyShift[date] = null;
                            }
                            else
                            {
                                listDailyShift.Add(date, null);
                            }
                        }
                        else if (roster.Type == RosterType.E_CHANGE_SHIFT.ToString())
                        {
                            if (listDailyShift.ContainsKey(date))
                            {
                                //Null khi time-off đã có trước
                                if (listDailyShift[date] != null)
                                {
                                    //Ưu tiên ca làm việc được thay đổi
                                    listDailyShift[date] = roster.MonShiftID;
                                }
                            }
                            else
                            {
                                listDailyShift.Add(date, roster.MonShiftID);
                            }
                        }

                        else if (roster.Type == RosterType.E_DEFAULT.ToString())
                        {
                            if (!listDailyShift.ContainsKey(date))
                            {
                                if (date.DayOfWeek == DayOfWeek.Monday && roster.MonShiftID != null)
                                {
                                    listDailyShift.Add(date, roster.MonShiftID);
                                }
                                else if (date.DayOfWeek == DayOfWeek.Tuesday && roster.TueShiftID != null)
                                {
                                    listDailyShift.Add(date, roster.TueShiftID);
                                }
                                else if (date.DayOfWeek == DayOfWeek.Wednesday && roster.WedShiftID != null)
                                {
                                    listDailyShift.Add(date, roster.WedShiftID);
                                }
                                else if (date.DayOfWeek == DayOfWeek.Thursday && roster.ThuShiftID != null)
                                {
                                    listDailyShift.Add(date, roster.ThuShiftID);
                                }
                                else if (date.DayOfWeek == DayOfWeek.Friday && roster.FriShiftID != null)
                                {
                                    listDailyShift.Add(date, roster.FriShiftID);
                                }
                                else if (date.DayOfWeek == DayOfWeek.Saturday && roster.SatShiftID != null)
                                {
                                    listDailyShift.Add(date, roster.SatShiftID);
                                }
                                else if (date.DayOfWeek == DayOfWeek.Sunday && roster.SunShiftID != null)
                                {
                                    listDailyShift.Add(date, roster.SunShiftID);
                                }
                            }
                        }
                    }
                }

                #endregion

                if (listRosterTypeGroup != null && listRosterTypeGroup.Count() > 0)
                {
                    #region triet.mai Loai E_ROSTERGROUP loại Đăng ký tăng ca theo nhóm

                    //Logic khá phức tạp 
                    //1. Độ ưu tiên thì đứng sau Att_Roster (loại khác Vd: dateOff, ChangeShift)
                    //2. tìm cái roster của từng ngày và kiêm tra cai tên của RosterGroupName >> Chạy qua bảng Att_RosterGroup để tìm Shift

                    var lstRosterTypeGroup_ByProfile = listRosterTypeGroup.Where(m => m.ProfileID == profileID).ToList();
                    var lstRoster_Type_RosterGroup = lstRosterTypeGroup_ByProfile.OrderByDescending(m => m.DateStart).ToList();
                    bool isContinue = true;//Dung de chay nguoc cac roster lay cai moi nhat va ko chay nua => tang toc;

                    foreach (var roster in lstRoster_Type_RosterGroup)
                    {
                        if (!isContinue)
                        {
                            continue;
                        }

                        if (roster.DateStart <= dateFrom)
                        {
                            isContinue = false;
                        }

                        DateTime start = dateFrom < roster.DateStart ? roster.DateStart : dateFrom;
                        DateTime end = dateTo;

                        if (roster.Type == RosterType.E_ROSTERGROUP.ToString())
                        {
                            for (DateTime date = start; date <= end; date = date.AddDays(1))
                            {
                                if (!listDailyShift.ContainsKey(date))
                                {
                                    var rosterGroup = listRosterGroup.Where(m => m.RosterGroupName == roster.RosterGroupName
                                        && m.DateStart <= date && m.DateEnd >= date).FirstOrDefault();

                                    if (rosterGroup != null)
                                    {
                                        if (date.DayOfWeek == DayOfWeek.Monday && rosterGroup.MonShiftID != null)
                                        {
                                            listDailyShift.Add(date, rosterGroup.MonShiftID);
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Tuesday && rosterGroup.TueShiftID != null)
                                        {
                                            listDailyShift.Add(date, rosterGroup.TueShiftID);
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Wednesday && rosterGroup.WedShiftID != null)
                                        {
                                            listDailyShift.Add(date, rosterGroup.WedShiftID);
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Thursday && rosterGroup.ThuShiftID != null)
                                        {
                                            listDailyShift.Add(date, rosterGroup.ThuShiftID);
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Friday && rosterGroup.FriShiftID != null)
                                        {
                                            listDailyShift.Add(date, rosterGroup.FriShiftID);
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Saturday && rosterGroup.SatShiftID != null)
                                        {
                                            listDailyShift.Add(date, rosterGroup.SatShiftID);
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Sunday && rosterGroup.SunShiftID != null)
                                        {
                                            listDailyShift.Add(date, rosterGroup.SunShiftID);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion
                }
            }

            return listDailyShift;
        }

        public static Dictionary<DateTime, Cat_Shift> GetDailyShifts(Guid profileID, DateTime dateFrom, DateTime dateTo,
            List<Att_RosterEntity> listRoster, List<Att_RosterGroupEntity> listRosterGroup, List<Cat_Shift> listShift)
        {
            Dictionary<DateTime, Cat_Shift> listDailyShift = new Dictionary<DateTime, Cat_Shift>();

            if (profileID != Guid.Empty && listRoster != null && listRoster.Count() > 0)
            {
                #region Shift theo roster

                listRoster = listRoster.Where(d => d != null && d.ProfileID == profileID
                    && d.Status == RosterStatus.E_APPROVED.ToString()).ToList();

                string typeRosterGroup = RosterType.E_ROSTERGROUP.ToString();
                var listRosterTypeGroup = listRoster.Where(d => d.Type == typeRosterGroup).ToList();

                foreach (var roster in listRoster)
                {
                    DateTime dateStart = dateFrom;
                    DateTime dateEnd = dateTo;

                    if (roster.DateStart != null && roster.DateStart > dateFrom)
                    {
                        dateStart = roster.DateStart;
                    }

                    if (roster.DateEnd != null && roster.DateEnd < dateTo)
                    {
                        dateEnd = roster.DateEnd;
                    }

                    for (DateTime date = dateStart.Date; date <= dateEnd; date = date.AddDays(1))
                    {
                        if (roster.Type == RosterType.E_TIME_OFF.ToString())
                        {
                            if (listDailyShift.ContainsKey(date))
                            {
                                //Ngày nghỉ không dùng
                                listDailyShift[date] = null;
                            }
                            else
                            {
                                listDailyShift.Add(date, null);
                            }
                        }
                        else if (roster.Type == RosterType.E_CHANGE_SHIFT.ToString())
                        {
                            if (listDailyShift.ContainsKey(date))
                            {
                                //Null khi time-off đã có trước
                                if (listDailyShift[date] != null)
                                {
                                    //Ưu tiên ca làm việc được thay đổi
                                    listDailyShift[date] = listShift.FirstOrDefault(d => d.ID == roster.MonShiftID);
                                }
                            }
                            else
                            {
                                listDailyShift.Add(date, listShift.FirstOrDefault(d => d.ID == roster.MonShiftID));
                            }
                        }

                        else if (roster.Type == RosterType.E_DEFAULT.ToString())
                        {
                            if (!listDailyShift.ContainsKey(date))
                            {
                                if (date.DayOfWeek == DayOfWeek.Monday && roster.MonShiftID != null)
                                {
                                    listDailyShift.Add(date, listShift.FirstOrDefault(d => d.ID == roster.MonShiftID));
                                }
                                else if (date.DayOfWeek == DayOfWeek.Tuesday && roster.TueShiftID != null)
                                {
                                    listDailyShift.Add(date, listShift.FirstOrDefault(d => d.ID == roster.TueShiftID));
                                }
                                else if (date.DayOfWeek == DayOfWeek.Wednesday && roster.WedShiftID != null)
                                {
                                    listDailyShift.Add(date, listShift.FirstOrDefault(d => d.ID == roster.WedShiftID));
                                }
                                else if (date.DayOfWeek == DayOfWeek.Thursday && roster.ThuShiftID != null)
                                {
                                    listDailyShift.Add(date, listShift.FirstOrDefault(d => d.ID == roster.ThuShiftID));
                                }
                                else if (date.DayOfWeek == DayOfWeek.Friday && roster.FriShiftID != null)
                                {
                                    listDailyShift.Add(date, listShift.FirstOrDefault(d => d.ID == roster.FriShiftID));
                                }
                                else if (date.DayOfWeek == DayOfWeek.Saturday && roster.SatShiftID != null)
                                {
                                    listDailyShift.Add(date, listShift.FirstOrDefault(d => d.ID == roster.SatShiftID));
                                }
                                else if (date.DayOfWeek == DayOfWeek.Sunday && roster.SunShiftID != null)
                                {
                                    listDailyShift.Add(date, listShift.FirstOrDefault(d => d.ID == roster.SunShiftID));
                                }
                            }
                        }
                    }
                }

                #endregion

                if (listRosterTypeGroup != null && listRosterTypeGroup.Count() > 0)
                {
                    #region triet.mai Loai E_ROSTERGROUP loại Đăng ký tăng ca theo nhóm

                    //Logic khá phức tạp 
                    //1. Độ ưu tiên thì đứng sau Att_Roster (loại khác Vd: dateOff, ChangeShift)
                    //2. tìm cái roster của từng ngày và kiêm tra cai tên của RosterGroupName >> Chạy qua bảng Att_RosterGroup để tìm Shift

                    var lstRosterTypeGroup_ByProfile = listRosterTypeGroup.Where(m => m.ProfileID == profileID).ToList();
                    var lstRoster_Type_RosterGroup = lstRosterTypeGroup_ByProfile.OrderByDescending(m => m.DateStart).ToList();
                    bool isContinue = true;//Dung de chay nguoc cac roster lay cai moi nhat va ko chay nua => tang toc;

                    foreach (var roster in lstRoster_Type_RosterGroup)
                    {
                        if (!isContinue)
                        {
                            continue;
                        }

                        if (roster.DateStart <= dateFrom)
                        {
                            isContinue = false;
                        }

                        DateTime start = dateFrom < roster.DateStart ? roster.DateStart : dateFrom;
                        DateTime end = dateTo;

                        if (roster.Type == RosterType.E_ROSTERGROUP.ToString())
                        {
                            for (DateTime date = start; date <= end; date = date.AddDays(1))
                            {
                                if (!listDailyShift.ContainsKey(date))
                                {
                                    var rosterGroup = listRosterGroup.Where(m => m.RosterGroupName == roster.RosterGroupName
                                        && m.DateStart <= date && m.DateEnd >= date).FirstOrDefault();

                                    if (rosterGroup != null)
                                    {
                                        if (date.DayOfWeek == DayOfWeek.Monday && rosterGroup.MonShiftID != null)
                                        {
                                            listDailyShift.Add(date, listShift.FirstOrDefault(d => d.ID == rosterGroup.MonShiftID));
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Tuesday && rosterGroup.TueShiftID != null)
                                        {
                                            listDailyShift.Add(date, listShift.FirstOrDefault(d => d.ID == rosterGroup.TueShiftID));
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Wednesday && rosterGroup.WedShiftID != null)
                                        {
                                            listDailyShift.Add(date, listShift.FirstOrDefault(d => d.ID == rosterGroup.WedShiftID));
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Thursday && rosterGroup.ThuShiftID != null)
                                        {
                                            listDailyShift.Add(date, listShift.FirstOrDefault(d => d.ID == rosterGroup.ThuShiftID));
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Friday && rosterGroup.FriShiftID != null)
                                        {
                                            listDailyShift.Add(date, listShift.FirstOrDefault(d => d.ID == rosterGroup.FriShiftID));
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Saturday && rosterGroup.SatShiftID != null)
                                        {
                                            listDailyShift.Add(date, listShift.FirstOrDefault(d => d.ID == rosterGroup.SatShiftID));
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Sunday && rosterGroup.SunShiftID != null)
                                        {
                                            listDailyShift.Add(date, listShift.FirstOrDefault(d => d.ID == rosterGroup.SunShiftID));
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #endregion
                }
            }

            return listDailyShift;
        }

        public static Dictionary<DateTime, List<Guid?>> GetDailyShifts(DateTime dateFrom, DateTime dateTo,
            Guid profileID, List<Att_RosterEntity> listRoster, List<Att_RosterGroupEntity> listRosterGroup)
        {
            Dictionary<DateTime, List<Guid?>> listDailyShift = new Dictionary<DateTime, List<Guid?>>();

            if (profileID != Guid.Empty && listRoster != null && listRoster.Count() > 0)
            {
                #region Shift theo roster

                listRoster = listRoster.Where(d => d != null && d.ProfileID == profileID
                    && d.Status == RosterStatus.E_APPROVED.ToString()).ToList();

                foreach (var roster in listRoster)
                {
                    DateTime dateStart = dateFrom;
                    DateTime dateEnd = dateTo;

                    if (roster.DateStart != null && roster.DateStart > dateFrom)
                    {
                        dateStart = roster.DateStart;
                    }

                    if (roster.DateEnd != null && roster.DateEnd < dateTo)
                    {
                        dateEnd = roster.DateEnd;
                    }

                    for (DateTime date = dateStart.Date; date <= dateEnd; date = date.AddDays(1))
                    {
                        List<Guid?> listShiftIDByDate = new List<Guid?>();

                        if (roster.Type == RosterType.E_TIME_OFF.ToString())
                        {
                            if (listDailyShift.ContainsKey(date))
                            {
                                //Ngày nghỉ không sử dụng
                                listDailyShift[date] = null;
                            }
                            else
                            {
                                listDailyShift.Add(date, null);
                            }
                        }
                        else if (roster.Type == RosterType.E_CHANGE_SHIFT.ToString())
                        {
                            if (listDailyShift.ContainsKey(date))
                            {
                                //Null khi time-off đã có trước
                                if (listDailyShift[date] != null
                                    && listDailyShift[date].Count() > 0)
                                {
                                    listShiftIDByDate.Add(roster.MonShiftID);
                                    listShiftIDByDate.Add(roster.MonShift2ID);

                                    //Ưu tiên ca làm việc được thay đổi
                                    listDailyShift[date] = listShiftIDByDate;
                                }
                            }
                            else
                            {
                                listShiftIDByDate.Add(roster.MonShiftID);
                                listShiftIDByDate.Add(roster.MonShift2ID);
                                listDailyShift.Add(date, listShiftIDByDate);
                            }
                        }
                        else if (roster.Type == RosterType.E_DEFAULT.ToString())
                        {
                            if (!listDailyShift.ContainsKey(date))
                            {
                                if (date.DayOfWeek == DayOfWeek.Monday)
                                {
                                    if (roster.MonShiftID.HasValue)
                                    {
                                        listShiftIDByDate.Add(roster.MonShiftID);
                                    }
                                    if (roster.MonShift2ID.HasValue)
                                    {
                                        listShiftIDByDate.Add(roster.MonShift2ID);
                                    }
                                }
                                else if (date.DayOfWeek == DayOfWeek.Tuesday)
                                {
                                    if (roster.TueShiftID.HasValue)
                                    {
                                        listShiftIDByDate.Add(roster.TueShiftID);
                                    }
                                    if (roster.TueShift2ID.HasValue)
                                    {
                                        listShiftIDByDate.Add(roster.TueShift2ID);
                                    }
                                }
                                else if (date.DayOfWeek == DayOfWeek.Wednesday)
                                {
                                    if (roster.WedShiftID.HasValue)
                                    {
                                        listShiftIDByDate.Add(roster.WedShiftID);
                                    }
                                    if (roster.WedShift2ID.HasValue)
                                    {
                                        listShiftIDByDate.Add(roster.WedShift2ID);
                                    }
                                }
                                else if (date.DayOfWeek == DayOfWeek.Thursday)
                                {
                                    if (roster.ThuShiftID.HasValue)
                                    {
                                        listShiftIDByDate.Add(roster.ThuShiftID);
                                    }
                                    if (roster.ThuShift2ID.HasValue)
                                    {
                                        listShiftIDByDate.Add(roster.ThuShift2ID);
                                    }
                                }
                                else if (date.DayOfWeek == DayOfWeek.Friday)
                                {
                                    if (roster.FriShiftID.HasValue)
                                    {
                                        listShiftIDByDate.Add(roster.FriShiftID);
                                    }
                                    if (roster.FriShift2ID.HasValue)
                                    {
                                        listShiftIDByDate.Add(roster.FriShift2ID);
                                    }
                                }
                                else if (date.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    if (roster.SatShiftID.HasValue)
                                    {
                                        listShiftIDByDate.Add(roster.SatShiftID);
                                    }
                                    if (roster.SatShift2ID.HasValue)
                                    {
                                        listShiftIDByDate.Add(roster.SatShift2ID);
                                    }
                                }
                                else if (date.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    if (roster.SunShiftID.HasValue)
                                    {
                                        listShiftIDByDate.Add(roster.SunShiftID);
                                    }
                                    if (roster.SunShift2ID.HasValue)
                                    {
                                        listShiftIDByDate.Add(roster.SunShift2ID);
                                    }
                                }

                                if (listShiftIDByDate.Count() > 0)
                                {
                                    listDailyShift.Add(date, listShiftIDByDate);
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Shift theo roster group

                string typeRosterGroup = RosterType.E_ROSTERGROUP.ToString();
                var listRosterTypeGroup = listRoster.Where(d => d.Type == typeRosterGroup).ToList();

                if (listRosterTypeGroup != null && listRosterTypeGroup.Count() > 0)
                {
                    //Logic khá phức tạp 
                    //1. Độ ưu tiên thì đứng sau Att_Roster (loại khác Vd: dateOff, ChangeShift)
                    //2. tìm cái roster của từng ngày và kiêm tra cai tên của RosterGroupName >> Chạy qua bảng Att_RosterGroup để tìm Shift

                    var lstRosterTypeGroup_ByProfile = listRosterTypeGroup.Where(m => m.ProfileID == profileID).ToList();
                    var lstRoster_Type_RosterGroup = lstRosterTypeGroup_ByProfile.OrderByDescending(m => m.DateStart).ToList();
                    bool isContinue = true;//Dung de chay nguoc cac roster lay cai moi nhat va ko chay nua => tang toc;

                    foreach (var roster in lstRoster_Type_RosterGroup)
                    {
                        if (!isContinue)
                        {
                            continue;
                        }

                        if (roster.DateStart <= dateFrom)
                        {
                            isContinue = false;
                        }

                        RosterType type = (RosterType)Common.GetEnumValue(typeof(RosterType), roster.Type);
                        DateTime start = dateFrom < roster.DateStart ? roster.DateStart : dateFrom;
                        DateTime end = dateTo;

                        if (roster.Type == RosterType.E_ROSTERGROUP.ToString())
                        {
                            for (DateTime date = start; date <= end; date = date.AddDays(1))
                            {
                                if (!listDailyShift.ContainsKey(date))
                                {
                                    List<Guid?> listShiftIDByDate = new List<Guid?>();

                                    var rosterGroup = listRosterGroup.Where(m => m.DateStart <= date && m.DateEnd >= date
                                        && m.RosterGroupName == roster.RosterGroupName).FirstOrDefault();

                                    if (rosterGroup != null)
                                    {
                                        if (date.DayOfWeek == DayOfWeek.Monday)
                                        {
                                            if (rosterGroup.MonShiftID.HasValue)
                                            {
                                                listShiftIDByDate.Add(rosterGroup.MonShiftID);
                                            }
                                            if (rosterGroup.MonShift2ID.HasValue)
                                            {
                                                listShiftIDByDate.Add(rosterGroup.MonShift2ID);
                                            }
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Tuesday)
                                        {
                                            if (rosterGroup.TueShiftID.HasValue)
                                            {
                                                listShiftIDByDate.Add(rosterGroup.TueShiftID);
                                            }
                                            if (rosterGroup.TueShift2ID.HasValue)
                                            {
                                                listShiftIDByDate.Add(rosterGroup.TueShift2ID);
                                            }
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Wednesday)
                                        {
                                            if (rosterGroup.WedShiftID.HasValue)
                                            {
                                                listShiftIDByDate.Add(rosterGroup.WedShiftID);
                                            }
                                            if (rosterGroup.WedShift2ID.HasValue)
                                            {
                                                listShiftIDByDate.Add(rosterGroup.WedShift2ID);
                                            }
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Thursday)
                                        {
                                            if (rosterGroup.ThuShiftID.HasValue)
                                            {
                                                listShiftIDByDate.Add(rosterGroup.ThuShiftID);
                                            }
                                            if (rosterGroup.ThuShift2ID.HasValue)
                                            {
                                                listShiftIDByDate.Add(rosterGroup.ThuShift2ID);
                                            }
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Friday)
                                        {
                                            if (rosterGroup.FriShiftID.HasValue)
                                            {
                                                listShiftIDByDate.Add(rosterGroup.FriShiftID);
                                            }
                                            if (rosterGroup.FriShift2ID.HasValue)
                                            {
                                                listShiftIDByDate.Add(rosterGroup.FriShift2ID);
                                            }
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Saturday)
                                        {
                                            if (rosterGroup.SatShiftID.HasValue)
                                            {
                                                listShiftIDByDate.Add(rosterGroup.SatShiftID);
                                            }
                                            if (rosterGroup.SatShift2ID.HasValue)
                                            {
                                                listShiftIDByDate.Add(rosterGroup.SatShift2ID);
                                            }
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Sunday)
                                        {
                                            if (rosterGroup.SunShiftID.HasValue)
                                            {
                                                listShiftIDByDate.Add(rosterGroup.SunShiftID);
                                            }
                                            if (rosterGroup.SunShift2ID.HasValue)
                                            {
                                                listShiftIDByDate.Add(rosterGroup.SunShift2ID);
                                            }
                                        }
                                    }

                                    if (listShiftIDByDate.Count() > 0)
                                    {
                                        listDailyShift.Add(date, listShiftIDByDate);
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion
            }

            return listDailyShift;
        }

        public static Dictionary<DateTime, List<Cat_Shift>> GetDailyShifts(DateTime dateFrom, DateTime dateTo, Guid profileID,
            List<Att_RosterEntity> listRoster, List<Att_RosterGroupEntity> listRosterGroup, List<Cat_Shift> listShift)
        {
            Dictionary<DateTime, List<Cat_Shift>> listDailyShift = new Dictionary<DateTime, List<Cat_Shift>>();

            if (profileID != Guid.Empty && listRoster != null && listRoster.Count() > 0)
            {
                #region Shift theo roster

                listRoster = listRoster.Where(d => d != null && d.ProfileID == profileID
                    && d.Status == RosterStatus.E_APPROVED.ToString()).ToList();

                foreach (var roster in listRoster)
                {
                    DateTime dateStart = dateFrom;
                    DateTime dateEnd = dateTo;

                    if (roster.DateStart != null && roster.DateStart > dateFrom)
                    {
                        dateStart = roster.DateStart;
                    }

                    if (roster.DateEnd != null && roster.DateEnd < dateTo)
                    {
                        dateEnd = roster.DateEnd;
                    }

                    for (DateTime date = dateStart.Date; date <= dateEnd; date = date.AddDays(1))
                    {
                        List<Cat_Shift> listShiftIDByDate = new List<Cat_Shift>();

                        if (roster.Type == RosterType.E_TIME_OFF.ToString())
                        {
                            if (listDailyShift.ContainsKey(date))
                            {
                                //Ngày nghỉ không sử dụng
                                listDailyShift[date] = null;
                            }
                            else
                            {
                                listDailyShift.Add(date, null);
                            }
                        }
                        else if (roster.Type == RosterType.E_CHANGE_SHIFT.ToString())
                        {
                            if (listDailyShift.ContainsKey(date))
                            {
                                //Null khi time-off đã có trước
                                if (listDailyShift[date] != null
                                    && listDailyShift[date].Count() > 0)
                                {
                                    listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == roster.MonShiftID));
                                    listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == roster.MonShift2ID));

                                    //Ưu tiên ca làm việc được thay đổi
                                    listDailyShift[date] = listShiftIDByDate;
                                }
                            }
                            else
                            {
                                listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == roster.MonShiftID));
                                listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == roster.MonShift2ID));
                                listDailyShift.Add(date, listShiftIDByDate);
                            }
                        }
                        else if (roster.Type == RosterType.E_DEFAULT.ToString())
                        {
                            if (!listDailyShift.ContainsKey(date))
                            {
                                if (date.DayOfWeek == DayOfWeek.Monday)
                                {
                                    if (roster.MonShiftID.HasValue)
                                    {
                                        listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == roster.MonShiftID));
                                    }
                                    if (roster.MonShift2ID.HasValue)
                                    {
                                        listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == roster.MonShift2ID));
                                    }
                                }
                                else if (date.DayOfWeek == DayOfWeek.Tuesday)
                                {
                                    if (roster.TueShiftID.HasValue)
                                    {
                                        listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == roster.TueShiftID));
                                    }
                                    if (roster.TueShift2ID.HasValue)
                                    {
                                        listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == roster.TueShift2ID));
                                    }
                                }
                                else if (date.DayOfWeek == DayOfWeek.Wednesday)
                                {
                                    if (roster.WedShiftID.HasValue)
                                    {
                                        listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == roster.WedShiftID));
                                    }
                                    if (roster.WedShift2ID.HasValue)
                                    {
                                        listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == roster.WedShift2ID));
                                    }
                                }
                                else if (date.DayOfWeek == DayOfWeek.Thursday)
                                {
                                    if (roster.ThuShiftID.HasValue)
                                    {
                                        listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == roster.ThuShiftID));
                                    }
                                    if (roster.ThuShift2ID.HasValue)
                                    {
                                        listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == roster.ThuShift2ID));
                                    }
                                }
                                else if (date.DayOfWeek == DayOfWeek.Friday)
                                {
                                    if (roster.FriShiftID.HasValue)
                                    {
                                        listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == roster.FriShiftID));
                                    }
                                    if (roster.FriShift2ID.HasValue)
                                    {
                                        listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == roster.FriShift2ID));
                                    }
                                }
                                else if (date.DayOfWeek == DayOfWeek.Saturday)
                                {
                                    if (roster.SatShiftID.HasValue)
                                    {
                                        listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == roster.SatShiftID));
                                    }
                                    if (roster.SatShift2ID.HasValue)
                                    {
                                        listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == roster.SatShift2ID));
                                    }
                                }
                                else if (date.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    if (roster.SunShiftID.HasValue)
                                    {
                                        listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == roster.SunShiftID));
                                    }
                                    if (roster.SunShift2ID.HasValue)
                                    {
                                        listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == roster.SunShift2ID));
                                    }
                                }

                                if (listShiftIDByDate.Count() > 0)
                                {
                                    listDailyShift.Add(date, listShiftIDByDate);
                                }
                            }
                        }
                    }
                }

                #endregion

                #region Shift theo roster group

                string typeRosterGroup = RosterType.E_ROSTERGROUP.ToString();
                var listRosterTypeGroup = listRoster.Where(d => d.Type == typeRosterGroup).ToList();

                if (listRosterTypeGroup != null && listRosterTypeGroup.Count() > 0)
                {
                    //Logic khá phức tạp 
                    //1. Độ ưu tiên thì đứng sau Att_Roster (loại khác Vd: dateOff, ChangeShift)
                    //2. tìm cái roster của từng ngày và kiêm tra cai tên của RosterGroupName >> Chạy qua bảng Att_RosterGroup để tìm Shift

                    var lstRosterTypeGroup_ByProfile = listRosterTypeGroup.Where(m => m.ProfileID == profileID).ToList();
                    var lstRoster_Type_RosterGroup = lstRosterTypeGroup_ByProfile.OrderByDescending(m => m.DateStart).ToList();
                    bool isContinue = true;//Dung de chay nguoc cac roster lay cai moi nhat va ko chay nua => tang toc;

                    foreach (var roster in lstRoster_Type_RosterGroup)
                    {
                        if (!isContinue)
                        {
                            continue;
                        }

                        if (roster.DateStart <= dateFrom)
                        {
                            isContinue = false;
                        }

                        RosterType type = (RosterType)Common.GetEnumValue(typeof(RosterType), roster.Type);
                        DateTime start = dateFrom < roster.DateStart ? roster.DateStart : dateFrom;
                        DateTime end = dateTo;

                        if (roster.Type == RosterType.E_ROSTERGROUP.ToString())
                        {
                            for (DateTime date = start; date <= end; date = date.AddDays(1))
                            {
                                if (!listDailyShift.ContainsKey(date))
                                {
                                    List<Cat_Shift> listShiftIDByDate = new List<Cat_Shift>();

                                    var rosterGroup = listRosterGroup.Where(m => m.DateStart <= date && m.DateEnd >= date
                                        && m.RosterGroupName == roster.RosterGroupName).FirstOrDefault();

                                    if (rosterGroup != null)
                                    {
                                        if (date.DayOfWeek == DayOfWeek.Monday)
                                        {
                                            if (rosterGroup.MonShiftID.HasValue)
                                            {
                                                listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == rosterGroup.MonShiftID));
                                            }
                                            if (rosterGroup.MonShift2ID.HasValue)
                                            {
                                                listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == rosterGroup.MonShift2ID));
                                            }
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Tuesday)
                                        {
                                            if (rosterGroup.TueShiftID.HasValue)
                                            {
                                                listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == rosterGroup.TueShiftID));
                                            }
                                            if (rosterGroup.TueShift2ID.HasValue)
                                            {
                                                listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == rosterGroup.TueShift2ID));
                                            }
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Wednesday)
                                        {
                                            if (rosterGroup.WedShiftID.HasValue)
                                            {
                                                listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == rosterGroup.WedShiftID));
                                            }
                                            if (rosterGroup.WedShift2ID.HasValue)
                                            {
                                                listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == rosterGroup.WedShift2ID));
                                            }
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Thursday)
                                        {
                                            if (rosterGroup.ThuShiftID.HasValue)
                                            {
                                                listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == rosterGroup.ThuShiftID));
                                            }
                                            if (rosterGroup.ThuShift2ID.HasValue)
                                            {
                                                listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == rosterGroup.ThuShift2ID));
                                            }
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Friday)
                                        {
                                            if (rosterGroup.FriShiftID.HasValue)
                                            {
                                                listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == rosterGroup.FriShiftID));
                                            }
                                            if (rosterGroup.FriShift2ID.HasValue)
                                            {
                                                listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == rosterGroup.FriShift2ID));
                                            }
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Saturday)
                                        {
                                            if (rosterGroup.SatShiftID.HasValue)
                                            {
                                                listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == rosterGroup.SatShiftID));
                                            }
                                            if (rosterGroup.SatShift2ID.HasValue)
                                            {
                                                listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == rosterGroup.SatShift2ID));
                                            }
                                        }
                                        else if (date.DayOfWeek == DayOfWeek.Sunday)
                                        {
                                            if (rosterGroup.SunShiftID.HasValue)
                                            {
                                                listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == rosterGroup.SunShiftID));
                                            }
                                            if (rosterGroup.SunShift2ID.HasValue)
                                            {
                                                listShiftIDByDate.Add(listShift.FirstOrDefault(d => d.ID == rosterGroup.SunShift2ID));
                                            }
                                        }
                                    }

                                    if (listShiftIDByDate.Count() > 0)
                                    {
                                        listDailyShift.Add(date, listShiftIDByDate);
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion
            }

            return listDailyShift;
        }

        #endregion

        #region GetAnnualBySeniority
        /// <summary>
        /// Tính số ngày phép năm được cộng thêm do thâm niên theo quy định.
        /// </summary>
        /// <param name="monthYearEval"></param>
        /// <param name="dateHire"></param>
        /// <param name="gradeCfg"></param>
        /// <returns></returns>
        public static double GetAnnualBySeniority(DateTime monthYearEval, DateTime? dateHire, Cat_GradeAttendance gradeCfg)
        {
            return GetAnnualBySeniority(monthYearEval, DateTime.MinValue, dateHire, gradeCfg);
        }

        /// <summary>
        /// Tính số ngày phép năm được cộng thêm do thâm niên theo quy định.
        /// </summary>
        /// <param name="monthYearEval"></param>
        /// <param name="monthYearEffect"></param>
        /// <param name="dateHire"></param>
        /// <param name="gradeCfg"></param>
        /// <returns></returns>
        public static double GetAnnualBySeniority(DateTime monthYearEval, DateTime monthYearEffect, DateTime? dateHire, Cat_GradeAttendance gradeCfg)
        {
            EnumDropDown.OptionReceive optionReceive = EnumDropDown.OptionReceive.E_FULLYEAR;

            if (gradeCfg != null && !string.IsNullOrWhiteSpace(gradeCfg.OptionReceive))
            {
                if (!Enum.TryParse<EnumDropDown.OptionReceive>(gradeCfg.OptionReceive, out optionReceive))
                {
                    optionReceive = EnumDropDown.OptionReceive.E_FULLYEAR;
                }
            }

            return GetAnnualBySeniority(monthYearEval, monthYearEffect, dateHire,
                optionReceive, gradeCfg.Seniority, gradeCfg.TotalDayAnnualLeaveOnYear);
        }

        /// <summary>
        /// Tính số ngày phép năm được cộng thêm do thâm niên theo quy định.
        /// </summary>
        /// <param name="monthYearEval">Tháng tính</param>
        /// <param name="monthYearEffect">Ngày áp dụng quy định</param>
        /// <param name="dateHire">Ngày vào làm của một nhân viên</param>
        /// <param name="optionReceive"></param>
        /// <param name="seniorityConfig">Số năm thâm niên tối thiểu</param>
        /// <param name="totalDayAnnualLeaveOnYear">Tổng số ngày phép năm</param>
        /// <returns></returns>
        public static double GetAnnualBySeniority(DateTime monthYearEval, DateTime monthYearEffect, DateTime? dateHire,
            EnumDropDown.OptionReceive optionReceive, double? seniorityConfig, double? totalDayAnnualLeaveOnYear)
        {
            double result = 0D;

            if (dateHire.HasValue && seniorityConfig.HasValue)
            {
                dateHire = dateHire.Value.Date.AddDays(1 - dateHire.Value.Day);
                monthYearEffect = monthYearEffect.Date.AddDays(1 - monthYearEffect.Day);
                dateHire = dateHire.Value < monthYearEffect ? monthYearEffect : dateHire;
                monthYearEval = monthYearEval.Date.AddDays(1 - monthYearEval.Day);

                int seniority = Convert.ToInt16(seniorityConfig.Value);
                DateTime seniorityYear = dateHire.Value.AddYears(seniority);

                if (optionReceive == EnumDropDown.OptionReceive.E_FULLMONTH)
                {
                    while (seniorityYear <= monthYearEval)
                    {
                        seniorityYear = seniorityYear.AddYears(seniority);
                        result++;
                    }
                }
                else if (optionReceive == EnumDropDown.OptionReceive.E_FULLYEAR
                    || optionReceive == EnumDropDown.OptionReceive.E_RATEBYMONTH)
                {
                    while (seniorityYear.Year <= monthYearEval.Year)
                    {
                        seniorityYear = seniorityYear.AddYears(seniority);
                        result++;
                    }

                    if (optionReceive == EnumDropDown.OptionReceive.E_RATEBYMONTH)
                    {
                        if (seniorityYear.Month > 1 && seniorityYear.Year == monthYearEval.Year)
                        {
                            totalDayAnnualLeaveOnYear = totalDayAnnualLeaveOnYear.HasValue ? totalDayAnnualLeaveOnYear.Value : 0;
                            double rateSeniority = (1 / totalDayAnnualLeaveOnYear.Value) * (12 - dateHire.Value.Month);
                            result += Math.Round(rateSeniority, 1);
                        }
                    }
                }
                else if (optionReceive == EnumDropDown.OptionReceive.E_NEXTYEAR)
                {
                    //Trường hợp đặc biệt nếu nhân viên vào làm tháng 1 thì tính qua năm sau.
                    while ((seniorityYear.Year == monthYearEval.Year && seniorityYear.Month == 1)
                        || seniorityYear.Year < monthYearEval.Year)
                    {
                        seniorityYear = seniorityYear.AddYears(seniority);
                        result++;
                    }
                }
            }

            return result;
        }

        #endregion

        #region GetAnnualLeaveReceive

        /// <summary>
        /// Tính số ngày phép năm tích lũy được đến thời điểm currentMonth.
        /// </summary>
        /// <param name="currentMonth">Tháng đang xét</param>
        /// <param name="gradeCfg"></param>
        /// <param name="dateHire">Ngày vào làm của nhân viên.</param>
        /// <param name="dateEndProbation">Ngày kết thúc thử việc</param>
        /// <param name="dateQuit">Ngày nghỉ làm của nhân viên</param>
        /// <param name="monthStartAnnualLeave">Tháng bắt đầu tính phép năm.</param>
        /// <param name="initAnnualValue">Phép năm có sẵn từ trước</param>
        /// <param name="lstLeaveDay">DS nghỉ phép theo loại trong cấu hình từ đầu năm tới giờ</param>
        /// <returns></returns>
        public static Double GetAnnualLeaveReceive(int Year, DateTime currentMonth, Cat_GradeAttendance gradeCfg, DateTime? dateHire,
            DateTime? dateEndProbation, DateTime? dateQuit, int? monthStartAnnualLeave, double? initAnnualValue, Cat_Position pos, Hre_ProfileMultiField profile
            , List<Att_LeaveDayInfo> lstLeaveDay, List<Sys_AllSetting> lstAllSetting, List<Hre_HDTJob> lstHDTJob_ByProfile, List<DateTime> lstDayOff, List<Att_LeaveDayInfo> lstLeaveDayAllYear,string userLogin)
        {
            double result = 0;
            int Month = currentMonth.Month;
            Cat_JobTitle CatJobTitle = new Cat_JobTitle();
            if (profile.JobTitleID != null && profile.JobTitleID != Guid.Empty)
            {
                CatJobTitle = lstCatJobTitle.Where(s => s.ID == profile.JobTitleID).FirstOrDefault();
            }

            if (gradeCfg == null)
            {
                return result;
            }

            if (string.IsNullOrWhiteSpace(gradeCfg.FormulaAnnualLeave))
            {
                throw new Exception("Formula Annual Leave not found.");
            }

            //Lấy khoảng thời gian của kỳ lương
            DateTime dtStart, dtEnd;
            Att_AttendanceServices.GetSalaryDateRange(gradeCfg, null, null, currentMonth, out dtStart, out dtEnd);

            double totalDayAnnualLeaveOnYear = gradeCfg.TotalDayAnnualLeaveOnYear.Get_Integer();
            double seniority = GetAnnualBySeniority(currentMonth, dateHire, gradeCfg);
            string formulaAnnualLeave = gradeCfg.FormulaAnnualLeave;
            Formula formula = new Formula(formulaAnnualLeave);

            //đâsdasd



            #region abc
            #region Param
            lstHDTJob_ByProfile = lstHDTJob_ByProfile.Where(m => m.DateFrom != null && m.Type != null).OrderBy(m => m.DateFrom).ToList();
            ParamGetConfigANL paramConfig = new ParamGetConfigANL();
            //set du lieu
            GetConfigANL(lstAllSetting, out paramConfig);
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
                DateCheckByMonth = new DateTime(Year, Month, 1);
                if (Month < monthBeginYear)
                {
                    DateCheckByMonth = new DateTime(Year + 1, Month, 1);
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
                DateTime beginMonthOfDateTime = new DateTime(DateStartProfile.Value.Year, DateStartProfile.Value.Month, dayBeginFullMonth);
                bool IsAdd1Month = false;
                for (DateTime dateCheck = beginMonthOfDateTime; dateCheck < DateStartProfile.Value.Date; dateCheck = dateCheck.AddDays(1))
                {
                    if (!lstDayOff.Any(m => m == dateCheck))
                    {
                        IsAdd1Month = true;
                        break;
                    }
                }
                if (IsAdd1Month)
                {
                    DateStartProfile = new DateTime(DateStartProfile.Value.AddMonths(1).Year, DateStartProfile.Value.AddMonths(1).Month, 1);
                }
                else
                {
                    DateStartProfile = new DateTime(DateStartProfile.Value.Year, DateStartProfile.Value.Month, dayBeginFullMonth);

                }
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
            if (DateCheckByMonth == null)
            {
                #region ANL and Leave
                if (formulaAnnualLeave.Contains(Formula.FormulaConstant.ANL_NORMAL.ToString()))
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
                if (formulaAnnualLeave.Contains(Formula.FormulaConstant.ANL_SENIOR.ToString()))
                {
                    double value = 0;
                    int MonBegin = monthInYearSenior == 0 ? 1 : monthInYearSenior;
                    //--DateStartProfile
                    DateTime Monthyear = new DateTime(Year, MonBegin, 1);

                    double dateLeave = lstLeaveDayAllYear.Where(m => m.TotalDuration != null).Sum(m => m.TotalDuration.Value);
                    dateLeave += (lstLeaveDayAllYear.Where(m => m.TotalDuration == null).Sum(m => m.Duration) / 8);

                    DateTime dateRoundProfileStart = DateStartProfile.Value.AddMonths(-monthRoundUp);

                    double Days = (Monthyear - dateRoundProfileStart).TotalDays - dateLeave;
                    double YearSenior = Math.Round(Days / 365, MidpointRounding.AwayFromZero);
                    int level = (int)(YearSenior / (seniorMonth / 12));
                    value = level * anlSeniorMoreThanNormal * 12;
                    formula.Parameters.Add(Formula.FormulaConstant.ANL_SENIOR.ToString(), value);
                }
                if (formulaAnnualLeave.Contains(Formula.FormulaConstant.ANL_LEAVE_NON_HAVEANL.ToString()))
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
                            numLeave += item.LeaveDays ?? 0;
                        }

                    }
                    value = ((int)(numLeave / dayPerMonth)) + ((numLeave % dayPerMonth) >= anlRoundUp ? 1 : 0);
                    formula.Parameters.Add(Formula.FormulaConstant.ANL_LEAVE_NON_HAVEANL.ToString(), value);
                }
                if (formulaAnnualLeave.Contains(Formula.FormulaConstant.ANL_WORK_HDT4.ToString()))
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
                    monthCount = ((int)(dayCount / dayPerMonth)) + (((dayCount % dayPerMonth) / dayPerMonth) >= anlRoundUp ? 1 : 0);
                    value = monthCount * anlHDT4MoreThanNormal;
                    formula.Parameters.Add(Formula.FormulaConstant.ANL_WORK_HDT4.ToString(), value);
                }
                if (formulaAnnualLeave.Contains(Formula.FormulaConstant.ANL_WORK_HDT5.ToString()))
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

                    monthCount = ((int)(dayCount / dayPerMonth)) + (((dayCount % dayPerMonth) / dayPerMonth) >= anlRoundUp ? 1 : 0);
                    value = monthCount * anlHDT5MoreThanNormal;
                    formula.Parameters.Add(Formula.FormulaConstant.ANL_WORK_HDT5.ToString(), value);
                }
                #endregion
            }
            else
            {
                #region BHXH
                if (formulaAnnualLeave.Contains(Formula.FormulaConstant.ANL_NORMAL.ToString()))
                {
                    double value = 0;
                    if (DateStartInYear <= DateCheckByMonth)
                    {
                        value = anlFullYear;
                    }
                    formula.Parameters.Add(Formula.FormulaConstant.ANL_NORMAL.ToString(), value);
                }
                if (formulaAnnualLeave.Contains(Formula.FormulaConstant.ANL_SENIOR.ToString()))
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
                if (formulaAnnualLeave.Contains(Formula.FormulaConstant.ANL_WORK_HDT4.ToString()))
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
                if (formulaAnnualLeave.Contains(Formula.FormulaConstant.ANL_WORK_HDT5.ToString()))
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
            #endregion


            double AnnualDays = 0;
            if (profile != null && CatJobTitle != null)
            {
                AnnualDays = CatJobTitle.AnnualDays ?? 0;
            }

            if (formulaAnnualLeave.Contains(Formula.FormulaConstant.INS_PROBATION.ToString()))
            {
                DateTime midDate = new DateTime(currentMonth.Year, currentMonth.Month, 15);
                bool isProbation = dateEndProbation.HasValue && dateEndProbation.Value > midDate;
                formula.Parameters.Add(Formula.FormulaConstant.INS_PROBATION.ToString(), isProbation.ToString());
            }
            if (formulaAnnualLeave.Contains(Formula.FormulaConstant.TOTAL.ToString()))
            {
                formula.Parameters.Add(Formula.FormulaConstant.TOTAL.ToString(), totalDayAnnualLeaveOnYear);
            }
            if (formulaAnnualLeave.Contains(Formula.FormulaConstant.CURRENTYEAR.ToString()))
            {
                formula.Parameters.Add(Formula.FormulaConstant.CURRENTYEAR.ToString(), currentMonth.Year);
            }
            if (formulaAnnualLeave.Contains(Formula.FormulaConstant.TOTAL_LEAVE_BY_TYPE_IN_MONTH.ToString()))
            {

                if (ProfileCurrentID != profile.ID || dtEnd.Month == 1)
                {
                    _Num_TOTAL_LEAVE_BY_TYPE_IN_MONTH = 0;
                    ProfileCurrentID = profile.ID;
                }


                DateTime DateBeginMonth = dtStart;
                DateTime DateEndMonth = dtEnd;
                string E_FULLSHIFT = LeaveDayDurationType.E_FULLSHIFT.ToString();
                List<string> lstCodeLeave = lstCodeLeaveNonAnl;
                double Sum = 0;
                if (lstCodeLeave.Count > 0)
                {
                    Guid guidNewRelease = Guid.NewGuid();

                    string status = string.Empty;
                    BaseService baseService = new BaseService();
                    List<object> lst3ParamFT = new List<object>();
                    lst3ParamFT.Add(null);
                    lst3ParamFT.Add(DateBeginMonth);
                    lst3ParamFT.Add(DateEndMonth);
                    var dataAtt_LeaveDay = baseService.GetData<Att_LeaveDay>(lst3ParamFT, ConstantSql.hrm_att_getdata_LeaveDay_Inner, userLogin, ref status).ToList();
                    var lstLeaveTotalDuration = dataAtt_LeaveDay.Where(m => m.ProfileID == profile.ID
                        //&& m.DateStart <= DateEndMonth
                        //&& m.DateEnd >= DateBeginMonth
                         && m.DurationType == E_FULLSHIFT
                         && m.TotalDuration != null
                         && m.Cat_LeaveDayType != null
                         && lstCodeLeave.Contains(m.Cat_LeaveDayType.Code))
                        .Select(m => m.TotalDuration);
                    foreach (var item in lstLeaveTotalDuration)
                    {
                        if (item != null)
                        {
                            Sum += item.Value;
                        }
                    }
                    if (Sum > 13)
                        _Num_TOTAL_LEAVE_BY_TYPE_IN_MONTH = _Num_TOTAL_LEAVE_BY_TYPE_IN_MONTH + 1;
                    //EntityService.Instance.ReleaseContext(guidNewRelease);
                }
                formula.Parameters.Add(Formula.FormulaConstant.TOTAL_LEAVE_BY_TYPE_IN_MONTH.ToString(), _Num_TOTAL_LEAVE_BY_TYPE_IN_MONTH);
            }
            if (formulaAnnualLeave.Contains(Formula.FormulaConstant.MONTHHIRE.ToString()))
            {
                double monthHire = dateHire == null ? 1 : dateHire.Value.Month;
                formula.Parameters.Add(Formula.FormulaConstant.MONTHHIRE.ToString(), monthHire);
            }
            if (formulaAnnualLeave.Contains(Formula.FormulaConstant.YEARHIRE.ToString()))
            {
                int yearHire = dateHire != null ? dateHire.Value.Year : 0;
                formula.Parameters.Add(Formula.FormulaConstant.YEARHIRE.ToString(), yearHire);
            }
            if (formulaAnnualLeave.Contains(Formula.FormulaConstant.ADDITIONAL_ANNUAL.ToString()))
            {
                initAnnualValue = initAnnualValue.HasValue ? initAnnualValue.Value : 0;
                formula.Parameters.Add(Formula.FormulaConstant.ADDITIONAL_ANNUAL.ToString(), initAnnualValue.Value);
            }
            if (formulaAnnualLeave.Contains(Formula.FormulaConstant.MONTHENDPRO.ToString()))
            {
                #region Tháng kết thúc thử việc

                if (dateEndProbation.HasValue)
                {
                    DateTime fromDate = currentMonth.Date.AddDays(1 - currentMonth.Day);
                    DateTime toDate = fromDate.Date.AddMonths(1).AddSeconds(-1);
                    GetDuration(gradeCfg, currentMonth, out fromDate, out toDate);

                    int monthstart = dateEndProbation.Value.Month;
                    if (currentMonth.Year > dateEndProbation.Value.Year)
                    {
                        monthstart = monthStartAnnualLeave == null ? 1 : monthStartAnnualLeave.Value;
                    }
                    else if (currentMonth.Year == dateEndProbation.Value.Year)
                    {
                        monthstart = dateEndProbation.Value.Month;

                        if (dateEndProbation.Value.Day > 15)
                        {
                            monthstart++;
                        }

                        //Tháng đang xét nhỏ hơn tháng vào cty
                        if (currentMonth.Month < monthstart)
                        {
                            return 0;
                        }
                    }
                    else if (currentMonth.Year < dateEndProbation.Value.Year)
                    {
                        return 0;
                    }

                    formula.Parameters.Add(Formula.FormulaConstant.MONTHENDPRO.ToString(), monthstart);
                }

                #endregion
            }
            if (formulaAnnualLeave.Contains(Formula.FormulaConstant.MONTHSTART_PROB.FormulaToString()))
            {
                #region Tháng bắt đầu thử việc

                DateTime dateTemp = DateTime.Now;
                if (dateEndProbation.HasValue)
                {
                    dateTemp = dateEndProbation.Value;
                }
                else if (dateHire.HasValue)
                {
                    dateTemp = dateHire.Value;
                }

                int monthstart = dateTemp.Month;
                if (currentMonth.Year > dateTemp.Year)
                {
                    monthstart = monthStartAnnualLeave == null ? 1 : monthStartAnnualLeave.Value;
                }
                else if (currentMonth.Year == dateTemp.Year)
                {
                    monthstart = dateTemp.Month;
                    if (dateTemp.Day > 15)
                    {
                        monthstart++;
                    }

                    //Trường hợp tháng vào cty nhỏn hơn tháng bắt đầu tính phép năm
                    if (monthStartAnnualLeave != null && monthstart < monthStartAnnualLeave.Value)
                    {
                        monthstart = monthStartAnnualLeave.Value;
                    }

                    //Tháng đang xét nhỏ hơn tháng vào cty
                    if (currentMonth.Month < monthstart)
                    {
                        return 0;
                    }
                }
                else if (currentMonth.Year < dateTemp.Year)
                {
                    return 0;
                }

                formula.Parameters.Add(Formula.FormulaConstant.MONTHSTART_PROB.ToString(), monthstart);

                #endregion
            }
            if (formulaAnnualLeave.Contains(Formula.FormulaConstant.MONTHSTART2.FormulaToString()))
            {
                #region MONTHSTART2

                if (!dateHire.HasValue)
                {
                    return 0;
                }

                double monthstart = dateHire.Value.Month;
                if (currentMonth.Year > dateHire.Value.Year)
                {
                    monthstart = monthStartAnnualLeave == null ? 1 : monthStartAnnualLeave.Value;
                }
                else if (currentMonth.Year == dateHire.Value.Year)
                {
                    DateTime monthHirePro = dateHire.Value;
                    DateTime fromDate = currentMonth.Date.AddDays(1 - currentMonth.Day);
                    DateTime toDate = fromDate.Date.AddMonths(1).AddSeconds(-1);

                    Att_AttendanceServices.GetMonthSalary(gradeCfg, dateHire.Value, out monthHirePro);
                    Att_AttendanceLib.GetDuration(gradeCfg, monthHirePro, out fromDate, out toDate);
                    Double countDay = toDate.Date.Subtract(dateHire.Value.Date).TotalDays;

                    if (countDay <= 20 && countDay >= 10)
                    {
                        monthstart += 0.5;
                    }
                    else if (countDay < 10)
                    {
                        monthstart += 1;
                    }

                    //cung nam nhung thang dang xet nho hon thang vao cty
                    if (currentMonth.Month < dateHire.Value.Month)
                    {
                        return 0;
                    }
                }
                else if (currentMonth.Year < dateHire.Value.Year)
                {
                    return 0;
                }

                formula.Parameters.Add(Formula.FormulaConstant.MONTHSTART2.ToString(), monthstart);

                #endregion
            }
            if (formulaAnnualLeave.Contains(Formula.FormulaConstant.MONTHSTART.FormulaToString()))
            {
                #region MONTHSTART

                if (dateHire != null)
                {
                    int monthstart = dateHire.Value.Month;
                    if (currentMonth.Year > dateHire.Value.Year)
                    {
                        monthstart = monthStartAnnualLeave == null ? 1 : monthStartAnnualLeave.Value;
                    }
                    else if (currentMonth.Year == dateHire.Value.Year)
                    {
                        monthstart = dateHire.Value.Month;
                        if (dateHire.Value.Day > 15)
                        {
                            monthstart++;
                        }

                        //cung nam nhung thang dang xet nho hon thang vao cty
                        if (currentMonth.Month < monthstart)
                        {
                            return 0;
                        }
                    }
                    else if (currentMonth.Year < dateHire.Value.Year)
                    {
                        return 0;
                    }

                    formula.Parameters.Add(Formula.FormulaConstant.MONTHSTART.ToString(), monthstart);
                }

                #endregion
            }
            if (formulaAnnualLeave.Contains(Formula.FormulaConstant.TERMINATION.ToString()))
            {
                #region TERMINATION

                bool isTermination = dateQuit.HasValue && dateQuit.Value.Year == currentMonth.Year && dateQuit.Value.Month == currentMonth.Month;
                formula.Parameters.Add(Formula.FormulaConstant.TERMINATION.ToString(), isTermination);

                #endregion
            }
            if (formulaAnnualLeave.Contains(Formula.FormulaConstant.MONTHEND.ToString()))
            {
                #region MONTHEND

                int monthEnd = 0;

                if (dateQuit.HasValue && dateQuit.Value.Year == currentMonth.Year)
                {
                    if (dateQuit.Value.Day > 15)
                    {
                        monthEnd = dateQuit.Value.Month;
                    }
                    else
                    {
                        monthEnd = dateQuit.Value.Month - 1;
                    }
                }

                formula.Parameters.Add(Formula.FormulaConstant.MONTHEND.ToString(), monthEnd);

                #endregion
            }
            if (formulaAnnualLeave.Contains(Formula.FormulaConstant.CURRENTMONTH.ToString()))
            {
                #region CURRENTMONTH

                int month = 0;
                month = currentMonth.Month;

                if (dateQuit.HasValue && dateQuit.Value.Year == currentMonth.Year
                    && dateQuit.Value.Month == currentMonth.Month)
                {
                    if (dateQuit.Value.Day > 15)
                    {
                        month = currentMonth.Month;
                    }
                    else
                    {
                        month = currentMonth.Month - 1;
                    }
                }

                formula.Parameters.Add(Formula.FormulaConstant.CURRENTMONTH.ToString(), month);

                #endregion
            }
            if (formulaAnnualLeave.Contains(Formula.FormulaConstant.SENIOR_BONUS_LEAVE.ToString()))
            {
                formula.Parameters.Add(Formula.FormulaConstant.SENIOR_BONUS_LEAVE.ToString(), seniority);
            }
            if (formulaAnnualLeave.Contains(Formula.FormulaConstant.SENIOR_BONUS_LEAVE_FROM_.ToString()))
            {
                #region SENIOR_BONUS_LEAVE_FROM

                Double hourOnWorkDate = 8;

                if (gradeCfg.HourOnWorkDate.HasValue)
                {
                    hourOnWorkDate = gradeCfg.HourOnWorkDate.Value;
                }

                int idx = formulaAnnualLeave.IndexOf(Formula.FormulaConstant.SENIOR_BONUS_LEAVE_FROM_.ToString());

                while (idx != -1)
                {
                    int idx2 = formulaAnnualLeave.IndexOf(@"]", idx) - 1;
                    int lengh = Formula.FormulaConstant.SENIOR_BONUS_LEAVE_FROM_.ToString().Length;
                    string monthYearString = formulaAnnualLeave.Substring(idx + lengh, idx2 - idx - lengh + 1);
                    string year = monthYearString.Substring(4);
                    string month = monthYearString.Substring(2, 2);

                    string fullParamString = Formula.FormulaConstant.SENIOR_BONUS_LEAVE_FROM_.ToString() + monthYearString;
                    if (!formula.Parameters.ContainsKey(fullParamString))
                    {
                        DateTime monthYearEffect = new DateTime(int.Parse(year), int.Parse(month), 1);
                        seniority = GetAnnualBySeniority(currentMonth, monthYearEffect, dateHire, gradeCfg);
                        formula.Parameters.Add(fullParamString, seniority);
                    }
                    idx = formulaAnnualLeave.IndexOf(Formula.FormulaConstant.SENIOR_BONUS_LEAVE_FROM_.ToString(), idx2);
                }

                #endregion
            }
            if (gradeCfg.FormulaAnnualLeave.Contains(Formula.FormulaConstant.MONTHENDPRO.ToString()))
            {
                #region MONTHENDPRO
                DateTime fromDate = new DateTime(currentMonth.Year, currentMonth.Month, 1);
                DateTime toDate = fromDate.AddMonths(1).AddMinutes(-1);
                Att_AttendanceServices.GetSalaryDateRange(gradeCfg, null, null, currentMonth, out fromDate, out toDate);

                int monthstart = 0;
                if (dateEndProbation != null)
                {
                    monthstart = dateEndProbation.Value.Month;
                }
                if (dateEndProbation != null && currentMonth.Year > dateEndProbation.Value.Year)
                {
                    monthstart = 1;
                    monthstart = monthStartAnnualLeave ?? 1;
                }
                else if (dateEndProbation != null && currentMonth.Year == dateEndProbation.Value.Year)
                {
                    if (dateEndProbation != null)
                    {
                        if (dateEndProbation.Value.Day > 15)
                            monthstart = dateEndProbation.Value.Month;
                    }
                    monthstart++;
                    // cung nam nhung thang dang xet nho hon thang vao cty
                    if (currentMonth.Month < monthstart)
                        return 0;
                }
                else if (dateEndProbation != null && currentMonth.Year < dateEndProbation.Value.Year)
                {
                    return 0;
                }
                formula.Parameters.Add(Formula.FormulaConstant.MONTHENDPRO.ToString(), monthstart);
                #endregion
            }
            if (gradeCfg.FormulaAnnualLeave.Contains(Formula.FormulaConstant.POSITION_CODE.ToString()))
            {
                string PositionCode = string.Empty;
                if (pos != null && pos.Code != null)
                {
                    PositionCode = pos.Code;
                }
                formula.Parameters.Add(Formula.FormulaConstant.POSITION_CODE.ToString(), PositionCode);
            }
            if (gradeCfg.FormulaAnnualLeave.Contains(Formula.FormulaConstant.CURRENTMONTH_INSALARY.ToString()))
            {
                int month = 0;
                month = currentMonth.Month;
                formula.Parameters.Add(Formula.FormulaConstant.CURRENTMONTH_INSALARY.ToString(), month);
            }
            if (gradeCfg.FormulaAnnualLeave.Contains(Formula.FormulaConstant.IS_PROBATION.ToString()))
            {
                int flag = 0;

                if ((dateHire != null && dateHire >= dtStart && dateHire <= dtEnd) ||
                    (dateEndProbation != null && dateEndProbation >= dtStart && dateEndProbation <= dtEnd) ||
                    (dateHire != null && dateHire <= dtEnd && dateEndProbation != null && dateEndProbation >= dtStart))
                {
                    flag = 1;
                }
                formula.Parameters.Add(Formula.FormulaConstant.IS_PROBATION.ToString(), flag);
            }

            if (gradeCfg.FormulaAnnualLeave.Contains(Formula.FormulaConstant.YEAR_OF_DATEHIRE.ToString()))
            {
                int year = 0;
                if (dateHire != null && dateHire >= dtStart && dateHire <= dtEnd)
                {
                    year = dateHire.Value.Year;
                }
                formula.Parameters.Add(Formula.FormulaConstant.YEAR_OF_DATEHIRE.ToString(), year);
            }
            if (gradeCfg.FormulaAnnualLeave.Contains(Formula.FormulaConstant.MONTH_OF_DATEHIRE.ToString()))
            {
                int month = 0;
                if (dateHire != null && dateHire >= dtStart && dateHire <= dtEnd)
                {
                    month = dateHire.Value.Month;
                }
                formula.Parameters.Add(Formula.FormulaConstant.MONTH_OF_DATEHIRE.ToString(), month);
            }
            if (gradeCfg.FormulaAnnualLeave.Contains(Formula.FormulaConstant.YEAR_OF_DATEENDPROBATION.ToString()))
            {
                int year = 0;
                if (dateEndProbation != null && dateEndProbation >= dtStart && dateEndProbation <= dtEnd)
                {
                    year = dateEndProbation.Value.Year;
                }
                formula.Parameters.Add(Formula.FormulaConstant.YEAR_OF_DATEENDPROBATION.ToString(), year);
            }
            if (gradeCfg.FormulaAnnualLeave.Contains(Formula.FormulaConstant.MONTH_OF_DATEENDPROBATION.ToString()))
            {
                int month = 0;
                if (dateEndProbation != null && dateEndProbation >= dtStart && dateEndProbation <= dtEnd)
                {
                    month = dateEndProbation.Value.Month;
                }
                formula.Parameters.Add(Formula.FormulaConstant.MONTH_OF_DATEENDPROBATION.ToString(), month);
            }
            if (gradeCfg.FormulaAnnualLeave.Contains(Formula.FormulaConstant.DAYOFDATEHIRE.ToString()))
            {
                int Day = 0;
                if (dateHire != null)
                    Day = dateHire.Value.Day;
                formula.Parameters.Add(Formula.FormulaConstant.DAYOFDATEHIRE.ToString(), Day);
            }

            if (gradeCfg.FormulaAnnualLeave.Contains(Formula.FormulaConstant.DAYOFDATEQUIT.ToString()))
            {
                int Day = 0;
                if (dateQuit != null)
                    Day = dateQuit.Value.Day;
                formula.Parameters.Add(Formula.FormulaConstant.DAYOFDATEQUIT.ToString(), Day);
            }
            if (gradeCfg.FormulaAnnualLeave.Contains(Formula.FormulaConstant.JOBTITLE_ANNUAL.ToString()))
            {
                formula.Parameters.Add(Formula.FormulaConstant.JOBTITLE_ANNUAL.ToString(), AnnualDays);
            }
            //Lấy tháng của DateHire, lấy kỳ lương theo tháng đó (tháng của DateHire), sau đó xuất ra Năm của kỳ lương đó.
            //vd: DateHire là 26/08/2014 ==>kỳ lương T8 là 25/07/2014-24/08/2014, ktra DateHire có thuộc kỳ T8 ko, nếu ko thì ktra trong kỳ lương T9.
            if (gradeCfg.FormulaAnnualLeave.Contains(Formula.FormulaConstant.YEAR_OF_SALARY_IF_DATEHIRE_BELONG.ToString()))
            {
                int year = 0;
                DateTime dateStart, dateEnd;
                if (dateHire != null)
                {
                    DateTime dtmonthCheck = new DateTime(dateHire.Value.Year, dateHire.Value.Month, 1);
                    //Lấy khoảng thời gian của kỳ lương
                    Att_AttendanceServices.GetSalaryDateRange(gradeCfg, null, null, dtmonthCheck, out dateStart, out dateEnd);
                    if (dateHire >= dateStart && dateHire <= dateEnd)
                    {
                        year = dateEnd.Year;
                    }
                    else
                    {
                        DateTime dateStartAdd = dateStart.AddMonths(1);
                        DateTime dateEndAdd = dateEnd.AddMonths(1);
                        if (dateHire >= dateStartAdd && dateHire <= dateEndAdd)
                        {
                            year = dateEndAdd.Year;
                        }
                    }
                }
                formula.Parameters.Add(Formula.FormulaConstant.YEAR_OF_SALARY_IF_DATEHIRE_BELONG.ToString(), year);
            }
            //Lấy tháng của DateHire, lấy kỳ lương theo tháng đó (tháng của DateHire), sau đó xuất ra Tháng của kỳ lương đó.
            if (gradeCfg.FormulaAnnualLeave.Contains(Formula.FormulaConstant.MONTH_OF_SALARY_IF_DATEHIRE_BELONG.ToString()))
            {
                int month = 0;
                DateTime dateStart, dateEnd;
                if (dateHire != null)
                {
                    DateTime dtmonthCheck = new DateTime(dateHire.Value.Year, dateHire.Value.Month, 1);
                    //Lấy khoảng thời gian của kỳ lương
                    Att_AttendanceServices.GetSalaryDateRange(gradeCfg, null, null, dtmonthCheck, out dateStart, out dateEnd);
                    if (dateHire >= dateStart && dateHire <= dateEnd)
                    {
                        month = dateEnd.Month;
                    }
                    else
                    {
                        DateTime dateStartAdd = dateStart.AddMonths(1);
                        DateTime dateEndAdd = dateEnd.AddMonths(1);
                        if (dateHire >= dateStartAdd && dateHire <= dateEndAdd)
                        {
                            month = dateEndAdd.Month;
                        }
                    }
                }
                formula.Parameters.Add(Formula.FormulaConstant.MONTH_OF_SALARY_IF_DATEHIRE_BELONG.ToString(), month);
            }
            //XuChi..Phần tử lấy số ngày thâm niên (theo đk của Danieli)
            if (gradeCfg.FormulaAnnualLeave.Contains(Formula.FormulaConstant.THAMNIEN_DANIELI.ToString()))
            {
                int thamnien = 0;
                DateTime dayRule = new DateTime(2014, 10, 4);  //Ngày quy định fixed của Danieli
                DateTime currentDay = DateTime.Now.Date;       //Ngày hiện tại lúc kiểm tra
                if (dateHire != null)
                {
                    DateTime dayHire = dateHire.Value.Date;
                    //Nếu ngày vào làm vào sau ngày quy định (logic: thâm niên 5 năm thì cộng 1)
                    if (dayHire >= dayRule)
                    {
                        while (dayHire.AddYears(5) <= currentDay)
                        {
                            dayHire = dayHire.AddYears(5);
                            thamnien++;
                        }
                    }
                    else //Nếu ngày vào làm trước ngày quy định 
                    {
                        //logic 1: thâm niêm 3 năm thì cộng 1
                        while (dayHire.AddYears(3) <= dayRule)
                        {
                            dayHire = dayHire.AddYears(3);
                            thamnien++;
                        }
                        //logic 2: sau logic 1, sẽ có ngày dayHire gần với ngày dayRule nhất, 
                        //khi đó dayHire sẽ đc so sánh với currentDay, sau đó tính thâm niên 5 năm thì cộng 1
                        while (dayHire.AddYears(5) <= currentDay)
                        {
                            dayHire = dayHire.AddYears(5);
                            thamnien++;
                        }
                    }
                }
                formula.Parameters.Add(Formula.FormulaConstant.THAMNIEN_DANIELI.ToString(), thamnien);
            }

            result = Convert.ToDouble(formula.Evaluate());
            if ((result - (int)result) > 0)
            {
                double dental = result - (int)result;
                result = dental >= 0.5 ? (int)result + 1 : (int)result;
            }
            return result;
            return result;
        }

        public static void GetConfigANL(List<Sys_AllSetting> lstAllSeting, out ParamGetConfigANL paramConfig)
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



            //            
            //AppConfig.  -
            //AppConfig.  -
            //AppConfig. -
            //AppConfig. -
            //AppConfig.  -
            //AppConfig. -
            //AppConfig. -
            //AppConfig. -
            //AppConfig. -
            //AppConfig. -
            //AppConfig. -
            //AppConfig.


            if (lstAllSeting.Any(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_MONTHBEGINYEAR.ToString()))
            {
                int value = 0;
                int.TryParse(lstAllSeting.Where(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_MONTHBEGINYEAR.ToString()).FirstOrDefault().Value1, out value);
                value = value == 0 ? 1 : value;
                paramConfig.monthBeginYear = value;
            }

            if (lstAllSeting.Any(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_DAYBEGIN_FULLMONTH.ToString()))
            {
                int value = 0;
                int.TryParse(lstAllSeting.Where(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_DAYBEGIN_FULLMONTH.ToString()).FirstOrDefault().Value1, out value);
                value = value == 0 ? 1 : value;
                paramConfig.dayBeginFullMonth = value;
            }

            if (lstAllSeting.Any(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_SENIOR_MONTH.ToString()))
            {
                int value = 0;
                int.TryParse(lstAllSeting.Where(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_SENIOR_MONTH.ToString()).FirstOrDefault().Value1, out value);
                paramConfig.seniorMonth = value;
            }

            if (lstAllSeting.Any(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_DAY_PER_MONTH.ToString()))
            {
                int value = 0;
                int.TryParse(lstAllSeting.Where(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_DAY_PER_MONTH.ToString()).FirstOrDefault().Value1, out value);
                paramConfig.dayPerMonth = value;
            }

            if (lstAllSeting.Any(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_ROUND_UP.ToString()))
            {
                double value = 0;
                double.TryParse(lstAllSeting.Where(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_ROUND_UP.ToString()).FirstOrDefault().Value1, out value);
                paramConfig.anlRoundUp = value;
            }
            if (lstAllSeting.Any(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_TYPE_PROFILE_BEGIN.ToString()))
            {
                paramConfig.typeProfileBegin = lstAllSeting.Where(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_TYPE_PROFILE_BEGIN.ToString()).FirstOrDefault().Value1;
            }

            if (lstAllSeting.Any(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_DAY_MAX_IN_MONTH_GET_ACTUAL.ToString()))
            {
                int value = 0;
                int.TryParse(lstAllSeting.Where(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_DAY_MAX_IN_MONTH_GET_ACTUAL.ToString()).FirstOrDefault().Value1, out value);
                paramConfig.maxInMonthToGetAct = value;
            }

            if (lstAllSeting.Any(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_NORMAL_CAN_GET_FULLYEAR.ToString()))
            {
                double value = 0;
                double.TryParse(lstAllSeting.Where(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_NORMAL_CAN_GET_FULLYEAR.ToString()).FirstOrDefault().Value1, out value);
                paramConfig.anlFullYear = (double)value / (double)12;
            }

            if (lstAllSeting.Any(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_SENIOR_CAN_GET_EACH_LEVEL.ToString()))
            {
                double value = 0;
                double.TryParse(lstAllSeting.Where(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_SENIOR_CAN_GET_EACH_LEVEL.ToString()).FirstOrDefault().Value1, out value);
                paramConfig.anlSeniorMoreThanNormal = (double)value / (double)12;
            }

            if (lstAllSeting.Any(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_HDT4_CAN_GET_MORE_NORMAL.ToString()))
            {
                double value = 0;
                double.TryParse(lstAllSeting.Where(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_HDT4_CAN_GET_MORE_NORMAL.ToString()).FirstOrDefault().Value1, out value);
                paramConfig.anlHDT4MoreThanNormal = (double)value / (double)12;
            }

            if (lstAllSeting.Any(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_HDT5_CAN_GET_MORE_NORMAL.ToString()))
            {
                double value = 0;
                double.TryParse(lstAllSeting.Where(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_HDT5_CAN_GET_MORE_NORMAL.ToString()).FirstOrDefault().Value1, out value);
                paramConfig.anlHDT5MoreThanNormal = (double)value / (double)12;
            }

            if (lstAllSeting.Any(m => m.Name == AppConfig.HRM_ATT_CONFIG_LEAVE_NON_ALN_CODES.ToString()))
            {
                string CodeLeaveNonANL = lstAllSeting.Where(m => m.Name == AppConfig.HRM_ATT_CONFIG_LEAVE_NON_ALN_CODES.ToString()).FirstOrDefault().Value1;
                char[] ext = new char[] { ',' };
                if (!string.IsNullOrEmpty(CodeLeaveNonANL))
                {
                    paramConfig.lstCodeLeaveNonANL = CodeLeaveNonANL.Split(ext, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
            }

            if (lstAllSeting.Any(m => m.Name == AppConfig.HRM_ATT_CONFIG_MONTH_INYEAR_TO_COMPUTE_SENIOR.ToString()))
            {
                int value = 0;
                int.TryParse(lstAllSeting.Where(m => m.Name == AppConfig.HRM_ATT_CONFIG_MONTH_INYEAR_TO_COMPUTE_SENIOR.ToString()).FirstOrDefault().Value1, out value);
                paramConfig.monthInYearSenior = value;
            }

            if (lstAllSeting.Any(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_ROUND_UP.ToString()))
            {
                int value = 0;
                int.TryParse(lstAllSeting.Where(m => m.Name == AppConfig.HRM_ATT_CONFIG_ANL_ROUND_UP.ToString()).FirstOrDefault().Value1, out value);
                paramConfig.monthRoundUp = value;
            }


            #endregion
        }

        /// <summary>
        /// Tính số ngày phép năm tích lũy được đến thời điểm currentMonth.
        /// </summary>
        /// <param name="currentMonth">Tháng đang xét</param>
        /// <param name="gradeCfg"></param>
        /// <param name="profile">Nhân viên muốn tính</param>
        /// <param name="annualLeave">Các thông số liên quan nhân viên</param>
        /// <returns></returns>


        /// <summary>
        /// Tính số ngày phép năm tích lũy được đến thời điểm currentMonth.
        /// </summary>
        /// <param name="currentMonth">Tháng đang xét</param>
        /// <param name="gradeCfg"></param>
        /// <param name="profile">Nhân viên muốn tính</param>
        /// <param name="annualLeave">Các thông số liên quan nhân viên</param>
        /// <returns></returns>

        #endregion

        /// <summary>
        /// Kiểm tra một ngày có phải là ngày làm việc không?
        /// </summary>
        /// <param name="date"></param>
        /// <param name="gradeCfg"></param>
        /// <param name="listDailyShift"></param>
        /// <param name="listDayOff"></param>
        /// <returns></returns>

        public static bool IsWorkDay(DateTime date, Cat_GradeAttendance gradeCfg,
            Dictionary<DateTime, Cat_Shift> listDailyShift, List<Cat_DayOff> listDayOff)
        {
            bool result = false;

            if (gradeCfg.RosterType == GradeRosterType.E_ISROSTER.ToString()
                || gradeCfg.RosterType == GradeRosterType.E_ISROSTER_ORG.ToString())
            {
                result = IsWorkDay(date, listDailyShift);
            }
            else if (gradeCfg.RosterType == GradeRosterType.E_ISDEFAULT.ToString())
            {
                result = IsWorkDay(date, gradeCfg, listDayOff);
            }
            else if (gradeCfg.RosterType == GradeRosterType.E_ISHOLIDAY.ToString())
            {
                if (listDayOff == null || !listDayOff.Any(d => d.DateOff.Date == date.Date
                    && d.Type == HolidayType.E_WEEKEND_HLD.ToString()))
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Kiểm tra một ngày có phải là ngày làm việc không?
        /// Chỉ áp dụng cho trường hợp RosterType = E_ISDEFAULT
        /// </summary>
        /// <param name="date"></param>
        /// <param name="gradeCfg"></param>
        /// <param name="listDayOff"></param>
        /// <returns></returns>

        public static bool IsWorkDay(DateTime date, Cat_GradeAttendance gradeCfg, List<Cat_DayOff> listDayOff)
        {
            bool result = false;

            if (listDayOff == null || !listDayOff.Any(d => d.DateOff.Date == date.Date
                && d.Type == HolidayType.E_WEEKEND_HLD.ToString()))
            {
                if (gradeCfg.RosterType == GradeRosterType.E_ISDEFAULT.ToString())
                {
                    if (date.DayOfWeek == DayOfWeek.Monday && gradeCfg.WorkOnMondayID != null)
                    {
                        result = true;
                    }
                    else if (date.DayOfWeek == DayOfWeek.Tuesday && gradeCfg.WorkOnTuesdayID != null)
                    {
                        result = true;
                    }
                    else if (date.DayOfWeek == DayOfWeek.Wednesday && gradeCfg.WorkOnWednesdayID != null)
                    {
                        result = true;
                    }
                    else if (date.DayOfWeek == DayOfWeek.Thursday && gradeCfg.WorkOnThursdayID != null)
                    {
                        result = true;
                    }
                    else if (date.DayOfWeek == DayOfWeek.Friday && gradeCfg.WorkOnFridayID != null)
                    {
                        result = true;
                    }
                    else if (date.DayOfWeek == DayOfWeek.Saturday && gradeCfg.WorkOnSaturdayID != null)
                    {
                        result = true;
                    }
                    else if (date.DayOfWeek == DayOfWeek.Sunday && gradeCfg.WorkOnSundayID != null)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Kiểm tra một ngày có phải là ngày làm việc không?
        /// </summary>
        /// <param name="date"></param>
        /// <param name="listDailyShift"></param>
        /// <returns></returns>
        public static bool IsWorkDay(DateTime date, Dictionary<DateTime, Cat_Shift> listDailyShift)
        {
            return listDailyShift != null && listDailyShift.Any(d =>
                d.Key.Date == date.Date && d.Value != null);
        }

        #region GetSalaryDuration

        /// <summary>
        /// Lấy khoảng thời gian tính lương theo grade config.
        /// </summary>
        /// <param name="gradeCfg"></param>
        /// <param name="monthYear"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        public static void GetDuration(Cat_GradeAttendance gradeCfg,
            DateTime monthYear, out DateTime dateFrom, out DateTime dateTo)
        {
            int day = 1;

            if (gradeCfg != null)
            {
                if (gradeCfg.SalaryTimeType == EnumDropDown.SalaryTimeType.E_LASTMONTH.ToString())
                {
                    monthYear = monthYear.AddMonths(-1);
                }

                day = gradeCfg.SalaryTimeDay.Get_Integer() <= 0 ? 1 : gradeCfg.SalaryTimeDay.Get_Integer();
                dateFrom = new DateTime(monthYear.Year, monthYear.Month, day);
                dateTo = dateFrom.AddMonths(1).AddSeconds(-1);
            }
            else
            {
                GetDuration(monthYear, out dateFrom, out dateTo);
            }
        }

        /// <summary>
        /// Khoảng thời gian đầu tháng đến cuối tháng.
        /// </summary>
        /// <param name="monthYear"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        public static void GetDuration(DateTime monthYear, out DateTime dateFrom, out DateTime dateTo)
        {
            dateFrom = monthYear.Date.AddDays(1 - monthYear.Day);
            dateTo = dateFrom.AddMonths(1).AddSeconds(-1);
        }
        #endregion
    }
}
