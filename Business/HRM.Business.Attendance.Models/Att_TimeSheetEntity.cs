using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class Att_TimeSheetEntity : HRMBaseModel
    {
        public Nullable<System.Guid> ProfileID { get; set; }
        public string ProfileName { get; set; }
        public Nullable<System.Guid> RoleID { get; set; }
        public string RoleName { get; set; }
        public Nullable<System.Guid> JobTypeID { get; set; }
        public string JobTypeName { get; set; }
        public Nullable<double> NoHour { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Note { get; set; }
        public Nullable<double> Sector { get; set; }
        public string CodeEmp { get; set; }
    }
}
