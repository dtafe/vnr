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
    
    public partial class Att_OvertimeExtra
    {
        public System.Guid ID { get; set; }
        public System.Guid ProfileID { get; set; }
        public System.Guid OvertimeTypeID { get; set; }
        public bool IsNightShift { get; set; }
        public Nullable<bool> IsConfirm { get; set; }
        public Nullable<bool> IsExcludeFromAudit { get; set; }
        public System.DateTime WorkDate { get; set; }
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
    }
}
