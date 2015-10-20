using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_ClassModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_Class_ClassName)]
        public string ClassName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_TimeTraining)]
        public Nullable<System.DateTime> StartDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_EndDate)]
        public Nullable<System.DateTime> EndDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_Teacher)]
        public string Teacher { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_Teacher)]
        public Guid? TeacherID { get; set; }
        public string TeacherName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_PlaneName)]
        public Nullable<System.Guid> PlanID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_PlaneName)]
        public string PlanName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_CourseName)]
        public System.Guid CourseID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_CourseName)]
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_Topic)]
        public Guid? TopicID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_Topic)]
        public string TopicName { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_IsTrainingOut)]
        public bool IsTrainingOut { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_IsTrainingInside)]
        public bool IsTrainingInside { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_ContentTraining)]
        public string ContentTraining { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_TrainingPlace)]
        public string TrainingPlace { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_Schedule)]
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
        [DisplayName(ConstantDisplay.HRM_Tra_Class_MassNumProfileAttendanced)]
        public Nullable<int> MassNumProfileAttendanced { get; set; }
        public string MassResult { get; set; }
        public string MassAttendedSection { get; set; }
        public Nullable<System.DateTime> DateReminder2 { get; set; }
        public string EmailReminder2 { get; set; }

        public string AttachedFiles { get; set; }
        public string AmountUnit { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_TotalAmount)]
        public Nullable<double> TotalAmount { get; set; }
        public string ClassType { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_Formula)]
        public string Formula { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_RankGroupName)]
        public Nullable<System.Guid> RankingGroupID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_RankGroupName)]
        public string RankGroupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_ScheduleTimeStart)]
        public Nullable<System.DateTime> ScheduleTimeStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_ScheduleTimeEnd)]
        public Nullable<System.DateTime> ScheduleTimeEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_TypeHandleFomular)]
        public string TypeHandleFomular { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_StandardScoreToPass)]
        public Nullable<double> StandardScoreToPass { get; set; }
        public Nullable<bool> IsAutoCreated { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_TypeHandleFomularNormal)]
        public bool TypeHandleFomularNormal { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_TypeHandleFomularAverage)]
        public bool TypeHandleFomularAverage { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_CertificateName)]
        public Nullable<System.Guid> CertificateID { get; set; }
        public string CertificateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_ExpectedCost)]
        public Nullable<double> ExpectedCost { get; set; }
        public string PlaceTrainner { get; set; }
        public Nullable<System.Guid> CurrencyID2 { get; set; }
        public Guid? ClassIDTemp { get; set; }

        public Nullable<System.Guid> ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Training_TrainingRequirements)]
        public Guid? RequirementTrainID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_RequirementTrainNames)]
        public string RequirementTrainName { get; set; }
        private Guid _id = Guid.Empty;
        public Guid Class_ID
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

        [DisplayName(ConstantDisplay.HRM_Tra_Course_IsComplex)]
        public Nullable<bool> IsComplex { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_IsNormal)]
        public Nullable<bool> IsNormal { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_IsTypeHandleFomular)]
        public bool IsTypeHandleFomular { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_IsTypeHandleFomularAuto)]
        public bool IsTypeHandleFomularAuto { get; set; }
        public string TrainerOtherList { get; set; }
        public string TrainerOtherListCode { get; set; }

        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string TrainerOtherList = "TrainerOtherList";
            public const string ClassName = "ClassName";
            public const string StartDate = "StartDate";
            public const string EndDate = "EndDate";
            public const string Teacher = "Teacher";
            public const string PlanName = "PlanName";
            public const string CourseName = "CourseName";
            public const string CourseCode = "CourseCode";
            public const string Topic = "Topic";
            public const string Code = "Code";
            public const string OrgStructureName = "OrgStructureName";
            public const string IsTrainingOut = "IsTrainingOut";
            public const string ContentTraining = "ContentTraining";
            public const string TrainingPlace = "TrainingPlace";
            public const string Schedule = "Schedule";
            public const string DateReminder = "DateReminder";
            public const string EmailReminder = "EmailReminder";
            public const string DatePlanStart = "DatePlanStart";
            public const string DatePlanEnd = "DatePlanEnd";
            public const string Status = "Status";
            public const string Duration = "Duration";
            public const string CurrencyName1 = "CurrencyName1";
            public const string CurrencyName2 = "CurrencyName2";
            public const string Amount = "Amount";
            public const string IsDuringWork = "IsDuringWork";
            public const string MassNumProfileRegisted = "MassNumProfileRegisted";
            public const string MassNumProfileAttendanced = "MassNumProfileAttendanced";
            public const string MassResult = "MassResult";
            public const string MassAttendedSection = "MassAttendedSection";
            public const string DateReminder2 = "DateReminder2";
            public const string EmailReminder2 = "EmailReminder2";
            public const string TopicName = "TopicName";
            public const string RankGroupName = "RankGroupName";
            public const string AmountUnit = "AmountUnit";
            public const string TotalAmount = "TotalAmount";
            public const string ClassType = "ClassType";
            public const string Formula = "Formula";
            public const string ScheduleTimeStart = "ScheduleTimeStart";
            public const string ScheduleTimeEnd = "ScheduleTimeEnd";
            public const string TypeHandleFomular = "TypeHandleFomular";
            public const string StandardScoreToPass = "StandardScoreToPass";
            public const string IsAutoCreated = "IsAutoCreated";
            public const string CertificateName = "CertificateName";
            public const string ExpectedCost = "ExpectedCost";
            public const string PlaceTrainner = "PlaceTrainner";
            public const string IsTrainingInside = "IsTrainingInside";
            public const string TeacherName = "TeacherName";
            public const string TraineeCertificateID = "TraineeCertificateID";
            public const string RequirementTrainName = "RequirementTrainName";
        }
    }

    public class Tra_ClassSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_Class_ClassName)]
        public string ClassName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Class_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Class_CourseName)]
        public Nullable<System.Guid> CourseID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Class_StartDate)]
        public Nullable<System.DateTime> StartDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Class_EndDate)]
        public Nullable<System.DateTime> EndDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Class_ScheduleTimeStart)]
        public Nullable<System.DateTime> ScheduleTimeStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Class_ScheduleTimeEnd)]
        public Nullable<System.DateTime> ScheduleTimeEnd { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
        [DisplayName(ConstantDisplay.HRM_Training_TrainingRequirements)]
        public string RequirementTrainID { get; set; }
    }

    public class Tra_ClassInSideSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_Class_ClassName)]
        public string ClassName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_Code)]
        public string Code { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Tra_ClassFinishSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_Class_ClassName)]
        public string ClassName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_Code)]
        public string Code { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Tra_ClassProcessInSideSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_Class_ClassName)]
        public string ClassName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Course_Code)]
        public string Code { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Tra_ClassMultiModel
    {
        public Guid ID { get; set; }
        public string ClassName { get; set; }
        public int TotalRow { get; set; }
    }

}
