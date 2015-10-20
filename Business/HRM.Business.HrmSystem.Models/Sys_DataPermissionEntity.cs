using HRM.Business.BaseModel;
using System;
namespace HRM.Business.HrmSystem.Models
{
    public class Sys_DataPermissionEntity : HRMBaseModel
    {
        public string Code { get; set; }
        public string UserName { get; set; }
        public string GroupName { get; set; }
        public Guid UserID { get; set; }
        public Guid GroupID { get; set; }
        public byte[] Branches { get; set; }
        public byte[] DataGroups { get; set; }
        public string BranchesName { get; set; }
        public string DataGroup { get; set; }
        public string OrgStructure { get; set; }
        public string WorkPlace { get; set; }

    }
}
