using System;


namespace HRM.Business.Category.Models
{
    public class Cat_VeteranUnionPositionEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string VeteranUnionPositionName { get; set; }
        public string Notes { get; set; }
    }
    public class Cat_VeteranUnionPositionMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string VeteranUnionPositionName { get; set; }
    }
}

