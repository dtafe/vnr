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
    public class Att_ReportExceptionDataAdvModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        [MaxLength(150)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        [MaxLength(50)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        [MaxLength(50)]
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_InDate)]
        public DateTime? InDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_WorkDate)]
        public DateTime? WorkDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_InTime)]
        public DateTime? InTime { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OutDate)]
        public DateTime? OutDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OutTime)]
        public DateTime? OutTime { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ShiftId)]
        [MaxLength(150)]
        public string ShiftName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ShiftInTime)]
        public DateTime? ShiftInTime { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ShiftOutTime)]
        public DateTime? ShiftOutTime { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_PayrollGroup)]
        public string PayrollGroup { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_StatusEmployee)]
        public string StatusEmployee { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportExceptionDataAdv_ExcludeManagerStaff)]
        public bool ExcludeManagerStaff { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportExceptionDataAdv_ShowListLeaveDay)]
        public bool ShowListLeaveDay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportExceptionDataAdv_NoShift)]
        public bool NoShift { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportExceptionDataAdv_LessHours)]
        public bool LessHours { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportExceptionDataAdv_DifferenceMoreRoster)]
        public bool DifferenceMoreRoster { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportExceptionDataAdv_OTDuplicate)]
        public bool OTDuplicate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportExceptionDataAdv_MissInOut)]
        public bool MissInOut { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportExceptionDataAdv_NoScan)]
        public bool NoScan { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateExport)]
        public DateTime DateExport { get; set; }
        public double? More { get; set; }
        public double? Less { get; set; }
        public Guid ExportId { get; set; }
        public string ValueFields { get; set; }
        public string InHour { get; set; }
        public string OutHour { get; set; }
        public partial class FieldNames
        {
            public const string WorkDate = "WorkDate";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";
            public const string InDate = "InDate";
            public const string InTime = "InTime";
            public const string OutTime = "OutTime";
            public const string OutDate = "OutDate";
            public const string ShiftName = "ShiftName";
            public const string ShiftInTime = "ShiftInTime";
            public const string ShiftOutTime = "ShiftOutTime";
            public const string InHour = "InHour";
            public const string OutHour = "OutHour";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";
        }
    }
}
