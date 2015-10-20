using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.Recruitment.Models
{
    public class Rec_CandidateModel : BaseViewModel
    {
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        public string WorkPlaceName { get; set; }
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

        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Score1)]
        public double? Score1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Score2)]
        public double? Score2 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_Score3)]
        public double? Score3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Interview_ResultInterview)]
        public string ResultInterview { get; set; }

        public Nullable<bool> IsFilterCV { get; set; }
        public object ListInvalidData { get; set; }
        public object ListValidData { get; set; }

        [DisplayName(ConstantDisplay.HRM_REC_Candidate_Experience)]
        public string Experience { get; set; }
        public string UserCreateID { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_LevelEye)]
        public Nullable<double> LevelEye { get; set; }
        public string ProfileIds { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_UserApproveID)]
        public Guid? UserApproveID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Roster_UserApproveID2)]
        public Guid? UserApproveID2 { get; set; }
        //nguon tuyen
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_SourceAdsID)]
        public Guid? SourceAdsID { get; set; }
        public string SourceAdsName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_CodeCandidate)]
        public string CodeCandidate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_CandidateName)]
        public string CandidateName { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_CandidateNumber)]
        public Nullable<int> CandidateNumber { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        public string udGender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_DayOfBirth)]
        [DataType(DataType.Date)]
        public System.DateTime DateOfBirth { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDNo)]
        public string IdentifyNumber { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PassportNo)]
        public string PassportNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_DateIssuePassport)]
        public Nullable<System.DateTime> DateIssuePassport { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_DateExpiresPassport)]
        public Nullable<System.DateTime> DateExpiresPassport { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_HomePhone)]
        public string Phone { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CellPhone)]
        public string Mobile { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_CellPhone2)]
        public string Mobile2 { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_Email)]
        public string Email { get; set; }
        public string Eyesight { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_WorkingTime)]
        public string TimeWorkType { get; set; }
        //Gioi tinh
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        [MaxLength(50)]
        public string GenderView { get; set; }
        //san sang di cong tac
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_IsReadyBizTrip)]
        public Nullable<bool> IsReadyBizTrip { get; set; }

        public Nullable<bool> IsAcceptOvertime { get; set; }
        public string PeopleGuarantee { get; set; }
        public string PositionGuarantee { get; set; }

        public string Vehicles { get; set; }
        public string SourceAds { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_HealthStatus)]
        public string HealthStatus { get; set; }
        //dan toc
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EthnicID)]
        public Guid? EthnicID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_EthnicGroup_EthnicGroupName)]
        public string EthnicName { get; set; }
        //ton giao
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ReligionID)]
        public Guid? ReligionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Religion_ReligionName)]
        public string ReligionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Height)]
        public Nullable<double> Height { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Weight)]
        public Nullable<double> Weight { get; set; }
        //tinh trang hon nhan
        [DisplayName(ConstantDisplay.HRM_HR_Profile_MarriageStatus)]
        public Guid? MarriageStatusID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_MarriageStatus)]
        [MaxLength(50)]
        public string MarriageStatusView { get; set; }
        public Guid? UserWaitApproveID { get; set; }
        public string UserWaitApproveName { get; set; }
        public Nullable<System.DateTime> DateApprove { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_PersonalPlan)]
        public string PersonalPlan { get; set; }
        public string PersionalWebsite { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_PersonalTarget)]
        public string PersonalTarget { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_SpecialSkill)]
        public string SpecialSkill { get; set; }
        //dinh huong nghe nghiep
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_OrientationCareer)]
        public string OrientationCareer { get; set; }
        //dinh huong lau dai
        public string LongtermOrientation { get; set; }
        public string ShorttermOrientation { get; set; }
        //thanh vien hoi
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_MemberOfAssociation)]
        public string MemberOfAssociation { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_WorkplaceSuggestion)]
        public string WorkplaceSuggestion { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_Status)]
        public string Status { get; set; }
        public string udStatus { get; set; }


        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_StatusLast)]
        public string StatusLast { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_DateApply)]
        public Nullable<System.DateTime> DateApply { get; set; }
        //ngay bat dau di lam
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_DateStartWorking)]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DateStartWorking { get; set; }

        //hih thuc lam viec

        [DisplayName(ConstantDisplay.HRM_REC_Candidate_WorkingType)]
        public string WorkingType { get; set; }
        //ky
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_WorkingTypePeriod)]
        public string WorkingTypePeriod { get; set; }
        [DisplayName(ConstantDisplay.HRM_Insurance_InsuranceRecord_Comment)]
        public string Assessment { get; set; }
        //diem tiem nang

        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_TotalMark)]
        public Nullable<double> ScorePotential { get; set; }



        public Guid? RecruitmentCampaignID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_List)]
        public string RecruitmentCampaignName { get; set; }
        public Guid? OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_JobVacancyIDs)]
        public string JobVacancyName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_JobVacancyID)]
        public string JobVacancyCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_Label)]
        public Guid? TagID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_Label)]
        public string TagName { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_Position)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_Position)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_PositionCode)]
        public string PositionCode { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_SalarySuggest)]
        public string SalarySuggest { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_SalarySuggest)]
        public double? SalarySuggestMoney { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_SalaryApprove)]
        public string SalaryApprove { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_SalaryApprove)]
        public double? SalaryApproveMoney { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_SalaryCurrent)]
        public string SalaryCurrent { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_SalaryCurrent)]
        public double? SalaryCurrentMoney { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_SalaryProbationary)]
        public Nullable<double> SalaryProbationary { get; set; }
        public Guid? CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public Guid? CurrencyID1 { get; set; }
        public string CurrencyName1 { get; set; }
        public Guid? CurrencyID2 { get; set; }
        public string CurrencyName2 { get; set; }
        public Guid? CurrencyID3 { get; set; }
        public string CurrencyName3 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Weaknesses)]
        public string Weak { get; set; }
        [DisplayName(ConstantDisplay.HRM_Evaluation_Performance_Strengths)]
        public string Strong { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_YearExperience)]
        public Nullable<double> YearOfExperience { get; set; }
        //cong bo ket qua
        public Nullable<bool> IsAnnounceResult { get; set; }
        public string AssociationProfessional { get; set; }
        //nguoi gioi thiu
        public string IntroducePerson { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_FileAttachment)]
        public string FileAttachment { get; set; }

        public Guid? RecSourceID { get; set; }
        public string RecSourceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDDateOfIssue)]
        public Nullable<System.DateTime> IDDateOfIssue { get; set; }

        public string IDPlaceOfIssue { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TAddressID)]
        public string TAddress { get; set; }
        public Guid? TCountryID { get; set; }
        public string TCountryName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_TAProvince)]
        public Guid? TProvinceID { get; set; }
        public string TProvinceName { get; set; }
        public Guid? TDistrictID { get; set; }
        public string TDistrictName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PAddressID)]
        public string PAddress { get; set; }
        public Guid? PCountryID { get; set; }
        public string PCountryName { get; set; }
        public Guid? PProvinceID { get; set; }
        public string PProvinceName { get; set; }
        public Guid? PDistrictID { get; set; }
        public string PDistrictName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_NationalityID)]
        public Guid? NationalityID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_NationalityName)]
        [MaxLength(150)]
        public string NationalityName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_MarriageStatus)]
        public string MarriageStatus { get; set; }
        //phu cap
        [DisplayName(ConstantDisplay.HRM_HR_AppendixContract_AllowanceID5)]
        public Nullable<double> Allowance { get; set; }
        //phu cap quan ly
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_AllowanceID1)]
        public Guid? AllowanceID1 { get; set; }

        //phu cap ki nang
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_AllowanceID2)]
        public Guid? AllowanceID2 { get; set; }
        //phu cap ngon ngu
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_AllowanceID3)]
        public Guid? AllowanceID3 { get; set; }
        //phu cap di lai
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_AllowanceID4)]
        public Guid? AllowanceID4 { get; set; }
        //bac he so luong
        [DisplayName(ConstantDisplay.HRM_HR_Contract_RankRateID)]
        public Guid? SalaryClassID { get; set; }

        //yeu cau tuyen

        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_JobVacancyIDs)]
        public Guid? JobVacancyID { get; set; }
        //da qua loc hs
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_IsFilteredFile)]
        public Nullable<bool> IsFilteredFile { get; set; }
        //ngay tuyen
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_JoiningDate)]
        public Nullable<System.DateTime> JoiningDate { get; set; }
        //thoi gian lam viec
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_WorkingTime)]
        public Nullable<System.DateTime> WorkingTime { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_ProbationDay)]
        public Nullable<int> ProbationDay { get; set; }
        //tay nghe
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_SkillLevel)]
        public string SkillLevel { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_DateHire)]
        public Nullable<System.DateTime> DateHire { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_NameEnglish)]
        public string NameEnglish { get; set; }
        public Guid? BirthDayID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PlaceOfBirth)]
        public string PlaceOfBirth { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IDPlaceOfIssue)]
        public Guid? PlaceOfIssueID { get; set; }
        public string PlaceOfIssueName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EducationLevel)]
        public Guid? EducationLevelID { get; set; }
        public string EducationLevelName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Origin)]
        public string Origin { get; set; }
    //    public Guid? CurrencyID1 { get; set; }
        public string CurrencyIDName { get; set; }
        public string ImagePath { get; set; }
        public string PlaceWrokLast { get; set; }
        public Nullable<bool> IsRecruiting { get; set; }
        public string PepolePresent { get; set; }
        public string SalaryPayment { get; set; }
        public string ApproveHistory { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_IsBlackList)]
        public Nullable<bool> IsBlackList { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_PassFilterResume)]
        public Nullable<bool> PassFilterResume { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_LevelInterview)]
        public Nullable<int> LevelInterview { get; set; }

        public Nullable<double> Allowance1 { get; set; }
        public Nullable<double> Allowance2 { get; set; }
        public Nullable<double> Allowance3 { get; set; }
        public Nullable<double> Allowance4 { get; set; }
        public string JobConditionIDs { get; set; }

        [DisplayName(ConstantDisplay.HRM_REC_Candidate_CallNumber)]
        public Nullable<int> CallNumber { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_ReasonDeny)]
        public string ReasonDeny { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_StatusHire)]
        public string StatusHire { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_Status)]
        public string StatusView { get; set; }
        public string listId { get; set; }
        // Trình độ chuyên môn
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_GenaralHealth)]
        public double? GenaralHealth { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_WriteTest)]
        public double? WriteTest { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_Interview)]
        public double? Interview { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_DiseaseListIDs)]
        public string DiseaseListIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_LevelEyeRight)]
        public Nullable<double> LevelEyeRight { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_Musculoskeletal)]
        public Nullable<double> Musculoskeletal { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_BloodPressure)]
        public Nullable<double> BloodPressure { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_HeartBeat)]
        public Nullable<double> HeartBeat { get; set; }
        public string DiseaseListCodes { get; set; }

        public string HealthStatusView { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_DateExam)]
        public Nullable<System.DateTime> DateExam { get; set; }
        public string RankName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_ReasonFailFilter)]
        public string ReasonFailFilter { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_GraduateSchool)]
        public string GraduateSchool { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_Specialisation)]
        public string Specialisation { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_YearGraduation)]
        public Nullable<System.DateTime> YearGraduation { get; set; }

        public string lstprofileid { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_ReasonBlackListID)]
        public Nullable<System.Guid> ReasonBlackListID { get; set; }
            [DisplayName(ConstantDisplay.HRM_REC_Candidate_ReasonBlackListID)]
        public string ReasonBlackListName { get; set; }
        

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string ReasonFailFilter = "ReasonFailFilter";
            public const string DateApply = "DateApply";
            public const string RankName = "RankName";
            public const string DateExam = "DateExam";
            public const string CandidateName = "CandidateName";
            public const string CandidateNumber = "CandidateNumber";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string UserUpdate = "UserUpdate";
            public const string CodeCandidate = "CodeCandidate";
            public const string HealthStatus = "HealthStatus";
            public const string HealthStatusView = "HealthStatusView";
            public const string Status = "Status";
            public const string StatusView = "StatusView";
            public const string StatusLast = "StatusLast";
            public const string Gender = "Gender";
            public const string GenderView = "GenderView";
            public const string DateOfBirth = "DateOfBirth";
            public const string Mobile = "Mobile";
            public const string Phone = "Phone";
            public const string Email = "Email";
            public const string LevelInterview = "LevelInterview";
            public const string TAddress = "TAddress";
            public const string JobVacancyName = "JobVacancyName";
            public const string JobVacancyCode = "JobVacancyCode";
            public const string RecruitmentCampaignName = "RecruitmentCampaignName";
            public const string Assessment = "Assessment";
            public const string TagName = "TagName";
            public const string CallNumber = "CallNumber";
            public const string ReasonDeny = "ReasonDeny";
            public const string ScorePotential = "ScorePotential";
            public const string ReasonBlackListName = "ReasonBlackListName";
            public const string DiseaseListIDs = "DiseaseListIDs";
            public const string PositionCode = "PositionCode";
            public const string PositionName = "PositionName";
            public const string Score1 = "Score1";
            public const string Score2 = "Score2";
            public const string Score3 = "Score3";
            public const string ResultInterview = "ResultInterview";
        }
    }
    public class Rec_CandidateSearchModel
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

        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_JobVacancyIDs)]
        public Guid? JobVacancyID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RecruitmentCampaignName)]
        public Guid? RecruitmentCampaignID { get; set; }

        // Ngày Ứng Tuyển
        public DateTime? DateApplyFrom { get; set; }

        public DateTime? DateApplyTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RankID)]
        public Nullable<System.Guid> RankID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_ReasonFailFilter)]
        public string ReasonFailFilter { get; set; }
        // Ngày Thi
        public DateTime? DateExamFrom { get; set; }
        public DateTime? DateExamTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public Nullable<System.Guid> WorkPlaceID { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_EducationLevel)]
        public Guid? EducationLevelID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_Evaluated)]
        public string Evaluated { get; set; }
        public bool? IsEvaluated { get; set; }

        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Rec_FilterCandidateModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_DateFrom)]
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_JobVacancyIDs)]
        public string JobVacancyIDs { get; set; }
        [DefaultValue(false)]
        public bool GetListFail { get; set; }

        //[DisplayName(ConstantDisplay.HRM_Rec_Candidate_IsIncludeEvaCandidate)]
        [DefaultValue(false)]
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_IsIncludeEvaCandidate)]
        public bool IsIncludeEvaCandidate { get; set; }
    }

    public class Rec_CandidateMultiModel
    {
        public Guid ID { get; set; }
        public string CandidateName { get; set; }
    }

    public class Rec_CandidateGeneralMultiSearchModel
    {
        public Guid? CandidateID { get; set; }
        public string Keyword { get; set; }
        public string OrgStructureID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Rec_EnrolledCandidateSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_Label)]
        public Guid? TagID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CellPhone)]
        public string Mobile { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_CodeCandidate)]
        public string CodeCandidate { get; set; }
        //// Ngày sinh từ ngày đến ngày      
        //public DateTime? DateOfBirthFrom { get; set; }
        //public DateTime? DateOfBirthTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_HR_Profile_TAProvince)]
        public Guid? TProvinceID { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Rec_Candidate_Status)]
       // public string Status { get; set; }
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
        public DateTime? DateApplyFrom { get; set; }
        public DateTime? DateApplyTo { get; set; }
        // Ngày Thi
        public DateTime? DateExamFrom { get; set; }
        public DateTime? DateExamTo { get; set; }
        //Nơi Làm Việc
        [DisplayName(ConstantDisplay.HRM_Canteen_Report_workPlaceId)]
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_EducationLevel)]
        public Guid? EducationLevelID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Rec_FailCandidateSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_Label)]
        public Guid? TagID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CellPhone)]
        public string Mobile { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
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
        // Ngày thi
        public DateTime? WorkDateFrom { get; set; }
        public DateTime? WorkDateTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RankID)]
        public Nullable<System.Guid> RankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_JobVacancyID)]
        public Nullable<System.Guid> JobVacancyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_LevelInterview)]
        public int? LevelInterview { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_RecruitmentCampaign)]
        public Guid? RecruitmentCampaignID { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_Position)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_WorkPlaceName)]
        public Guid? WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_DateApply)]
        public DateTime? DateApplyFrom { get; set; }
        public DateTime? DateApplyTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_DateExam)]
        public DateTime? DateExamFrom { get; set; }
        public DateTime? DateExamTo { get; set; }
    }
    public class Rec_WaitingInterviewSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_Label)]
        public Guid? TagID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CellPhone)]
        public string Mobile { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_Position)]
        public Guid? PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_RecruitmentCampaign)]
        public Guid? RecruitmentCampaignID { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_DateApply)]
        public DateTime? DateApplyFrom { get; set; }

        public DateTime? DateApplyTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_EducationLevelIDs)]
        public Guid? EducationLevelID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_DateExam)]
        public DateTime? DateExamFrom { get; set; }
        public DateTime? DateExamTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_WorkPlace_WorkPlaceName)]
        public Guid? WorkPlaceID { get; set; }
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

        [DisplayName(ConstantDisplay.HRM_REC_Candidate_LevelInterview)]
        public int? LevelInterview { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RankID)]
        public Nullable<System.Guid> RankID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_JobVacancyID)]
        public Nullable<System.Guid> JobVacancyID { get; set; }

        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Rec_CandidateInBlackSearchModel
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
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_JobVacancyIDs)]
        public Guid? JobVacancyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RecruitmentCampaignName)]
        public Guid? RecruitmentCampaignID { get; set; }
        // Ngày Ứng Tuyển
        public DateTime? DateApplyFrom { get; set; }
        public DateTime? DateApplyTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RankID)]
        public Nullable<System.Guid> RankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_Candidate_ReasonFailFilter)]
        public string ReasonFailFilter { get; set; }
        // Ngày Thi
        public DateTime? DateExamFrom { get; set; }
        public DateTime? DateExamTo { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_ReasonBlackListID)]
        public Nullable<System.Guid> ReasonBlackListID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Rec_CandidateFailToGetTheJobSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Attendance_Overtime_Label)]
        public Guid? TagID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_CellPhone)]
        public string Mobile { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleID)]
        public Guid? JobTitleID { get; set; }
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
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
