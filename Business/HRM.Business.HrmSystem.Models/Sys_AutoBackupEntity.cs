
using System;
using HRM.Business.BaseModel;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_AutoBackupEntity : HRMBaseModel
    {
        public string Code { get; set; }
        public string AutoBackupName { get; set; }
        public string ProcedureName { get; set; }
        public bool? IsActivate { get; set; }
        public string Email { get; set; }
        public int? TimeWaiting { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateExpired { get; set; }
        public DateTime? LastStart { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
    }
}
