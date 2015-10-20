using System;


namespace HRM.Business.Category.Models
{
    public class Cat_ProductTypeEntity : HRM.Business.BaseModel.HRMBaseModel
    {


        public string TypeName { get; set; }
        public string Description { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        
    }
    public class Cat_ProductTypeMultiEntity
    {
        public Guid ID { get; set; }
        public string TypeName { get; set; }
    }

}

