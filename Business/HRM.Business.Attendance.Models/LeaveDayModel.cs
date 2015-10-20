using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Business.Attendance.Models
{
    public class LeaveDayModel
    {
        public Guid Id { get; set; }
        public Guid LeaveDayTypeID { get; set; }
        public string DurationType { get; set; }
        public double? LeaveHours { get; set; }
        public double? LeaveDays { get; set; }
        public Guid? ProfileID { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
    public class LeaveDayValidate
    {
        private Guid _ProfileID;
        public Guid ProfileID
        {
            get
            {
                return _ProfileID;
            }
            set
            {
                _ProfileID = value;
            }
        }
        private DateTime _DateStart;
        public DateTime DateStart
        {
            get
            {
                return _DateStart;
            }
            set
            {
                _DateStart = value;
            }

        }
        private DateTime _DateEnd;
        public DateTime DateEnd
        {
            get
            {
                return _DateEnd;
            }
            set
            {
                _DateEnd = value;
            }

        }

        private double _Duration;
        public double Duration
        {
            get
            {
                return _Duration;
            }
            set
            {
                _Duration = value;
            }

        }
        private double _TotalDuration;
        public double TotalDuration
        {
            get
            {
                return _TotalDuration;
            }
            set
            {
                _TotalDuration = value;
            }

        }

        public class FieldNames
        {
            public const string ProfileID = "ProfileID";
            public const string DateStart = "DateStart";
            public const string DateEnd = "DateEnd";
            public const string Duration = "Duration";
            public const string TotalDuration = "TotalDuration";
        }

    }
}
