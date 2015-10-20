using System;

namespace HRM.Business.Category.Models
{
    public class Cat_DataGroupDetailEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public System.Guid DataGroupID { get; set; }
        public string FieldName { get; set; }
        public string ChildFieldName { get; set; }
        public string Value { get; set; }
        public string Notes { get; set; }
        public string ObjectName { get; set; }
        public string ChildFieldName1 { get; set; }
        public string Exclusions { get; set; }
        public string ForeignKey { get; set; }
    }


 
}
