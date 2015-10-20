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
    
    public partial class Med_TypeResultHealth
    {
        public Med_TypeResultHealth()
        {
            this.Med_AnnualHealth = new HashSet<Med_AnnualHealth>();
            this.Med_AnnualResult = new HashSet<Med_AnnualResult>();
            this.Med_Patient = new HashSet<Med_Patient>();
        }
    
        public System.Guid ID { get; set; }
        public string TypeResultHealthName { get; set; }
        public string Description { get; set; }
        public string IPUpdate { get; set; }
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
    
        public virtual ICollection<Med_AnnualHealth> Med_AnnualHealth { get; set; }
        public virtual ICollection<Med_AnnualResult> Med_AnnualResult { get; set; }
        public virtual ICollection<Med_Patient> Med_Patient { get; set; }
    }
}
