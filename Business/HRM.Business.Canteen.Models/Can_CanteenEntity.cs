using System;

namespace HRM.Business.Canteen.Models
{
    public class Can_CanteenEntity : HRM.Business.BaseModel.HRMBaseModel
    { 
        public string CanteenCode { get; set; }      
        public string CanteenName { get; set; }
        public Guid? LocationID { get; set; }
        public string LocationName { get; set; }      
        public string Notes { get; set; }
    }
    public class Can_CanteenMultiEntity
    {
        public Guid ID { get; set; }
        public string CanteenName { get; set; }       
    }

}
