using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_LockObjectItemEntity : HRMBaseModel
    {
        public System.Guid LockObjectID { get; set; }
        public string ObjectName { get; set; }
        public System.DateTime DateStart { get; set; }
        public System.DateTime DateEnd { get; set; }
        public string Type { get; set; }
        public string Commend { get; set; }
    }
}
