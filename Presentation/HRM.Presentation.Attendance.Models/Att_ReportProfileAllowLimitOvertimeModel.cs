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
    public class Att_ReportProfileAllowLimitOvertimeModel : BaseViewModel
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
 
        [DisplayName(ConstantDisplay.HRM_Attendance_BranchCode)]
        public string BranchCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_BranchName1)]
        public string BranchName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_GroupCode)]
        public string GroupCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_Group_GroupName)]
        public string GroupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        public string DepartmentCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentName1)]
        public string DepartmentName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        public string TeamCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TeamName1)]
        public string TeamName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        public string SectionCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_SectionName1)]
        public string SectionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_DivisionCode)]
        public string DivisionCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Division)]
        public string DivisionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Position)]
        public string Position { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public string JobTitle { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_UserExport)]
        public string UserExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateExport)]
        public string DateExport { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public string OrgStructureID { get; set; }
        public Guid ExportId { get; set; }

        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public partial class FieldNames
        {
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string PositionName = "PositionName";
            public const string BranchCode = "BranchCode";
            public const string BranchName = "BranchName";
            public const string GroupCode = "GroupCode";
            public const string GroupName = "GroupName";
            public const string DepartmentCode = "DepartmentCode";
            public const string DepartmentName = "DepartmentName";
            public const string TeamCode = "TeamCode";
            public const string TeamName = "TeamName";
            public const string SectionCode = "SectionCode";
            public const string SectionName = "SectionName";
            public const string DivisionCode = "DivisionCode";
            public const string DivisionName = "DivisionName";
            public const string Position = "Position";
            public const string JobTitle = "JobTitle";
            public const string UserExport = "UserExport";
            public const string DateExport = "DateExport";
        }
    }
}
