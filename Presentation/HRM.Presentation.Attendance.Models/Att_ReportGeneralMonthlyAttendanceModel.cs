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
    public class Att_ReportGeneralMonthlyAttendanceModel : BaseViewModel
    {
        //[DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
         [DisplayName(ConstantDisplay.HRM_HR_Dependant_MonthOfEffect)]
        
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        [MaxLength(50)]
        public string GroupCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        [MaxLength(50)]
        public string SectionCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        [MaxLength(150)]
        public string DepartmentName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        [MaxLength(150)]
        public string CodeOrg { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_BranchCode)]
        [MaxLength(50)]
        public string Division { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_PaidLeaveDays)]
        public Double? PaidLeaveDays { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_SickLeave)]
        public Double? SickLeave { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DcrSalaryDay)]
        public Double? DcrSalaryDay { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_LunchAllowance)]
        public Double? LunchAllowance { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceAllowance)]
        public Double? AttendanceAllowance { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_OvertimeHours)]
        public Double? OvertimeHours { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Remark)]
        public string Remark { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public string OrgStructureID { get; set; }

        public Guid? ShiftID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ShiftId)]
        public string ShiftIDs { get; set; }

        public Guid? PayrollID{ get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_PayrollID)]
        public string PayrollIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_GroupByTypes)]
        public string GroupByTypes { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_HourMonthlyWorkings)]
        public string HourMonthlyWorkings { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Status)]
        public string StatusEmployees { get; set; }
      
        [DisplayName(ConstantDisplay.HRM_Attendance_isIncludeQuitEmp)]
        public bool isIncludeQuitEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ExcludeIfWorkingDayIsZero)]
        public bool ExcludeIfWorkingDayIsZero { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_IsShowOTHourRow)]
        public bool IsShowOTHourRow { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_NoDisplay0Data)]
        public bool NoDisplay0Data { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateExport)]
        public DateTime DateExport { get; set; }

        public Guid ExportId { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string GroupCode = "GroupCode";
            public const string SectionCode = "SectionCode";
            public const string CodeOrg = "CodeOrg";
            public const string Division = "Division";
            public const string PaidLeaveDays = "PaidLeaveDays";
            public const string SickLeave = "SickLeave";
            public const string DcrSalaryDay = "DcrSalaryDay";
            public const string LunchAllowance = "LunchAllowance";
            public const string AttendanceAllowance = "AttendanceAllowance";
            public const string OvertimeHours = "OvertimeHours";
            public const string Remark = "Remark";
            public const string OrgStructureID = "OrgStructureID";
            public const string ShiftIDs = "ShiftIDs";
            public const string GroupByTypes = "GroupByTypes";
            public const string HourMonthlyWorkings = "HourMonthlyWorkings";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";
        }
    }
}
