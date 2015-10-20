using System;
using System.Collections.Generic;

namespace HRM.Business.Laundry.Models
{
    public class Lau_ReportExceptClothingEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Guid? ProfileID { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string LineName { get; set; }
        public Guid? LineID { get; set; }
        public string LockerName { get; set; }
        public Guid? LockerID { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        // Số bộ giặt trong tháng
        public int? AmountClothing { get; set; }
        // Tieu chuẩn giặt trong tháng
        public int? SumMonthAmount { get; set; }
        // Số bộ vượt tiêu chuẩn
        public int? ExceedingStandards { get; set; }
        // Tiền trừ vào lương
        public double? SubtractSalary { get; set; }
        public List<Guid?> ShiftID { get; set; }
        public string ShiftName { get; set; }
    }
}
