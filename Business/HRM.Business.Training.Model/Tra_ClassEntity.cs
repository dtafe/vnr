using System;


namespace HRM.Business.Training.Models
{
    public class Tra_ClassEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string ClassName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Teacher { get; set; }
        public string TeacherName { get; set; }
        public Nullable<System.Guid> PlanID { get; set; }
        public string PlanName { get; set; }
        public System.Guid CourseID { get; set; }
        public string CourseName { get; set; }
        public bool? IsComplex { get; set; }
        public string Code { get; set; }
        public string Topic { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        public bool IsTrainingOut { get; set; }
        public string ContentTraining { get; set; }
        public string TrainingPlace { get; set; }
        public string Schedule { get; set; }
        public Nullable<System.DateTime> DateReminder { get; set; }
        public string EmailReminder { get; set; }
        public Nullable<System.DateTime> DatePlanStart { get; set; }
        public Nullable<System.DateTime> DatePlanEnd { get; set; }
        public string Status { get; set; }
        public Nullable<double> Duration { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public string CurrencyName1 { get; set; }
        public string CurrencyName2 { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<bool> IsDuringWork { get; set; }
        public Nullable<int> MassNumProfileRegisted { get; set; }
        public Nullable<int> MassNumProfileAttendanced { get; set; }
        public string MassResult { get; set; }
        public string MassAttendedSection { get; set; }
        public Nullable<System.DateTime> DateReminder2 { get; set; }
        public string EmailReminder2 { get; set; }
        public string TopicName { get; set; }
        public string AttachedFiles { get; set; }
        public string AmountUnit { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public string ClassType { get; set; }
        public string Formula { get; set; }
        public Nullable<System.Guid> RankingGroupID { get; set; }
        public string RankGroupName { get; set; }
        public Nullable<System.DateTime> ScheduleTimeStart { get; set; }
        public Nullable<System.DateTime> ScheduleTimeEnd { get; set; }
        public string TypeHandleFomular { get; set; }
        public Nullable<double> StandardScoreToPass { get; set; }
        public Nullable<bool> IsAutoCreated { get; set; }
        public Nullable<System.Guid> CertificateID { get; set; }
        public string CertificateName { get; set; }
        public Nullable<double> ExpectedCost { get; set; }
        public string PlaceTrainner { get; set; }
        public Nullable<System.Guid> CurrencyID2 { get; set; }
        public string TrainerOtherList { get; set; }
        public Guid? TeacherID { get; set; }
        public string CourseCode { get; set; }
        public Guid? RequirementTrainID { get; set; }
        public string RequirementTrainName { get; set; }
    }

    public class Tra_ClassMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string ClassName { get; set; }
    }

}

