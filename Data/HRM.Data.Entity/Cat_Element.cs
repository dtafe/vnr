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
    
    public partial class Cat_Element
    {
        public System.Guid ID { get; set; }
        public string ElementCode { get; set; }
        public string ElementName { get; set; }
        public Nullable<System.Guid> ColumnID { get; set; }
        public string Formula { get; set; }
        public Nullable<long> OrderNumber { get; set; }
        public Nullable<bool> Invisible { get; set; }
        public Nullable<bool> IsBold { get; set; }
        public Nullable<long> DisplayIndex { get; set; }
        public string Type { get; set; }
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
        public string ElementLevel { get; set; }
        public string ElementType { get; set; }
        public Nullable<System.Guid> GradePayrollID { get; set; }
        public string MethodPayroll { get; set; }
        public string TabType { get; set; }
        public Nullable<bool> IsApplyGradeAll { get; set; }
    
        public virtual Sys_DynamicColumn Sys_DynamicColumn { get; set; }
        public virtual Cat_GradePayroll Cat_GradePayroll { get; set; }
    }
}