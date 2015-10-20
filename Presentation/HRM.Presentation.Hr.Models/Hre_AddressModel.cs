using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using HRM.Presentation.Service;

namespace HRM.Presentation.Hr.Models
{
    public class Hre_AddressModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Address_AddressName)]
        [MaxLength(150)]
        public string AddressName { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Address_Code)]
        [MaxLength(50)]
        public string AddressCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Address_CountryID)]
        public Guid? CountryID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Address_ProvinceID)]
        public Guid? ProvinceID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Address_DistrictID)]
        public Guid? DistrictID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Country_CountryName)]
        [MaxLength(150)]
        public string CountryName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_Province_ProvinceName)]
        [MaxLength(150)]
        public string ProvinceName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_District_DistrictName)]
        [MaxLength(150)]
        public string DistrictName { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string AddressCode = "AddressCode";
            public const string AddressName = "AddressName";
            public const string CountryID = "CountryID";
            public const string CountryName = "CountryName";
            public const string ProvinceID = "ProvinceID";
            public const string ProvinceName = "ProvinceName";
            public const string DistrictID = "DistrictID";
            public const string DistrictName = "DistrictName";
        }
    }
    public class Hre_AddressSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Address_CountryID)]
        public Guid? CountryID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Address_ProvinceID)]
        public Guid? ProvinceID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Address_DistrictID)]
        public Guid? DistrictID { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string CountryID = "CountryID";
            public const string ProvinceID = "ProvinceID";
            public const string DistrictID = "DistrictID";
        }
    }
}
