using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
namespace HRM.Presentation.Category.Models
{
    public class CatCountryMultiModel
    {
        public Guid ID { get; set; }
        public string CountryName { get; set; }
    }
    public class CatCountryModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Country_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Country_CountryName)]
        public string CountryName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Country_CountryNameES)]
        public string CountryNameES { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Country_Notes)]
        public string Notes { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string CountryName = "CountryName";
            public const string CountryNameES = "CountryNameES";
            public const string Notes = "Notes";
        }
    }
    public class CatCountrySearchModel {      
        [DisplayName(ConstantDisplay.HRM_Category_Country_CountryName)]
        public string CountryName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Country_CountryNameES)]
        public string CountryNameES { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Country_Code)]
        public string CountryCode { get; set; }

        public bool? IsExport { get; set; }

        public string ValueFields { get; set; }
    }
}
