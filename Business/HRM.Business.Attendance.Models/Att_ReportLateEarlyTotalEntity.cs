using HRM.Business.BaseModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportLateEarlyTotalEntity : HRMBaseModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string OrgStructureName { get; set; }
        public DateTime? SDateFrom { get; set; }
        public DateTime? SDateTo { get; set; }
        public string OrgStructureID { get; set; }
        public Guid? ExportId { get; set; }
        public string ValueFields { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Less2Hour)]
        public double? Less2Hour { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Over2Hour)]
        public double? Over2Hour { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TotalLateEarly)]
        public double? TotalLateEarly { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ForgetTamscan)]
        public double? ForgetTamscan { get; set; }
        public DateTime DateExport { get; set; }
        public string UserExport { get; set; }
        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string BranchCode = "BranchCode";
            public const string BranchName = "BranchName";
            public const string DepartmentCode = "DepartmentCode";
            public const string DepartmentName = "DepartmentName";
            public const string TeamCode = "TeamCode";
            public const string TeamName = "TeamName";
            public const string SectionCode = "SectionCode";
            public const string SectionName = "SectionName";
            public const string OrgStructureName = "OrgStructureName";
            public const string Less2Hour = "Less2Hour";
            public const string Over2Hour = "Over2Hour";
            public const string TotalLateEarly = "TotalLateEarly";
            public const string ForgetTamscan = "ForgetTamscan";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";
        }

    }
}
