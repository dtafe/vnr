using System;


namespace HRM.Business.Category.Models
{
    public class Cat_OvertimeResonEntity : HRM.Business.BaseModel.HRMBaseModel
    {

        public string OvertimeResonName { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string OvertimeResonNameEng { get; set; }
        
        public string UserUpdate { get; set; }
        public DateTime? DateUpdate { get; set; }
        
    }
    public class Cat_OvertimeResonMultiEntity
    {
        public Guid ID { get; set; }
        public string OvertimeResonName { get; set; }
    }
}

