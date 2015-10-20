using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_JobVacancyEntity : HRMBaseModel
    {
        public string AreaPostJobName { get; set; }
        public Nullable<System.Guid> AreaPostJobID { get; set; }
        public string Code { get; set; }
        public string JobVacancyName { get; set; }
        public System.Guid PositionID { get; set; }
        public string PositionName { get; set; }
        public Nullable<System.Guid> UserApproveID { get; set; }
        public string UserApproveName { get; set; }
        public Nullable<System.Guid> DeptInChargeID { get; set; }
        public string DeptInChargeName { get; set; }
        public Nullable<double> SalaryMax { get; set; }
        public Nullable<double> SalaryMin { get; set; }
        public string FileAttachment { get; set; }
        public Nullable<System.Guid> RecruitmentCampaignID { get; set; }
        public string RecruitmentCampaignName { get; set; }
        public string JobDescription { get; set; }
        public string Workplace { get; set; }
        public string Requirement { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        public string SourceAds { get; set; }
        public string JobLevel { get; set; }
        public string PayrollNumber { get; set; }
        public Nullable<int> TotalEmpPosition { get; set; }
        public string EmpNameReplace { get; set; }
        public string PositionReplace { get; set; }
        public Nullable<System.Guid> ReasonsForReplacementID { get; set; }
        public string ReasonsForReplacementName { get; set; }
        public string JobLevelReplace { get; set; }
        public string DateProposalFormat { get; set; }
        public string DateStartFormat { get; set; }
        public string DateEndFormat { get; set; }
        public string DateCompleteFormat { get; set; }
        public string DatePublishFormat { get; set; }
        public string DateEndReceiveCVFormat { get; set; }
        
        public Nullable<System.DateTime> DateProposal { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateComplete { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<System.DateTime> DatePublish { get; set; }
        public Nullable<bool> IsExternal { get; set; }
        public Nullable<bool> IsApprove { get; set; }
        public Nullable<bool> IsFirstApprove { get; set; }
        public Nullable<bool> IsRevaluation { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string LastStatus { get; set; }
        public string UserApprove { get; set; }
        public string UserApprove1 { get; set; }
        public Nullable<System.DateTime> DateApprove { get; set; }
        public Nullable<System.DateTime> DateApprove1 { get; set; }
        public bool IsBelongToPlan { get; set; }
        public string UserSubmit { get; set; }
        public Nullable<System.DateTime> DateSubmit { get; set; }
        public Nullable<System.Guid> ProfileID { get; set; }
        public string ProfileName { get; set; }
        public Nullable<System.Guid> JobTitleID { get; set; }
        public string JobTitleName { get; set; }
        public Nullable<int> FemaleRequestedNo { get; set; }
        public Nullable<System.Guid> UserAssingID { get; set; }
        public string UserAssingName { get; set; }
        public string URL { get; set; }
        public Nullable<System.DateTime> DateStartApprove { get; set; }
        public Nullable<System.DateTime> DateEndApprove { get; set; }
        public Nullable<double> ValueApprove { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<System.Guid> SourceAdsID { get; set; }
        public string SourceAdsName { get; set; }
        public Nullable<System.Guid> PositionReplaceID { get; set; }
        public string PositionReplaceName { get; set; }
        public string SalaryPayment { get; set; }
        public string Notes { get; set; }
        public string ApproveHistory { get; set; }
        public Nullable<double> Height { get; set; }
        public Nullable<double> Weight { get; set; }
        public Nullable<double> LevelEyes { get; set; }
        public Nullable<double> DurationFailPrevious { get; set; }
        public Nullable<bool> IsTerminateInCompany { get; set; }
        public Nullable<int> FromAge { get; set; }
        public Nullable<int> ToAge { get; set; }
        public string Gender { get; set; }
        public string DiseaseIDs { get; set; }
        public Nullable<int> NoLevelInterview { get; set; }
        public string FormJobIDs { get; set; }
        public string EducationLevelIDs { get; set; }
        public string LanguageIDs { get; set; }
        public string ExperienceIDs { get; set; }
        public string JobConditionIDs { get; set; }
        public Guid? JobVacancyID { get; set; }
        public Nullable<bool> IsFilterCV { get; set; }
        public Nullable<int> NoPost { get; set; }
        public Nullable<System.DateTime> DateCheck { get; set; }
        public Nullable<System.DateTime> DateInterview { get; set; }
        public Nullable<System.Guid> SourceAdID { get; set; }
        public string SourceAdName { get; set; }
        public Nullable<System.Guid> RankID { get; set; }
        public string JobVacancyRankName { get; set; }
        public Nullable<System.DateTime> DateEndReceiveCV { get; set; }
        public Nullable<System.Guid> WorkPlaceID { get; set; }
        public string WorkPlaceName { get; set; }
        public string Experience { get; set; }
        public string Specialized { get; set; }
    }
    public class Rec_JobVacancyMultiEntity
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string JobVacancyName { get; set; }
    }
    
}
