using System;
using HRM.Business.BaseModel;

namespace HRM.Business.Category.Models
{
    public class Cat_ModelEntity : HRMBaseModel
    {
        public string ModelType { get; set; }
        public string ModelCode { get; set; }
        public string ModelName { get; set; }
        public Guid? ColorID { get; set; }
        public string ColorName { get; set; }
        public DateTime DateApply { get; set; }
        public Double? NormalPrice { get; set; }
        public Double? SpecialPrice { get; set; }
        public Double? WHolePrice { get; set; }
        public string Note { get; set; }
    }
    public class Cat_ModelMultiEntity
    {
        
        public string ModelName { get; set; }
        public int TotalRow { get; set; }
    }
}
