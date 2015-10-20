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
    
    public partial class Sys_Resource
    {
        public Sys_Resource()
        {
            this.Rep_Master = new HashSet<Rep_Master>();
            this.Sys_GroupPermission = new HashSet<Sys_GroupPermission>();
        }
    
        public System.Guid ID { get; set; }
        public string ResourceName { get; set; }
        public string Category { get; set; }
        public string Notes { get; set; }
        public string Code { get; set; }
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
    
        public virtual ICollection<Rep_Master> Rep_Master { get; set; }
        public virtual ICollection<Sys_GroupPermission> Sys_GroupPermission { get; set; }
    }
}
