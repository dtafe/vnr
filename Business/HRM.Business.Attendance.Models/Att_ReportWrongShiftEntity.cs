using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;

namespace HRM.Business.Attendance.Models
{
    public class Att_ReportWrongShiftEntity
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }

        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string DepartmentCode { get; set; }
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string DivisionName { get; set; }
        public string Division { get; set; }
        public string DepartmentName { get; set; }

        public string PositionName { get; set; }
        public string JobtitleName { get; set; }
        public DateTime? Date { get; set; }
        public DateTime DateExport { get; set; }

        public string ShiftName { get; set; }
        public string ScheduleShift { get; set; }
        public string ActualShift { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }

        public string ApprovedShift { get; set; }
        public string Notes { get; set; }

        public DateTime? udOutTime { get; set; }
       // public List<int> OrgStructureID { get; set; }
        public string OrgCode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? ShiftActualID { get; set; }
        public int? ShiftApproveID { get; set; }
        public string Status { get; set; }
        public string UserExport { get; set; }
       


    }
}
