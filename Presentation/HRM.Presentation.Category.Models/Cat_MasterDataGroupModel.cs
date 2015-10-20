using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_MasterDataGroupModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_MasterDataGroup_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_MasterDataGroup_MasterDataGroupName)]
        public string MasterDataGroupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_MasterDataGroup_Notes)]
        public string Notes { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_MasterDataGroup_OrderNumber)]
        public int? OrderNumber { get; set; }


        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string MasterDataGroupName = "MasterDataGroupName";
            public const string Notes = "Notes";
            public const string OrderNumber = "OrderNumber";
        }
    }

    public class Cat_MasterDataGroupSearchModel
    {
        public string MasterDataGroupName { get; set; }
        public string Code { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Cat_MasterDataGroupMultiModel
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string MasterDataGroupName { get; set; }
    }

}
