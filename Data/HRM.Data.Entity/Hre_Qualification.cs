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
    
    public partial class Hre_Qualification
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<System.Guid> QualificationTypeID { get; set; }
        public Nullable<System.Guid> QualifiTypeID { get; set; }
        public string FieldOfTraining { get; set; }
        public string CertificateName { get; set; }
        public Nullable<System.DateTime> GraduationDate { get; set; }
        public string QualificationName { get; set; }
        public string TrainingPlace { get; set; }
        public string TrainingAddress { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateFinish { get; set; }
        public string Rank { get; set; }
        public Nullable<System.Guid> SpecialLevelID { get; set; }
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
    }
}
