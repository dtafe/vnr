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
    
    public partial class Med_Vaccination
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> DiseaseID { get; set; }
        public string VaccinationName { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DateVaccination { get; set; }
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
        public Nullable<System.Guid> LineID { get; set; }
        public Nullable<System.DateTime> DateExamination { get; set; }
        public Nullable<System.DateTime> DatePreVaccination { get; set; }
        public string ReasonForStatus { get; set; }
        public string Type { get; set; }
    
        public virtual Hre_Profile Hre_Profile { get; set; }
        public virtual Med_Disease Med_Disease { get; set; }
    }
}
