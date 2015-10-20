using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
  public class Kai_ReportPaymentoutModel : BaseViewModel
    {
         [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public Nullable<System.Guid> ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Kai_KaiZenData_KaizenName)]


        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public string Note { get; set; }
        public Double? AMOUNT { get; set; }


        public partial class FieldNames
        {
            public const string Note = "Note";
            public const string AMOUNT = "AMOUNT";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string ProfileID = "ProfileID";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }

    }
    public partial class Kai_ReportPaymentoutSearchModel
    {
        
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        public string OrgStructureID { get; set; }
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_MonthFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_MonthTo)]
        public DateTime? DateTo { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public Guid ExportID { get; set; }

        public ExportFileType ExportType { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
    }
}
