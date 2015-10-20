using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_ReportRewardEntity : HRMBaseModel
    {
        
        
        public string CodeEmp { get; set; }
        public string Reason { get; set; }
        public string RewardedTitleName { get; set; }
        public string RewardedTimeName { get; set; }
        public DateTime? DateOfEffective { get; set; }
        public string UserApproveName { get; set; }
        public string RequesterName { get; set; }
        public string OrgStructureID { get; set; }
        public string ProfileName { get; set; }
        public string PositionName { get; set; }
        public string JobTitleName { get; set; }
        public string OrgStructureName { get; set; }
        public string NoOfReward { get; set; }

        
        
    }
}
