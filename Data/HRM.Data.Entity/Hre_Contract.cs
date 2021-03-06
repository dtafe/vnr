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
    
    public partial class Hre_Contract
    {
        public Hre_Contract()
        {
            this.Hre_AppendixContract = new HashSet<Hre_AppendixContract>();
            this.Hre_ContractExtend = new HashSet<Hre_ContractExtend>();
        }
    
        public System.Guid ID { get; set; }
        public string Code { get; set; }
        public System.Guid ProfileID { get; set; }
        public string ContractNo { get; set; }
        public System.Guid ContractTypeID { get; set; }
        public Nullable<System.DateTime> DateSigned { get; set; }
        public System.DateTime DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public Nullable<System.Guid> PositionID { get; set; }
        public string ProfessionalTitle { get; set; }
        public string MeansOfTransportation { get; set; }
        public string WorkLocation { get; set; }
        public string Attachment { get; set; }
        public Nullable<double> Salary { get; set; }
        public Nullable<double> Allowance { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public Nullable<System.Guid> ServerID { get; set; }
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
        public Nullable<System.Guid> JobTitleID { get; set; }
        public Nullable<System.Guid> RankRateID { get; set; }
        public Nullable<System.Guid> ClassRateID { get; set; }
        public Nullable<System.Guid> AllowanceID1 { get; set; }
        public Nullable<double> Allowance1 { get; set; }
        public Nullable<System.Guid> AllowanceID2 { get; set; }
        public Nullable<double> Allowance2 { get; set; }
        public Nullable<System.Guid> AllowanceID3 { get; set; }
        public Nullable<double> Allowance3 { get; set; }
        public Nullable<System.Guid> CurenncyID { get; set; }
        public Nullable<System.Guid> CurenncyID1 { get; set; }
        public Nullable<System.Guid> CurenncyID2 { get; set; }
        public Nullable<System.Guid> CurenncyID3 { get; set; }
        public Nullable<System.Guid> CurenncyIDSalary { get; set; }
        public Nullable<double> PersonalRate { get; set; }
        public Nullable<System.Guid> AllowanceID4 { get; set; }
        public Nullable<double> Allowance4 { get; set; }
        public Nullable<System.Guid> CurenncyID4 { get; set; }
        public Nullable<System.Guid> QualificationID { get; set; }
        public string ProfileSing { get; set; }
        public Nullable<double> InsuranceAmount { get; set; }
        public Nullable<System.Guid> SalaryClassTypeID { get; set; }
        public string FormPaySalary { get; set; }
        public Nullable<System.Guid> ProfileSingID { get; set; }
        public Nullable<System.Guid> ProfileAuthorizeID { get; set; }
        public Nullable<double> HourWorkInMonth { get; set; }
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        public string FollowNo { get; set; }
        public Nullable<System.Guid> AllowanceID5 { get; set; }
        public Nullable<double> Allowance5 { get; set; }
        public Nullable<System.Guid> CurenncyID5 { get; set; }
        public Nullable<System.DateTime> DateAuthorize { get; set; }
        public Nullable<double> Bonus { get; set; }
        public string KPI { get; set; }
        public Nullable<System.Guid> MethodWorkID { get; set; }
        public Nullable<System.Guid> CurenncyID6 { get; set; }
        public string JobDescription { get; set; }
        public string Status { get; set; }
        public string ContractEvaType { get; set; }
        public Nullable<System.DateTime> DateOfContractEva { get; set; }
        public string EvaluationResult { get; set; }
        public string ContractResult { get; set; }
        public Nullable<int> DayContract { get; set; }
        public Nullable<int> DayExtend { get; set; }
        public Nullable<System.Guid> RankDetailForNextContract { get; set; }
        public string TypeOfPass { get; set; }
        public Nullable<System.Guid> NextContractTypeID { get; set; }
        public string Remark { get; set; }
        public string TypeSignNext { get; set; }
        public Nullable<System.DateTime> DateEndNextContract { get; set; }
        public Nullable<System.DateTime> TerminateDate { get; set; }
        public Nullable<int> NoPrint { get; set; }
        public string TypeHourWork { get; set; }
        public Nullable<System.Guid> AllowanceID6 { get; set; }
        public Nullable<System.DateTime> DateExtend { get; set; }
        public string StatusEvaluation { get; set; }
        public Nullable<int> TimesContract { get; set; }
    
        public virtual Cat_ContractType Cat_ContractType { get; set; }
        public virtual Cat_ContractType Cat_ContractType1 { get; set; }
        public virtual Cat_Currency Cat_Currency { get; set; }
        public virtual Cat_Currency Cat_Currency1 { get; set; }
        public virtual Cat_Currency Cat_Currency2 { get; set; }
        public virtual Cat_Currency Cat_Currency3 { get; set; }
        public virtual Cat_Currency Cat_Currency4 { get; set; }
        public virtual Cat_Currency Cat_Currency5 { get; set; }
        public virtual Cat_Currency Cat_Currency6 { get; set; }
        public virtual Cat_Currency Cat_Currency7 { get; set; }
        public virtual Cat_Currency Cat_Currency8 { get; set; }
        public virtual Cat_JobTitle Cat_JobTitle { get; set; }
        public virtual Cat_NameEntity Cat_NameEntity { get; set; }
        public virtual Cat_Position Cat_Position { get; set; }
        public virtual Cat_Qualification Cat_Qualification { get; set; }
        public virtual Cat_SalaryClass Cat_SalaryClass { get; set; }
        public virtual Cat_SalaryClassType Cat_SalaryClassType { get; set; }
        public virtual Cat_SalaryRank Cat_SalaryRank { get; set; }
        public virtual Cat_SalaryRank Cat_SalaryRank1 { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance1 { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance2 { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance3 { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance4 { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance5 { get; set; }
        public virtual Cat_WorkPlace Cat_WorkPlace { get; set; }
        public virtual ICollection<Hre_AppendixContract> Hre_AppendixContract { get; set; }
        public virtual Hre_Profile Hre_Profile { get; set; }
        public virtual Hre_Profile Hre_Profile1 { get; set; }
        public virtual Hre_Profile Hre_Profile2 { get; set; }
        public virtual ICollection<Hre_ContractExtend> Hre_ContractExtend { get; set; }
    }
}
