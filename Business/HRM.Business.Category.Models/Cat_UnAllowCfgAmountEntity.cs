using System;

namespace HRM.Business.Category.Models
{
    public class Cat_UnAllowCfgAmountEntity : HRM.Business.BaseModel.HRMBaseModel
    {
      
        public Nullable<System.Guid> UnusualAllowanceID { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<double> Amount { get; set; }
        public string UnusualAllowanceCfgName { get; set; }
     
    }


}
