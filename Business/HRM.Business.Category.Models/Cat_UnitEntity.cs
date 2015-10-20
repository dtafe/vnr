using System;

namespace HRM.Business.Category.Models
{
    public class Cat_UnitEntity : HRM.Business.BaseModel.HRMBaseModel
    {

        public string UnitName { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
    }

    public class Cat_UnitMultiEntity
    {
        public Guid ID { get; set; }
        public string UnitName { get; set; }
     
    }
}
