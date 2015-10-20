using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_CourseModel : BaseViewModel
    {

        [DisplayName(ConstantDisplay.HRM_Tra_Course_Code)]
        public string Code { get; set; }
        public string Place { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_TrainCategoryName)]
        public Nullable<System.Guid> TrainCategoryID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_RankGroupName)]
        public Nullable<System.Guid> RankingGroupID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_CourseName)]
        public string CourseName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_TrainCategoryName)]
        public string TrainCategoryName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_RankGroupName)]
        public string RankGroupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_SchoolFee)]
        public string SchoolFee { get; set; }
        private Guid? _CurrencyID;
        [DisplayName(ConstantDisplay.HRM_Category_Grade_CurrencyID)]
        public Guid? CurrencyID
        {
            get
            {
                return _CurrencyID;
            }
            set
            {
                _CurrencyID = value;
            }
        }
        public double? AmountSchoolFee { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Certificate_CertificateName)]
        public string Category { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_IsExternal)]
        public Nullable<bool> IsExternal { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_TrainningPlace)]
        public string TrainningPlace { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_StandardTheory)]
        public string StandardTheory { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_StandardPractice)]
        public string StandardPractice { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_StandardAttendance)]
        public string StandardAttendance { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_IsComplex)]
        public Nullable<bool> IsComplex { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_IsNormal)]
        public Nullable<bool> IsNormal { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_Formula)]
        public string Formula { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_TypeHandleFomular)]
        public string TypeHandleFomular { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_IsTypeHandleFomular)]
        public bool IsTypeHandleFomular { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_IsTypeHandleFomularAuto)]
        public bool IsTypeHandleFomularAuto { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_IsAutoCreated)]
        public Nullable<bool> IsAutoCreated { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_Score)]
        public Nullable<double> Score { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Course_MinimumScore)]
        public Nullable<double> MinimumScore { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_MaximumScore)]
        public Nullable<double> MaximumScore { get; set; }
        [DisplayName(ConstantDisplay.HRM_Cat_Topic_TopicName)]
        public Guid? TopicID { get; set; }

        public string strTopicCode { get; set; }

        public string SalaryClassName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Course_RankID)]
        public Nullable<System.Guid> RankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_Seniority)]
        public Nullable<int> Seniority { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_TimeOnCurrentRank)]
        public Nullable<int> TimeOnCurrentRank { get; set; }
        public string CourseName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_CourseID)]
        public Nullable<System.Guid> CourseID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_CourseID)]
        public string CourseListID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_IsAllRank)]
        public Nullable<bool> IsAllRank { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_IsAllRank)]
        public Nullable<bool> IsAllRankTrainner { get; set; }
        public string OrgListCode1 { get; set; }
        public string JobTitleListCode1 { get; set; }
        public string OrgListTrainerCode { get; set; }
        public string JobTitleListTrainerCode { get; set; }

        private Guid _id = Guid.Empty;
        public Guid Course_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;
            }
        }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_TimeCourse)]
        public int? TimeCourse { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_LotsOfClass)]
        public int? LotsOf { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_LotsOfClassMin)]
        public int? LotsOfClassMin { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_LotsOfClassMax)]
        public int? LotsOfClassMax { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_IsAttendance)]
        public bool? IsAttendance { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_ScoreStandard)]
        public int? ScoreStandard { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_JobTitle)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_RankID)]
        public string RankListID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_RankID)]
        public Nullable<System.Guid> RankTrainnerID { get; set; }
        //public Guid? RankTrainnerID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_RankID)]
        public string RankTrainerListID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_JobTitle)]
        public Guid? JobTitleTrainerID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_OrgStructure)]
        public Guid? OrgTrainerID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_Seniority)]
        public int? SeniorityTrainer { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_TimeOnCurrentRank)]
        public int? TimeOnCurrentRankTrainer { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_CourseID)]
        public Nullable<System.Guid> CourseTrainerID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_CourseID)]
        public string CourseListTrainerID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_OrgStructure)]
        public Guid? OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Course_JobTitle)]
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_JobTitle)]
        public string JobTitleTrainerName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_OrgStructure)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_OrgStructure)]
        public string OrgStructureTrainerName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_DurationMinCourse)]
        public Nullable<double> DurationMinCourse { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_DurationMinTrainerCourse)]
        public Nullable<double> DurationMinTrainerCourse { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Course_ExamTimes)]
        public int? ExamTimes { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_MaxSeniority)]
        public Nullable<double> MaxSeniority { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_MaxSeniorityTrainer)]
        public Nullable<double> MaxSeniorityTrainer { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_StatusCourse)]
        public string StatusCourse { get; set; }

        public string OrgStructureNameForTrainee { get; set; }
        public string OrgStructureCodeForTrainee { get; set; }
        public string OrgStructureNameForTrainer { get; set; }
        public string OrgStructureCodeForTrainer { get; set; }

        public string JobTileNameForTrainee { get; set; }
        public string JobTileNameForTrainer { get; set; }

        public string CourseNameForTrainee { get; set; }
        public string CourseNameForTrainer { get; set; }
        public string StatusCourseView { get; set; }
        
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string TrainCategoryName = "TrainCategoryName";
            public const string Code = "Code";
            public const string CourseName = "CourseName";
            public const string ExamTimes = "ExamTimes";
            public const string Description = "Description";
            public const string RankGroupName = "RankGroupName";
            public const string SchoolFee = "SchoolFee";
            public const string IsExternal = "IsExternal";
            public const string IsComplex = "IsComplex";
            public const string IsAutoCreated = "IsAutoCreated";
            public const string Category = "Category";
            public const string TrainningPlace = "TrainningPlace";
            public const string StandardTheory = "StandardTheory";
            public const string StandardPractice = "StandardPractice";
            public const string StandardAttendance = "StandardAttendance";
            public const string MinimumScore = "MinimumScore";
            public const string MaximumScore = "MaximumScore";
            public const string LotsOfClassMax = "LotsOfClassMax";
            public const string LotsOfClassMin = "LotsOfClassMin";
            public const string TimeCourse = "TimeCourse";
            public const string ScoreStandard = "ScoreStandard";
            public const string IsAttendance = "IsAttendance";
        }
    }

    public class Tra_CourseSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_Course_CourseName)]
        public string CourseName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_MinimumScore)]
        public Nullable<double> MinimumScore { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_MaximumScore)]
        public Nullable<double> MaximumScore { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_LotsOfClassMin)]
        public int? LotsOfClassMin { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_LotsOfClassMax)]
        public int? LotsOfClassMax { get; set; }

        public bool IsCreateTemplate { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Tra_CourseMultiModel
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string CourseName { get; set; }
        public int TotalRow { get; set; }
    }

}
