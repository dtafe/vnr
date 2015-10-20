using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Category.Models
{
    public class Cat_TopicModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_TopicName)]
        public string TopicName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_CorseID)]
        public Nullable<System.Guid> CorseID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_CorseID)]
        public string CourseName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_CertificateID)]
        public Nullable<System.Guid> CertificateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_CertificateID)]
        public string CertificateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_LeadTime)]
        public Nullable<double> LeadTime { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_DocumentCode)]
        public string DocumentCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_TrainingSample)]
        public string TrainingSample { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_TrainingCode)]
        public string TrainingCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_StandardTheory)]
        public Nullable<double> StandardTheory { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_StandardPractice)]
        public Nullable<double> StandardPractice { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_StandardAttendance)]
        public Nullable<double> StandardAttendance { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_CodeConstraint)]
        public string CodeConstraint { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_Formula)]
        public string Formula { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_RankingID)]
        public Nullable<System.Guid> RankingID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_RankingName)]
        public string RankingName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_RankingGroupID)]
        public Nullable<System.Guid> RankingGroupID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_RankingGroupID)]
        public string RankingGroupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_TypeHandleFomular)]
        public string TypeHandleFomular { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_IsComplex)]
        public Nullable<bool> IsComplex { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_MinimumScore)]
        public Nullable<double> MinimumScore { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_MaximumScore)]
        public Nullable<double> MaximumScore { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_IsNormal)]
        public Nullable<bool> IsNormal { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_IsTypeHandleFomular)]
        public bool IsTypeHandleFomular { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_IsTypeHandleFomularAuto)]
        public bool IsTypeHandleFomularAuto { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeScore_ScoreTypeID)]
        public Nullable<System.Guid> ScoreTypeID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public string ScoreType { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string TopicName = "TopicName";
            public const string Code = "Code";
            public const string CourseName = "CourseName";
            public const string CertificateName = "CertificateName";
            public const string LeadTime = "LeadTime";
            public const string DocumentCode = "DocumentCode";
            public const string TrainingSample = "TrainingSample";
            public const string TrainingCode = "TrainingCode";
            public const string StandardTheory = "StandardTheory";
            public const string StandardPractice = "StandardPractice";
            public const string StandardAttendance = "StandardAttendance";
            public const string CodeConstraint = "CodeConstraint";
            public const string Formula = "Formula";
            public const string RankingName = "RankingName";
            public const string RankingGroupName = "RankingGroupName";
            public const string TypeHandleFomular = "TypeHandleFomular";
            public const string MinimumScore = "MinimumScore";
            public const string MaximumScore = "MaximumScore";
            public const string ScoreType = "ScoreType";
        }
    }

    public class Cat_TopicSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_TopicName)]
        public string TopicName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_Code)]
        public string TopicCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_MinimumScore)]
        public Nullable<double> MinimumScore { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_MaximumScore)]
        public Nullable<double> MaximumScore { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Cat_TopicMultiModel
    {
        public Guid ID { get; set; }
        public string TopicName { get; set; }
        public int TotalRow { get; set; }
    }

}
