//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRM.Presentation.Hrm7.Service.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Att_Overtime
    {
        public Att_Overtime()
        {
            this.Att_LeaveDay = new HashSet<Att_LeaveDay>();
            this.Att_TimeOffInLieu = new HashSet<Att_TimeOffInLieu>();
        }
    
        public System.Guid ID { get; set; }
        public System.Guid ProfileID { get; set; }
        public System.Guid OvertimeTypeID { get; set; }
        public bool IsNightShift { get; set; }
        public Nullable<bool> IsConfirm { get; set; }
        public Nullable<bool> IsExcludeFromAudit { get; set; }
        public System.DateTime WorkDate { get; set; }
        public Nullable<System.DateTime> WorkDateEnd { get; set; }
        public Nullable<System.DateTime> BreakStart { get; set; }
        public Nullable<System.DateTime> BreakEnd { get; set; }
        public double RegisterHours { get; set; }
        public Nullable<double> ConvertedHours { get; set; }
        public Nullable<System.DateTime> TimeFrom { get; set; }
        public Nullable<System.DateTime> TimeTo { get; set; }
        public System.Guid UserApproveID { get; set; }
        public Nullable<System.Guid> UserConfirmID { get; set; }
        public Nullable<double> ApproveHours { get; set; }
        public string MethodPayment { get; set; }
        public string Status { get; set; }
        public string ReasonOT { get; set; }
        public string DurationType { get; set; }
        public string ConfirmComment { get; set; }
        public string ApproveComment { get; set; }
        public double ConfirmHours { get; set; }
        public string Comment { get; set; }
        public string UserRegister { get; set; }
        public Nullable<System.DateTime> DateRegister { get; set; }
        public string UserApprove { get; set; }
        public Nullable<System.DateTime> DateFirstApprove { get; set; }
        public Nullable<System.DateTime> DateApprove { get; set; }
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
        public string RegisterCode { get; set; }
        public Nullable<double> BasicHours { get; set; }
        public string SerialCode { get; set; }
        public Nullable<System.Guid> MenuID { get; set; }
        public Nullable<System.Guid> FoodID { get; set; }
        public Nullable<System.Guid> ShiftID { get; set; }
        public Nullable<System.Guid> Food2ID { get; set; }
        public Nullable<System.Guid> Menu2ID { get; set; }
        public Nullable<System.Guid> OvertimeResonID { get; set; }
        public string Label { get; set; }
        public Nullable<System.DateTime> LeaveDayDate { get; set; }
        public Nullable<System.DateTime> InTime { get; set; }
        public Nullable<System.DateTime> OutTime { get; set; }
        public Nullable<System.Guid> UserApproveID2 { get; set; }
        public Nullable<System.DateTime> WorkDateRoot { get; set; }
        public Nullable<bool> IsNotValidContinue { get; set; }
        public string SendEmailStatus { get; set; }
        public Nullable<bool> IsNotValidLimitTime { get; set; }
        public Nullable<System.Guid> SpecialReasonID { get; set; }
        public Nullable<double> AnalyseHour { get; set; }
    
        public virtual ICollection<Att_LeaveDay> Att_LeaveDay { get; set; }
        public virtual Can_Food Can_Food { get; set; }
        public virtual Can_Food Can_Food1 { get; set; }
        public virtual Can_Menu Can_Menu { get; set; }
        public virtual Can_Menu Can_Menu1 { get; set; }
        public virtual Cat_OvertimeReson Cat_OvertimeReson { get; set; }
        public virtual Cat_OvertimeReson Cat_OvertimeReson1 { get; set; }
        public virtual Cat_OvertimeType Cat_OvertimeType { get; set; }
        public virtual Cat_Shift Cat_Shift { get; set; }
        public virtual Hre_Profile Hre_Profile { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo1 { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo2 { get; set; }
        public virtual ICollection<Att_TimeOffInLieu> Att_TimeOffInLieu { get; set; }
    }
}
