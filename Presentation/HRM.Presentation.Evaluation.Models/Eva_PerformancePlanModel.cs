using System;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Evaluation.Models
{
    public class Eva_PerformancePlanModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformancePlan_PerformancePlanName)]
        public string PerformancePlanName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Comment)]
        public string Comment { get; set; }
         [DisplayName(ConstantDisplay.HRM_Evaluation_PerformancePlan_StartDate)]
        public DateTime StartDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformancePlan_EndDate)]
        public DateTime EndDate { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string PerformancePlanName = "PerformancePlanName";
            public const string StartDate = "StartDate";
            public const string EndDate = "EndDate";
            public const string Comment = "Comment";
        }
    }

    public class Eva_PerformancePlanSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformancePlan_PerformancePlanName)]
        public string PerformancePlanName { get; set; }
        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }
        public DateTime? EndDateFrom { get; set; }
        public DateTime? EndDateTo { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Eva_PerformancePlanMultiModel
    {
        public Guid ID { get; set; }
        public string PerformancePlanName { get; set; }
    }
}
