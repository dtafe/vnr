using System;


namespace HRM.Business.Category.Models
{
    public class Cat_SkillEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string SkillName { get; set; }
        public string Note { get; set; }

    }

    public class Cat_SkillMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string SkillName { get; set; }
    }
}

