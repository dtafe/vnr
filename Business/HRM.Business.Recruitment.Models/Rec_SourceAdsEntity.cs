using System;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_SourceAdsEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string SourceAdsName { get; set; }
        public string Notes { get; set; }
    }
    public class Rec_SourceAdsMultiEntity
    {
        public Guid ID { get; set; }
        public string SourceAdsName { get; set; }
    }


}
