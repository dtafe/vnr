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
    public class Att_ReportDailyAttendanceModel : BaseViewModel
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

        // [DisplayName(ConstantDisplay.HRM_Attendance_InTime)]
        //public DateTime? InTime { get; set; }
        //   [DisplayName(ConstantDisplay.HRM_Attendance_OutTime)]
        //public DateTime? OutTime { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDate)]
        public DateTime? WorkDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ReportWrongShift_ApprovedShift)]
        public string ApprovedShift { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ReportWrongShift_Status)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Notes)]
        public string Notes { get; set; }


        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_ShiftName)]
        public Guid? ShiftID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_ShiftName)]
        public string ShiftIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Shift_ShiftName)]
        public string ShiftName { get; set; }

        public string OrgCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Position )]
        public string PositionName { get; set; }

           [DisplayName(ConstantDisplay.HRM_Attendance_JobTitle)]
        public string JobtitleName { get; set; }
        public DateTime DateExport { get; set; }
        public Guid ExportId { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_excludeNotInOut)]
        public bool excludeNotInOut { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }

        public Guid? PayrollID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_PayrollID)]
        public string PayrollIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_Type)]
        public string Type { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_SrcType)]
        public string SrcType { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Notes)]
        public string Note { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_LateDuration1)]
        public int LateDuration1 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_EarlyDuration1)]
        public int EarlyDuration1 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_LateEarlyDuration)]
        public int LateEarlyDuration { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_LateEarlyRoot)]
        public int LateEarlyRoot { get; set; }

          [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_LateEarlyReason)]
        public string LateEarlyReason { get; set; }

         [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_InTimeRoot)]
        public DateTime? InTimeRoot { get; set; }

         [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_OutTimeRoot)]
        public DateTime? OutTimeRoot { get; set; }

       
        public string ProfileOrgCode { get; set; }

         [DisplayName(ConstantDisplay.HRM_Attendance_EarlyDurationLess2)]
        public string EarlyLateLess2h { get; set; }

         [DisplayName(ConstantDisplay.HRM_Attendance_EarlyDurationMore2)]
        public string EarlyLateOver2h { get; set; }

         [DisplayName(ConstantDisplay.HRM_Attendance_BranchName1)]
        public string BranchName { get; set; }

         [DisplayName(ConstantDisplay.HRM_Attendance_TeamName1)]
        public string TeamName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_SectionName1)]
        public string SectionName { get; set; }

           [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }

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
            public const string excludeNotInOut = "isIncludeQuitEmp";
            public const string OrgStructureID = "OrgStructureID";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string ShiftName = "ShiftName";
            public const string OrgCode = "OrgCode";
            public const string PositionName = "PositionName";
            public const string JobtitleName = "JobtitleName";
            public const string DateExport = "DateExport";
            public const string WorkDate = "WorkDate";
            public const string Type = "Type";
            public const string SrcType = "SrcType";
            public const string Note = "Note";
            public const string EarlyDuration1 = "EarlyDuration1";
            public const string LateEarlyDuration = "LateEarlyDuration";
            public const string LateEarlyRoot = "LateEarlyRoot";
            public const string LateEarlyReason = "LateEarlyReason";
            public const string InTimeRoot = "InTimeRoot";
            public const string OutTimeRoot = "OutTimeRoot";
            public const string ProfileOrgCode = "ProfileOrgCode";
            public const string UserExport = "UserExport";
            public const string EarlyLateLess2h = "EarlyLateLess2h";
            public const string EarlyLateOver2h = "EarlyLateOver2h";
            public const string BranchName = "BranchName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string LateDuration1 = "LateDuration1";
            public const string BrandCode = "BrandCode";
        }
    }
}
