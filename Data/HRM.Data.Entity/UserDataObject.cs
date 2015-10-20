using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VnResource.Helper.Setting;
using VnResource.Helper.Data;

namespace HRM.Data.Entity
{
    public partial class Att_Workday
    {
        public Guid? Shift2ID { get; set; }
        public Guid? ShiftActual2 { get; set; }
        public Guid? ShiftApprove2 { get; set; }
    }

    public partial class Att_AttendanceTableItem
    {
        public Guid? Shift2ID { get; set; }
        public Cat_Shift Cat_Shift1 { get; set; }

        #region Cat_DataGroupDetail

        public DateTime? udLateEarlyDuration
        {
            get
            {
                if (LateEarlyMinutes > 0)
                    return Common.GetDateTimeHour(LateEarlyMinutes / 60.0);
                return null;
            }
        }

        public DateTime? udUnpaidLeaveHours
        {
            get
            {
                if (UnpaidLeaveHours > 0)
                    return Common.GetDateTimeHour(UnpaidLeaveHours);

                return null;
            }
        }
        public DateTime? udPaidLeaveHours
        {
            get
            {
                if (PaidLeaveHours > 0)
                    return Common.GetDateTimeHour(PaidLeaveHours);

                return null;
            }
        }

        public DateTime? udWorkPaidHours
        {
            get
            {
                if (DutyCode == "E_ON")
                    return Common.GetDateTimeHour(WorkPaidHours);

                return null;
            }
        }
        public DateTime? udNightShiftHours
        {
            get
            {
                if (NightShiftHours > 0)
                    return Common.GetDateTimeHour(NightShiftHours);

                return null;
            }
        }
        public DateTime? udRealNightShiftHours
        {
            get
            {
                if (NightShiftHours > 0)
                    return Common.GetDateTimeHour(NightShiftHours + (RealLateEarlyMinutes != null ? RealLateEarlyMinutes.Value : 0 / 60));

                return null;
            }
        }
        public DateTime? udTotalOvertime
        {
            get
            {
                Double totalOT = OvertimeHours + ExtraOvertimeHours + ExtraOvertimeHours2 + ExtraOvertimeHours3;

                if (totalOT > 0)
                    return Common.GetDateTimeHour(totalOT);

                return null;
            }
        }
        public Double udTotalOvertimeHours
        {
            get
            {
                Double totalOT = 0;
                totalOT = OvertimeHours + ExtraOvertimeHours + ExtraOvertimeHours2 + ExtraOvertimeHours3;
                return totalOT;
            }
        }

        public DateTime? udOvertimeHours
        {
            get
            {
                if (OvertimeHours > 0)
                    return Common.GetDateTimeHour(OvertimeHours);

                return null;
            }
        }
        public DateTime? udExtraOvertimeHours
        {
            get
            {
                if (ExtraOvertimeHours > 0)
                    return Common.GetDateTimeHour(ExtraOvertimeHours);

                return null;
            }
        }
        Double _udNotHiredHours = 0;
        public Double udNotHiredHours
        {
            get { return _udNotHiredHours; }
            set { _udNotHiredHours = value; }
        }

        Double _udTerminatedHours = 0;
        public Double udTerminatedHours
        {
            get
            {
                return _udTerminatedHours;
            }
            set { _udTerminatedHours = value; }
        }
        public string udShift
        {
            get
            {
                if (this.Cat_Shift == null)
                    return string.Empty;
                return this.Cat_Shift.ShiftName;
            }
        }
        String _udPregEarlyType = string.Empty;
        public String udPregEarlyType
        {
            get
            {
                return _udPregEarlyType;
            }
            set { _udPregEarlyType = value; }
        }

        public double udStandardWorkHours
        {
            get
            {
                if (Cat_Shift != null && Cat_Shift.udStandardWorkHours > 0)
                {
                    return Cat_Shift.udStandardWorkHours;
                }

                if (AvailableHours > 0)
                {
                    return AvailableHours;
                }

                return 8;
            }
        }

