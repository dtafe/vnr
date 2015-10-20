using System;

namespace HRM.Business.Category.Models
{
    public class Cat_ExchangeRateEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public System.Guid CurrencyBaseID { get; set; }
        public string CurrencyBaseCode { get; set; }
        public string CurrencyDestCode { get; set; }
        public string CurrencyBaseName { get; set; }
        public System.Guid CurrencyDestID { get; set; }
        public string CurrencyDestName { get; set; }
        public double SellingRate { get; set; }
        public Nullable<double> BuyingRate { get; set; }
        public System.DateTime MonthOfEffect { get; set; }
        public string Comment { get; set; }
        public string Type { get; set; }
    }
}
