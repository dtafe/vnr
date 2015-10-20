using System;

namespace HRM.Business.Category.Models
{
    public class Cat_EthnicGroupEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string EthnicGroupName { get; set; }
        public string Code { get; set; }
    }

    public class Cat_EthnicGroupMultiEntity
    {
        public Guid ID { get; set; }
        public string EthnicGroupName { get; set; }
    }
}
