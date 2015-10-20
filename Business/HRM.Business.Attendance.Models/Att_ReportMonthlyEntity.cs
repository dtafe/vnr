using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

using HRM.Infrastructure.Utilities;
using System.ComponentModel;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportMonthlyEntity : HRMBaseModel
    {
        public DateTime? DateHire { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public string PositionName { get; set; }
        public Nullable<DateTime> DateQuit { get; set; }
        public Nullable<double> TotalDayHavePaid { get; set; }
        public Nullable<double> RealdayWorking { get; set; }
        public Nullable<double> LateEarlyTotal { get; set; }
        public Nullable<double> TotalNightShiftHourOver8 { get; set; }
        public Nullable<double> TotalNightShiftHourUnder8 { get; set; }
        public Nullable<double> HourOvertimeOver8PerDay { get; set; }
        public partial class FieldNames
        {
            public const string DateHire = "DateHire";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string DepartmentName = "DepartmentName";
            public const string SectionName = "SectionName";
            public const string PositionName = "PositionName";
            public const string DateQuit = "DateQuit";
            public const string TotalDayHavePaid = "TotalDayHavePaid";
            public const string RealdayWorking = "RealdayWorking";
            public const string TotalNightShiftHourOver8 = "TotalNightShiftHourOver8";
            public const string TotalNightShiftHourUnder8 = "TotalNightShiftHourUnder8";
            public const string HourOvertimeOver8PerDay = "HourOvertimeOver8PerDay";
            public const string LateEarlyTotal = "LateEarlyTotal";

        }
    }
}
