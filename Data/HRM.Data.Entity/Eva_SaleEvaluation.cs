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
    
    public partial class Eva_SaleEvaluation
    {
        public System.Guid ID { get; set; }
        public string SaleEvaluationName { get; set; }
        public Nullable<System.DateTime> Year { get; set; }
        public string PerfixName { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public Nullable<double> TagetNumber { get; set; }
        public Nullable<double> ResultNumber { get; set; }
        public string ResultLevel { get; set; }
        public Nullable<double> BonusLevel { get; set; }
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
        public string Code { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<System.Guid> PerfixID { get; set; }
        public Nullable<double> ResultPercent { get; set; }
        public Nullable<System.Guid> SalesTypeID { get; set; }
        public string Note { get; set; }
    
        public virtual Cat_Perfix Cat_Perfix { get; set; }
        public virtual Hre_Profile Hre_Profile { get; set; }
        public virtual Eva_SalesType Eva_SalesType { get; set; }
    }
}
