using HRM.Infrastructure.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_GroupModel : BaseViewModel
    {
        [MaxLength(32)]
        [DisplayName(ConstantDisplay.HRM_System_Group_Code)]
        public string Code { get; set; }

        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_System_Group_GroupName)]
        public string GroupName { get; set; } 

        [DisplayName(ConstantDisplay.HRM_System_Group_IsActivate)]
        public bool IsActivate { get; set; }

        [MaxLength(100)]
        [DisplayName(ConstantDisplay.HRM_System_Group_Description)]
        public string Notes { get; set; }

        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string GroupName = "GroupName";
            public const string IsActivate = "IsActivate";
            public const string Notes = "Notes";
        }
      
    }

    public class Sys_GroupSearchModel
    {
        public string GroupName { get; set; } 
    }

    public class Sys_GroupMultiModel
    {
        public Guid ID { get; set; }
        public string GroupName { get; set; }
        public string Code { get; set; }
    }

    
}