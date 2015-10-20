using System;


namespace HRM.Business.Category.Models
{
    public class Cat_SelfDefenceMilitiaPositionEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string SelfDefenceMilitiaPositionName { get; set; }
        public string Notes { get; set; }
    }
    public class Cat_SelfDefenceMilitiaPositionMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string SelfDefenceMilitiaPositionName { get; set; }
    }
}

