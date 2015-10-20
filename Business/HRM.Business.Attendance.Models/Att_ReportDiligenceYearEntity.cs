using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportDiligenceYearEntity : HRMBaseModel
    {
    
        public string DepartmentName { get; set; }
    
        public string DepartmentCode { get; set; }
  
        public DateTime? Date { get; set; }
        public string ShiftName { get; set; }
        public double Sizes { get; set; }
        public double Absence { get; set; }
 
        public List<int?> OrgStructureID { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string UserExport { get; set; }
        public DateTime DateExport { get; set; }

        public partial class FieldNames 
        {
         
            public const string Date = "Date";
            public const string ShiftName = "ShiftName";
            public const string OrgStructureName = "OrgStructureName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string DepartmentCode = "DepartmentCode";
            public const string Sizes = "Sizes";
            public const string Absence = "Absence";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string UserExport = "UserExport";
            public const string DateExport = "DateExport";
        }
       
    }
}
