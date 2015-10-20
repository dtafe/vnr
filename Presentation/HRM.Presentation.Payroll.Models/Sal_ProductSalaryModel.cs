using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Payroll.Models
{
    public class Sal_ProductSalaryModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public Nullable<System.Guid> ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Product_ProductName)]
        public Nullable<System.Guid> ProductID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ProductItem_ProductItemName)]
        public Nullable<System.Guid> ProductItemID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_AnnualDetail_MonthYear)]
        public Nullable<System.DateTime> MonthYear { get; set; }
        public Nullable<double> QtyPrevious { get; set; }
        public Nullable<double> QtyActual { get; set; }
        public Nullable<double> QtySalary { get; set; }
        public Nullable<double> QtyNext { get; set; }
        [DisplayName(ConstantDisplay.FIN_ClaimItem_Amount)]
        public Nullable<double> Amount { get; set; }
        [DisplayName(ConstantDisplay.FIN_ClaimItem_CurrencyName)]
        public Nullable<System.Guid> CurrencyID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Product_ProductName)]
        public string ProductName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ProductItem_ProductItemName)]
        public string ProductItemName { get; set; }
        [DisplayName(ConstantDisplay.FIN_ClaimItem_CurrencyName)]
        public string CurrencyName { get; set; }

        public partial class FieldNames
        {
            public const string ProfileName = "ProfileName";
            public const string ProductName = "ProductName";
            public const string MonthYear = "MonthYear";
            public const string CurrencyName = "CurrencyName";
            public const string ProductItemName = "ProductItemName";
            public const string QtyPrevious = "QtyPrevious";
            public const string QtyActual = "QtyActual";
            public const string QtySalary = "QtySalary";
            public const string QtyNext = "QtyNext";
            public const string Amount = "Amount";
        }
    }


    public class Sal_ProductSalarySearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructure { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Product_ProductName)]
        public Guid? ProductID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ProductItem_ProductItemName)]
        public Guid? ProductItemID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Common_Search_Month)]
        public DateTime? MonthStart { get; set; }
        public DateTime? MonthEnd { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
