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
    public class Rec_ReportJobVacancyModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_HR_Profile_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_REC_Candidate_Position)]
        public string PositionName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Quantity)]
        public Nullable<int> Quantity { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_RankID)]
        public string SalaryRankName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_DateProposal)]
        public Nullable<System.DateTime> DateProposal { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_DateStart)]
        public Nullable<System.DateTime> DateStart { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecruitmentCampaign_DateEnd)]
        public Nullable<System.DateTime> DateEnd { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_JobVacancy_DateCheck)]
        public Nullable<System.DateTime> DateCheck { get; set; }
        public partial class FieldNames
        {
            public const string OrgStructureName = "OrgStructureName";
            public const string PositionName = "PositionName";
            public const string Quantity = "Quantity";
            public const string SalaryRankName = "SalaryRankName";
            public const string Type = "Type";
            public const string DateProposal = "DateProposal";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string DateCheck = "DateCheck";
        }
    }
}
