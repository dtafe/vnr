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
    
    public partial class Med_Patient
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<System.Guid> DiseaseID { get; set; }
        public Nullable<System.DateTime> DateOfInfection { get; set; }
        public Nullable<System.DateTime> DateTreatmentFrom { get; set; }
        public Nullable<System.DateTime> DateTreatmentTo { get; set; }
        public Nullable<System.DateTime> Status { get; set; }
        public string Description { get; set; }
        public string Symptom { get; set; }
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
        public Nullable<System.Guid> LineID { get; set; }
        public Nullable<System.Guid> TypeResultHealthID { get; set; }
        public string TypeHealthCheck { get; set; }
        public Nullable<double> Height { get; set; }
        public Nullable<double> Weight { get; set; }
    
        public virtual Hre_Profile Hre_Profile { get; set; }
        public virtual Med_Disease Med_Disease { get; set; }
        public virtual Med_TypeResultHealth Med_TypeResultHealth { get; set; }
    }
}