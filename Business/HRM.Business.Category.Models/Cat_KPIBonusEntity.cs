using System;


namespace HRM.Business.Category.Models
{
    public class Cat_KPIBonusEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string KPIBonusName { get; set; }
        public string Code { get; set; }
        //public double? Value { get; set; }
      
        public string Note { get; set; }
        public bool? IsTotalRevenue { get; set; }
        public Nullable<System.Guid> UnitID { get; set; }

        public string UnitName { get; set; }
    }

    public class Cat_KPIBonusMultiEntity
    {
        public Guid ID { get; set; }
        public string KPIBonusName { get; set; }
    }
}
