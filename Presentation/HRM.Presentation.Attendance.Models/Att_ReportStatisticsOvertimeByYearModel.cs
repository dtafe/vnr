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
    public class Att_ReportStatisticsOvertimeByYearModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeBranch)]
        public string CodeBranch { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeOrg)]
        public string CodeOrg { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeTeam)]
        public string CodeTeam { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeSection)]
        public string CodeSection { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeJobtitle)]
        public string CodeJobtitle { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CodePosition)]
        public string CodePosition { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_BranchName)]
        public string BranchName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_OrgName)]
        public string OrgName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_TeamName)]
        public string TeamName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_SectionName)]
        public string SectionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateExport)]
        public DateTime DateExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_OvertimeTypeID)]
        public List<Guid?> OvertimeTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_NoDisplay0Data)]
        public bool NoDisplay0Data { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_SumHourCO)]
        public double SumHourCO { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualLeave_Year)]
        public int Year { get; set; }
        public Guid ExportId { get; set; }
         [DisplayName(ConstantDisplay.HRM_Attendance_isIncludeQuitEmp)]
        public bool IsIncludeQuitEmp { get; set; }

        public decimal M1 { get; set; }
        public decimal M2 { get; set; }
        public decimal M3 { get; set; }
        public decimal M4 { get; set; }
        public decimal M5 { get; set; }
        public decimal M6 { get; set; }
        public decimal M7 { get; set; }
        public decimal M8 { get; set; }
        public decimal M9 { get; set; }
        public decimal M10 { get; set; }
        public decimal M11 { get; set; }
        public decimal M12 { get; set; }


        public partial class FieldNames
        {
           public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string CodeBranch = "CodeBranch";
            public const string CodeOrg = "CodeOrg";
            public const string CodeTeam = "CodeTeam";
            public const string CodeSection = "CodeSection";
            public const string CodeJobtitle = "CodeJobtitle";
            public const string CodePosition = "CodePosition";
            public const string BranchName = "BranchName";
            public const string OrgName = "OrgName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string DateExport = "DateExport";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string UserExport = "UserExport";
            public const string SumHourCO = "SumHourCO";
            public const string Year = "Year";
            public const string M1 = "M1";
            public const string M2 = "M2";
            public const string M3 = "M3";
            public const string M4 = "M4";
            public const string M5 = "M5";
            public const string M6 = "M6";
            public const string M7 = "M7";
            public const string M8 = "M8";
            public const string M9 = "M9";
            public const string M10 = "M10";
            public const string M11 = "M11";
            public const string M12 = "M12";

        }
    }
}                               
                                