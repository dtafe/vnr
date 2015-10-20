using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Evaluation.Models
{
    public class Eva_KPIBuildingModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceTemplateOptionLabel)]
        public Guid? PerformanceTemplateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceTemplate)]
        public string PerformanceTemplateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformancePlan_PerformancePlanName)]
        public Guid? PerformancePlanID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformancePlan_PerformancePlanName)]
        public string PerformancePlanName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_PeriodFromDate)]
        public DateTime? PeriodFromDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_PeriodToDate)]
        public DateTime? PeriodToDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Tag)]
        public Guid? TagID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Tag)]
        public string TagName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_DueDate)]
        public DateTime? DueDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_TotalMark)]
        public Nullable<double> TotalMark { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Strengths)]
        public string Strengths { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Weaknesses)]
        public string Weaknesses { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_ResultNote)]
        public string ResultNote { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Level)]
        public Guid? LevelID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Level)]
        public string LevelName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Proportion)]
        public Nullable<double> Proportion { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_AttachFile)]
        public string AttachFile { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_CurrentStatus)]
        public string CurrentStatus { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_PerformanceTime)]
        public string PerformanceTime { get; set; }
        public string Formula { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Reward_UserApproveID)]
        public Guid EvaluatorID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceTypeID)]
        public Guid? PerformanceTypeID { get; set; }
        public string PerformanceTypeName { get; set; }
        public List<Guid> EvaluatorIDs { get; set; }
        public string EvaluatorIDList { get; set; }
        public string AttachFileLast { get; set; }
        public string PerformanceEvaStatus { get; set; }
        public List<string> AttachFiles { get; set; }

        public List<Eva_KPIModel> KPIs { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }



        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string PositionName = "PositionName";
            public const string TagName = "TagName";
            public const string JobTitleName = "JobTitleName";
            public const string PerformanceTemplateName = "PerformanceTemplateName";
            public const string PerformancePlanName = "PerformancePlanName";
            public const string LevelName = "LevelName";
            public const string PerformanceTime = "PerformanceTime";
            public const string Status = "Status";
            public const string TotalMark = "TotalMark";
            public const string PerformanceTypeName = "PerformanceTypeName";
            public const string PerformanceEvaStatus = "PerformanceEvaStatus";
            public const string ProfileID = "ProfileID";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }

    public class Eva_KPIBuildingSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public Guid? JobTitleID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Evaluation_Tag)]
        public Guid? PerformanceTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformancePlan_PerformancePlanName)]
        public Guid? PerformancePlanID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_TotalMark)]
        public double? TotalMarkFrom { get; set; }
        public double? TotalMarkTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Level)]
        public System.Guid? LevelID { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
   
  
}