        public double udWorkHours
        {
            get
            {
                if (Cat_Shift != null && Cat_Shift.WorkHours > 0)
                {
                    return Cat_Shift.WorkHours.Value;
                }

                if (AvailableHours > 0)
                {
                    return AvailableHours;
                }

                return 8;
            }
        }

        public partial class FieldNames
        {
            public const string udLateEarlyDuration = "udLateEarlyDuration";
            public const string udUnpaidLeaveHours = "udUnpaidLeaveHours";
            public const string udPaidLeaveHours = "udPaidLeaveHours";
            public const string udWorkPaidHours = "udWorkPaidHours";
            public const string udNightShiftHours = "udNightShiftHours";
            public const string udRealNightShiftHours = "udRealNightShiftHours";
            public const string udTotalOvertimeHours = "udTotalOvertimeHours";
            public const string udTotalOvertime = "udTotalOvertime";
            public const string udOvertimeHours = "udOvertimeHours";
            public const string udExtraOvertimeHours = "udExtraOvertimeHours";
            public const string udShift = "udShift";
            public const string udNotHiredHours = "udNotHiredHours";
            public const string udTerminatedHours = "udTerminatedHours";
            public const string udPregEarlyType = "udPregEarlyType";
        }
        #endregion
    }
    public class OvertimeInfo
    {
        private DateTime dateFrom;

        /// <summary>
        /// Ngày bắt đầu ca làm việc.
        /// </summary>
        public DateTime DateFrom
        {
            get
            {
                if (BreaktPoints.Any(d => d.Key.TimeOfDay < dateFrom.TimeOfDay &&
                    d.Key.AddHours(d.Value).TimeOfDay > dateFrom.TimeOfDay))
                {
                    var breakPoint = BreaktPoints.Where(d => d.Key.TimeOfDay < dateFrom.TimeOfDay &&
                        d.Key.AddHours(d.Value).TimeOfDay > dateFrom.TimeOfDay).FirstOrDefault();

                    if (breakPoint.Key != null)
                    {
                        //Trường hợp giờ đăng ký dateFrom nằm trong khoản giờ nghỉ giữa ca làm việc breakPoint
                        dateFrom.Add(breakPoint.Key.AddHours(breakPoint.Value).TimeOfDay.Subtract(dateFrom.TimeOfDay));
                    }
                }

                return dateFrom;
            }
            set { dateFrom = value; }
        }

        private DateTime dateTo;

        /// <summary>
        /// Ngày kết thúc ca làm việc.
        /// </summary>
        public DateTime DateTo
        {
            get
            {
                if (UsingTotalHours && TotalHours > 0)
                {
                    dateTo = DateFrom.AddHours(TotalHours);

                    var list = BreaktPoints.Where(d => DateFrom.Date.Add(d.Key.TimeOfDay) < dateTo
                        && DateFrom.Date.Add(d.Key.TimeOfDay).AddHours(d.Value) > DateFrom).ToList();

                    foreach (var item in list)
                    {
                        TimeSpan first = DateFrom.Subtract(DateFrom.Date.Add(item.Key.TimeOfDay));
                        if (first.TotalHours > 0)
                        {
                            dateTo = dateTo.AddHours(-first.TotalHours).AddHours(item.Value);
                        }
                        else
                        {
                            dateTo = dateTo.AddHours(item.Value);
                        }
                    }
                }

                return dateTo;
            }
            set { dateTo = value; }
        }

        /// <summary>
        /// Các mốc thời gian ca ngày (tính bằng giờ).
        /// </summary>
        public DateTime[] DayShiftPoints { get; set; }

        /// <summary>
        /// Các mốc thời gian ca đêm (tính bằng giờ).
        /// </summary>
        public DateTime[] NightShiftPoints { get; set; }

        /// <summary>
        /// Danh sách DayOff theo cấu hình.
        /// </summary>
        public List<Cat_DayOff> ListDayOff { get; set; }
        /// <summary>
        /// Danh sách LeaveDay loại mã "HLD".
        /// </summary>
        public List<Att_LeaveDay> ListLeaveDay { get; set; }
        /// <summary>
        /// Hồ sơ nhân viên đang xét tăng ca.
        /// </summary>
        public Hre_Profile Hre_Profile { get; set; }

