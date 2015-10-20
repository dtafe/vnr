using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_ReportSummaryDependantDeductionModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_ProfileID)]
        public Guid ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public string CodeTax { get; set; }
        public string DependantName { get; set; }
        public Nullable<System.DateTime> CompleteDate { get; set; }
        public DateTime? MonthOfEffect { get; set; }
        public DateTime? MonthOfExpiry { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_MonthOfExpiry)]
        public DateTime? MonthFrom { get; set; }
        public DateTime? MonthTo { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string CodeTax = "CodeTax";
            public const string DependantName = "DependantName";
            public const string CompleteDate = "CompleteDate";
            public const string MonthOfEffect = "MonthOfEffect";
            public const string MonthOfExpiry = "MonthOfExpiry";
        }
    }
}
