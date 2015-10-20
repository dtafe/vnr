using System;
using System.Collections.Generic;
using HRM.Business.BaseModel;

namespace HRM.Business.Evaluation.Models
{
    public class Eva_PerformanceEvaDetailEntity : HRMBaseModel
    {
        public Guid PerfomanceEvaID { get; set; }
        public Guid? PerfomanceDetailID { get; set; }
        public string Code { get; set; }
        public string KPIName { get; set; }
        public double? Mark { get; set; }
        public string Evaluation { get; set; }
        public double? MinimumRating { get; set; }
        public double? MaximumRating { get; set; }
        public string Scale { get; set; }
        public string MeasuredSource { get; set; }
        public int? OrderEva { get; set; }
        public string Evaluator { get; set; }
        public Guid? KPIID { get; set; }
        public double? Rate { get; set; }
        public string DescriptionKPIFix { get; set; }
        public string DescriptionKP { get; set; }
        public string KPIColor { get; set; }
        public string NameEntityName { get; set; }
        public int? OrderNumber { get; set; }
        public int? Stt { get; set; }
      
    }
  
}
