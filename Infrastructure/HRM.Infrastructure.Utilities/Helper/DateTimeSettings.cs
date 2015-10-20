
using System.Globalization;
namespace HRM.Infrastructure.Utilities.Helpers
{
    public class DateTimeSettings 
    {
        private string _datetimeformate = "dd/MM/yyyy";
        private string _formattime = "dd/MM/yyyy";

        /// <summary>
        /// Gets or sets a default store time zone identifier
        /// </summary>
        public string DefaultStoreTimeZoneId { get; set; }

        /// <summary>
        /// Gets or sets a default default format
        /// </summary>
        public string DefaultStringFormatDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether customers are allowed to select theirs time zone
        /// </summary>
        public bool AllowCustomersToSetTimeZone { get; set; }
    }
    public class WeekDay
    {
        private string _dayValue = string.Empty;
        public string DayValue
        {
            get { return _dayValue; }
            set { _dayValue = value; }
        }
        private string _dayName = string.Empty;
        public string DayName
        {
            get { return _dayName; }
            set { _dayName = value; }
        }
    }
    public enum Quarter
    {
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4
    }

    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }
}