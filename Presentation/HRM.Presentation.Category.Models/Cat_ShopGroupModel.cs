using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Category.Models
{
    public class Cat_ShopGroupModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_ShopGroup_ShopGroupName)]
        public string ShopGroupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ShopGroup_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ShopGroup_Description)]
        public string Description { get; set; }

        public partial class FieldNames
        {
            public const string ShopGroupName = "ShopGroupName";
            public const string Code = "Code";
            public const string Description = "Description";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }
    public class Cat_ShopGroupMultiModel
    {
        public Guid ID { get; set; }
        public string ShopGroupName { get; set; }
    }

    public class Cat_ShopGroupSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_ShopGroup_ShopGroupName)]
        public string ShopGroupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ShopGroup_Code)]
        public string Code { get; set; }

        public string ValueFields { get; set; }
    }

}
