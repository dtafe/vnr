using System;

namespace HRM.Business.Laundry.Models
{
    public class Lau_MachineOfLineEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }


        public string Code { get; set; }

        public Guid? LineID { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string LineName { get; set; }
     
        public string Notes { get; set; }

    }
    public class Lau_MachineOfLineMultiEntity
    {
        public Guid ID { get; set; }
        public string MachineOfLineMachineCode { get; set; }
    }
    
}
