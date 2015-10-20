using HRM.Business.BaseModel;
using System;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportExceptionDataAdvEntity : HRMBaseModel
    {
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string DepartmentName { get; set; }
        public string Position { get; set; }
        public string JobTitle { get; set; }
        public DateTime? InDate { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutDate { get; set; }
        public DateTime? OutTime { get; set; }
        public string ShiftName { get; set; }
        public DateTime? ShiftInTime { get; set; }
        public DateTime? ShiftOutTime { get; set; }
        public string OrgStructureID { get; set; }
        public string PayrollIDs { get; set; }
        public string StatusEmployees { get; set; }
        public bool ExcludeManagerStaff { get; set; }
        public bool ShowListLeaveDay { get; set; }
        public bool NoShift { get; set; }
        public bool LessHours { get; set; }
        public bool DifferenceMoreRoster { get; set; }
        public bool OTDuplicate { get; set; }
        public bool NoScan { get; set; }
        public double? More { get; set; }
        public double? Less { get; set; }
        public DateTime DateExport { get; set; }
        public string UserExport { get; set; }
        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string LeaveDayTypeName = "LeaveDayTypeName";
            public const string OvertimeTypeName = "OvertimeTypeName";
            public const string CodeEmp = "CodeEmp";
            public const string PositionName = "PositionName";
            public const string PositionCode = "PositionCode";
            public const string JobTitleName = "JobTitleName";
            public const string JobTitleCode = "JobTitleCode";
            public const string OrgStructureName = "OrgStructureName";
            public const string OrgStructureCode = "OrgStructureCode";
            public const string udDateOut = "udDateOut";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";

            public const string WorkDate = "WorkDate";
            public const string InTime = "InTime";
            public const string OutTime = "OutTime";
            public const string InHour = "InHour";
            public const string OutHour = "OutHour";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";
        }
    }

    public class Att_ReportExceptionDataAdv_ConditionEntity
    {
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string OrgStructureID { get; set; }
        public string PayrollGroup { get; set; }
        public string StatusEmployee { get; set; }
        public bool ExcludeManagerStaff { get; set; }
        public bool ShowListLeaveDay { get; set; }
        public bool NoShift { get; set; }
        public bool LessHours { get; set; }
        public bool DifferenceMoreRoster { get; set; }
        public bool OTDuplicate { get; set; }
        public bool MissInOut { get; set; }
        public bool NoScan { get; set; }
        public double? More { get; set; }
        public double? Less { get; set; }
    }
}
