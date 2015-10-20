using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportProfileAllowLimitOvertimeEntity : HRMBaseModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string Position { get; set; }
        public string JobTitle { get; set; }
        public string UserExport { get; set; }
        public string DateExport { get; set; }
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
