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
    
    public partial class Cat_Project
    {
        public Cat_Project()
        {
            this.Att_Task = new HashSet<Att_Task>();
            this.Cat_TaskCategory = new HashSet<Cat_TaskCategory>();
        }
    
        public System.Guid ID { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
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
    
        public virtual ICollection<Att_Task> Att_Task { get; set; }
        public virtual ICollection<Cat_TaskCategory> Cat_TaskCategory { get; set; }
    }
}
