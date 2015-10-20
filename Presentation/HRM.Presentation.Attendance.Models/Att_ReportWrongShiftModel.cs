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
    public class Att_ReportWrongShiftModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_BranchCode)]
        public string BranchCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        public string DepartmentCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        public string TeamCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        public string SectionCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Date)]
        public DateTime? Date { get; set; }

        public string DepartmentName { get; set; }
        public string Divisionname { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ReportWrongShift_ScheduleShift)]
        public string ScheduleShift { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ReportWrongShift_ActualShift)]
        public string ActualShift { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_InTime)]
        public DateTime? TimeIn { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_OutTime)]
        public DateTime? TimeOut { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ReportWrongShift_ApprovedShift)]
        public string ApprovedShift { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ReportWrongShift_Status)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Notes)]
        public string Notes { get; set; }


        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_ReleaseNote_DateRelease)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Shift_ShiftName)]
        public Guid? ShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_ShiftName)]
        public string ShiftIDs { get; set; }
        public string ShiftName { get; set; }
        public string OrgCode { get; set; }
        public string PositionName { get; set; }
        public string JobtitleName { get; set; }
              [DisplayName(ConstantDisplay.HRM_Attendance_DateExport)]
        public DateTime DateExport { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }
        public Guid ExportId { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_isIncludeQuitEmp)]
        public bool isIncludeQuitEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }

        public string ValueFields { get; set; }


        public partial class FieldNames
        {
            public const string Id = "Id";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";

            public const string BranchCode = "BranchCode";
            public const string DepartmentCode = "DepartmentCode";
            public const string TeamCode = "TeamCode";
            public const string SectionCode = "SectionCode";

            public const string Date = "Date";

            public const string ScheduleShift = "ScheduleShift";
            public const string ActualShift = "ActualShift";
            public const string TimeIn = "TimeIn";
            public const string TimeOut = "TimeOut";
            public const string ApprovedShift = "ApprovedShift";
            public const string Status = "Status";
            public const string Notes = "Notes";
            public const string isIncludeQuitEmp = "isIncludeQuitEmp";

            public const string OrgStructureID = "OrgStructureID";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";

            public const string ShiftName = "ShiftName";
            public const string OrgCode = "OrgCode";
            public const string PositionName = "PositionName";
            public const string JobtitleName = "JobtitleName";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";

        }
    }
}
