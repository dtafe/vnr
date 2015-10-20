using System;


namespace HRM.Business.Category.Models
{
    public class Cat_TradeUnionistPositionEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string TradeUnionistPositionName { get; set; }
        public string Notes { get; set; }
    }
    public class Cat_TradeUnionistPositionMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string TradeUnionistPositionName { get; set; }
    }
}

