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
    public class Rec_RecCostDetailModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_Rec_RecCostDetail_RecCampaignID)]
        public Nullable<System.Guid> RecCampaignID { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecCostDetail_RecCampaignID)]
        public string RecCampaignName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecCostDetail_OrderNumber)]
        public Nullable<int> OrderNumber { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecCostDetail_CostName)]
        public string CostName { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecCostDetail_Amount)]
        public Nullable<double> Amount { get; set; }
        [DisplayName(ConstantDisplay.HRM_Rec_RecCostDetail_Description)]
        public string Description { get; set; }
        public bool IsExport { get; set; }
        public string ValueFields { get; set; }

        public Nullable<double> TotalAmount { get; set; } 

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string RecCampaignName = "RecCampaignName";
            public const string OrderNumber = "OrderNumber";
            public const string CostName = "CostName";
            public const string Amount = "Amount";
            public const string Description = "Description";
            public const string TotalAmount = "TotalAmount";
        }
    }
}
