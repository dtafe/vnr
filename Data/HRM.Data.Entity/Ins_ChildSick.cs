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
    
    public partial class Ins_ChildSick
    {
        public Ins_ChildSick()
        {
            this.Ins_InsuranceRecord = new HashSet<Ins_InsuranceRecord>();
        }
    
        public System.Guid ID { get; set; }
        public System.Guid ProfileID { get; set; }
        public string ChildSickName { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
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
        public Nullable<System.Guid> RelativeID { get; set; }
    
        public virtual Hre_Profile Hre_Profile { get; set; }
        public virtual ICollection<Ins_InsuranceRecord> Ins_InsuranceRecord { get; set; }
    }
}
