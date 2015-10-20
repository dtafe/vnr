using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class CatJobTitleModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_JobTitleName_StringLength)]
        [Required(ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_JobTitleName_Required)]       
        public string JobTitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleNameEn)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_JobTitleNameEn_StringLength)]
        public string JobTitleNameEn { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_PositionID)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_OrgStructureID)]
        public Guid? OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_CostCentreID)]
        public Guid? CostCentreID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_GradeCfgID)]
        public Guid? GradeCfgID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_SalaryJobGroupID)]
        public Guid? SalaryJobGroupID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleCode)]
        [StringLength(50, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_JobTitleCode_StringLength)]
        public string JobTitleCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobLevel)]
        public int? JobLevel { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobDescription)]
        public string JobDescription { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_Notes)]
        public string Notes { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_SalaryRate)]
        public double? SalaryRate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_IsSystem)]
        public bool? IsSystem { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_SalaryMax)]
        public double? SalaryMax { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_SalaryMin)]
        public double? SalaryMin { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_TaskShortTerm)]
        [StringLength(1000, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_TaskShortTerm_StringLength)]
        public string TaskShortTerm { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_TaskLongTerm)]
        [StringLength(1000, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_TaskLongTerm_StringLength)]
        public string TaskLongTerm { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_Permission)]
        [StringLength(1000, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_Permission_StringLength)]
        public string Permission { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_KPIID)]
        public Guid? KPIID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_ReqEducation)]
        [StringLength(1000, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_ReqEducation_StringLength)]
        public string ReqEducation { get; set; }


        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_ReqLanguage)]
        [StringLength(1000, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_ReqLanguage_StringLength)]
        public string ReqLanguage { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_ReqComputing)]
        [StringLength(1000, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_ReqComputing_StringLength)]        
        public string ReqComputing { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_ReqSkill)]
        [StringLength(1000, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_ReqSkill_StringLength)]    
        public string ReqSkill { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_ReqMain)]
        [StringLength(1000, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_ReqMain_StringLength)]    
        public string ReqMain { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_SkillPriority)]
        [StringLength(1000, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_SkillPriority_StringLength)]
        public string SkillPriority { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_FileAttachment)]
        [StringLength(1000, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_FileAttachment_StringLength)]
        public string FileAttachment { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_CodeCatJobTitle)]
        [StringLength(32, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_CodeCatJobTitle_StringLength)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_TargetAmountUnit)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_TargetAmountUnit_StringLength)]
        public string TargetAmountUnit { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_ReqDocuments)]
        [StringLength(1000, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_ReqDocuments_StringLength)]
        public string ReqDocuments { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_TargetAmount)]        
        public double? TargetAmount { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_TotalSalaryBudget)] 
        public double? TotalSalaryBudget { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_TotalOvertimeBudget)] 
        public double? TotalOvertimeBudget { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_OrderCatJobTitle)] 
        public int? Order { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_AnnualDays)] 
        public double? AnnualDays { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_UnusuallyAllowance1)] 
        public double? UnusuallyAllowance1 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_UnusuallyAllowance2)] 
        public double? UnusuallyAllowance2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_UnusuallyAllowance3)] 
        public double? UnusuallyAllowance3 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_IsWorkingHard)] 
        public bool? IsWorkingHard { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_PermissionOvertimeType)]
        [StringLength(1000, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_PermissionOvertimeType_StringLength)]
        public string PermissionOvertimeType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_IndustryCode)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_IndustryCode_StringLength)]
        public string IndustryCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobCode)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_JobCode_StringLength)]
        public string JobCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_Qualification)]
        [StringLength(3000, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_Qualification_StringLength)]
        public string Qualification { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_LaborType)]
        [StringLength(150, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_LaborType_StringLength)]
        public string LaborType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_FormType)]
        [StringLength(150, ErrorMessage = ConstantDisplay.HRM_Category_JobTitle_FormType_StringLength)]
        public string FormType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_EmployeeTypeID)]
        public Guid? EmployeeTypeID { get; set; }
    }
}
