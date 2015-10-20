using System;

namespace HRM.Business.Training.Models
{
    public class Tra_TraineeScoreEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Nullable<System.Guid> TraineeTopicID { get; set; }
        public string TraineeTopicName { get; set; }
        public Nullable<System.Guid> ScoreTypeID { get; set; }
        public string ScoreTypeName { get; set; }
        public Nullable<double> Score { get; set; }
        public Nullable<double> TheoryScores { get; set; }
        public Nullable<double> PracticeScores { get; set; }
        public Nullable<double> AttendanceScores { get; set; }
        public string Type { get; set; }
        public Nullable<int> Times { get; set; }
        public Nullable<int> NumOrder { get; set; }

        public Nullable<Guid> TraineeID { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string OrgStructureName { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public string Code { get; set; }
        public string WorkPlaceName { get; set; }
        public string TypeError { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }
        public string Status { get; set; }
        public string StatusView { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? Result { get; set; }
        public Guid? OrgStructorId { get; set; }
        public string TrainLevelName { get; set; }
        public string RequirementTrainName { get; set; }
        
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string CodeEmp = "CodeEmp";
            public const string TraineeName = "TraineeName";
            public const string ProfileName = "ProfileName";
            public const string TypeError = "TypeError";

            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";



        }
    }
}

