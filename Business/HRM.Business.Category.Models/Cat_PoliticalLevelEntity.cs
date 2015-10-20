using System;


namespace HRM.Business.Category.Models
{
    public class Cat_PoliticalLevelEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string PoliticalLevelName { get; set; }
        public string Notes { get; set; }
    }
    public class Cat_PoliticalLevelMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string PoliticalLevelName { get; set; }
    }
}

