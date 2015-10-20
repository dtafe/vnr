using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class RosterModel
    {
        public Guid ID { get; set; }
        public Guid? ProfileID { get; set; }
        public string Type { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public Guid? MonShiftID { get; set; }
        public Guid? TueShiftID { get; set; }
        public Guid? WedShiftID { get; set; }
        public Guid? ThuShiftID { get; set; }
        public Guid? FriShiftID { get; set; }
        public Guid? SatShiftID { get; set; }
        public Guid? SunShiftID { get; set; }
    }

    public class WorkdayConfig
    { 
        public string MaxHoursOneShift { get; set; }
        public string MaxHoursNextInOut { get; set; }
        public string MinMinutesSameAtt { get; set; }
        public string TypeLoadData { get; set; }
        public string SymbolIn { get; set; }
        public string SymbolOut { get; set; }
        public string DetectShift { get; set; }
        public string DetectWrongShift { get; set; }
    }
}
