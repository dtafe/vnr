using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HRM.Business.Finance.Models
{
    public class FIN_PurchaseRequestEntity : HRMBaseModel
    {
        public Guid? FunctionID { get; set; }
        public string FunctionName { get; set; }
        public Guid? BudgetOwnerID { get; set; }
        public string BudgetOwnerName { get; set; }
        public Guid? ChannelID { get; set; }
        public string ChannelName { get; set; }
        public DateTime? BudgetChargedIn { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public Guid? SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Guid? UserApprovedID { get; set; }
        public Guid? UserCreateID { get; set; }
        public string UserApprovedName { get; set; }
        public double? Total { get; set; }
        public string Code { get; set; }
        public string UserCreateName { get; set; }
        public Guid? CurrencyID1 { get; set; }
        public Guid? CurrencyID2 { get; set; }
        public int? CurrencyRate1 { get; set; }
        public int? CurencryRate2 { get; set; }
        public string CurrencyName1 { get; set; }
        public string CurrencyName2 { get; set; }
    }
}
