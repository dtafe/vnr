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
    
    public partial class Med_Prenatal
    {
        public Med_Prenatal()
        {
            this.Med_PrenatalItem = new HashSet<Med_PrenatalItem>();
            this.Med_PrenatalResult = new HashSet<Med_PrenatalResult>();
        }
    
        public System.Guid ID { get; set; }
        public string PrenatalName { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<System.DateTime> DateReceived { get; set; }
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
        public string PrenatalCheckType { get; set; }
        public string RegistationInsurancePlace { get; set; }
        public Nullable<System.Guid> LineID { get; set; }
        public string Notes { get; set; }
        public Nullable<bool> IsMiscarriage { get; set; }
        public Nullable<System.DateTime> DateMiscarriage { get; set; }
        public Nullable<System.DateTime> DatePregnancy { get; set; }
        public Nullable<int> PregnancyTime { get; set; }
        public Nullable<System.DateTime> DateStartBeforeBirth { get; set; }
        public Nullable<System.DateTime> DateEndBeforeBirth { get; set; }
        public Nullable<System.DateTime> DateStartAfterBirth { get; set; }
        public Nullable<System.DateTime> DateEndAfterBirth { get; set; }
        public Nullable<System.DateTime> DateStartRetire { get; set; }
        public Nullable<System.DateTime> DateEndRetire { get; set; }
        public Nullable<System.DateTime> DateFirstChangeCard { get; set; }
    
        public virtual Hre_Profile Hre_Profile { get; set; }
        public virtual ICollection<Med_PrenatalItem> Med_PrenatalItem { get; set; }
        public virtual ICollection<Med_PrenatalResult> Med_PrenatalResult { get; set; }
    }
}
