using System;
using System.Collections.Generic;
using HRM.Infrastructure.Utilities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_GroupUserModel : BaseViewModel
    {
        [MaxLength(50)]
        [DisplayName(ConstantDisplay.HRM_System_GroupUser_Code)]
        public string Code { get; set; }

        [MaxLength(150)]
        [DisplayName(ConstantDisplay.HRM_System_GroupUser_GroupUserName)]
        public string GroupUserName { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_GroupUser_Note)]
        public string Note { get; set; }

        public string LoginName { get; set; }
        public string UserName { get; set; }

        public string GroupName { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_GroupUser_GroupID)]
        public Guid? GroupID { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_GroupUser_UserID)]
        public Guid? UserID { get; set; }

        public List<Guid> OrgStructuresId { get; set; }
        public Guid[] OrgStructures { get; set; }
        public string OrgStructuresName { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string GroupUserName = "GroupUserName";
            public const string Note = "Note";
            public const string GroupID = "GroupID";
            public const string LoginName = "LoginName";
            public const string UserName = "UserName";
            public const string GroupName = "GroupName";
            public const string UserID = "UserID";
            public const string OrgStructuresName = "OrgStructuresName";
        }
      
    }
}