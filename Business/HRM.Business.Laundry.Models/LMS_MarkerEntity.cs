using System;

namespace HRM.Business.Laundry.Models
{
    public class LMS_MarkerEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        
        public string MarkerName { get; set; }
        
        public string Note { get; set; }

    }

    public class Lau_MarkerMultiEntity
   {
        public Guid ID { get; set; }
       public string MarkerName { get; set; }    
   }
    
}
