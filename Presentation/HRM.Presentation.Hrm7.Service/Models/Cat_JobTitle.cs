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
    
    public partial class Cat_JobTitle
    {
        public Cat_JobTitle()
        {
            this.Cat_JobTitleAllowance = new HashSet<Cat_JobTitleAllowance>();
            this.Hre_AppendixContract = new HashSet<Hre_AppendixContract>();
            this.Hre_Contract = new HashSet<Hre_Contract>();
            this.Hre_HistoryProfile = new HashSet<Hre_HistoryProfile>();
            this.Hre_HRPlanItem = new HashSet<Hre_HRPlanItem>();
            this.Hre_Profile = new HashSet<Hre_Profile>();
            this.Hre_Profile1 = new HashSet<Hre_Profile>();
            this.Hre_ProfilePromotion = new HashSet<Hre_ProfilePromotion>();
            this.Hre_ProfilePromotion1 = new HashSet<Hre_ProfilePromotion>();
            this.Hre_Relatives = new HashSet<Hre_Relatives>();
            this.Hre_WorkHistory = new HashSet<Hre_WorkHistory>();
            this.Rec_Candidate = new HashSet<Rec_Candidate>();
            this.Rec_JobVacancy = new HashSet<Rec_JobVacancy>();
        }
    
        public System.Guid ID { get; set; }
        public string JobTitleName { get; set; }
        public string JobTitleNameEn { get; set; }
        public Nullable<System.Guid> PositionID { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public Nullable<System.Guid> CostCentreID { get; set; }
        public Nullable<System.Guid> GradeCfgID { get; set; }
        public Nullable<System.Guid> SalaryJobGroupID { get; set; }
        public string JobTitleCode { get; set; }
        public Nullable<int> JobLevel { get; set; }
        public Nullable<double> SalaryRate { get; set; }
        public Nullable<bool> IsSystem { get; set; }
        public Nullable<double> SalaryMax { get; set; }
        public Nullable<double> SalaryMin { get; set; }
        public string TaskShortTerm { get; set; }
        public string TaskLongTerm { get; set; }
        public string Permission { get; set; }
        public Nullable<System.Guid> KPIID { get; set; }
        public string ReqEducation { get; set; }
        public string ReqLanguage { get; set; }
        public string ReqComputing { get; set; }
        public string ReqSkill { get; set; }
        public string ReqMain { get; set; }
        public string SkillPriority { get; set; }
        public string FileAttachment { get; set; }
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
        public string Code { get; set; }
        public string TargetAmountUnit { get; set; }
        public string ReqDocuments { get; set; }
        public Nullable<double> TargetAmount { get; set; }
        public Nullable<double> TotalSalaryBudget { get; set; }
        public Nullable<double> TotalOvertimeBudget { get; set; }
        public Nullable<int> Order { get; set; }
        public Nullable<double> AnnualDays { get; set; }
        public Nullable<double> UnusualAllowance1 { get; set; }
        public Nullable<double> UnusualAllowance2 { get; set; }
        public Nullable<double> UnusualAllowance3 { get; set; }
        public Nullable<bool> IsWorkingHard { get; set; }
        public string PermissionOvertimeType { get; set; }
        public string IndustryCode { get; set; }
        public string JobCode { get; set; }
        public string Qualification { get; set; }
        public string LaborType { get; set; }
        public string FormType { get; set; }
        public Nullable<System.Guid> EmployeeTypeID { get; set; }
        public string Notes { get; set; }
        public string JobDescription { get; set; }
    
        public virtual Cat_CostCentre Cat_CostCentre { get; set; }
        public virtual Cat_EmployeeType Cat_EmployeeType { get; set; }
        public virtual Cat_GradeCfg Cat_GradeCfg { get; set; }
        public virtual Cat_OrgStructure Cat_OrgStructure { get; set; }
        public virtual Cat_Position Cat_Position { get; set; }
        public virtual Cat_SalaryJobGroup Cat_SalaryJobGroup { get; set; }
        public virtual ICollection<Cat_JobTitleAllowance> Cat_JobTitleAllowance { get; set; }
        public virtual ICollection<Hre_AppendixContract> Hre_AppendixContract { get; set; }
        public virtual ICollection<Hre_Contract> Hre_Contract { get; set; }
        public virtual ICollection<Hre_HistoryProfile> Hre_HistoryProfile { get; set; }
        public virtual ICollection<Hre_HRPlanItem> Hre_HRPlanItem { get; set; }
        public virtual ICollection<Hre_Profile> Hre_Profile { get; set; }
        public virtual ICollection<Hre_Profile> Hre_Profile1 { get; set; }
        public virtual ICollection<Hre_ProfilePromotion> Hre_ProfilePromotion { get; set; }
        public virtual ICollection<Hre_ProfilePromotion> Hre_ProfilePromotion1 { get; set; }
        public virtual ICollection<Hre_Relatives> Hre_Relatives { get; set; }
        public virtual ICollection<Hre_WorkHistory> Hre_WorkHistory { get; set; }
        public virtual ICollection<Rec_Candidate> Rec_Candidate { get; set; }
        public virtual ICollection<Rec_JobVacancy> Rec_JobVacancy { get; set; }
    }
}
