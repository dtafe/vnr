using System;

namespace HRM.Business.Canteen.Models
{
    public class Can_LocationEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string LocationCode { get; set; }
        public string LocationName { get; set; }        
        public string Note { get; set; }
    }
   public class Can_LocationMultiEntity
   {
       public Guid ID { get; set; }
       public string LocationName { get; set; }    
   }
    
}
