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
    
    public partial class Cat_Role
    {
        public Cat_Role()
        {
            this.Att_TimeSheet = new HashSet<Att_TimeSheet>();
            this.Cat_JobType = new HashSet<Cat_JobType>();
        }
    
        public System.Guid ID { get; set; }
        public string RoleName { get; set; }
        public string Code { get; set; }
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
    
        public virtual ICollection<Att_TimeSheet> Att_TimeSheet { get; set; }
        public virtual ICollection<Cat_JobType> Cat_JobType { get; set; }
    }
}