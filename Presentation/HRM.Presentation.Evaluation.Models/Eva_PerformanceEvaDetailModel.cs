using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Evaluation.Models
{
    public class Eva_PerformanceEvaDetailModel : BaseViewModel
    {

        public Guid PerfomanceEvaID { get; set; }
        public Guid? PerfomanceDetailID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_KPIName)]
        public string KPIName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceDetail_Mark)]
        public double? Mark { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceDetail_Evaluation)]
        [DataType(DataType.Text)]
        [UIHint("TextArea")]
        public string Evaluation { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_MinimumRating)]
        public double? MinimumRating { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_MaximumRating)]
        public double? MaximumRating { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_KPI_Scale)]
        public string Scale { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_KPI_MeasuredSource)]
        public string MeasuredSource { get; set; }
        public string Evaluator { get; set; }
        public int? OrderEva { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Rate)]
        public double? Rate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceForDetail_DescriptionKPIFix)]
        [UIHint("TextArea")]
        [DataType(DataType.Text)]
        public string DescriptionKPIFix { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceForDetail_DescriptionKP)]
        [UIHint("TextArea")]
        [DataType(DataType.Text)]
        public string DescriptionKP { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_KPI_KPIColor)]
        public string KPIColor { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_NameEntityObject)]
        public string NameEntityName { get; set; }
        public int? OrderNumber { get; set; }
        public int? Stt { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string KPIName = "KPIName";
            public const string Mark = "Mark";
            public const string Scale = "Scale";
            public const string MeasuredSource = "MeasuredSource";
            public const string Evaluation = "Evaluation";
            public const string MinimumRating = "MinimumRating";
            public const string MaximumRating = "MaximumRating";
            public const string Evaluator = "Evaluator";
            public const string Rate = "Rate";
            public const string DescriptionKPIFix = "DescriptionKPIFix";
            public const string DescriptionKP = "DescriptionKP";
            public const string KPIColor = "KPIColor";
            public const string NameEntityName = "NameEntityName";
            public const string OrderNumber = "OrderNumber";
            public const string Stt = "Stt";
        }
    }

    public class Eva_PerformanceEvaDetailSearchModel
    {
        //[DisplayName(ConstantDisplay.HRM_Evaluation_TemplateName)]
        //public string PerformanceTemplateID { get; set; }

    }
}