        private Dictionary<DateTime, double> breaktPoints;

        /// <summary>
        /// Các mốc thời gian nghỉ trong ngày.
        /// </summary>
        public Dictionary<DateTime, double> BreaktPoints
        {
            get
            {
                if (breaktPoints == null)
                {
                    breaktPoints = new Dictionary<DateTime, double>();
                }

                return breaktPoints;
            }
        }

        /// <summary>
        /// Tổng số giờ tăng ca.
        /// </summary>
        public double TotalHours { get; set; }

        /// <summary>
        /// Không truyền DateTo khi đăng ký.
        /// </summary>
        public bool UsingTotalHours { get; set; }

        public OvertimeInfo(bool usingTotalHours)
        {
            UsingTotalHours = usingTotalHours;
        }
    }

    public class LockObjectInfo
    {
        public object Entity { get; set; }
        public string TableName { get; set; }
        public CheckLockType Type { get; set; }
        public string FieldName { get; set; }
        public string FieldStart { get; set; }
        public string FieldEnd { get; set; }

        public class FieldNames
        {
            public const string FieldInfo = "FieldInfo";
            public const string TableName = "TableName";
            public const string Type = "Type";
            public const string FieldName = "FieldName";
            public const string FieldStart = "FieldStart";
            public const string FieldEnd = "FieldEnd";
        }
    }

    public partial class Cat_GradeAttendance
    {
        #region Cat_GradeAttendance
        //public double ValueOT(double _value,Guid userid)
        //{
        //   GradeCfgDAO dao = new GradeCfgDAO();
        //   return this.RoundValueOT(_value,this.ID,userid) 
        //}


        private string _udGradeCfg = string.Empty;
        public string udGradeCfg
        {
            get
            {
                return this.GradeAttendanceName;
            }
        }
        public partial class FieldNames
        {
            public const string udValue = "udValue";
            public const string udGradeCfg = "udGradeCfg";
        }
        /// <summary>
        /// Get the standard work hour per day
        /// </summary>
        /// <param name="monthYear">the month to evaluate</param>
        /// <returns></returns>
        public Double GetWorkHouPerDay(DateTime monthYear)
        {
            if (this.HourOnWorkDate == null)
                return 0;
            //throw new VNRException(ExceptionType.NULL, "HourOnWorkDate is not defined in Grade " + udGradeCfg);
            Double res = this.HourOnWorkDate.Value;

            if (this.ExpHourPerDay.HasValue)
            {
                for (DateTime monthIdx = ExpHourFrom.Value
                    ; monthIdx <= ExpHourTo.Value
                    ; monthIdx = monthIdx.AddMonths(1))
                {
                    if (monthIdx.Month == monthYear.Month && monthIdx.Year == monthYear.Year)
                    {
                        res = ExpHourPerDay.Value;
                        break;
                    }
                }
            }
            return res;
        }

        public void GetSalaryDateRange(DateTime monthYear, out DateTime from, out DateTime to)
        {
            //if (SalaryTimeType == "E_LASTMONTH")
            //{
            //    from = new DateTime(monthYear.AddMonths(-1).Year, monthYear.AddMonths(-1).Month, this.SalaryTimeDay);
            //}
            //else //same month
            //{
            //from = new DateTime(monthYear.Year, monthYear.Month, this.SalaryTimeDay);
            //}
            from = new DateTime(monthYear.Year, monthYear.Month, 1);
            to = from.AddMonths(1).AddMinutes(-1);
        }
        #endregion
    }

    public partial class Att_TAMScanLog
    {
        #region Att_TAMScanLog

        private Hre_Profile _udProfile = null;
        public Hre_Profile udProfile
        {
            get
            {
                return _udProfile;
            }
            set
            {
                _udProfile = value;
            }
        }

        private String _udProfileName = null;
        //public String udProfileName
        //{
        //    get
        //    {
        //        if (_udProfileName != null)
        //            return _udProfileName;

        //        if (Hre_Profile != null)
        //        {
        //            _udProfileName = Hre_Profile.ProfileName;
        //        }

