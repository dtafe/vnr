using System;

namespace HRM.Business.Evaluation.Models
{
    public class Eva_PerformancePlanEntity : BaseModel.HRMBaseModel
    {
        public string PerformancePlanName { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Comment { get; set; }
    }

    public class Eva_PerformancePlanMultiEntity
    {
        public Guid ID { get; set; }
        public string PerformancePlanName { get; set; }
    }
}
