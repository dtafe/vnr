using System;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_GroupConditionEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string GroupName { get; set; }
        public Nullable<int> LevelInterview { get; set; }
        public string JobConditionIDs { get; set; }
    }

    public class Rec_GroupConditionMultiEntity
    {
        public Guid ID { get; set; }
        public string GroupName { get; set; }
    }
}
