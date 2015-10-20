using System;


namespace HRM.Business.Training.Models
{
    public class Tra_CourseTopicEntity : HRM.Business.BaseModel.HRMBaseModel
    {
  
        public Guid CourseID { get; set; }
        
        public Guid TopicID { get; set; }
        public string TopicName { get; set; }
        public bool? IsComplex { get; set; }
        public string Code { get; set; }
        
        public class Tra_CourseTopicMultiByCourseIDEntity
        {
            public int TotalRow { get; set; }
            public Guid ID { get; set; }
            public string TopicName { get; set; }
        }
       
    }

   

}

