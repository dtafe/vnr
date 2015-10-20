using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.HrmSystem.Models
{
    public partial class PermissionMappingModel : BaseViewModel
    {
        public PermissionMappingModel()
        {
            AvailablePermissions = new List<Sys_GroupPermissionModel>();
            AvailableRolesName = new List<RolesNameModel>();
            Allowed = new Dictionary<string, IDictionary<int, bool>>();
        }
        public IList<Sys_GroupPermissionModel> AvailablePermissions { get; set; }
        public IList<RolesNameModel> AvailableRolesName { get; set; }

        //[permission system name] / [customer role id] / [allowed]
        public IDictionary<string, IDictionary<int, bool>> Allowed { get; set; }

        #region Sys_Group

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
        public string Description { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_Group_Description)]
        public string Notes { get; set; }


        public partial class FieldNames
        {
            public const string Code = "Code";
            public const string GroupName = "GroupName";
            public const string IsActive = "IsActive";
            public const string Description = "Description";
        }


        #endregion

    }
}
