using System;


namespace HRM.Business.Category.Models
{
    public class Cat_SyncItemEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public Guid? SyncID { get; set; }
        public string OuterField { get; set; }
        public string InnerField { get; set; }
        public bool? AllowNull { get; set; }
        public bool? AllowDuplicate { get; set; }
        public int? DuplicateGroup { get; set; }
        public string FilterValues { get; set; }
        public bool? IsExcluded { get; set; }
        public string Description { get; set; }
    
    }

}

