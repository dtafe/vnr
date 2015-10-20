using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class CatProductModel:BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Product_ProductName)]
        public string ProductName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Product_Unit)]
        public string Unit { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Product_CurrencyID)]
        public Guid? CurrencyID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Product_CurrencyName)]
        public string CurrencyName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Product_BonusPerUnit)]
        public double BonusPerUnit { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Product_EffectiveDate)]
        public Nullable<System.DateTime> EffectiveDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Product_ProductTypeID)]
        public Nullable<System.Guid> ProductTypeID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Product_ProductTypeName)]
        public string ProductTypeName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Product_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Product_ProductionTime)]

        public Nullable<double> ProductionTime { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Product_Note)]
        public string Note { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProductName = "ProductName";
            public const string Unit = "Unit";
            public const string CurrencyID = "CurrencyID";
            public const string CurrencyName = "CurrencyName";
            public const string BonusPerUnit = "BonusPerUnit";
            public const string EffectiveDate = "EffectiveDate";
            public const string ProductTypeID = "ProductTypeID";
            public const string ProductTypeName = "ProductTypeName";
            public const string Code = "Code";
            public const string ProductionTime = "ProductionTime";
            public const string Note = "Note";
        }
    }

    public class CatProductSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Product_ProductName)]
        public string ProductName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Product_CurrencyID)]
      
        public Guid? CurrencyID { get; set; }

        

        [DisplayName(ConstantDisplay.HRM_Category_Product_EffectiveDateFrom)]
        public System.DateTime? EffectiveDateFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Product_EffectiveDateTo)]
        public System.DateTime? EffectiveDateTo { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_ProductMultiModel 
    {
        public Guid ID { get; set; }
        public string ProductName { get; set; }
    }


}
