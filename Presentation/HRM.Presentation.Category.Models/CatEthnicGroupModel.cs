using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class CatEthnicGroupModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_EthnicGroup_EthnicGroupName)]
        [StringLength(100, ErrorMessage = ConstantDisplay.HRM_Category_EthnicGroup_EthnicGroupName_StringLength)]
        public string EthnicGroupName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_EthnicGroup_Code)]
        [StringLength(50, ErrorMessage = ConstantDisplay.HRM_Category_EthnicGroup_Code_StringLength)]
        public string Code { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string EthnicGroupName = "EthnicGroupName";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";

        }
    }

    public class CatEthnicGroupSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_EthnicGroup_EthnicGroupName)]
        public string EthnicGroupName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Category_EthnicGroup_Code)]
        public string EthnicGroupCode { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string EthnicGroupName = "EthnicGroupName";
        }
    }

    public class Cat_EthnicGroupMultiModel
    {
        public Guid ID { get; set; }
        public string EthnicGroupName { get; set; }
    }
}