        //        return _udProfileName;
        //    }
        //    set
        //    {
        //        _udProfileName = value;
        //    }
        //}

        private String _udCodeEmp = null;
        //public String udCodeEmp
        //{
        //    get
        //    {
        //        if (_udCodeEmp != null)
        //            return _udCodeEmp;

        //        if (Hre_Profile != null)
        //        {
        //            _udCodeEmp = Hre_Profile.CodeEmp;
        //        }

        //        return _udCodeEmp;
        //    }
        //    set
        //    {
        //        _udCodeEmp = value;
        //    }
        //}

        private string _udType = string.Empty;
        public string udType
        {
            get
            {
                return _udType;
            }
            set
            {
                _udType = value;
            }

        }
        private DateTime? _udWorkDate = null;
        public DateTime? udWorkDate
        {
            get
            {
                return _udWorkDate;
            }
            set
            {
                _udWorkDate = value;
            }

        }

        //public String udSource
        //{
        //    get
        //    {
        //        return LanguageManager.GetString(this.Status);
        //    }
        //}

        public partial class FieldNames
        {
            public const string udProfile = "udProfile";
            public const string udProfileName = "udProfileName";
            public const string udCodeEmp = "udCodeEmp";
            public const string udType = "udType";
            public const string udSource = "udSource";
        }
        #endregion
    }

    public partial class Att_Overtime
    {
        #region Att_Overtime
        public string udIsNonOvertime
        {
            get
            {
                string result = string.Empty;
                if (this != null && this.IsNonOvertime != null && this.IsNonOvertime == true)
                {
                    result = "X";
                }
                return result;
            }
        }

        public partial class FieldNames
        {
            public const string udIsNonOvertime = "udIsNonOvertime";
        }
        #endregion
    }

    public class WorkHistoryInfo
    {
        public Guid ProfileID { get; set; }
        public Guid? OrganizationStructureID { get; set; }
        public DateTime DateEffective { get; set; }
    }

    public class WorkDay
    {
        public DateTime WorkDate;
        public Cat_Shift Cat_Shift;
        public DateTime? FirstInTime;
        public DateTime? FirstOutTime;
        public DateTime? LastInTime;
        public DateTime? LastOutTime;

        public DateTime? ShiftInTime;
        public DateTime? ShiftOutTime;
        public DateTime? ShiftBreakInTime;
        public DateTime? ShiftBreakOutTime;

        //Tính theo giờ
        public Double WorkDuration = 0D;
        public Double FirstDuration = 0D;
        public Double LastDuration = 0D;
        public Double NightShiftDuration = 0D;
        public Double MaxNightDuration = 0D;

        //Tổng trễ sơm của toàn ca
        public Double LateDuration = 0D;
        public Double EarlyDuration = 0D;
        public Double LateEarlyDuration = 0D;

        //Tổng trễ sơm của nữa ca đầu
        public Double FirstEarlyDuration = 0D;
        public Double LastEarlyDuration = 0D;

        //Tổng trễ sơm của nữa ca cuối
        public Double FirstLateDuration = 0D;
        public Double LastLateDuration = 0D;

        //Tổng time làm việc theo ca sau khi validate
        public Double FirstDurationValidated = 0D;
        public Double LastDurationValidated = 0D;

        public int GetLateInMins()
        {
            if (FirstInTime != null)
            {
                //DateTime shiftIntime = Cat_Shift.InTime;
                DateTime shiftIntime = FirstInTime.Value.Date.AddHours(Cat_Shift.InTime.Hour).AddMinutes(Cat_Shift.InTime.Minute).AddSeconds(Cat_Shift.InTime.Second);
                double lateMin = shiftIntime.Subtract(FirstInTime.Value).TotalMinutes;

                if (lateMin < 0)
                {
                    return ((int)lateMin) * (-1);
                }
            }
            return 0;
        }

        //public int GetEarlyOutMins()
        //{
        //    if (LastOutTime != null)
        //    {
        //        //DateTime shiftIntime = Cat_Shift.InTime;
        //        DateTime shiftOutTime = LastOutTime.Value.Date.AddHours(Cat_Shift.udCoOut.Hour).AddMinutes(Cat_Shift.udCoOut.Minute).AddSeconds(Cat_Shift.udCoOut.Second);
        //        double earlyMin = shiftOutTime.Subtract(LastOutTime.Value).TotalMinutes;

