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
    public class Att_AttendanceTableItemModel : BaseViewModel
    {
        //public double StdWorkDayCount { get; set; }
        //public double RealWorkDayCount { get; set; }
        //public double PaidWorkDayCount { get; set; }
        //public double AnlDayTaken { get; set; }
        //public double AnlDayAvailable { get; set; }
        //public double LateEarlyDeductionHours { get; set; }
        //public string Note { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTableItem_AttendanceTableID)]
        public System.Guid AttendanceTableID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTableItem_WorkDate)]
        public System.DateTime WorkDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTableItem_AvailableHours)]
        public double AvailableHours { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTableItem_ShiftID)]
        public Nullable<System.Guid> ShiftID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTableItem_WorkHours)]
        public double WorkHours { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_ShiftName)]
        public string ShiftName { get; set; }


        public Nullable<System.DateTime> FirstInTime { get; set; }
        public Nullable<System.DateTime> LastOutTime { get; set; }
        public int LateEarlyMinutes { get; set; }
        public double UnpaidLeaveHours { get; set; }
        public double PaidLeaveHours { get; set; }
        public double WorkPaidHours { get; set; }
        public double NightShiftHours { get; set; }
        public Nullable<System.Guid> OvertimeTypeID { get; set; }
        public double TotalOvertimeHours { get; set; }
        public double OvertimeHours { get; set; }
        public string OvertimeTypeName { get; set; }

        public string ValueFields { get; set; }

        public string HDTJobType { get; set; }

        public partial class FieldNames
        {
            public const string FirstInTime = "FirstInTime";
            public const string LastOutTime = "LastOutTime";
            public const string LateEarlyMinutes = "LateEarlyMinutes";
            public const string UnpaidLeaveHours = "UnpaidLeaveHours";
            public const string PaidLeaveHours = "PaidLeaveHours";
            public const string WorkPaidHours = "WorkPaidHours";
            public const string NightShiftHours = "NightShiftHours";
            public const string OvertimeTypeID = "OvertimeTypeID";
            public const string OvertimeTypeName = "OvertimeTypeName";
            public const string OvertimeHours = "OvertimeHours";
            public const string TotalOvertimeHours = "TotalOvertimeHours";

            public const string AttendanceTableID = "AttendanceTableID";
            public const string WorkDate = "WorkDate";
            public const string AvailableHours = "AvailableHours";
            public const string ShiftID = "ShiftID";
            public const string WorkHours = "WorkHours";
            public const string ProfileID = "ProfileID";
            public const string ShiftName = "ShiftName";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
          
        }
    }
}
