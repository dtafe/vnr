using System;

namespace HRM.Business.Category.Models
{
    public class Cat_ImportItemEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }
        public string Code { get; set; }
        public Guid? ImportID { get; set; }
        public string ImportName { get; set; }
        public string ChildFieldLevel1 { get; set; }
        public string ChildFieldLevel2 { get; set; }
        public string ExcelField { get; set; }
        public bool AllowNull { get; set; }
        public bool AllowDuplicate { get; set; }
        public long? DuplicateGroup { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsDefaultValue { get; set; }
        public string DefaultValue { get; set; }

    }
    public class Cat_ImportItemMultiEntity
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
}