        //        if (earlyMin > 0)
        //        {
        //            return ((int)earlyMin) * (-1);
        //        }
        //    }
        //    return 0;
        //}
    }

    public partial class Cat_Shift
    {
        #region Cat_Shift

        private string _udShiftName;
        //public string udShiftName
        //{
        //    get
        //    {
        //        string[] strs = new string[] { QueryObjects.EntityType + "=" + ClassNames.Cat_Shift };
        //        string querystr = Common.GetQueryString(ClassNames.Cat_Shift, this.ID.ToString(), FormEditStyle.E_EDIT_OBJECT, strs);
        //        string url = VnResourceWeb.Utils.Common.GetPathHost() + "BaseControls/Views/FormEditBase.aspx?" + querystr;
        //        string title = LanguageManager.GetString(ClassNames.Cat_Shift);
        //        //string str = "openPopupDialogAjax('" + url + "', null, null, '" + title + "')";
        //        _udShiftName = "<a href='#' onclick=\"openPopupDialogAjax('" + url + "', null, null, '" + title + "')\">" + this.ShiftName + "</a>";
        //        return _udShiftName;
        //    }
        //}

        private Double _udDurationBreakTime;
        public Double udDurationBreakTime
        {
            get
            {
                if (this.CoBreakIn != null && this.CoBreakOut != null)
                    return Double.Parse(Math.Abs(CoBreakOut - CoBreakIn).ToString());

                return 0.0;
            }
            set { _udDurationBreakTime = value; }
        }

        private DateTime _udCoBreakIn;
        public DateTime udCoBreakIn
        {
            get
            {
                if (InTime == null)
                    return SqlDateTime.MinValue.Value;
                _udCoBreakIn = this.InTime.AddHours(this.CoBreakIn);
                return _udCoBreakIn;
            }
            set { _udCoBreakIn = value; }
        }
        private DateTime _udCoBreakOut;
        public DateTime udCoBreakOut
        {
            get
            {
                if (InTime == null)
                    return SqlDateTime.MinValue.Value;
                _udCoBreakOut = this.InTime.AddHours(this.CoBreakOut);
                return _udCoBreakOut;
            }
            set { _udCoBreakOut = value; }
        }
        private DateTime _udCoOut;
        private DateTime _udCoIn;

        public DateTime udCoOut
        {
            get
            {
                if (InTime == null)
                    return SqlDateTime.MinValue.Value;
                _udCoOut = this.InTime.AddHours(this.CoOut);
                return _udCoOut;
            }
            set { _udCoOut = value; }
        }
        public DateTime udCoIn
        {
            get
            {
                if (InTime == null)
                    return SqlDateTime.MinValue.Value;
                _udCoIn = this.InTime;
                return _udCoIn;
            }
            set { _udCoIn = value; }
        }
        private DateTime _udMinIn;
        public DateTime udMinIn
        {
            get
            {
                if (InTime == null)
                    return SqlDateTime.MinValue.Value;
                _udMinIn = this.InTime.AddHours(-this.MinIn);
                return _udMinIn;
            }
            set { _udMinIn = value; }
        }
        private DateTime _udMaxOut;
        public DateTime udMaxOut
        {
            get
            {
                if (udCoOut == null)
                    return SqlDateTime.MinValue.Value;
                _udMaxOut = this.udCoOut.AddHours(this.MaxOut);
                return _udMaxOut;
            }
            set { _udMaxOut = value; }
        }

        public double udCoBreakStart { get; set; }
        public double udCoBreakEnd { get; set; }

        public double udStandardWorkHours
        {
            get
            {
                if (this.StdWorkHours.GetDouble() > 0)
                {
                    //Số giờ đủ 1 công
                    return this.StdWorkHours.GetDouble();
                }

                return this.WorkHours.GetDouble();
            }
        }

