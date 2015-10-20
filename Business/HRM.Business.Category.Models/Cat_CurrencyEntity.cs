using System;

namespace HRM.Business.Category.Models
{
    public class Cat_CurrencyMultiEntity 
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string CurrencyName { get; set; }
    }
    public class Cat_CurrencyEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string CurrencyName { get; set; }
        public string Description { get; set; }
        public Guid? ServerID { get; set; }
    }
}
