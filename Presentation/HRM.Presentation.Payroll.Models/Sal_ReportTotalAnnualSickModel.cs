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
    public class Sal_ReportTotalPITMonthlyModel 
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportBasicSalaryMonthly_ProfileName)]
        public string ProfileIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Payroll_CutOffDurationID)]
        public Nullable<System.Guid> CutOffDurationID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Element_ElementName)]
        public string ElementType { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }

        public ExportFileType ExportType { get; set; }


        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public string ValueFields { get; set; }
    }

    

}
