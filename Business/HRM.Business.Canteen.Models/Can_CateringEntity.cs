using System;

namespace HRM.Business.Canteen.Models
{
    public class Can_CateringEntity : HRM.Business.BaseModel.HRMBaseModel
    {       
        public string CateringCode { get; set; }       
        public string CateringName { get; set; }        
        public string Note { get; set; }
    }
    public class Can_CateringMultiEntity
    {
        public Guid ID { get; set; }
        public string CateringName { get; set; }
    }

}
