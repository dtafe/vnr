using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_TraineeScoreModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeScore_TraineeTopicID)]
        public Nullable<System.Guid> TraineeTopicID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeScore_TraineeTopicID)]
        public string TraineeTopicName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeScore_ScoreTypeID)]
        public Nullable<System.Guid> ScoreTypeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeScore_ScoreTypeID)]
        public string ScoreTypeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeScore_Score)]
        public Nullable<double> Score { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeScore_TheoryScores)]
        public Nullable<double> TheoryScores { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeScore_PracticeScores)]
        public Nullable<double> PracticeScores { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeScore_AttendanceScores)]
        public Nullable<double> AttendanceScores { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeScore_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_TraineeScore_Times)]
        public Nullable<int> Times { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Canteen_MealRecordMissing_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_CourseName)]
        public string CourseName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_ClassName)]
        public string ClassName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Tra_TraineeScore_Times)]
        public string Code { get; set; }
         public string E_UNIT { get; set; }
         public string E_DIVISION { get; set; }
         public string E_DEPARTMENT { get; set; }
         public string E_TEAM { get; set; }
         public string E_SECTION { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string TraineeTopicID = "TraineeTopicID";
            public const string TraineeTopicName = "TraineeTopicName";
            public const string ScoreTypeID = "ScoreTypeID";
            public const string ScoreTypeName = "ScoreTypeName";
            public const string Score = "Score";
            public const string TheoryScores = "TheoryScores";
            public const string PracticeScores = "PracticeScores";
            public const string AttendanceScores = "AttendanceScores";
            public const string Type = "Type";
            public const string Times = "Times";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string OrgStructureName = "OrgStructureName";
            public const string CourseName = "CourseName";
            public const string ClassName = "ClassName";
            public const string Code = "Code";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";

        }
    }
    public class Tra_TraineeScoreSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_ProfileID)]
        public string ProfileName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_CodeEmp)]
        public string CodeEmp { get; set; }

        [DisplayName(ConstantDisplay.HRM_Att_Report_OrgName)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Certificate_CourseID)]
        public Guid? CourseID { get; set; }

        public int? MinimumScore { get; set; }

        public int? MaximumScore { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_ClassID)]
        public Guid? ClassID { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
    }
    
}
