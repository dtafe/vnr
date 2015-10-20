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
    public class Att_AttendanceTableModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_MonthYear)]
        public DateTime? MonthYear { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_ProfileID)]
        public Guid ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_CutOffDurationID)]
        public Guid? CutOffDurationID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_Status)]
        [MaxLength(50)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_StdWorkDayCount)]
        public double StdWorkDayCount { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_RealWorkDayCount)]
        public double RealWorkDayCount { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_PaidWorkDayCount)]
        public double PaidWorkDayCount { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_PaidLeaveDays)]
        public Nullable<double> PaidLeaveDays { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_HourPerDay)]
        public double HourPerDay { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_NightShiftHours)]
        public double NightShiftHours { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_OTNightShiftHours)]
        public double? OTNightShiftHours { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_AnlDayTaken)]
        public double AnlDayTaken { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_AnlDayAvailable)]
        public double AnlDayAvailable { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_DateStart)]
        public DateTime? DateStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_DateEnd)]
        public DateTime? DateEnd { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_LateEarlyDeductionHours)]
        public double LateEarlyDeductionHours { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_UserRegister)]
        [MaxLength(150)]
        public string UserRegister { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_DateRegister)]
        public DateTime? DateRegister { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_UserApprove)]
        [MaxLength(150)]
        public string UserApprove { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_DateApprove)]
        public DateTime? DateApprove { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_Note)]
        [MaxLength(1000)]
        public string Note { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }


        [DisplayName(ConstantDisplay.HRM_Attendance_CutOffDuration_CutOffDurationName)]
        [MaxLength(50)]
        public string CutOffDurationName { get; set; }

        public string LeaveDay1Name { get; set; }
        public string LeaveDay2Name { get; set; }
        public string LeaveDay3Name { get; set; }
        public string LeaveDay4Name { get; set; }
        public string Overtime1Name { get; set; }
        public string Overtime2Name { get; set; }
        public string Overtime3Name { get; set; }
        public string Overtime4Name { get; set; }
        public string Overtime5Name { get; set; }
        public string Overtime6Name { get; set; }

        public Nullable<System.Guid> LeaveDay1Type { get; set; }
        public double LeaveDay1Hours { get; set; }
        public Nullable<System.Guid> LeaveDay2Type { get; set; }
        public double LeaveDay2Hours { get; set; }
        public Nullable<System.Guid> LeaveDay3Type { get; set; }
        public double LeaveDay3Hours { get; set; }
        public Nullable<System.Guid> LeaveDay4Type { get; set; }
        public double LeaveDay4Hours { get; set; }
        public Nullable<System.Guid> Overtime1Type { get; set; }
        public double Overtime1Hours { get; set; }
        public Nullable<System.Guid> Overtime2Type { get; set; }
        public double Overtime2Hours { get; set; }
        public Nullable<System.Guid> Overtime3Type { get; set; }
        public double Overtime3Hours { get; set; }
        public Nullable<System.Guid> Overtime4Type { get; set; }
        public double Overtime4Hours { get; set; }
        public Nullable<System.Guid> Overtime5Type { get; set; }
        public double Overtime5Hours { get; set; }
        public Nullable<System.Guid> Overtime6Type { get; set; }
        public double Overtime6Hours { get; set; }


        public string HDTJobType1 { get; set; }
        public string HDTJobType2 { get; set; }
        public string HDTJobType3 { get; set; }
        public Nullable<int> HDTJobDayCount1 { get; set; }
        public Nullable<int> HDTJobDayCount2 { get; set; }
        public Nullable<int> HDTJobDayCount3 { get; set; }

        public partial class FieldNames
        {
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string MonthYear = "MonthYear";
            public const string CutOffDurationID = "CutOffDurationID";
            public const string CutOffDurationName = "CutOffDurationName";
            public const string Status = "Status";
            public const string StdWorkDayCount = "StdWorkDayCount";
            public const string RealWorkDayCount = "RealWorkDayCount";
            public const string PaidWorkDayCount = "PaidWorkDayCount";
            public const string HourPerDay = "HourPerDay";
            public const string NightShiftHours = "NightShiftHours";
            public const string OTNightShiftHours = "OTNightShiftHours";
            public const string AnlDayTaken = "AnlDayTaken";
            public const string AnlDayAvailable = "AnlDayAvailable";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string LateEarlyDeductionHours = "LateEarlyDeductionHours";
            public const string UserRegister = "UserRegister";
            public const string DateRegister = "DateRegister";
            public const string UserApprove = "UserApprove";
            public const string DateApprove = "DateApprove";
            public const string Note = "Note";
            public const string CodeEmp = "CodeEmp";
         
        }
    }
    public class ProfileIdAndCutOffIdModel
    {
        public Guid ProfileId { get; set; }
        public Guid CutOffId { get; set; }
    }
}
