using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportShiftAdjustmentEntity : HRMBaseModel
    {       
        public DateTime? FromDate { get; set; }     
        public DateTime? ToDate { get; set; }       
        public string CodeEmp { get; set; }       
        public string ProfileName { get; set; }       
        public string PositionName { get; set; }       
        public string JobtitleName { get; set; }     
        public DateTime? Date { get; set; }    
        public string ShiftName { get; set; }     
        public List<int?> OrgStructureID { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public double? SumLateEarlyMinute { get; set; }
        public int ExportId { get; set; }
    }
}
