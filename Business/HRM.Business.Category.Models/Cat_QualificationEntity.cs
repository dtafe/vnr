using System;
using HRM.Business.BaseModel;

namespace HRM.Business.Category.Models
{
    public class Cat_QualificationEntity : HRMBaseModel
    {
        public string Code { get; set; }
        public string QualificationName { get; set; }
        public string Notes { get; set; }


    }
    public class Cat_QualificationMultiEntity
    {
        public Guid ID { get; set; }
        public string QualificationName { get; set; }
    }
    public class Cat_QualificationMultiLevelEntity
    {
        public Guid ID { get; set; }
        public string NameEntityName { get; set; }
    }
}

