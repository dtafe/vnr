
using HRM.Business.BaseModel;
using System;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_ProcessApprovedMultiEntity
    {
        public string ProcessName { get; set; }
        public Guid ID { get; set; }
    }

    public class Sys_ProcessApprovedEntity : HRMBaseModel
    {
        public string ProcessName { get; set; }
        public string Description { get; set; }
        public string Approved1 { get; set; }
        public string Approved2 { get; set; }
        public string Approved3 { get; set; }
        public string Approved4 { get; set; }
        public string Approved5 { get; set; }
        public string Approved6 { get; set; }
        public string Approved7 { get; set; }
        public string Approved8 { get; set; }
        public string Approved9 { get; set; }
        public string Approved10 { get; set; }
    }
}
