using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.HrmSystem.Models
{
    public class Sys_ColumnModeEntity
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> UserInfoID { get; set; }
        public string GridControlName { get; set; }
        public string ColumnMode { get; set; }
        public Nullable<int> MaxDisplayedRow { get; set; }
    }
}
