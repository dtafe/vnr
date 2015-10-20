using System;

namespace HRM.Business.Category.Models
{
    public class Cat_DistrictEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }
        public string Code { get; set; }
        public Guid? ProvinceID { get; set; }
        public string DistrictName { get; set; }
        public string Notes { get; set; }

        public string ProvinceName { get; set; }
    }
    public class Cat_DistrictMultiEntity
    {
        public Guid ID { get; set; }
        public string DistrictName { get; set; }
    }
}
