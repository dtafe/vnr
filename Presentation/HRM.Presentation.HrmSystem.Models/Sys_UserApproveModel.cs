using HRM.Infrastructure.Utilities;
using System;
using System.ComponentModel;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_UserApproveModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_System_Resource_Code)]
        public string Code { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_UserApprove_UserApproveName)]
        public Guid UserApproveID { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_UserApprove_UserApproveName)]
        public string UserApproveName { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Reward_OrgStructureName)]
        public Guid? OrgStructureID { get; set; }
        [DisplayName(ConstantDisplay.HRM_HR_Reward_OrgStructureName)]
        public string OrgStructureName { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_UserApprove_Type)]
        public string Type { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_UserApprove_Notes)]
        public string Notes { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_UserApprove_IsMasterApprove)]
        public bool? IsMasterApprove { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_UserApprove_IsNoGetMail)]
        public bool? IsNoGetMail { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_UserApprove_IsAllowApproveMySelf)]
        public bool? IsAllowApproveMySelf { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_UserApprove_CurrentStatus)]
        public string CurrentStatus { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_UserApprove_UserApproveName)]
        public string UserRequestName { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_UserApprove_UserRequestID)]
        public Nullable<System.Guid> UserRequestID { get; set; }
        public partial class FieldNames
        {
            public const string UserApproveName = "UserApproveName";
            public const string OrgStructureName = "OrgStructureName";
            public const string Type = "Type";
            public const string UserRequestName = "UserRequestName";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
            public const string CurrentStatus = "CurrentStatus";
            public const string UserRequestID = "UserRequestID";
        }

    }
    public class Sys_UserApproveSearchModel
    {
        [DisplayName(ConstantDisplay.HRM_System_UserApprove_UserApproveName)]
        public string UserApproveID { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_UserApprove_Type)]
        public string Type { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }

    public class Sys_UserApproveMultiModel
    {
        public Guid ID { get; set; }
        public Guid UserApproveID { get; set; }
        public string UserApproveName { get; set; }
    }
}

