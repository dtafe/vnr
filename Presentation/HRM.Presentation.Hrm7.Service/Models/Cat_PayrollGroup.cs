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
    
    public partial class Cat_PayrollGroup
    {
        public Cat_PayrollGroup()
        {
            this.Hre_Profile = new HashSet<Hre_Profile>();
            this.Hre_WorkHistory = new HashSet<Hre_WorkHistory>();
        }
    
        public System.Guid ID { get; set; }
        public string PayrollGroupName { get; set; }
        public string Code { get; set; }
        public Nullable<int> SalaryDateStart { get; set; }
        public Nullable<int> SalaryMonthStart { get; set; }
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
        public Nullable<System.Guid> ReportMappingID { get; set; }
        public Nullable<System.Guid> ReportMappingID1 { get; set; }
        public Nullable<int> OrderNumber { get; set; }
    
        public virtual Cat_ReportMapping Cat_ReportMapping { get; set; }
        public virtual Cat_ReportMapping Cat_ReportMapping1 { get; set; }
        public virtual ICollection<Hre_Profile> Hre_Profile { get; set; }
        public virtual ICollection<Hre_WorkHistory> Hre_WorkHistory { get; set; }
    }
}