        public double udAvailableHours
        {
            get
            {
                if (this.StdWorkHours.GetDouble() > 0)
                {
                    //Giờ công tính lương khi đi làm đủ ca - lấy giờ nào lớn hơn
                    if (this.StdWorkHours.GetDouble() > this.WorkHours.GetDouble())
                    {
                        return this.StdWorkHours.GetDouble();
                    }
                }

                return this.WorkHours.GetDouble();
            }
        }

        public partial class FieldNames
        {
            public const string udShiftName = "udShiftName";
            public const string udCoBreakIn = "udCoBreakIn";
            public const string udCoBreakOut = "udCoBreakOut";
            public const string udCoOut = "udCoOut";
            public const string udMinIn = "udMinIn";
            public const string udMaxOut = "udMaxOut";
        }
        #endregion
    }
    public class OrgNameClass
    {
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string BrandName { get; set; }
        public string TeamName { get; set; }

        public string DepartmentCode { get; set; }
        public string SectionCode { get; set; }
        public string BrandCode { get; set; }
        public string TeamCode { get; set; }
        public class FieldNames
        {
            public const string DepartmentName = "DepartmentName";
            public const string SectionName = "SectionName";
            public const string BrandName = "BrandName";
            public const string TeamName = "TeamName";
            public const string DepartmentCode = "DepartmentCode";
            public const string SectionCode = "SectionCode";
            public const string BrandCode = "BrandCode";
            public const string TeamCode = "TeamCode";
        }
    }

    public partial class Sal_Grade
    {
        #region Sal_GradeAllowance

        //BaseDataProfile _profile;
        //BaseDataProfile Profile
        //{
        //    get
        //    {
        //        if (_profile == null)
        //            _profile = Hre_Profile.Profile(this.Hre_Profile, this.ProfileID);
        //        return _profile;
        //    }
        //}

        private DateTime? _udMonthOfEffect = null;
        public DateTime? udMonthOfEffect
        {
            get
            {
                return this.MonthStart;
            }
        }
        //private string _udLinkDepartmentName = string.Empty;
        //public string udLinkDepartmentName
        //{
        //    get
        //    {
        //        _udLinkDepartmentName = string.Empty;
        //        List<Cat_Department> lstDepartment = ((List<Cat_Department>)System.Web.HttpContext.Current.Cache[CachObjectsName.ListCacheOrgStructure]);
        //        if (lstDepartment != null)
        //        {
        //            Cat_Department dDepartment = lstDepartment.FirstOrDefault(p => p.ID == this.Profile.OrgStructureID);
        //            List<string> strParent = new List<string>();
        //            while (dDepartment != null)
        //            {
        //                strParent.Add(dDepartment.DepartmentName);
        //                if (dDepartment.ParentID != null && dDepartment.ParentID != Guid.Empty)
        //                    dDepartment = lstDepartment.FirstOrDefault(p => p.ID == dDepartment.ParentID);
        //                else
        //                    dDepartment = null;
        //            }
        //            for (int i = strParent.Count - 1; i >= 0; i--)
        //            {
        //                _udLinkDepartmentName += strParent[i] + "\\";
        //            }
        //        }
        //        return _udLinkDepartmentName;
        //    }
        //    set
        //    {
        //        _udLinkDepartmentName = value;
        //    }
        //}
        //private string _udLinkDepartmentCode = string.Empty;
        //public string udLinkDepartmentCode
        //{
        //    get
        //    {
        //        _udLinkDepartmentCode = string.Empty;
        //        List<Cat_Department> lstDepartment = ((List<Cat_Department>)System.Web.HttpContext.Current.Cache[CachObjectsName.ListCacheOrgStructure]);
        //        if (lstDepartment != null)
        //        {
        //            Cat_Department dDepartment = lstDepartment.FirstOrDefault(p => p.ID == this.Profile.OrgStructureID);
        //            List<string> strParent = new List<string>();
        //            while (dDepartment != null)
        //            {
        //                strParent.Add(dDepartment.Code);
        //                if (dDepartment.ParentID != null && dDepartment.ParentID != Guid.Empty)
        //                    dDepartment = lstDepartment.FirstOrDefault(p => p.ID == dDepartment.ParentID);
        //                else
        //                    dDepartment = null;
        //            }
        //            for (int i = strParent.Count - 1; i >= 0; i--)
        //            {
        //                _udLinkDepartmentCode += strParent[i] + "\\";
        //            }
        //        }
        //        return _udLinkDepartmentCode;
        //    }
        //    set
        //    {
        //        _udLinkDepartmentCode = value;
        //    }
        //}
        //private string _udCodeEmp = string.Empty;
        //public string udCodeEmp
        //{
        //    get
        //    {
        //        if (this.Hre_Profile != null && this.Profile.CodeEmp != null)
        //            _udCodeEmp = this.Profile.CodeEmp;
        //        return _udCodeEmp;
        //    }
        //}
        //private string _udPosition = string.Empty;
        //public string udPosition
        //{
        //    get
        //    {
        //        if (this.Profile != null && this.Profile.PositionName != null)
        //            _udPosition = this.Profile.PositionName;
        //        return _udPosition;
        //    }
        //}

