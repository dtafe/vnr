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
    
    public partial class Cat_DataGroupDetail
    {
        public System.Guid ID { get; set; }
        public System.Guid DataGroupID { get; set; }
        public string FieldName { get; set; }
        public string ChildFieldName { get; set; }
        public string Value { get; set; }
        public string Notes { get; set; }
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
        public string ObjectName { get; set; }
        public string ChildFieldName1 { get; set; }
        public string Exclusions { get; set; }
    
        public virtual Cat_DataGroup Cat_DataGroup { get; set; }
    }
}
