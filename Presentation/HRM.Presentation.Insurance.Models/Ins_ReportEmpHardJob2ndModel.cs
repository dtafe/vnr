using System;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Insurance.Models
{
    public class Ins_ReportEmpHardJob2ndModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Insurance_ReportInsuranceTotal_Stt)]
        public int Stt { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_AnalyzeInsurance_JobName)]
        public string JobName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_HDTJobType_Type)]
        public string HDTJobType { get; set; }
        [DisplayName(ConstantDisplay.HRM_INS_Ins_ReportEmpHardJob2nd_DaysNonHDTJob)]
        public int DaysNonHDTJob { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_AmountSubtractHDTJob)]
        public double? AmountHDTIns { get; set; }
        public DateTime? MonthYear { get; set; }
        public bool? IsExport { get; set; }
        public Guid ExportId { get; set; }
        public string OrgStructureID { get; set; }

        public partial class FieldNames
        {
            public const string Stt = "Stt";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string JobName = "JobName";
            public const string HDTJobType = "HDTJobType";
            public const string AmountHDTIns = "AmountHDTIns";
            public const string DaysNonHDTJob = "DaysNonHDTJob";
        }
    }
    public class Ins_ReportEmpHardJob2ndSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Ins_ReportEmpHardJob_MonthInsurance)]
        public Nullable<DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Hr_INS_DateTo)]
        public Nullable<DateTime> DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

}
