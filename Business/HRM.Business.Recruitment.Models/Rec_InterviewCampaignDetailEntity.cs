using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_InterviewCampaignDetailEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public Nullable<System.DateTime> DateInterview1 { get; set; }
        public Nullable<System.DateTime> DateInterview2 { get; set; }
        public Nullable<System.DateTime> DateInterview3 { get; set; }
        public Nullable<System.DateTime> DateInterview4 { get; set; }
        public Nullable<System.DateTime> DateInterview5 { get; set; }
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
        public string EducationLevelName { get; set; }
        public Nullable<System.Guid> InterviewCampaignID { get; set; }
        public string InterviewCampaignName { get; set; }
        public Nullable<System.Guid> CandidateID { get; set; }
        public Nullable<System.DateTime> DateInterview { get; set; }
        public Nullable<System.DateTime> HourFrom { get; set; }
        public Nullable<System.DateTime> HourTo { get; set; }
        public string listCandidateIds { get; set; }
        public string CodeCandidate { get; set; }
        public string CandidateName { get; set; }
        public string OrgStructureName { get; set; }

        public Guid? JobVacancyID { get; set; }
        public string JobVacancyName { get; set; }

        public Guid? PositionID { get; set; }
        public string PositionName { get; set; }
        public Nullable<int> LevelInterview { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
        public string Gender { get; set; }
        public string GenderView { get; set; }
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

        public string MarriageStatus { get; set; }

        public string PositionCode { get; set; }
        public string PlaceOfBirth { get; set; }

        public string Specialisation { get; set; }
        public string SkillLevel { get; set; }
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
        public string GraduatedLevelName { get; set; }
        public string Assessment { get; set; }
        public string Strong { get; set; }
        public string Weak { get; set; }
        public string OrientationCareer { get; set; }
        public string PersonalTarget { get; set; }
        public string PersonalPlan { get; set; }
        public string SpecialSkill { get; set; }
        public string Description { get; set; }

        public Nullable<double> LevelEye { get; set; }
        public Nullable<double> LevelEyeRight { get; set; }
        public string SourceAdsName { get; set; }
    }
}
