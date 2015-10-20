using System;

namespace HRM.Business.Laundry.Models
{
    public class LMS_LockerLMSEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }

        public string Code { get; set; }
        
        public string LockerLMSName { get; set; }
        
        public string Note { get; set; }

    }

    public class Lau_LockerMultiEntity
   {
        public Guid ID { get; set; }
       public string LockerLMSName { get; set; }    
   }
    
}
