using System;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Category.Models
{
    public class Cat_AccountTypeModel : BaseViewModel
    {
         [DisplayName(ConstantDisplay.HRM_Category_AccountType_AccountTypeName)]
        public string AccountTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_AccountType_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_AccountType_Description)]
        public string Description { get; set; }
        public partial class FieldNames
        {
            public const string AccountTypeName = "AccountTypeName";
            public const string Code = "Code";
            public const string Description = "Description";
        }
        

    }
    public class Cat_AccountTypeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_AccountType_AccountTypeName)]
        public string AccountTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_AccountType_Code)]
        public string Code { get; set; }

        public string ValueFields { get; set; }
    }
    public class Cat_AccountTypeMultiModel
    {
        public Guid ID { get; set; }
        public string AccountTypeName { get; set; }

    }
}
