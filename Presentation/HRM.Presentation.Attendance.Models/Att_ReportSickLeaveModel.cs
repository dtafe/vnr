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
    public class Att_ReportSickLeaveModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FromDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ToDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]      
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]      
        public string ProfileName { get; set; }

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

        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        public string SectionCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_DivisionCode)]       
        public string DivisionCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ShiftId)]      
        public string ShiftName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Remark)]
        public string Remark { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ReportSickLeave_BugetYearP)]
        public int? BugetYearP { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ReportSickLeave_BugetYearSC)]
        public int? BugetYearSC { get; set; }
       
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportSickLeave_TotalP)]
        public double? TotalP { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ReportSickLeave_TotalSC)]
        public double? TotalSC { get; set; }
     
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportSickLeave_BalanceP)]
        public double? BalanceP { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ReportSickLeave_BalanceSC)]
        public double? BalanceSC { get; set; }

        public Dictionary<DateTime, Dictionary<string, double>> MonthLeave { get; set; }

         [DisplayName(ConstantDisplay.HRM_Attendance_ReportSickLeave_P)]
        public double? MonthP { get; set; }
         [DisplayName(ConstantDisplay.HRM_Attendance_ReportSickLeave_SC)]
        public double? MonthSC { get; set; }
         public Guid ExportId { get; set; }

        public partial class FieldNames
        {
            public const string Id = "Id";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string FromDate = "FromDate";
            public const string ToDate = "ToDate";
            public const string PositionName = "PositionName";
            public const string JobtitleName = "JobtitleName";
            public const string BranchCode = "BranchCode";
            public const string DivisionCode = "DivisionCode";
            public const string DepartmentCode = "DepartmentCode";
            public const string TeamCode = "TeamCode";
            public const string SectionCode = "SectionCode";
            
            public const string ShiftName = "ShiftName";
            public const string InTime = "InTime";
            public const string OutTime = "OutTime";

            public const string Paid = "Paid";
            public const string Remark = "Remark";

            public const string OrgStructureID = "OrgStructureID";
            public const string BugetYearP = "BugetYearP";
            public const string BugetYearSC = "BugetYearSC";
           
            public const string TotalP = "TotalP";
            public const string TotalSC = "TotalSC";
            public const string BalanceP = "BalanceP";
            public const string BalanceSC = "BalanceSC";
            public const string MonthP = "MonthP";
            public const string MonthSC = "MonthSC";
        }
    }
}
