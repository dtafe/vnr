using System;

namespace HRM.Business.Category.Models
{
    public class Cat_OwnerEntity : HRM.Business.BaseModel.HRMBaseModel
    {

  
        public string OwnerName { get; set; }
        public string OwnerType { get; set; }
        public Nullable<System.Guid> OwnerParentID { get; set; }
        public Nullable<int> OrderNumber { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }

        public string OwnerParentName { get; set; }
    }
    public class Cat_OwnerMultiEntity
    {
        public Guid ID { get; set; }
        public string OwnerName { get; set; }
    }


}
