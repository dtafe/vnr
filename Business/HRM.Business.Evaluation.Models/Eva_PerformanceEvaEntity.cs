using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;

namespace HRM.Business.Evaluation.Models
{

    public class Eva_PerformanceEvaSendMailEntity
    {
        public Guid ID { get; set; }
        public Guid? PerformanceID { get; set; }
        public int? OrderEva { get; set; }
        public Guid? EvaluatorID { get; set; }
        public string Status { get; set; }
        public Guid? ProfileIDofEvaluator { get; set; }
    }

    public class Eva_PerformanceEvaEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public Guid? PerformanceID { get; set; }
        public int? OrderEva { get; set; }
        public DateTime? DateEva { get; set; }
        public Guid? EvaluatorID { get; set; }
        public string EvaluatorName { get; set; }
        public string TemplateName { get; set; }
        public string PerformancePlanName { get; set; }
        public string PerformanceTime { get; set; }
        public double? TotalMark { get; set; }
        public string Strengths { get; set; }
        public string Weaknesses { get; set; }
        public string ResultNote { get; set; }
        public Guid? LevelID { get; set; }
        public string LevelName { get; set; }
        public double? Proportion { get; set; }
        public string AttachFile { get; set; }
        public string Status { get; set; }
        public string ProfileName { get; set; }
        public string JobTitleNameOfProfile { get; set; }
        public DateTime? DatePerformOfProfile { get; set; }
        public string JobTitleNameOfEvaluator { get; set; }
        public DateTime? DatePerformOfEvaluator { get; set; }
        public string OrgStructureNameOfProfile { get; set; }
        public string OrgStructureNameOfEvaluator{ get; set; }
        public int? OrderNo { get; set; }
        public Guid? KPIID { get; set; }
        public string AttachFileLast { get; set; }
        public List<Eva_PerformanceEvaDetailEntity> PerformanceEvaDetails { get; set; }
        public double? TotalMarkFrom { get; set; }
        public double? TotalMarkTo { get; set; }
        public Guid? PerformanceTypeID { get; set; }
        public string PerformanceTypeName { get; set; }
        public string PositionName { get; set; }
        public string JobTitleName { get; set; }
        public string OrgStructureName { get; set; }
        public string CodeEmp { get; set; }
        public Guid? ProfileID { get; set; }
        public string FormulaOfPerformanceTemplate { get; set; }
        public bool? IsEvaluation { get; set; }
        public bool? IsSuperiorHasPerformance { get; set; }
        public bool? IsEvaluator { get; set; }

        public string Level1Name { get; set; }
        public string Level2Name { get; set; }
        public Nullable<System.Guid> RankDetailID { get; set; }
        public Nullable<System.Guid> RankID { get; set; }
        public Nullable<System.DateTime> YearEvaluation { get; set; }
        public Nullable<System.Guid> Level1ID { get; set; }
        public Nullable<double> Allowance1 { get; set; }
        public Nullable<double> Allowance2 { get; set; }
        public Nullable<double> Allowance3 { get; set; }
        public Nullable<double> Allowance4 { get; set; }
        public Nullable<double> Allowance5 { get; set; }
        public Nullable<double> Allowance6 { get; set; }
        public Nullable<double> Allowance7 { get; set; }
        public Nullable<System.DateTime> DateEffect { get; set; }
        public Nullable<System.DateTime> MonthReward { get; set; }
        public string GrossAmount { get; set; }
        public double? BasicGrossAmount { get; set; }

        public string SalaryClassName { get; set; }
        public string SalaryRankName { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_SECTION { get; set; }
        public string E_TEAM { get; set; }

    }
  
}
