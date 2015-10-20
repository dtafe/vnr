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
    
    public partial class Cat_SalAdjustmentCampaign
    {
        public Cat_SalAdjustmentCampaign()
        {
            this.Sal_BasicSalary = new HashSet<Sal_BasicSalary>();
        }
    
        public System.Guid ID { get; set; }
        public string Code { get; set; }
        public string SalAdjustmentCampaignName { get; set; }
        public System.DateTime EffectiveDate { get; set; }
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
    
        public virtual ICollection<Sal_BasicSalary> Sal_BasicSalary { get; set; }
    }
}
