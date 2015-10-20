namespace HRM.Infrastructure.Utilities
{
    /// <summary>
    /// Sử dụng cho mục đích tạo ra biến không thay đổi cho các chuổi đinh dạng
    /// vd: 
    /// </summary>
    public class ConstantFormat
    {
        #region General
        /// <summary>
        /// Định dạng dd/mm/yyyy hh:mm
        /// vd: 20/05/2014 18:30
        /// </summary>
        public const string HRM_Format_Grid_LongDate = "{0:dd/MM/yyyy HH:mm}";

        // Format grid HH:mm
        public const string HRM_Format_Grid_ShortTime = "HH:mm:ss";


        /// <summary>
        /// Định dạng dd/mm/yyyy HH:mm:ss
        /// vd: 20/05/2014 18:30:20
        /// </summary>
        public const string HRM_Format_Grid_LongDate2 = "{0:dd/MM/yyyy HH:mm:ss}";

        /// <summary>
        /// Định dạng mm/yyyy 
        /// vd: 05/2014 
        /// </summary>
        public const string HRM_Format_Grid_MonthYear = "{0:MM/yyyy}";

        /// <summary>
        /// Định dạng mm/dd/yyyy 
        /// vd: 02/05/2014 
        /// </summary>
        public const string HRM_Format_Grid_MonthDayYear = "{0:MM/dd/yyyy}";

        /// <summary>
        /// Định dạng dd/MM/yyyy 
        /// vd: 02/05/2014 
        /// </summary>
        public const string HRM_Format_DayMonthYear = "dd/MM/yyyy";

        /// <summary>
        /// Định dạng dd/MM/yyyy HH:mm
        /// vd: 02/05/2014 12:30
        /// </summary>
        public const string HRM_Format_DayMonthYear_HoursMin = "dd/MM/yyyy HH:mm";

        /// <summary>
        /// Định dạng dd/MM/yyyy HH:mm:ss
        /// vd: 02/05/2014 12:30:59
        /// </summary>
        public const string HRM_Format_DayMonthYear_HoursMinSecond = "dd/MM/yyyy HH:mm:ss";

        /// <summary>
        /// Định dạng dd/MM/yyyy HH:mm:ss tt
        /// vd: 02/05/2014 12:30:59 PM
        /// </summary>
        public const string HRM_Format_DayMonthYear_HoursMinSecond_TT = "dd/MM/yyyy HH:mm:ss tt";

        /// <summary>
        /// Định dạng yyyy-MM-dd HH:mm:ss
        /// vd: 2014-05-01 12:30:59.000000
        /// </summary>
        public const string HRM_Format_YearMonthDay_HoursMinSecond_ffffff = "yyyy-MM-dd HH:mm:ss.ffffff";


        /// <summary>
        /// Định dạng dd/MM/yyyy hh:mm
        /// vd: 02/05/2014 12:30
        /// </summary>
        public const string HRM_Format_DayMonthYearHoursMinSec = "dd/MM/yyyy hh:mm:ss tt";

        /// <summary>
        /// Định dạng dd/MM/yyyy HH:mm tt
        /// vd: 02/05/2014 12:30 AM
        /// </summary>
        public const string HRM_Format_DayMonthYear_HoursMin_TT = "dd/MM/yyyy hh:mm tt";

        /// <summary>
        /// Định dạng MM/yyyy 
        /// vd: 05/2014 
        /// </summary>
        public const string HRM_Format_MonthYear = "MM/yyyy";

        /// <summary>
        /// Định dạng dd/MM
        /// </summary>
        public const string HRM_Format_DayMonth = "dd/MM";

        /// <summary>
        /// Định dạng HH:mm 
        /// vd: 12:30 
        /// </summary>
        public const string HRM_Format_HourMin = "HH:mm";

        /// <summary>
        /// Định dạng HH:mm tt
        /// vd: 12:30 AM
        /// </summary>
        public const string HRM_Format_HourMin_TT = "hh:mm tt";
        /// <summary>
        /// Định dạng hh:mm tt khi xuất excel
        /// vd: 12:30 AM
        /// </summary>
        public const string HRM_Format_HourMin_AM_PM = "hh:mm AM/PM";


        /// <summary>
        /// Định dạng hh:mm:ss tt
        /// vd: 12:30:08 AM
        /// </summary>
        public const string HRM_Format_HourMinSecond_TT = "hh:mm:ss tt";

        /// <summary>
        /// Định dạng hh:mm:ss tt
        /// vd: 12:30:08 AM
        /// </summary>
        public const string HRM_Format_HourMinSecond = "HH:mm:ss";

        /// <summary>
        /// Định dạng số nguyên 
        /// vd: 12 
        /// </summary>
        public const string HRM_Format_Int = "n0";

        /// <summary>
        /// Định dạng yyyy 
        /// vd: 2014 
        /// </summary>
        public const string HRM_Format_Year = "yyyy";

        /// <summary>
        /// Định dạng yyyy-MM-dd HH:mm:ss.ffffff
        /// </summary>
        public const string HRM_Format_YearMonthDay_HoursMinSecond = "yyyy-MM-dd HH:mm:ss.ffffff";

        /// <summary>
        /// Định dạng số nguyên 
        /// vd: 21000 
        /// </summary>
        public const string HRM_Format_Number_Integer = "0";

        /// <summary>
        /// Định dạng thập phân khi xuất excel
        /// vd: 21.0 
        /// </summary>
        public const string HRM_Format_Number_Double = "0.0";

        /// <summary>
        /// Định dạng thập phân
        /// vd: 25.25
        /// </summary>
        public const string HRM_Format_Number_Double2 = "0.00";

        /// <summary>
        /// Định dạng tiền
        /// vd: 21,000 
        /// </summary>
        public const string HRM_Format_Money = "n0";

        /// <summary>
        /// Định dạng Phần trăm
        /// vd: 21%
        /// </summary>
        public const string HRM_Format_Percent = "{0:# \\%}";

        /// <summary>
        /// Định dạng Phần trăm cho lưới
        /// vd: 21%
        /// </summary>
        public const string HRM_Format_Grid_Percent = "p";

        /// <summary>
        /// Định dạng MM/dd/yyyy HH:mm tt
        /// vd: 02/05/2014 12:30 AM
        /// </summary>
        public const string HRM_Format_MonthDayYear_HoursMin_TT = "MM/dd/yyyy hh:mm tt";


        #endregion
    }
}
