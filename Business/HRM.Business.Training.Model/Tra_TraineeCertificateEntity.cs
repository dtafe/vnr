using System;


namespace HRM.Business.Training.Models
{
    public class Tra_TraineeCertificateEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Nullable<System.Guid> TraineeID { get; set; }
        public string TraineeName { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
        public Nullable<System.Guid> CertificateID { get; set; }
        public string CertificateName { get; set; }
        public Nullable<System.DateTime> DateCertificate { get; set; }
        public Nullable<System.DateTime> DateExpireCertificate { get; set; }
        public string PlaceCertificate { get; set; }
        public string Note { get; set; }
        public string UserApprove { get; set; }
        public Nullable<System.DateTime> DateApprove { get; set; }
        public Nullable<bool> IsNonWaring { get; set; }
        public string OrgStructureName { get; set; }
        public string CourseName { get; set; }
        public string ClassName { get; set; }
        public string TeacherName { get; set; }
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

    }
}

