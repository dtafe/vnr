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
    public class Rec_InterviewCampaignModel : BaseViewModel
    {
        public Nullable<int> LevelInterview { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_InterviewCampaign_InterviewCampaignName)]
        public string InterviewCampaignName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_InterviewCampaign_HoldDate)]
        public DateTime? HoldDate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_InterviewCampaign_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_InterviewCampaign_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_InterviewCampaign_DateFrom)]
        public Nullable<System.DateTime> DateInterviewFrom { get; set; }
        [DisplayName(ConstantDisplay.HRM_Recruitment_InterviewCampaign_DateTo)]
        public Nullable<System.DateTime> DateInterviewTo { get; set; }
        public string listCandidateIds { get; set; }
        public List<Guid> listCandidateGuidIds { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string InterviewCampaignName = "InterviewCampaignName";
            public const string HoldDate = "HoldDate";
            public const string Code = "Code";
            public const string Description = "Description";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string UserUpdate = "UserUpdate";
            public const string DateInterviewFrom = "DateInterviewFrom";
            public const string DateInterviewTo = "DateInterviewTo";
        }
    }
    public class Rec_InterviewCampaignSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Recruitment_InterviewCampaign_InterviewCampaignName)]
        public string InterviewCampaignName { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Rec_InterviewCampaignMultiModel
    {
        public Guid ID { get; set; }
        public string InterviewCampaignName { get; set; }
        public int TotalRow { get; set; }
    }
}
