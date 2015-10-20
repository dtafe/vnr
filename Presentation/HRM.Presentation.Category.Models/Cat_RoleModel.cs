using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_RoleModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Role_RoleName)]
        public string RoleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Role_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Role_Note)]
        public string Note { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string RoleName = "RoleName";
            public const string Note = "Note";
        }
    }

    public class Cat_RoleSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Role_RoleName)]
        public string RoleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Role_Code)]
        public string Code { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }


    }

    public class Cat_RoleMultiModel
    {
        public Guid ID { get; set; }
        public string RoleName { get; set; }
        public int TotalRow { get; set; }
    }
}
