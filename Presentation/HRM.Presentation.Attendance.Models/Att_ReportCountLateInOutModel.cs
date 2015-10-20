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
    public class Att_ReportCountLateInOutModelExport
    {

      
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_BranchCode)]
        public string BranchCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        public string TeamCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        public string SectionCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeJobtitle)]
        public string CodeJobtitle { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CodePosition)]
        public string CodePosition { get; set; }


        [DisplayName(ConstantDisplay.HRM_Att_Report_Status)]
        public string ShiftName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_ShiftID)]
        public string ShiftIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeOrg)]
        public string CodeOrg { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_BranchName)]
        public string BranchName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DepartmentName)]
        public string DepartmentName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_TeamName)]
        public string TeamName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_SectionName)]
        public string SectionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_OrgName)]
        public string OrgName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_EarlyDurationMore2)]
        public double EarlyDurationMore2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_EarlyDurationLess2)]
        public double EarlyDurationLess2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CountForget)]
        public double CountForget { get; set; }
  
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateTo)]
        public DateTime? DateTo { get; set; }
 
        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateExport)]
        public DateTime DateExport { get; set; }
    }
    public class Att_ReportCountLateInOutModel : BaseViewModel
    {

        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_BranchCode)]
        public string BranchCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        public string TeamCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        public string SectionCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeJobtitle)]
        public string CodeJobtitle { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CodePosition)]
        public string CodePosition { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_Status)]
        public string ShiftName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_WorkDay_ShiftID)]
        public string ShiftIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeOrg)]
        public string CodeOrg { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_BranchName)]
        public string BranchName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DepartmentName)]
        public string DepartmentName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_TeamName)]
        public string TeamName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_SectionName)]
        public string SectionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_OrgName)]
        public string OrgName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_EarlyDurationMore2)]
        public double EarlyDurationMore2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_EarlyDurationLess2)]
        public double EarlyDurationLess2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CountForget)]
        public double CountForget { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateExport)]
        public DateTime DateExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateTo)]
        public DateTime? DateTo { get; set; }
 

        public Guid ExportId { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_isIncludeQuitEmp)]
        public bool IsIncludeQuitEmp { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Attendance_IsNoteAllowZero)]
        [DisplayName(ConstantDisplay.HRM_Att_Report_NoDisplay0Data)]
        public bool IsNoteAllowZero { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public string ProfileID { get; set; }
        public partial class FieldNames
        {
            public const string Id = "Id";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string BranchCode = "BranchCode";
            public const string DepartmentCode = "DepartmentCode";
            public const string TeamCode = "TeamCode";
            public const string SectionCode = "SectionCode";
            public const string CodeJobtitle = "CodeJobtitle";
            public const string CodePosition = "CodePosition";
            public const string Date = "Date";
            public const string DateExport = "DateExport";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string UserExport = "UserExport";
            public const string Time = "Time";
            public const string Status = "Status";
            public const string ShiftName = "ShiftName";
            public const string MachineNo = "MachineNo";
            public const string CodeOrg = "CodeOrg";
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string OrgName = "OrgName";
            public const string EarlyDurationMore2 = "EarlyDurationMore2";
            public const string EarlyDurationLess2 = "EarlyDurationLess2";
            public const string CountForget = "CountForget";
        }
    }                           
}                               
                                