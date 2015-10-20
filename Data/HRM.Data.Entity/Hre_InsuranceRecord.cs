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
    
    public partial class Hre_InsuranceRecord
    {
        public System.Guid ID { get; set; }
        public System.Guid ProfileID { get; set; }
        public string InsuranceType { get; set; }
        public System.DateTime RecordDate { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public Nullable<System.DateTime> DateSuckle { get; set; }
        public string TypeSuckle { get; set; }
        public string TypeSick { get; set; }
        public Nullable<System.DateTime> DateStartWorking { get; set; }
        public double DayCount { get; set; }
        public Nullable<double> DayCountOld { get; set; }
        public Nullable<double> LeaveInYear { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
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
        public Nullable<System.Guid> RelativesID { get; set; }
        public Nullable<System.Guid> ChildSickID { get; set; }
    
        public virtual Hre_Profile Hre_Profile { get; set; }
        public virtual Hre_Relatives Hre_Relatives { get; set; }
        public virtual Ins_ChildSick Ins_ChildSick { get; set; }
    }
}
