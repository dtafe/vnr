using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportDetailOvertimeVer2Entity : HRMBaseModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string BranchCode { get; set; }
        public string DepartmentCode { get; set; }
        public string TeamCode { get; set; }
        public string SectionCode { get; set; }
        public DateTime? WorkDay { get; set; }
        public string ShiftName { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public string OTType { get; set; }
        public Dictionary<string, double> OTHour { get; set; }
        public Dictionary<string, double> OTHourConfirm { get; set; }
        public List<int> OrgStructureID { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? DateExport { get; set; }
        public string UserExport { get; set; }
    }
}
