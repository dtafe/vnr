using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.Evaluation.Models
{
    public class Eva_KPIModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Evaluation_NameEntityObject)]

        public System.Guid? NameEntityID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_NameEntityObject)]
        public string NameEntityObject { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_MinimumRating)]
        public double MinimumRating { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_MaximumRating)]
        public double MaximumRating { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Rate)]
        public double Rate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Level)]
        public string Level { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Comment)]
        public string Comment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_KPIName)]
        public string KPIName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_KPI_MeasuredSource)]
        public string MeasuredSource { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_NameEntityObject)]
        public string NameEntityName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_KPI_Scale)]
        public string Scale { get; set; }
        public int? OrderNumber { get; set; }
        public int? Stt { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_TotalMark)]
        public double? Mark { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceEva_ResultNote)]
        public string Evaluation { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceForDetail_DescriptionKPIFix)]
        [DataType(DataType.Text)]
        [UIHint("TextArea")]
        public string DescriptionKPIFix { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceForDetail_DescriptionKP)]
        [DataType(DataType.Text)]
        [UIHint("TextArea")]
        public string DescriptionKP { get; set; }
        public Guid? PerformanceForDetailID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_FixedLeave)]
        public bool? IsKPIFix { get; set; }

        [DisplayName(ConstantDisplay.HRM_Evaluation_KPI_KPIColor)]
        public string KPIColor { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string NameEntityObject = "NameEntityObject";
            public const string MinimumRating = "MinimumRating";
            public const string MaximumRating = "MaximumRating";
            public const string Rate = "Rate";
            public const string Level = "Level";
            public const string Comment = "Comment";
            public const string Code = "Code";
            public const string KPIName = "KPIName";
            public const string MeasuredSource = "MeasuredSource";
            public const string Scale = "Scale";
            public const string NameEntityName = "NameEntityName";
            public const string Mark = "Mark";
            public const string Evaluation = "Evaluation";
            public const string DescriptionKPIFix = "DescriptionKPIFix";
            public const string DescriptionKP = "DescriptionKP";
            public const string IsKPIFix = "IsKPIFix";
            public const string KPIColor = "KPIColor";
            public const string OrderNumber = "OrderNumber";
            public const string Stt = "Stt";
        }
    }
    public class Eva_KPIMultiModel
    {
        public Guid ID { get; set; }
        public string KPIName { get; set; }
    }
    public class Eva_KPISearchModel
    {
        public string PerformanceTemplateID { get; set; }
    }
    public class Eva_KPISearch1Model
    {
        public string KPIName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }



    public class Eva_KPIPerformanceIDSearchModel
    {
        public string PerformanceTemplateID { get; set; }
        public string PerformanceID { get; set; }
    }
}
