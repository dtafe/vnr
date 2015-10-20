using System;


namespace HRM.Business.Category.Models
{
    public class Cat_UsualAllowanceLevelEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public System.Guid AllowanceID { get; set; }
        public string UsualAllowanceName { get; set; }
        public string UsualAllowanceLevelName { get; set; }
        public double Amount { get; set; }
        public System.Guid CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string Comment { get; set; }
        public string Code { get; set; }
        public System.DateTime? MonthApply { get; set; }
    }

    
    
}

