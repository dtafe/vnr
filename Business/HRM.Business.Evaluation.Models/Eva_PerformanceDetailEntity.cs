using System;

namespace HRM.Business.Evaluation.Models
{
    public class Eva_PerformanceDetailEntity : BaseModel.HRMBaseModel
    {
        public Guid PerformanceTemplateID { get; set; }
        public Guid KPIID { get; set; }
        public string Comment { get; set; }
        public string Code { get; set; }
        public int? OrderNumber { get; set; }
        public double? Rate { get; set; }
    }
}
