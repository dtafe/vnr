using System;

namespace HRM.Business.Category.Models
{
    public class Cat_SourceAdsEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string SourceAdsName { get; set; }
        public string Description { get; set; }
    }
    public class Cat_SourceAdsMultiEntity
    {
        public Guid ID { get; set; }
        public string SourceAdsName { get; set; }
    }


}
