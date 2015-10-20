using HRM.Infrastructure.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Laundry.Models
{
    /// <summary> BC Tổng Hợp Trừ Tiền Giặt </summary>
    public class Lau_ReportSummaryExceptClothingModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_ProfileID)]
        public Guid? ProfileID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_ProfileID)]
        public string CodeEmp { get; set; }

        [MaxLength(150)]
        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_ProfileName)]
        public string ProfileName { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_LockerName)]
        public string LockerName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_LineName)]
        public string LineName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_ReportSummaryExceptClothing_Amount)]
        public int? SumMonthAmount { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_ReportSummaryExceptClothing_ExceedingStandards)]
        public int? ExceedingStandards { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_ReportSummaryExceptClothing_MonthStandards)]
        public int? StandardAmount { get; set; }
        
        [DisplayName(ConstantDisplay.HRM_Laundry_ReportSummaryExceptClothing_SubtractSalary)]
        public double? SubtractSalary { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_ReportSummaryExceptClothing_CodeBranch)]
        public string BranchCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_ReportSummaryExceptClothing_CodeOrg)]
        public string DepartmentCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_ReportSummaryExceptClothing_CodeTeam)]
        public string TeamCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_ReportSummaryExceptClothing_CodeSection)]
        public string SectionCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_ReportSummaryExceptClothing_BranchName)]
        public string BranchName { get; set; }

        public string DepartmentName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_ReportSummaryExceptClothing_TeamName)]
        public string TeamName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_ReportSummaryExceptClothing_SectionName)]
        public string SectionName { get; set; }
        
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string ProfileName = "ProfileName";
            public const string CodeEmp = "CodeEmp";
            public const string LockerName = "LockerName";
            public const string LineName = "LineName";
            public const string SumMonthAmount = "SumMonthAmount";
            public const string StandardAmount = "StandardAmount";
            public const string ExceedingStandards = "ExceedingStandards";
            public const string SubtractSalary = "SubtractSalary";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";

            public const string BranchCode = "BranchCode";
            public const string DepartmentCode = "DepartmentCode";
            public const string TeamCode = "TeamCode";
            public const string SectionCode = "SectionCode";
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
        }
    }

    public class Lau_ReportSummaryExceptClothingSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_ProfileName)]
        public string ProfileIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_OrgStructure_OrgStructureName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_ProfileID)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_LineName)]
        public Guid? LineID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_LaundryRecord_LockerName)]
        public Guid? LockerID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_ShiftID)]
        public Guid? ShiftID { get; set; }
        
        //public string selectedIds { get; set; }
        
        public string ValueFields { get; set; }
        
        public bool IsExport { get; set; }

        public int ExportID { get; set; }
    }
}
