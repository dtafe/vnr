using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_InterviewEntity : HRMBaseModel
    {
        public Nullable<int> CandidateNumber { get; set; }
        public string LanguageCode { get; set; }
        public Guid? QuestionQuestionaireID { get; set; }
        public string Code { get; set; }
        public string InterviewName { get; set; }
        public DateTime? DateInterview { get; set; }
        public string Status { get; set; }
        public Guid? CandidateID { get; set; }
        public Guid? InterviewCompaignID { get; set; }
        public string Interviewer1 { get; set; }
        public string Interviewer2 { get; set; }
        public Guid? ProfileID1 { get; set; }
        public Guid? ProfileID2 { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public double? Score1 { get; set; }
        public double? Score2 { get; set; }
        public double? Score { get; set; }
        public Guid? UserApproveID { get; set; }
        public string Hour { get; set; }
        public string Address { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
        public string FileAttachment { get; set; }
        public Int32? LevelInterview { get; set; }
        public string ResultInterview { get; set; }
        public string ResultInterviewView { get; set; }
        public Guid? InterviewCampaignDetailID { get; set; }
        public Guid? JobVacancyID { get; set; }
        public string CandidateName { get; set; }
        public string PositionName { get; set; }
        public string Totalinterview { get; set; }
        public string Totalcomment { get; set; }
        public string TagName { get; set; }
        public string CodeCandidate { get; set; }
        public Guid? TagID { get; set; }
        public Guid? RecruitmentHistoryID { get; set; }
        public Guid? GroupConditionID { get; set; }
        public string GroupName { get; set; }
        public string Interviewer3 { get; set; }
        public double? Score3 { get; set; }
        public Nullable<System.Guid> LanguageID { get; set; }
        public string LanguageName { get; set; }
    }
}
