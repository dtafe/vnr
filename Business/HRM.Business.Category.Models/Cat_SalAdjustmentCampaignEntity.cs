using HRM.Business.BaseModel;
using System;

namespace HRM.Business.Category.Models
{
    public class Cat_SalAdjustmentCampaignMultiEntity 
    {
        public Guid ID { get; set; }
        public string SalAdjustmentCampaignName { get; set; }
        public string Code { get; set; }
    }
    public class Cat_SalAdjustmentCampaignEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
      
    }
}
