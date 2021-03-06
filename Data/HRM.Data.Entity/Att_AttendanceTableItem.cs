//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRM.Data.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Att_AttendanceTableItem
    {
        public System.Guid ID { get; set; }
        public System.Guid AttendanceTableID { get; set; }
        public System.DateTime WorkDate { get; set; }
        public string DutyCode { get; set; }
        public double AvailableHours { get; set; }
        public Nullable<System.DateTime> FirstInTime { get; set; }
        public Nullable<System.DateTime> LastOutTime { get; set; }
        public Nullable<System.Guid> ShiftID { get; set; }
        public int LateEarlyMinutes { get; set; }
        public int LateInMinutes { get; set; }
        public int EarlyOutMinutes { get; set; }
        public double UnpaidLeaveHours { get; set; }
        public double PaidLeaveHours { get; set; }
        public double WorkPaidHours { get; set; }
        public double WorkHours { get; set; }
        public double NightShiftHours { get; set; }
        public Nullable<System.Guid> LeaveTypeID { get; set; }
        public double LeaveHours { get; set; }
        public Nullable<System.Guid> ExtraLeaveTypeID { get; set; }
        public double ExtraLeaveHours { get; set; }
        public Nullable<System.Guid> OvertimeTypeID { get; set; }
        public double OvertimeHours { get; set; }
        public string OvertimeDurationType { get; set; }
        public Nullable<System.Guid> ExtraOvertimeTypeID { get; set; }
        public double ExtraOvertimeHours { get; set; }
        public string ExtraOvertimeDurationType { get; set; }
        public bool IsUsingCompanyBus { get; set; }
        public bool IsHavingPregTreatment { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsExpired { get; set; }
        public string ServerUpdate { get; set; }
        public string ServerCreate { get; set; }
        public string UserUpdate { get; set; }
        public string UserCreate { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public Nullable<System.DateTime> DateUpdate { get; set; }
        public Nullable<System.Guid> UserLockID { get; set; }
        public Nullable<System.DateTime> DateLock { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string IPCreate { get; set; }
        public string IPUpdate { get; set; }
        public Nullable<System.Guid> ExtraOvertimeType2ID { get; set; }
        public Nullable<System.Guid> ExtraOvertimeType3ID { get; set; }
        public string ExtraOvertimeDurationType2 { get; set; }
        public string ExtraOvertimeDurationType3 { get; set; }
        public double ExtraOvertimeHours3 { get; set; }
        public double ExtraOvertimeHours2 { get; set; }
        public double OvertimeRegisterHours { get; set; }
        public double ExtraOvertimeRegisterHours { get; set; }
        public double ExtraOvertimeRegisterHours1 { get; set; }
        public double ExtraOvertimeRegisterHours2 { get; set; }
        public double ExtraOvertimeRegisterHours3 { get; set; }
        public Nullable<double> RealLateEarlyMinutes { get; set; }
        public Nullable<double> RealNightShiftMinutes { get; set; }
        public Nullable<System.Guid> LeaveWorkDayType { get; set; }
        public Nullable<double> LeaveWorkDayHour { get; set; }
        public Nullable<double> WorkHourFirstHaftShift { get; set; }
        public Nullable<double> WorkHourLastHaftShift { get; set; }
        public Nullable<double> OTNightShiftHours { get; set; }
        public Nullable<System.Guid> MissInOutReasonID { get; set; }
        public Nullable<double> UnpaidLeaveDays { get; set; }
        public Nullable<double> PaidLeaveDays { get; set; }
        public Nullable<double> LeaveDays { get; set; }
        public Nullable<double> ExtraLeaveDays { get; set; }
        public Nullable<double> LeaveWorkDayDays { get; set; }
        public Nullable<System.DateTime> RootInTime { get; set; }
        public Nullable<System.DateTime> RootOutTime { get; set; }
        public Nullable<double> WorkPaidHourNonPreg { get; set; }
        public Nullable<double> NightAmountSalary { get; set; }
        public Nullable<double> OT1AmountSalary { get; set; }
        public Nullable<double> OT2AmountSalary { get; set; }
        public Nullable<double> OT3AmountSalary { get; set; }
        public Nullable<double> OT4AmountSalary { get; set; }
        public Nullable<double> OT5AmountSalary { get; set; }
        public Nullable<double> OT6AmountSalary { get; set; }
        public Nullable<double> StdAmountSalary { get; set; }
        public Nullable<double> DayAmountSalary { get; set; }
        public string HDTJobType { get; set; }
    
        public virtual Att_AttendanceTable Att_AttendanceTable { get; set; }
        public virtual Cat_LeaveDayType Cat_LeaveDayType { get; set; }
        public virtual Cat_LeaveDayType Cat_LeaveDayType1 { get; set; }
        public virtual Cat_LeaveDayType Cat_LeaveDayType2 { get; set; }
        public virtual Cat_OvertimeType Cat_OvertimeType { get; set; }
        public virtual Cat_OvertimeType Cat_OvertimeType1 { get; set; }
        public virtual Cat_OvertimeType Cat_OvertimeType2 { get; set; }
        public virtual Cat_OvertimeType Cat_OvertimeType3 { get; set; }
        public virtual Cat_Shift Cat_Shift { get; set; }
    }
}
