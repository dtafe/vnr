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
    
    public partial class Att_CutOffDuration
    {
        public Att_CutOffDuration()
        {
            this.Att_AttendanceTable = new HashSet<Att_AttendanceTable>();
            this.Att_AttendanceTable_BK = new HashSet<Att_AttendanceTable_BK>();
            this.Sal_PayCommission = new HashSet<Sal_PayCommission>();
            this.Sal_PayrollTable_BK = new HashSet<Sal_PayrollTable_BK>();
            this.Sal_PayCommission1 = new HashSet<Sal_PayCommission>();
            this.Sal_PayrollEstimate = new HashSet<Sal_PayrollEstimate>();
            this.Sal_PayrollLocker = new HashSet<Sal_PayrollLocker>();
            this.Sal_PayrollTable = new HashSet<Sal_PayrollTable>();
            this.Sal_PayrollStatusHistory = new HashSet<Sal_PayrollStatusHistory>();
            this.Sal_PayrollStatus = new HashSet<Sal_PayrollStatus>();
            this.Sal_Productive = new HashSet<Sal_Productive>();
            this.Sal_SalaryDepartment = new HashSet<Sal_SalaryDepartment>();
            this.Sal_WorkingResult = new HashSet<Sal_WorkingResult>();
        }
    
        public System.Guid ID { get; set; }
        public string CutOffDurationName { get; set; }
        public System.DateTime MonthYear { get; set; }
        public System.DateTime DateStart { get; set; }
        public System.DateTime DateEnd { get; set; }
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
        public string DurationType { get; set; }
        public Nullable<bool> IsInsuranceSocial { get; set; }
        public Nullable<System.Guid> TypeDurationID { get; set; }
        public Nullable<System.DateTime> OvertimeStart { get; set; }
        public Nullable<System.DateTime> OvertimeEnd { get; set; }
        public Nullable<System.DateTime> LeavedayStart { get; set; }
        public Nullable<System.DateTime> LeavedayEnd { get; set; }
    
        public virtual ICollection<Att_AttendanceTable> Att_AttendanceTable { get; set; }
        public virtual ICollection<Att_AttendanceTable_BK> Att_AttendanceTable_BK { get; set; }
        public virtual Cat_NameEntity Cat_NameEntity { get; set; }
        public virtual ICollection<Sal_PayCommission> Sal_PayCommission { get; set; }
        public virtual ICollection<Sal_PayrollTable_BK> Sal_PayrollTable_BK { get; set; }
        public virtual ICollection<Sal_PayCommission> Sal_PayCommission1 { get; set; }
        public virtual ICollection<Sal_PayrollEstimate> Sal_PayrollEstimate { get; set; }
        public virtual ICollection<Sal_PayrollLocker> Sal_PayrollLocker { get; set; }
        public virtual ICollection<Sal_PayrollTable> Sal_PayrollTable { get; set; }
        public virtual ICollection<Sal_PayrollStatusHistory> Sal_PayrollStatusHistory { get; set; }
        public virtual ICollection<Sal_PayrollStatus> Sal_PayrollStatus { get; set; }
        public virtual ICollection<Sal_Productive> Sal_Productive { get; set; }
        public virtual ICollection<Sal_SalaryDepartment> Sal_SalaryDepartment { get; set; }
        public virtual ICollection<Sal_WorkingResult> Sal_WorkingResult { get; set; }
    }
}
