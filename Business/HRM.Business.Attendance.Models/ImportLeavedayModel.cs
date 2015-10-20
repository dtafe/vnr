using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class ImportLeavedayModel
    {
        public Guid ProfileID { get; set; }
        public string CodeEmp { get; set; }
        public string ProfileName { get; set; }
        public string CodeOrg { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public double Duration { get; set; }
        public double TotalDuration { get; set; }
        public double LeaveDays { get; set; }
        public double LeaveHours { get; set; }
        public string DurationType { get; set; }
        public string LeaveDayType { get; set; }
        public Guid LeaveDayTypeID { get; set; }
        public string Description { get; set; }

        public partial class FieldNames
        {
            public const string LeaveDays = "LeaveDays";
            public const string LeaveHours = "LeaveHours";
            public const string LeaveDayType = "LeaveDayType";
            public const string Description = "Description";
        }
    }
}
