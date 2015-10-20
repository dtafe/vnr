using System;
using HRM.Business.BaseModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HRM.Business.Hr.Models
{
    public class Hre_RewardEntity : HRMBaseModel
    {
        public int TotalRow { get; set; }
        public Guid ProfileID { get; set; }
        [MaxLength(50)]
        public string CodeEmp { get; set; }
        public DateTime DateOfIssuance { get; set; }
        [MaxLength(1000)]
        public string Reason { get; set; }
        public Guid? RewardedTitleID { get; set; }
        public Guid? RewardedDecidingOrgID { get; set; }
        public Guid? RewardedTimeID { get; set; }
        public DateTime? DateOfEffective { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public Guid? UserApproveID { get; set; }
        [MaxLength(150)]
        public string UserApproveName { get; set; }
        [MaxLength(150)]
        public string RequesterName { get; set; }
        public Guid? RequesterID { get; set; }
        public double? RewardValue { get; set; }
        [MaxLength(50)]
        public string NoOfReward { get; set; }
        [MaxLength(1000)]
        public string Attachment { get; set; }
        public Guid PositionID { get; set; }
        public List<Guid?> OrgStructureID { get; set; }
        [MaxLength(150)]
        public string ProfileName { get; set; }
        [MaxLength(150)]
        public string PositionName { get; set; }
        [MaxLength(150)]
        public string JobTitleName { get; set; }
        [MaxLength(150)]
        public string OrgStructureName { get; set; }
        public Nullable<System.Guid> RewardedTypeID { get; set; }
        public string RewardedTypeName { get; set; }

        private Guid _id = Guid.Empty;
        public Guid Reward_ID
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
        public string E_UNIT { get; set; }
        public string E_DIVISION { get; set; }
        public string E_DEPARTMENT { get; set; }
        public string E_TEAM { get; set; }
        public string E_SECTION { get; set; }

        
    }
}
