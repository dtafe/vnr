using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class AnnualLeaveDetailModel
    {
        public Guid ID { get; set; }
        public DateTime? MonthStart { get; set; }
        public DateTime? MonthEnd { get; set; }
        public string Type { get; set; }
        public Guid? ProfileID { get; set; }
        public double? Month1 { get; set; }
        public double? Month2 { get; set; }
        public double? Month3 { get; set; }
        public double? Month4 { get; set; }
        public double? Month5 { get; set; }
        public double? Month6 { get; set; }
        public double? Month7 { get; set; }
        public double? Month8 { get; set; }
        public double? Month9 { get; set; }
        public double? Month10 { get; set; }
        public double? Month11 { get; set; }
        public double? Month12 { get; set; }
    }
}
