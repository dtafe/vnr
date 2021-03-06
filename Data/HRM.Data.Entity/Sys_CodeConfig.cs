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
    
    public partial class Sys_CodeConfig
    {
        public Sys_CodeConfig()
        {
            this.Sys_CodeConfigItem = new HashSet<Sys_CodeConfigItem>();
        }
    
        public System.Guid ID { get; set; }
        public string ObjectName { get; set; }
        public string FieldName { get; set; }
        public string Formula { get; set; }
        public Nullable<int> Ordinal { get; set; }
        public Nullable<bool> IsOnInitObject { get; set; }
        public Nullable<bool> IsResetByDay { get; set; }
        public Nullable<bool> IsResetByMonth { get; set; }
        public Nullable<bool> IsResetByYear { get; set; }
        public Nullable<bool> IsResetByUser { get; set; }
        public Nullable<bool> IsResetByField { get; set; }
        public string ResetByField1 { get; set; }
        public string ResetByField2 { get; set; }
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
    
        public virtual ICollection<Sys_CodeConfigItem> Sys_CodeConfigItem { get; set; }
    }
}
