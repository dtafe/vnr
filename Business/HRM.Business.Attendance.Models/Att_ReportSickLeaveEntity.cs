using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportSickLeaveEntity : HRMBaseModel
    {      
        public DateTime? DateFrom { get; set; }
      
        public DateTime? DateTo { get; set; }
     
        public string CodeEmp { get; set; }
       
        public string ProfileName { get; set; }

        public string BranchCode { get; set; }
        public string DepartmentCode { get; set; }
        public string TeamCode { get; set; }      
        public string SectionCode { get; set; }
       
        public string DivisionCode { get; set; }
      
        public string ShiftName { get; set; }

        public Dictionary<string, double> BudgetYear { get; set; }
      
        public string Remark { get; set; }
       
        public List<int> OrgStructureID { get; set; }

        public Dictionary<DateTime, Dictionary<string, double>> MonthLeave { get; set; }
      
        public int? BugetYearP { get; set; }
      
        public int? BugetYearSC { get; set; }
               
        public double? TotalP { get; set; }
       
        public double? TotalSC { get; set; }
      
        public double? BalanceP { get; set; }
       
        public double? BalanceSC { get; set; }
       
        public string PositionName { get; set; }
        public string JobtitleName { get; set; }
        public double? Paid { get; set; }
        public DateTime? DateExport { get; set; }

        public double? Code { get; set; }

        public double? MonthP { get; set; }
        public double? MonthSC { get; set; }
        public string UserExport { get; set; }
        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string BranchCode = "BranchCode";
            public const string orgOrg = "orgOrg";
            public const string TeamCode = "TeamCode";
            public const string TotalP = "TotalP";
            public const string TotalSC = "TotalSC";
            public const string SumANL = "SumANL";
            public const string Data = "Data";
            public const string DataHeader = "DataHeader";
            public const string SumSICK = "SumSICK";
            public const string CodeSection = "CodeSection";
            public const string BranchName = "BranchName";
            public const string OrgName = "OrgName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string CodePosition = "CodePosition";
            public const string CodeJobtitle = "CodeJobtitle";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string DepartmentCode = "DepartmentCode";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";
        }
    }
}
