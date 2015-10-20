using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_RequirementTrainDetailModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrainDetail_RequirementTrainID)]
        public Nullable<System.Guid> RequirementTrainID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrainDetail_RequirementTrainName)]
        public string RequirementTrainName { get; set; }
        public Nullable<System.Guid> PlanID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrainDetail_ProfileName)]
        public Nullable<System.Guid> ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrainDetail_ProfileName)]
        public string ProfileName { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public Nullable<System.Guid> ClassID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrainDetail_CourseID)]
        public Nullable<System.Guid> CourseID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrainDetail_CourseName)]
        public string CourseName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrainDetail_TopicID)]
        public Nullable<System.Guid> TopicID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrainDetail_TopicName)]
        public string TopicName { get; set; }
        public string TopicXML { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrainDetail_DateStart)]
        public Nullable<System.DateTime> DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrainDetail_DateEnd)]
        public Nullable<System.DateTime> DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Tra_RequirementTrainDetail_STT)]
        public string STT { get; set; }

        public string CodeEmp { get; set; }
    
        public partial class FieldNames
        {
            public const string STT = "STT";
            public const string ProfileName = "ProfileName";
            public const string CourseName = "CourseName";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string Description = "Description";
            public const string CodeEmp = "CodeEmp";
        }
        public class Tra_TopicByCourseIDModel
        {
            public Guid ID { get; set; }
            public string TopicName { get; set; }
            public int TotalRow { get; set; }
        }
    
       
    }
}
