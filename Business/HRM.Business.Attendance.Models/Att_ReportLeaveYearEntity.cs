using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportLeaveYearEntity : HRMBaseModel
    {      
        public DateTime? FromDate { get; set; }
      
        public DateTime? ToDate { get; set; }
     
        public string CodeEmp { get; set; }
       
        public string ProfileName { get; set; }

        public string BranchCode { get; set; }
        public string DepartmentCode { get; set; }      
        public string TeamCode { get; set; }      
        public string SectionCode { get; set; }
       
        public string DivisionCode { get; set; }
      
        public string ShiftName { get; set; }
      
        public string Remark { get; set; }
       
        public List<int> OrgStructureID { get; set; }
      
        public int? BugetYearP { get; set; }

        public int? BugetYearSC { get; set; }
        public List<Guid?> LeaveDayTypeIDs { get; set; }
        //public string LeaveDayTypeName { get; set; }

        public Dictionary<string, double> BudgetYear { get; set; }

        public Dictionary<DateTime, Dictionary<string, double>> MonthLeave { get; set; }
               
        public double? TotalP { get; set; }
       
        public double? TotalSC { get; set; }
      
        public double? BalanceP { get; set; }
       
        public double? BalanceSC { get; set; }

        public string PositionName { get; set; }
        public string JobtitleName { get; set; }
        public double? Paid { get; set; }
        public DateTime? DateExport { get; set; }

        public double? Code { get; set; }

        public double? P { get; set; }
        public double? SC { get; set; }
        public double? SU { get; set; }
        public double? SD { get; set; }
        public double? SP { get; set; }
        public double? DSP { get; set; }
        public double? DP { get; set; }
        public double? DA { get; set; }
        public double? D { get; set; }
        public double? Bus { get; set; }
        public double? SH { get; set; }
        public double? M { get; set; }
        public double? CL { get; set; }
        public double? AL { get; set; }
        public double? CO { get; set; }
        public double? DL { get; set; }
        public double? SM { get; set; }
        public double? TSC { get; set; }
     
    }
}
