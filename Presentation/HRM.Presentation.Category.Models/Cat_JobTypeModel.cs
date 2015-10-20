using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_JobTypeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_JobType_JobTypeName)]
        public string JobTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobType_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobType_RoleID)]
        public Nullable<System.Guid> RoleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobType_RoleName)]
        public string RoleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Role_Note)]
        public string Note { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string JobTypeName = "JobTypeName";
            public const string Code = "Code";
            public const string RoleID = "RoleID";
            public const string RoleName = "RoleName";
            public const string Note = "Note";
        }
    }

    public class Cat_JobTypeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_JobType_JobTypeName)]
        public string JobTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_JobType_Code)]
        public string Code { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_JobTypeMultiModel
    {
        public Guid ID { get; set; }
        public string JobTypeName { get; set; }
        public int TotalRow { get; set; }
    }
}
