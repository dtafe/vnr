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
    
    public partial class Cat_OrgStructure
    {
        public Cat_OrgStructure()
        {
            this.Att_Roster = new HashSet<Att_Roster>();
            this.Cat_DayOff = new HashSet<Cat_DayOff>();
            this.Cat_JobTitle = new HashSet<Cat_JobTitle>();
            this.Cat_JobTitleAllowance = new HashSet<Cat_JobTitleAllowance>();
            this.Cat_LeaveDayType = new HashSet<Cat_LeaveDayType>();
            this.Cat_OrgStructure1 = new HashSet<Cat_OrgStructure>();
            this.Cat_OvertimeType = new HashSet<Cat_OvertimeType>();
            this.Cat_Position = new HashSet<Cat_Position>();
            this.Cat_ShiftItem = new HashSet<Cat_ShiftItem>();
            this.Hre_FacilityIssue = new HashSet<Hre_FacilityIssue>();
            this.Hre_HistoryProfile = new HashSet<Hre_HistoryProfile>();
            this.Hre_HRPlanItem = new HashSet<Hre_HRPlanItem>();
            this.Hre_Profile = new HashSet<Hre_Profile>();
            this.Hre_Profile1 = new HashSet<Hre_Profile>();
            this.Hre_ProfileMove = new HashSet<Hre_ProfileMove>();
            this.Hre_ProfileMove1 = new HashSet<Hre_ProfileMove>();
            this.Hre_ProfilePromotion = new HashSet<Hre_ProfilePromotion>();
            this.Hre_ProfilePromotion1 = new HashSet<Hre_ProfilePromotion>();
            this.Hre_WorkHistory = new HashSet<Hre_WorkHistory>();
            this.Hre_WorkHistory1 = new HashSet<Hre_WorkHistory>();
            this.Rec_Candidate = new HashSet<Rec_Candidate>();
            this.Rec_JobVacancy = new HashSet<Rec_JobVacancy>();
            this.Rec_JobVacancy1 = new HashSet<Rec_JobVacancy>();
            this.Rec_RecruitmentCampaignItem = new HashSet<Rec_RecruitmentCampaignItem>();
            this.Sal_SalaryDepartment = new HashSet<Sal_SalaryDepartment>();
            this.Sal_SalaryFund = new HashSet<Sal_SalaryFund>();
            this.Sal_SalaryProductLine = new HashSet<Sal_SalaryProductLine>();
            this.Sal_SchemesStructure = new HashSet<Sal_SchemesStructure>();
            this.Sys_AuditLog = new HashSet<Sys_AuditLog>();
            this.Sys_ConfigProcessApprove = new HashSet<Sys_ConfigProcessApprove>();
            this.Sys_RankByRegion = new HashSet<Sys_RankByRegion>();
            this.Sys_UserApprove = new HashSet<Sys_UserApprove>();
            this.Sys_UserInfo = new HashSet<Sys_UserInfo>();
            this.Tra_Class = new HashSet<Tra_Class>();
            this.Tra_TrainerMailReminder = new HashSet<Tra_TrainerMailReminder>();
        }
    
        public System.Guid ID { get; set; }
        public string OrgStructureName { get; set; }
        public string OrgStructureNameEN { get; set; }
        public string Code { get; set; }
        public string CodeBranch { get; set; }
        public bool IsRoot { get; set; }
        public string StandardWdType { get; set; }
        public Nullable<int> DateStart { get; set; }
        public Nullable<int> MonthStart { get; set; }
        public string AddressDetail { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public Nullable<System.Guid> TypeID { get; set; }
        public Nullable<System.Guid> ParentID { get; set; }
        public Nullable<System.Guid> BranchID { get; set; }
        public Nullable<System.Guid> ServerID { get; set; }
        public int OrderNumber { get; set; }
        public Nullable<int> OrderOrg { get; set; }
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
        public Nullable<System.DateTime> FoundationDate { get; set; }
        public string ProfessionalActivities { get; set; }
        public string FuntionDescription { get; set; }
        public Nullable<System.DateTime> TerminationDate { get; set; }
        public string Status { get; set; }
        public Nullable<int> HeadCountPlan { get; set; }
        public Nullable<double> Budget { get; set; }
        public string DecisionNo { get; set; }
        public Nullable<System.DateTime> DecisionDate { get; set; }
        public string DecisionBy { get; set; }
        public Nullable<System.DateTime> DateExpected { get; set; }
        public string Fax { get; set; }
        public Nullable<System.Guid> GroupCostCentreID { get; set; }
        public Nullable<int> OrderExcel { get; set; }
        public Nullable<System.Guid> MonShiftID { get; set; }
        public Nullable<System.Guid> TueShiftID { get; set; }
        public Nullable<System.Guid> WedShiftID { get; set; }
        public Nullable<System.Guid> ThuShiftID { get; set; }
        public Nullable<System.Guid> FriShiftID { get; set; }
        public Nullable<System.Guid> SatShiftID { get; set; }
        public Nullable<System.Guid> SunShiftID { get; set; }
        public Nullable<System.Guid> HeadDeptID { get; set; }
        public string OrgGroupName { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string FileAttach { get; set; }
        public string CodePath { get; set; }
        public string OrgNamePath { get; set; }
    
        public virtual ICollection<Att_Roster> Att_Roster { get; set; }
        public virtual Cat_CostCentre Cat_CostCentre { get; set; }
        public virtual ICollection<Cat_DayOff> Cat_DayOff { get; set; }
        public virtual ICollection<Cat_JobTitle> Cat_JobTitle { get; set; }
        public virtual ICollection<Cat_JobTitleAllowance> Cat_JobTitleAllowance { get; set; }
        public virtual ICollection<Cat_LeaveDayType> Cat_LeaveDayType { get; set; }
        public virtual ICollection<Cat_OrgStructure> Cat_OrgStructure1 { get; set; }
        public virtual Cat_OrgStructure Cat_OrgStructure2 { get; set; }
        public virtual ICollection<Cat_OvertimeType> Cat_OvertimeType { get; set; }
        public virtual ICollection<Cat_Position> Cat_Position { get; set; }
        public virtual ICollection<Cat_ShiftItem> Cat_ShiftItem { get; set; }
        public virtual ICollection<Hre_FacilityIssue> Hre_FacilityIssue { get; set; }
        public virtual ICollection<Hre_HistoryProfile> Hre_HistoryProfile { get; set; }
        public virtual ICollection<Hre_HRPlanItem> Hre_HRPlanItem { get; set; }
        public virtual ICollection<Hre_Profile> Hre_Profile { get; set; }
        public virtual ICollection<Hre_Profile> Hre_Profile1 { get; set; }
        public virtual ICollection<Hre_ProfileMove> Hre_ProfileMove { get; set; }
        public virtual ICollection<Hre_ProfileMove> Hre_ProfileMove1 { get; set; }
        public virtual ICollection<Hre_ProfilePromotion> Hre_ProfilePromotion { get; set; }
        public virtual ICollection<Hre_ProfilePromotion> Hre_ProfilePromotion1 { get; set; }
        public virtual ICollection<Hre_WorkHistory> Hre_WorkHistory { get; set; }
        public virtual ICollection<Hre_WorkHistory> Hre_WorkHistory1 { get; set; }
        public virtual ICollection<Rec_Candidate> Rec_Candidate { get; set; }
        public virtual ICollection<Rec_JobVacancy> Rec_JobVacancy { get; set; }
        public virtual ICollection<Rec_JobVacancy> Rec_JobVacancy1 { get; set; }
        public virtual ICollection<Rec_RecruitmentCampaignItem> Rec_RecruitmentCampaignItem { get; set; }
        public virtual ICollection<Sal_SalaryDepartment> Sal_SalaryDepartment { get; set; }
        public virtual ICollection<Sal_SalaryFund> Sal_SalaryFund { get; set; }
        public virtual ICollection<Sal_SalaryProductLine> Sal_SalaryProductLine { get; set; }
        public virtual ICollection<Sal_SchemesStructure> Sal_SchemesStructure { get; set; }
        public virtual ICollection<Sys_AuditLog> Sys_AuditLog { get; set; }
        public virtual ICollection<Sys_ConfigProcessApprove> Sys_ConfigProcessApprove { get; set; }
        public virtual ICollection<Sys_RankByRegion> Sys_RankByRegion { get; set; }
        public virtual ICollection<Sys_UserApprove> Sys_UserApprove { get; set; }
        public virtual ICollection<Sys_UserInfo> Sys_UserInfo { get; set; }
        public virtual ICollection<Tra_Class> Tra_Class { get; set; }
        public virtual ICollection<Tra_TrainerMailReminder> Tra_TrainerMailReminder { get; set; }
    }
}
