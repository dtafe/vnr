using System;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Insurance.Models
{
    public class Ins_ReportEmpHardJobModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Insurance_ReportInsuranceTotal_Stt)]
        public int Stt { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public bool? IsExport { get; set; }
        public Guid ExportId { get; set; }

        public partial class FieldNames
        {
            public const string Stt = "Stt";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";

        }
    }
    public class Ins_ReportEmpHardJobSearchModel
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
