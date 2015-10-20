using System;


namespace HRM.Business.Category.Models
{
    public class Cat_WorkPlaceEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string WorkPlaceName { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Nullable<System.Guid> ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public Guid? RegionID { get; set; }
        public Nullable<System.Guid> DistrictID { get; set; }
        public string DistrictName { get; set; }
        public string Address { get; set; }
        public Nullable<int> OrderNumber { get; set; }
    }
    public class Cat_WorkPlaceMultiEntity
    {
        public Guid ID { get; set; }
        public string WorkPlaceName { get; set; }
    }
}

