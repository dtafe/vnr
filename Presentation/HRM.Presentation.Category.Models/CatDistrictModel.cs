using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class CatDistrictMultiModel
    {
        public Guid ID { get; set; }
        public string DistrictName { get; set; }
    }
    public class CatDistrictModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_District_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_District_ProvinceId)]
        public Guid? ProvinceID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_District_DistrictName)]
        public string DistrictName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_District_Notes)]
        public string Notes { get; set; }

        public string ProvinceName { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string ProvinceID = "ProvinceID";
            public const string DistrictName = "DistrictName";
            public const string Notes = "Notes";
            public const string ProvinceName = "ProvinceName";
        }

    }

    public class CatDistrictSearchModel {

        [DisplayName(ConstantDisplay.HRM_Category_District_DistrictName)]
        public string DistrictName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_District_Code)]
        public string DistrictCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_District_ProvinceId)]
        public Guid? ProvinceID { get; set; }

        public bool? IsExport { get; set; }

        public string ValueFields { get; set; }
    }
}
