using System;

namespace HRM.Business.Category.Models
{
    public class Cat_PivotItemEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }

        public Guid? PivotID { get; set; }
        public int? PivotSheetIndex { get; set; }
        public int? PivotHeaderRowIndex { get; set; }
        public int? PivotColumnStart { get; set; }
        public int? PivotColumnEnd { get; set; }
        public string PivotColumnName { get; set; }
        public int? StartRowIndex { get; set; }
        public int? StartColumnIndex { get; set; }
        public int? SkipRowNumbers { get; set; }
        public int? TargetSheetIndex { get; set; }
        public string TargetSheetName { get; set; }
        public string Description { get; set; }
    }
}
