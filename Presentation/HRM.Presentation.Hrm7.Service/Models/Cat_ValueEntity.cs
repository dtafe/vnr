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
    
    public partial class Cat_ValueEntity
    {
        public System.Guid ID { get; set; }
        public string Type { get; set; }
        public string ValueEntityName { get; set; }
        public Nullable<double> Value { get; set; }
        public Nullable<System.DateTime> DateOfEffect { get; set; }
        public Nullable<double> Value2 { get; set; }
        public Nullable<double> Value3 { get; set; }
        public string ValueString { get; set; }
        public string ValueString2 { get; set; }
        public string ValueString3 { get; set; }
        public string Comment { get; set; }
        public string UserCreate { get; set; }
        public string ServerCreate { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public string UserUpdate { get; set; }
        public string ServerUpdate { get; set; }
        public Nullable<System.DateTime> DateUpdate { get; set; }
        public Nullable<System.Guid> UserLockID { get; set; }
        public Nullable<System.DateTime> DateLock { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string IPCreate { get; set; }
        public string IPUpdate { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
    
        public virtual Cat_Currency Cat_Currency { get; set; }
    }
}