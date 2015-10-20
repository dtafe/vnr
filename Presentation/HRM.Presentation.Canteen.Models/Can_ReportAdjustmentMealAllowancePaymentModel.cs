using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;

namespace HRM.Presentation.Canteen.Models
{
    public class Can_ReportAdjustmentMealAllowancePaymentModel : BaseViewModel
    {
        public Guid ExportID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_ProfileName)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_OrgName)]
        public string OrgName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_BranchName)]
        public string BranchName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DepartmentName)]
        public string DepartmentName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_TeamName)]
        public string TeamName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Attendance_SectionCode)]
        public string SectionName { get; set; } 
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DateTo)]
        public DateTime? DateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_UserPrint)]
        public string UserPrint { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_DatePrint)]
        public DateTime? DatePrint { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_OrgStructureId)]
        public List<Guid?> OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_OrgStructureId)]
        public string OrgStructureName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_CountCard)]
        public int? CountCard { get; set; }//Tong lan quet the

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_SumAmount)]
        public double? SumAmount { get; set; }// tong so tien ho tro

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_CountEatNotStandar)]
        public int? CountEatNotStandar { get; set; }// so bua an ko dat chuan

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_AmountEatNotStandar)]
        public double? AmountEatNotStandar { get; set; }// so tien an o ko dat chuan

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_CountForgetMore)]
        public int? CountForgetMore { get; set; }// tong so lan quen quet the nhieu lan

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_AmountForgetMore)]
        public double? AmountForgetMore { get; set; }// tong so tien quen quet the

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_AmountSubtractCardMore)]
        public double? AmountSubtractCardMore { get; set; }// so tien tru do quet the nhieu lan

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_CountNotWorkHasEat)]
        public int? CountNotWorkHasEat { get; set; }// so lan ko quet the nhung co an

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_CountHDTJob)]
        public int? CountHDTJob { get; set; }// so lan quet the co HDTJOb

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_AmountSubtractHDTJob)]
        public double? AmounSubtractHDTJob { get; set; }// tong so lan do quet the nhieu lan

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_AmountNotWorkHasEat)]
        public double? AmountNotWorkHasEat { get; set; } // so tien trừ không đi làm nhưng có ăn

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_CountCardMore)]
        public int? CountCardMore { get; set; }// tong so lan do quet the nhieu lan

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_SumCardMore)]
        public double? SumCardMore { get; set; }// tong tiền trừ do quet the nhieu lan

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_CountCardWorngStandar)]
        public int? CountCardWorngStandar { get; set; }// so lan quet the sai tieu chuan

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_AmountSubtractWorngStandar)]
        public double? AmountSubtractWorngStandar { get; set; }// so tien tru the sai tieu chuan

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_Amount3OnMonth)]
        public double? Amount3OnMonth { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_TotalMealAllowance)]
        public double? TotalMealAllowance { get; set; }
        public partial class FieldNames
        {
            public const string SectionName = "SectionName";
            public const string TotalMealAllowance = "TotalMealAllowance";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgName = "OrgName";
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string UserPrint = "UserPrint";
            public const string DatePrint = "DatePrint";
            public const string OrgStructureID = "OrgStructureID";
            public const string CountCard = "CountCard";
            public const string SumAmount = "SumAmount";
            public const string CountEatNotStandar = "CountEatNotStandar";
            public const string AmountEatNotStandar = "AmountEatNotStandar";
            public const string CountForgetMore = "CountForgetMore";
            public const string AmountForgetMore = "AmountForgetMore";
            public const string AmountSubtractCardMore = "AmountSubtractCardMore";
            public const string CountNotWorkHasEat = "CountNotWorkHasEat";
            public const string CountHDTJob = "CountHDTJob";
            public const string AmounSubtractHDTJob = "AmounSubtractHDTJob";
            public const string CountNotWorkButHasEat = "CountNotWorkButHasEat";
            public const string CountCardMore = "CountCardMore";
            public const string CountCardWorngStandar = "CountCardWorngStandar";
            public const string AmountSubtractWorngStandar = "AmountSubtractWorngStandar";
            public const string SumCardMore = "SumCardMore";
            public const string AmountNotWorkHasEat = "AmountNotWorkHasEat";
            public const string Amount3OnMonth = "Amount3OnMonth";

        }
    }
}
