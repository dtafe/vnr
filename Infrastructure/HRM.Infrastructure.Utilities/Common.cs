using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Configuration;
using System.IO;
using System.Linq;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using Evaluant.Calculator;
using Evaluant.Calculator.Domain;
using HRM.Infrastructure.Utilities.Helper;
using System.Collections;
using System.Data.SqlTypes;
using VnResource.Helper.Data;
using VnResource.Helper.Setting;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;

namespace HRM.Infrastructure.Utilities
{
    public static class Common
    {
        public static string HostPath;
        //HieuVan: Chuyển 1 List thành DataTable
        public static DataTable ToDataTable<T>(List<T> items)
        {

            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }




        private static int _screenWidth = 0;
        public static int ScreenWidth
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session[SessionObjects.ScreenWidth] != null)
                    {
                        _screenWidth = (int)HttpContext.Current.Session[SessionObjects.ScreenWidth];
                    }
                }
                return _screenWidth;
            }
        }

        public const int ThreadSleep = 100;
        public const int LockTimeOut = 15;
        /// <summary>
        /// Lam tron so tien 
        /// </summary>
        /// <param name="money"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static Double GetRoundMoney(Double money, int countNumber)
        {
            String a = "1";
            for (int i = 0; i < countNumber; i++)
            {
                a += "0";
            }
            int number = Convert.ToInt32(a);
            Double numberPresent = Math.Round(Math.Round(money) / number, 0, MidpointRounding.AwayFromZero);
            Double res = numberPresent * number;
            return res;
        }

        #region Send mail

        /// <summary>
        /// Hieu.Van
        /// Hàm gửi email
        /// </summary>
        /// <param name="Host"></param>
        /// <param name="Port"></param>
        /// <param name="MailFrom"></param>
        /// <param name="MailUserName"></param>
        /// <param name="MailPassword"></param>
        /// <param name="EmailTest"></param>
        /// <param name="IsSSL"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool ProcessSendMail(string subject, string Host, int Port, string MailFrom, string MailUserName, string MailPassword, string EmailTo, bool IsSSL, string body, string fullPath, List<string> lstCcMail)
        {
            var isSuccess = false;
            #region Sent Mail
            try
            {
                MailAddress to = new MailAddress(EmailTo);
                MailAddress from = new MailAddress(MailFrom);
                MailMessage mail = new MailMessage(from, to);
                mail.Subject = subject;

                if (fullPath != null && fullPath != string.Empty)
                {
                    string Filename = fullPath.Split('/').LastOrDefault();
                    string dirpath = Common.GetPath(Common.DownloadURL);
                    string attachmentFilename = dirpath + Filename;
                    attachmentFilename = attachmentFilename.Replace("/", "\\");

                    Attachment attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                    disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                    disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                    disposition.FileName = Path.GetFileName(attachmentFilename);
                    disposition.Size = new FileInfo(attachmentFilename).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    mail.Attachments.Add(attachment);
                }
                if (lstCcMail != null && lstCcMail.Count > 0)
                {
                    foreach (var CcMail in lstCcMail)
                    {
                        MailAddress cc = new MailAddress(CcMail);
                        mail.CC.Add(cc);
                    }
                }
                
                mail.IsBodyHtml = true;
                body = HttpUtility.HtmlDecode(body);
                mail.Body = body;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = Host;
                smtp.Port = Port;

                smtp.Credentials = new NetworkCredential(MailFrom, MailPassword);
                smtp.EnableSsl = IsSSL;


                smtp.Send(mail);
                isSuccess = true;
            }
            catch (Exception)
            {
                isSuccess = false;
            }
            #endregion
            return isSuccess;
        }

        #endregion

        private static string ScreenScrapeHtml(string p)
        {
            throw new NotImplementedException();
        }

        #region Thao tác với XML

        public static List<T> GetDataFromXml<T>(string xmlFile, string tableName) where T : class
        {
            DataSet dataSet = new DataSet();
            if (!string.IsNullOrEmpty(xmlFile))
            {
                if (File.Exists(xmlFile))
                {
                    dataSet.ReadXml(xmlFile);
                    if (dataSet.Tables.Count > 0)
                    {
                        var tableData = dataSet.FindTable(tableName);
                        var modelData = tableData.Translate<T>();
                        return modelData;
                    }
                }
                else
                {
                    var settingPath = Common.GetPath("Settings");
                    if (Directory.Exists(settingPath))
                    {
                        string filePath = settingPath + "\\" + xmlFile;
                        if (File.Exists(filePath))
                        {
                            dataSet.ReadXml(filePath);
                            return dataSet.FindTable(tableName).Translate<T>();
                        }
                    }
                }
            }

            return null;
        }

        public static DataTable FindTable(this DataSet dataSet, string tableName)
        {
            if (dataSet.Tables.Count > 0)
            {
                foreach (DataTable table in dataSet.Tables)
                {
                    if (table.TableName == tableName)
                    {
                        return table;
                    }
                }
            }

            return new DataTable();
        }

        #endregion

        #region Copy Data

        /// <summary>
        /// Copy data từ một đối tượng sang một đối tượng khác
        /// </summary>
        /// <typeparam name="T">Đối tượng đầu ra của dữ liệu</typeparam>
        /// <param name="sourceData">Dữ liệu đầu vào để copy data</param>
        /// <returns></returns>
        public static T Copy<T>(this object sourceData) where T : class, new()
        {
            if (sourceData != null)
            {
                var sourceType = sourceData.GetType();
                if (sourceType.IsClass)
                {
                    T entity = new T();
                    var sourceProperties = sourceType.GetProperties();
                    foreach (var property in sourceProperties)
                    {
                        if (entity.HasProperty(property.Name))
                        {
                            entity.SetPropertyValue(property.Name, sourceData.GetPropertyValue(property.Name));
                        }
                    }

                    return entity;
                }

                return null;
            }

            return null;
        }

        #endregion

        public static TEnumType GetEnumValue<TEnumType>(string value)
        {
            int index = Enum.GetNames(typeof(TEnumType)).ToList().IndexOf(value);
            if (index == -1)
                throw new VNRException(ExceptionType.FRAMEWORK, "Wrong type format");
            return (TEnumType)Enum.GetValues(typeof(TEnumType)).Get(index);
        }

        /// <summary>
        /// Kiểm tra đối tượng có giá trị không
        /// </summary>
        /// <param name="obj">Đối tượng cần kiểm tra</param>
        /// <returns></returns>
        public static bool HasValue(this object obj)
        {
            var defaultdate = new DateTime(0001, 01, 01);
            bool isHasValue = true;

            //Nếu là null
            if (obj == null)
            {
                isHasValue = false;
            }
            //Nếu là kiểu Guid thì kiểm tra có giá trị không
            else if (obj is Guid)
            {
                if ((Guid)obj == Guid.Empty)
                {
                    isHasValue = false;
                }
            }
            //Nếu là kiểu ngày tháng thì kiểm tra có null ko và có để ngày mặc định với giá trị ko có ?
            else if (obj is DateTime)
            {
                if ((DateTime)obj.GetPropertyValue("Date") == defaultdate.Date)
                {
                    isHasValue = false;
                }
            }
            // Nếu giá trị là chuổi rỗng hoặc khoảng trắng
            else if (string.IsNullOrWhiteSpace(obj.GetString()))
            {
                isHasValue = false;
            }

            return isHasValue;
        }

        public static byte[] ListNumbersToBinary(this List<int> orderNumbers)
        {
            if (orderNumbers.Count == 0)
                return new byte[0];
            int maxOrder = orderNumbers.Max();
            int byteNumbers = (maxOrder / 8) + 1;
            byte[] bytes = new byte[byteNumbers];
            foreach (int orderNumber in orderNumbers)
            {
                int index = orderNumber / 8;
                int offset = orderNumber % 8;
                bytes[index] = (byte)(bytes[index] | 1 << offset);
            }
            return bytes;
        }
        public static List<int> GetListNumbersFromBinary(this byte[] binary)
        {
            List<int> result = new List<int>();
            if (binary == null)
                return result;
            byte[] bytes = binary.Clone() as byte[];//.ToArray();
            for (int i = 0; i < binary.Length; i++)
            {
                byte temp = bytes[i];
                int count = 0;
                while (temp > 0)
                {
                    if (temp % 2 == 1)
                        result.Add(count + i * 8);
                    temp = (byte)(temp >> 1);
                    count++;
                }
            }
            return result;
        }

        /// <summary>
        /// So sanh 2 đối tượng theo toán tử truyền vào
        /// </summary>
        /// <param name="value">Giá trị</param>
        /// <param name="valueCompare">Giá trị compare</param>
        /// <param name="strOperator">Default: > </param>
        /// <returns></returns>
        public static bool CompareByOperator(this object value, object valueCompare, string strOperator)
        {
            bool check = true;
            if (value != null && valueCompare != null)
            {
                var resultCompare = value.Compare(valueCompare);
                if (strOperator == "=")
                {
                    strOperator = "==";
                }
                else if (strOperator == ">>=")
                {
                    strOperator = "<=";
                }
                else if (strOperator == ">>")
                {
                    strOperator = "<";
                }
                switch (strOperator)
                {
                    case ">":
                        if (resultCompare <= 0) check = false;
                        break;
                    case "<":
                        if (resultCompare >= 0) check = false;
                        break;
                    case "<=":
                        if (resultCompare > 0) check = false;
                        break;
                    case "==":
                        if (resultCompare > 0 || resultCompare < 0) check = false;
                        break;
                    default:
                        if (resultCompare < 0) check = false;
                        break;
                }
            }

            return check;
        }

        public static void RemoveRange(this IList parent, IList child)
        {
            if (parent == null)
                return;
            for (int i = 0; i < child.Count; i++)
                parent.Remove(child[i]);
        }

        /// <summary>
        /// Kiểm tra xem có phải là phương thức + - * / hay không
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsExpression(string value)
        {
            if (value == "+" || value == "-" || value == "*" || value == "/")
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Kiểm tra chuỗi có phải là chuỗi số thực hay không
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumber(string value)
        {
            float t;
            bool result = float.TryParse(value, out t);
            return result;
        }
        public static int getAllSundayInMonth(DateTime dateFrom, DateTime dateTo)
        {
            int countSundayInYear = 0;
            for (DateTime idx = dateFrom; idx <= dateTo; idx = idx.AddDays(1))
            {
                //DateTime dt = dateFrom;
                if (idx.DayOfWeek == DayOfWeek.Sunday)
                    countSundayInYear++;
            }
            return countSundayInYear;
        }
        /// <summary>
        /// Lay tong so ngay trong nam
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int GetDaysInAYear(int year)
        {
            DateTime date = new DateTime(year, 12, 31);
            return date.DayOfYear; ;
        }
        /// <summary>
        /// This function will return all the dates of sundays in a Year
        /// for example if you pass if year is 2009
        /// it will return all the dates of sundays in the year in form of list
        /// </summary>
        /// <param name="year">Year</param>
        /// <returns>List<String> all dates of sundays in form of list string </returns>
        public static int getAllSundayInYear(int Year)
        {
            int countSundayInYear = 0;
            for (int month = 1; month <= 12; month++)
            {
                DateTime dt = new DateTime(Year, month, 1);
                int firstSundayOfMonth = (int)dt.DayOfWeek;
                if (firstSundayOfMonth != 0)
                {
                    dt = dt.AddDays((6 - firstSundayOfMonth) + 1);
                }
                while (dt.Month == month)
                {
                    countSundayInYear++;
                    dt = dt.AddDays(7);
                }
            }
            return countSundayInYear;
        }
        /// <summary>
        /// Nối Chuỗi giờ vào 1 Ngày 
        /// VD: Nối string "12:34:56" vào Date 01/02/2014
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static DateTime JoinTimeInDate(DateTime _date, string time)
        {
            if (time == null)
                return _date;

            var _arr = time.Split(':');
            if (_arr[0].ToString().Equals("__"))
                _arr[0] = "00";
            if (_arr[1].ToString().Equals("__"))
                _arr[1] = "00";
            if (_arr[2].ToString().Equals("__"))
                _arr[2] = "00";
            TimeSpan _time = new TimeSpan(int.Parse(_arr[0].ToString()), int.Parse(_arr[1].ToString()), int.Parse(_arr[2].ToString()));
            double _hours = _time.TotalHours;
            return _date.AddHours(_hours);
        }

        /// <summary>
        /// Kiểm tra 2 mảng from-to có bị chồng chéo với nhau hay không?
        /// </summary>
        /// <param name="dateFrom1">From của mảng thứ nhất</param>
        /// <param name="dateTo1">To của mảng thứ nhất</param>
        /// <param name="dateFrom2">From của mảng thứ hai</param>
        /// <param name="dateTo2">To của mảng thứ hai</param>
        /// <returns>Giao hay chồng chéo nhau</returns>
        public static bool IsOverlap(DateTime dateFrom1, DateTime dateTo1, DateTime dateFrom2, DateTime dateTo2)
        {
            return dateFrom1 <= dateTo2 && dateTo1 >= dateFrom2;//2 mảng form-to có giao nhau ít nhất 1 điểm
        }

        public static List<SelectListItem> Day()
        {
            List<SelectListItem> lstDay = new List<SelectListItem> 
            {
            new SelectListItem {Text = "", Value = ""},
            new SelectListItem {Text = "01", Value = "1"},
            new SelectListItem {Text = "02", Value = "2"},
            new SelectListItem {Text = "03", Value = "3"},
            new SelectListItem {Text = "04", Value = "4"},
            new SelectListItem {Text = "05", Value = "5"},
            new SelectListItem {Text = "06", Value = "6"},
            new SelectListItem {Text = "07", Value = "7"},
            new SelectListItem {Text = "08", Value = "8"},
            new SelectListItem {Text = "09", Value = "9"},
            new SelectListItem {Text = "10", Value = "10"},
            new SelectListItem {Text = "11", Value = "11"},
            new SelectListItem {Text = "12", Value = "12"},
            new SelectListItem {Text = "13", Value = "13"},
            new SelectListItem {Text = "14", Value = "14"},
            new SelectListItem {Text = "15", Value = "15"},
            new SelectListItem {Text = "16", Value = "16"},
            new SelectListItem {Text = "17", Value = "17"},
            new SelectListItem {Text = "18", Value = "18"},
            new SelectListItem {Text = "19", Value = "19"},
            new SelectListItem {Text = "20", Value = "20"},
            new SelectListItem {Text = "21", Value = "21"},
            new SelectListItem {Text = "22", Value = "22"},
            new SelectListItem {Text = "23", Value = "23"},
            new SelectListItem {Text = "24", Value = "24"},
            new SelectListItem {Text = "25", Value = "25"},
            new SelectListItem {Text = "26", Value = "26"},
            new SelectListItem {Text = "27", Value = "27"},
            new SelectListItem {Text = "28", Value = "28"},
            new SelectListItem {Text = "29", Value = "29"},
            new SelectListItem {Text = "30", Value = "30"},
            new SelectListItem {Text = "31", Value = "31"},
            };

            return lstDay;
        }

        public static List<SelectListItem> Time()
        {
            List<SelectListItem> lstTime = new List<SelectListItem> 
            {
            new SelectListItem {Text = "Lần 1", Value = "1"},
            new SelectListItem {Text = "Lần 2", Value = "2"},
            };
            return lstTime;
        }

        public static List<SelectListItem> Month()
        {

            List<SelectListItem> lstMonth = new List<SelectListItem> 
            { 
            new SelectListItem {Text = "", Value = ""},
            new SelectListItem {Text = "01", Value = "1"},
            new SelectListItem {Text = "02", Value = "2"},
            new SelectListItem {Text = "03", Value = "3"},
            new SelectListItem {Text = "04", Value = "4"},
            new SelectListItem {Text = "05", Value = "5"},
            new SelectListItem {Text = "06", Value = "6"},
            new SelectListItem {Text = "07", Value = "7"},
            new SelectListItem {Text = "08", Value = "8"},
            new SelectListItem {Text = "09", Value = "9"},
            new SelectListItem {Text = "10", Value = "10"},
            new SelectListItem {Text = "11", Value = "11"},
            new SelectListItem {Text = "12", Value = "12"},
            
            };
            return lstMonth;
        }

        public static bool IsAscii(string part)
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes(part);
            string partAscii = Encoding.ASCII.GetString(asciiBytes);
            return part == partAscii;
        }
        /// <summary>
        /// Get the hour in DateTime format
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeHour(Double hourToConvert)
        {
            int minute = Convert.ToInt32(Math.Floor(hourToConvert * 60));
            return GetDateTimeHour(minute);
        }

        #region Config Table
        public static DataTable ConfigTable(this DataTable dtSource)
        {
            return dtSource.ConfigTable(null);
        }
        public static DataTable ConfigTable(this DataTable dtSource, bool IsFormatNumber)
        {
            return dtSource.ConfigTable(null, IsFormatNumber);
        }
        public static DataTable ConfigTable(this DataTable dtSource, Dictionary<string, Dictionary<string, object>> config)
        {
            return dtSource.ConfigTable(config, false);
        }
        /// <summary>
        /// [Chuc.Nguyen] - Cấu hình datatable cho lưới động
        /// </summary>
        /// <param name="dtSource">Datatable chứa dữ liệu</param>
        /// <returns></returns>
        public static DataTable ConfigTable(this DataTable dtSource, Dictionary<string, Dictionary<string, object>> config, bool IsFormatNumber)
        {

            var coColumns = dtSource.Columns.Count;
            var columns = new List<Dictionary<string, object>>();
            var colTypes = new Dictionary<string, Dictionary<string, object>>();

            for (int i = 0; i < coColumns; i++)
            {
                var column = new Dictionary<string, object>();
                var dicType = new Dictionary<string, object>();
                var colName = dtSource.Columns[i].ColumnName;
                var colCaption = dtSource.Columns[i].Caption;
                Type colType = dtSource.Columns[i].DataType;

                //Xóa các ký tự đặc biệt khi chuyển dữ liệu thành tên column
                string newColName = Regex.Replace(colName, @"[^0-9a-zA-Z_]+", "");
                //Kiểm tra nếu tên column bắt đầu bằng số thì xử lý thêm dấu '_' cho hợp lệ
                if (Regex.IsMatch(newColName, @"^\d"))
                {
                    newColName = "_" + newColName;
                }

                Dictionary<string, object> getConfigCol = null;
                if (config != null)
                {
                    if (config.ContainsKey(newColName))
                    {
                        getConfigCol = config[newColName];
                    }
                }

                string field = newColName, format = string.Empty;
                string title = colName.TranslateString();
                bool locked = false, hidden = false;
                int width = 150;

                if (!string.IsNullOrWhiteSpace(colCaption))
                {
                    title = colCaption.TranslateString();
                }

                if (getConfigCol != null)
                {
                    foreach (var item in getConfigCol)
                    {
                        switch (item.Key)
                        {
                            case "field":
                                field = item.Value.ToString();
                                break;
                            case "title":
                                title = item.Value.GetString().TranslateString();
                                break;
                            case "width":
                                width = (int)item.Value;
                                break;
                            case "locked":
                                locked = (bool)item.Value;
                                break;
                            case "hidden":
                                hidden = (bool)item.Value;
                                break;
                            case "format":
                                format = item.Value.ToString();
                                break;
                        }
                    }
                }
                column.Add("field", field);
                column.Add("title", title);
                column.Add("width", width);
                if (locked)
                {
                    column.Add("locked", locked);
                }
                if (hidden)
                {
                    column.Add("hidden", hidden);
                }
                if (!string.IsNullOrEmpty(format))
                {
                    column.Add("format", format);
                }


                if (colType == typeof(DateTime))
                {
                    dicType.Add("type", "date");
                    if (string.IsNullOrEmpty(format))
                    {
                        column.Add("format", "{0:dd/MM/yyyy}");
                    }
                }
                else if (colType == typeof(Double) ||
                    colType == typeof(float) ||
                    colType == typeof(Decimal) ||
                    colType == typeof(int))
                {
                    dicType.Add("type", "number");
                    if (IsFormatNumber)
                    {
                        column.Add("format", "{0:n}");
                    }
                }
                else
                {
                    dicType.Add("type", "string");
                }

                columns.Add(column);
                dtSource.Columns[i].ColumnName = newColName;
                colTypes.Add(newColName, dicType);
            }

            var strColumnsJson = columns.ConvertListDictionaryToJson();
            var strJsonFields = "{" + colTypes.ConvertDictionaryToJson() + "}";
            int coCol = columns.Count;

            if (dtSource.Rows.Count == 0)
            {
                DataRow dr = dtSource.NewRow();
                dtSource.Rows.Add(dr);
            }
            DataColumn dc = new DataColumn("TempColumn", typeof(string));
            dtSource.Columns.Add(dc);
            dtSource.Rows[0][coCol] = strColumnsJson + "|" + strJsonFields;
            return dtSource;
        }

        private static string ConvertDictionaryToJson(this Dictionary<string, Dictionary<string, object>> dics)
        {
            var strResult = string.Empty;
            foreach (var item in dics)
            {
                var str = string.Empty;
                var dic = new Dictionary<string, object>();
                foreach (var i in item.Value)
                {
                    dic.Add(i.Key, i.Value);
                }
                str += dic.ConvertDictionaryToJson() + ",";
                str = str.Substring(0, str.Length - 1);
                strResult += "\"" + item.Key + "\" : {" + str + "},";
            }

            if (!string.IsNullOrWhiteSpace(strResult))
            {
                strResult = strResult.Substring(0, strResult.Length - 1);
            }

            return strResult;
        }
        private static string ConvertListDictionaryToJson(this List<Dictionary<string, object>> listDic)
        {
            var strResult = string.Empty;

            foreach (var item in listDic)
            {
                strResult += "{" + item.ConvertDictionaryToJson() + "},";
            }

            if (!string.IsNullOrWhiteSpace(strResult))
            {
                strResult = strResult.Substring(0, strResult.Length - 1);
            }

            return strResult;
        }

        private static string ConvertDictionaryToJson(this Dictionary<string, object> dic)
        {
            var strResult = string.Empty;
            foreach (var item in dic)
            {
                var value = item.Value;
                if (value.GetType() == typeof(string))
                {
                    value = "\"" + value + "\"";
                }
                strResult += "\"" + item.Key + "\" : " + value + ",";
            }

            if (!string.IsNullOrWhiteSpace(strResult))
            {
                strResult = strResult.Substring(0, strResult.Length - 1);
            }

            return strResult;
        }

        public static DataTable ConfigDatatable(this DataTable dt)
        {
            DataTable table = null;
            table = dt.Clone();
            var col = table.Columns.Count;
            for (int i = 0; i < col; i++)
            {
                if (table.Columns[i].DataType != typeof(string))
                {
                    //if (table.Columns[i].DataType == typeof(DateTime) || table.Columns[i].DataType == typeof(DateTime?))
                    //{
                    //    table.Columns[i].Format("dd/MMM/yyyy");
                    //}
                    table.Columns[i].DataType = typeof(string);
                }
            }
            DataRow row = table.NewRow();
            for (int i = 0; i < col; i++)
            {
                var colName = table.Columns[i].ColumnName;
                var config = colName;//.TranslateString();
                if (!string.IsNullOrEmpty(colName))
                {
                    //var startColName = colName.Substring(0, 1);
                    //int startNumber = 0;
                    ////string field = Regex.Replace(colName, @"/^\w\w\w$/", string.Empty);

                    ////table.Columns[i].ColumnName = field;

                    ////if (colName.Contains(" ") || colName.Contains("?") || colName.Contains("%") || colName.Contains("!") || colName.Contains("&") || colName.Contains("*") || colName.Contains("-"))
                    ////{
                    //colName = colName.Replace(" ", "");
                    //colName = colName.Replace("?", "");
                    //colName = colName.Replace("%", "");
                    //colName = colName.Replace("!", "");
                    //colName = colName.Replace("&", "");
                    //colName = colName.Replace("*", "");
                    //colName = colName.Replace("-", "");
                    //colName = colName.Replace(".", "");
                    //table.Columns[i].ColumnName = colName;
                    ////}
                    //if (int.TryParse(startColName, out startNumber))
                    //{
                    //    table.Columns[i].ColumnName = "_" + colName;
                    //}


                    //Xóa các ký tự đặc biệt khi chuyển dữ liệu thành tên column
                    string newColName = Regex.Replace(colName, @"[^0-9a-zA-Z_]+", "");
                    //Kiểm tra nếu tên column bắt đầu bằng số thì xử lý thêm dấu '_' cho hợp lệ
                    if (Regex.IsMatch(newColName, @"^\d"))
                    {
                        newColName = "_" + newColName;
                    }

                    table.Columns[i].ColumnName = newColName;
                }
                row[i] = config;
                //table.Rows.Add(row[i]);
            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow r in dt.Rows)
                {
                    //table.ImportRow(r);
                    table.Rows.Add(r.ItemArray);
                }
            }
            table.Rows.Add(row);
            return table;
        }

        #endregion
        public static string ConvertDataTabletoString(this DataTable datatable)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            Dictionary<string, object> row;
            foreach (DataRow dr in datatable.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in datatable.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }

            return serializer.Serialize(rows);
        }
        public static string DotNetToOracle(string text)
        {
            if (text == null)
                return null;
            if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
                return text;
            string strGuidReturn = string.Empty;
            List<string> lstGuids = new List<string>();
            var strGuids = text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in strGuids)
            {
                Guid guid = Guid.Empty;
                Guid.TryParse(item, out guid);
                lstGuids.Add(BitConverter.ToString(guid.ToByteArray()).Replace("-", ""));
            }
            return string.Join(",", lstGuids);
        }
        public static Guid OracleToDotNet(string text)
        {
            if (text == null)
                return Guid.Empty;
            if (Common.UseDataBaseName == DATABASETYPE.SQLSERVER.ToString())
                return ConvertToGuid(text);
            byte[] bytes = ParseHex(text);
            Guid guid = new Guid(bytes);
            return guid;
        }
        public static byte[] ParseHex(string text)
        {
            // Not the most efficient code in the world, but
            // it works...
            byte[] ret = new byte[text.Length / 2];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = Convert.ToByte(text.Substring(i * 2, 2), 16);
            }
            return ret;
        }

        #region Temp Login
        //private static int _userId = 0;
        //public static int UserId { 
        //    get { 
        //        return _userId; 
        //    }
        //    set { 
        //        _userId = value; 
        //    } 
        //}
        //public static string UserName { get; set; }
        //public static string Password { get; set; }
        #endregion
        public static object Get(this IEnumerable items, int index)
        {
            int result = 0;
            foreach (object item in items)
            {
                if (result == index)
                    return item;
                result++;
            }
            //throw new VNRException(ExceptionType.FRAMEWORK, "Index out of range");
            return false;
        }

        public static Guid ConvertToGuid(string strKey)
        {
            try
            {
                return new Guid(strKey);
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public static object GetEnumValue(Type enumType, string value)
        {
            if (value == null || value == string.Empty)
                return null;
            int index = Enum.GetNames(enumType).ToList().IndexOf(value);
            //if (index == -1)
            //    throw new VNRException(ExceptionType.FRAMEWORK, "Wrong type format");
            return Enum.GetValues(enumType).Get(index);
        }

        /// <summary>
        /// Hàm lấy ra ngày đầu tuần và ngày kết thúc tuần
        /// </summary>
        /// <param name="dateIn">Ngày check</param>
        /// <param name="BeginWeek">Đầu tuần</param>
        /// <param name="EndWeek">cuối tuần</param>
        public static void GetStartEndWeek(DateTime dateIn, out DateTime BeginWeek, out DateTime EndWeek)
        {
            if ((int)dateIn.DayOfWeek == 0) // Neu la chu nhat
            {
                BeginWeek = dateIn.AddDays(-6);
            }
            else
            {
                BeginWeek = dateIn.AddDays(-((int)dateIn.DayOfWeek - 1));
            }

            EndWeek = BeginWeek.AddDays(6);
            EndWeek = EndWeek.AddDays(1).AddMinutes(-1);
        }

        /// <summary>
        /// Sets a property on an object to a valuae.
        /// </summary>
        /// <param name="instance">The object whose property to set.</param>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <param name="value">The value to set the property to.</param>
        public static void SetProperty(object instance, string propertyName, object value)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            Type instanceType = instance.GetType();
            PropertyInfo pi = instanceType.GetProperty(propertyName);

            pi.SetValue(instance, value, new object[0]);
        }

        /// <summary> Chuyển string sang List Guid (cách bởi dấu ,) </summary>
        /// <param name="listString"></param>
        /// <returns></returns>
        public static List<Guid> StringToList(this string listString)
        {
            List<Guid> lstGuid = new List<Guid>();

            if (!string.IsNullOrWhiteSpace(listString))
            {
                string[] listItem = listString.Split(',');
                for (int i = 0; i < listItem.Length; i++)
                {
                    var item = listItem[i];
                    if (!string.IsNullOrEmpty(item))
                    {
                        var itemValue = ConvertToGuid(item);
                        if (itemValue != Guid.Empty)
                        {
                            lstGuid.Add(itemValue);
                        }
                    }
                }
            }

            return lstGuid;
        }

        public static string ListToString(this List<Guid?> list)
        {
            string result = string.Empty;

            if (list != null)
            {
                list = list.Where(m => m != null).ToList();
                if (list == null) return result;
                int i = 0;

                foreach (Guid s in list)
                {
                    if (i == 0)
                        result += s;
                    else result += ", " + s;
                    i++;
                }
            }

            return result;
        }

        /// <summary>
        /// Lấy đường dẫn file template cấu hình
        /// </summary>
        public static string UploadURL
        {
            get
            {
                var obj = ConfigurationManager.AppSettings["UploadURL"];
                if (string.IsNullOrEmpty(obj))
                    return "Uploads/";
                return obj;
            }
        }

        /// <summary>
        /// Lấy đường dẫn file template cấu hình
        /// </summary>
        public static string TemplateURL
        {
            get
            {
                var obj = ConfigurationManager.AppSettings["TemplateURL"];
                if (string.IsNullOrEmpty(obj))
                    return "Templates/";
                return obj;
            }
        }
        /// <summary>
        /// Lấy đường dẫn file Script
        /// </summary>
        public static string ScriptPath
        {
            get
            {
                var obj = ConfigurationManager.AppSettings["ScriptPath"];
                if (string.IsNullOrEmpty(obj))
                    return string.Empty;
                return obj;
            }
        }
        /// <summary>
        /// Lấy đường dẫn file Store
        /// </summary>
        public static string StorePath
        {
            get
            {
                var obj = ConfigurationManager.AppSettings["StorePath"];
                if (string.IsNullOrEmpty(obj))
                    return string.Empty;
                return obj;
            }
        }
        /// <summary>
        /// Lấy đường dẫn file thư mục lưu hình ảnh
        /// </summary>
        public static string ImagesURL
        {
            get
            {
                var obj = ConfigurationManager.AppSettings["TemplateURL"];
                if (string.IsNullOrEmpty(obj))
                    return "Images/";
                return obj;
            }
        }
        public static void CreateNewFolder(string path, string folderName)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Path.Combine(path, folderName);
            }

        }

        /// <summary>
        /// Lấy đường dẫn file download
        /// </summary>
        public static string DownloadURL
        {
            get
            {
                var obj = ConfigurationManager.AppSettings["DownloadURL"];
                if (string.IsNullOrEmpty(obj))
                    return "Downloads/";
                return obj;
            }
        }

        /// <summary>
        /// Lấy loại database sql server hay oracle
        /// </summary>
        public static string UseDataBaseName
        {
            get
            {
                var obj = ConfigurationManager.AppSettings["UseDataBaseName"];
                if (string.IsNullOrEmpty(obj))
                    return DATABASETYPE.SQLSERVER.ToString();
                return obj;
            }
        }

        public static string CompanyName
        {
            get
            {
                var obj = ConfigurationManager.AppSettings["CompanyName"];
                if (string.IsNullOrEmpty(obj))
                {
                    return string.Empty;
                }
                return obj;
            }
        }

        /// <summary>
        /// Config supper user
        /// </summary>
        public static string UserNameSystem
        {
            get
            {
                var obj = ConfigurationManager.AppSettings["UserName"];
                if (string.IsNullOrEmpty(obj))
                    return string.Empty;
                return obj;
            }
        }

        /// <summary>
        /// Config supper user
        /// </summary>
        public static string ProfileID
        {
            get
            {
                var obj = ConfigurationManager.AppSettings["ProfileID"];
                if (string.IsNullOrEmpty(obj))
                    return string.Empty;
                return obj;
            }
        }

        /// <summary>
        /// Config supper user
        /// </summary>
        public static string PasswordSystem
        {
            get
            {
                var obj = ConfigurationManager.AppSettings["Password"];
                if (string.IsNullOrEmpty(obj))
                    return string.Empty;
                return obj;
            }
        }


        /// <summary>Kiểm tra phải là googleSignIn</summary>
        public static bool IsGoogleSignIn
        {
            get
            {
                var isGoogleSignIn = false;
                var obj = ConfigurationManager.AppSettings["IsGoogleSignIn"];
                if (!string.IsNullOrEmpty(obj))
                {
                    bool.TryParse(obj, out isGoogleSignIn);
                }
                return isGoogleSignIn;
            }
        }

        public static string GoogleClientID
        {
            get
            {
                var googleClientID = ConfigurationManager.AppSettings["GoogleClientID"];
                return googleClientID.GetString();
            }
        }

        public static string GetPath(string path)
        {
            var assembly = "HRM.Presentation.Main";
            return GetPath(path, assembly);
        }
        public static string GetPathPortal(string path)
        {
            var assembly = "HRM.Presentation.EmpPortal";
            return GetPath(path, assembly);
        }
        public static string GetPath(string path, string assembly)
        {
            var serverPath = string.Empty;
            if (HttpContext.Current == null)
            {
                serverPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "");
            }
            else
            {
                if (HttpContext.Current.Server != null)
                {
                    serverPath = HttpContext.Current.Server.MapPath("~/");
                }
            }
            if (!string.IsNullOrEmpty(serverPath))
            {
                string[] listItem = serverPath.Split('\\');
                string pathTemp = listItem[0];
                for (int i = 1; i < listItem.Length - 2; i++)
                {
                    pathTemp = pathTemp + "\\" + listItem[i];
                }
                var fullPath = pathTemp + "\\" + assembly + "\\" + path;
                CreateNewFolder(fullPath, path);
                return fullPath;
            }

            return path;
        }

        public static string GetPathCustom(string path)
        {
            if (HttpContext.Current != null && HttpContext.Current.Server != null)
            {
                var serverPath = HttpContext.Current.Server.MapPath("~/");
                string[] listItem = serverPath.Split('\\');
                string pathTemp = listItem[0];
                for (int i = 1; i < listItem.Length - 3; i++)
                {
                    pathTemp = pathTemp + "\\" + listItem[i];
                }
                var fullPath = pathTemp + path;
                CreateNewFolder(fullPath, path);
                return fullPath;
            }
            return path;
        }
        public static string GetFileName(string fileName)
        {
            string[] splits = fileName.Split('\\');
            return splits[splits.Length - 1];
        }

        /// <summary>
        /// [Chuc.Nguyen] - Xuât nhiều dữ liệu
        /// </summary>
        /// <param name="hostPath"></param>
        /// <param name="isZipFile"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public static string MultiExport(string hostPath, bool isZipFile, string outputPath)
        {
            string downloadPath = string.Empty;
            try
            {
                string folderStore = outputPath;
                string dirpath = Common.GetPath(Common.DownloadURL + folderStore);

                if (isZipFile)
                {
                    string strPathStoreFileZip = Common.GetPath(Common.DownloadURL);
                    folderStore = folderStore + ".zip";
                    CompressFile.CreateZipFile(strPathStoreFileZip, folderStore, new DirectoryInfo(dirpath));
                    string fileName = folderStore;
                    downloadPath = hostPath + "/" + Common.DownloadURL + fileName;
                    string filePath = strPathStoreFileZip + "\\" + folderStore;
                }
                else
                {
                    string fileName = Common.GetFileName(outputPath);
                    downloadPath = hostPath + fileName;
                    if (!File.Exists(outputPath))
                    {
                        return NotificationType.Error.ToString() + "," + ConstantMessages.PathNotExists.TranslateString();
                    }
                }
                if (Directory.Exists(dirpath))
                {
                    System.IO.DirectoryInfo folder = new DirectoryInfo(dirpath);

                    foreach (FileInfo file in folder.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in folder.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                    Directory.Delete(dirpath);
                }
                return NotificationType.Success.ToString() + "," + downloadPath;
            }
            catch (Exception ex)
            {
                return NotificationType.Error.ToString() + "," + ex.Message;
            }
        }

        public static double RoundMinuteDown(double Minute, double HourCheck)
        {
            if (HourCheck == 0)
                return Minute;
            double MinuteCheck = HourCheck * 60;
            int TimeAllow = (int)(Minute / MinuteCheck);
            return TimeAllow * MinuteCheck;

        }

        /// <summary>
        /// Khi thêm module phải cập nhật vào hàm này.
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static string GetModuleName(string keyName, bool useOtherGroup)
        {
            string moduleName = keyName;

            if (keyName.StartsWith(ModuleKey.Sys.ToString()))
            {
                moduleName = ModuleName.System.ToString();
            }
            else if (keyName.StartsWith(ModuleKey.Cat.ToString()))
            {
                moduleName = ModuleName.Category.ToString();
            }
            else if (keyName.StartsWith(ModuleKey.Att.ToString()))
            {
                moduleName = ModuleName.Attendance.ToString();
            }
            else if (keyName.StartsWith(ModuleKey.Can.ToString()))
            {
                moduleName = ModuleName.Canteen.ToString();
            }
            else if (keyName.StartsWith(ModuleKey.Hre.ToString()))
            {
                moduleName = ModuleName.HR.ToString();
            }
            else if (keyName.StartsWith(ModuleKey.Lau.ToString()) || keyName.StartsWith("LMS"))
            {
                moduleName = ModuleName.Laundry.ToString();
            }
            else if (keyName.StartsWith(ModuleKey.Med.ToString()))
            {
                moduleName = ModuleName.Medical.ToString();
            }
            else if (keyName.StartsWith(ModuleKey.Rec.ToString()))
            {
                moduleName = ModuleName.Recruitment.ToString();
            }
            else if (keyName.StartsWith(ModuleKey.Tra.ToString()))
            {
                moduleName = ModuleName.Training.ToString();
            }
            else if (keyName.StartsWith(ModuleKey.Sal.ToString()))
            {
                moduleName = ModuleName.Salary.ToString();
            }
            else if (keyName.StartsWith(ModuleKey.Ins.ToString()))
            {
                moduleName = ModuleName.Insurance.ToString();
            }
            else if (keyName.StartsWith(ModuleKey.Eva.ToString()))
            {
                moduleName = ModuleName.Evaluation.ToString();
            }
            else if (keyName.StartsWith(ModuleKey.Fin.ToString()) || keyName.StartsWith("FIN"))
            {
                moduleName = ModuleName.Finance.ToString();
            }
            else if (keyName.StartsWith(ModuleKey.Pur.ToString()) || keyName.StartsWith("PUR"))
            {
                moduleName = ModuleName.Purchase.ToString();
            }
            else if (useOtherGroup)
            {
                moduleName = ResourceType.Other.ToString();
            }
            else if (keyName.StartsWith(ModuleKey.Kai.ToString()))
            {
                moduleName = ModuleName.Kaizen.ToString();
            }
            else if (keyName.StartsWith(ModuleKey.Hrd.ToString()))
            {
                moduleName = ModuleName.HRDetail.ToString();
            }
            return moduleName;
        }

        public static bool Get_Boolean(this bool? value)
        {
            return value == null ? false : value.Value;
        }
        public static double Get_Double(this double? number)
        {
            return number == null ? 0 : number.Value;
        }
        public static Guid Get_Guid(this Guid? guid)
        {
            return guid == null ? Guid.Empty : guid.Value;
        }
        public static int Get_Integer(this int? number)
        {
            return number == null ? 0 : number.Value;
        }

        public class DefaultFieldNames
        {
            #region DefaultFieldNames
            public const string UserLockID = "UserLockID";
            public const string DateLock = "DateLock";
            public const string PToString = "PToString";
            public const string IsSystem = "IsSystem";
            public const string IsVisible = "IsVisible";

            //public const string CreateInfo = "CreateInfo";
            //public const string UpdateInfo = "UpdateInfo";
            //public const string AddressType = "AddressType";
            //public const string Level = "Level";
            //public const string Skill = "Skill";
            //public const string Type = "Type";
            public const string ID = "ID";
            public const string Code = "Code";
            public const string ServerUpdate = "ServerUpdate";
            public const string ServerCreate = "ServerCreate";
            public const string UserUpdate = "UserUpdate";
            public const string UserCreate = "UserCreate";
            public const string DateCreate = "DateCreate";
            public const string DateUpdate = "DateUpdate";
            public const string IPCreate = "IPCreate";
            public const string IPUpdate = "IPUpdate";
            //public const string UserApprSys_UserInfo = "UserApprSys_UserInfo";
            //public const string DateAppr = "DateAppr";
            public const string IsDefault = "IsDefault";
            public const string IsDelete = "IsDelete";
            public const string Checked = "Checked";
            public const string Cat_OrgStructure = "Cat_OrgStructure";
            public const string Cat_DataGroup = "Cat_DataGroup";
            public const string Hre_Profile = "Hre_Profile";
            public const string ProfileID = "ProfileID";
            public const string Status = "Status";
            public const string AttachFileName = "AttachFileName";
            public const string AttachFileData = "AttachFileData";
            public const string HasAttachment = "HasAttachment";
            public const string OrderNumber = "OrderNumber";
            //public const string QualificationName = "QualificationName";
            //public const string QualificationType = "QualificationType";
            //public const string EntityType = "EntityType";
            //public const string Marriage_status = "Marriage_status";
            //public const string ContractType = "ContractType";
            //public const string DependantType = "DependantType";
            //public const string MonShift = "MonShift";
            //public const string TueShift = "TueShift";
            //public const string WedShift = "WedShift";
            //public const string ThuShift = "ThuShift";
            //public const string FriShift = "FriShift";
            //public const string SatShift = "SatShift";
            //public const string SunShift = "SunShift";
            public const string OrgStructures = "OrgStructures";
            public const string OrgStructure = "OrgStructure";
            public const string ProfileStatus = "ProfileStatus";
            public const string Month = "Month";

            #endregion
        }
        public static object GetProperty_Value(this object entity, string propertyName)
        {
            if (entity == null || string.IsNullOrWhiteSpace(propertyName))
            {
                return null;
            }

            PropertyInfo proInfo = entity.GetType().GetProperty(propertyName);
            if (proInfo == null || !proInfo.CanRead)
                return null;

            return proInfo.GetValue(entity, null);
        }

        /// <summary>
        /// Lay danh sach datasource của dropdownlist theo Enum
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <returns></returns>
        public static IList<SelectListItem> GetEnumValues<T>()
        {
            IList<SelectListItem> listEnumValue = Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(x => new SelectListItem { Text = EnumDropDown.GetEnumDescription(x), Value = x.ToString() })
            .ToList();

            return listEnumValue;
        }
        /// <summary>
        /// Lấy giá trị của đối tượng DateTime?
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>Nếu dateTime = null thì trả về min</returns>
        public static DateTime Get_DateTimeOrMin(this DateTime? dateTime)
        {
            return dateTime != null ? dateTime.Value : SqlDateTime.MinValue.Value;
        }

        public static int SessionTimeOut
        {
            get
            {
                int timeout = 0;
                var obj = ConfigurationManager.AppSettings["SessionTimeOut"];
                if (string.IsNullOrEmpty(obj))
                    return 10;

                int.TryParse(obj, out timeout);
                if (timeout < 5)
                {
                    timeout = 10;
                }
                return timeout;
            }
        }

        public static string[] GetFieldNames(this Type entityType)
        {
            return entityType.GetNestedType("FieldNames").GetFields().Select(fi => fi.Name).ToArray();
        }

        public static bool IsEntityCollection(this Type type)
        {
            if (type == null)
                return false;
            return type.Name.Contains("EntityCollection`1");
        }

        public static bool IsEntityReference(this Type type)
        {
            if (type == null)
                return false;
            return type.Name.Contains("EntityReference`1");
        }

        public static bool IsEntityType(this Type type)
        {
            if (type == null)
                return false;
            if (type.IsEntityCollection())
                return false;
            //return type.FullName.StartsWith("VnResourceWeb.Entities.");
            return type.GetInterface("IEntity") != null;
        }

        const string FindText = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
        const string ReplText = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";

        public static string ChuyenTVKhongDau(string strVietNamese)
        {
            int index = -1;
            while ((index = strVietNamese.IndexOfAny(FindText.ToCharArray())) != -1)
            {
                int index2 = FindText.IndexOf(strVietNamese[index]);
                strVietNamese = strVietNamese.Replace(strVietNamese[index], ReplText[index2]);
            }
            return strVietNamese.Trim().Replace(" ", "_");
        }

        //public static object GetPropertyValue(this object entity, string propertyName)
        //{
        //    if (entity == null || string.IsNullOrWhiteSpace(propertyName))
        //    {
        //        return null;
        //    }

        //    PropertyInfo proInfo = entity.GetType().GetProperty(propertyName);
        //    if (proInfo == null || !proInfo.CanRead)
        //        return null;

        //    return proInfo.GetValue(entity, null);
        //}

        // Son.Vo - 20141007 - chuyển từ list Entity Sang datatable để xuất template theo file word
        public static System.Data.DataTable ConvertIListToDataTable(IList listObject, string[] fieldsConvert)
        {
            System.Data.DataTable tb = new System.Data.DataTable();
            if (listObject == null)
                return tb;

            Type entityType = listObject.AsQueryable().ElementType;
           // string[] fieldNames = entityType.GetFieldNames();
            DataColumn column = null;
            Type properType = null;
            foreach (PropertyInfo properInfo in entityType.GetProperties())
            {
                if (properInfo.PropertyType.IsEntityCollection() || properInfo.PropertyType.IsEntityReference())
                    continue;
                if (fieldsConvert != null && !fieldsConvert.Contains(properInfo.Name) && fieldsConvert.Length > 0)
                    continue;
                column = new DataColumn();
                column.ColumnName = properInfo.Name;

                properType = properInfo.PropertyType;
                if (properType.IsEntityType())
                {
                    column.DataType = typeof(Guid);
                }
                else if (properType == typeof(DateTime) || properType == typeof(DateTime?))
                    column.DataType = typeof(DateTime);
                else if (properType == typeof(Int16) || properType == typeof(Int16?))
                    column.DataType = typeof(Int16);
                else if (properType == typeof(Int32) || properType == typeof(Int32?))
                    column.DataType = typeof(Int32);
                else if (properType == typeof(Double) || properType == typeof(Double?))
                    column.DataType = typeof(Double);
                else if (properType == typeof(Guid) || properType == typeof(Guid?))
                    column.DataType = typeof(Guid);
                else if (properType == typeof(Boolean) || properType == typeof(Boolean?))
                    column.DataType = typeof(Boolean);
                else if (properType == typeof(String))
                    column.DataType = typeof(String);
                //column.DataType = properType;
                tb.Columns.Add(column);
            }
            DataRow row = null;
            foreach (object obj in listObject)
            {
                row = tb.NewRow();
                for (int i = 0; i < tb.Columns.Count; i++)
                {
                    object currentValue = obj.GetPropertyValue(tb.Columns[i].ColumnName);
                    if (currentValue != null)
                    {
                        if (currentValue.GetType().IsEntityType())
                        {
                            //string[] fieldNameEntity = new string[]{fieldNames[i], DefaultFieldNames.ID};
                            object currentEntity = currentValue.GetPropertyValue(DefaultFieldNames.ID);
                            row[tb.Columns[i].ColumnName] = currentEntity;
                        }
                        else
                            row[tb.Columns[i].ColumnName] = currentValue;
                    }
                }
                tb.Rows.Add(row);
            }
            return tb;
        }

        /// <summary>
        /// Lam tron 1 so 
        /// theo buoc nhay. (truong hop lam tron OT)
        /// </summary>
        /// <param name="_hour"></param>
        /// <param name="upToHours"></param>
        /// <param name="minuteRound"></param>
        /// <param name="IsUpTo"></param>
        /// <returns></returns>
        public static Double Round(Double _hour, double upToHours, double minuteRound, Boolean IsUpTo)
        {
            //double minuteRound = 30;
            Double minute = _hour * 60.0;
            Double _uptoMinute = upToHours * 60;
            Double min = 0;

            if (_uptoMinute != 0)
            {
                int intRange = (int)(minute / (_uptoMinute));
                int a = (intRange % 2);

                Double dtDiv = minute % _uptoMinute;
                dtDiv = dtDiv + (a * _uptoMinute);

                if (IsUpTo)
                {
                    if ((dtDiv) >= _uptoMinute)
                    {
                        intRange++;
                    }
                }

                min = (intRange * _uptoMinute);
            }
            else
            {
                min = minute;
            }

            return min / 60;
        }

        #region ScreenManager  (Tung.Ly 20140715)

        #region Lấy các thông tin của file Screen_Info.xml

        #region public methods
        /// <summary> Lấy danh sách điều kiện search  </summary>
        /// <typeparam name="T">Kiểu đối tượng tìm kiếm</typeparam>
        /// <param name="modelSearch">Giá trị đối tượng tìm kiếm </param>
        /// <param name="screenName">Name của tag Screen trong SCREEN_INFO.xml</param>
        /// <returns></returns>
        public static List<FilterFieldInfo> BuildFilterInfo<T>(T modelSearch, string screenName)
        {
            FilterFieldInfo[] listSettings = GetFilterFields(screenName, typeof(T));
            List<FilterFieldInfo> listFilterInfo = new List<FilterFieldInfo>();

            if (listSettings != null && listSettings.Count() > 0)
            {
                List<IGrouping<VnResource.Helper.Setting.FilterType, FilterFieldInfo>> listGroupFilterField = null;
                listGroupFilterField = listSettings.GroupBy(d => d.Type).ToList();
                //listGroupFilterField : ds filterfield chua co gia tri
                foreach (IGrouping<VnResource.Helper.Setting.FilterType, FilterFieldInfo> groupFilterField in listGroupFilterField)
                {
                    #region FilterType là Text

                    if (groupFilterField.Key == VnResource.Helper.Setting.FilterType.Text)
                    {
                        var listModelFields = GetFilterFields<T>(modelSearch);//ds filterfield co gia tri
                        foreach (var filterFieldInfo in listModelFields)
                        {
                            if (filterFieldInfo.FilterValue1 != null && !string.IsNullOrWhiteSpace(filterFieldInfo.FilterValue1.ToString()))
                            {
                                FilterFieldInfo filterField = groupFilterField.Where(d => GetControlID(d.Name) == filterFieldInfo.Name).FirstOrDefault();

                                if (filterField != null && !listFilterInfo.Contains(filterField))
                                {
                                    listFilterInfo.Add(filterFieldInfo);
                                }
                            }
                        }
                    }

                    #endregion

                    #region number

                    else if (groupFilterField.Key == VnResource.Helper.Setting.FilterType.Number)
                    {
                        var listModelFields = GetFilterFields<T>(modelSearch).Where(p => p.Type == VnResource.Helper.Setting.FilterType.Number).ToArray();//ds filterfield co gia tri

                        foreach (var modelField in listModelFields)
                        {
                            FilterFieldInfo filterField = groupFilterField.Where(d =>
                                GetControlID(d.Name) == modelField.Name).FirstOrDefault();

                            if (filterField != null && !listFilterInfo.Contains(filterField))
                            {
                                filterField.FilterValue1 = DataHelper.TryGetValue(modelField.FilterValue1, typeof(int));
                                listFilterInfo.Add(modelField);//Chỉ hỗ trợ filter theo số nguyên.
                            }
                        }
                    }

                    #endregion

                    #region DateTime

                    else if (groupFilterField.Key == FilterType.DateTime)
                    {
                        var listModelFields = GetFilterFields<T>(modelSearch).Where(p => p.Type == FilterType.DateTime).ToArray();//ds filterfield co gia tri

                        foreach (var modelField in listModelFields)
                        {
                            if (modelField != null)
                            {
                                FilterFieldInfo filterField = groupFilterField.Where(d => GetControlID(d.Name) == modelField.Name).FirstOrDefault();

                                if (filterField != null && !listFilterInfo.Contains(filterField))
                                {
                                    filterField.FilterValue1 = DataHelper.TryGetValue(modelField.FilterValue1, typeof(DateTime));
                                    listFilterInfo.Add(modelField);
                                }
                            }

                        }
                    }

                    #endregion
                }
            }
            return listFilterInfo;
        }

        /// <summary> Lấy danh sách điều kiện search  </summary>
        /// <typeparam name="T">Kiểu đối tượng tìm kiếm</typeparam>
        /// <param name="modelSearch">Giá trị đối tượng tìm kiếm </param>
        /// <returns></returns>
        public static List<FilterFieldInfo> BuildFilterInfo<T>(T modelSearch)
        {
            return BuildFilterInfo<T>(modelSearch, string.Empty);
        }
        #endregion

        #region Private methods

        private static string GetControlID(string filterName)
        {
            var aaa = filterName.Split('.');
            if (aaa.Any())
            {
                return aaa.LastOrDefault();
            }
            return filterName.GetString().Replace(".", "");
        }

        private static FilterFieldInfo[] GetFilterFields(string screenName, Type objectType)
        {
            FilterFieldInfo[] listSettings = null;
            ScreenManager screenManager = new ScreenManager();
            screenManager.SettingPath = GetPath("Settings");

            if (!string.IsNullOrWhiteSpace(screenName))
            {
                listSettings = screenManager.GetFilterSettings(string.Empty, screenName);
            }

            if ((listSettings == null || listSettings.Count() <= 0) && objectType != null)
            {
                listSettings = screenManager.GetFilterSettings(string.Empty, objectType.Name);
            }

            return listSettings;
        }

        private static VnResource.Helper.Setting.FilterType GetFilterType(Type filterType)
        {
            var filterTypeResult = new VnResource.Helper.Setting.FilterType();
            if (filterType == typeof(string))
            {
                filterTypeResult = VnResource.Helper.Setting.FilterType.Text;
            }
            else if (filterType == typeof(Boolean))
            {
                filterTypeResult = VnResource.Helper.Setting.FilterType.Boolean;
            }
            else if (IsNumericType(filterType))
            {
                filterTypeResult = VnResource.Helper.Setting.FilterType.Number;
            }
            else if (filterType == typeof(DateTime))
            {
                filterTypeResult = VnResource.Helper.Setting.FilterType.DateTime;
            }


            return filterTypeResult;
        }

        private static bool IsNumericType(Type type)
        {
            TypeCode typeCode = Type.GetTypeCode(type);
            //The TypeCode of numerical types are between SByte (5) and Decimal (15).
            return (int)typeCode >= 5 && (int)typeCode <= 15;
        }

        /// <summary> Lấy danh sách property của một class </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelName"></param>
        /// <returns></returns>
        private static FilterFieldInfo[] GetFilterFields<T>(T modelName)
        {
            List<FilterFieldInfo> listSetting = new List<FilterFieldInfo>();
            var fields = new List<FilterFieldInfo>();
            foreach (var prop in modelName.GetType().GetProperties())
            {
                var value = prop.GetValue(modelName, null);
                if (value != null)
                {
                    FilterFieldInfo field = new FilterFieldInfo
                    {
                        Name = prop.Name,
                        FilterValue1 = value,
                        Type = GetFilterType(Type.GetType(prop.PropertyType.FullName))
                    };
                    fields.Add(field);
                }
            }
            return fields.ToArray();
        }

        #endregion

        #endregion

        #endregion

        /// <summary>
        /// Convert chuoi thanh kieu date
        /// _str: dd/MM/yyyy
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static DateTime ConvertStringToDateTime(string _str, string format)
        {
            DateTime dtReturn = SqlDateTime.MinValue.Value;
            try
            {
                if (format == string.Empty || format == "None")
                    dtReturn = DateTime.Parse(_str);
                else
                {
                    if (!_str.IsNullOrEmpty())
                        dtReturn = DateTime.ParseExact(_str.Trim(), format, CultureInfo.InvariantCulture);
                }
            }
            catch (Exception)
            {

            }
            return dtReturn;
        }

        public static string FormulaToString(this Formula.FormulaConstant o)
        {
            if (o.IsNullOrEmpty()) return string.Empty;
            return "[" + o.ToString() + "]";
        }

        public static string ReplaceSpace(this string value)
        {
            if (value != null)
            {
                return value.Replace(" ", "");
            }
            else
            {
                return "null";
            }
        }

        /// <summary>
        /// Hàm tính thời gian chạy từ DateTimeStart => DateTimeEnd
        /// </summary>
        /// <param name="DateTimeStart"></param>
        /// <param name="DateTimeEnd"></param>
        /// <returns></returns>
        public static string ComputeTime(DateTime DateTimeStart, DateTime DateTimeEnd)
        {
            return DateTimeEnd.Subtract(DateTimeStart).TotalMinutes.ToString("N2");
        }

        /// <summary>
        /// Convert chuỗi datetime thành list Ngày - Tháng - Năm
        /// </summary>
        /// <param name="DateTime"></param>
        /// <param name="Char"></param>
        /// <returns></returns>
        public static List<string> SplitStringDatetime(string DateTime, char Char)
        {
            List<string> result = DateTime.Split(Char).ToList();

            if (result.Count() == 3)
            {
                return result;
            }
            else if (result.Count() == 2)
            {
                //nếu là tháng năm
                if (result.FirstOrDefault().Length == 4 || result.LastOrDefault().Length == 4)
                {
                    result.Insert(0, "01");
                    return result;
                }
                else//nếu là ngày tháng
                {
                    result.Add("1001");
                    return result;
                }
            }
            else
            {
                if (result.FirstOrDefault().Length == 4)
                {
                    result.Insert(0, "01");
                    result.Insert(0, "01");
                    return result;
                }
                else
                {
                    result.Add("01");
                    result.Add("1001");
                    return result;
                }
            }
        }
    }

    #region Formular

    //[Tung.Ly 20141004] : copy từ bản 7 oracle
    public class Formula
    {
        #region FormulaConstant
        public enum FormulaConstant
        {
            #region Function
            //Function:ham Or nhu Excel
            //Created By: Tran.Pham
            //Created Date: 07/04/2011
            OR,
            //Function:ham Round (Value,number of round)
            //Created By: Tran.Pham
            //Created Date: 07/04/2011
            ROUND,
            //Function:ham ROUNDCUSTOMIZE(Double _hour, double upToHours, double minuteRound, Boolean IsUpTo)
            //Created By: LamLe ()
            //Created Date: 26/11/2012
            ROUNDCUSTOMIZE,
            MFLOOR,
            //Function:ham Roundup nhu Excel
            //Created By: Vi.nguyen
            //Created Date: 08/04/2012
            ROUNDUP,
            //Function:ham isnull nhu sql
            //Created By: Vi.nguyen
            //Created Date: 08/04/2012
            ISNULL,
            //Function:ham int nhu Excel
            //Created By: Hien.Pham
            //Created Date: 20121026
            INT,

            #endregion

            #region Công Thức Phụ Cấp
            //Function:tiền theo level
            //Created By: Tran.Pham
            //Created Date: 08/03/2011
            LEVEL,
            //Function:Ngày công chuẩn
            //Created By: Tran.Pham
            //Created Date: 08/03/2011
            STDDAY,
            //Function:Giờ công chuẩn
            //Created By: Hien.Pham
            //Created Date: 08/03/2011
            STD_HOUR_PER_DAY,
            //Function:Ngày công thực tế 
            //Created By: LamLe
            //Created Date: 08/03/2011
            REALDAY,
            //Function:Dem so Ngày công thực tế (co quet the thi tinh 1 ngay)
            //Created By: Tran.Pham
            //Created Date: 08/03/2011
            REALDAY_COUNT,
            //Function: Dem so Ngày công thực tế di lam duoc huong thai san 1h(co quet the thi tinh 1 ngay)
            //Created By: Tran.Pham
            //Created Date: 08/03/2011
            REALDAY_PREGNANCY_COUNT,
            //Function:Ngày công thực tế và ngày nghỉ có lương
            //Created By: Tran.Pham
            //Created Date: 08/03/2011
            REALDAY_AND_PAIDLEAVE_COUNT,
            //Function:Ngày công tính lương đã trừ trễ sớm
            //Created By: Tran.Pham
            //Created Date: 08/03/2011
            WORKING_PAIDLEAVE_DEDUCTION_LATE_EARLY_DAY,

            //Function: Ngày công tính lương đã trừ trễ sớm sau khi kết thúc thử việc trong tháng
            //Created By: SonNgo
            //Created Date: 20120627
            PAID_DAYS_DEDUCTION_LATE_EARLY_DAY_END_PROBATION,

            //Function: số ngày công tính lương sau khi kết thúc thử việc trong tháng
            //Created By: SonNgo
            //Created Date: 20120713
            PAID_DAYS_END_PROBATION,

            //Function: số ngày nghỉ không tính lương sau khi kết thúc thử việc trong tháng
            //Created By: SonNgo
            //Created Date: 20120731
            UNPAID_DAYS_END_PROBATION,

            //Function: số ngày hieu luc tinh phu cap phat sinh
            //Created By: hien.pham
            //Created Date: 20130207
            DAYS_OFFEFFECT_COUNT,

            //Function:So ngay nghi phep nam
            //Created By: Lam.Le
            //Created Date: 23/05/2011
            ANNUALLEAVE_COUNT,
            //Function:Phep nam con lai cho den thang
            //Created By: Lam.Le
            //Created Date: 17/09/2012
            ANNUAL_AVAILABLE_COUNT,
            //Function:So ngay nghi khong luong 
            //Created By: Lam.Le
            //Created Date: 23/05/2011
            UNPAIDLEAVE_COUNT,
            //Function: Ngay di lam duoc tra luong
            //Created By: LamLe 
            //Created Date: 28/11/2011 
            WORKINGPAIDDAY,
            //Function:So ngay nghi có luong
            //Created By: Tran.Pham
            //Created Date: 3/06/2011
            PAIDLEAVE_COUNT,
            //Function:Ngày làm ca đêm
            //Created By: Tran.Pham
            //Created Date: 08/03/2011
            NIGHTDAY_COUNT,
            //Function: Số ngày đi trễ
            //Created By: Tran.Pham
            //Created Date: 08/03/2011
            //LATE_COUNT, // SonNgo unuse

            //Function: So ngay nghi khong luong va tre som
            //Created By: Lam.Le
            //Created Date: 23/05/2011
            UNPAIDLEAVE_LATEEARLY_DAY,

            //Function: Số ngày đi trễ || về sớm
            //Created By: Tran.Pham
            //Created Date: 08/03/2011
            LATE_EARLY_COUNT,
            //Function: Số ngày đi trễ || về sớm
            //Created By: Tran.Pham
            //Created Date: 08/03/2011
            LATE_EARLY_HOURS,
            //Function: Lay tat ca nhung ngay khong quet the.
            //Created By: Tran.Pham
            //Created Date: 08/03/2011
            LEAVEDAY_COUNT,
            //Function: Vao trong thang
            //Created By: Tran.Pham
            //Created Date: 08/03/2011
            NEWCOME,
            //Function: Nghi trong thang
            //Created By: Tran.Pham
            //Created Date: 08/03/2011            
            TERMINATION,

            //Function: Ma phong ban
            //Created By: LamLe 
            //Created Date: 28/11/2011 
            CODE_BRANCH,
            //Function: Dang thu viec
            //Created By: LamLe 
            //Created Date: 28/11/2011 
            PROBATION,
            //Function: Dang thu viec
            //Created By: LamLe 
            //Created Date: 28/11/2011 
            IS_DATEHIRE_BF_15,
            //Function: Kiểm tra tháng tính lương có hết hạn thử việc hay không
            //Created By: SonNgo 
            //Created Date: 28/08/2012 
            E_END_PROBATION_SALMONTH,

            //Function: Thông tin Mã chi phí
            //Created By: SơnNgo 
            //Created Date: 23/08/2012 
            COSTCENTER_CODE,

            //Function: Kiểm tra thời gian kết thúc thử việc của NV vào ngày mấy của tháng tính lương, vd: END_PROBATION_ON_20
            //Created By: SonNgo 
            //Created Date: 28/11/2011 
            END_PROBATION_ON_,

            //Function: Nghi viec tinh giong BHXH. Dua vao ngay 15
            //Created By: LamLe 
            //Created Date: 28/11/2011             
            INS_TERMINATION,
            //Function: Thu viec tinh giong BHXH. Dua vao ngay 15
            //Created By: LamLe 
            //Created Date: 28/11/2011 
            INS_PROBATION,
            //Function: Dung cho XP, ngay ket thuc thu viec + them 1 roi so sanh lon hon or nho ngay 15 cua thang = > tra ve true or false
            //Created By: LamLe 
            //Created Date: 03/07/2011 
            INS_PROBATION_ADD_1,
            //Function: số lần tăng ca đầu giờ (được approve) và >=1.5 giờ
            //Created By: Tran.Pham
            //Created Date: 08/03/2011
            FGL_EARLY_OVERTIME_COUNT,
            //Function: số ngày thường làm việc sau 19h30
            //Created By: Tran.Pham
            //Created Date: 08/03/2011
            WEEKDAY_WORK_LATE_AFTER_19h30_COUNT,

            //Function: số ngày nghỉ full-shift bị tính trong trường hợp tính pc chuyên cần của FGL
            //- khám thai: CHK
            //- phép năm: ANL
            //- duong suc: HEA
            //- nghi con nho:NBC
            //- cong ty cho nghi: 
            //Created By: Tran.Pham
            //Created Date: 08/03/2011
            FGL_DILIGENT_LEAVEDAY_COUNT,

            //Function: số ngày đi làm > 4hrs trong ngày chủ nhật và ngày lễ
            //- Chủ nhật: 
            //- Ngày lễ: 
            //Created By: Lam.LE
            //Created Date: 08/03/2011
            FGL_SUNDAY_HOLIDAY_WORK_GREATER_4HOURS_COUNT,

            //Function: Lay gio tang ca ngay cuoi tuan dayoff va Holiday
            //- Chủ nhật: 
            //- Ngày lễ: 
            //Created By: Lam.LE
            //Created Date: 08/03/2011
            OT_DAYOFF_HOLIDAY_WORK_GREATER_6HOURS_COUNT,

            //Function: số giờ đi làm trong ngày chủ nhật và ngày lễ
            //- Chủ nhật: 
            //- Ngày lễ: 
            //Created By: Tran.Pham
            //Created Date: 23/06/2011
            FGL_SUNDAY_HOLIDAY_WORK_HOURS,

            //Function:Ngày công tinh luong cua TAISUN = PaidWorkDayCount(ngay cua ctrinh) + ngay nghi phep tinh luong
            //Created By: Lam.Le
            //Created Date: 23/05/2011
            TAISUN_PAIDWORKDAY_COUNT,

            //Function: Tính số giờ OT quá 8h ngày chủ nhật
            //Create by: Khang. Huynh
            OT_SUNDAY_HOLIDAY_GREATER_8H,
            //Số giờ làm thêm ca đem
            //Create by: Hien.Pham
            OT_NIGHTSHIFT_HOURS,
            //Function: Tính số ngày OT > 4h/ ngày
            //Create by: SonNgo
            OT_UPPER_4H,

            //Function: Tính số ngày OT > 4h/ ngày
            //Create by: SonNgo
            OT_UPPER_8H,
            //Function: Tính số giờ OT quá 8h ngày thứ 7 nghỉ bù
            //Create by: Khang. Huynh
            OT_SATURDAY_GREATER_8H,
            //Function: Tính số lượng tuần đc tính phụ cấp chuyên cần
            //Create by: Khang. Huynh
            WEEKLY_ATTENDANCE_ALLOWANCE,
            //Function: Tính tháng thâm niên
            //Created By: Son.Ngo
            //Created Date: 23/05/2011
            SENIORITY_MONTHS,

            //Function: Tính năm thâm niên từ ngày 01/01 hàng năm
            // ex: Vao cong ty thang 10/2011 den 01/01/2012 tham nien = 0 nam
            //                               den 01/01/2013 tham nien = 1 nam
            //Created By: Khang.Huynh
            //Created Date: 
            SENIORITY_YEAR_FUJIKURA,

            //Function: Ngay vao lam
            //Created Date: 
            SENIORITY_YEAR_FUJIKURA_BY_DATEHIRE,
            //Function:So ngay có In/Out
            //Created By: Trân.Pham
            //Created Date: 26/05/2011
            //SENIORITY_YEAR, // SonNgo unuse
            //Function:So ngay có In/Out
            //Created By: Trân.Pham
            //Created Date: 26/05/2011
            INOUTDAY_COUNT,

            //Function:So ngay có In/Out loai tru ngay nghi le va cuoi tuan
            //Created By: Lam.Le
            //Created Date: 02/06/2011
            INOUTDAY_GREATER_4HOURS_EXCLUDE_HOLIDAYANDWEEKEND_COUNT,
            //Function:So ngay có In/Out loai tru ngay nghi le va cuoi tuan, lon hon or bang 4h. Pataya - DucHo
            //Created By: Lam.Le
            //Created Date: 10/06/2013
            INOUTDAY_GREATER_EQUAL_4HOURS_EXCLUDE_HOLIDAYANDWEEKEND_COUNT,
            //Function: So ngay có In/Out >=8h loai tru ngay nghi le va cuoi tuan
            //Created By: SonNgo
            //Created Date: 10/12/2011
            INOUTDAY_UPPER_8HOURS_EXCLUDE_HOLIDAYANDWEEKEND_COUNT,


            //Function: Đếm số ngày quét thẻ >= 8h/ngày
            //Created By: SonNgo
            //Created Date: 15/07/2012
            WORKHOURS_UPPER_8HOURS,

            //Function: So ngay có In/Out >=8h loai tru ngay nghi le va cuoi tuan (D/v Cong nhan co tham nien >= 2 nam)
            //Created By: SonNgo
            //Created Date: 10/12/2011
            INOUTDAY_UPPER_8HOURS_AND_SENIORITY_UPPER_2YEARS_EXCLUDE_HOLIDAYANDWEEKEND_COUNT,

            //Function:số lượng con nhỏ valid trong tháng tính lương
            //Created By: Son.Ngo
            //Created Date: 15/06/2011
            NEW_BORN_CHILD_COUNT,

            //Function: Tinh so tien BHXH
            //Created By: Lam.Le
            //Created Date: 12/08/2011
            AMOUNT_INSURANCE,

            //Function: Dem so ngay lam ca có mã số phía sau dấu "_"
            //Created By: Tran.Pham
            //Created Date: 12/08/2011
            SHIFT_COUNT_,//Code1_Code2_

            //Function: Dem so ngay lam ca tang ca có mã số phía sau dấu "_" >= 8
            //Created By: Hien.Pham
            //Created Date: 2013/10/21 
            OVERTIME_SHIFT_HOURS_,

            //Function: Dem so ngay lam ca có mã số phía sau dấu "_" >= 4
            //Created By: Hien.Pham
            //Created Date: 2013/10/21 great
            SHIFT4HOURS_COUNT_,//Code1_Code2_

            //Function: Dem so ngay lam ca tang ca có mã số phía sau dấu "_" >= 8
            //Created By: Hien.Pham
            //Created Date: 2013/10/21 
            OVERTIME_SHIFT_COUNT_,

            //Function: Dem so ngay lam ca tang ca có mã số phía sau dấu "_" >= 8
            //Created By: Hien.Pham
            //Created Date: 2013/10/21 
            OVERTIME4HOURS_SHIFT_COUNT_,

            //Function: Dem so ngay lam ca tang ca có mã số phía sau dấu "_" >= 8
            //Created By: Hien.Pham
            //Created Date: 2013/10/21 
            OVERTIME8HOURS_SHIFT_COUNT_,

            //Function: Dem so lan nghi trong thang có mã Code phía sau dấu "_"
            //Created By: SonNgo //LamLe - 20121008 - Chi dem so lan nghi 
            //Created Date: 03/01/2012
            LEAVEDAY_COUNT_,//Code1_Code2_

            //Function: Dem so ngay nghi trong thang có mã Code phía sau dấu "_"
            //Created By: LamLe
            //Created Date: 03/01/2012
            LEAVE_DAY_,//Code1_Code2_

            //Function: Dem so ngay nghi trong thang có mã Code phía sau dấu "_", trong khoản thời gian từ ngày kết thúc thử việc -> cuối tháng
            //Created By: SonNgo
            //Created Date: 20120627
            LEAVEDAY_END_PROBATION_COUNT_,//Code1_Code2_

            //Function: Tính Phụ cấp trừ cho tháng này từ Ngày nghỉ không lương của tháng trước
            //Created By: Son.Ngo
            //Created Date: 13/08/2011
            //UNPAID_LEAVEDAY_PREVMONTH, // SonNgo unuse

            //Function: Tính Phụ cấp cộng cho tháng này từ OT và nghỉ hưởng lương của tháng trước
            //Created By: Son.Ngo
            //Created Date: 20/08/2011
            //PAID_OT_PREVMONTH, // SonNgo unuse

            //Function: Tính lương thời gian từ ngày nghỉ đến cuối tháng để trừ lại Tổng lương - Áp dụng cho CPG
            //Created By: Son.Ngo
            //Created Date: 20/08/2011
            //SUBTRACT_ALLOWANCE_QUIT_EMPLOYEE_CPG, // SonNgo don't use

            //Function: Lương giờ
            //Created By: Son.Ngo
            //Created Date: 22/08/2011
            HOURLY_RATE,
            //Function: Lương giờ 1
            //Created By: Son.Ngo
            //Created Date: 22/08/2011
            HOURLY_RATE1,
            //Function: Có hưởng được phụ cấp tuần hay ko?
            //Created By: Hien.Pham
            //Created Date: 22/08/2011
            RECEIVE_WEEK_ATTENDANCE_FJK,
            //Function: Có hưởng được phụ cấp Tháng hay ko?
            //Created By: Hien.Pham
            //Created Date: 22/08/2011
            RECEIVE_MONTH_ATTENDANCE_FJK,
            //Function: Lương giờ sau dieu chinh
            //Created By: Son.Ngo
            //Created Date: 22/08/2011
            HOURLY_RATE2,
            //Function: số ca 3 làm trong tháng ban ngay
            //Created By: hien.pham 
            //Created Date: 10/01/2012
            COUNT_SHIFTA_N,
            //Function: số ca 3 làm trong tháng ban dem
            //Created By: hien.pham 
            //Created Date: 10/01/2012
            COUNT_SHIFTA_D,
            //Function: số ca 3 làm trong tháng ban dem
            //Created By: hien.pham 
            //Created Date: 10/01/2012
            COUNT_SHIFT_3,
            //Function: số tuần trong tháng
            //Created By: hien.pham 
            //Created Date: 10/01/2012
            COUNT_WEEK_IN_MONTH,

            //Function: Phụ cấp tiếng nhật. 
            //Created By: lam.le    
            //Created Date: 11/07/2012
            JAPANESE_COUNT,

            //Function: Kiem tra co ma so thue hay ko. 
            //Created By: lam.le    
            //Created Date: 11/07/2012
            CHECK_CODETAX,

            //Function: Kiem tra loại nhân viên. 
            //Created By: hien.pham   EmployeeType
            //Created Date: 07/03/2013
            EMPLOYEE_TYPE,

            //Function: Tổng số ngày nghỉ việc trong tháng tính lương
            //Created By: son.ngo    
            //Created Date: 28/07/2012
            TOTAL_DAYS_QUIT_COMPANY,

            //Function: Số ngày chưa vào công ty trong tháng tính lương
            //Created By: son.ngo    
            //Created Date: 28/07/2012
            TOTAL_DAYS_NOT_HIRE_COMPANY,

            //Function: Ngày nghỉ việc <= ngày kết thúc thử việc
            //Created By: son.ngo    
            //Created Date: 02/08/2012
            E_UNPASS_PROBATION,

            //Function: Nghỉ việc nhưng không kịp trả thẻ BHYT trong tháng
            //Created By: son.ngo    
            //Created Date: 31/08/2012
            E_NOT_RECIVIED_INSUBOOK_IN_TERMINATE_MONTH,

            //Function: Sinh nhật trong tháng
            //Created By: son.ngo    
            //Created Date: 04/09/2012
            E_BIRTHDAY,

            //Function: Ngày quốc tế phụ nữ (chỉ áp dụng cho Nữ)
            //Created By: son.ngo    
            //Created Date: 04/09/2012
            E_International_Women_Day,

            //Function: Kiểm tra trong tháng có bị kỷ luật
            //Created By: son.ngo    
            //Created Date: 08/09/2012
            E_DISCIPLINE,

            #endregion

            #region Salary
            /// <summary>
            /// Ngay vao cong ty
            /// </summary>
            DAY_HIREDATE,
            /// <summary>
            /// Luong dong BHXH
            /// </summary>
            INSURANCE_SALARY,
            /// <summary>
            /// Luong dong BHXH
            /// </summary>
            INSURANCE_SALARY_VND,
            /// <summary>
            /// Luong co ban
            /// </summary>
            BASIC_SALARY,
            /// <summary>
            /// Luong co ban
            /// </summary>
            BASIC_SALARY_VND,
            /// <summary>
            /// Luong tinh PIT
            /// </summary>
            SALARY_WITH_PIT,
            /// <summary>
            /// luong tinh PIT cua Thoi vu khong tru giam tru gia canh
            /// </summary>
            SALARY_PIT_CASUAL,
            /// <summary>
            /// money giam tru gia canh + Nguoi phu thuoc
            /// </summary>
            DEPENDANT_AMOUNT,
            #endregion

            #region Công Thức Phép Năm
            //Phep nam
            //Function:Tổng số ngày phép năm/phep benh/phep om
            //Created By: Tran.Pham
            //Created Date: 08/03/2011
            TOTAL,
            //Ngay phep duoc cong them trong nam. Cau hinh trong Att_AnnualLeave
            //Function: Att_AnnualLeave.AdditionalValue
            //Created By: Lam.Le
            //Created Date: 10/06/2011
            ADDITIONAL_ANNUAL,
            //Function: Lấy năm của kỳ lương nếu thỏa đk ngày vào làm thuộc kỳ lương
            YEAR_OF_SALARY_IF_DATEHIRE_BELONG,
            //Function: Lấy tháng của kỳ lương nếu thỏa đk ngày vào làm thuộc kỳ lương
            MONTH_OF_SALARY_IF_DATEHIRE_BELONG,
            //Function: Năm vào làm
            YEAR_OF_DATEHIRE,
            //Function: Tháng vào làm
            MONTH_OF_DATEHIRE,
            //Function: Năm kết thúc thử việc
            YEAR_OF_DATEENDPROBATION,
            //Function: Tháng kết thúc thử việc
            MONTH_OF_DATEENDPROBATION,
            //Function: Lấy năm hiện tại theo tháng tính lương
            CURRENTYEAR_INSALARY,
            //Function : Lấy tháng hiện tại theo kỳ lương
            CURRENTMONTH_INSALARY,
            //Function: Kiểm tra ngày bắt đầu thử việc hoặc ngày KT thử việc có thuộc kỳ lương ko
            IS_PROBATION,
            //Function: ngày kế thúc thử việc
            //Created By: Lam.Le
            //Created Date: 08/03/2011
            MONTHENDPRO,
            //Function:Tháng bắt dầu vào làm
            //Created By: Tran.Pham
            //Created Date: 08/03/2011
            MONTHSTART,

            TOTAL_LEAVE_BY_TYPE_IN_MONTH,

            DAYOFDATEHIRE,
            DAYOFDATEQUIT,

            JOBTITLE_ANNUAL,

            //Function: Tháng bắt đầu vào làm 
            //          Ngay 1-5 => 1     6-25 => 0.5   26-31=>0
            //Created By: Trung.Le
            //Created Date: 08/05/2011
            MONTHSTART2,
            //Function: Tháng kết thúc thử việc
            //          Ngay 1-5 => 1     6-25 => 0.5   26-31=>0
            //Created By: Trung.Le
            //Created Date: 26/05/2012
            MONTHSTART_PROB,
            MONTHEND,
            MONTHEND_BYPERIOD,
            HRE_ISQUITBEFORE15,
            //Function: Tháng nghỉ việc
            //Created By: Tran.Pham
            //Created Date: 08/03/2011
            CURRENTMONTH,
            //Function: Số ngày phép được hương dựa trên thăm niên làm việc
            //Created By: Hien.Pham
            //Created Date: 28/06/2011 
            //Function:Tháng vào làm
            //Created By: hien.pham
            //Created Date: 15/01/2013
            MONTHHIRE,

            //Năm vào làm
            YEARHIRE,

            //Function:Năm import
            //Created By: hien.pham
            //Created Date: 15/01/2013
            CURRENTYEAR,

            //Mã Chức Vụ
            POSITION_CODE,

            //Phần tử lấy số ngày thâm niên (theo đk của Danieli)
            THAMNIEN_DANIELI,

            SENIOR_BONUS_LEAVE,

            //Function: Số ngày phép được hương dựa trên thâm niên làm việc (với tuỳ biến tháng áp dụng quy tắc tính phép năm)
            //Created By: SơnNgô
            //Created Date: 07/05/2012
            SENIOR_BONUS_LEAVE_FROM_,
            #endregion

            #region Ngày công chuẩn
            //Function: Số ngày của tháng trừ ngày CN
            //Created By: Trân.Pham
            //Created Date: 20/07/2011
            DAYS_IN_MONTH_EXCLUDE_SUNDAY,

            //Function: Số ngày của tháng trừ ngày CN VÀ ngày nghỉ
            //Created By: Yen.Ho
            //Created Date: 19/08/2011
            DAYS_IN_MONTH_EXCLUDE_SUNDAY_HOLIDAY,

            //Function: Số lần được nhân phụ cấp breakfast
            //Created By: hien.pham
            //Created Date: 19/02/2012
            OT_BREAKFAST_COUNT,
            //Function: Số ngày lễ trong tháng
            //Created By: tran.pham
            //Created Date: 20/12/2011
            H,
            //Function: Số ngày lễ trong tháng
            //Created By: vi.nguyen
            //Created Date: 20/12/2011
            SAT,

            //Function: Số ngày CN trong tháng
            //Created By: tran.pham
            //Created Date: 20/12/2011
            S,

            /// <summary>
            /// LamLe - 20120611 - #0014210 Số ngày roster trong thang
            /// </summary>
            R,

            /// <summary>
            /// LamLe - 20120611 - #0014210 Số ngày roster trong thang, khong tinh ngay nghi le
            /// </summary>
            R_NOT_H,
            //Function: Số ngày của tháng tính công
            //Created By: tran.pham
            //Created Date: 20/12/2011
            D,
            /// <summary>
            /// Ngày công chuẩn tính theo quy tắt Fujikura
            /// </summary>
            WORKDAY_STANDARD_PER_MONTH,

            /// <summary>
            /// Số ca làm việc dựa theo thời gian của kỳ công
            /// </summary>
            SHIFT_COUNT,

            /// <summary>
            /// Tổng số ngaày cuủa năm trừ cn & ngay nghỉ
            /// </summary>
            WORKDAY_AVERAGE_PER_MONTH,

            //Function: Dem so ngay lam ca có mã số phía sau dấu "_"
            //Created By: SonNgo
            //Created Date: 14/03/2012
            SHIFT_PRE_MONTH_COUNT_,//Code1_Code2_

            #endregion

            #region Phụ cấp bất thường
            //Phep nam
            //Function:Phụ cấp hoàn thành công việc
            //Created By: hien.pham
            //Created Date: 17/11/2011
            PERFORMANCE_ALL,
            #endregion

            #region Ngày Nghỉ
            /// <summary>
            /// Ngày nghỉ trong tháng
            /// </summary>
            VALUE,

            #endregion

            #region Cấu Hình Công Thức Phép Năm

            /// <summary>
            /// Tháng bắt đầu tính phép nắm Mặc định là tháng 1
            /// </summary>
            CONFIG_ANL_MONTHBEGINYEAR,

            /// <summary>
            /// Ngày bắt đầu để tính tháng Full
            /// Sau ngày này sẽ chuyển tháng bắt đầu qua tháng sau
            /// lấy ngày này để tính phép thâm niên
            /// </summary>
            CONFIG_ANL_DAYBEGIN_FULLMONTH,

            /// <summary>
            /// Số tháng để bắt đầu tính thâm niên
            /// </summary>
            CONFIG_ANL_SENIOR_MONTH,

            /// <summary>
            /// Số ngày trung bình cho 1 tháng mặc đinh 30
            /// </summary>
            CONFIG_ANL_DAY_PER_MONTH,

            /// <summary>
            /// Hệ số để làm tròn lên và làm tròn xuống mặc định 0.5
            /// </summary>
            CONFIG_ANL_ROUND_UP,

            /// <summary>
            /// Loai để bắt đầu tính phép cho 1 nhân viên vd. Ngày DateHire, ngày Hến Hạn thử việc v.v
            /// Value của phần tử này là Enum AnlProfileTypeBegin
            /// </summary>
            CONFIG_ANL_TYPE_PROFILE_BEGIN,

            /// <summary>
            /// Số Ngày để bắt đầu tính Nghỉ phép và tính quy tắc HDT4 HDT5
            /// </summary>
            CONFIG_ANL_DAY_MAX_IN_MONTH_GET_ACTUAL,


            /// <summary>
            /// số phép năm bình thường có thể nhận 1 năm
            /// </summary>
            CONFIG_ANL_NORMAL_CAN_GET_FULLYEAR,
            /// <summary>
            /// số phép năm Có có thể nhận cho 1 bậc . Mặc định là 1
            /// </summary>
            CONFIG_ANL_SENIOR_CAN_GET_EACH_LEVEL,
            /// <summary>
            /// số phép năm Có có thể nhận nhiều hơn NORMAL . Mặc định là 2
            /// </summary>
            CONFIG_ANL_HDT4_CAN_GET_MORE_NORMAL,
            /// <summary>
            /// số phép năm Có có thể nhận nhiều hơn NORMAL . Mặc định là 4
            /// </summary>
            CONFIG_ANL_HDT5_CAN_GET_MORE_NORMAL,
            /// <summary>
            /// Các loại mã ngày nghỉ không được hưởng phép năm
            /// </summary>
            CONFIG_LEAVE_NON_ALN_CODES,
            /// <summary>
            /// thang trong nam de tinh phem nam honda la thang 10 hang nam
            /// </summary>
            CONFIG_MONTH_INYEAR_TO_COMPUTE_SENIOR,
            /// <summary>
            /// Số tháng được phép làm tròn lên
            /// </summary>
            CONFIG_MONTH_ROUND_UP,

            #endregion

            #region Công Thức Phép Năm 2

            /// <summary>
            /// Số tháng đc tính phép năm bình thường
            /// </summary>
            ANL_NORMAL,

            /// <summary>
            /// Số tháng nghỉ ko được hưởng phép
            /// </summary>
            ANL_LEAVE_NON_HAVEANL,

            /// <summary>
            /// Lây ra tháng Hiện Tại
            /// </summary>
            ANL_CURRENTMONTH,

            /// <summary>
            /// Tính ra số tháng được hưởng thâm niên
            /// </summary>
            ANL_SENIOR,

            /// <summary>
            /// Tính ra nhân viên đó tháng đó có làm công việc nặng nhọc loại 4 hay không ?? Trả về 1 hoặc 0
            /// </summary>
            ANL_WORK_HDT4,


            /// <summary>
            /// Tính ra nhân viên đó tháng đó có làm công việc nặng nhọc loại 5 hay không ?? Trả về 1 hoặc 0
            /// </summary>
            ANL_WORK_HDT5,


            #endregion
        }
        public enum FormulaOvertimeConstant
        {
            //Function:So gio lam tron buoc nhay
            //Created By: LamLe
            //Created Date: 20/11/2012
            CONFIG_STEPUP_ROUND,
            //Function:Gio vao khi tang ca
            //Created By: LamLe
            //Created Date: 20/11/2012
            CONFIG_MINUTE_ROUND,
            //Function:Gio vao khi tang ca
            //Created By: LamLe
            //Created Date: 20/11/2012
            CONFIG_ISUP_ROUND,

            //Function:Gio vao khi tang ca
            //Created By: LamLe
            //Created Date: 20/11/2012
            IN,
            //Function:Gio ra khi tang ca
            //Created By: LamLe
            //Created Date: 20/11/2012
            OUT,
            //Function:vao giua ca khi tang ca
            //Created By: LamLe
            //Created Date: 20/11/2012
            BREAK_START,
            //Function:ra giua ca khi tang ca
            //Created By: LamLe
            //Created Date: 20/11/2012
            BREAK_END,
            //Function:bat dau tang ca
            //Created By: LamLe
            //Created Date: 20/11/2012
            OT_START,
            //Function:ket thuc tang ca
            //Created By: LamLe
            //Created Date: 20/11/2012
            OT_END,
            //Function:Gio xac nhan chuan.
            //Created By: LamLe
            //Created Date: 20/11/2012
            HOURS_CONFIRM_DEFAULT_OT,
            //Function:Gio nghi giua ca
            //Created By: LamLe
            //Created Date: 20/11/2012
            HOURS_BREAK_SHIFT,
            //Function:Gio nghi giua OT
            //Created By: LamLe
            //Created Date: 20/11/2012
            HOURS_BREAK_OT,
            //Function:Kiem tra co dang ky suat an ko
            //Created By: LamLe
            //Created Date: 20/11/2012
            IS_REG_FOOD,
            //Function:Ma ca lam viec, dang ky OT
            //Created By: LamLe
            //Created Date: 20/11/2012
            CODE_SHIFT_OT,
            //Function:Loai duration OT
            //Created By: LamLe
            //Created Date: 20/11/2012
            TYPE_DURATION_OT,
            //Function:Loai cua Fuji: Neu ngay tang ca khong phai ngay lam viec(isworkday == false) va tang ca thuoc shift "O" & "D"  thi duoc + them 0.5
            //Created By: LamLe
            //Created Date: 20/11/2012
            FUJI_BONUS_BREAK_SHIFT_HOURS,
            //Function:Loai cua Fuji: Neu h xac nhan tang ca chuan > 2h thi khong tru gio an khi tang ca (0.5) ; <= 2 tru gio an khi tang ca. (dk co dang ky suat an.)  
            //Created By: LamLe
            //Created Date: 20/11/2012
            FUJI_DEDUCTION_BREAK_FOOD_HOURS
        }
        #endregion

        protected string formula;

        public Formula(string formula)
        {
            if (formula == null || formula == String.Empty)
                throw new
                    ArgumentException("formula can't be empty", "formula");

            this.formula = formula;
        }

        protected CommonTree Parse(string expression)
        {
            ECalcLexer lexer = new ECalcLexer(new ANTLRStringStream(expression));
            ECalcParser parser = new ECalcParser(new CommonTokenStream(lexer));

            try
            {
                RuleReturnScope rule = parser.expression();
                if (parser.HasError)
                {
                    throw new EvaluationException(parser.ErrorMessage + " " + parser.ErrorPosition);
                }

                return rule.Tree as CommonTree;
            }
            catch (EvaluationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new EvaluationException(e.Message, e);
            }
        }

        public object Evaluate()
        {
            EvaluationVisitor visitor = new EvaluationVisitor();
            visitor.EvaluateFunction += new EvaluateFunctionHandler(HandleFunction);
            visitor.EvaluateParameter += EvaluateParameter;
            visitor.Parameters = parameters;

            LogicalExpression.Create(Parse(formula)).Accept(visitor);
            return visitor.Result;
        }

        void HandleFunction(string name, Evaluant.Calculator.FunctionArgs args)
        {
            if (name == Formula.FormulaConstant.OR.ToString())
                args.Result = Convert.ToBoolean(args.Parameters[0]) || Convert.ToBoolean(args.Parameters[1]);
            else if (name == "AND")//Formula.FormulaConstant.AND.ToString())
            {
                args.Result = Convert.ToBoolean(args.Parameters[0]) && Convert.ToBoolean(args.Parameters[1]);
            }
            else if (name == Formula.FormulaConstant.ROUND.ToString())
            {
                //20120816 vinguyen
                //xu ly round(a,-2)

                if (int.Parse(args.Parameters[1].ToString()) < 0)
                {
                    int intPara0 = (int)Convert.ToDouble(args.Parameters[0]);
                    int intPara1 = (int)Convert.ToDouble(args.Parameters[1]);

                    if (intPara0 == 0) args.Result = 0;
                    else
                        args.Result = intPara0 - (int)(intPara0 % Math.Pow(10, Math.Abs(intPara1)));
                }
                else
                {
                    args.Result = Math.Round(Convert.ToDouble(args.Parameters[0])
                        , Convert.ToInt16(args.Parameters[1]), MidpointRounding.AwayFromZero);
                }
            }
            else if (name == Formula.FormulaConstant.ROUNDCUSTOMIZE.ToString())
            {
                Double _hours = Convert.ToDouble(args.Parameters[0]);
                Double upToHours = Convert.ToDouble(args.Parameters[1]);
                Double minuteRound = Convert.ToDouble(args.Parameters[2]);
                Boolean isUpto = Convert.ToBoolean(args.Parameters[3]);
                args.Result = Common.Round(_hours, upToHours, minuteRound, isUpto);
            }
            else if (name == Formula.FormulaConstant.ROUNDUP.ToString())
            {
                int intValue = (int)Convert.ToDouble(args.Parameters[0]);
                double doubleValue = intValue / Math.Pow(10, Math.Abs(Convert.ToInt16(args.Parameters[1])));
                args.Result = Math.Ceiling(doubleValue) * Math.Pow(10, Math.Abs(Convert.ToInt16(args.Parameters[1])));

            }
            else if (name == Formula.FormulaConstant.ISNULL.ToString())
            {
                if (args.Parameters[0] != null)
                    args.Result = Convert.ToDouble(args.Parameters[0]);
                else
                    args.Result = Convert.ToDouble(args.Parameters[1]);
            }
            else if (name == Formula.FormulaConstant.INT.ToString())
            {
                int intValue = (int)Convert.ToDouble(args.Parameters[0]);
                args.Result = intValue;

            }
            else if (name == "floor")//Formula.FormulaConstant.ROUND.ToString())
            {
                args.Result = Math.Floor(Convert.ToDouble(args.Parameters[0]));
            }
            else if (name == Formula.FormulaConstant.MFLOOR.ToString())
            {
                args.Result = MFLOOR(Convert.ToDouble(args.Parameters[0]), Convert.ToDouble(args.Parameters[1]));
            }
            //else if (name == Formula.FormulaConstant.PIT.ToString())
            //{
            //    Double mainSalary = Convert.ToDouble(args.Parameters[0]);
            //    string strFormula = args.Parameters[1].ToString();
            //    args.Result = EvaluatePITFormula(mainSalary, strFormula);
            //}
        }

        #region MATH.FLOOR with Significance

        double MFLOOR(Double value, Double Significance)
        {
            return Math.Round(value / Significance) * Significance;
        }


        #endregion

        public event EvaluateFunctionHandler EvaluateFunction;
        public event EvaluateParameterHandler EvaluateParameter;

        private Hashtable parameters = new Hashtable();

        public Hashtable Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        /// <summary>
        /// Kiem tra formula co chua mot param hay ko?
        /// </summary>
        /// <param name="paramName">Ten Parameter</param>
        /// <returns></returns>
        public bool ContainParam(FormulaConstant paramName)
        {
            bool result = false;
            string bracketParamName = "[" + paramName.ToString() + "]";
            if (!formula.IsNullOrEmpty())
            {
                if (formula.Contains(bracketParamName))
                    result = true;
            }
            return result;
        }

    }



    #endregion

}
