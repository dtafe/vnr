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
    
    public partial class RG_TEMP_2114634907_0
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<decimal> Year { get; set; }
        public Nullable<System.DateTime> MonthStart { get; set; }
        public Nullable<System.DateTime> MonthEnd { get; set; }
        public string Type { get; set; }
        public Nullable<double> Month1 { get; set; }
        public Nullable<double> Month2 { get; set; }
        public Nullable<double> Month3 { get; set; }
        public Nullable<double> Month4 { get; set; }
        public Nullable<double> Month5 { get; set; }
        public Nullable<double> Month6 { get; set; }
        public Nullable<double> Month7 { get; set; }
        public Nullable<double> Month8 { get; set; }
        public Nullable<double> Month9 { get; set; }
        public Nullable<double> Month10 { get; set; }
        public Nullable<double> Month11 { get; set; }
        public Nullable<double> Month12 { get; set; }
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
    
        public virtual Hre_Profile Hre_Profile { get; set; }
    }
}
