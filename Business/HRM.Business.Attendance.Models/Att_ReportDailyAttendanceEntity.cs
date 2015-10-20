using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportDailyAttendanceEntity : HRMBaseModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }

        public string BranchCode { get; set; }
        public string DepartmentCode { get; set; }
        public string TeamCode { get; set; }
        public string SectionCode { get; set; }
        public string Divisionname { get; set; }
        public string DepartmentName { get; set; }

        public string PositionName { get; set; }
        public string JobTitleName { get; set; }
        public DateTime? Date { get; set; }
        public DateTime DateExport { get; set; }

        public string ShiftName { get; set; }
        public string ScheduleShift { get; set; }
        public string ActualShift { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }

        //public DateTime? InTime { get; set; }
        //public DateTime? OutTime { get; set; }
        public string ApprovedShift { get; set; }
        public string Notes { get; set; }

        public DateTime? udOutTime { get; set; }
        public List<int> OrgStructureID { get; set; }
        public string OrgCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? ShiftActualID { get; set; }
        public int? ShiftApproveID { get; set; }
      //  public string ShiftName { get; set; }
        public string ShiftNameActual { get; set; }
        public string ShiftNameApprove { get; set; }
        public DateTime? WorkDate { get; set; }
        public string Type { get; set; }
        public string SrcType { get; set; }
        public string Note { get; set; }
        public int LateDuration1 { get; set; }
        public int EarlyDuration1 { get; set; }
        public int LateEarlyDuration { get; set; }
        public int LateEarlyRoot { get; set; }
        public string LateEarlyReason { get; set; }
        public DateTime? InTimeRoot { get; set; }
        public DateTime? OutTimeRoot { get; set; }
      //  public string DepartmentCode { get; set; }
       // public string SectionCode { get; set; }
      //  public string BrandCode { get; set; }
       // public string TeamCode { get; set; }
        public string ProfileOrgCode { get; set; }
        public string UserExport { get; set; }
        public string EarlyLateLess2h { get; set; }
        public string EarlyLateOver2h { get; set; }
        public string Status { get; set; }
       // public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string TeamName { get; set; }
        public string SectionName { get; set; }
   
    

        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string ShiftName = "ShiftName";
            public const string ActualShift = "ActualShift";
            public const string ApprovedShift = "ApprovedShift";
            public const string TimeIn = "TimeIn";
            public const string TimeOut = "TimeOut";
            public const string WorkDate = "WorkDate";
            public const string Type = "Type";
            public const string SrcType = "SrcType";
            public const string Status = "Status";
            public const string Note = "Note";
            public const string LateDuration1 = "LateDuration1";
            public const string EarlyDuration1 = "EarlyDuration1";
            public const string LateEarlyDuration = "LateEarlyDuration";
            public const string LateEarlyRoot = "LateEarlyRoot";
            public const string LateEarlyReason = "LateEarlyReason";
            public const string InTimeRoot = "InTimeRoot";
            public const string OutTimeRoot = "OutTimeRoot";
            public const string DepartmentCode = "DepartmentCode";
            public const string SectionCode = "SectionCode";
            public const string BrandCode = "BrandCode";
            public const string TeamCode = "TeamCode";
            public const string ProfileOrgCode = "ProfileOrgCode";
            public const string UserExport = "UserExport";
            public const string DateExport = "DateExport";
            public const string EarlyLateLess2h = "EarlyLateLess2h";
            public const string EarlyLateOver2h = "EarlyLateOver2h";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string BranchName = "BranchName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string BranchCode = "BranchCode";
            public const string PositionName = "SectionName";
            public const string JobTitleName = "JobTitleName";
        }

    }
    
}
