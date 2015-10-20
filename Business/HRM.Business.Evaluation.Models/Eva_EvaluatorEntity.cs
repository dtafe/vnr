using System;
using System.Collections.Generic;
using HRM.Business.BaseModel;

namespace HRM.Business.Evaluation.Models
{
    public class Eva_EvaluatorEntity : HRMBaseModel
    {
        public Guid? ProfileID { get; set; }
        public Guid? EvaluatorID { get; set; }
        public int? OrderNo { get; set; }
        public string Note { get; set; }
        public string ProfileName { get; set; }
        public string EvaluatorName { get; set; }
        public List<Guid> EvaluatorIDs { get; set; }
        public Guid? PerformanceTypeID { get; set; }
        public string PerformanceTypeName { get; set; }
        public double? Rate { get; set; }
        public Guid? PerformanceTemplateID { get; set; }
        public string PerformanceTemplateName { get; set; }

    }

    public class Eva_EvaluatorMultiEntity
    {
        public Guid ID { get; set; }
        public string ProfileName { get; set; }
    }

}
