using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Evaluation.Models
{
    public class Eva_EvaluatorModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_ProfileName)]
        public Guid? ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_EvaluatorName)]
        public Guid? EvaluatorID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_OrderNo)]
        public int? OrderNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_Note)]
        public string Note { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_EvaluatorName)]
        public string EvaluatorName { get; set; }

        public List<Guid> EvaluatorIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Tag)]
        public Guid? PerformanceTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceTemplateOptionLabel)]
        public Guid? PerformanceTemplateID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Evaluation_Tag)]
        public string PerformanceTypeName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_Rate)]
        public double? Rate { get; set; }

         [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceTemplate)]
         public string PerformanceTemplateName { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ProfileID = "ProfileID";
            public const string EvaluatorID = "EvaluatorID";
            public const string OrderNo = "OrderNo";
            public const string Note = "Note";
            public const string ProfileName = "ProfileName";
            public const string EvaluatorName = "EvaluatorName";
            public const string PerformanceTypeName = "PerformanceTypeName";
            public const string Rate = "Rate";
        }
    }
    //public class Eva_EvaluatorMultiModel
    //{
    //    public Guid ID { get; set; }
    //    public string KPIName { get; set; }
    //}
    public class Eva_EvaluatorSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Evaluation_Evaluator_ProfileName)]
        public string ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Tag)]
        public Guid? PerformanceTypeID { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Eva_EvaluatorMultiModel
    {
        public Guid ID { get; set; }
        public string ProfileName { get; set; }
    }

}
