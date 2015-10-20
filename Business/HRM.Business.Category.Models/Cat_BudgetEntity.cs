using System;


namespace HRM.Business.Category.Models
{
    public class Cat_BudgetEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string BudgetName { get; set; }
        public string Description { get; set; }
    }
    public class Cat_BudgetMultiEntity
    {
     //   public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string BudgetName { get; set; }
    }


}