        //public string udCat_JobTitle
        //{
        //    get
        //    {
        //        return this.Profile.JobTitleName;
        //    }
        //}
        //public string udDepartment
        //{
        //    get
        //    {
        //        return this.Profile.OrgStructureName;
        //    }
        //}
        public string udJobTitleEN
        {
            get
            {
                if (this.Hre_Profile != null && this.Hre_Profile.Cat_JobTitle != null && this.Hre_Profile.Cat_JobTitle.JobTitleNameEn != null)
                    return this.Hre_Profile.Cat_JobTitle.JobTitleNameEn;

                return "";
            }
        }

        //private string _udProfileName = string.Empty;
        //public string udProfileName
        //{
        //    get
        //    {
        //        if (this.Profile != null)
        //        {
        //            _udProfileName = this.Profile.ProfileName;
        //        }
        //        return _udProfileName;
        //    }
        //}

        private string _udGradeConfg = string.Empty;
        public string udGradeConfg
        {
            get
            {
                if (this.Cat_GradeCfg != null)
                {
                    _udGradeConfg = this.Cat_GradeCfg.GradeCfgName;
                }
                return _udGradeConfg;
            }
        }

        private string _udEndMonth = string.Empty;
        public string udEndMonth
        {
            get
            {
                if (this.MonthEnd != null)
                {
                    _udEndMonth = string.Format("{0:MM/yyyy}", this.MonthEnd);
                }
                return _udEndMonth;
            }
        }

        public partial class FieldNames
        {
            public const string udPosition = "udPosition";
            public const string udCodeEmp = "udCodeEmp";
            public const string udProfileName = "udProfileName";
            public const string udGradeConfg = "udGradeConfg";
            public const string udLinkDepartmentCode = "udLinkDepartmentCode";
            public const string udLinkDepartmentName = "udLinkDepartmentName";
            public const string udMonthOfEffect = "udMonthOfEffect";
            public const string udEndMonth = "udEndMonth";
            public const string udCat_JobTitle = "udCat_JobTitle";
            public const string udDepartment = "udDepartment";
            public const string udJobTitleEN = "udJobTitleEN";
        }
        #endregion
    }

    public partial class Sal_SalaryDepartmentItem
    {
        #region Sal_SalaryDepartmentItem
        private Double _udOT1 = 0;
        public Double udOT1
        {
            get { return _udOT1; }
            set { _udOT1 = value; }
        }

        private Double _udOT2 = 0;
        public Double udOT2
        {
            get { return _udOT2; }
            set { _udOT2 = value; }
        }

        private Double _udOT3 = 0;
        public Double udOT3
        {
            get { return _udOT3; }
            set { _udOT3 = value; }
        }

        private Double _udOT4 = 0;
        public Double udOT4
        {
            get { return _udOT4; }
            set { _udOT4 = value; }
        }

        private Double _udOT5 = 0;
        public Double udOT5
        {
            get { return _udOT5; }
            set { _udOT5 = value; }
        }

        private Double _udOT6 = 0;
        public Double udOT6
        {
            get { return _udOT6; }
            set { _udOT6 = value; }
        }

