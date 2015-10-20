using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_ScoreTypeModel : BaseViewModel
    {

        [DisplayName(ConstantDisplay.HRM_Tra_ScoreType_ScoreTypeName)]

        public string ScoreTypeName { get; set; }
        public string TopicName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Tra_ScoreType_Code)]
        public string Code { get; set; }
         [DisplayName(ConstantDisplay.HRM_Tra_ScoreType_Description)]
        public string Description { get; set; }
         [DisplayName(ConstantDisplay.HRM_Tra_ScoreType_NumOrder)]
        public int? NumOrder { get; set; }
        public bool? IsTotal { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ScoreTypeName = "ScoreTypeName";
            public const string Code = "Code";
            public const string NumOrder = "NumOrder";
            public const string IsTotal = "IsTotal";
            public const string Description = "Description";
        }
    }

    public class Tra_ScoreTypeSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_ScoreType_ScoreTypeName)]
        public string ScoreTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_ScoreType_Code)]
        public string Code { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Tra_ScoreTypeMultiModel
    {
        public Guid ID { get; set; }
        public string ScoreTypeName { get; set; }
        public int TotalRow { get; set; }
    }

}
