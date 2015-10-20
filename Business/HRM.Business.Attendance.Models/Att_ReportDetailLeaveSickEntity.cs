using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportDetailLeaveSickEntity
    {
         public string CodeEmp { get; set; }

        public string ProfileName { get; set; }

        public string BranchCode { get; set; }

        public string DepartmentCode { get; set; }

        public string TeamCode { get; set; }

        public string SectionCode { get; set; }

        public DateTime? Date { get; set; }

        public string DivisionCode { get; set; }

        public string ShiftName { get; set; }

        public string PositionName { get; set; }
        public string JobtitleName { get; set; }

        public Dictionary<string, double> Time { get; set; }

        public List<int> OrgStructureID { get; set; }

        public Dictionary<string, Dictionary<string, double>> Leave { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public string IsPaid { get; set; }
        public string SC { get; set; }
        public string SU { get; set; }
        public string SD { get; set; }
        public double? CodeStatistic { get; set; }
        public string Remark { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string UserExport { get; set; }
        public DateTime? DateExport { get; set; }
    }
}
