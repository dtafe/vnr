using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class Att_AnnualLeaveEntity : HRMBaseModel
    {
        public Guid ProfileID { get; set; }
        public int Year { get; set; }
        public int MonthStart { get; set; }
        public double InitAnlValue { get; set; }
        public double InitSickValue { get; set; }
        public double InitSaveSickValue { get; set; }
        public double? AnlValueLastYear { get; set; }
        public DateTime? ExpireAnlValueLastYear { get; set; }
        public string AnlMonthReset { get; set; }
        public int? MonthResetAnlOfBeforeYear { get; set; }
        public string ProfileName { get; set; }
        public string CodeEmp { get; set; }
    }
}
