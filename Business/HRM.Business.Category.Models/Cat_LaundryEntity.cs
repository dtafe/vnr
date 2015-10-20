using System;

namespace HRM.Business.Category.Models
{
    public class Cat_LaundryMultiEntity 
    {
        public Guid ID { get; set; }
        public string ShiftName { get; set; }
    }
    public class Cat_LaundryEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }
        public string Code { get; set; }
        public string LaundryName { get; set; }
        public double LaundryPrice { get; set; }
        public string Description { get; set; }
    }
}
