using HRM.Presentation.UI.Controls.Kendo.ControlBase;
using Kendo.Mvc.UI.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kendo.Mvc.UI;

namespace HRM.Presentation.UI.Controls.Kendo.DatePicker
{
    #region VnrDateTimepickerBuilder
    public static class VnrDateTimePickerControl
    {
        public static DateTimePickerBuilder VnrDateTimePicker(this HtmlHelper helper, DateTimepickerInfomation dt)
        {
            return helper.Kendo().DateTimePicker()
                .Name(dt.Name)
                .Start(dt.Start)
                .Depth(dt.Depth)
                .Min(dt.Min)
                .Max(dt.Max)
                ;

        }
    }
    #endregion

    #region DateTimepickerInfomation
    public class DateTimepickerInfomation : VnrPropertiesBase
    {
        #region Properties

        private CalendarView start;
        private CalendarView depth;
        private string min;
        private string max;

        Dictionary<string, string> format;
        Dictionary<string, string> timeformat;



        /// <summary>
        /// Giá trị hiển thị ban đầu(VD: Theo tháng hay năm )
        /// </summary>
        public CalendarView Start
        {
            get { return start; }
            set { start = value; }
        }
        public CalendarView Depth
        {
            get { return depth; }
            set { depth = value; }
        }
        //Giá trị hiển thị nhỏ nhất
        public string Min
        {
            get { return min; }
            set { min = value; }
        }
        //Giá triọ hiển thị lớn nhất
        public string Max
        {
            get { return max; }
            set { max = value; }
        }

        public Dictionary<string, string> Format
        {
            get
            {
                if (format == null)
                {
                    format = new Dictionary<string, string>();
                }

                return format;
            }
            internal set
            {
                format = value;
            }
        }

        public Dictionary<string, string> Timeformat
        {
            get { return timeformat; }
            set { timeformat = value; }
        }

        #endregion

        #region Methods

        #endregion

    }
    #endregion
}
