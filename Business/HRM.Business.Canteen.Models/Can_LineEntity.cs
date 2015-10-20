using System;

namespace HRM.Business.Canteen.Models
{
    public class Can_LineEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Guid? CanteenID { get; set; }
        public string CanteenName { get; set; }
        public Guid? CateringID { get; set; }
        public string CateringName { get; set; }      
        public string LineCode { get; set; }       
        public string LineName { get; set; }

        public string MachineCode { get; set; }
       
        public double? Amount { get; set; }
        public string HDTJ { get; set; }
        public bool? Standard { get; set; }
        public bool? IsHDTJOB { get; set; }
        public DateTime? DateEffect { get; set; }
        public string Note { get; set; }        
    }
    public class Can_LineMultiEntity
    {
        public Guid ID { get; set; }
        public string LineName { get; set; }
    }
}
