using System;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;

namespace HRM.Presentation.Evaluation.Models
{
    public class Eva_PerformanceForDetailModel : BaseViewModel
    {
        public int Stt { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance)]
        public Guid? PerformanceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance)]
        public string PerformanceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_KPIName)]
        public Guid? KPIID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_KPIName)]
        public string KPIName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceDetail_Mark)]
         public double? Mark { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceDetail_Evaluation)]
        public string Evaluation { get; set; }
        public int? OrderKPI { get; set; }
        public string Formula { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceForDetail_DescriptionKPIFix)]
        public string DescriptionKPIFix { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_PerformanceForDetail_DescriptionKP)]
        public string DescriptionKP { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_FixedLeave)]
        public bool? IsKPIFix { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_KPI_KPIColor)]
        public string KPIColor { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_NameEntityObject)]
        public string NameEntityName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Rate)]
        public double? Rate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Rate)]
        public int? OrderNumber { get; set; }
        public Guid PerformanceForDetailID { get { return ID; } }
        public partial class FieldNames
        {
            public const string PerformanceForDetailID = "PerformanceForDetailID";
            public const string Stt = "Stt";
            public const string ID = "ID";
            public const string PerformanceName = "PerformanceName";
            public const string KPIName = "KPIName";
            public const string Mark = "Mark";
            public const string Evaluation = "Evaluation";
            public const string OrderKPI = "OrderKPI";
            public const string Formula = "Formula";
            public const string DescriptionKPIFix = "DescriptionKPIFix";
            public const string DescriptionKP = "DescriptionKP";
            public const string IsKPIFix = "IsKPIFix";
            public const string KPIColor = "KPIColor";
            public const string NameEntityName = "NameEntityName";
            public const string Rate = "Rate";
        }
    }
    public class Eva_PerformanceForDetailSearchModel
    {
        public Guid? PerformanceID { get; set; }
    }

   
}
