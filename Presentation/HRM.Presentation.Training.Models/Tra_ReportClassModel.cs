using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_ReportClassModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_Class_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_ClassName)]
        public string ClassName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Class_StartDate)]
        public Nullable<System.DateTime> StartDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Class_EndDate)]
        public Nullable<System.DateTime> EndDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Class_ScheduleTimeStart)]
        public Nullable<System.DateTime> ScheduleTimeStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Class_ScheduleTimeEnd)]
        public Nullable<System.DateTime> ScheduleTimeEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_Class_TrainingPlace)]
        public string TrainingPlace { get; set; }

        public string TrainerOtherList { get; set; }
        public string TrainerOtherListCode { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Class_Teacher)]
        public string Teacher { get; set; }
        [DisplayName(ConstantDisplay.HRM_Training_TrainingRequirements)]
        public Guid? RequirementTrainID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrain_RequirementTrainNames)]
        public string RequirementTrainName { get; set; }
        public partial class FieldNames
        {
            public const string ClassName = "ClassName";
            public const string Code = "Code";
            public const string StartDate = "StartDate";
            public const string EndDate = "EndDate";
            public const string ScheduleTimeStart = "ScheduleTimeStart";
            public const string ScheduleTimeEnd = "ScheduleTimeEnd";
            public const string TrainingPlace = "TrainingPlace";
            public const string Teacher = "Teacher";
            public const string TraineeCertificateID = "TraineeCertificateID";
            public const string RequirementTrainName = "RequirementTrainName";
        }
    }
    public class Tra_ReportClassSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_Trainee_ClassID)]
        public Guid? ClassID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Class_StartDate)]
        public Nullable<System.DateTime> StartDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Class_EndDate)]
        public Nullable<System.DateTime> EndDate { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Class_ScheduleTimeStart)]
        public Nullable<System.DateTime> ScheduleTimeStart { get; set; }

        [DisplayName(ConstantDisplay.HRM_Tra_Class_ScheduleTimeEnd)]
        public Nullable<System.DateTime> ScheduleTimeEnd { get; set; }

        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        [DisplayName(ConstantDisplay.HRM_Training_TrainingRequirements)]
        public string RequirementTrainID { get; set; }
    }

 

}
