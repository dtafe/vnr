using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class PregnancyModel
    {
        public Guid ID { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string TypePregnancyEarly { get; set; }
        public Guid? ProfileID { get; set; }
    }
}
