using System;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Evaluation.Models
{
    public class Eva_PerformanceTemplateModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Evaluation_TemplateName)]
        public string TemplateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PositionID)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceTypeID)]
        public Guid? PerformanceTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Comment)]
        public string Comment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_CategoryKPI)]
        public Guid? CategoryKPIID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_EvaKPI)]
        public Guid? EvaKPIID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public Guid? OrgID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Formula)]
        public string Formula { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Formula1)]
        public string Formula1 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceTypeID)]
        public string PerformanceTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        public string JobTitleName { get; set; }

        
        public partial class FieldNames
        {
            public const string TemplateName = "TemplateName";
            public const string PositionID = "PositionID";
            public const string PerformanceTypeID = "PerformanceTypeID";
            public const string Comment = "Comment";
            public const string Code = "Code";
            public const string Formula = "Formula";
            public const string PerformanceTypeName = "PerformanceTypeName";
            public const string OrgStructureName = "OrgStructureName";
            public const string OrgID = "OrgID";
            public const string EvaKPIID = "EvaKPIID";
            public const string CategoryKPIID = "CategoryKPIID";
            public const string JobTitleID = "JobTitleID";
            public const string JobTitleName = "JobTitleName";
        }
    }

    public class Eva_PerformanceTemplateSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Evaluation_TemplateName)]
        public string TemplateName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Evaluation_Code)]
        public string Code { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    
    }

    public class Eva_PerformanceTemplateMultiModel
    {      
        public Guid ID { get; set; }
        public string TemplateName { get; set; }
        public Guid? JobTitleID { get; set; }
    }
}
