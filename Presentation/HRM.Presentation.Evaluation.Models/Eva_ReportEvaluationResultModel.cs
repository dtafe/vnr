using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Evaluation.Models
{
    public class Eva_ReportEvaluationResultModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CostCentreName)]
        public string CostCentreName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_ParentID)]
        public string Department { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_OrgStructureName)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_WorkHistory_SupervisiorID)]
        public string SupervisorID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public string JobTitle { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_DateHire)]
        public DateTime? DateHire { get; set; }

        [DisplayName(ConstantDisplay.HRM_Eva_ReportEvaluationResult_YearOfService)]
        public int YearOfService { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_ReportEvaluationResult_MarkLevel1)]
        public double? MarkLevel1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_ReportEvaluationResult_MarkLevel2)]
        public double? MarkLevel2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_ReportEvaluationResult_EvaluationLevel2)]
        public string EvaluationLevel2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_ReportEvaluationResult_Level)]
        public string Level { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_ReportEvaluationResult_MarkLevel3)]
        public double? MarkLevel3 { get; set; }
        public Guid ExportID { get; set; }
        public string ValueFields { get; set; }

        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }




        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string CostCentreName = "CostCentreName";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string Department = "Department";
            public const string OrgStructureName = "OrgStructureName";
            public const string SupervisorID = "SupervisorID";
            public const string JobTitle = "JobTitle";
            public const string DateHire = "DateHire";
            public const string YearOfService = "YearOfService";
            public const string MarkLevel1 = "MarkLevel1";
            public const string MarkLevel2 = "MarkLevel2";
            public const string EvaluationLevel2 = "EvaluationLevel2";
            public const string Level = "Level";
            public const string MarkLevel3 = "MarkLevel3";
            public const string PositionName = "PositionName";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }

    public class Eva_ReportEvaluationResultSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_PeriodFromDate)]
        public DateTime? PeriodFromDate { get; set; }
        public DateTime? PeriodToDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_TemplateName)]
        public Guid PerformanceTemplateID { get; set; }

        public DateTime Month { get; set; }
        public Guid ExportID { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public ExportFileType ExportType { get; set; }

        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
    }
}
