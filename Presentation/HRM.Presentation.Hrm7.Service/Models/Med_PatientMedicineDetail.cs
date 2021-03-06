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
    
    public partial class Med_PatientMedicineDetail
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> HistoryMedicalID { get; set; }
        public Nullable<System.Guid> MedicineID { get; set; }
        public Nullable<int> QuantilyPerDay { get; set; }
        public Nullable<double> QuantilyPerTime { get; set; }
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
        public Nullable<double> Quantily { get; set; }
        public string UnitPertime { get; set; }
        public Nullable<int> NumOrder { get; set; }
    
        public virtual Med_HistoryMedical Med_HistoryMedical { get; set; }
        public virtual Med_Medicine Med_Medicine { get; set; }
    }
}
