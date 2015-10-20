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
    public class Sys_ResourceModel : BaseViewModel
    {
       [DisplayName(ConstantDisplay.HRM_System_Resource_Code)]
       [StringLength(32, ErrorMessage = ConstantDisplay.HRM_System_Resource_Code_StringLength)]
       public string Code { get; set; }

       [DisplayName(ConstantDisplay.HRM_System_Resource_ResourceType)]
       [StringLength(32, ErrorMessage = ConstantDisplay.HRM_System_Resource_ResourceType_StringLength)]
       public string ResourceType { get; set; }

       [DisplayName(ConstantDisplay.HRM_System_Resource_ResourceName)]
       [StringLength(50, ErrorMessage = ConstantDisplay.HRM_System_Resource_ResourceName_StringLength)]
       public string ResourceName { get; set; }

       [DisplayName(ConstantDisplay.HRM_System_Resource_ModuleName)]
       [StringLength(50, ErrorMessage = ConstantDisplay.HRM_System_Resource_ModuleName_StringLength)]
       public string ModuleName { get; set; }

       [DisplayName(ConstantDisplay.HRM_System_Resource_Description)]
       [StringLength(100, ErrorMessage = ConstantDisplay.HHRM_System_Resource_Description_StringLength)]
       public string Description { get; set; }
       public partial class FieldNames
       {
           public const string Code = "Code";
           public const string ResourceType = "ResourceType";
           public const string ResourceName = "ResourceName";
           public const string ModuleName = "ModuleName";
           public const string Description = "Description";
           public const string View = "View";
           public const string Add = "Add";
           public const string Edit = "Edit";
           public const string Delete = "Delete";
           public const string Export = "Export";
           public const string Import = "Import";
       }

        public bool View { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool Export { get; set; }
        public bool Import { get; set; }
    }
}
