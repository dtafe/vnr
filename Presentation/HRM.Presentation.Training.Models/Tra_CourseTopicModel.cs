using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;

namespace HRM.Presentation.Training.Models
{
    public class Tra_CourseTopicModel : BaseViewModel
    {

        [DisplayName(ConstantDisplay.HRM_Tra_CourseTopic_Code)]
        public string Code { get; set; }
        public string TopicName { get; set; }
        public Guid TopicID { get; set; }

        public Guid CourseID { get; set; }
      
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string TopicName = "TopicName";
            public const string Code = "Code";
            public const string CourseName = "CourseName";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }
    public class Tra_CourseTopicMultiByCourseIDModel
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string TopicName { get; set; }
    }
 

}
