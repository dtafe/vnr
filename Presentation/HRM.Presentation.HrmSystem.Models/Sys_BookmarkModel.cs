using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HRM.Presentation.HrmSystem.Models
{
    public class Sys_BookmarkModel: BaseViewModel
    {
        public Guid UserID { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_Bookmark_Link)]
        public string Link { get; set; }
        [DisplayName(ConstantDisplay.HRM_System_Bookmark_Name)]
        public string Name { get; set; }

        [DisplayName(ConstantDisplay.HRM_System_Bookmark_HotKey)]
        public string UserCreate { get; set; }

     public partial class FieldNames
     {
            public const string Link = "Link";
            public const string Name = "Name";
     }

    }
}
