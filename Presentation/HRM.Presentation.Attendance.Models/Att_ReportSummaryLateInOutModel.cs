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
    public class Att_ReportSummaryLateInOutModel : BaseViewModel
    {
        public string LateForWork { get; set; }
        public string EarlyGoHome { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        public string GroupCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        public string SectionCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        public string DepartmentName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_DivisionCode)]
        public string DivisionCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_NumTimeLate)]
        public int? NumTimeLate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_NumTimeEarly)]
        public int? NumTimeEarly { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_LateMinutes)]
        public int? LateMinutes { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_EarlyMinutes)]
        public int? EarlyMinutes { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Remark)]
        public string Remark { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_OrgStructureID)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Position_PositionName)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_JobTitle_JobTitleName)]
        public string JobtitleName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_BranchCode)]
        public string BranchCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        public string DepartmentCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        public string TeamCode { get; set; }

        public string PositionCode { get; set; }
        public string JobtitleCode { get; set; }
           public Guid? ShiftID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ShiftId)]
        public string ShiftIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_isIncludeQuitEmp)]
        public bool isIncludeQuitEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_OutTime)]
        public DateTime? udOutTime { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_InTime)]
        public DateTime? udInTime { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ShiftId)]
        public string ShiftName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_EarlyDurationMore2)]
        public string EarlyDurationMore2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_EarlyDurationLess2)]
        public string EarlyDurationLess2 { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_Attendance_Date)]
        public DateTime? Date { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_MissInOutReason)]
        public string udTAMScanReasonMiss { get; set; }
       
        public Guid ExportId { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateExport)]
        public DateTime DateExport { get; set; }

        public partial class FieldNames
        {          
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string NumTimeLate = "NumTimeLate";
            public const string NumTimeEarly = "NumTimeEarly";
            public const string LateMinutes = "LateMinutes";
            public const string EarlyMinutes = "EarlyMinutes";
            public const string Remark = "Remark";
            public const string OrgStructureID = "OrgStructureID";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string PositionName = "PositionName";
            public const string JobtitleName = "JobtitleName";
            public const string BranchCode = "BranchCode";
            public const string DivisionCode = "DivisionCode";
            public const string DepartmentCode = "DepartmentCode";
            public const string TeamCode = "TeamCode";
            public const string SectionCode = "SectionCode";
            public const string Date = "Date";
            public const string EarlyDurationLess2 = "EarlyDurationLess2";
            public const string EarlyDurationMore2 = "EarlyDurationMore2";
            public const string udInTime = "udInTime";
            public const string ShiftName = "ShiftName";
            public const string udOutTime = "udOutTime";
            public const string udTAMScanReasonMiss = "udTAMScanReasonMiss";
            public const string UserExport = "UserExport";
            public const string DateExport = "DateExport";
        }
    }
}
