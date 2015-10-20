
using System;
using HRM.Business.BaseModel;
namespace HRM.Business.HrmSystem.Models
{
    public class Sys_VersionEntity : HRMBaseModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Note { get; set; }
    }
}
