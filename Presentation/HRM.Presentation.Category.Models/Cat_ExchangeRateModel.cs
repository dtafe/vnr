using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_ExchangeRateModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_ExchangeRate_CurrencyBaseName)]
        public System.Guid CurrencyBaseID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ExchangeRate_CurrencyBaseName)]
        public string CurrencyBaseName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ExchangeRate_CurrencyDestName)]
        public System.Guid CurrencyDestID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ExchangeRate_CurrencyDestName)]
        public string CurrencyDestName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ExchangeRate_SellingRate)]
        public double SellingRate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ExchangeRate_BuyingRate)]
        public Nullable<double> BuyingRate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ExchangeRate_MonthOfEffect)]
        public System.DateTime MonthOfEffect { get; set; }
        public string Comment { get; set; }
        public string Type { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string CurrencyBaseName = "CurrencyBaseName";
            public const string CurrencyDestName = "CurrencyDestName";
            public const string SellingRate = "SellingRate";
            public const string BuyingRate = "BuyingRate";
            public const string MonthOfEffect = "MonthOfEffect";
        }
    }

    public class Cat_ExchangeRateSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_ExchangeRate_CurrencyBaseName)]
        public System.Guid? CurrencyBaseID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }


}
