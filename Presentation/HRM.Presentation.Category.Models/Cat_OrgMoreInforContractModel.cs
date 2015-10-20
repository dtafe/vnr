using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class Cat_OrgMoreInforContractModel : BaseViewModel
    {
        public Nullable<System.Guid> OrgMoreInforID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInforContract_ContactName)]
        public string ContactName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInforContract_Area)]
        public string Area { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInforContract_Department)]
        public string Department { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInforContract_Position)]
        public string Position { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInforContract_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInforContract_Telephone)]
        public string Telephone { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInforContract_CellPhone)]
        public string CellPhone { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_OrgMoreInforContract_Email)]
        public string Email { get; set; }
        private Guid _id = Guid.Empty;

        public Guid OrgMoreInfor_ID
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

            public const string ContactName = "ContactName";
            public const string Area = "Area";
            public const string Department = "Department";
            public const string Position = "Position";
            public const string BillingAddress = "BillingAddress";
            public const string Description = "Description";
            public const string TelePhone = "TelePhone";
            public const string CellPhone = "CellPhone";
            public const string Email = "Email";
           
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
          
        }
    }

}
