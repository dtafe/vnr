using System;
using System.Collections.Generic;

namespace HRM.Business.Canteen.Models
{
    public class Can_ReportSumaryReturnMoneyEatEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string BranchName { get; set; }     
        public string DepartmentName { get; set; }    
        public string TeamName { get; set; }      
        public string SectionName { get; set; }
        public string CateringName { get; set; }
        public List<Guid?> CateringID { get; set; }
        public string CanteenName { get; set; }
        public List<Guid?> CanteenID { get; set; }
        public string LineName { get; set; }
        public List<Guid?> LineID { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string UserPrint { get; set; }
        public DateTime? DatePrint { get; set; }
        public Double? CountCardMore { get; set; }
        public Double? SumAmountCardMore { get; set; }
        public Double? CountNotWorkButHasEat { get; set; }
        public Double? SumNotWorkButHasEat { get; set; }
        public Double? SumAmountMustSubtract { get; set; }
        public Double? MoneyMustSubtractHDT { get; set; }
        public Double? SumMoneyMustSubtract { get; set; }
        public partial class FieldNames
        {
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string BranchName = "BranchName";
            public const string DepartmentName = "DepartmentName";
            public const string TeamName = "TeamName";
            public const string SectionName = "SectionName";
            public const string CateringName = "CateringName";
            public const string CanteenName = "CanteenName";
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
            public const string CountWorkDay = "CountWorkDay";
            public const string CountSandard = "CountSandard";
            public const string UserPrint = "UserPrint";
            public const string DatePrint = "DatePrint";
        }
    }
}
