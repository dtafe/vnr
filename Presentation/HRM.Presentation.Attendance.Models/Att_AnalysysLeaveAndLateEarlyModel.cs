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
    public class Att_AnalysysLeaveAndLateEarlyModel : BaseViewModel
    {
        //public bool? IsSave { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
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
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDate)]
        public DateTime? WorkDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ShiftId)]
        [MaxLength(50)]
        public string ShiftName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Early)]
        public double? Early { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_RoundEarly)]
        public double? RoundEarly { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Late)]
        public double? Late { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_RoundLate)]
        public double? RoundLate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_LateEarly)]
        public double? LateEarly { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_InTime)]
        public DateTime? InTime { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OutTime)]
        public DateTime? OutTime { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_LeaveDayType_Code)]
        public string udLeavedayCode1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_Status)]
        public string udLeavedayStatus1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public string OrgStructureIds { get; set; }

      
        public partial class FieldNames
        {
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string WorkDate = "WorkDate";
            public const string ShiftName = "ShiftName";
            public const string Early = "Early";
            public const string RoundEarly = "RoundEarly";
            public const string Late = "Late";
            public const string RoundLate = "RoundLate";
            public const string LateEarly = "LateEarly";

            public const string InTime = "InTime";
            public const string OutTime = "OutTime";
            public const string udLeavedayCode1 = "udLeavedayCode1";
            public const string udLeavedayStatus1 = "udLeavedayStatus1";

        }
    }
}
