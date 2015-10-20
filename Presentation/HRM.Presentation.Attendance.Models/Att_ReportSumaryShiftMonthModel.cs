using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Attendance.Models
{
    public class Att_ReportSumaryShiftMonthModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ShiftId)]
        public string ShiftID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_isIncludeQuitEmp)]
        public bool isIncludeQuitEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_NoDisplay0Data)]
        public bool NoDisplay0Data { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ComputeWorkDay_ProfileID)]
        public string ProfileIDs { get; set; }


        public string CodeEmp { get; set; }

        public string ProfileName { get; set; }
        public string UserExport { get; set; }
        public Guid ExportId { get; set; }
    }
}
