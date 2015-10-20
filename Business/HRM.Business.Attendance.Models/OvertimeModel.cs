using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class OvertimeModel
    {
        public Guid ID { get; set; }
        public Guid? OvertimeTypeID { get; set; }
        public string DurationType { get; set; }
        public DateTime WorkDate { get; set; }
        public double? ApproveHours { get; set; }
        public double? ConfirmHours { get; set; }
        public double? RegisterHours { get; set; }
        public Guid? ProfileID { get; set; }
    }
}
