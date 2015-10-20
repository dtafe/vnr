using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class CatWorkPlaceModel : BaseViewModel
    {
        public Nullable<int> OrderNumber { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_WorkPlaceName)]
        public string WorkPlaceName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_Description)]
        public string Description { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_ProvinceID)]
        public Nullable<System.Guid> ProvinceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_ProvinceID)]
        public string ProvinceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_DistrictID)]
        public Nullable<System.Guid> DistrictID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_DistrictID)]
        public string DistrictName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_Address)]
        public string Address { get; set; }

        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_RegionName)]
        public string RegionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_RegionID)]
        public Guid? RegionID { get; set; }
        public partial class FieldNames
        {
            public const string WorkPlaceName = "WorkPlaceName";
            public const string Code = "Code";
            public const string Description = "Description";
            public const string ProvinceName = "ProvinceName";
            public const string DistrictName = "DistrictName";
            public const string Address = "Address";
          
        }
    }
    public class CatWorkPlaceSearchModel {
        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_WorkPlaceName)]
        public string WorkPlaceName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class CatWorkPlaceMultihModel {
        public Guid ID { get; set; }
        public string WorkPlaceName { get; set; }
        public int? OrderNumber { get; set; }
    }
}
