using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class RosterGroupModel
    {
        public Guid ID { get; set; }
        public string RosterGroupName { get; set; }
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


    public class RosterGroupTableModel
    {
        public string RosterGroupName { get; set; }
        public DateTime Date { get; set; }
        public Guid ShifID { get; set; }
    }

}
