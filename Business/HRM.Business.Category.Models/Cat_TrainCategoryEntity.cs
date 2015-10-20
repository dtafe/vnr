using System;


namespace HRM.Business.Category.Models
{
    public class Cat_TrainCategoryEntity : HRM.Business.BaseModel.HRMBaseModel
    {

        public string TrainCategoryName { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }

    public class Cat_TrainCategoryMultiEntity 
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string TrainCategoryName { get; set; }
    }
}

