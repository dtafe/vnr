using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class AttendanceTableModel
    {
        public Guid? ID { get; set; }
        public Guid? ProfileID { get; set; }
        public double? AnlDayTaken { get; set; }
        public double? AnlDayAdjacent { get; set; }
        public double? SickDayTaken { get; set; }
        public double? SickDayAdjacent { get; set; }
    }
}
