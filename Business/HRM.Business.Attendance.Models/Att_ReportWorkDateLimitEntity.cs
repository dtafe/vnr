using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportWorkDateLimitEntity : HRMBaseModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string EmpCode { get; set; }
        public string EmployeeName { get; set; }
        public string PositionName { get; set; }
        public string JobTitleName { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? WorkDate { get; set; }
        public string OvertimeType { get; set; }
        public double? RegisterHours { get; set; }
        public double? ApproveHours { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public double? ConvertedHours { get; set; } 
        public double? ConfirmHours { get; set; }
        public string Status { get; set; }
        public List<int?> OrgStructureId { get; set; }
        public List<int?> PositionId { get; set; }
        public int? TemplateId { get; set; }
    }
}
