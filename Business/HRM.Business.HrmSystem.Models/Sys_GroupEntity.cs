
using HRM.Business.BaseModel;
using System;
namespace HRM.Business.HrmSystem.Models
{
    public class Sys_GroupEntity : HRMBaseModel
    {
        public string Code { get; set; }
        public string GroupName { get; set; }
        public bool IsActivate { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
    }
    public class Sys_GroupMultiEntity
    {
        public Guid ID { get; set; }
        public string GroupName { get; set; }
        public string Code { get; set; }
    }
}
  