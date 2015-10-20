using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_AsynTaskEntity
    {
        public System.Guid ID { get; set; }
        public string Type { get; set; }
        public string Summary { get; set; }
        public string Status { get; set; }
        public double PercentComplete { get; set; }
        public System.DateTime TimeStart { get; set; }
        public Nullable<System.DateTime> TimeEnd { get; set; }
        public string ServerUpdate { get; set; }
        public string ServerCreate { get; set; }
        public string UserUpdate { get; set; }
        public string UserCreate { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public Nullable<System.DateTime> DateUpdate { get; set; }
        public Nullable<System.Guid> UserLockID { get; set; }
        public Nullable<System.DateTime> DateLock { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string IPCreate { get; set; }
        public string IPUpdate { get; set; }
        public string Description { get; set; }
        public string Dept { get; set; }
        public int TotalRow { get; set; }
        public int TotalComputedProfileForTask { get; set; }
    }
}
