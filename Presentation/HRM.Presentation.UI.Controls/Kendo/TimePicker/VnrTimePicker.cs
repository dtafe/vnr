using HRM.Presentation.UI.Controls.Kendo.ControlBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kendo.Mvc.UI.Fluent;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using VnResource.Helper.Data;

namespace HRM.Presentation.UI.Controls.Kendo.DatePicker
{
    #region VnrTimepickerBuilder
    public static class VnrTimePicker
    {
        public static TimePickerBuilder TimePickerControl(this HtmlHelper helper, TimepickerInfomation time)
        {
            return helper.Kendo().TimePicker()
                .Name(time.Name)
                .Culture(time.Culture)
                .Format(time.Format)
                .ParseFormats(time.ParseFormats)
                .Interval(time.Interval)
                .Min(time.Min)
                .Max(time.Max)
                ;
        }
    }
    #endregion

    #region TimepickerInfomation
    public class TimepickerInfomation : VnrPropertiesBase
    {
        #region Properties

        private string culture;
        private int interval = 30;
        private string min;
        private string max;
        private string format;
        private string[] parseFormats;


        /// <summary>
        /// Ngôn ngữ hiển thị trong Timepicker (VD: vi-VN)
        /// </summary>
        public string Culture
        {
            get { return culture; }
            set { culture = value; }
        }
        
        /// <summary>
        /// Khoảng cách tính bằng phút giữa các giá trị (mặc định là 30p)
        /// </summary>
        public int Interval
        {
            get { return interval; }
            set { interval = value; }
        }

        /// <summary>
        /// Giá trị hiển thị nhỏ nhất
        /// </summary>
        public string Min
        {
            get { return min; }
            set { min = value; }
        }

        /// <summary>
        /// Giá trị hiển thị lớn nhất
        /// </summary>
        public string Max
        {
            get { return max; }
            set { max = value; }
        }

        /// <summary>
        /// Định dạng kiểu hiển thị của control
        /// </summary>
        public string Format
        {
            get { return format; }
            set { format = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] ParseFormats
        {
            get { return parseFormats; }
            set { parseFormats = value; }
        }

        #endregion

        #region Methods

        #endregion

        #region Events

        #endregion

    }
    #endregion
}
