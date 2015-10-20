using HRM.Business.BaseModel;
using System;

namespace HRM.Business.Category.Models
{
    public class Cat_OrgMoreInforEntity : HRMBaseModel
    {
        public string ServicesType { get; set; }
        public Nullable<System.DateTime> ContractFrom { get; set; }
        public Nullable<System.DateTime> ContractTo { get; set; }
        public string BillingCompanyName { get; set; }
        public string BillingAddress { get; set; }
        public string TaxCode { get; set; }
        public string Description { get; set; }
        public string DurationPay { get; set; }
        public string RecipientInvoice { get; set; }
        public string TelePhone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public Guid? OrgStructureID { get; set; }
    }
}
