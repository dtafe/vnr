using System;
using HRM.Business.BaseModel;


namespace HRM.Business.Category.Models
{
    public class Cat_ReceivePlaceEntity : HRMBaseModel
    {
        public string Code { get; set; }
        public string ReceivePlace { get; set; }
        public string Note { get; set; }
    }
}
