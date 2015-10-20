using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HRM.Business.Finance.Models
{
    public class Fin_CashAdvanceItemEntity : HRMBaseModel
    {
        public Nullable<System.Guid> CashAdvanceID { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public Nullable<double> Amount { get; set; }
        public string Remark { get; set; }
        public string CashAdvanceItemName { get; set; }
        public string StatusView { get; set; }
        public string CurrencyName { get; set; }
        public double? TotalAmount { get; set; }

    }
}
