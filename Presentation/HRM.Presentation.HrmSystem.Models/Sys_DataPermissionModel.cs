using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_DataPermissionModel : BaseViewModel
    {
       [DisplayName(ConstantDisplay.HRM_System_DataPermission_Code)]
       [StringLength(50, ErrorMessage = ConstantDisplay.HRM_System_DataPermission_Code_StringLength)]
       public string Code { get; set; }

       [DisplayName(ConstantDisplay.HRM_System_DataPermission_UserID)]
       public Guid UserID { get; set; }

       [DisplayName(ConstantDisplay.HRM_System_DataPermission_GroupID)]
       public Guid GroupID { get; set; }

       [DisplayName(ConstantDisplay.HRM_System_DataPermission_Branches)]
       public List<int> Branches { get; set; }

       [DisplayName(ConstantDisplay.HRM_System_DataPermission_DataGroups)]
       public byte[] DataGroups { get; set; }

       [DisplayName(ConstantDisplay.HRM_System_User_UserName)]
       public string UserName { get; set; }

       [DisplayName(ConstantDisplay.HRM_System_Group_GroupName)]
       public string GroupName { get; set; }
       public string BranchesName { get; set; }
       public bool IsChecked { get; set; }
       public string DataGroup { get; set; }
       public string OrgStructure { get; set; }

       public Guid? WorkPlaceID { get; set; }

       public string WorkPlace { get; set; }
       public partial class FieldNames
       {
           public const string IsChecked = "IsChecked";
           public const string ID = "ID";
           public const string Code = "Code";
           public const string UserID = "UserID";
           public const string UserName = "UserName";
           public const string GroupName = "GroupName";
           public const string GroupID = "GroupID";
           public const string BranchesName = "BranchesName";
           public const string DataGroups = "DataGroups";
           public const string DataGroup = "DataGroup";
           public const string OrgStructure = "OrgStructure";
       }
    }
}
