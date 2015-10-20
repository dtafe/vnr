using System;

namespace HRM.Business.Training.Models
{
    public class Tra_ScoreTopicEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Guid? ScoreTypeID { get; set; }
        public string ScoreTypeName { get; set; }
        public Guid? TopicID { get; set; }
        public string TopicName { get; set; }
        public bool? IsComplex { get; set; }
        public string Code { get; set; }
        public string ScoreTypeCode { get; set; }
    }
}

