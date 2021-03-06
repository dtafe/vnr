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
    
    public partial class Eva_PerformanceResults
    {
        public System.Guid ID { get; set; }
        public System.Guid PerformanceReviewID { get; set; }
        public System.Guid KPIID { get; set; }
        public string KPIName { get; set; }
        public Nullable<double> MarkPerformance { get; set; }
        public string EvaluationPerformance { get; set; }
        public Nullable<double> MarkSuppervisor { get; set; }
        public string EvaluationSuppervisor { get; set; }
        public Nullable<double> MarkApproved { get; set; }
        public string EvaluationApproved { get; set; }
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
    
        public virtual Eva_KPI Eva_KPI { get; set; }
        public virtual Eva_PerformanceReview Eva_PerformanceReview { get; set; }
    }
}
