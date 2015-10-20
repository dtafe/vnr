using System;


namespace HRM.Business.Category.Models
{
    public class Cat_YouthUnionPositionEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string YouthUnionPositionName { get; set; }
        public string Notes { get; set; }
    }
    public class Cat_YouthUnionPositionMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string YouthUnionPositionName { get; set; }
    }
}

