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
    
    public partial class Sys_UserDataGroup
    {
        public System.Guid ID { get; set; }
        public string Code { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public Nullable<System.Guid> GroupID { get; set; }
        public byte[] DataGroups { get; set; }
        public string ServerUpdate { get; set; }
        public string ServerCreate { get; set; }
        public string UserUpdate { get; set; }
        public string UserCreate { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public Nullable<System.DateTime> DateUpdate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.Guid> UserLockID { get; set; }
        public Nullable<System.DateTime> DateLock { get; set; }
        public string IPCreate { get; set; }
        public string IPUpdate { get; set; }
    
        public virtual Sys_Group Sys_Group { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
    }
}