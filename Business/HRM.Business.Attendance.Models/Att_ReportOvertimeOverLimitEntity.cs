using HRM.Business.BaseModel;
using System;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportOvertimeOverLimitEntity : HRMBaseModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string DepartmentName { get; set; }
        public string Position { get; set; }
        public double? OTHours { get; set; }
    }
}
