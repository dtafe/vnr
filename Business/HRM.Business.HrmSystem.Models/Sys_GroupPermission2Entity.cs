
using System;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_GroupPermission2Entity : HRM.Business.BaseModel.HRMBaseModel 
    {
        public string Code { get; set; }
        public Guid GroupID { get; set; }
        public Guid ResourceID { get; set; }
        public Nullable<long> PrivilegeNumber { get; set; }
        public string GroupName { get; set; }
        public string ResourceName { get; set; }
        public string ModuleName { get; set; }
        public string Category { get; set; }
        public string ResourceType { get; set; }
    }
}
