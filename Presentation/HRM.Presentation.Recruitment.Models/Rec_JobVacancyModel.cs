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
    public class Rec_JobVacancyModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_AreaPostJobID)]
        public string AreaPostJobName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_AreaPostJobID)]
        public Nullable<System.Guid> AreaPostJobID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Specialized)]
        public string Specialized { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_JobVacancyName)]
        public string JobVacancyName { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_Position)]
        public System.Guid PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_Position)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_UserApproveName)]
        public Nullable<System.Guid> UserApproveID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_UserApproveName)]
        public string UserApproveName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_DeptInChargeName)]
        public Nullable<System.Guid> DeptInChargeID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_DeptInChargeName)]
        public string DeptInChargeName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_SalaryMax)]
        public Nullable<double> SalaryMax { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_SalaryMin)]
        public Nullable<double> SalaryMin { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_FileAttachment)]
        public string FileAttachment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RecruitmentCampaignName)]
        public Nullable<System.Guid> RecruitmentCampaignID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RecruitmentCampaignName)]
        public string RecruitmentCampaignName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_JobDescription)]
        public string JobDescription { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Workplace)]
        public string Workplace { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Requirement)]
        public string Requirement { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public Nullable<System.Guid> OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_SourceAds)]
        public string SourceAds { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_JobLevel)]
        public string JobLevel { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_PayrollNumber)]
        public string PayrollNumber { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_TotalEmpPosition)]
        public Nullable<int> TotalEmpPosition { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_EmpNameReplace)]
        public string EmpNameReplace { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_PositionReplace)]
        public string PositionReplace { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_ReasonsForReplacementName)]
        public Nullable<System.Guid> ReasonsForReplacementID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_ReasonsForReplacementName)]
        public string ReasonsForReplacementName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_JobLevelReplace)]
        public string JobLevelReplace { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_DateProposal)]
        public Nullable<System.DateTime> DateProposal { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_DateEnd)]
        public Nullable<System.DateTime> DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_DateStart)]
        public Nullable<System.DateTime> DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_DateComplete)]
        public Nullable<System.DateTime> DateComplete { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Quantity)]
        public Nullable<int> Quantity { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_DatePublish)]
        public Nullable<System.DateTime> DatePublish { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_IsExternal)]
        public Nullable<bool> IsExternal { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_IsApprove)]
        public Nullable<bool> IsApprove { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_IsFirstApprove)]
        public Nullable<bool> IsFirstApprove { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_IsRevaluation)]
        public Nullable<bool> IsRevaluation { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Status)]
        public string Status { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_LastStatus)]
        public string LastStatus { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_UserApprove)]
        public string UserApprove { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_UserApprove1)]
        public string UserApprove1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_DateApprove)]
        public Nullable<System.DateTime> DateApprove { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_DateApprove1)]
        public Nullable<System.DateTime> DateApprove1 { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_IsBelongToPlan)]
        public bool IsBelongToPlan { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_UserSubmit)]
        public string UserSubmit { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_DateSubmit)]
        public Nullable<System.DateTime> DateSubmit { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public Nullable<System.Guid> ProfileID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_ProfileName)]
        public string ProfileName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public Nullable<System.Guid> JobTitleID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_JobTitleName)]
        public string JobTitleName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_FemaleRequestedNo)]
        public Nullable<int> FemaleRequestedNo { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_UserAssingName)]
        public Nullable<System.Guid> UserAssingID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_UserAssingName)]
        public string UserAssingName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_URL)]
        public string URL { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_DateStartApprove)]
        public Nullable<System.DateTime> DateStartApprove { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_DateEndApprove)]
        public Nullable<System.DateTime> DateEndApprove { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_ValueApprove)]
        public Nullable<double> ValueApprove { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Cost)]
        public Nullable<double> Cost { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_SourceAdsName)]
        public Nullable<System.Guid> SourceAdsID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_SourceAdsName)]
        public string SourceAdsName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_PositionReplaceName)]
        public Nullable<System.Guid> PositionReplaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_PositionReplaceName)]
        public string PositionReplaceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_SalaryPayment)]
        public string SalaryPayment { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Notes)]
        public string Notes { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_ApproveHistory)]
        public string ApproveHistory { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Height)]
        public Nullable<double> Height { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Weight)]
        public Nullable<double> Weight { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_LevelEyes)]
        public Nullable<double> LevelEyes { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_DurationFailPrevious)]
        public Nullable<double> DurationFailPrevious { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_IsTerminateInCompany)]
        public Nullable<bool> IsTerminateInCompany { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_FromAge)]
        public Nullable<int> FromAge { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_ToAge)]
        public Nullable<int> ToAge { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Gender)]
        public string Gender { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_DiseaseIDs)]
        public string DiseaseIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_NoLevelInterview)]
        public Nullable<int> NoLevelInterview { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_FormJobIDs)]
        public string FormJobIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_EducationLevelIDs)]
        public string EducationLevelIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_LanguageIDs)]
        public string LanguageIDs { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_ExperienceIDs)]
        public string ExperienceIDs { get; set; }
        public string JobConditionIDs { get; set; }
        public Guid? JobVacancyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_IsFilterCV)]
        public Nullable<bool> IsFilterCV { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_NoPost)]
        public Nullable<int> NoPost { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_DateCheck)]
        public Nullable<System.DateTime> DateCheck { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_DateInterview)]
        public Nullable<System.DateTime> DateInterview { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_SourceAdsName)]
        public Nullable<System.Guid> SourceAdID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_SourceAdsName)]
        public string SourceAdName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RankID)]
        public Nullable<System.Guid> RankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RankID)]
        public string JobVacancyRankName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_DateEndReceiveCV)]
        public Nullable<System.DateTime> DateEndReceiveCV { get; set; }
        public Guid ExportID { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public string ValueFields { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_WorkPlaceID)]
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_WorkPlaceID)]
        public string WorkPlaceName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Experience)]
        public string Experience { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Workplace = "Workplace";
            public const string WorkPlaceName = "WorkPlaceName";
            public const string DateProposal = "DateProposal";
            public const string DateCheck = "DateCheck";
            public const string OrgStructureName = "OrgStructureName";
            public const string DateEndReceiveCV = "DateEndReceiveCV";
            public const string JobVacancyRankName = "JobVacancyRankName";
            public const string Code = "Code";
            public const string JobVacancyName = "JobVacancyName";
            public const string PositionName = "PositionName";
            public const string RecruitmentCampaignName = "RecruitmentCampaignName";
            public const string Quantity = "Quantity";
            public const string Type = "Type";
            public const string SourceAdsName = "SourceAdsName";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string Status = "Status";
            public const string SalaryMax = "SalaryMax";
            public const string SalaryMin = "SalaryMin";
            public const string UserApprove = "UserApprove";
            public const string DateApprove = "DateApprove";
            public const string UserSubmit = "UserSubmit";
            public const string DateSubmit = "DateSubmit";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
            public const string SourceAdName = "SourceAdName";
            public const string NoLevelInterview = "NoLevelInterview";
            public const string NoPost = "NoPost";
            public const string DateInterview = "DateInterview";
            public const string AreaPostJobName = "AreaPostJobName";
        }
    }

    public class Rec_JobVacancySearchModel
    {
        
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_DateStart)]
        public Nullable<System.DateTime> DateStartFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_DateEnd)]
        public Nullable<System.DateTime> DateStartTo { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_JobVacancyName)]
        public string JobVacancyName { get; set; }

        public Nullable<int> QuantityTo { get; set; }

        public Nullable<int> QuantityFrom { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Type)]
        public string Type { get; set; }

        [DisplayName(ConstantDisplay.HRM_REC_Candidate_Position)]
        public Nullable<System.Guid> PositionID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_SourceAdsName)]
        public Nullable<System.Guid> SourceAdID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RankID)]
        public Nullable<System.Guid> RankID { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Code)]
        public string Code { get; set; }

        public bool IsCreateTemplate { get; set; }

        public bool IsCreateTemplateForDynamicGrid { get; set; }

        public Guid ExportId { get; set; }

        public ExportFileType ExportType { get; set; }

        public bool IsExport { get; set; }

        public string ValueFields { get; set; }
       
    }
    public class Rec_JobVacancyMultiModel
    {
        public Guid ID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_JobVacancyName)]
        public string JobVacancyName { get; set; }
    }

    public class Rec_ReportJobVacancySearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_DateStart)]
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureID)]
        public string OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RankID)]
        public Nullable<System.Guid> RankID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Type)]
        public string Type { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportID { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
