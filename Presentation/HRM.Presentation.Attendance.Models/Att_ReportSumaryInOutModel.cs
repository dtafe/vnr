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
    public class Att_ReportSumaryInOutModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        public string GroupCode { get; set; }

        public string GroupName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        public string SectionCode { get; set; }

        public string SectionName { get; set; }

        public string TeamCode { get; set; }

        public string TeamName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        public string DepartmentName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        public string DepartmentCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        public string Division { get; set; }

        public string DivisionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Date)]
        public DateTime? Date { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ShiftId)]
        public string ShiftName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_InTime)]
        public DateTime? udTimeIn { get; set; }
        public DateTime? InTime { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_OutTime)]
        public DateTime? udTimeOut { get; set; }
        public DateTime? OutTime { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Remark)]
        public string Remark { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public string OrgStructureID { get; set; }
        public Guid? ShiftID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ShiftId)]
        public string ShiftIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_isIncludeQuitEmp)]
        public bool isIncludeQuitEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateExport)]
        public DateTime DateExport { get; set; }

        public Guid ExportId { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ReportWrongShift_ApprovedShift)]
        public string ApprovedShift { get; set; }

        public partial class FieldNames
        {
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string GroupCode = "GroupCode";
            public const string GroupName = "GroupName";
            public const string SectionCode = "SectionCode";
            public const string SectionName = "SectionName";
            public const string DepartmentName = "DepartmentName";
            public const string DepartmentCode = "DepartmentCode";
            public const string Division = "Division";
            public const string DivisionName = "DivisionName";
            public const string Date = "Date";
            public const string ShiftName = "ShiftName";
            public const string InTime = "InTime";
            public const string OutTime = "OutTime";
            public const string Remark = "Remark";
            public const string OrgStructureID = "OrgStructureID";
            public const string udTimeIn = "udTimeIn";
            public const string udTimeOut = "udTimeOut";
            public const string TeamCode = "TeamCode";
            public const string TeamName = "TeamName";
            public const string UserExport = "UserExport";
            public const string DateExport = "DateExport";
            public const string ApprovedShift = "ApprovedShift";
        }
    }


}
