using System;

namespace HRM.Business.Evaluation.Models
{
    public class Eva_PerformanceForDetailEntity : BaseModel.HRMBaseModel
    {
        public int Stt { get; set; }
        public Guid? PerformanceID { get; set; }
        public string PerformanceName { get; set; }
        public Guid? KPIID { get; set; }
        public string KPIName { get; set; }
        public double? Mark { get; set; }
        public string Evaluation { get; set; }
        public int? OrderKPI { get; set; }
        public string Formula { get; set; }
        public string DescriptionKPIFix { get; set; }
        public string DescriptionKP { get; set; }
        public bool? IsKPIFix { get; set; }
        public string KPIColor { get; set; }
        public string NameEntityName { get; set; }
        public double? Rate { get; set; }
        public int? OrderNumber { get; set; }
      
    }
}
