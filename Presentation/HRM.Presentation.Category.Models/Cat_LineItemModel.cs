using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_LineItemModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_LineItem_BrandID)]
        public Nullable<System.Guid> BrandID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_LineItem_BrandID)]
        public string BrandName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_LineItem_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_LineItem_LineItemName)]
        public string LineItemName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_LineItem_Note)]
        public string Note { get; set; }
        
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string LineItemName = "LineItemName";
            public const string BrandName = "BrandName";
            public const string Code = "Code";
            public const string Note = "Note";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class Cat_LineItemSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_LineItem_LineItemName)]
        public string LineItemName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_LineItemMultiModel
    {
        public Guid ID { get; set; }
        public string LineItemName { get; set; }
        public int TotalRow { get; set; }
    }

}
