using System;


namespace HRM.Business.Category.Models
{
    public class Cat_SalaryClassEntity : HRM.Business.BaseModel.HRMBaseModel
    {

        public string SalaryClassName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public System.Guid? SalaryClassTypeID { get; set; }
        public string SalaryClassTypeName{ get; set; }
        public string AbilityTitleVNI { get; set; }
        public string AbilityTitleENG { get; set; }
        public string AbilityTitleVNIABILITY { get; set; }
        public string AbilityTitleEngABILITY { get; set; }
        public int? OrderNumber { get; set; }
        public Nullable<System.Guid> AbilityTitleID { get; set; }
        public string AbilityTitleCode { get; set; }
    }

    public class Cat_SalaryClassMultiEntity
    {
        public System.Guid ID { get; set; }
        public string Code { get; set; }
        public string SalaryClassName { get; set; }
    }
    
}

