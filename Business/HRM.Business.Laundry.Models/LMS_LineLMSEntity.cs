using System;

namespace HRM.Business.Laundry.Models
{
    public class LMS_LineLMSEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }

        public string Code { get; set; }

        public string LineLMSName { get; set; }
        
        public string Note { get; set; }

        public Guid? MarkerID { get; set; }

        public Guid? LockerID { get; set; }

        public string MachineCode { get; set; }

        public string LockerLMSName { get; set; }

        public string MarkerName { get; set; }

        public double? Amount { get; set; }

        public DateTime? DateEffect { get; set; }

    }

    public class Lau_LineMultiEntity
   {
        public Guid ID { get; set; }
        public string LineLMSName { get; set; }    
   }
    
}
