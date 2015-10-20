using HRM.Infrastructure.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.Laundry.Models
{
    public class Lau_ReportSummaryNumberWashModel : BaseViewModel
    {
        [MaxLength(150)]
        [DisplayName(ConstantDisplay.HRM_Laundry_Marker_MarkerName)]
        public string MarkerName { get; set; }
        public Guid? MarkerID { get; set; }
        [MaxLength(200)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineName)]
        public string LineName { get; set; }
        public Guid? LineID { get; set; }
        [MaxLength(150)]
        [DisplayName(ConstantDisplay.HRM_Laundry_Locker_LockerName)]
        public string LockerName { get; set; }
        public Guid? LockerID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Laundry_LaundryPrice)]     
        public double? Price { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_ReportSummaryNumberWash_Volumne)]
        public int? Volumne { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_ReportSummaryNumberWash_TotalAmount)]
        public double? TotalAmount { get; set; }
        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string BranchCode = "BranchCode";
            public const string BranchName = "BranchName";
            public const string DepartmentCode = "DepartmentCode";
            public const string DepartmentName = "DepartmentName";
            public const string TeamCode = "TeamCode";
            public const string TeamName = "TeamName";
            public const string SectionCode = "SectionCode";
            public const string SectionName = "SectionName";         
            public const string LineID = "LineID";
            public const string LineName = "LineName";
            public const string LockerID = "LockerID";
            public const string LockerName = "LockerName";
            public const string Price = "Price";
            public const string Volumne = "Volumne";
            public const string TotalAmount = "TotalAmount";
        }
    }

    public class Lau_ReportSummaryNumberWashSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        public List<Guid?> ProfileID { get; set; }       
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineName)]
        public List<Guid?> LineID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineName)]
        public List<Guid?> LockerID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_OrgStructureId)]
        public List<Guid?> OrgStructureID { get; set; }
    }
}
