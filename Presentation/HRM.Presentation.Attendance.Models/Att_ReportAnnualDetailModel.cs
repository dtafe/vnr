using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;

namespace HRM.Presentation.Attendance.Models
{
    public class Att_ReportAnnualDetailModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportBasicSalaryMonthly_ProfileName)]
        public string ProfileIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Common_Search_Duration)]
        public Nullable<DateTime> DateFrom { get; set; }
        public Nullable<DateTime> DateTo { get; set; }


        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public Guid? WorkingPlaceID { get; set; }

        public int? Year { get; set; }

        public string ReportName { get; set; }
        public ExportFileType ExportType { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_StatusSyn)]
        public string StatusSyn { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_WorkingPlace)]
        public string WorkPlace { get; set; }

        public string WorkingPlaceName { get; set; }

        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public string ValueFields { get; set; }
    }

}
