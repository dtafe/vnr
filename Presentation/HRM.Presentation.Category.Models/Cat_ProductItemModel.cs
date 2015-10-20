using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Category.Models
{
    public class Cat_ProductItemModel : BaseViewModel
    {
        public string ProductItemCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Item)]
        public Nullable<System.Guid> ProductID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ProductItem_ProductItemName)]
        public string ProductItemName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ProductItem_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ProductItem_UnitPrice)]
        public Nullable<double> UnitPrice { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ProductItem_BonusPrice)]
        public Nullable<double> BonusPrice { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ProductItem_Note1)]
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public string Note3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ProductItem_RateStage)]
        public Nullable<double> RateStage { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ProductItem_ProductionTime)]
        public Nullable<double> ProductionTime { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ProductItem_CurrencyName)]
        public Nullable<System.Guid> CurrencyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ProductItem_ProductName)]
        public string ProductName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ProductItem_CurrencyName)]
        public string CurrencyName { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProductName = "ProductName";
            public const string UnitPrice = "UnitPrice";
            public const string CurrencyID = "CurrencyID";
            public const string CurrencyName = "CurrencyName";
            public const string BonusPrice = "BonusPrice";
            public const string ProductItemName = "ProductItemName";
            public const string Code = "Code";
            public const string ProductItemCode = "ProductItemCode";
            public const string ProductionTime = "ProductionTime";
            public const string Note1 = "Note1";
            public const string ProductID = "ProductID";
            public const string RateStage = "RateStage";
        }
    }

    public class Cat_ProductItemSearchModel {
        [DisplayName(ConstantDisplay.HRM_Category_ProductItem_ProductItemName)]
        public string ProductItemName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ProductItem_Code)]
        public string Code { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_ProductItemMultiModel
    {
        public Guid ID { get; set; }
        public string ProductItemName { get; set; }
    }

   
}
