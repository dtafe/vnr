using System;

namespace HRM.Business.Category.Models
{
    public class Cat_PurchaseItemsEntity : HRM.Business.BaseModel.HRMBaseModel
    {

        public string PurchaseItemName { get; set; }
        public string PurchaseItemCode { get; set; }
        public string PurchaseItemCost { get; set; }
        public Guid? OwnerID { get; set; }
        public string OwnerName { get; set; }
    }

    public class Cat_PurchaseItemsMultiEntity
    {
        public Guid ID { get; set; }
        public string PurchaseItemName { get; set; }
     
    }
}