        private Double _udTotalHoursProduct = 0;
        public Double udTotalHoursProduct
        {
            get
            {
                return _udTotalHoursProduct;
            }
            set
            {
                _udTotalHoursProduct = value;
            }
        }

        private Double _udTotalHoursOTPaid = 0;
        public Double udTotalHoursOTPaid
        {
            get
            {
                return _udTotalHoursOTPaid;
            }
            set
            {
                _udTotalHoursOTPaid = value;
            }
        }
        public string udCodeEmp
        {
            get
            {
                return this.Hre_Profile.CodeEmp;
            }
        }
        public string udRate
        {
            get
            {
                return this.Rate != null ? this.Rate.ToString() : string.Empty;
            }
        }
        public partial class FieldNames
        {
            public const string udOT1 = "udOT1";
            public const string udOT2 = "udOT2";
            public const string udOT3 = "udOT3";
            public const string udOT4 = "udOT4";
            public const string udOT5 = "udOT5";
            public const string udOT6 = "udOT6";
            public const string udTotalHoursProduct = "udTotalHoursProduct";
            public const string udTotalHoursOTPaid = "udTotalHoursOTPaid";
            public const string udCodeEmp = "udCodeEmp";
            public const string udRate = "udRate";
        }
        #endregion
    }
    public class CoupleTime
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public class FieldNames
        {
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
        }
    }


    public partial class Sal_PayrollTableItem
    {
        #region Sal_PayrollTableItem

        public Sal_PayrollTableItem(Sal_GradeAllowance alw)
        {
            this.Name = alw.Cat_UsualAllowance.UsualAllowanceName;
            this.Code = alw.Cat_UsualAllowance.Code;
            this.Value = alw.Cat_UsualAllowanceLevel.Amount.ToString();
            this.ValueType = typeof(Double).ToString();
        }

        public Sal_PayrollTableItem(Sal_UnusualAllowance alw)
        {
            this.Name = alw.Cat_UnusualAllowanceCfg.UnusualAllowanceCfgName;
            this.Code = alw.Cat_UnusualAllowanceCfg.Code;
            this.Value = alw.Amount.ToString();
            this.ValueType = typeof(Double).ToString();
        }

        public string udValue
        {
            get
            {
                if (ValueType == typeof(Double).ToString() || ValueType == typeof(double).ToString() && !String.IsNullOrEmpty(Value))
                {
                    // double value = Convert.ToDouble(Value);
                    //tan.do : kiem tra khi loi tra ve 0
                    double value = 0;
                    double.TryParse(Value, out value);
                    if (value > 1000)
                        return value.ToString("0,0.00", CultureInfo.InvariantCulture);
                    return value.ToString();
                }
                return Value;
            }
        }

        /// <summary>
        /// Gia tri so 2 so thap phan
        /// </summary>
        public string udValueN2
        {
            get
            {
                string _udValueN2 = string.Empty;

                if (ValueType == typeof(Double).ToString()
                    || ValueType == typeof(double).ToString()
                    && !String.IsNullOrEmpty(Value))
                {
                    double value = 0;
                    double.TryParse(Value, out value);

                    if (value > 1000)
                    {
                        _udValueN2 = value.ToString("0,0.00", CultureInfo.InvariantCulture);
                    }

                    _udValueN2 = value.ToString("N2");
                }
                else
                {
                    _udValueN2 = Value;
                }

                _udValueN2 = IsBold ? "<b>" + _udValueN2 + "</b>" : _udValueN2;
                return _udValueN2;
            }
        }

        public bool Invisible { get; set; }
        public bool IsBold { get; set; }
        public int DisplayIndex { get; set; }
        public int TabCount { get; set; }

        public partial class FieldNames
        {
            public const string udValue = "udValue";
            public const string udValueN2 = "udValueN2";
        }
        #endregion
    }

    public class LockObjectItem
    {
        public string ObjectName { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public byte[] OrgStructures { get; set; }
        public byte[] PayrollGroups { get; set; }
        public Guid? WorkPlaceID { get; set; }
    }
}
