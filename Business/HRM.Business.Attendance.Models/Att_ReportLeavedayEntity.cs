using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportLeavedayEntity : HRMBaseModel
    {        
        public DateTime FromDate { get; set; }
      
        public DateTime ToDate { get; set; }

        public string CodeEmp { get; set; }
      
        public string ProfileName { get; set; }

        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string PositionCode { get; set; }
        public string PositionName { get; set; }
        public string JobTitleCode { get; set; }
        public string JobTitleName { get; set; }
        public DateTime? DateLeaday { get; set; }
      

        public double? Code { get; set; }

       
        public DateTime DateExport { get; set; }
      
        
        public string ShiftName { get; set; }

       

      
        public Dictionary<string, double> Time { get; set; }

        public List<int> OrgStructureID { get; set; }

        public Dictionary<string, Dictionary<string, double>> Leave { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        //public double? Paid { get; set; }
        public string Paid { get; set; }
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
        public string Remark { get; set; }

        public string UserExport { get; set; }
        public partial class FieldNames
        {
            //ma nv
            public const string CodeEmp = "CodeEmp";
            //ten nv
            public const string ProfileName = "ProfileName";
            //ma chi nhanh
            public const string BranchCode = "BranchCode";
            public const string BranchName = "BranchName";
            public const string GroupCode = "GroupCode";
            public const string GroupName = "GroupName";
            //ma phong ban
            public const string DepartmentCode = "DepartmendCode";
            public const string DepartmentName = "DepartmentName";
            //ma nhom ma to
            public const string TeamCode = "TeamCode";
            public const string TeamName = "TeamName";
            //ma nhom
            public const string SectionCode = "SectionCode";
            public const string SectionName = "SectionName";
            public const string DivisionCode = "DivisionCode";
            public const string DivisionName = "DivisionName";
            public const string PositionCode = "PositionCode";
            public const string PositionName = "PositionName";
            public const string JobTitleCode = "JobTitleCode";
            public const string JobTitleName = "JobTitleName";
            public const string DateLeaday = "DateLeaday";
            public const string Paid = "Paid";
            public const string InTime = "InTime";
            public const string OutTime = "OutTime";
            public const string ShiftName = "ShiftName";
            public const string FromDate = "FromDate";
            public const string ToDate = "ToDate";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";

        }
        
    }
}
