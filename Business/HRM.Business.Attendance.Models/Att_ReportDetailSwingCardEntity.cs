using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportDetailSwingCardEntity
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string BranchCode { get; set; }
        public string GroupCode { get; set; }
        public string DepartmentCode { get; set; }
        public string TeamCode { get; set; }
        public string SectionCode { get; set; }
        public string CodeJobtitle { get; set; }
        public string CodePosition { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }
        public string ShiftName { get; set; }
        public string MachineNo { get; set; }
        public string CodeOrg { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; }
        public string OrgName { get; set; }
        public string CardCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string UserExport { get; set; }
        public DateTime? DateExport { get; set; }

        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string CardCode = "CardCode";
            public const string WorkDate = "WorkDate";
            public const string TimeLog = "TimeLog";
            public const string MachineNo = "MachineNo";
            public const string Status = "Status";
            public const string ShiftName = "ShiftName";
            public const string CodeBranch = "CodeBranch";
            public const string CodeDepartment = "CodeDepartment";
            public const string CodeTeam = "CodeTeam";
            public const string CodeSection = "CodeSection";
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string DateExport = "DateExport";
        }
    }
}
