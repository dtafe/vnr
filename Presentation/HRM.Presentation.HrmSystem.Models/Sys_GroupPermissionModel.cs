using System;
using System.Collections.Generic;
using HRM.Infrastructure.Utilities;
using System.ComponentModel;
using VnResource.Helper.Data;
using VnResource.Helper.Security;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_GroupPermissionModel : BaseViewModel
    {
        [DisplayName(ConstantDisplay.HRM_System_GroupPermission_Code)]
        public string Code { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_GroupPermission_GroupID)]
        public Guid GroupID { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_GroupPermission_ResourceID)]
        public Guid ResourceID { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_GroupPermission_PrivilegeNumber)]
        public int? PrivilegeNumber { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_Group_GroupName)]
        public string GroupName { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_Resource_ResourceName)]
        public string ResourceName { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_Resource_ResourceName)]
        public string ResourceNameTranslate { get; set; }

        public string Category { get; set; }

        public string ModuleName { get; set; }
        public string ResourceType { get; set; }
        public string ResourceTypeTranslate { get; set; }

        public partial class FieldNames
        {
            public const string ID = "ID";
            public const string Code = "Code";
            public const string GroupID = "GroupID";
            public const string GroupName = "GroupName";
            public const string ResourceID = "ResourceID";
            public const string ResourceName = "ResourceName";
            public const string ModuleName = "ModuleName";
            public const string ResourceType = "ResourceType";
            public const string PrivilegeNumber = "PrivilegeNumber";
            public const string Category = "Category";
            public const string ResourceTypeTranslate = "ResourceTypeTranslate";
        }
    }

    public class RolesNameModel 
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}