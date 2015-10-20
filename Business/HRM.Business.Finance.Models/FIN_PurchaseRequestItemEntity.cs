using System;
using HRM.Business.BaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HRM.Business.Finance.Models
{
    public class FIN_PurchaseRequestItemEntity : HRMBaseModel
    {
        public Guid? PurchaseRequestID { get; set; }
        public Guid? CostCenterID { get; set; }
        public string Code { get; set; }
        public string CateCodeType { get; set; }
        public string Name { get; set; }
        public Guid? ProjectID { get; set; }
        public string ProjectName { get; set; }
        public Guid? ItemID { get; set; }
        public string PurchaseItemName { get; set; }
        public int? Quantity { get; set; }
        public double? UnitPrice { get; set; }
        public double? Amount { get; set; }
        public double? AmountConvert { get; set; }
        
    }
}
