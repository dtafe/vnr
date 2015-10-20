using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportDetailedMonthlyStayEntity
    {      
  
      
        public string CodeEmp { get; set; }
       
        public string ProfileName { get; set; }

        public string BranchCode { get; set; }
        public string DepartmentCode { get; set; }      
        public string TeamCode { get; set; }      
        public string SectionCode { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }      
       
        public string ShiftName { get; set; }
    
        public List<int> OrgStructureID { get; set; }
       
        public int? TotalDateLeave { get; set; }

        public Dictionary<string, Dictionary<string, double>> Leave { get; set; }

        public string PositionName { get; set; }
        public string JobTitleName { get; set; }
        
      

        public double? Code { get; set; }
        public string DivisionCode { get; set; }
        
        public double? Paid { get; set; }
        public double? SC { get; set; }
        public double? SU { get; set; }
        public double? SD { get; set; }
        public double? SP { get; set; }
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

        public string BranchName { get; set; }
        public string OrgName { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; }
        public string Remark { get; set; }
        public string LeaveDayTypeName { get; set; }
        public string DateQuit { get; set; }
        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
        public string UserExport { get; set; }
        public DateTime DateExport { get; set; }

        public string E_COMPANY { get; set; }
        public string E_BRANCH { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string BranchCode = "BranchCode";
            public const string TeamCode = "TeamCode";
            public const string SectionCode = "SectionCode";
            public const string PositionName = "PositionName";
            public const string JobTitleName = "JobTitleName";
            public const string BranchName = "BranchName";
            public const string OrgName = "OrgName";
            public const string SectionName = "SectionName";
            public const string TeamName = "TeamName";
            public const string DepartmentCode = "DepartmentCode";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";
            public const string Paid = "Paid";
            public const string LeaveDayTypeName = "LeaveDayTypeName";
            public const string DateQuit = "DateQuit";

            public const string E_COMPANY = "E_COMPANY";
            public const string E_BRANCH = "E_BRANCH";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
        }
    }
}
