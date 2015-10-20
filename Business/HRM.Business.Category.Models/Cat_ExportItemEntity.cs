using System;

namespace HRM.Business.Category.Models
{
    public class Cat_ExportItemEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }
        public string ExportItemCode { get; set; }
        public Guid? ExportID { get; set; }
        public string DataField { get; set; }
        public string ExcelField { get; set; }
        public string FormatString { get; set; }
        public string Description { get; set; }
        public bool? IsFixedCell { get; set; }
        public long? GroupOrder { get; set; }
        public bool? SubTotal { get; set; }
        public string ExportName { get; set; }
    }
}
