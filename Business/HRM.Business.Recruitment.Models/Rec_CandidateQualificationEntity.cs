using System;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_CandidateQualificationEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string QualificationName { get; set; }
        public Nullable<System.Guid> QualificationTypeID { get; set; }
        public string TrainingPlace { get; set; }
        public string TrainingAddress { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateFinish { get; set; }
        public string Rank { get; set; }
        public Nullable<System.Guid> SpecialLevelID { get; set; }
        public string CertificateName { get; set; }
        public Nullable<System.DateTime> GraduationDate { get; set; }
        public string FieldOfTraining { get; set; }
        public string Comment { get; set; }
        public Nullable<System.Guid> CandidateID { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public Nullable<System.Guid> NameEntityID { get; set; }
        public Nullable<int> MonthLearn { get; set; }
    }
}
