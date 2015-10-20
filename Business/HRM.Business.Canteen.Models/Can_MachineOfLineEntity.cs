using System;

namespace HRM.Business.Canteen.Models
{
    public class Can_MachineOfLineEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string MachineCode { get; set; }
        public Guid? LineID { get; set; }
        public string LineName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }        
        public string Note { get; set; }
    }

    public class Can_MachineOfLineMultiEntity 
    {
        public Guid ID { get; set; }
        public string MachineCode { get; set; }
    }
}
