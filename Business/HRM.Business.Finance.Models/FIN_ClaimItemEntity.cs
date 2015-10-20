using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HRM.Business.Finance.Models
{
    public class FIN_ClaimItemEntity : HRMBaseModel
    {
        public Nullable<System.Guid> ClaimID { get; set; }
        public string ClaimName { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Description { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }
        public string Description4 { get; set; }
        public Nullable<double> Amount { get; set; }
        public double? Total { get; set; }
        public string ClaimItemName { get; set; }
        public string DocumentNumber { get; set; }
        public string StatusView { get; set; }
        public string CurrencyName { get; set; }
        public Guid? CurrencyID { get; set; }
        public double? TotalAmount { get; set; }
    }
}
