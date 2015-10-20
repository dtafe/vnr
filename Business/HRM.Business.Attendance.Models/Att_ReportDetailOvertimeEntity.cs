using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportDetailOvertimeEntity : HRMBaseModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string CodeBranch { get; set; }
        public string CodeOrg { get; set; }
        public string CodeTeam { get; set; }
        public string CodeSection { get; set; }
        public string CodeJobtitle { get; set; }
        public string CodePosition { get; set; }
        public string UserExport { get; set; }
        public string BranchName { get; set; }
        public string OrgName { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; }
        public DateTime? DateOvertime { get; set; }
        public DateTime DateExport { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime? udInTime { get; set; }
        public DateTime? udOutTime { get; set; }
        public string ShiftName { get; set; }
        public string udSubmitApproveHour { get; set; }
        public List<Guid?> OverTimeTypeID { get; set; }
        public string ReasonOT { get; set; }
        public partial class FieldNames
        {
            public const string ReasonOT = "ReasonOT";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string CodeBranch = "CodeBranch";
            public const string CodeOrg = "CodeOrg";
            public const string CodeTeam = "CodeTeam";
            public const string CodeSection = "CodeSection";
            public const string CodeJobtitle = "CodeJobtitle";
            public const string CodePosition = "CodePosition";
            public const string UserExport = "UserExport";
            public const string BranchName = "BranchName";
            public const string OrgName = "OrgName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string DateOvertime = "DateOvertime";
            public const string DateExport = "DateExport";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string udInTime = "udInTime";
            public const string udOutTime = "udOutTime";
            public const string ShiftName = "ShiftName";
            public const string udSubmitApproveHour = "udSubmitApproveHour";
            public const string OverTimeTypeID = "OverTimeTypeID";
        }
    }
}
