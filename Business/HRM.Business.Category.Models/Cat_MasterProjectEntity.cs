using System;

namespace HRM.Business.Category.Models
{
    public class Cat_MasterProjectEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string Type { get; set; }
        public string Note { get; set; }
    }
    //public class Cat_MasterProjectMultiEntity
    //{
    //    public Guid ID { get; set; }
    //    public string SourceAdsName { get; set; }
    //}


}
