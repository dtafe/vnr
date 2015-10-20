using System;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace HRM.Presentation.Evaluation.Models
{
    public class Eva_PerformanceEvaModel : BaseViewModel
    {
        public int TotalRow { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_TemplateName)]
        public Guid? PerformanceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_OrderEva)]
        public int? OrderEva { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_DateEva)]
        public DateTime? DateEva { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_EvaluatorName)]
        public Guid? EvaluatorID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_PerformanceEvaWaitingEva_MarkResult)]
        public double? TotalMark { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Strengths)]
        public string Strengths { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Weaknesses)]
        public string Weaknesses { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_ResultNote)]
        public string ResultNote { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_LevelID)]
        public Guid? LevelID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_LevelID)]
        public string LevelName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Proportion)]
        public double? Proportion { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_AttachFile)]
        public string AttachFile { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_EvaluatorName)]
        public string EvaluatorName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_TemplateName)]
        public string TemplateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformancePlan_PerformancePlanName)]
        public string PerformancePlanName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_PerformanceTime)]
        public string PerformanceTime { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public string JobTitleNameOfProfile { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_DateCreate)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DatePerformOfProfile { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public string JobTitleNameOfEvaluator { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_DateCreate)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DatePerformOfEvaluator { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureNameOfProfile { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureNameOfEvaluator { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_OrderNo)]
        public int? OrderNo { get; set; }
        public Guid? KPIID { get; set; }
        public string AttachFileLast { get; set; }
        public List<Eva_PerformanceEvaDetailModel> PerformanceEvaDetails { get; set; }
        public double? TotalMarkFrom { get; set; }
        public double? TotalMarkTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Tag)]
        public Guid? PerformanceTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Tag)]
        public string PerformanceTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        public List<string> AttachFiles { get; set; }
        public Guid? ProfileID { get; set; }
        public string FormulaOfPerformanceTemplate { get; set; }
        public bool? IsEvaluation { get; set; }
        /// <summary> Kiểm tra cấp sau đã đánh giá chưa </summary>
        public bool? IsSuperiorHasPerformance { get; set; }
        public bool? IsEvaluator { get; set; }
        public DateTime? DateStartMark { get; set; }
        public DateTime? DateEndMark { get; set; }


        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_RankDetailID)]
        public Nullable<System.Guid> RankDetailID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_RankDetailID)]
        public string RankDetailName { get; set; }

        public Nullable<System.Guid> RankID { get; set; }
        public Nullable<System.DateTime> YearEvaluation { get; set; }
        public string Level1Name { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_Level1ID)]
        public Nullable<System.Guid> Level1ID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_Level1ID)]
        public string Level2Name { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_Allowance1)]
        public Nullable<double> Allowance1 { get; set; }


        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_Allowance2)]
        public Nullable<double> Allowance2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_Allowance3)]
        public Nullable<double> Allowance3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_Allowance4)]
        public Nullable<double> Allowance4 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_Allowance5)]
        public Nullable<double> Allowance5 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_Allowance6)]
        public Nullable<double> Allowance6 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_Allowance7)]
        public Nullable<double> Allowance7 { get; set; }
        public Nullable<System.DateTime> DateEffect { get; set; }
        public Nullable<System.DateTime> MonthReward { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_GrossAmount)]
        public string GrossAmount { get; set; }

        public double? BasicGrossAmount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_SalaryClassName)]
        public string SalaryClassName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_SalaryRankName)]
        public string SalaryRankName { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }

        public partial class FieldNames
        {
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_SECTION = "E_SECTION";
            public const string E_TEAM = "E_TEAM";

            public const string PerformanceID = "PerformanceID";
            public const string OrderEva = "OrderEva";
            public const string DateEva = "DateEva";
            public const string EvaluatorID = "EvaluatorID";
            public const string TotalMark = "TotalMark";
            public const string Strengths = "Strengths";
            public const string Weaknesses = "Weaknesses";
            public const string ResultNote = "ResultNote";
            public const string LevelName = "LevelName";
            public const string Proportion = "Proportion";
            public const string AttachFile = "AttachFile";
            public const string Status = "Status";
            public const string EvaluatorName = "EvaluatorName";
            public const string TemplateName = "TemplateName";
            public const string PerformancePlanName = "PerformancePlanName";
            public const string PerformanceTime = "PerformanceTime";
            public const string ProfileName = "ProfileName";
            public const string PerformanceTypeName = "PerformanceTypeName";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";
            public const string OrgStructureName = "OrgStructureName";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileID = "ProfileID";

            public const string RankDetailName = "RankDetailName";
            public const string Level1Name = "Level1Name";
            public const string Level2Name = "Level2Name";
            public const string Allowance1 = "Allowance1";
            public const string Allowance2 = "Allowance2";
            public const string Allowance3 = "Allowance3";
            public const string Allowance4 = "Allowance4";
            public const string Allowance5 = "Allowance5";
            public const string Allowance6 = "Allowance6";
            public const string Allowance7 = "Allowance7";
            public const string GrossAmount = "GrossAmount";
            public const string BasicGrossAmount = "BasicGrossAmount";
            public const string SalaryClassName = "SalaryClassName";
            public const string SalaryRankName = "SalaryRankName";
        }
    }

    public class Eva_PerformanceEvaSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Evaluation_TemplateName)]
        public string PerformanceID { get; set; }

    }
    public class Eva_PerformanceEvaProfileSearchModel
    {

        //[DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_EvaluatorName)]
        //public Guid? EvaluatorID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Tag)]
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Tag)]
        public Guid? PerformanceTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformancePlan_PerformancePlanName)]
        public Guid? PerformancePlanID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_TotalMark)]
        public double? TotalMarkFrom { get; set; }
        public double? TotalMarkTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Level)]
        public System.Guid? LevelID { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Eva_PerformanceEvaWaitEvaSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_EvaluatorName)]
        public string EvaluatorID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Tag)]
        public Guid? PerformanceTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformancePlan_PerformancePlanName)]
        public Guid? PerformancePlanID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_TotalMark)]
        public double? TotalMarkFrom { get; set; }
        public double? TotalMarkTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Level)]
        public System.Guid? LevelID { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Eva_PerformanceEvaActiveSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_EvaluatorName)]
        public Guid? EvaluatorID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Tag)]
        public Guid? PerformanceTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformancePlan_PerformancePlanName)]
        public Guid? PerformancePlanID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_TotalMark)]
        public double? TotalMarkFrom { get; set; }      
        public double? TotalMarkTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_LevelID)]
        public System.Guid? LevelID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Status)]
        public string PerformaceEvaStatus { get; set; }   
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
