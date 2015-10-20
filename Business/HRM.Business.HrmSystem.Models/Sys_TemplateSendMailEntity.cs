using System;


namespace HRM.Business.HrmSystem.Models
{
     public class Sys_TemplateSendMailEntity
    {
        public System.Guid ID { get; set; }
        public string TemplateName { get; set; }
        public string Description { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
    }
}
