using System;


namespace HRM.Business.Category.Models
{
    public class Cat_ConditionalColorEntity : HRM.Business.BaseModel.HRMBaseModel
    {

        public string ConditionalColorName { get; set; }
        public string ConditionalCode { get; set; }
        public string PropertyName { get; set; }
        public string Condition { get; set; }
        public string Value { get; set; }
        public string ColorCode { get; set; }
        public string BGColorCode { get; set; }
        
    }

}

