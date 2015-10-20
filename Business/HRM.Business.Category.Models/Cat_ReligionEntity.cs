using System;


namespace HRM.Business.Category.Models
{
    public class Cat_ReligionEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string ReligionName { get; set; }
        public string Code { get; set; }
    }

    public class Cat_ReligionMultiEntity
    {
        public Guid ID { get; set; }
        public string ReligionName { get; set; }
    }
}

