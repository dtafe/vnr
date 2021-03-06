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
    
    public partial class Sal_SalaryFund
    {
        public Sal_SalaryFund()
        {
            this.Sal_SalaryFundItem = new HashSet<Sal_SalaryFundItem>();
        }
    
        public System.Guid ID { get; set; }
        public System.Guid OrgStructureID { get; set; }
        public System.DateTime MonthYear { get; set; }
        public Nullable<double> PlanAmount { get; set; }
        public Nullable<double> ActualAmount { get; set; }
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
        public Nullable<System.DateTime> MonthYearEnd { get; set; }
    
        public virtual Cat_OrgStructure Cat_OrgStructure { get; set; }
        public virtual ICollection<Sal_SalaryFundItem> Sal_SalaryFundItem { get; set; }
    }
}
