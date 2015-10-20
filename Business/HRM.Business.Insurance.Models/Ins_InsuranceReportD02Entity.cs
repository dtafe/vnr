using System;
using HRM.Business.BaseModel;

namespace HRM.Business.Insurance.Models
{
    public class Ins_InsuranceReportD02Entity : HRMBaseModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string SocialInsNo { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? EndProbationDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Position { get; set; }
        public string JobTitle { get; set; }
        public double? OldBasicSalary { get; set; }
        public double? NewBasicSalary { get; set; }
        public Guid? ProfileID { get; set; }


        public string TitleMonthNow { get; set; }
        public DateTime? MonthNow { get; set; }
        public DateTime? FromMonth { get; set; }
        public DateTime? ToMonth { get; set; }
        public string RateAdditional { get; set; }
        public string NotCardHealth { get; set; }
        public double? RateBHXH { get; set; }
        public double? RateBHYT { get; set; }
        public double? RateBHTN { get; set; }

        public double? TotalSalalryCom { get; set; }
        public double? CountProfile { get; set; }
        public string Comment { get; set; }
        /// <summary> Trạng Thái (tăng Lao Động,Tăng Mức Đóng, Giảm Lao Động, ......) </summary>
        public string Status { get; set; }
        /// <summary> Trạng thái (Tăng ,Giảm ...) </summary>
        public string Status1 { get; set; }
        public string DateMonth { get; set; }
        public string STT { get; set; }
        public int ItemOrder { get; set; }
        /// <summary> Số thứ tự theo nhóm tăng,giảm... (I,II,I.1,II.1) </summary>
        public string OrderGroup { get; set; }
        public string BOLD { get; set; }
        public string CodeParentOrgLevel { get; set; }
        public double? Allowance1 { get; set; }
        public double? Allowance2 { get; set; }
        public double? Allowance3 { get; set; }
        public double? Allowance4 { get; set; }
        public double? AllowanceAdditional { get; set; }
        public string JobName{ get; set; }
        public Guid? WorkPlaceID { get; set; }
        public Guid? SocialInsPlaceID { get; set; }
        /// <summary>Tổng Số người đóng BHXH</summary>
        public int? SociaInsCountPro { get; set; }
        /// <summary>Tổng Lương đóng BHXH</summary>
        public double? SociaInsTotalSalary { get; set; }
        /// <summary>Tổng Số người đóng BHYT</summary>
        public int? HealthInsCountPro { get; set; }
        /// <summary>Tổng Lương đóng BHYT</summary>
        public double? HealthInsTotalSalary { get; set; }
        /// <summary>Tổng Số Người đóng BHTN</summary>
        public int? UnEmpInsCountPro { get; set; }
        /// <summary>Tổng Lương đóng BHTN</summary>
        public double? UnEmpInsTotalSalary { get; set; }
        /// <summary>Lương Cao Nhất</summary>
        public double? MaxSalary { get; set; }
        /// <summary>Lương Thấp Nhất</summary>
        public double? MinSalary { get; set; }
        public bool? IsPayBack { get; set; }
        public Guid? PayBackID { get; set; }
    }
}
