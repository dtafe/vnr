using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportWarringOvertimeEntity : HRMBaseModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; }
        public string PositionName { get; set; }
        public string JobTitleName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime DateExport { get; set; }
        public string UserExport { get; set; }
        public string HourOverTime { get; set; }

        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string OrgStructureCode = "OrgStructureCode";
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";
            public const string PositionCode = "PositionCode";
            public const string JobTitleCode = "JobTitleCode";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string HourOverTime = "HourOverTime";
            public const string Color = "Color";
        }
    }
}
