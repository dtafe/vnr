using System;


namespace HRM.Business.Training.Models
{
    public class Tra_ReportClassEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public string ClassName { get; set; }

        public Nullable<System.DateTime> StartDate { get; set; }

        public Nullable<System.DateTime> EndDate { get; set; }

        public Nullable<System.DateTime> ScheduleTimeStart { get; set; }

        public Nullable<System.DateTime> ScheduleTimeEnd { get; set; }
        public string TrainingPlace { get; set; }
        public string TrainerOtherList { get; set; }
        public string TrainerOtherListCode { get; set; }
        public string Teacher { get; set; }
        public Guid? RequirementTrainID { get; set; }
        public string RequirementTrainName { get; set; }
        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string ClassName = "ClassName";
            public const string StartDate = "StartDate";
            public const string EndDate = "EndDate";
            public const string ScheduleTimeStart = "ScheduleTimeStart";
            public const string ScheduleTimeEnd = "ScheduleTimeEnd";
            public const string TrainingPlace = "TrainingPlace";
            public const string Teacher = "Teacher";
        }
 
    }
}

