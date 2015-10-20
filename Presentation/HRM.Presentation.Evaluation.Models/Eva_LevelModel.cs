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
    public class Eva_LevelModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Eva_Level_LevelNameField)]
        public string LevelNameField { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_Level_LevelName)]
        public string LevelName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_Level_MinimumRating)]
        public double? MinimumRating { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_Level_MaximumRating)]
        public double? MaximumRating { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_Level_Comment)]
        public string Comment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_Level_PerformanceTypeID)]
        public Guid? PerformanceTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Eva_Level_PerformanceTypeName)]
        public string PerformanceTypeName { get; set; }
        public int TotalRow { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string LevelName = "LevelName";
            public const string MinimumRating = "MinimumRating";
            public const string MaximumRating = "MaximumRating";
            public const string Comment = "Comment";
            public const string PerformanceTypeID = "PerformanceTypeID";
            public const string PerformanceTypeName = "PerformanceTypeName";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
            public const string LevelNameField = "LevelNameField";
        }
    }
    public class Eva_LevelMultiModel
    {
        public Guid ID { get; set; }
        public string LevelName { get; set; }
    }
    public class Eva_LevelSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Eva_Level_LevelName)]
        public string LevelName { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
