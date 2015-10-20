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
    public class Att_ReportSummaryLeaveYearSickModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualLeave_Year)]
        public int Year { get; set; }
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

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ReportSummaryLeaveYearSick_TotalAllowYear)]
        public double? TotalAllowYear { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ReportSummaryLeaveYearSick_TotalAllowSick)]
        public double? TotalAllowSick { get; set; }

        public double? P1 { get; set; }
        public double? SC1 { get; set; }
        public double? P2 { get; set; }
        public double? SC2 { get; set; }
        public double? P3 { get; set; }
        public double? SC3 { get; set; }
        public double? P4 { get; set; }
        public double? SC4 { get; set; }
        public double? P5 { get; set; }
        public double? SC5 { get; set; }
        public double? P6 { get; set; }
        public double? SC6 { get; set; }
        public double? P7 { get; set; }
        public double? SC7 { get; set; }
        public double? P8 { get; set; }
        public double? SC8 { get; set; }
        public double? P9 { get; set; }
        public double? SC9 { get; set; }
        public double? P10 { get; set; }
        public double? SC10 { get; set; }
        public double? P11 { get; set; }
        public double? SC11 { get; set; }
        public double? P12 { get; set; }
        public double? SC12 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Remark)]
        public string Remark { get; set; }
        public Guid ExportId { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_NoDisplay0Data)]
        public bool NoDisplay0Data { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Leaveday_LeaveDayTypeID)]
        public string LeaveDayTypeIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_UserExport)]
        public string UserExport { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateExport)]
        public DateTime DateExport { get; set; }

        public string ValueFields { get; set; }

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

            public const string TotalAllowYear = "TotalAllowYear";
            public const string TotalAllowSick = "TotalAllowSick";
            public const string P1 = "P1";
            public const string SC1 = "SC1";
            public const string P2 = "P2";
            public const string SC2 = "SC2";
            public const string P3 = "P3";
            public const string SC3 = "SC3";
            public const string P4 = "P4";
            public const string SC4 = "SC4";
            public const string P5 = "P5";
            public const string SC5 = "SC5";
            public const string P6 = "P6";
            public const string SC6 = "SC6";
            public const string P7 = "P7";
            public const string SC7 = "SC7";
            public const string P8 = "P8";
            public const string SC8 = "SC8";
            public const string P9 = "P9";
            public const string SC9 = "SC9";
            public const string P10 = "P10";
            public const string SC10 = "SC10";
            public const string P11 = "P11";
            public const string SC11 = "SC11";
            public const string P12 = "P12";
            public const string SC12 = "SC12";
            public const string NoDisplay0Data = "NoDisplay0Data";
            public const string LeaveDayTypeIDs = "constantName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string UserExport = "UserExport";
            public const string DateExport = "DateExport";
        }
    }
}
