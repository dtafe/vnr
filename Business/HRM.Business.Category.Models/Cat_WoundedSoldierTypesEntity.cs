using System;


namespace HRM.Business.Category.Models
{
    public class Cat_WoundedSoldierTypesEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string WoundedSoldierTypesName { get; set; }
        public string Notes { get; set; }
    }
    public class Cat_WoundedSoldierTypesMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string WoundedSoldierTypesName { get; set; }
    }
}

