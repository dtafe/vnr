using System;


namespace HRM.Business.Training.Models
{
    public class Tra_CertificateEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string CertificateName { get; set; }
        public string Code { get; set; }
        public string Place { get; set; }
        public Nullable<System.Guid> CourseID { get; set; }
        public string CourseName { get; set; }
        public Nullable<System.Guid> TopicID { get; set; }
        public string TopicName { get; set; }
        public string Description { get; set; }

        public string UnitTraining { get; set; }
    }

    public class Tra_CertificateMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string CertificateName { get; set; }
    }

}

