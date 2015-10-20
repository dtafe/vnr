using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_RequirementTrainModel : BaseViewModel
    {
        public bool? isAnalysis { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileID)]
        public System.Guid ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileCode)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Payroll_UnusualED_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Plan_Code)]
        public string PlanCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_PlanName)]
        public string PlanName { get; set; }
         [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_PlanName)]
        public Nullable<System.Guid> PlanID { get; set; }
        public Nullable<System.Guid> TopicID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_DateFrom)]
        public Nullable<System.DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_DateTo)]
        public Nullable<System.DateTime> DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_UserApproveName)]
        public string UserApproveName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_UserApproveID)]
        public Nullable<System.Guid> UserApproveID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_Comment)]
        public string Comment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_Status)]
        public string StatusView { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_RequirementTrainName)]
        public string RequirementTrainName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_CourseName)]
        public Nullable<System.Guid> CourseID { get; set; }
        public Nullable<System.DateTime> DateApprove { get; set; }
        public string ApproveXML { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_Code)]
        public string Code { get; set; }
        public Nullable<System.Guid> TrainCategoryID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_Target)]
        public string Target { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_ResultAchieved)]
        public string ResultAchieved { get; set; }
        public DateTime? DateSeniority { get; set; }
        public string CreatedPosition { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_UserApproveName2)]
        public string UserApproveName2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_UserApproveID2)]
        public Nullable<System.Guid> UserApproveID2 { get; set; }
        public Nullable<System.DateTime> DateApprove2 { get; set; }
        public string CommentApprove2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_UserApproveName3)]
        public string UserApproveName3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_UserApproveID3)]
        public Nullable<System.Guid> UserApproveID3 { get; set; }
        public Nullable<System.DateTime> DateApprove3 { get; set; }
        public string CommentApprove3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_IsTrainingOutside)]
        public Nullable<bool> IsTrainingOutside { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_IsTrainingInside)]
        public Nullable<bool> IsTrainingInside { get; set; }
         [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_TrainingDeptIdea)]
        public string TrainingDeptIdea { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_ManagementDeptIdea)]
        public string ManagementDeptIdea { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_StatusApprove)]
        public string StatusApprove { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_StatusApprove2)]
        public string StatusApprove2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_StatusApprove3)]
        public string StatusApprove3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_PersonRequirement)]
        public string PersonRequirement { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_Notes)]
        public string Notes { get; set; }
        private Guid _id = Guid.Empty;
        public Guid RequirementTrain_ID
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
        public string lstCodeEmp { get; set; }
        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string RequirementTrainName = "RequirementTrainName";
            public const string PlanCode = "PlanCode";
            public const string PlanName = "PlanName";
            public const string Status = "Status";
            public const string StatusView = "StatusView";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
        }
    }

    public class Tra_RequirementTrainSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_RequirementTrainName)]
        public string RequirementTrainName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_DateFrom)]
        public Nullable<System.DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_DateTo)]
        public Nullable<System.DateTime> DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_Plan)]

        public string PlanIDs { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Tra_RequirementTrainInSideSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_RequirementTrainName)]
        public string RequirementTrainName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_DateFrom)]
        public Nullable<System.DateTime> DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_DateTo)]
        public Nullable<System.DateTime> DateTo { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }


}
