//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRM.Data.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rec_Candidate
    {
        public Rec_Candidate()
        {
            this.Cat_Attachment = new HashSet<Cat_Attachment>();
            this.Hre_CandidateHistory = new HashSet<Hre_CandidateHistory>();
            this.Hre_Profile = new HashSet<Hre_Profile>();
            this.Hre_StopWorking = new HashSet<Hre_StopWorking>();
            this.Rec_CandidateBusiness = new HashSet<Rec_CandidateBusiness>();
            this.Rec_CandidateComputingLevel = new HashSet<Rec_CandidateComputingLevel>();
            this.Rec_CandidateLanguageLevel = new HashSet<Rec_CandidateLanguageLevel>();
            this.Rec_CandidateQualification = new HashSet<Rec_CandidateQualification>();
            this.Rec_CandidateSourceAds = new HashSet<Rec_CandidateSourceAds>();
            this.Rec_InterviewCampaignDetail = new HashSet<Rec_InterviewCampaignDetail>();
            this.Rec_Interview = new HashSet<Rec_Interview>();
            this.Rec_InterviewHistory = new HashSet<Rec_InterviewHistory>();
            this.Rec_Investigation = new HashSet<Rec_Investigation>();
            this.Rec_RecruitmentHistory = new HashSet<Rec_RecruitmentHistory>();
            this.Rec_Reference = new HashSet<Rec_Reference>();
            this.Rec_Relative = new HashSet<Rec_Relative>();
            this.Rec_SoftSkill = new HashSet<Rec_SoftSkill>();
            this.Rec_SupplerRelation = new HashSet<Rec_SupplerRelation>();
            this.Rec_CandidateHistory = new HashSet<Rec_CandidateHistory>();
        }
    
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> SourceAdsID { get; set; }
        public string CandidateName { get; set; }
        public string Gender { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string IdentifyNumber { get; set; }
        public string PassportNo { get; set; }
        public Nullable<System.DateTime> DateIssuePassport { get; set; }
        public Nullable<System.DateTime> DateExpiresPassport { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Eyesight { get; set; }
        public string TimeWorkType { get; set; }
        public Nullable<bool> IsAcceptOvertime { get; set; }
        public string PeopleGuarantee { get; set; }
        public string PositionGuarantee { get; set; }
        public string Vehicles { get; set; }
        public string TAddress { get; set; }
        public string SourceAds { get; set; }
        public string HealthStatus { get; set; }
        public Nullable<System.Guid> EthnicID { get; set; }
        public Nullable<System.Guid> ReligionID { get; set; }
        public Nullable<double> Height { get; set; }
        public Nullable<double> Weight { get; set; }
        public Nullable<System.Guid> MarriageStatusID { get; set; }
        public Nullable<System.Guid> UserWaitApproveID { get; set; }
        public Nullable<System.DateTime> DateApprove { get; set; }
        public string PersonalPlan { get; set; }
        public string PersionalWebsite { get; set; }
        public string PersonalTarget { get; set; }
        public string SpecialSkill { get; set; }
        public string OrientationCareer { get; set; }
        public string LongtermOrientation { get; set; }
        public string ShorttermOrientation { get; set; }
        public string MemberOfAssociation { get; set; }
        public string WorkplaceSuggestion { get; set; }
        public string Status { get; set; }
        public string StatusLast { get; set; }
        public Nullable<System.DateTime> DateApply { get; set; }
        public Nullable<System.DateTime> DateStartWorking { get; set; }
        public string WorkingType { get; set; }
        public string WorkingTypePeriod { get; set; }
        public string Assessment { get; set; }
        public Nullable<double> ScorePotential { get; set; }
        public Nullable<System.Guid> RecruitmentCampaignID { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public Nullable<System.Guid> JobVacancyID { get; set; }
        public Nullable<System.Guid> TagID { get; set; }
        public Nullable<System.Guid> PositionID { get; set; }
        public string SalarySuggest { get; set; }
        public string SalaryApprove { get; set; }
        public string SalaryCurrent { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public string Weak { get; set; }
        public string Strong { get; set; }
        public Nullable<bool> IsReadyBizTrip { get; set; }
        public Nullable<double> YearOfExperience { get; set; }
        public Nullable<bool> IsAnnounceResult { get; set; }
        public string AssociationProfessional { get; set; }
        public string IntroducePerson { get; set; }
        public string FileAttachment { get; set; }
        public string ServerUpdate { get; set; }
        public string ServerCreate { get; set; }
        public string UserUpdate { get; set; }
        public string UserCreate { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public Nullable<System.DateTime> DateUpdate { get; set; }
        public Nullable<System.Guid> UserLockID { get; set; }
        public Nullable<System.DateTime> DateLock { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string IPCreate { get; set; }
        public string IPUpdate { get; set; }
        public Nullable<System.Guid> RecSourceID { get; set; }
        public Nullable<System.DateTime> IDDateOfIssue { get; set; }
        public string IDPlaceOfIssue { get; set; }
        public Nullable<System.Guid> TCountryID { get; set; }
        public Nullable<System.Guid> TProvinceID { get; set; }
        public Nullable<System.Guid> TDistrictID { get; set; }
        public string PAddress { get; set; }
        public Nullable<System.Guid> PCountryID { get; set; }
        public Nullable<System.Guid> PProvinceID { get; set; }
        public Nullable<System.Guid> PDistrictID { get; set; }
        public Nullable<System.Guid> NationalityID { get; set; }
        public string MarriageStatus { get; set; }
        public Nullable<double> Allowance { get; set; }
        public Nullable<System.Guid> JobTitleID { get; set; }
        public Nullable<int> ProbationDay { get; set; }
        public string SkillLevel { get; set; }
        public Nullable<System.DateTime> DateHire { get; set; }
        public string NameEnglish { get; set; }
        public Nullable<System.Guid> BirthDayID { get; set; }
        public string PlaceOfBirth { get; set; }
        public Nullable<System.Guid> PlaceOfIssueID { get; set; }
        public Nullable<System.Guid> EducationLevelID { get; set; }
        public string Origin { get; set; }
        public Nullable<System.Guid> CurrencyID1 { get; set; }
        public string ImagePath { get; set; }
        public string PlaceWrokLast { get; set; }
        public Nullable<bool> IsRecruiting { get; set; }
        public string CodeCandidate { get; set; }
        public string PepolePresent { get; set; }
        public string SalaryPayment { get; set; }
        public string ApproveHistory { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsBlackList { get; set; }
        public Nullable<bool> PassFilterResume { get; set; }
        public Nullable<int> LevelInterview { get; set; }
        public Nullable<double> SalaryProbationary { get; set; }
        public Nullable<double> Allowance1 { get; set; }
        public Nullable<System.Guid> AllowanceID1 { get; set; }
        public Nullable<double> Allowance2 { get; set; }
        public Nullable<System.Guid> AllowanceID2 { get; set; }
        public Nullable<double> Allowance3 { get; set; }
        public Nullable<System.Guid> AllowanceID3 { get; set; }
        public Nullable<double> Allowance4 { get; set; }
        public Nullable<System.Guid> AllowanceID4 { get; set; }
        public Nullable<bool> IsFilteredFile { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public Nullable<System.DateTime> WorkingTime { get; set; }
        public Nullable<System.Guid> SalaryClassID { get; set; }
        public Nullable<int> CallNumber { get; set; }
        public string ReasonDeny { get; set; }
        public string StatusHire { get; set; }
        public Nullable<double> DurationFailPrevious { get; set; }
        public Nullable<double> LevelEye { get; set; }
        public Nullable<double> GenaralHealth { get; set; }
        public Nullable<double> WriteTest { get; set; }
        public Nullable<double> Interview { get; set; }
        public string DiseaseListIDs { get; set; }
        public Nullable<System.Guid> CurrencyID2 { get; set; }
        public Nullable<System.Guid> CurrencyID3 { get; set; }
        public Nullable<double> LevelEyeRight { get; set; }
        public Nullable<double> Musculoskeletal { get; set; }
        public Nullable<double> BloodPressure { get; set; }
        public Nullable<double> HeartBeat { get; set; }
        public string DiseaseListCodes { get; set; }
        public Nullable<System.DateTime> DateExam { get; set; }
        public string ReasonFailFilter { get; set; }
        public string GraduateSchool { get; set; }
        public string Specialisation { get; set; }
        public Nullable<System.DateTime> YearGraduation { get; set; }
        public string Experience { get; set; }
        public string Mobile2 { get; set; }
        public Nullable<System.Guid> ReasonBlackListID { get; set; }
        public Nullable<int> NoEmailPass { get; set; }
        public Nullable<int> NoEmailFail { get; set; }
        public Nullable<int> CandidateNumber { get; set; }
        public Nullable<System.Guid> WorkPlaceID { get; set; }
    
        public virtual ICollection<Cat_Attachment> Cat_Attachment { get; set; }
        public virtual Cat_Country Cat_Country { get; set; }
        public virtual Cat_Country Cat_Country1 { get; set; }
        public virtual Cat_Country Cat_Country2 { get; set; }
        public virtual Cat_Currency Cat_Currency { get; set; }
        public virtual Cat_Currency Cat_Currency1 { get; set; }
        public virtual Cat_Currency Cat_Currency2 { get; set; }
        public virtual Cat_Currency Cat_Currency3 { get; set; }
        public virtual Cat_District Cat_District { get; set; }
        public virtual Cat_District Cat_District1 { get; set; }
        public virtual Cat_EthnicGroup Cat_EthnicGroup { get; set; }
        public virtual Cat_JobTitle Cat_JobTitle { get; set; }
        public virtual Cat_NameEntity Cat_NameEntity { get; set; }
        public virtual Cat_NameEntity Cat_NameEntity1 { get; set; }
        public virtual Cat_NameEntity Cat_NameEntity2 { get; set; }
        public virtual Cat_OrgStructure Cat_OrgStructure { get; set; }
        public virtual Cat_Position Cat_Position { get; set; }
        public virtual Cat_Province Cat_Province { get; set; }
        public virtual Cat_Province Cat_Province1 { get; set; }
        public virtual Cat_Province Cat_Province2 { get; set; }
        public virtual Cat_Province Cat_Province3 { get; set; }
        public virtual Cat_Religion Cat_Religion { get; set; }
        public virtual Cat_SalaryClass Cat_SalaryClass { get; set; }
        public virtual Cat_SourceAds Cat_SourceAds { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance1 { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance2 { get; set; }
        public virtual Cat_UsualAllowance Cat_UsualAllowance3 { get; set; }
        public virtual Cat_WorkPlace Cat_WorkPlace { get; set; }
        public virtual ICollection<Hre_CandidateHistory> Hre_CandidateHistory { get; set; }
        public virtual ICollection<Hre_Profile> Hre_Profile { get; set; }
        public virtual ICollection<Hre_StopWorking> Hre_StopWorking { get; set; }
        public virtual Rec_JobVacancy Rec_JobVacancy { get; set; }
        public virtual Rec_RecruitmentCampaign Rec_RecruitmentCampaign { get; set; }
        public virtual Rec_Tag Rec_Tag { get; set; }
        public virtual Sys_UserInfo Sys_UserInfo { get; set; }
        public virtual ICollection<Rec_CandidateBusiness> Rec_CandidateBusiness { get; set; }
        public virtual ICollection<Rec_CandidateComputingLevel> Rec_CandidateComputingLevel { get; set; }
        public virtual ICollection<Rec_CandidateLanguageLevel> Rec_CandidateLanguageLevel { get; set; }
        public virtual ICollection<Rec_CandidateQualification> Rec_CandidateQualification { get; set; }
        public virtual ICollection<Rec_CandidateSourceAds> Rec_CandidateSourceAds { get; set; }
        public virtual ICollection<Rec_InterviewCampaignDetail> Rec_InterviewCampaignDetail { get; set; }
        public virtual ICollection<Rec_Interview> Rec_Interview { get; set; }
        public virtual ICollection<Rec_InterviewHistory> Rec_InterviewHistory { get; set; }
        public virtual ICollection<Rec_Investigation> Rec_Investigation { get; set; }
        public virtual ICollection<Rec_RecruitmentHistory> Rec_RecruitmentHistory { get; set; }
        public virtual Rec_RecWorkHistory Rec_RecWorkHistory { get; set; }
        public virtual ICollection<Rec_Reference> Rec_Reference { get; set; }
        public virtual ICollection<Rec_Relative> Rec_Relative { get; set; }
        public virtual ICollection<Rec_SoftSkill> Rec_SoftSkill { get; set; }
        public virtual ICollection<Rec_SupplerRelation> Rec_SupplerRelation { get; set; }
        public virtual ICollection<Rec_CandidateHistory> Rec_CandidateHistory { get; set; }
    }
}
