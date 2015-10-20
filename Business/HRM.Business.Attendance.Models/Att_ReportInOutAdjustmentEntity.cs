using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportInOutAdjustmentEntity
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string ShiftName { get; set; }
        public string ShiftNameActual { get; set; }
        public string ShiftNameApprove { get; set; }
        public DateTime? InTime1 { get; set; }
        public DateTime? OutTime1 { get; set; }
        public DateTime? InTimeRoot { get; set; }
        public DateTime? OutTimeRoot { get; set; }
        public DateTime WorkDate { get; set; }
        public String Type { get; set; }
        public String SrcType { get; set; }
        public String Status { get; set; }
        public String Note { get; set; }
        public Double? LateDuration1 { get; set; }
        public Double? EarlyDuration1 { get; set; }
        public Double? LateEarlyDuration { get; set; }
        public Double? LateEarlyRoot { get; set; }
        public String LateEarlyReason { get; set; }
        public String DepartmentCode { get; set; }
        public String DepartmentName { get; set; }
        public String SectionCode { get; set; }
        public String SectionName { get; set; }
        public String BrandCode { get; set; }
        public String BrandName { get; set; }
        public String TeamCode { get; set; }
        public String TeamName { get; set; }
        public String PositionCode { get; set; }
        public String PositionName { get; set; }
        public String ProfileOrgCode { get; set; }
        public String ProfileOrgName { get; set; }
        public String UserExport { get; set; }
        public DateTime DateExport { get; set; }
        public String EarlyLateLess2h { get; set; }
        public String EarlyLateOver2h { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string MissInOutReason { get; set; }
    }
}
