using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_TimeOffInLieuEntity : HRMBaseModel
    {
        public System.Guid ProfileID { get; set; }
        public string Type { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<double> AccumulateLeaves { get; set; }
        public Nullable<double> UnusualLeaves { get; set; }
        public Nullable<double> TakenLeaves { get; set; }
        public Nullable<double> RemainLeaves { get; set; }
        public Nullable<System.Guid> OvertimeID { get; set; }
        public Nullable<System.Guid> LeaveDayID { get; set; }
        public Nullable<System.DateTime> DateApprove { get; set; }
        public string Notes { get; set; }
        public string OTRate { get; set; }
        public Nullable<double> OTHours { get; set; }
        public Nullable<double> LeaveHours { get; set; }
        public Nullable<double> ConvertedCashHours { get; set; }
        public Nullable<double> BalanceHours { get; set; }
        public Nullable<System.DateTime> CashOutStatus { get; set; }
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
        public Nullable<bool> IsBeginYear { get; set; }
    }
}
