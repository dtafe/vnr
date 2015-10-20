using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class CompensationDetailModel
    {
        public Guid ID { get; set; }
        public Guid? ProfileID { get; set; }
        public int? Year { get; set; }
        public DateTime? MonthYear { get; set; }
        public int? MonthBeginInYear { get; set; }
        public int? MonthResetInitAvailable { get; set; }
        public int? MonthStartProfile { get; set; }
        public double? Available { get; set; }
        public double? LeaveInMonth { get; set; }
        public double? OvertimeInMonth { get; set; }
        public double? TotalLeaveBef { get; set; }
        public double? TotalOvertimeBef { get; set; }
        public double? Remain { get; set; }
        public double? InitAvailable { get; set; }
    }
}
