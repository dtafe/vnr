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
    public class Rec_RecruitmentHistoryModel : BaseViewModel
    {
        public Nullable<System.DateTime> DateInterview1 { get; set; }
        public Nullable<System.DateTime> DateInterview2 { get; set; }
        public Nullable<System.DateTime> DateInterview3 { get; set; }
        public Nullable<System.DateTime> DateInterview4 { get; set; }
        public Nullable<System.DateTime> DateInterview5 { get; set; }
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


        public Nullable<int> CandidateNumber { get; set; }
        public System.Guid CandidateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Contract_RankRateID)]
        public Guid? SalaryClassID { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_ProbationDay)]
        //public Nullable<int> ProbationDay { get; set; }
        public string CurrencyName3 { get; set; }
        public string CurrencyName1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_PassFilterResume)]
        public Nullable<bool> PassFilterResume { get; set; }
        public string CurrencyName2 { get; set; }
        public string CurrencyName { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_SalaryApprove)]
        public double? SalaryApproveMoney { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_SalaryCurrent)]
        public double? SalaryCurrentMoney { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_SalarySuggest)]
        public double? SalarySuggestMoney { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_CodeCandidate)]
        public string CodeCandidate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_CandidateName)]
        public string CandidateName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_ReportBirthday_PositionName)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_JoiningDate)]
        public Nullable<System.DateTime> JoiningDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_SourceAdsID)]         
        public Nullable<System.Guid> SourceAdsID { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string IdentifyNumber { get; set; }
        public string PassportNo { get; set; }
        public Nullable<System.DateTime> DateIssuePassport { get; set; }
        public Nullable<System.DateTime> DateExpiresPassport { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Eyesight { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_WorkingTime)]
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
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_WorkplaceSuggestion)]
        public string WorkplaceSuggestion { get; set; }
        public string Status { get; set; }

        public string StatusLast { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_DateApply)]
        public Nullable<System.DateTime> DateApply { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_DateStartWorking)]
        public Nullable<System.DateTime> DateStartWorking { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_WorkingType)]
        public string WorkingType { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_WorkingTypePeriod)]
        public string WorkingTypePeriod { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceRecord_Comment)]
        public string Assessment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_TotalMark)]
        public Nullable<double> ScorePotential { get; set; }
        public Nullable<System.Guid> RecruitmentCampaignID { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_List)]
        public Nullable<System.Guid> JobVacancyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_Label)]
        public Nullable<System.Guid> TagID { get; set; }
        public Nullable<System.Guid> PositionID { get; set; }
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_SalarySuggest)]
        public string SalarySuggest { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_SalaryApprove)]
        public string SalaryApprove { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_SalaryCurrent)]
        public string SalaryCurrent { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Weaknesses)]
        public string Weak { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Strengths)]
        public string Strong { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_IsReadyBizTrip)]
        public Nullable<bool> IsReadyBizTrip { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_YearExperience)]
        public double YearOfExperience { get; set; }
        public Nullable<bool> IsAnnounceResult { get; set; }
        public string AssociationProfessional { get; set; }
        public string IntroducePerson { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_FileAttachment)]
        public string FileAttachment { get; set; }
        public string ApproveHistory { get; set; }
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
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Nullable<System.Guid> JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_ProbationDay)]
        public int? ProbationDay { get; set; }
         [DisplayName(ConstantDisplay.HRM_REC_Candidate_SkillLevel)]
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
        public string PepolePresent { get; set; }
        public string SalaryPayment { get; set; }
        public string DateBirthOrther { get; set; }
        public Nullable<bool> IsDateBirthOrther { get; set; }
             [DisplayName(ConstantDisplay.HRM_HR_Profile_IsBlackList)]
        public Nullable<bool> IsBlackList { get; set; }
        public string StatusHealth { get; set; }
        public Nullable<double> Allowance1 { get; set; }
        public Nullable<double> Allowance2 { get; set; }
        public Nullable<double> Allowance3 { get; set; }
        public Nullable<double> Allowance4 { get; set; }
             [DisplayName(ConstantDisplay.HRM_REC_Candidate_AllowanceID1)]
        public Nullable<System.Guid> AllowanceID1 { get; set; }
          [DisplayName(ConstantDisplay.HRM_REC_Candidate_AllowanceID2)]
        public Nullable<System.Guid> AllowanceID2 { get; set; }
            [DisplayName(ConstantDisplay.HRM_REC_Candidate_AllowanceID3)]
        public Nullable<System.Guid> AllowanceID3 { get; set; }
               [DisplayName(ConstantDisplay.HRM_REC_Candidate_AllowanceID4)]
        public Nullable<System.Guid> AllowanceID4 { get; set; }
        public Nullable<System.Guid> CurrencyID2 { get; set; }
        public Nullable<System.Guid> CurrencyID3 { get; set; }
        public Nullable<System.Guid> CurrencyID4 { get; set; }
             [DisplayName(ConstantDisplay.HRM_REC_Candidate_SalaryProbationary)]
        public Nullable<double> SalaryProbationary { get; set; }
        public string TimeWork { get; set; }
        public Nullable<System.DateTime> DateRequest { get; set; }
        public Nullable<double> DurationFailPrevious { get; set; }
        public Nullable<double> LevelEye { get; set; }
        public Nullable<double> GenaralHealth { get; set; }
        public Nullable<double> WriteTest { get; set; }
        public Nullable<double> Interview { get; set; }
        public string DiseaseListIDs { get; set; }
        public Nullable<int> LevelInterview { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_DateExam)]
        public Nullable<System.DateTime> DateExam { get; set; }
        public string StatusView { get; set; }
        public string GenderView { get; set; }
        public string RankName { get; set; }
        public string JobVacancyName { get; set; }
        public string ReasonFailFilter { get; set; }

        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        public string JobVacancyCode { get; set; }
        public string SourceAdsName { get; set; }
        public string MarriageStatusView { get; set; }
        public string EducationLevelName { get; set; }
        public string GraduateSchool { get; set; }
        public Nullable<System.DateTime> YearGraduation { get; set; }
        public string Specialisation { get; set; }
        public partial class FieldNames
        {
            public const string CandidateNumber = "CandidateNumber";
            public const string RankName = "RankName";
            public const string JobVacancyName = "JobVacancyName";
            public const string StatusView = "StatusView";
            public const string GenderView = "GenderView";
            public const string DateExam = "DateExam";
            public const string DateApply = "DateApply";
            public const string CodeCandidate = "CodeCandidate";
            public const string CandidateName = "CandidateName";
            public const string OrgStructureName = "OrgStructureName";
            public const string PositionName = "PositionName";
            public const string Gender = "Gender";
            public const string DateOfBirth = "DateOfBirth";
            public const string JoiningDate = "JoiningDate";
            public const string Status = "Status";
            public const string Mobile = "Mobile";
            public const string Phone = "Phone";
            public const string Email = "Email";
            public const string TAddress = "TAddress";
            public const string LevelInterview = "LevelInterview";
            public const string DateStartWorking = "DateStartWorking";
            public const string E_UNIT = "E_UNIT";
            public const string E_DIVISION = "E_DIVISION";
            public const string E_DEPARTMENT = "E_DEPARTMENT";
            public const string E_TEAM = "E_TEAM";
            public const string E_SECTION = "E_SECTION";
        }
    }
    public class Rec_RecruitmentHistorySearchModel
    {
        public System.Guid CandidateID { get; set; }
        public string ValueFields { get; set; }
        public bool IsExport { get; set; }
    }
    public class Rec_RecruitmentHistorySearchModelNew
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_Label)]
        public Guid? TagID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IdentifyNumber { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CellPhone)]
        public string Mobile { get; set; }

        [DisplayName(ConstantDisplay.HRM_REC_Candidate_Position)]
        public Guid? PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_CodeCandidate)]
        public string CodeCandidate { get; set; }

        // Ngày sinh từ ngày đến ngày      
        public DateTime? DateOfBirthFrom { get; set; }

        public DateTime? DateOfBirthTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_TAProvince)]
        public Guid? TProvinceID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_Status)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_CandidateName)]
        public string CandidateName { get; set; }
        // Ngày bắt đầu đi làm
        public DateTime? WorkDateFrom { get; set; }

        public DateTime? WorkDateTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RankID)]
        public Nullable<System.Guid> RankID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_JobVacancyID)]
        public Nullable<System.Guid> JobVacancyID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RecruitmentCampaignName)]
        public Guid? RecruitmentCampaignID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public Nullable<System.Guid> WorkPlaceID { get; set; }

        public DateTime? DateApplyFrom { get; set; }
        public DateTime? DateApplyTo { get; set; }

        public DateTime? DateExamFrom { get; set; }
        public DateTime? DateExamTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_ReasonFailFilter)]
        public string ReasonFailFilter { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_EducationLevel)]
        public Guid? EducationLevelID { get; set; }

        [DisplayName(ConstantDisplay.HRM_REC_Candidate_LevelInterview)]
        public int? LevelInterview { get; set; }

        public bool IsCreateTemplate { get; set; }

        public bool IsCreateTemplateForDynamicGrid { get; set; }

        //public Nullable<DateTime> DateQuit { get; set; }

        //public Nullable<DateTime> DtToQuit { get; set; }

        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

    }

    public class Rec_RecruitmentHistorySearchModelNew1
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_Label)]
        public Guid? TagID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CellPhone)]
        public string Mobile { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_Position)]
        public Guid? PositionID { get; set; }
        // Chuyên môn chính

        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_CodeCandidate)]
        public string CodeCandidate { get; set; }
        // Ngày sinh từ ngày đến ngày      
        public DateTime? DateOfBirthFrom { get; set; }
        public DateTime? DateOfBirthTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TAProvince)]
        public Guid? TProvinceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_Status)]
        public string Status { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_CandidateName)]
        public string CandidateName { get; set; }
        // Ngày bắt đầu đi làm
        public DateTime? WorkDateFrom { get; set; }
        public DateTime? WorkDateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
