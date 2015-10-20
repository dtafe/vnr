using System;

namespace HRM.Business.Category.Models
{
    public class Cat_TrainLevelEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string TrainLevelName { get; set; }
        public double Expiredate { get; set; }
        public Nullable<System.Guid> CertificateID { get; set; }
        public string Description { get; set; }
    }

    public class Cat_TrainLevelMultiEntity {
        public int TotalRow { get; set; }
        public Guid ID { get; set; }
        public string TrainLevelName { get; set; }
    }
}
