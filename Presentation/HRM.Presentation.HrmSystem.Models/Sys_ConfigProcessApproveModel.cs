using HRM.Infrastructure.Utilities;
using System;
using System.ComponentModel;

namespace HRM.Presentation.HrmSystem.Models
{
    public  class Sys_ConfigProcessApproveModel : BaseViewModel
    {
         [DisplayName(ConstantDisplay.HRM_System_ConfigPA_ConfigProcessApproveName)]
        public string ConfigProcessApproveName { get; set; }
         [DisplayName(ConstantDisplay.HRM_System_ConfigPA_DeptID)]
        public Guid? DeptID { get; set; }
         [DisplayName(ConstantDisplay.HRM_System_ConfigPA_Function)]
        public string Function { get; set; }
         [DisplayName(ConstantDisplay.HRM_System_ConfigPA_CurrentStatus)]
        public string CurrentStatus { get; set; }
         [DisplayName(ConstantDisplay.HRM_System_ConfigPA_NextStatus)]
        public string NextStatus { get; set; }
         [DisplayName(ConstantDisplay.HRM_HR_Reward_Description)]
        public string Description { get; set; }
         [DisplayName(ConstantDisplay.HRM_System_ConfigPA_IsDefault)]
        public bool? IsDefault { get; set; }
         [DisplayName(ConstantDisplay.HRM_System_ConfigPA_TypeInCharge)]
        public string TypeInCharge { get; set; }
         [DisplayName(ConstantDisplay.HRM_System_ConfigPA_InCharge)]
        public string InCharge { get; set; }
         [DisplayName(ConstantDisplay.HRM_System_ConfigPA_NextStatusFormular)]
        public string NextStatusFormular { get; set; }
         public string OrgStructureName { get; set; }
        public partial class FieldNames
        {
            public const string ConfigProcessApproveName = "ConfigProcessApproveName";
            public const string DeptID = "DeptID";
            public const string Function = "Function";
            public const string CurrentStatus = "CurrentStatus";
            public const string NextStatus = "NextStatus";
            public const string Description = "Description";
            public const string IsDefault = "IsDefault";
            public const string TypeInCharge = "TypeInCharge";
            public const string InCharge = "InCharge";
            public const string NextStatusFormular = "NextStatusFormular";
            public const string UserUpdate = "UserUpdate";
            public const string DateUpdate = "DateUpdate";
        }
    }
    public class Sys_ConfigProcessApproveSearchModel 
    {
        [DisplayName(ConstantDisplay.HRM_System_ConfigPA_ConfigProcessApproveName)]
        public string ConfigProcessApproveName { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_ConfigPA_DeptID)]
        public Guid? DeptID { get; set; }
        public bool? IsExport { get; set; }
        public string ValueFields { get; set; }
    }
    public class Sys_ConfigProcessApproveMultiModel 
    {
        public Guid ID { get; set; }
        public string ConfigProcessApproveName { get; set; }
    }
}
