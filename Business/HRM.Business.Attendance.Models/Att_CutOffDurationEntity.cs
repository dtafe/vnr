using HRM.Business.BaseModel;
using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class Att_CutOffDurationMultiEntity
    {
        public Guid ID { get; set; }
        public int TotalRow { get; set; }
        public string CutOffDurationName { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
    public class Att_CutOffDurationEntity : HRMBaseModel
    {
        public string CutOffDurationName { get; set; }
        public System.DateTime MonthYear { get; set; }
        public System.DateTime DateStart { get; set; }
        public System.DateTime DateEnd { get; set; }
        public string DurationType { get; set; }
        public Nullable<bool> IsInsuranceSocial { get; set; }

        public Nullable<System.DateTime> OvertimeStart { get; set; }
        public Nullable<System.DateTime> OvertimeEnd { get; set; }
        public Nullable<System.DateTime> LeavedayStart { get; set; }
        public Nullable<System.DateTime> LeavedayEnd { get; set; }
    }
}
