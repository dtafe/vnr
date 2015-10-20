using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;


namespace HRM.Business.Category.Models
{
    public class Cat_PaymentMethodEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public double? AdvancePayment { get; set; }
        public double? LongTimePayment { get; set; }
        public double? PaymentPerMonth { get; set; }
        public double? DeadlinePayment { get; set; }
        public string PaymentMethod { get; set; }
        public string Note { get; set; }
        public string Code { get; set; }
    }
}
