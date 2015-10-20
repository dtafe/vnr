using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_RecruitmentCampaignItemEntity : HRMBaseModel
    {
        public System.Guid RecruitmentCampaignID { get; set; }
        public string RecruitmentCampaignName { get; set; }
        public System.Guid PositionID { get; set; }
        public string PositionName { get; set; }
        public int HeadCount { get; set; }
        public Nullable<System.DateTime> DateCount { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        public int HrPlanHC { get; set; }
        public int VacancyHC { get; set; }
        public int RecPlanHC { get; set; }
        public string Recruit_Plan { get; set; }
        public string Submited { get; set; }
        public string Hired { get; set; }
        public string Remain_Vacancy { get; set; }
        public string Comment { get; set; }
    }
}
