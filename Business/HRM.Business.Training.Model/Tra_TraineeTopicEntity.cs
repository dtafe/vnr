using System;


namespace HRM.Business.Training.Models
{
    public class Tra_TraineeTopicEntity : HRM.Business.BaseModel.HRMBaseModel
    {
  
        public Guid? TraineeID { get; set; }
        
        public Guid? TopicID { get; set; }
        public string Code { get; set; }
        public double? Score { get; set; }
       
    }

   

}

