using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRM.Presentation.Service;

namespace HRM.Presentation.Attendance.Models
{
    public class Att_ImportLeavedayModel : BaseViewModel
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
            public const string ProfileID = "ProfileID";
            public const string CodeEmp = "CodeEmp";
            public const string ProfileName = "ProfileName";
            public const string CodeOrg = "CodeOrg";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string Duration = "Duration";
            public const string TotalDuration = "TotalDuration";
            public const string LeaveDays = "LeaveDays";
            public const string LeaveHours = "LeaveHours";
            public const string DurationType = "DurationType";
            public const string LeaveDayType = "LeaveDayType";
            public const string LeaveDayTypeID = "LeaveDayTypeID";
            public const string Description = "Description";
        }
    }
}
