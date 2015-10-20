using System;

namespace HRM.Business.Category.Models
{
    public class Cat_RelativeTypeMultiEntity 
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string RelativeTypeName { get; set; }
        public string Code { get; set; }
    }

    public class Cat_RelativeTypeEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string RelativeTypeName { get; set; }
        public string Code { get; set; }
    }
}
