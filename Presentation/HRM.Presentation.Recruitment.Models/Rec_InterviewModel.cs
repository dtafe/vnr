using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Recruitment.Models
{
    public class Rec_InterviewModel : BaseViewModel
    {
        public Nullable<int> CandidateNumber { get; set; }
        public string LanguageCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_QuestionQuestionaireID)]
        public Guid? QuestionQuestionaireID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_InterviewName)]
        public string InterviewName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_DateInterview)]
        public DateTime? DateInterview { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_CandidateID)]
        public Guid? CandidateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_InterviewCompaignID)]
        public Guid? InterviewCompaignID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Interviewer1)]
        public string Interviewer1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Interviewer2)]
        public string Interviewer2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_ProfileID1)]
        public Guid? ProfileID1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_ProfileID2)]
        public Guid? ProfileID2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Comment1)]
        public string Comment1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Comment2)]
        public string Comment2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Score1)]
        public double? Score1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Score2)]
        public double? Score2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Score)]
        public double? Score { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_UserApproveID)]
        public Guid? UserApproveID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Hour)]
        public string Hour { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Address)]
        public string Address { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Comment)]
        public string Comment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_FileAttachment)]
        public string FileAttachment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_LevelInterview)]
        public Int32? LevelInterview { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_ResultInterview)]
        public string ResultInterview { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_ResultInterview)]
        public string ResultInterviewView { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_InterviewCampaignDetailID)]
        public Guid? InterviewCampaignDetailID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_JobVacancyID)]
        public Guid? JobVacancyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_JobVacancyName)]
        public Guid? JobVacancyName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_CodeEmp)]
        public string CodeEmp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_CandidateName)]
        public string CandidateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_PositionName)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Totalinterview)]
        public string Totalinterview { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Totalcomment)]
        public string Totalcomment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_TagName)]
        public string TagName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_CodeCandidate)]
        public string CodeCandidate { get; set; }
        public Guid? TagID { get; set; }
        public Guid? RecruitmentHistoryID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_GroupConditionID)]
        public Guid? GroupConditionID { get; set; }
        public string GroupName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_ConditionalColor_Condition)]
        public string Condition { get; set; }
        public string ConditionTemp { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Interviewer3)]
        public string Interviewer3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Score3)]
        public double? Score3 { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_Interview_LanguageID)]
        public Nullable<System.Guid> LanguageID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_LanguageID)]
        public string LanguageName { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string LanguageID = "LanguageID";
            public const string LanguageName = "LanguageName";
            public const string Interviewer3 = "Interviewer3";
            public const string Score3 = "Score3";
            public const string QuestionQuestionaireID = "QuestionQuestionaireID";
            public const string Code = "Code";
            public const string InterviewName = "InterviewName";
            public const string DateInterview = "DateInterview";
            public const string Status = "Status";
            public const string CandidateID = "CandidateID";
            public const string InterviewCompaignID = "InterviewCompaignID";
            public const string Interviewer1 = "Interviewer1";
            public const string Interviewer2 = "Interviewer2";
            public const string ProfileID1 = "ProfileID1";
            public const string ProfileID2 = "ProfileID2";
            public const string Comment1 = "Comment1";
            public const string Comment2 = "Comment2";
            public const string Score1 = "Score1";
            public const string Score2 = "Score2";
            public const string Score = "Score";
            public const string UserApproveID = "UserApproveID";
            public const string Hour = "Hour";
            public const string Address = "Address";
            public const string Comment = "Comment";
            public const string Description = "Description";
            public const string FileAttachment = "FileAttachment";
            public const string LevelInterview = "LevelInterview";
            public const string ResultInterview = "ResultInterview";
            public const string ResultInterviewView = "ResultInterviewView";
            public const string InterviewCampaignDetailID = "InterviewCampaignDetailID";
            public const string JobVacancyID = "JobVacancyID";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string CandidateName = "CandidateName";
            public const string PositionName = "PositionName";
            public const string Totalinterview = "Totalinterview";
            public const string Totalcomment = "Totalcomment";
            public const string TagName = "TagName";
            public const string CodeCandidate = "CodeCandidate";
            public const string TagID = "TagID";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
            public const string RecruitmentHistoryID = "RecruitmentHistoryID";
            public const string GroupConditionID = "GroupConditionID";
            public const string GroupName = "GroupName";
            public const string CandidateNumber = "CandidateNumber";
        }
    }
    public class Rec_InterviewSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_CandidateName)]
        public string CandidateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_TagName)]
        public Guid? TagID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_CodeCandidate)]
        public string CodeCandidate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateFrom)]
        public DateTime? DateFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Att_Report_DateTo)]
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_JobVacancyID)]
        public Guid? JobVacancyID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IdentifyNumber { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CellPhone)]
        public string Mobile { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_Position)]
        public Guid? PositionID { get; set; }
        // Ngày sinh từ ngày đến ngày      
        public DateTime? DateOfBirthFrom { get; set; }
        public DateTime? DateOfBirthTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_TAProvince)]
        public Guid? TProvinceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_Status)]
        public string Status { get; set; }
        // Ngày bắt đầu đi làm
        public DateTime? WorkDateFrom { get; set; }
        public DateTime? WorkDateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RankID)]
        public Nullable<System.Guid> RankID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
