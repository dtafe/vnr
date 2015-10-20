using HRM.Infrastructure.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.Laundry.Models
{
    public class Lau_ReportExceptClothingModel : BaseViewModel
    {
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_BranchCode)]
        [MaxLength(50)]
        public string BranchCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_BranchCode)]
        [MaxLength(50)]
        public string BranchName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        [MaxLength(50)]
        public string DepartmentCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_DepartmentCode)]
        [MaxLength(50)]
        public string DepartmentName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        [MaxLength(50)]
        public string TeamCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_TeamCode)]
        [MaxLength(50)]
        public string TeamName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        [MaxLength(50)]
        public string SectionCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        [MaxLength(50)]
        public string SectionName { get; set; }       
        [MaxLength(200)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineName)]
        public string LineName { get; set; }
        public Guid? LineID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineName)]
        public string LockerName { get; set; }
        public Guid? LockerID { get; set; }
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateTo)]
        public DateTime? DateTo { get; set; }
        // Số bộ giặt trong tháng
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_AmountClothing)]
        public int? AmountClothing { get; set; }
        // Tieu chuẩn giặt trong tháng
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_SumMonthAmount)]
        public int? SumMonthAmount { get; set; }
        // Số bộ vượt tiêu chuẩn
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_ExceedingStandards)]
        public int? ExceedingStandards { get; set; }
        // Tiền trừ vào lương
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_SubtractSalary)]
        public double? SubtractSalary { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_ShiftId)]
        public List<Guid?> ShiftID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_ShiftId)]
        public string ShiftName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Laundry_ReportEmpDetail_DateWash)]
        public DateTime? TimeLog { get; set; }

        public double? Data1 { get; set; }
        public double? Data2 { get; set; }
        public double? Data3 { get; set; }
        public double? Data4 { get; set; }
        public double? Data5 { get; set; }
        public double? Data6 { get; set; }
        public double? Data7 { get; set; }
        public double? Data8 { get; set; }
        public double? Data9 { get; set; }
        public double? Data10 { get; set; }
        public double? Data11 { get; set; }
        public double? Data12 { get; set; }
        public double? Data13 { get; set; }
        public double? Data14 { get; set; }
        public double? Data15 { get; set; }
        public double? Data16 { get; set; }
        public double? Data17 { get; set; }
        public double? Data18 { get; set; }
        public double? Data19 { get; set; }
        public double? Data20 { get; set; }
        public double? Data21 { get; set; }
        public double? Data22 { get; set; }
        public double? Data23 { get; set; }
        public double? Data24 { get; set; }
        public double? Data25 { get; set; }
        public double? Data26 { get; set; }
        public double? Data27 { get; set; }
        public double? Data28 { get; set; }
        public double? Data29 { get; set; }
        public double? Data30 { get; set; }
        public double? Data31 { get; set; }
        public partial class FieldNames
        {
            public const string TimeLog = "TimeLog";
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
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string AmountClothing = "AmountClothing";
            public const string SumMonthAmount = "SumMonthAmount";
            public const string ExceedingStandards = "ExceedingStandards";
            public const string SubtractSalary = "SubtractSalary";
            public const string ShiftID = "ShiftID";
            public const string ShiftName = "ShiftName";

            public const string Data1 = "Data1";
            public const string Data2 = "Data2";
            public const string Data3 = "Data3";
            public const string Data4 = "Data4";
            public const string Data5 = "Data5";
            public const string Data6 = "Data6";
            public const string Data7 = "Data7";
            public const string Data8 = "Data8";
            public const string Data9 = "Data9";
            public const string Data10 = "Data10";
            public const string Data11 = "Data11";
            public const string Data12 = "Data12";
            public const string Data13 = "Data13";
            public const string Data14 = "Data14";
            public const string Data15 = "Data15";
            public const string Data16 = "Data16";
            public const string Data17 = "Data17";
            public const string Data18 = "Data18";
            public const string Data19 = "Data19";
            public const string Data20 = "Data20";
            public const string Data21 = "Data21";
            public const string Data22 = "Data22";
            public const string Data23 = "Data23";
            public const string Data24 = "Data24";
            public const string Data25 = "Data25";
            public const string Data26 = "Data26";
            public const string Data27 = "Data27";
            public const string Data28 = "Data28";
            public const string Data29 = "Data29";
            public const string Data30 = "Data30";
            public const string Data31 = "Data31";
        }
    }

    public class Lau_ReportExceptClothingSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        public List<Guid?> ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_Line_LineName)]
        public List<Guid?> LineID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Laundry_Locker_LockerName)]
        public List<Guid?> LockerID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_ShiftId)]
        public List<Guid?> ShiftID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_OrgStructureId)]
        public List<Guid?> OrgStructureID { get; set; }
    }
}
