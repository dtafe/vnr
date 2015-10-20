using System;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_TagEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string TagName { get; set; }
        public string EntityType { get; set; }
    }

    public class Rec_TagMultiEntity
    {
        public Guid ID { get; set; }
        public string TagName { get; set; }
    }
}
