using System;


namespace HRM.Business.Training.Models
{
    public class Tra_CourseEntity : HRM.Business.BaseModel.HRMBaseModel
    {

        public string Code { get; set; }
        public string Place { get; set; }
        public Nullable<System.Guid> TrainCategoryID { get; set; }
        public Nullable<System.Guid> RankingGroupID { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public string TrainCategoryName { get; set; }
        public string RankGroupName { get; set; }
        public string SchoolFee { get; set; }
        public string Category { get; set; }
        public Nullable<bool> IsExternal { get; set; }
        public string TrainningPlace { get; set; }
        public string StandardTheory { get; set; }
        public string StandardPractice { get; set; }
        public string StandardAttendance { get; set; }
        public Nullable<bool> IsComplex { get; set; }
        public string Formula { get; set; }
        public string TypeHandleFomular { get; set; }
        public Nullable<bool> IsAutoCreated { get; set; }
        public Nullable<double> MinimumScore { get; set; }
        public Nullable<double> MaximumScore { get; set; }

        public string SalaryClassName { get; set; }
        public Nullable<System.Guid> RankID { get; set; }
        public Nullable<int> Seniority { get; set; }
        public Nullable<int> TimeOnCurrentRank { get; set; }
        public string CourseName1 { get; set; }
        public Nullable<System.Guid> CourseID { get; set; }
        public string CourseListID { get; set; }
        public int? TimeCourse { get; set; }
        public int? LotsOfClassMin { get; set; }
        public int? LotsOfClassMax { get; set; }
        public bool? IsAttendance { get; set; }
        public int? ScoreStandard { get; set; }
        public Guid? JobTitleID { get; set; }
        public string RankListID { get; set; }
        public string RankTrainerListID { get; set; }
        public Guid? JobTitleTrainerID { get; set; }
        public Guid? OrgTrainerID { get; set; }
        public int? SeniorityTrainer { get; set; }
        public int? TimeOnCurrentRankTrainer { get; set; }
        public string CourseListTrainerID { get; set; }
        public Guid? OrgStructureID { get; set; }
        public Guid? RankTrainnerID { get; set; }

        public string JobTitleName { get; set; }
        public string JobTitleTrainerName {get;set;}
        public string OrgStructureName{get;set;}
        public string OrgStructureTrainerName{get;set;}

        public Guid? PlanID { get; set; }
        public Guid? RequirementTrainID { get; set; }
        public string OrgListCode1 { get; set; }
        public string JobTitleListCode1 { get; set; }
        public string OrgListTrainerCode { get; set; }
        public string JobTitleListTrainerCode { get; set; }
        public Nullable<double> DurationMinCourse { get; set; }
        public Nullable<double> DurationMinTrainerCourse { get; set; }
        public int? ExamTimes { get; set; }
        public Nullable<double> MaxSeniority { get; set; }
        public Nullable<double> MaxSeniorityTrainer { get; set; }
        public string StatusCourse { get; set; }
        public string StatusCourseView { get; set; }
        

    }

    public class Tra_CourseMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string CourseName { get; set; }
    }

}

