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
    
    public partial class Can_TamScanLogCMS
    {
        public System.Guid ID { get; set; }
        public string CardCode { get; set; }
        public Nullable<System.DateTime> TimeLog { get; set; }
        public string Type { get; set; }
        public string SrcType { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public string MachineCode { get; set; }
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
        public string CodeEmp { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
    
        public virtual Hre_Profile Hre_Profile { get; set; }
    }
}
