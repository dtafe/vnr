using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportSummaryLeaveYearSickEntity : HRMBaseModel
    {
        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public string CodeEmp { get; set; }

        public string ProfileName { get; set; }

        public string BranchCode { get; set; }
        public string DepartmentCode { get; set; }
        public string TeamCode { get; set; }
        public string SectionCode { get; set; }

        public string DivisionCode { get; set; }

        public string ShiftName { get; set; }

        public List<int> OrgStructureID { get; set; }

        public string PositionName { get; set; }
        public string JobTitleName { get; set; }
        public string BranchName { get; set; }
        public string OrgName { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; }

        public DateTime DateExport { get; set; }


        public double? TotalAllowYear { get; set; }
        public double? TotalAllowSick { get; set; }
        public double? P { get; set; }
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
        public string Remark { get; set; }

        public string UserExport { get; set; }
        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string BranchCode = "BranchCode";
            public const string TeamCode = "TeamCode";
            public const string SectionCode = "SectionCode";
            public const string JobTitleName = "JobTitleName";
            public const string PositionName = "PositionName";
            public const string BranchName = "BranchName";
            public const string OrgName = "OrgName";
            public const string SectionName = "SectionName";
            public const string TeamName = "TeamName";
            public const string DepartmentCode = "DepartmentCode";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";
            public const string TotalAllowYear = "TotalAllowYear";
            public const string TotalAllowSick = "TotalAllowSick";
        }
    }
}
