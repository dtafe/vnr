﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HondaEntities : DbContext
    {
        public HondaEntities()
            : base("name=HondaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Att_AdvanceTableItem> Att_AdvanceTableItem { get; set; }
        public virtual DbSet<Att_AllowLimitOvertime> Att_AllowLimitOvertime { get; set; }
        public virtual DbSet<Att_AnnualLeave> Att_AnnualLeave { get; set; }
        public virtual DbSet<Att_AnnualLeaveDetail> Att_AnnualLeaveDetail { get; set; }
        public virtual DbSet<Att_AttendanceTable> Att_AttendanceTable { get; set; }
        public virtual DbSet<Att_AttendanceTableItem> Att_AttendanceTableItem { get; set; }
        public virtual DbSet<Att_AttendanceTableNote> Att_AttendanceTableNote { get; set; }
        public virtual DbSet<Att_BusinessTrip> Att_BusinessTrip { get; set; }
        public virtual DbSet<Att_CompanyCarSchedule> Att_CompanyCarSchedule { get; set; }
        public virtual DbSet<Att_CutOffDuration> Att_CutOffDuration { get; set; }
        public virtual DbSet<Att_InOut> Att_InOut { get; set; }
        public virtual DbSet<Att_LeaveDay> Att_LeaveDay { get; set; }
        public virtual DbSet<Att_Overtime> Att_Overtime { get; set; }
        public virtual DbSet<Att_OvertimeExtra> Att_OvertimeExtra { get; set; }
        public virtual DbSet<Att_Pregnancy> Att_Pregnancy { get; set; }
        public virtual DbSet<Att_Productivity> Att_Productivity { get; set; }
        public virtual DbSet<Att_Roster> Att_Roster { get; set; }
        public virtual DbSet<Att_RosterGroup> Att_RosterGroup { get; set; }
        public virtual DbSet<Att_TAMScan> Att_TAMScan { get; set; }
        public virtual DbSet<Att_TAMScanLog> Att_TAMScanLog { get; set; }
        public virtual DbSet<Att_Task> Att_Task { get; set; }
        public virtual DbSet<Att_TimeOffInLieu> Att_TimeOffInLieu { get; set; }
        public virtual DbSet<Att_Workday> Att_Workday { get; set; }
        public virtual DbSet<Can_BackPay> Can_BackPay { get; set; }
        public virtual DbSet<Can_Canteen> Can_Canteen { get; set; }
        public virtual DbSet<Can_Catering> Can_Catering { get; set; }
        public virtual DbSet<Can_Food> Can_Food { get; set; }
        public virtual DbSet<Can_FoodRegister> Can_FoodRegister { get; set; }
        public virtual DbSet<Can_Line> Can_Line { get; set; }
        public virtual DbSet<Can_Location> Can_Location { get; set; }
        public virtual DbSet<Can_MachineOfLine> Can_MachineOfLine { get; set; }
        public virtual DbSet<Can_MealAllowanceToMoney> Can_MealAllowanceToMoney { get; set; }
        public virtual DbSet<Can_MealAllowanceTypeSetting> Can_MealAllowanceTypeSetting { get; set; }
        public virtual DbSet<Can_MealRecord> Can_MealRecord { get; set; }
        public virtual DbSet<Can_MealRecordMissing> Can_MealRecordMissing { get; set; }
        public virtual DbSet<Can_Menu> Can_Menu { get; set; }
        public virtual DbSet<Can_MenuFood> Can_MenuFood { get; set; }
        public virtual DbSet<Can_TamScanLog> Can_TamScanLog { get; set; }
        public virtual DbSet<Can_TamScanLogCMS> Can_TamScanLogCMS { get; set; }
        public virtual DbSet<Cat_AccidentType> Cat_AccidentType { get; set; }
        public virtual DbSet<Cat_AccountBooking> Cat_AccountBooking { get; set; }
        public virtual DbSet<Cat_AccountType> Cat_AccountType { get; set; }
        public virtual DbSet<Cat_AppendixContractType> Cat_AppendixContractType { get; set; }
        public virtual DbSet<Cat_ArmedForceTypes> Cat_ArmedForceTypes { get; set; }
        public virtual DbSet<Cat_Attachment> Cat_Attachment { get; set; }
        public virtual DbSet<Cat_Bank> Cat_Bank { get; set; }
        public virtual DbSet<Cat_Budget> Cat_Budget { get; set; }
        public virtual DbSet<Cat_BusinessTripPurpose> Cat_BusinessTripPurpose { get; set; }
        public virtual DbSet<Cat_BusinessTrips> Cat_BusinessTrips { get; set; }
        public virtual DbSet<Cat_Category> Cat_Category { get; set; }
        public virtual DbSet<Cat_CommunistPartyPosition> Cat_CommunistPartyPosition { get; set; }
        public virtual DbSet<Cat_ContractType> Cat_ContractType { get; set; }
        public virtual DbSet<Cat_CostCentre> Cat_CostCentre { get; set; }
        public virtual DbSet<Cat_Country> Cat_Country { get; set; }
        public virtual DbSet<Cat_Currency> Cat_Currency { get; set; }
        public virtual DbSet<Cat_DataGroup> Cat_DataGroup { get; set; }
        public virtual DbSet<Cat_DataGroupDetail> Cat_DataGroupDetail { get; set; }
        public virtual DbSet<Cat_DayOff> Cat_DayOff { get; set; }
        public virtual DbSet<Cat_DisciplinedDecidingOrgs> Cat_DisciplinedDecidingOrgs { get; set; }
        public virtual DbSet<Cat_DisciplinedTypes> Cat_DisciplinedTypes { get; set; }
        public virtual DbSet<Cat_District> Cat_District { get; set; }
        public virtual DbSet<Cat_EmployeeType> Cat_EmployeeType { get; set; }
        public virtual DbSet<Cat_EntityLabel> Cat_EntityLabel { get; set; }
        public virtual DbSet<Cat_EthnicGroup> Cat_EthnicGroup { get; set; }
        public virtual DbSet<Cat_ExchangeRate> Cat_ExchangeRate { get; set; }
        public virtual DbSet<Cat_ExtensionInfo> Cat_ExtensionInfo { get; set; }
        public virtual DbSet<Cat_FacilityIssuesCategory> Cat_FacilityIssuesCategory { get; set; }
        public virtual DbSet<Cat_FacilityIssueTemplate> Cat_FacilityIssueTemplate { get; set; }
        public virtual DbSet<Cat_GradeAllowance> Cat_GradeAllowance { get; set; }
        public virtual DbSet<Cat_GradeCfg> Cat_GradeCfg { get; set; }
        public virtual DbSet<Cat_HDTJobType> Cat_HDTJobType { get; set; }
        public virtual DbSet<Cat_Interface> Cat_Interface { get; set; }
        public virtual DbSet<Cat_InterfaceItem> Cat_InterfaceItem { get; set; }
        public virtual DbSet<Cat_JobTitle> Cat_JobTitle { get; set; }
        public virtual DbSet<Cat_JobTitleAllowance> Cat_JobTitleAllowance { get; set; }
        public virtual DbSet<Cat_JobTitleMapping> Cat_JobTitleMapping { get; set; }
        public virtual DbSet<Cat_JobTitleMarket> Cat_JobTitleMarket { get; set; }
        public virtual DbSet<Cat_LateEarlyRule> Cat_LateEarlyRule { get; set; }
        public virtual DbSet<Cat_LeaveDayType> Cat_LeaveDayType { get; set; }
        public virtual DbSet<Cat_Locker> Cat_Locker { get; set; }
        public virtual DbSet<Cat_MappingAllowance> Cat_MappingAllowance { get; set; }
        public virtual DbSet<Cat_NameEntity> Cat_NameEntity { get; set; }
        public virtual DbSet<Cat_NonConformingRecord> Cat_NonConformingRecord { get; set; }
        public virtual DbSet<Cat_OrgStructure> Cat_OrgStructure { get; set; }
        public virtual DbSet<Cat_OvertimeReson> Cat_OvertimeReson { get; set; }
        public virtual DbSet<Cat_OvertimeType> Cat_OvertimeType { get; set; }
        public virtual DbSet<Cat_PayrollElement> Cat_PayrollElement { get; set; }
        public virtual DbSet<Cat_PayrollElementReplace> Cat_PayrollElementReplace { get; set; }
        public virtual DbSet<Cat_PayrollGroup> Cat_PayrollGroup { get; set; }
        public virtual DbSet<Cat_Perfix> Cat_Perfix { get; set; }
        public virtual DbSet<Cat_PerformanceType> Cat_PerformanceType { get; set; }
        public virtual DbSet<Cat_PITConfig> Cat_PITConfig { get; set; }
        public virtual DbSet<Cat_PITFormula> Cat_PITFormula { get; set; }
        public virtual DbSet<Cat_PlanType> Cat_PlanType { get; set; }
        public virtual DbSet<Cat_PoliticalLevel> Cat_PoliticalLevel { get; set; }
        public virtual DbSet<Cat_Position> Cat_Position { get; set; }
        public virtual DbSet<Cat_Process> Cat_Process { get; set; }
        public virtual DbSet<Cat_Product> Cat_Product { get; set; }
        public virtual DbSet<Cat_ProductionLine> Cat_ProductionLine { get; set; }
        public virtual DbSet<Cat_ProductItem> Cat_ProductItem { get; set; }
        public virtual DbSet<Cat_ProductType> Cat_ProductType { get; set; }
        public virtual DbSet<Cat_Project> Cat_Project { get; set; }
        public virtual DbSet<Cat_Province> Cat_Province { get; set; }
        public virtual DbSet<Cat_Qualification> Cat_Qualification { get; set; }
        public virtual DbSet<Cat_RateInsurance> Cat_RateInsurance { get; set; }
        public virtual DbSet<Cat_Region> Cat_Region { get; set; }
        public virtual DbSet<Cat_RelativeType> Cat_RelativeType { get; set; }
        public virtual DbSet<Cat_Religion> Cat_Religion { get; set; }
        public virtual DbSet<Cat_ReportMapping> Cat_ReportMapping { get; set; }
        public virtual DbSet<Cat_ReportMappingItem> Cat_ReportMappingItem { get; set; }
        public virtual DbSet<Cat_ReqDocument> Cat_ReqDocument { get; set; }
        public virtual DbSet<Cat_ResignReason> Cat_ResignReason { get; set; }
        public virtual DbSet<Cat_RewardedDecidingOrgs> Cat_RewardedDecidingOrgs { get; set; }
        public virtual DbSet<Cat_RewardedTime> Cat_RewardedTime { get; set; }
        public virtual DbSet<Cat_RewardedTitles> Cat_RewardedTitles { get; set; }
        public virtual DbSet<Cat_RewardedType> Cat_RewardedType { get; set; }
        public virtual DbSet<Cat_SalAdjustmentCampaign> Cat_SalAdjustmentCampaign { get; set; }
        public virtual DbSet<Cat_SalaryClass> Cat_SalaryClass { get; set; }
        public virtual DbSet<Cat_SalaryClassType> Cat_SalaryClassType { get; set; }
        public virtual DbSet<Cat_SalaryJobGroup> Cat_SalaryJobGroup { get; set; }
        public virtual DbSet<Cat_SalaryMarket> Cat_SalaryMarket { get; set; }
        public virtual DbSet<Cat_SalaryMarketDetail> Cat_SalaryMarketDetail { get; set; }
        public virtual DbSet<Cat_SalaryRank> Cat_SalaryRank { get; set; }
        public virtual DbSet<Cat_Section> Cat_Section { get; set; }
        public virtual DbSet<Cat_SelfDefenceMilitiaPosition> Cat_SelfDefenceMilitiaPosition { get; set; }
        public virtual DbSet<Cat_Shift> Cat_Shift { get; set; }
        public virtual DbSet<Cat_ShiftItem> Cat_ShiftItem { get; set; }
        public virtual DbSet<Cat_SocialInsurance> Cat_SocialInsurance { get; set; }
        public virtual DbSet<Cat_SourceAds> Cat_SourceAds { get; set; }
        public virtual DbSet<Cat_Symbol> Cat_Symbol { get; set; }
        public virtual DbSet<Cat_TAMScanReasonMiss> Cat_TAMScanReasonMiss { get; set; }
        public virtual DbSet<Cat_TaskCategory> Cat_TaskCategory { get; set; }
        public virtual DbSet<Cat_Team> Cat_Team { get; set; }
        public virtual DbSet<Cat_TemplateChart> Cat_TemplateChart { get; set; }
        public virtual DbSet<Cat_TemplateChartColumn> Cat_TemplateChartColumn { get; set; }
        public virtual DbSet<Cat_TemplateChartItem> Cat_TemplateChartItem { get; set; }
        public virtual DbSet<Cat_Topic> Cat_Topic { get; set; }
        public virtual DbSet<Cat_TradeUnionistPosition> Cat_TradeUnionistPosition { get; set; }
        public virtual DbSet<Cat_TrainCategory> Cat_TrainCategory { get; set; }
        public virtual DbSet<Cat_TrainLevel> Cat_TrainLevel { get; set; }
        public virtual DbSet<Cat_UnusualAllowanceCfg> Cat_UnusualAllowanceCfg { get; set; }
        public virtual DbSet<Cat_UsualAllowance> Cat_UsualAllowance { get; set; }
        public virtual DbSet<Cat_UsualAllowanceLevel> Cat_UsualAllowanceLevel { get; set; }
        public virtual DbSet<Cat_ValueEntity> Cat_ValueEntity { get; set; }
        public virtual DbSet<Cat_VeteranUnionPosition> Cat_VeteranUnionPosition { get; set; }
        public virtual DbSet<Cat_Village> Cat_Village { get; set; }
        public virtual DbSet<Cat_WorkGroup> Cat_WorkGroup { get; set; }
        public virtual DbSet<Cat_WorkPlace> Cat_WorkPlace { get; set; }
        public virtual DbSet<Cat_WoundedSoldierTypes> Cat_WoundedSoldierTypes { get; set; }
        public virtual DbSet<Cat_YouthUnionPosition> Cat_YouthUnionPosition { get; set; }
        public virtual DbSet<Eva_KPI> Eva_KPI { get; set; }
        public virtual DbSet<Eva_Level> Eva_Level { get; set; }
        public virtual DbSet<Eva_PerformanceDetail> Eva_PerformanceDetail { get; set; }
        public virtual DbSet<Eva_PerformancePlan> Eva_PerformancePlan { get; set; }
        public virtual DbSet<Eva_PerformanceResults> Eva_PerformanceResults { get; set; }
        public virtual DbSet<Eva_PerformanceReview> Eva_PerformanceReview { get; set; }
        public virtual DbSet<Eva_PerformanceTemplate> Eva_PerformanceTemplate { get; set; }
        public virtual DbSet<Eva_SaleEvaluation> Eva_SaleEvaluation { get; set; }
        public virtual DbSet<Eva_TagEva> Eva_TagEva { get; set; }
        public virtual DbSet<Hre_Accident> Hre_Accident { get; set; }
        public virtual DbSet<Hre_AppendixContract> Hre_AppendixContract { get; set; }
        public virtual DbSet<Hre_ArchiveProfile> Hre_ArchiveProfile { get; set; }
        public virtual DbSet<Hre_BusinessTrip> Hre_BusinessTrip { get; set; }
        public virtual DbSet<Hre_BusinessTripCosting> Hre_BusinessTripCosting { get; set; }
        public virtual DbSet<Hre_BusinessTripProfile> Hre_BusinessTripProfile { get; set; }
        public virtual DbSet<Hre_CandidateHistory> Hre_CandidateHistory { get; set; }
        public virtual DbSet<Hre_CardHistory> Hre_CardHistory { get; set; }
        public virtual DbSet<Hre_Contract> Hre_Contract { get; set; }
        public virtual DbSet<Hre_Dependant> Hre_Dependant { get; set; }
        public virtual DbSet<Hre_Discipline> Hre_Discipline { get; set; }
        public virtual DbSet<Hre_Facility> Hre_Facility { get; set; }
        public virtual DbSet<Hre_FacilityInventory> Hre_FacilityInventory { get; set; }
        public virtual DbSet<Hre_FacilityIssue> Hre_FacilityIssue { get; set; }
        public virtual DbSet<Hre_FacilityItem> Hre_FacilityItem { get; set; }
        public virtual DbSet<Hre_FacilityUsed> Hre_FacilityUsed { get; set; }
        public virtual DbSet<Hre_HDTJob> Hre_HDTJob { get; set; }
        public virtual DbSet<Hre_HistoryProfile> Hre_HistoryProfile { get; set; }
        public virtual DbSet<Hre_HRPlan> Hre_HRPlan { get; set; }
        public virtual DbSet<Hre_HRPlanItem> Hre_HRPlanItem { get; set; }
        public virtual DbSet<Hre_InsuranceRecord> Hre_InsuranceRecord { get; set; }
        public virtual DbSet<Hre_Profile> Hre_Profile { get; set; }
        public virtual DbSet<Hre_ProfileBusinessTrip> Hre_ProfileBusinessTrip { get; set; }
        public virtual DbSet<Hre_ProfileCertificate> Hre_ProfileCertificate { get; set; }
        public virtual DbSet<Hre_ProfileComputingLevel> Hre_ProfileComputingLevel { get; set; }
        public virtual DbSet<Hre_ProfileLanguageLevel> Hre_ProfileLanguageLevel { get; set; }
        public virtual DbSet<Hre_ProfileMove> Hre_ProfileMove { get; set; }
        public virtual DbSet<Hre_ProfilePartyUnion> Hre_ProfilePartyUnion { get; set; }
        public virtual DbSet<Hre_ProfileProductItem> Hre_ProfileProductItem { get; set; }
        public virtual DbSet<Hre_ProfilePromotion> Hre_ProfilePromotion { get; set; }
        public virtual DbSet<Hre_ProfileQualification> Hre_ProfileQualification { get; set; }
        public virtual DbSet<Hre_Relatives> Hre_Relatives { get; set; }
        public virtual DbSet<Hre_Reward> Hre_Reward { get; set; }
        public virtual DbSet<Hre_SoftSkill> Hre_SoftSkill { get; set; }
        public virtual DbSet<Hre_Uniform> Hre_Uniform { get; set; }
        public virtual DbSet<Hre_WorkHistory> Hre_WorkHistory { get; set; }
        public virtual DbSet<Ins_ChildSick> Ins_ChildSick { get; set; }
        public virtual DbSet<Ins_InsuranceSalary> Ins_InsuranceSalary { get; set; }
        public virtual DbSet<Ins_LeaveDayIns> Ins_LeaveDayIns { get; set; }
        public virtual DbSet<Ins_ReportD02> Ins_ReportD02 { get; set; }
        public virtual DbSet<Ins_ReportD02Item> Ins_ReportD02Item { get; set; }
        public virtual DbSet<Med_AnnualElement> Med_AnnualElement { get; set; }
        public virtual DbSet<Med_AnnualHealth> Med_AnnualHealth { get; set; }
        public virtual DbSet<Med_AnnualResult> Med_AnnualResult { get; set; }
        public virtual DbSet<Med_Disease> Med_Disease { get; set; }
        public virtual DbSet<Med_Epidemic> Med_Epidemic { get; set; }
        public virtual DbSet<Med_HistoryMedical> Med_HistoryMedical { get; set; }
        public virtual DbSet<Med_Medicine> Med_Medicine { get; set; }
        public virtual DbSet<Med_MedicineDetail> Med_MedicineDetail { get; set; }
        public virtual DbSet<Med_Patient> Med_Patient { get; set; }
        public virtual DbSet<Med_PatientMedicineDetail> Med_PatientMedicineDetail { get; set; }
        public virtual DbSet<Med_Prenatal> Med_Prenatal { get; set; }
        public virtual DbSet<Med_PrenatalItem> Med_PrenatalItem { get; set; }
        public virtual DbSet<Med_PrenatalResult> Med_PrenatalResult { get; set; }
        public virtual DbSet<Med_TypeResultHealth> Med_TypeResultHealth { get; set; }
        public virtual DbSet<Med_Vaccination> Med_Vaccination { get; set; }
        public virtual DbSet<Rec_Candidate> Rec_Candidate { get; set; }
        public virtual DbSet<Rec_CandidateBusiness> Rec_CandidateBusiness { get; set; }
        public virtual DbSet<Rec_CandidateComputingLevel> Rec_CandidateComputingLevel { get; set; }
        public virtual DbSet<Rec_CandidateHistory> Rec_CandidateHistory { get; set; }
        public virtual DbSet<Rec_CandidateLanguageLevel> Rec_CandidateLanguageLevel { get; set; }
        public virtual DbSet<Rec_CandidateQualification> Rec_CandidateQualification { get; set; }
        public virtual DbSet<Rec_CandidateSourceAds> Rec_CandidateSourceAds { get; set; }
        public virtual DbSet<Rec_Department> Rec_Department { get; set; }
        public virtual DbSet<Rec_Interview> Rec_Interview { get; set; }
        public virtual DbSet<Rec_InterviewCampaign> Rec_InterviewCampaign { get; set; }
        public virtual DbSet<Rec_InterviewHistory> Rec_InterviewHistory { get; set; }
        public virtual DbSet<Rec_Investigation> Rec_Investigation { get; set; }
        public virtual DbSet<Rec_JobVacancy> Rec_JobVacancy { get; set; }
        public virtual DbSet<Rec_Question> Rec_Question { get; set; }
        public virtual DbSet<Rec_Questionaire> Rec_Questionaire { get; set; }
        public virtual DbSet<Rec_QuestionaireJobVacancy> Rec_QuestionaireJobVacancy { get; set; }
        public virtual DbSet<Rec_QuestionBank> Rec_QuestionBank { get; set; }
        public virtual DbSet<Rec_QuestionChoice> Rec_QuestionChoice { get; set; }
        public virtual DbSet<Rec_QuestionType> Rec_QuestionType { get; set; }
        public virtual DbSet<Rec_RecruitmentCampaign> Rec_RecruitmentCampaign { get; set; }
        public virtual DbSet<Rec_RecruitmentCampaignItem> Rec_RecruitmentCampaignItem { get; set; }
        public virtual DbSet<Rec_RecWorkHistory> Rec_RecWorkHistory { get; set; }
        public virtual DbSet<Rec_Reference> Rec_Reference { get; set; }
        public virtual DbSet<Rec_Relative> Rec_Relative { get; set; }
        public virtual DbSet<Rec_Response> Rec_Response { get; set; }
        public virtual DbSet<Rec_ResponseBool> Rec_ResponseBool { get; set; }
        public virtual DbSet<Rec_ResponseMultiple> Rec_ResponseMultiple { get; set; }
        public virtual DbSet<Rec_ResponseRank> Rec_ResponseRank { get; set; }
        public virtual DbSet<Rec_ResponseSingle> Rec_ResponseSingle { get; set; }
        public virtual DbSet<Rec_ResponseText> Rec_ResponseText { get; set; }
        public virtual DbSet<Rec_SoftSkill> Rec_SoftSkill { get; set; }
        public virtual DbSet<Rec_SourceAds> Rec_SourceAds { get; set; }
        public virtual DbSet<Rec_SupplerRelation> Rec_SupplerRelation { get; set; }
        public virtual DbSet<Rec_Tag> Rec_Tag { get; set; }
        public virtual DbSet<Rep_Column> Rep_Column { get; set; }
        public virtual DbSet<Rep_Condition> Rep_Condition { get; set; }
        public virtual DbSet<Rep_ConditionItem> Rep_ConditionItem { get; set; }
        public virtual DbSet<Rep_Master> Rep_Master { get; set; }
        public virtual DbSet<RG_TEMP_2114634907_0> RG_TEMP_2114634907_0 { get; set; }
        public virtual DbSet<Sal_BasicSalary> Sal_BasicSalary { get; set; }
        public virtual DbSet<Sal_CodeAlocal> Sal_CodeAlocal { get; set; }
        public virtual DbSet<Sal_CostByAccountNoTemp> Sal_CostByAccountNoTemp { get; set; }
        public virtual DbSet<Sal_CostCentreSal> Sal_CostCentreSal { get; set; }
        public virtual DbSet<Sal_Grade> Sal_Grade { get; set; }
        public virtual DbSet<Sal_GradeAllowance> Sal_GradeAllowance { get; set; }
        public virtual DbSet<Sal_PayrollLocker> Sal_PayrollLocker { get; set; }
        public virtual DbSet<Sal_PayrollTable> Sal_PayrollTable { get; set; }
        public virtual DbSet<Sal_PayrollTableAddition> Sal_PayrollTableAddition { get; set; }
        public virtual DbSet<Sal_PayrollTableFull> Sal_PayrollTableFull { get; set; }
        public virtual DbSet<Sal_PayrollTableFullItem> Sal_PayrollTableFullItem { get; set; }
        public virtual DbSet<Sal_PayrollTableItem> Sal_PayrollTableItem { get; set; }
        public virtual DbSet<Sal_Productive> Sal_Productive { get; set; }
        public virtual DbSet<Sal_ProductiveImport> Sal_ProductiveImport { get; set; }
        public virtual DbSet<Sal_SalaryDepartment> Sal_SalaryDepartment { get; set; }
        public virtual DbSet<Sal_SalaryDepartmentItem> Sal_SalaryDepartmentItem { get; set; }
        public virtual DbSet<Sal_SalaryFund> Sal_SalaryFund { get; set; }
        public virtual DbSet<Sal_SalaryFundItem> Sal_SalaryFundItem { get; set; }
        public virtual DbSet<Sal_SalaryInformation> Sal_SalaryInformation { get; set; }
        public virtual DbSet<Sal_SalaryProductLine> Sal_SalaryProductLine { get; set; }
        public virtual DbSet<Sal_SalaryProductLineEmployee> Sal_SalaryProductLineEmployee { get; set; }
        public virtual DbSet<Sal_SchemesProduction> Sal_SchemesProduction { get; set; }
        public virtual DbSet<Sal_SchemesProfile> Sal_SchemesProfile { get; set; }
        public virtual DbSet<Sal_SchemesStructure> Sal_SchemesStructure { get; set; }
        public virtual DbSet<Sal_UnusualAllowance> Sal_UnusualAllowance { get; set; }
        public virtual DbSet<Sal_UnusualPay> Sal_UnusualPay { get; set; }
        public virtual DbSet<Sal_UnusualPayItem> Sal_UnusualPayItem { get; set; }
        public virtual DbSet<Sal_WorkingResult> Sal_WorkingResult { get; set; }
        public virtual DbSet<Sys_AllowedIP> Sys_AllowedIP { get; set; }
        public virtual DbSet<Sys_AppConfig> Sys_AppConfig { get; set; }
        public virtual DbSet<Sys_AsynTask> Sys_AsynTask { get; set; }
        public virtual DbSet<Sys_AttachFiles> Sys_AttachFiles { get; set; }
        public virtual DbSet<Sys_AuditLog> Sys_AuditLog { get; set; }
        public virtual DbSet<Sys_ChangedObject> Sys_ChangedObject { get; set; }
        public virtual DbSet<Sys_CodeObject> Sys_CodeObject { get; set; }
        public virtual DbSet<Sys_CodeObjectByUserNDate> Sys_CodeObjectByUserNDate { get; set; }
        public virtual DbSet<Sys_ColumnMode> Sys_ColumnMode { get; set; }
        public virtual DbSet<Sys_ConfigProcessApprove> Sys_ConfigProcessApprove { get; set; }
        public virtual DbSet<Sys_DashBoard> Sys_DashBoard { get; set; }
        public virtual DbSet<Sys_DashBoardUser> Sys_DashBoardUser { get; set; }
        public virtual DbSet<Sys_EmailLog> Sys_EmailLog { get; set; }
        public virtual DbSet<Sys_EnumFilter> Sys_EnumFilter { get; set; }
        public virtual DbSet<Sys_Event> Sys_Event { get; set; }
        public virtual DbSet<Sys_Group> Sys_Group { get; set; }
        public virtual DbSet<Sys_GroupLabel> Sys_GroupLabel { get; set; }
        public virtual DbSet<Sys_GroupPermission> Sys_GroupPermission { get; set; }
        public virtual DbSet<Sys_LayOut> Sys_LayOut { get; set; }
        public virtual DbSet<Sys_LockObject> Sys_LockObject { get; set; }
        public virtual DbSet<Sys_LockObjectItem> Sys_LockObjectItem { get; set; }
        public virtual DbSet<Sys_RankByRegion> Sys_RankByRegion { get; set; }
        public virtual DbSet<Sys_Resource> Sys_Resource { get; set; }
        public virtual DbSet<Sys_Server> Sys_Server { get; set; }
        public virtual DbSet<Sys_UserApprove> Sys_UserApprove { get; set; }
        public virtual DbSet<Sys_UserDataGroup> Sys_UserDataGroup { get; set; }
        public virtual DbSet<Sys_UserGroup> Sys_UserGroup { get; set; }
        public virtual DbSet<Sys_UserInfo> Sys_UserInfo { get; set; }
        public virtual DbSet<Sys_UsingUser> Sys_UsingUser { get; set; }
        public virtual DbSet<Temp_CatchData> Temp_CatchData { get; set; }
        public virtual DbSet<Temp_CatchDataDetail> Temp_CatchDataDetail { get; set; }
        public virtual DbSet<Temp_ManagerDownload> Temp_ManagerDownload { get; set; }
        public virtual DbSet<Tra_Certificate> Tra_Certificate { get; set; }
        public virtual DbSet<Tra_Class> Tra_Class { get; set; }
        public virtual DbSet<Tra_Course> Tra_Course { get; set; }
        public virtual DbSet<Tra_CourseTopic> Tra_CourseTopic { get; set; }
        public virtual DbSet<Tra_Plan> Tra_Plan { get; set; }
        public virtual DbSet<Tra_Ranking> Tra_Ranking { get; set; }
        public virtual DbSet<Tra_RankingGroup> Tra_RankingGroup { get; set; }
        public virtual DbSet<Tra_Reason> Tra_Reason { get; set; }
        public virtual DbSet<Tra_RequirementTrain> Tra_RequirementTrain { get; set; }
        public virtual DbSet<Tra_RequirementTrainDetail> Tra_RequirementTrainDetail { get; set; }
        public virtual DbSet<Tra_RequirementTrainingApprove> Tra_RequirementTrainingApprove { get; set; }
        public virtual DbSet<Tra_ScoreTopic> Tra_ScoreTopic { get; set; }
        public virtual DbSet<Tra_ScoreType> Tra_ScoreType { get; set; }
        public virtual DbSet<Tra_Trainee> Tra_Trainee { get; set; }
        public virtual DbSet<Tra_TraineeCertificate> Tra_TraineeCertificate { get; set; }
        public virtual DbSet<Tra_TraineeScore> Tra_TraineeScore { get; set; }
        public virtual DbSet<Tra_TraineeTopic> Tra_TraineeTopic { get; set; }
        public virtual DbSet<Tra_TrainerMailReminder> Tra_TrainerMailReminder { get; set; }
        public virtual DbSet<TT_TEMPATTENDANCETABLE> TT_TEMPATTENDANCETABLE { get; set; }
        public virtual DbSet<TT_TEMPPAYROLL> TT_TEMPPAYROLL { get; set; }
        public virtual DbSet<SalaryChangeDepartment> SalaryChangeDepartments { get; set; }
    }
}
