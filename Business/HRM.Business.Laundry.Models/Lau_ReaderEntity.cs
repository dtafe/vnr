using System;

namespace HRM.Business.Laundry.Models
{
    public class Lau_ReaderEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }

        public string ReaderCode { get; set; }

        public string ReaderName { get; set; }
        
        public string Notes { get; set; }

        public Guid LockerId { get; set; }
        public string LockerName { get; set; }
    }

   public class Lau_ReaderMultiEntity
   {
       public int ID { get; set; }
       public string ReaderName { get; set; }    
   }
    
}
