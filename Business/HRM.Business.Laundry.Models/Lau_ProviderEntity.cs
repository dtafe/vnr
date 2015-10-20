using System;

namespace HRM.Business.Laundry.Models
{
    public class Lau_ProviderEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }

        public string ProviderCode { get; set; }
       
        public string LaundryName { get; set; }
      
        public string Notes { get; set; }
    }

    public class Lau_ProviderMultiEntity
   {
       public int ID { get; set; }
       public string LaundryName { get; set; }    
   }
    
}
