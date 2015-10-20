using System;


namespace HRM.Business.Category.Models
{
    public class Cat_BankEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string Notes { get; set; }
        public string CompBankCode { get; set; }
    }

    public class Cat_BankMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
    }

}

