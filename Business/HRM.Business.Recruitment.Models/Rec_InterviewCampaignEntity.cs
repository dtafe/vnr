using System;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_InterviewCampaignEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Nullable<int> LevelInterview { get; set; }
        public string InterviewCampaignName { get; set; }
        public DateTime? HoldDate { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DateInterviewFrom { get; set; }
        public Nullable<System.DateTime> DateInterviewTo { get; set; }
        public Guid? CodeCandidate { get; set; }
        public string CandidateName { get; set; }
        public string listCandidateIds { get; set; } 
    }


}
