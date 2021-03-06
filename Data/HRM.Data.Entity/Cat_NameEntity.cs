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
    
    public partial class Cat_NameEntity
    {
        public Cat_NameEntity()
        {
            this.Att_CutOffDuration = new HashSet<Att_CutOffDuration>();
            this.Cat_Model = new HashSet<Cat_Model>();
            this.Cat_OrgStructure = new HashSet<Cat_OrgStructure>();
            this.Eva_EvalutionData = new HashSet<Eva_EvalutionData>();
            this.Eva_KPI = new HashSet<Eva_KPI>();
            this.Hre_Contract = new HashSet<Hre_Contract>();
            this.Hre_Discipline = new HashSet<Hre_Discipline>();
            this.Hre_Profile = new HashSet<Hre_Profile>();
            this.Hre_Profile1 = new HashSet<Hre_Profile>();
            this.Hre_Profile2 = new HashSet<Hre_Profile>();
            this.Hre_Profile3 = new HashSet<Hre_Profile>();
            this.Hre_ProfileComputingLevel = new HashSet<Hre_ProfileComputingLevel>();
            this.Hre_ProfileComputingLevel1 = new HashSet<Hre_ProfileComputingLevel>();
            this.Hre_ProfileLanguageLevel = new HashSet<Hre_ProfileLanguageLevel>();
            this.Hre_ProfileLanguageLevel1 = new HashSet<Hre_ProfileLanguageLevel>();
            this.Hre_ProfileLanguageLevel2 = new HashSet<Hre_ProfileLanguageLevel>();
            this.Hre_ProfileQualification = new HashSet<Hre_ProfileQualification>();
            this.Hre_StopWorking = new HashSet<Hre_StopWorking>();
            this.Hre_WorkHistory = new HashSet<Hre_WorkHistory>();
            this.Hre_WorkHistory1 = new HashSet<Hre_WorkHistory>();
            this.Cat_OrgStructure1 = new HashSet<Cat_OrgStructure>();
            this.Pur_MCAM = new HashSet<Pur_MCAM>();
            this.Rec_Candidate = new HashSet<Rec_Candidate>();
            this.Rec_Candidate1 = new HashSet<Rec_Candidate>();
            this.Rec_Candidate2 = new HashSet<Rec_Candidate>();
            this.Rec_CandidateComputingLevel = new HashSet<Rec_CandidateComputingLevel>();
            this.Rec_CandidateComputingLevel1 = new HashSet<Rec_CandidateComputingLevel>();
            this.Rec_CandidateLanguageLevel = new HashSet<Rec_CandidateLanguageLevel>();
            this.Rec_CandidateLanguageLevel1 = new HashSet<Rec_CandidateLanguageLevel>();
            this.Rec_CandidateLanguageLevel2 = new HashSet<Rec_CandidateLanguageLevel>();
            this.Rec_CandidateQualification = new HashSet<Rec_CandidateQualification>();
            this.Rec_CandidateQualification1 = new HashSet<Rec_CandidateQualification>();
            this.Rec_Interview = new HashSet<Rec_Interview>();
            this.Rec_JobVacancy = new HashSet<Rec_JobVacancy>();
            this.Rec_JobVacancy1 = new HashSet<Rec_JobVacancy>();
            this.Rec_JobVacancy2 = new HashSet<Rec_JobVacancy>();
            this.Rec_Relative = new HashSet<Rec_Relative>();
            this.Sal_HoldSalary = new HashSet<Sal_HoldSalary>();
            this.Hre_Profile4 = new HashSet<Hre_Profile>();
        }
    
        public System.Guid ID { get; set; }
        public string Code { get; set; }
        public string NameEntityName { get; set; }
        public string NameEntityType { get; set; }
        public string EnumType { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
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
        public Nullable<int> Order { get; set; }
    
        public virtual ICollection<Att_CutOffDuration> Att_CutOffDuration { get; set; }
        public virtual ICollection<Cat_Model> Cat_Model { get; set; }
        public virtual ICollection<Cat_OrgStructure> Cat_OrgStructure { get; set; }
        public virtual ICollection<Eva_EvalutionData> Eva_EvalutionData { get; set; }
        public virtual ICollection<Eva_KPI> Eva_KPI { get; set; }
        public virtual ICollection<Hre_Contract> Hre_Contract { get; set; }
        public virtual ICollection<Hre_Discipline> Hre_Discipline { get; set; }
        public virtual ICollection<Hre_Profile> Hre_Profile { get; set; }
        public virtual ICollection<Hre_Profile> Hre_Profile1 { get; set; }
        public virtual ICollection<Hre_Profile> Hre_Profile2 { get; set; }
        public virtual ICollection<Hre_Profile> Hre_Profile3 { get; set; }
        public virtual ICollection<Hre_ProfileComputingLevel> Hre_ProfileComputingLevel { get; set; }
        public virtual ICollection<Hre_ProfileComputingLevel> Hre_ProfileComputingLevel1 { get; set; }
        public virtual ICollection<Hre_ProfileLanguageLevel> Hre_ProfileLanguageLevel { get; set; }
        public virtual ICollection<Hre_ProfileLanguageLevel> Hre_ProfileLanguageLevel1 { get; set; }
        public virtual ICollection<Hre_ProfileLanguageLevel> Hre_ProfileLanguageLevel2 { get; set; }
        public virtual ICollection<Hre_ProfileQualification> Hre_ProfileQualification { get; set; }
        public virtual ICollection<Hre_StopWorking> Hre_StopWorking { get; set; }
        public virtual ICollection<Hre_WorkHistory> Hre_WorkHistory { get; set; }
        public virtual ICollection<Hre_WorkHistory> Hre_WorkHistory1 { get; set; }
        public virtual ICollection<Cat_OrgStructure> Cat_OrgStructure1 { get; set; }
        public virtual ICollection<Pur_MCAM> Pur_MCAM { get; set; }
        public virtual ICollection<Rec_Candidate> Rec_Candidate { get; set; }
        public virtual ICollection<Rec_Candidate> Rec_Candidate1 { get; set; }
        public virtual ICollection<Rec_Candidate> Rec_Candidate2 { get; set; }
        public virtual ICollection<Rec_CandidateComputingLevel> Rec_CandidateComputingLevel { get; set; }
        public virtual ICollection<Rec_CandidateComputingLevel> Rec_CandidateComputingLevel1 { get; set; }
        public virtual ICollection<Rec_CandidateLanguageLevel> Rec_CandidateLanguageLevel { get; set; }
        public virtual ICollection<Rec_CandidateLanguageLevel> Rec_CandidateLanguageLevel1 { get; set; }
        public virtual ICollection<Rec_CandidateLanguageLevel> Rec_CandidateLanguageLevel2 { get; set; }
        public virtual ICollection<Rec_CandidateQualification> Rec_CandidateQualification { get; set; }
        public virtual ICollection<Rec_CandidateQualification> Rec_CandidateQualification1 { get; set; }
        public virtual ICollection<Rec_Interview> Rec_Interview { get; set; }
        public virtual ICollection<Rec_JobVacancy> Rec_JobVacancy { get; set; }
        public virtual ICollection<Rec_JobVacancy> Rec_JobVacancy1 { get; set; }
        public virtual ICollection<Rec_JobVacancy> Rec_JobVacancy2 { get; set; }
        public virtual ICollection<Rec_Relative> Rec_Relative { get; set; }
        public virtual ICollection<Sal_HoldSalary> Sal_HoldSalary { get; set; }
        public virtual ICollection<Hre_Profile> Hre_Profile4 { get; set; }
    }
}
