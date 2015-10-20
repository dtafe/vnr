using System;

namespace HRM.Business.Category.Models
{
    
    public class Cat_AppendixContractTypeEntity : BaseModel.HRMBaseModel
    {
        public int TotalRow { get; set; }
        public string AppendixContractName { get; set; }
        public string Code { get; set; }
        public Guid? ReportID { get; set; }
        public string ReportName { get; set; }
        public Guid? ExportID { get; set; }
        public string ExportName { get; set; }
    }
}
