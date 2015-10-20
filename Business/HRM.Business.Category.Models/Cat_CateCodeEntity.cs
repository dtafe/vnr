using System;

namespace HRM.Business.Category.Models
{
    public class Cat_CateCodeEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string CateCodeType { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
    }
    //public class Cat_SourceAdsMultiEntity
    //{
    //    public Guid ID { get; set; }
    //    public string SourceAdsName { get; set; }
    //}


}
