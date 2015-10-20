using HRM.Infrastructure.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Presentation.HrmSystem.Models
{
  
     public class Sys_TemplateSendMailModel : BaseViewModel
    {
       [DisplayName(ConstantDisplay.HRM_Sys_TemplateSendMail_TemplateName)]
        public string TemplateName { get; set; }
       [DisplayName(ConstantDisplay.HRM_HR_Discipline_DescriptionOfViolation)]
        public string Description { get; set; }
       [DisplayName(ConstantDisplay.HRM_Sys_TemplateSendMail_Subject)]
         public string Subject { get; set; }
       [DisplayName(ConstantDisplay.HRM_Sys_TemplateSendMail_Content)]
       public string Content { get; set; }
       [DisplayName(ConstantDisplay.HRM_Sys_TemplateSendMail_Type)]
       public string Type { get; set; }

        public partial class FieldNames
        {
            public const string TemplateName = "TemplateName";
            public const string Description = "Description";
            public const string Subject = "Subject";
            public const string Notes = "Notes";
            public const string ServerUpdate = "ServerUpdate";
            public const string ServerCreate = "ServerCreate";
            public const string UserUpdate = "UserUpdate";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
         
        }
    }
    public class Sys_TemplateSendMailSearchModel
    {
        public string TemplateName { get; set; }
    }

    public class Sys_TemplateSendMailMultiModel
    {
        public Guid ID { get; set; }
        public string TemplateName { get; set; }
    }
}
