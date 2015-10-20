using System;

namespace HRM.Business.Category.Models
{
    public class Cat_DisciplinedTypesMultiEntity 
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string DisciplinedTypesName { get; set; }
        public string Code { get; set; }
    }

    public class Cat_DisciplinedTypesEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string DisciplinedTypesName { get; set; }
        public string Code { get; set; }
        public string Notes { get; set; }
    }
}
