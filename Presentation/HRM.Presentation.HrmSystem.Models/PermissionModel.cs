using System;
using System.Collections.Generic;
using VnResource.Helper.Security;

namespace HRM.Presentation.HrmSystem.Models
{
    public class PermissionModel
    {
        public Guid UserID { get; set; }
        
        public PrivilegeType PrivilegeType { get; set; }

        public string Permission { get; set; }

        public bool IsAllowAccess { get; set; }
        
        public partial class FieldNames
        {
            public const string UserID = "UserID";
            public const string PrivilegeType = "PrivilegeType";
            public const string Permission = "Permission";
            public const string IsAllowAccess = "IsAllowAccess";
        }
      
    }

    /// <summary>
    /// [Hien.Nguyen]
    /// Mượn Page để tại 1 model giả để truyền dữ liệu
    /// </summary>
    public class TempPermissionModel
    {
        public List<string> Data { get; set; }
    }
}