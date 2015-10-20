using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class CatProvinceMultiModel
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string ProvinceName { get; set; }
    }
    public class CatProvinceModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Province_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Province_ProvinceName)]
        public string ProvinceName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Province_Notes)]
        public string Notes { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Province_CountryId)]
        public Guid? CountryID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Province_RegionId)]
        public Guid? RegionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Province_Order)]
        public int? Order { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Country_CountryName)]
        public string CountryName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Region_RegionName)]
        public string RegionName { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string ProvinceName = "ProvinceName";
            public const string Notes = "Notes";
            public const string CountryID = "CountryID";
            public const string RegionID = "RegionID";
            public const string Order = "Order";

            public const string CountryName = "CountryName";
            public const string RegionName = "RegionName";
        }
    }
    public class CatProvinceSearchModel {
        [DisplayName(ConstantDisplay.HRM_Category_Province_ProvinceName)]
        public string ProvinceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Province_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Province_CountryId)]
        public Guid? CountryID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Province_RegionId)]
        public Guid? RegionID { get; set; }

        public bool? IsExport { get; set; }

        public string ValueFields { get; set; }
    }
}
