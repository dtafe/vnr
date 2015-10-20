using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.ComponentModel;
namespace HRM.Presentation.Category.Models
{
    public class Cat_VillageModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Village_VillageName)]
        public string VillageName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Village_ProvinceID)]
        public Nullable<System.Guid> ProvinceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Village_ProvinceID)]
        public string ProvinceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Village_Notes)]
        public string Notes { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Village_DistrictID)]
        public Nullable<System.Guid> DistrictID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Village_DistrictID)]
        public string DistrictName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Village_Code)]
        public string Code { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string VillageName = "VillageName";
            public const string ProvinceName = "ProvinceName";
            public const string Notes = "Notes";
            public const string DistrictName = "DistrictName";
            public const string Code = "Code";
        }
    }
    public class Cat_VillageSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Village_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Village_VillageName)]
        public string VillageName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Village_ProvinceID)]
        public Nullable<System.Guid> ProvinceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Village_DistrictID)]
        public Nullable<System.Guid> DistrictID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_VillageMultiModel
    {
        public Guid ID { get; set; }
        public string VillageName { get; set; }
        public int TotalRow { get; set; }
    }
}
