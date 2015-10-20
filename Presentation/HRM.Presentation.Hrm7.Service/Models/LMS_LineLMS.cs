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
    
    public partial class LMS_LineLMS
    {
        public LMS_LineLMS()
        {
            this.LMS_LaundryRecord = new HashSet<LMS_LaundryRecord>();
            this.LMS_MachineOfLineLMS = new HashSet<LMS_MachineOfLineLMS>();
        }
    
        public System.Guid ID { get; set; }
        public string Code { get; set; }
        public string LineLMSName { get; set; }
        public Nullable<System.Guid> MarkerID { get; set; }
        public Nullable<System.Guid> LockerID { get; set; }
        public string MachineCode { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Note { get; set; }
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
    
        public virtual ICollection<LMS_LaundryRecord> LMS_LaundryRecord { get; set; }
        public virtual LMS_LockerLMS LMS_LockerLMS { get; set; }
        public virtual ICollection<LMS_MachineOfLineLMS> LMS_MachineOfLineLMS { get; set; }
        public virtual LMS_Marker LMS_Marker { get; set; }
    }
}
