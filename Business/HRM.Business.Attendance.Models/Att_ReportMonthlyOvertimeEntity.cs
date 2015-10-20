using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportMonthlyOvertimeEntity : HRMBaseModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string PositionName { get; set; }
        public string JobTitleName { get; set; }
        public string Department { get; set; }
        public string DepartmentCode { get; set; }
        public string DeptPath { get; set; }
        public string Section { get; set; }
        public string DepartmentInRoster { get; set; }
        public string ALLOWANCE_BREAKF { get; set; }
        public string WorkPlace { get; set; }
        public string OrgStructure { get; set; }
        public string OrgStructureCode { get; set; }
        public string WorkingPlace { get; set; }
        public string DataNightShift { get; set; }
        public double? NightShift { get; set; }
        public double? AdditionHours_NonWorkDay { get; set; }
        public double? AdditionHours_LeaveDay { get; set; }
        public double? LeaveHours { get; set; }
        public double? Totals { get; set; }
        public double? CumulativeOT { get; set; }
        public double? OT_EARLY { get; set; }
        public double? OT_HOME { get; set; }
        public double? OT_PROVIDER { get; set; }
        public double? OT_BU { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string UserExport { get; set; }
        public DateTime DateExport { get; set; }
        public string ReasonOT { get; set; }

        public partial class FieldNames
        {
            public const string ReasonOT = "ReasonOT";
            public const string Data = "Data";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";
            public const string Department = "Department";
            public const string DepartmentCode = "DepartmentCode";
            public const string DeptPath = "DeptPath";
            public const string Section = "Section";
            public const string DepartmentInRoster = "DepartmentInRoster";
            public const string ALLOWANCE_BREAKF = "ALLOWANCE_BREAKF";
            public const string WorkPlace = "WorkPlace";
            public const string OrgStructure = "OrgStructure";
            public const string OrgStructureCode = "OrgStructureCode";
            public const string WorkingPlace = "WorkingPlace";
            public const string DataNightShift = "DataNightShift";
            public const string NightShift = "NightShift";
            public const string AdditionHours_NonWorkDay = "AdditionHours_NonWorkDay";
            public const string AdditionHours_LeaveDay = "AdditionHours_LeaveDay";
            public const string LeaveHours = "LeaveHours";
            public const string Totals = "Totals";
            public const string CumulativeOT = "CumulativeOT";
            public const string OT_EARLY = "OT_EARLY";
            public const string OT_HOME = "OT_HOME";
            public const string OT_PROVIDER = "OT_PROVIDER";
            public const string OT_BU = "OT_BU";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string UserExport = "UserExport";
            public const string DateExport = "DateExport";
        }
    }

    public class Att_ReportMonthlyOvertimeConditionEntity
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string OrgStructureID { get; set; }
        public string StatusEmployee { get; set; }
        public List<Guid> PayrollGroup { get; set; }
        public string OvertimeStatus { get; set; }
        public List<Guid> CumulativeType { get; set; }
        public string TypeHour { get; set; }
        public bool OvertimeDetail { get; set; }
        public bool RemoveNotHasWorkday { get; set; }
        public bool IncludeAllEmployee { get; set; }
        public bool RemoveCompensation100 { get; set; }
        public bool RecordIns { get; set; }
        public bool Cumulative { get; set; }
    }
}
