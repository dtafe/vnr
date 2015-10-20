using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Recruitment.Models
{
    public class Rec_RecCostDetailEntity : HRMBaseModel
    {
        public Nullable<System.Guid> RecCampaignID { get; set; }
        public string RecCampaignName { get; set; }
        public Nullable<int> OrderNumber { get; set; }
        public string CostName { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public string Description { get; set; }
    }
}
