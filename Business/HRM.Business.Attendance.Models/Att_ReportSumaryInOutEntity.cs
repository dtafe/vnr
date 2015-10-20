using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportSumaryInOutEntity : HRMBaseModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime? DateExport { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string BranchCode { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string SectionCode { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public string JobtitleName { get; set; }
        public string DepartmentCode { get; set; }
        public string Division { get; set; }
        public string DivisionName { get; set; }
        public DateTime? Date { get; set; }
        public string ShiftName { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public string Remark { get; set; }
        public string TeamCode { get; set; }
        public List<int?> OrgStructureID { get; set; }
        public DateTime? udTimeIn { get; set; }
        public DateTime? udTimeOut{ get; set; }
        public string UserExport { get; set; }
        public string ApprovedShift { get; set; }
      
        
        
        
        public partial class FieldNames 
        {
         
            public const string Date = "Date";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string PositionName = "PositionName";
            public const string JobtitleName = "JobtitleName";
            public const string Division = "Division";
            public const string DivisionName = "DivisionName";
            public const string Department = "Department";
            public const string SectionCode = "SectionCode";
            public const string TeamCode = "TeamCode";
            public const string ShiftName = "ShiftName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string UserPrint = "UserPrint";
            public const string DatePrint = "DatePrint";
            public const string OrgStructureName = "OrgStructureName";
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string InTime = "InTime";
            public const string OutTime = "OutTime";
            public const string Remark = "Remark";
            public const string GroupCode = "GroupCode";
            public const string GroupName = "GroupName";
            public const string udTimeIn = "udTimeIn";
            public const string udTimeOut = "udTimeOut";
            public const string BranchCode = "BranchCode";
            public const string DepartmentCode = "DepartmentCode";
            public const string DateExport = "DateExport";
            public const string UserExport = "UserExport";
            public const string ApprovedShift = "ApprovedShift";
        }
       
    }
}
