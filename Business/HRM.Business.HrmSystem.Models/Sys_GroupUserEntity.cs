
using System;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_GroupUserEntity 
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string GroupUserName { get; set; }
        public string Note { get; set; }
        public Guid GroupID { get; set; }
        public string GroupName { get; set; }
        public Guid UserID { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public byte[] OrgStructures { get; set; }       
        public string OrgStructuresName { get; set; }       
    }
}
