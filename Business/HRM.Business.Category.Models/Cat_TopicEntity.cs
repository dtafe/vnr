using System;


namespace HRM.Business.Category.Models
{
    public class Cat_TopicEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string TopicName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public Nullable<System.Guid> CorseID { get; set; }
        public string CourseName { get; set; }
        public Nullable<System.Guid> CertificateID { get; set; }
        public string CertificateName { get; set; }
        public Nullable<double> LeadTime { get; set; }
        public string DocumentCode { get; set; }
        public string TrainingSample { get; set; }
        public string TrainingCode { get; set; }
        public Nullable<double> StandardTheory { get; set; }
        public Nullable<double> StandardPractice { get; set; }
        public Nullable<double> StandardAttendance { get; set; }
        public string CodeConstraint { get; set; }
        public string Formula { get; set; }
        public Nullable<System.Guid> RankingID { get; set; }
        public string RankingName { get; set; }
        public Nullable<System.Guid> RankingGroupID { get; set; }
        public string RankingGroupName { get; set; }
        public string TypeHandleFomular { get; set; }
        public Nullable<bool> IsComplex { get; set; }
        public Nullable<double> MinimumScore { get; set; }
        public Nullable<double> MaximumScore { get; set; }
        public Nullable<bool> IsNormal { get; set; }
        public bool IsTypeHandleFomular { get; set; }
        public bool IsTypeHandleFomularAuto { get; set; }
        public Nullable<System.Guid> ScoreTypeID { get; set; }
        private Guid _id = Guid.Empty;
        public Guid Topic_ID
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
        public string ScoreType { get; set; }

    }

    public class Cat_TopicMultiEntity
    {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string TopicName { get; set; }
    }
}

