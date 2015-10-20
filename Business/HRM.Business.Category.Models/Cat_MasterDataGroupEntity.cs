using System;

namespace HRM.Business.Category.Models
{
    public class Cat_MasterDataGroupEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string MasterDataGroupName { get; set; }
        public string Notes { get; set; }
        public int? OrderNumber { get; set; }
    }

    public class Cat_MasterDataGroupMultiEntity
    {
        public Guid ID { get; set; }
        public string MasterDataGroupName { get; set; }
    }

}
