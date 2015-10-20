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
    public class Att_ReportMonthlyTimeSheetModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        [MaxLength(150)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        [MaxLength(150)]
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        public string ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AttendanceTable_CutOffDurationID)]
        public Guid? CutOffDurationID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_GradeAttendance_GradeAttendanceName)]
        public Guid? GradeAttendanceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Dependant_MonthOfEffect)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }
        public DateTime? DateHire { get; set; }
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
        [DisplayName(ConstantDisplay.HRM_Attendance_isIncludeQuitEmp)]
        public bool isIncludeQuitEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_NoDisplay0Data)]
        public bool NoDisplay0Data { get; set; }
        public double StdWorkDayCount { get; set; }
        public double RealWorkDayCount { get; set; }
        public double PaidWorkDayCount { get; set; }
        public double AnlDayAvailable { get; set; }
        public double NightShiftHours { get; set; }
        public double LateEarlyDeductionHours { get; set; }
        public double AnlDayTaken { get; set; }
        public string Note { get; set; }
        public string WorkPlaceIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_WorkPlace)]
        public Guid WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateExport)]
        public DateTime DateExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_WorkHourType)]
        public string WorkHourType { get; set; }
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
            public const string StdWorkDayCount = "StdWorkDayCount";
            public const string RealWorkDayCount = "RealWorkDayCount";
            public const string PaidWorkDayCount = "PaidWorkDayCount";
            public const string AnlDayAvailable = "AnlDayAvailable";
            public const string NightShiftHours = "NightShiftHours";
            public const string LateEarlyDeductionHours = "LateEarlyDeductionHours";
            public const string AnlDayTaken = "AnlDayTaken";
            public const string Note = "Note";
            public const string UserExport = "UserExport";
            public const string DateExport = "DateExport";
            public const string DepartmentName = "DepartmentName";
            public const string DateHire = "DateHire";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";

        }
    }
}
