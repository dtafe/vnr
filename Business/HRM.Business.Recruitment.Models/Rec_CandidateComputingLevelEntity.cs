using System;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_CandidateComputingLevelEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        //public System.Guid ID { get; set; }
        public System.Guid CandidateID { get; set; }
        public System.Guid SpecialTypeID { get; set; }
        public string CandidateComputingLevelName { get; set; }
        public System.Guid SpecialLevelID { get; set; }
        public string Code { get; set; }
 
        public Nullable<System.Guid> UserLockID { get; set; }
        public Nullable<System.DateTime> DateLock { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string IPCreate { get; set; }
        public string IPUpdate { get; set; }
        public string Description { get; set; }
        public string PlaceTranning { get; set; }

        public string ComputingType { get; set; }

        public string ComputingLevel { get; set; }
    
    }
}
