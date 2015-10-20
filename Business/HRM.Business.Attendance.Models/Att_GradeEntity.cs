using HRM.Business.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class Att_GradeEntity : HRMBaseModel
    {
        public Guid? ProfileID { get; set; }
        public string ProfileName { get; set; }
        public Guid? GradeAttendanceID { get; set; }
        public string GradeAttendanceName { get; set; }
        public DateTime? MonthStart { get; set; }
        public DateTime? MonthEnd { get; set; }
        public string ProfileIDs { get; set; }
        public string OrgStructureID { get; set; }
    }
}
