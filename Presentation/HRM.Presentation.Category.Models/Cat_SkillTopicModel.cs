using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_SkillTopicModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Cat_SkillTopic_SkillID)]
        public Nullable<System.Guid> SkillID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_SkillTopic_SkillID)]
        public string SkillName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_SkillTopic_TopicID)]
        public Nullable<System.Guid> TopicID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_SkillTopic_TopicID)]
        public string TopicName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_SkillTopic_SkillTopicName)]
        public string SkillTopicName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_SkillTopic_Fomular)]
        public string Fomular { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_SkillTopic_Note)]
        public string Note { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string SkillName = "SkillName";
            public const string TopicName = "TopicName";
            public const string SkillTopicName = "SkillTopicName";
            public const string Fomular = "Fomular";
            public const string Note = "Note";

        }
    }
    public class Cat_SkillTopicSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Cat_SkillTopic_SkillTopicName)]
        public string SkillTopicName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

}
