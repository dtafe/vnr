using System;


namespace HRM.Business.Category.Models
{
    public class Cat_ArmedForceTypesEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string ArmedForceTypesName { get; set; }
        public string Notes { get; set; }
    }
    public class Cat_ArmedForceTypesMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string ArmedForceTypesName { get; set; }
    }
}

