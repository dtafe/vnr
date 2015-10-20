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
    public class Rec_RecruitmentCampaignItemModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign)]
        public System.Guid RecruitmentCampaignID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign)]
        public string RecruitmentCampaignName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public System.Guid PositionID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_PositionName)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaignItem_HeadCount)]
        public int HeadCount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaignItem_DateCount)]
        public Nullable<System.DateTime> DateCount { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public Nullable<System.Guid> OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaignItem_HrPlanHC)]
        public int HrPlanHC { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaignItem_VacancyHC)]
        public int VacancyHC { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaignItem_RecPlanHC)]
        public int RecPlanHC { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaignItem_Recruit_Plan)]
        public string Recruit_Plan { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaignItem_Submited)]
        public string Submited { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaignItem_Hired)]
        public string Hired { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaignItem_Remain_Vacancy)]
        public string Remain_Vacancy { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_UsualAllowance_Comment)]
        public string Comment { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string PositionName = "PositionName";
            public const string RecruitmentCampaignName = "RecruitmentCampaignName";
            public const string HeadCount = "HeadCount";
            public const string Comment = "Comment";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }

    public class Rec_RecruitmentCampaignItemSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign)]
        public Guid? RecruitmentCampaignID { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }
}
