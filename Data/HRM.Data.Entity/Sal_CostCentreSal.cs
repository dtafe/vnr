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
    
    public partial class Sal_CostCentreSal
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> CostCentreID { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<double> Rate { get; set; }
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
        public string ElementType { get; set; }
    
        public virtual Cat_CostCentre Cat_CostCentre { get; set; }
        public virtual Hre_Profile Hre_Profile { get; set; }
    }
}
