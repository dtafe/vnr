using HRM.Presentation.UI.Controls.Kendo.ControlBase;
using Kendo.Mvc.UI.Fluent;
using System.Collections.Generic;
using System.Web.Mvc;
using Kendo.Mvc.UI;

namespace HRM.Presentation.UI.Controls.Kendo.DatePicker
{
    #region VnrDatepickerBuilder
    public static class VnrDatePickerControl
    {
        public static DatePickerBuilder VnrDatePicker(this HtmlHelper helper, DatepickerInfomation date) 
        {
            return helper.Kendo().DatePicker()
                .Name(date.Name)
                .Enable(date.Enable)
                .Min(date.Min)
                .Max(date.Max)
                .ARIATemplate(date.ARIATemplate)
                .Start(date.Start)
                .Depth(date.Depth)
                ;
        }
    }
    #endregion

    #region DatepickerInfomation
    public class DatepickerInfomation : VnrPropertiesBase
    {
        #region Properties

        private string ariaTemplate;
        private CalendarView start;
        private CalendarView depth;
        private string min;
        private string max;
        Dictionary<string, string> format;


        /// <summary>
        /// Quy định mẫu chung
        /// </summary>
        public string ARIATemplate
        {
            get { return ariaTemplate; }
            set { ariaTemplate = value; }
        }

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

        #endregion

        #region Methods

        #endregion

        #region Events

        #endregion

    }
    #endregion
}
