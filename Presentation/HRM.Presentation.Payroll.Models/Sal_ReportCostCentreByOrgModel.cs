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
    public class Sal_ReportCostCentreByOrgModel : BaseViewModel
    {

        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string ElementName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_CutOffDurationID)]
        public string CutOffDurationIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ComparePayroll_ElementIDs)]
        public string ElementIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ComparePayroll_OrgStructureTypeID)]
        public Guid? OrgStructureTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_ComparePayroll_CompareType)]
        public string CompareType { get; set; }
        public Guid ExportId { get; set; }
        public string OrgStructureID { get; set; }

        public bool ShowChart { get; set; }
        public bool ShowChartByProfile { get; set; }
        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string ElementName = "ElementName";
            public const string CutOffDurationIDs = "CutOffDurationIDs";
            public const string CompareType = "CompareType";
            public const string OrgStructureID = "OrgStructureID";
            public const string ElementIDs = "ElementIDs";

        }
    }

    

}
