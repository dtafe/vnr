using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_ScoreTopicModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_ScoreType_ScoreTypeName)]
        public Guid? ScoreTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_ScoreType_ScoreTypeName)]
        public string ScoreTypeName { get; set; }
        public Guid? TopicID { get; set; }
        public string TopicName { get; set; }
        public bool? IsComplex { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_CourseTopic_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_ScoreType_Code)]
        public string ScoreTypeCode { get; set; }
      
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ScoreTypeName = "ScoreTypeName";
            public const string TopicName = "TopicName";
            public const string Code = "Code";
            public const string CourseName = "CourseName";
            public const string ScoreTypeCode = "ScoreTypeCode";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }

}
