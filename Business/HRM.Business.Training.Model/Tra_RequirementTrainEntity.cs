using System;


namespace HRM.Business.Training.Models
{
    public class Tra_RequirementTrainEntity : HRM.Business.BaseModel.HRMBaseModel
    {

        public System.Guid ProfileID { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string PlanCode { get; set; }
        public string PlanName { get; set; }
        public Nullable<System.Guid> PlanID { get; set; }
        public Nullable<System.Guid> TopicID { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public string UserApproveName { get; set; }
        public Nullable<System.Guid> UserApproveID { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public string StatusView { get; set; }
        public string RequirementTrainName { get; set; }
        public Nullable<System.Guid> CourseID { get; set; }
        public Nullable<System.DateTime> DateApprove { get; set; }
        public string ApproveXML { get; set; }
        public string Code { get; set; }
        public Nullable<System.Guid> TrainCategoryID { get; set; }
        public string Target { get; set; }
        public string ResultAchieved { get; set; }
        public string CreatedPosition { get; set; }
        public string UserApproveName2 { get; set; }
        public Nullable<System.Guid> UserApproveID2 { get; set; }
        public Nullable<System.DateTime> DateApprove2 { get; set; }
        public string CommentApprove2 { get; set; }
        public string UserApproveName3 { get; set; }
        public Nullable<System.Guid> UserApproveID3 { get; set; }
        public Nullable<System.DateTime> DateApprove3 { get; set; }
        public string CommentApprove3 { get; set; }
        public Nullable<bool> IsTrainingOutside { get; set; }
        public string TrainingDeptIdea { get; set; }
        public string ManagementDeptIdea { get; set; }
        public string StatusApprove { get; set; }
        public string StatusApprove2 { get; set; }
        public string StatusApprove3 { get; set; }
        public string PersonRequirement { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
    }
}

