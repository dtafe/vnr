
using System;
namespace HRM.Business.HrmSystem.Models
{
    public class Sys_UserApproveEntity : HRM.Business.BaseModel.HRMBaseModel
    {
        public string Code { get; set; }
        public System.Guid UserApproveID { get; set; }
        public string UserApproveName { get; set; }
        public Nullable<System.Guid> OrgStructureID { get; set; }
        public string OrgStructureName { get; set; }
        public string Type { get; set; }
        public string Notes { get; set; }
        public Nullable<bool> IsMasterApprove { get; set; }
        public Nullable<bool> IsNoGetMail { get; set; }
        public Nullable<bool> IsAllowApproveMySelf { get; set; }
        public string PermistionType { get; set; }
        public string CurrentStatus { get; set; }
        public Nullable<System.Guid> UserRequestID { get; set; }
        public string UserRequestName { get; set; }
    }
}
