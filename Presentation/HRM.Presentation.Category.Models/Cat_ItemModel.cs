using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_ItemModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Item_LineItemID)]
        public Nullable<System.Guid> LineItemID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Item_LineItemID)]
        public string LineItemName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Item_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Item_ItemName)]
        public string ItemName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Item_Note)]
        public string Note { get; set; }
        
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string LineItemName = "LineItemName";
            public const string ItemName = "ItemName";
            public const string Code = "Code";
            public const string Note = "Note";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class Cat_ItemSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_Item_ItemName)]
        public string ItemName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_ItemMultiModel
    {
        public Guid ID { get; set; }
        public string ItemName { get; set; }
        public int TotalRow { get; set; }
    }

}
