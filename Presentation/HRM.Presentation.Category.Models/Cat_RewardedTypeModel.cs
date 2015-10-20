using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_RewardedTypeModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_RewardedTypee_RewardedTypeName)]
        public string RewardedTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_RewardedType_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_RewardedType_Notes)]
        public string Notes { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string RewardedTypeName = "RewardedTypeName";
            public const string Code = "Code";
            public const string Notes = "Notes";
        }
    }

    public class Cat_RewardedTypeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_RewardedTypee_RewardedTypeName)]
        public string RewardedTypeName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_RewardedTypeMultiModel
    {
        public Guid ID { get; set; }
        public string RewardedTypeName { get; set; }
    }

}
