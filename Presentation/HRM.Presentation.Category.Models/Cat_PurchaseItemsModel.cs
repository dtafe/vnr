using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_PurchaseItemsModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_PurchaseItem_PurchaseItemName)]
        public string PurchaseItemName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_PurchaseItem_PurchaseItemCode)]
        public string PurchaseItemCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_PurchaseItem_PurchaseItemCost)]
        public string PurchaseItemCost { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_PurchaseItem_OwnerName)]
        public Guid? OwnerID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_PurchaseItem_OwnerName)]
        public string OwnerName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string PurchaseItemName = "PurchaseItemName";
            public const string PurchaseItemCode = "PurchaseItemCode";
            public const string PurchaseItemCost = "PurchaseItemCost";
            public const string OwnerName = "OwnerName";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class Cat_PurchaseItemsSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Category_PurchaseItem_PurchaseItemName)]
        public string PurchaseItemName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_PurchaseItemsMultiModel
    {
        public Guid ID { get; set; }
        public string PurchaseItemName { get; set; }
        public int TotalRow { get; set; }
    }

}
