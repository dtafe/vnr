using HRM.Presentation.Service;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Attendance.Models
{
    public class Att_ReportOvertimeOverLimitModel : BaseViewModel
    {

        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        [MaxLength(150)]
        public string DepartmentName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        [MaxLength(50)]
        public string Position { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OTHours)]
        public double? OTHours { get; set; }

        public partial class FieldNames
        {
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string DepartmentName = "DepartmentName";
            public const string Position = "Position";
            public const string OTHours = "OTHours";
        }
    }
}
