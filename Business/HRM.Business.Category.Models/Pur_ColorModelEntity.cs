using System;
using HRM.Business.BaseModel;


namespace HRM.Business.Category.Models
{
    public class PUR_ColorModelEntity : HRMBaseModel
    {
        public Guid? ModelID { get; set; }
        public string ColorCode { get; set; }
        public string Color { get; set; }
        public string Note { get; set; }
    }
}
