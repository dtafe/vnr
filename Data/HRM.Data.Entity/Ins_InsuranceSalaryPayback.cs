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
    
    public partial class Ins_InsuranceSalaryPayback
    {
        public Ins_InsuranceSalaryPayback()
        {
            this.Ins_ReportD02 = new HashSet<Ins_ReportD02>();
            this.Ins_ReportD02Item = new HashSet<Ins_ReportD02Item>();
        }
    
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<System.DateTime> MonthYear { get; set; }
        public Nullable<System.DateTime> FromMonthEffect { get; set; }
        public Nullable<System.DateTime> ToMonthEffect { get; set; }
        public Nullable<double> InsSalary { get; set; }
        public Nullable<double> InsSalaryPayBack { get; set; }
        public Nullable<double> InsSalaryAdjust { get; set; }
        public Nullable<double> AmoutHDTIns { get; set; }
        public Nullable<double> AmoutHDTInsPayBack { get; set; }
        public string JobtitleName { get; set; }
        public Nullable<bool> IsSocialIns { get; set; }
        public Nullable<bool> IsMedicalIns { get; set; }
        public Nullable<bool> IsUnemploymentIns { get; set; }
        public Nullable<double> SocialInsEmpRate { get; set; }
        public Nullable<double> HealthInsEmpRate { get; set; }
        public Nullable<double> UnemployEmpRate { get; set; }
        public Nullable<double> SocialInsComRate { get; set; }
        public Nullable<double> HealthInsComRate { get; set; }
        public Nullable<double> UnemployComRate { get; set; }
        public string Note { get; set; }
        public Nullable<System.Guid> TypeID { get; set; }
        public string Comment { get; set; }
        public Nullable<bool> IsCallPayBack { get; set; }
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
        public Nullable<System.Guid> SocialInsPlaceID { get; set; }
    
        public virtual Cat_Province Cat_Province { get; set; }
        public virtual Hre_Profile Hre_Profile { get; set; }
        public virtual Ins_TypeD02 Ins_TypeD02 { get; set; }
        public virtual ICollection<Ins_ReportD02> Ins_ReportD02 { get; set; }
        public virtual ICollection<Ins_ReportD02Item> Ins_ReportD02Item { get; set; }
    }
}
