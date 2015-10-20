using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;


namespace HRM.Presentation.Category.Models
{
    public class Cat_PaymentMethodModel : BaseViewModel
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
