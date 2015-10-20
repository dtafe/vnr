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
    
    public partial class Med_Medicine
    {
        public Med_Medicine()
        {
            this.Med_MedicineDetail = new HashSet<Med_MedicineDetail>();
            this.Med_PatientMedicineDetail = new HashSet<Med_PatientMedicineDetail>();
        }
    
        public System.Guid ID { get; set; }
        public string MedicineName { get; set; }
        public string Code { get; set; }
        public Nullable<double> Price { get; set; }
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
        public string Type { get; set; }
        public Nullable<int> NumOfMedHospital { get; set; }
        public string MedComponentName { get; set; }
        public string MedContent { get; set; }
        public string MedForm { get; set; }
        public string FirmMaker { get; set; }
        public string CountryMaker { get; set; }
        public string MedUnit { get; set; }
        public Nullable<bool> IsNewMedicine { get; set; }
        public Nullable<int> NumOrder { get; set; }
    
        public virtual ICollection<Med_MedicineDetail> Med_MedicineDetail { get; set; }
        public virtual ICollection<Med_PatientMedicineDetail> Med_PatientMedicineDetail { get; set; }
    }
}
