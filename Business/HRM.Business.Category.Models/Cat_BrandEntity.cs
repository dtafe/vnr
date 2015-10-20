using System;

namespace HRM.Business.Category.Models
{
    public class Cat_BrandEntity : HRM.Business.BaseModel.HRMBaseModel
    {
     
        public string BrandName { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
    }

    public class Cat_BrandMultiEntity
    {
        public Guid ID { get; set; }
        public string BrandName { get; set; }
     
    }
}
