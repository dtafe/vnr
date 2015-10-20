using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class CompensationConfigModel
    {
        public Guid ID { get; set; }
        public Guid? ProfileID { get; set; }
        public int? Year { get; set; }
        public int? MonthBeginInYear { get; set; }
        public double? InitAvailable { get; set; }
        public int? MonthResetInitAvailable { get; set; }
        public int? MonthStartProfile { get; set; }
    }
}
