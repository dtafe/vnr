using System;

namespace HRM.Business.Category.Models
{
    public class Cat_ImportEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }
        public string ImportName { get; set; }
        public string ImportCode { get; set; }
        public string ObjectName { get; set; }
        public long? SheetIndex { get; set; }
        public long? StartRowIndex { get; set; }
        public long? StartColumnIndex { get; set; }
        public string TemplateFile { get; set; }
        public string Description { get; set; }
        public bool IsProtected { get; set; }
        public string ProcessDuplicateData { get; set; }
        
    }

    public class Cat_SysTablesMultiEntity
    {
        public Guid? ObjectID { get; set; }
        public string Name { get; set; }
    }

    public class Cat_ImportMultiEntity
    {
        public Guid? ID { get; set; }
        public string ImportName { get; set; }
        public string ObjectName { get; set; }
    }
}
