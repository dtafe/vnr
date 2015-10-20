using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_RecruitmentCampaignEntity : HRMBaseModel
    {
        public string RecruitmentCampaignName { get; set; }
        public bool IsActivate { get; set; }
        public Nullable<double> Budget { get; set; }
        public Nullable<double> SalaryMaxBudget { get; set; }
        public Nullable<double> SalaryMinBudget { get; set; }
        public Nullable<double> SalaryCurrentBudget { get; set; }
        public Nullable<double> SalaryCampaignBudget { get; set; }
        public Nullable<double> SalaryMaxAccumulatedApproval { get; set; }
        public Nullable<double> SalaryMinAccumulatedApproval { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> DateInterviewFrom { get; set; }
        public Nullable<System.DateTime> DateInterviewTo { get; set; }
        public Nullable<System.DateTime> DateStart { get; set; }
        public Nullable<System.DateTime> DateEnd { get; set; }
        public string Code { get; set; }
        public string UserSubmit { get; set; }
        public Nullable<System.DateTime> DateSubmit { get; set; }
        public string Status { get; set; }
        public Nullable<double> ValueSubmit { get; set; }
        public string Source { get; set; }
        public Nullable<System.Guid> CurrencyID { get; set; }
        public string CurrencyName { get; set; }
    }
    public class Rec_RecruitmentCampaignMultiEntity
    {
        public Guid ID { get; set; }
        public string RecruitmentCampaignName { get; set; }
    }
}
