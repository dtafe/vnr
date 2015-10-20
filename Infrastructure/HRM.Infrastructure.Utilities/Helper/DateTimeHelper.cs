using System;
using System.Collections.Generic;
using System.Web;
using System.Globalization;
using System.Web.UI.WebControls;

namespace HRM.Infrastructure.Utilities.Helpers
{
    /// <summary>
    /// This class provides a set of commonly used DateTime functions and operators
    /// in a simpler syntax that the one required when manually coding.
    /// By  hien.pham
    /// </summary>
    public partial class DateTimeHelper
    {
        public DateTimeHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region Statands Datatime
        /// <summary>
        /// This method will take a DateTime string as the first parameter and the format
        /// of the DateTime string as the second parameter. It will then convert the string to DateTime

        /// object and return the newly create object
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <param name="stringFormat"></param>
        /// <returns></returns>
        public static DateTime ConvertStringToDateTime(string dateTimeString, string stringFormat)
        {
            if (dateTimeString != "" && stringFormat != "")
            {
                try
                {
                    DateTime result = DateTime.ParseExact(dateTimeString, stringFormat, CultureInfo.InvariantCulture);
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            else
            {
                throw new Exception("Both parameters must have value (dateTimeString, stringFormat)!");
            }
        }

        /// <summary>
        /// This method will return an integer representing the number of days between two
        /// particular dates passed to a method
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int GetDaysDifference(DateTime startDate, DateTime endDate)
        {
            TimeSpan timeDifference = endDate - startDate;

            try
            {
                return Int32.Parse(timeDifference.TotalDays.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// This method will return an integer representing the number of months between
        /// particular dates passed to a method
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int GetMonthsCountBetweenDates(DateTime startDate, DateTime endDate)
        {
            TimeSpan timeDifference = endDate - startDate;

            DateTime resultDate = DateTime.MinValue + timeDifference;

            int monthDifference = resultDate.Month - 1;

            return monthDifference;
        }

        /// <summary>
        /// This method will take 2 dates as input and return the Generic List of DateTime objects. Each
        /// object represent 1 of the months between the 2 dates and is set in the dd/MM/yyyy
        /// the current day of the month is set to 1. For example: 01/01/2010, 01/02/2010,
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static List<DateTime> GetMonthsBetweenDates(DateTime startDate, DateTime endDate)
        {
            TimeSpan timeDifference = endDate - startDate;

            DateTime resultDate = DateTime.MinValue + timeDifference;

            int monthDifference = resultDate.Month - 1;

            if (monthDifference > 0)
            {
                List<DateTime> result = new List<DateTime>();
                for (int i = 0; i < monthDifference; i++)
                {
                    DateTime tempDate = startDate.AddMonths(1);
                    DateTime dateToAdd = new DateTime(tempDate.Date.Year, tempDate.Month, 1);

                    result.Add(dateToAdd);
                }
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns an indication whether the year passed as method parameter is a leap year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsLeapYear(int year)
        {
            return DateTime.IsLeapYear(year);
        }

        /// <summary>
        /// This method returns a string array of week days which can be used for DropDownLists
        /// </summary>
        /// <returns></returns>
        public static string[] WeekDaysAsStringArray()
        {
            string[] result = new string[7];

            result[0] = "Monday";
            result[1] = "Tuesday";
            result[2] = "Wednesday";
            result[3] = "Thursday";
            result[4] = "Friday";
            result[5] = "Saturday";
            result[6] = "Sunday";

            return result;
        }

        /// <summary>
        /// This method returns a WeekDay (Custom class) Generic List of week days which can be used for DropDownLists and such
        /// </summary>
        /// <returns></returns>
        public static List<WeekDay> WeekDaysAsGenericCollection()
        {
            List<WeekDay> result = new List<WeekDay>();

            WeekDay tempDay = new WeekDay();
            tempDay.DayValue = "1";
            tempDay.DayName = "Monday";
            result.Add(tempDay);

            tempDay = new WeekDay();
            tempDay.DayValue = "2";
            tempDay.DayName = "Tuesday";
            result.Add(tempDay);

            tempDay = new WeekDay();
            tempDay.DayValue = "3";
            tempDay.DayName = "Wednesday";
            result.Add(tempDay);

            tempDay = new WeekDay();
            tempDay.DayValue = "4";
            tempDay.DayName = "Thursday";
            result.Add(tempDay);

            tempDay = new WeekDay();
            tempDay.DayValue = "5";
            tempDay.DayName = "Friday";
            result.Add(tempDay);

            tempDay = new WeekDay();
            tempDay.DayValue = "6";
            tempDay.DayName = "Saturday";
            result.Add(tempDay);

            tempDay = new WeekDay();
            tempDay.DayValue = "7";
            tempDay.DayName = "Sunday";
            result.Add(tempDay);

            return result;
        }

        /// <summary>
        /// This method returns a ListItem Generic List of week days which can be used for DropDownLists and such
        /// </summary>
        /// <returns></returns>
        public List<ListItem> WeekDaysAsListItemCollection()
        {
            List<ListItem> result = new List<ListItem>();

            ListItem tempItem = new ListItem();
            tempItem.Value = "1";
            tempItem.Text = "Monday";
            result.Add(tempItem);

            tempItem = new ListItem();
            tempItem.Value = "2";
            tempItem.Text = "Tuesday";
            result.Add(tempItem);

            tempItem = new ListItem();
            tempItem.Value = "3";
            tempItem.Text = "Wednesday";
            result.Add(tempItem);

            tempItem = new ListItem();
            tempItem.Value = "4";
            tempItem.Text = "Thursday";
            result.Add(tempItem);

            tempItem = new ListItem();
            tempItem.Value = "5";
            tempItem.Text = "Friday";
            result.Add(tempItem);

            tempItem = new ListItem();
            tempItem.Value = "6";
            tempItem.Text = "Saturday";
            result.Add(tempItem);

            tempItem = new ListItem();
            tempItem.Value = "7";
            tempItem.Text = "Sunday";
            result.Add(tempItem);

            return result;
        }

        #endregion

        #region Quarters
        public DateTime GetStartOfQuarter(int Year, Quarter Qtr)
        {
            if (Qtr == Quarter.First)    // 1st Quarter = January 1 to March 31
                return new DateTime(Year, 1, 1, 0, 0, 0, 0);
            else if (Qtr == Quarter.Second) // 2nd Quarter = April 1 to June 30
                return new DateTime(Year, 4, 1, 0, 0, 0, 0);
            else if (Qtr == Quarter.Third) // 3rd Quarter = July 1 to September 30
                return new DateTime(Year, 7, 1, 0, 0, 0, 0);
            else // 4th Quarter = October 1 to December 31
                return new DateTime(Year, 10, 1, 0, 0, 0, 0);
        }

        public DateTime GetEndOfQuarter(int Year, Quarter Qtr)
        {
            if (Qtr == Quarter.First)    // 1st Quarter = January 1 to March 31
                return new DateTime(Year, 3,
                       DateTime.DaysInMonth(Year, 3), 23, 59, 59, 999);
            else if (Qtr == Quarter.Second) // 2nd Quarter = April 1 to June 30
                return new DateTime(Year, 6,
                       DateTime.DaysInMonth(Year, 6), 23, 59, 59, 999);
            else if (Qtr == Quarter.Third) // 3rd Quarter = July 1 to September 30
                return new DateTime(Year, 9,
                       DateTime.DaysInMonth(Year, 9), 23, 59, 59, 999);
            else // 4th Quarter = October 1 to December 31
                return new DateTime(Year, 12,
                       DateTime.DaysInMonth(Year, 12), 23, 59, 59, 999);
        }

        public Quarter GetQuarter(Month Month)
        {
            if (Month <= Month.March)
                // 1st Quarter = January 1 to March 31
                return Quarter.First;
            else if ((Month >= Month.April) && (Month <= Month.June))
                // 2nd Quarter = April 1 to June 30
                return Quarter.Second;
            else if ((Month >= Month.July) && (Month <= Month.September))
                // 3rd Quarter = July 1 to September 30
                return Quarter.Third;
            else // 4th Quarter = October 1 to December 31
                return Quarter.Fourth;
        }

        public DateTime GetEndOfLastQuarter()
        {
            if ((Month)DateTime.Now.Month <= Month.March)
                //go to last quarter of previous year
                return GetEndOfQuarter(DateTime.Now.Year - 1, Quarter.Fourth);
            else //return last quarter of current year
                return GetEndOfQuarter(DateTime.Now.Year,
                  GetQuarter((Month)DateTime.Now.Month));
        }

        public DateTime GetStartOfLastQuarter()
        {
            if ((Month)DateTime.Now.Month <= Month.March)
                //go to last quarter of previous year
                return GetStartOfQuarter(DateTime.Now.Year - 1, Quarter.Fourth);
            else //return last quarter of current year
                return GetStartOfQuarter(DateTime.Now.Year,
                  GetQuarter((Month)DateTime.Now.Month));
        }

        public DateTime GetStartOfCurrentQuarter()
        {
            return GetStartOfQuarter(DateTime.Now.Year,
                   GetQuarter((Month)DateTime.Now.Month));
        }

        
        #endregion

        #region Weeks
        public static DateTime GetStartOfLastWeek()
        {
            int DaysToSubtract = (int)DateTime.Now.DayOfWeek + 7;
            DateTime dt =
              DateTime.Now.Subtract(System.TimeSpan.FromDays(DaysToSubtract));
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfLastWeek()
        {
            DateTime dt = GetStartOfLastWeek().AddDays(6);
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59, 999);
        }

        public static DateTime GetStartOfCurrentWeek()
        {
            int DaysToSubtract = (int)DateTime.Now.DayOfWeek;
            DateTime dt =
              DateTime.Now.Subtract(System.TimeSpan.FromDays(DaysToSubtract));
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfCurrentWeek()
        {
            DateTime dt = GetStartOfCurrentWeek().AddDays(6);
            return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59, 999);
        }
        #endregion

        #region Months

        public static DateTime GetStartOfMonth(Month Month, int Year)
        {
            return new DateTime(Year, (int)Month, 1, 0, 0, 0, 0);
        }

        public static DateTime GetStartOfMonth(int Month, int Year)
        {
            return new DateTime(Year, Month, 1, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfMonth(Month Month, int Year)
        {
            return new DateTime(Year, (int)Month,
               DateTime.DaysInMonth(Year, (int)Month), 23, 59, 59, 999);
        }

        public static DateTime GetEndOfMonth(int Month, int Year)
        {
            return new DateTime(Year, Month,
               DateTime.DaysInMonth(Year, Month), 23, 59, 59, 999);
        }

        public static DateTime GetStartOfLastMonth()
        {
            if (DateTime.Now.Month == 1)
                return GetStartOfMonth(12, DateTime.Now.Year - 1);
            else
                return GetStartOfMonth(DateTime.Now.Month - 1, DateTime.Now.Year);
        }

        public static DateTime GetEndOfLastMonth()
        {
            if (DateTime.Now.Month == 1)
                return GetEndOfMonth(12, DateTime.Now.Year - 1);
            else
                return GetEndOfMonth(DateTime.Now.Month - 1, DateTime.Now.Year);
        }

        public static DateTime GetStartOfCurrentMonth()
        {
            return GetStartOfMonth(DateTime.Now.Month, DateTime.Now.Year);
        }

        public static DateTime GetEndOfCurrentMonth()
        {
            return GetEndOfMonth(DateTime.Now.Month, DateTime.Now.Year);
        }
        #endregion

        #region Years
        public static DateTime GetStartOfYear(int Year)
        {
            return new DateTime(Year, 1, 1, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfYear(int Year)
        {
            return new DateTime(Year, 12,
              DateTime.DaysInMonth(Year, 12), 23, 59, 59, 999);
        }

        public static DateTime GetStartOfLastYear()
        {
            return GetStartOfYear(DateTime.Now.Year - 1);
        }

        public static DateTime GetEndOfLastYear()
        {
            return GetEndOfYear(DateTime.Now.Year - 1);
        }

        public static DateTime GetStartOfCurrentYear()
        {
            return GetStartOfYear(DateTime.Now.Year);
        }

        public static DateTime GetEndOfCurrentYear()
        {
            return GetEndOfYear(DateTime.Now.Year);
        }
        #endregion

        #region Days
        public static DateTime GetStartOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public static DateTime GetEndOfDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month,
                                 date.Day, 23, 59, 59, 999);
        }
        #endregion
    }
}