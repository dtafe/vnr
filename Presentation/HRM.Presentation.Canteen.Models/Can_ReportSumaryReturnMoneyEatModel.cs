using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.Canteen.Models
{
    public class Can_ReportSumaryReturnMoneyEatModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_CodeEmp)]
        [MaxLength(50)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        [MaxLength(150)]
        public string ProfileName { get; set; }

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
   
        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_CateringName)]
        public string CateringName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_CateringName)]
        public List<Guid?> CateringIDs { get; set; }

        [MaxLength(200)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_CanteenName)]
        public string CanteenName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_CanteenName)]
        public List<Guid?> CanteenIDs { get; set; }

        [MaxLength(200)]
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineName)]
        public string LineName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineName)]
        public List<Guid?> LineIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public List<Guid> workPlaceID { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Canteen_Report_IsIncludeQuitEmp)]
        public Boolean IsIncludeQuitEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_UserPrint)]
        public string UserPrint { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DatePrint)]
        public DateTime? DatePrint { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_CountCardMore)]
        public Double? CountCardMore { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_SumAmountCardMore)]
        public Double? SumAmountCardMore { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_CountNotWorkButHasEat)]
        public Double? CountNotWorkButHasEat { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_SumNotWorkButHasEat)]
        public Double? SumNotWorkButHasEat { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_SumAmountMustSubtract)]
        public Double? SumAmountMustSubtract { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_MoneyMustSubtractHDT)]
        public Double? MoneyMustSubtractHDT { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_SumMoneyMustSubtract)]
        public Double? SumMoneyMustSubtract { get; set; }

        public partial class FieldNames
        {
            public const string UserPrint = "UserPrint";
            public const string DatePrint = "DatePrint";
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
            public const string CateringName = "CateringName";
            public const string CateringID = "CateringID";
            public const string CanteenName = "CanteenName";
            public const string CanteenID = "CanteenID";
            public const string LineID = "LineID";
            public const string LineName = "LineName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string CountCardMore = "CountCardMore";
            public const string SumAmountCardMore = "SumAmountCardMore";
            public const string CountNotWorkButHasEat = "CountNotWorkButHasEat";
            public const string SumNotWorkButHasEat = "SumNotWorkButHasEat";
            public const string SumAmountMustSubtract = "SumAmountMustSubtract";
            public const string MoneyMustSubtractHDT = "MoneyMustSubtractHDT";
            public const string SumMoneyMustSubtract = "SumMoneyMustSubtract";
        }
    }

    public class Can_ReportSumaryReturnMoneyEatSearchModel 
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_ProfileName)]
        public List<Guid?> ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Catering_CateringName)]
        public List<Guid?> CateringIDs { get; set; }       
        [DisplayName(ConstantDisplay.HRM_Canteen_Canteen_CanteenName)]
        public List<Guid?> CanteenIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Line_LineName)]
        public List<Guid?> LineIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public List<Guid?> WorkPlaceID { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Canteen_Report_IsIncludeQuitEmp)]
        public Boolean IsIncludeQuitEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_OrgStructureId)]
        public List<Guid?> OrgStructureID { get; set; }
        public Guid ExportID { get; set; }
    }
}
