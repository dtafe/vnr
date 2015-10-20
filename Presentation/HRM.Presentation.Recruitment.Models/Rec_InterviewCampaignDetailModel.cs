using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Recruitment.Models
{
    public class Rec_InterviewCampaignDetailModel : BaseViewModel
    {
        public Nullable<System.DateTime> DateInterview1 { get; set; }
        public Nullable<System.DateTime> DateInterview2 { get; set; }
        public Nullable<System.DateTime> DateInterview3 { get; set; }
        public Nullable<System.DateTime> DateInterview4 { get; set; }
        public Nullable<System.DateTime> DateInterview5 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_InterviewCampaignDetail_CandidateNumber)]
        public Nullable<int> CandidateNumber { get; set; }
        public string LanguageCode1 { get; set; }
        public string LanguageCode2 { get; set; }
        public string LanguageCode3 { get; set; }
        public string LanguageCode4 { get; set; }
        public string LanguageCode5 { get; set; }
        public double? Score1_1 { get; set; }
        public double? Score1_2 { get; set; }
        public double? Score1_3 { get; set; }
        public string KQ1 { get; set; }

        public double? Score2_1 { get; set; }
        public double? Score2_2 { get; set; }
        public double? Score2_3 { get; set; }
        public string KQ2 { get; set; }

        public double? Score3_1 { get; set; }
        public double? Score3_2 { get; set; }
        public double? Score3_3 { get; set; }
        public string KQ3 { get; set; }

        public double? Score4_1 { get; set; }
        public double? Score4_2 { get; set; }
        public double? Score4_3 { get; set; }
        public string KQ4 { get; set; }

        public double? Score5_1 { get; set; }
        public double? Score5_2 { get; set; }
        public double? Score5_3 { get; set; }
        public string KQ5 { get; set; } 
        public string SourceAdsName { get; set; }
        public string EducationLevelName { get; set; }
        public Nullable<double> LevelEye { get; set; }
        public Nullable<double> LevelEyeRight { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_InterviewCampaignDetail_InterviewCampaignID)]
        public Guid? InterviewCampaignID { get; set; }
        public string InterviewCampaignName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_InterviewCampaignDetail_CandidateID)]
        public Guid? CandidateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_InterviewCampaignDetail_CodeCandidate)]
        public string CodeCandidate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_InterviewCampaignDetail_CandidateName)]
        public string CandidateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_InterviewCampaignDetail_DateInterview)]
        public DateTime? DateInterview { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_InterviewCampaignDetail_HourFrom)]
        [DataType(DataType.Time)]
        public DateTime? HourFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_InterviewCampaignDetail_HourTo)]
        [DataType(DataType.Time)]
        public DateTime? HourTo { get; set; }
        public string listCandidateIds { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Gender { get; set; }
        public string GenderView { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public Guid? OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        [MaxLength(150)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_JobVacancyIDs)]
        public string JobVacancyName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_JobVacancyID)]
        public string JobVacancyCode { get; set; }

        //yeu cau tuyen

        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_JobVacancyIDs)]
        public Guid? JobVacancyID { get; set; }

        [DisplayName(ConstantDisplay.HRM_REC_Candidate_Position)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_Position)]
        public string PositionName { get; set; }

        [DisplayName(ConstantDisplay.HRM_REC_Candidate_LevelInterview)]
        public Nullable<int> LevelInterview { get; set; }

        public System.DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string TAddress { get; set; }
        public string Status { get; set; }
        public Guid? RecruitmentHistoryID { get; set; }

        public string IdentifyNumber { get; set; }
        public Nullable<System.DateTime> IDDateOfIssue { get; set; }
        public string IDPlaceOfIssue { get; set; }
        public string PAddress { get; set; }
        public string Specialisation { get; set; }
        public string SkillLevel { get; set; }
        public string PositionCode { get; set; }
        public Double? Height { get; set; }
        public Double? Weight { get; set; }
        public Double? BloodPressure { get; set; }
        public Double? HeartBeat { get; set; }
        public Double? Musculoskeletal { get; set; }
        public string GraduateSchool { get; set; }
        public DateTime? YearGraduation { get; set; }
        public Double? WriteTest { get; set; }
        public Double? Score1 { get; set; }
        public Double? Score2 { get; set; }
        public DateTime? DateExam { get; set; }
        public string HealthStatus { get; set; }
        public string MarriageStatus { get; set; }
        public string GraduatedLevelName { get; set; }
        public string Assessment { get; set; }
        public string Strong { get; set; }
        public string Weak { get; set; }
        public string OrientationCareer { get; set; }
        public string PersonalTarget { get; set; }
        public string PersonalPlan { get; set; }
        public string SpecialSkill { get; set; }
        public string Description { get; set; }

        public partial class FieldNames
        {
            public const string CandidateNumber = "CandidateNumber";
            public const string IdentifyNumber = "IdentifyNumber";
            public const string IDDateOfIssue = "IDDateOfIssue";
            public const string IDPlaceOfIssue = "IDPlaceOfIssue";
            public const string PAddress = "PAddress";
            public const string ID = "ID";
            public const string InterviewCampaignID = "InterviewCampaignID";
            public const string InterviewCampaignName = "InterviewCampaignName";
            public const string CandidateID = "CandidateID";
            public const string DateInterview = "DateInterview";
            public const string HourFrom = "HourFrom";
            public const string HourTo = "HourTo";
            public const string CodeCandidate = "CodeCandidate";
            public const string CandidateName = "CandidateName";
            public const string DateFrom = "DateFrom";
            public const string DateTo = "DateTo";
            public const string Gender = "Gender";
            public const string GenderView = "GenderView";
            public const string OrgStructureID = "OrgStructureID";
            public const string JobVacancyName = "JobVacancyName";
            public const string JobVacancyCode = "JobVacancyCode";
            public const string PositionCode = "PositionCode";
            public const string PositionName = "PositionName";
            public const string LevelInterview = "LevelInterview";
            public const string DateOfBirth = "DateOfBirth";
            public const string Email = "Email";
            public const string Mobile = "Mobile";
            public const string Phone = "Phone";
            public const string TAddress = "TAddress";
            public const string Status = "Status";
            public const string RecruitmentHistoryID = "RecruitmentHistoryID";


            public const string Specialisation = "Specialisation";
            public const string SkillLevel = "SkillLevel";
            public const string Height = "Height";
            public const string Weight = "Weight";
            public const string BloodPressure = "BloodPressure";
            public const string HeartBeat = "HeartBeat";
            public const string Musculoskeletal = "Musculoskeletal";
            public const string GraduateSchool = "GraduateSchool";
            public const string YearGraduation = "YearGraduation";
            public const string WriteTest = "WriteTest";
            public const string Score1 = "Score1";
            public const string Score2 = "Score2";
            public const string DateExam = "DateExam";
            public const string HealthStatus = "HealthStatus";
            public const string MarriageStatus = "MarriageStatus";
            public const string GraduatedLevelName = "GraduatedLevelName";
            public const string Assessment = "Assessment";
            public const string Strong = "Strong";
            public const string Weak = "Weak";
            public const string OrientationCareer = "OrientationCareer";
            public const string PersonalTarget = "PersonalTarget";
            public const string PersonalPlan = "PersonalPlan";
            public const string SpecialSkill = "SpecialSkill";
            public const string Description = "Description";
        }
    }
    public class Rec_InterviewCampaignDetailSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Recruitment_InterviewCampaignDetail_CandidateName)]
        public string CandidateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_InterviewCampaignDetail_DateInterview)]
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string CodeCandidate { get; set; }
        public Guid? RankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_JobVacancyID)]
        public Nullable<System.Guid> JobVacancyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_Position)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_LevelInterview)]
        public int? LevelInterview { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_WorkPlaceName)]
        public Guid? WorkPlaceID { get; set; }
    }
}
