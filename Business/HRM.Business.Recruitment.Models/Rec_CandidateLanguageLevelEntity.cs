using System;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_CandidateLanguageLevelEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        //public System.Guid ID { get; set; }
        public Nullable<System.Guid> CandidateID { get; set; }
        public Nullable<System.Guid> SpecialTypeID { get; set; }
        public string CandidateLanguageLevelName { get; set; }
        public Nullable<System.Guid> SpecialLevelID { get; set; }
        public Nullable<System.Guid> SpecialSkillID { get; set; }
        public string Code { get; set; }
        public Nullable<System.Guid> UserLockID { get; set; }
        public Nullable<System.DateTime> DateLock { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string IPCreate { get; set; }
        public string IPUpdate { get; set; }
        public string PlaceTranning { get; set; }
        public string LanguageSkill { get; set; }
        public string LanguageType { get; set; }
        public string LanguageLevel { get; set; }
    }
}
