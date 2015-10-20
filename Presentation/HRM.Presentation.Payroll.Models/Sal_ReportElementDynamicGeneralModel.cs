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
    public class Sal_ReportElementDynamicGeneralModel 
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportBasicSalaryMonthly_ProfileName)]
        public string ProfileIDs { get; set; }  

        [DisplayName(ConstantDisplay.HRM_Payroll_CutOffDurationID)]
        public Nullable<System.Guid> CutOffDurationID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Element_ElementName)]
        public string ElementType { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public Guid? WorkingPlaceID { get; set; }

        public Guid UserID { get; set; }
        public string ReportName { get; set; }
        public ExportFileType ExportType { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_StatusSyn)]
        public string StatusSyn { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public string WorkPlace { get; set; }
               [DisplayName(ConstantDisplay.HRM_HR_Profile_Settlement)]
        public int? Settlement { get; set; }
               [DisplayName(ConstantDisplay.HRM_Attendance_AnnualLeave_Year)]
               public DateTime? Year { get; set; }
        public string WorkingPlaceName { get; set; }

        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
          [DisplayName(ConstantDisplay.HRM_Recruitment_UnusualAllowance_TypeUnusual)]
        public string UnsualAllowanceCfg { get; set; }
        public Guid ExportId { get; set; }
        public string ValueFields { get; set; }
    }   

}
