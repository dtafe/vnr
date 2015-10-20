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
    
    public partial class Ins_ReportD02Item
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> ReportD02ID { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<double> OldBasicSalary { get; set; }
        public Nullable<double> NewBasicSalary { get; set; }
        public Nullable<double> RateSocialIns { get; set; }
        public Nullable<double> RateHealthIns { get; set; }
        public Nullable<double> RateUnEmpIns { get; set; }
        public Nullable<bool> NotCardHealth { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> MonthFrom { get; set; }
        public Nullable<System.DateTime> MonthTo { get; set; }
        public Nullable<System.DateTime> MonthConvertRecord { get; set; }
        public Nullable<int> ItemOrder { get; set; }
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
        public Nullable<bool> IsUserCreate { get; set; }
        public string JobName { get; set; }
        public Nullable<double> Allowance1 { get; set; }
        public Nullable<double> Allowance2 { get; set; }
        public Nullable<double> Allowance3 { get; set; }
        public Nullable<double> AllowanceAdditional { get; set; }
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        public Nullable<System.Guid> SocialInsPlaceID { get; set; }
        public Nullable<bool> IsPayBack { get; set; }
        public Nullable<System.Guid> PayBackID { get; set; }
    
        public virtual Cat_Province Cat_Province { get; set; }
        public virtual Cat_WorkPlace Cat_WorkPlace { get; set; }
        public virtual Hre_Profile Hre_Profile { get; set; }
        public virtual Ins_InsuranceSalaryPayback Ins_InsuranceSalaryPayback { get; set; }
        public virtual Ins_ReportD02 Ins_ReportD02 { get; set; }
    }
}
