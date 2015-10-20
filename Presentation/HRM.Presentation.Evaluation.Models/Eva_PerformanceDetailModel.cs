using System;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Evaluation.Models
{
    public class Eva_PerformanceDetailModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Evaluation_TemplateName)]
        public Guid PerformanceTemplateID { get; set; }
        public Guid KPIID { get; set; }
        public string Comment { get; set; }
        public string Code { get; set; }
        public int? OrderNumber { get; set; }
        public double? Rate { get; set; }
        public partial class FieldNames
        {
            public const string PerformanceTemplateID = "PerformanceTemplateID";
            public const string KPIID = "KPIID";
            public const string Comment = "Comment";
            public const string Code = "Code";
        }
    }

   
}
