using System;

namespace HRM.Business.Category.Models
{
    public class Cat_SupplierEntity : HRM.Business.BaseModel.HRMBaseModel
    {

        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public string Note { get; set; }
    }

    public class Cat_SupplierMultiEntity
    {
        public Guid ID { get; set; }
        public string SupplierName { get; set; }
     
    }
}
