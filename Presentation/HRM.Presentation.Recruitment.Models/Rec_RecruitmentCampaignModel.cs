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
    public class Rec_RecruitmentCampaignModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign)]
        public string RecruitmentCampaignName { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_User_IsActivate)]
        public bool IsActivate { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_Budget)]
        public Nullable<double> Budget { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_SalaryMaxBudget)]
        public Nullable<double> SalaryMaxBudget { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_SalaryMinBudget)]
        public Nullable<double> SalaryMinBudget { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_SalaryCurrentBudget)]
        public Nullable<double> SalaryCurrentBudget { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_SalaryCampaignBudget)]
        public Nullable<double> SalaryCampaignBudget { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_SalaryMaxAccumulatedApproval)]
        public Nullable<double> SalaryMaxAccumulatedApproval { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_SalaryMinAccumulatedApproval)]
        public Nullable<double> SalaryMinAccumulatedApproval { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_Description)]
        public string Description { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_DateStart)]
        public Nullable<System.DateTime> DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_DateEnd)]
        public Nullable<System.DateTime> DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_UserSubmit)]
        public string UserSubmit { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_DateSubmit)]
        public Nullable<System.DateTime> DateSubmit { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_Status)]
        public string Status { get; set; }
        //[DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_ValueSubmit)]
        public Nullable<double> ValueSubmit { get; set; }

        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_Source)]
        public string Source { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Grade_CurrencyID)]
        public Nullable<System.Guid> CurrencyID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Category_Grade_CurrencyID)]
        public string CurrencyName { get; set; }

        private Guid _id = Guid.Empty;
        public Guid RecruitmentCampaign_ID
        {
            get
            {
                _id = ID;
                return _id;
            }
            set
            {
                _id = value;
                ID = value;
            }
        }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string RecruitmentCampaignName = "RecruitmentCampaignName";
            public const string SalaryMaxBudget = "SalaryMaxBudget";
            public const string SalaryMinBudget = "SalaryMinBudget";
            public const string Status = "Status";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string Description = "Description";
            public const string DateUpdate = "DateUpdate";
            public const string SalaryCurrentBudget = "SalaryCurrentBudget";
            public const string Budget = "Budget";
            public const string CurrencyName = "CurrencyName";
            public const string SalaryCampaignBudget = "SalaryCampaignBudget";
            
        }
    }

    public class Rec_RecruitmentCampaignSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign)]
        public string RecruitmentCampaignName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_Status)]
        public string Status { get; set; }
        public bool IsCreateTemplate { get; set; }
        public bool IsCreateTemplateForDynamicGrid { get; set; }
        public Guid ExportId { get; set; }
        public ExportFileType ExportType { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }
        
    }
    public class Rec_RecruitmentCampaignMultiModel
    {
        public Guid ID { get; set; }
        public string RecruitmentCampaignName { get; set; }
    }
}
