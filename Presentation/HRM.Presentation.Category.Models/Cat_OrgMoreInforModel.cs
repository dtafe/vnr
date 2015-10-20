using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class Cat_OrgMoreInforModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_ServicesType)]
        public string ServicesType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_ContractFrom)]
        public Nullable<System.DateTime> ContractFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_ContractTo)]
        public Nullable<System.DateTime> ContractTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_BillingCompanyName)]
        public string BillingCompanyName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_BillingAddress)]
        public string BillingAddress { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_TaxCode)]
        public string TaxCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_DurationPay)]
        public string DurationPay { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_RecipientInvoice)]
        public string RecipientInvoice { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_TelePhone)]
        public string TelePhone { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_CellPhone)]
        public string CellPhone { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInfor_Email)]
        public string Email { get; set; }
        private Guid _id = Guid.Empty;
 
        public Guid OrgStructure_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;
            }
        }

        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {

            public const string ServicesType = "ServicesType";
            public const string ContractFrom = "ContractFrom";
            public const string ContractTo = "ContractTo";
            public const string BillingCompanyName = "BillingCompanyName";
            public const string BillingAddress = "BillingAddress";
            public const string TaxCode = "TaxCode";
            public const string Description = "Description";
            public const string DurationPay = "DurationPay";
            public const string RecipientInvoice = "RecipientInvoice";
            public const string TelePhone = "TelePhone";
            public const string CellPhone = "CellPhone";
            public const string Email = "Email";
           
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
          
        }
    }

}
