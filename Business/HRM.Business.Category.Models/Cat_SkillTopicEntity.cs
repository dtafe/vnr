using System;


namespace HRM.Business.Category.Models
{
    public class Cat_SkillTopicEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Nullable<System.Guid> SkillID { get; set; }
        public string SkillName { get; set; }
        public Nullable<System.Guid> TopicID { get; set; }
        public string TopicName { get; set; }
        public string SkillTopicName { get; set; }
        public string Fomular { get; set; }
        public string Note { get; set; }
       
    }

}

