using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{

    public class Hre_ReportSummaryDependantSearchModel
    {
        public DateTime? MonthOfExpiry { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_DependantName)]
        [MaxLength(150)]
        public string DependantName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_RelationID)]
        public Guid? RelationID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Dependant_CompleteDateNew)]
        public Nullable<DateTime> DateFrom { get; set; }
        public Nullable<DateTime> DateTo { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }

        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Hre_ReportSummaryDependantModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_RelationID)]
        public Guid? RelationID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_ProfileID)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Dependant_DependantName)]
        [MaxLength(150)]
        public string DependantName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Dependant_MonthOfEffect)]
        public DateTime? MonthOfEffect { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Dependant_MonthOfExpiry)]
        public DateTime? MonthOfExpiry { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Dependant_CodeTax)]
        [MaxLength(50)]
        public string CodeTax { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Dependant_CompleteDate)]
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public Nullable<DateTime> DateFrom { get; set; }
  
        public int TotalDependant { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }

        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string CompleteDate = "CompleteDate";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string DependantName = "DependantName";
            public const string MonthOfEffect = "MonthOfEffect";
            public const string MonthOfExpiry = "MonthOfExpiry";
            public const string CodeTax = "CodeTax";
            public const string CodeEmp = "CodeEmp";
            public const string TotalDependant = "TotalDependant";
        }
    }
}
