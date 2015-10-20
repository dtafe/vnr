using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class CatCurrencyMultiModel {
        public Guid ID { get; set; }
        public string CurrencyName { get; set; }
    }
    public class CatCurrencyModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Currency_CurrencyName)]
        public string CurrencyName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Currency_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Currency_Description)]
        public string Description { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Category_Currency_ServerID)]
        public Guid? ServerID { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string CurrencyName = "CurrencyName";
            public const string Code = "Code";
            public const string Description = "Description";
        }
    }
    public class CatCurrencySearchModel {
        [DisplayName(ConstantDisplay.HRM_Category_Currency_CurrencyName)]
        public string CurrencyName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Currency_Code)]
        public string CurrencyCode { get; set; }

        public bool? IsExport { get; set; }

        public string ValueFields { get; set; }
    }

}
