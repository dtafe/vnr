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
    
    public partial class Eva_PerformanceEva
    {
        public Eva_PerformanceEva()
        {
            this.Eva_PerformanceEvaDetail = new HashSet<Eva_PerformanceEvaDetail>();
        }
    
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> PerformanceID { get; set; }
        public Nullable<int> OrderEva { get; set; }
        public Nullable<System.DateTime> DateEva { get; set; }
        public Nullable<System.Guid> EvaluatorID { get; set; }
        public Nullable<double> TotalMark { get; set; }
        public string Strengths { get; set; }
        public string Weaknesses { get; set; }
        public string ResultNote { get; set; }
        public Nullable<System.Guid> LevelID { get; set; }
        public Nullable<double> Proportion { get; set; }
        public string AttachFile { get; set; }
        public string Status { get; set; }
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
    
        public virtual Eva_Level Eva_Level { get; set; }
        public virtual Eva_Performance Eva_Performance { get; set; }
        public virtual ICollection<Eva_PerformanceEvaDetail> Eva_PerformanceEvaDetail { get; set; }
        public virtual Hre_Profile Hre_Profile { get; set; }
    }
}
