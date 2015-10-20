using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System.Globalization;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public static class VnrDateTimePickerBuilder
    {

        public static DateTimePickerBuilder VnrDateTimePicker<TModel>(this HtmlHelper<TModel> helper, DateTimePickerBuilderInfo builderInfo)
        {
            var dateTimePickerBuilder = helper.Kendo().DateTimePicker()
                .Enable(builderInfo.Enable)
                .Format(ConstantFormat.HRM_Format_DayMonthYear_HoursMin)
                .TimeFormat(builderInfo.TimeFormat)
                .Start(builderInfo.Start)
                .Depth(builderInfo.Depth)
                .Min(builderInfo.Min)
                .Max(builderInfo.Max);
                //.Culture(builderInfo.Culture);

            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                dateTimePickerBuilder.Name(builderInfo.Name);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.Value))
            {
                dateTimePickerBuilder.Value(builderInfo.Value);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
            {
                dateTimePickerBuilder.HtmlAttributes(new { style = builderInfo.StyleCss, placeholder = builderInfo.Placeholder });
            }

            return dateTimePickerBuilder;
        }

        public static DateTimePickerBuilder VnrDateTimePickerFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, DateTime>> expression, DateTimePickerBuilderInfo builderInfo)
        {
            var dateTimePickerBuilder = helper.Kendo().DateTimePickerFor(expression)
                .Enable(builderInfo.Enable)
                .Format(ConstantFormat.HRM_Format_DayMonthYear_HoursMin)
                .Start(builderInfo.Start)
                .Depth(builderInfo.Depth)
                .TimeFormat(builderInfo.TimeFormat)
                .Min(builderInfo.Min)
                .Max(builderInfo.Max);
                //.Culture(builderInfo.Culture);

            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                dateTimePickerBuilder.Name(builderInfo.Name);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.Value))
            {
                dateTimePickerBuilder.Value(builderInfo.Value);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
            {
                dateTimePickerBuilder.HtmlAttributes(new { style = builderInfo.StyleCss, placeholder = builderInfo.Placeholder });
            }

            return dateTimePickerBuilder;
        }

        public static DateTimePickerBuilder VnrDateTimePickerFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, DateTime?>> expression, DateTimePickerBuilderInfo builderInfo)
        {
            var dateTimePickerBuilder = helper.Kendo().DateTimePickerFor(expression)
                .Enable(builderInfo.Enable)
                .Start(builderInfo.Start)
                .Format(builderInfo.Format)
                .TimeFormat(builderInfo.TimeFormat)
                .Depth(builderInfo.Depth)
                .Min(builderInfo.Min)
                .Max(builderInfo.Max);
                //.Culture(builderInfo.Culture);
            if (!string.IsNullOrWhiteSpace(builderInfo.Format))
            {
                //dateTimePickerBuilder.Format(builderInfo.Format);
                //[Hien.Nguyen] 21/10/2014 - Đồng nhất lại định dạng là dd/MM/yyyy
                dateTimePickerBuilder.Format(ConstantFormat.HRM_Format_DayMonthYear_HoursMin);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                dateTimePickerBuilder.Name(builderInfo.Name);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.Value))
            {
                dateTimePickerBuilder.Value(builderInfo.Value);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
            {
                dateTimePickerBuilder.HtmlAttributes(new { style = builderInfo.StyleCss, placeholder = builderInfo.Placeholder });
            }

            return dateTimePickerBuilder;
        }
    }

    #region DateTimePicker Information
    public class DateTimePickerBuilderInfo : VnrBaseBuilderInfo
    {
        #region Properties

        //private string _format = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        //[Hien.Nguyen] Thay đổi lại format của DatePicker
        private string _format = ConstantFormat.HRM_Format_DayMonthYear_HoursMin;
        private string _timeFormat = ConstantFormat.HRM_Format_HourMin;
        private bool _enable = true;
        private string _culture = CultureInfo.CurrentCulture.ToString();
        private DateTime _min = new DateTime(1970, 1, 1);
        private DateTime _max = new DateTime(2050, 1, 1);
        private CalendarView _start = CalendarView.Month;
        private CalendarView _depth = CalendarView.Month;

        public string Placeholder { get; set; }
        public override string Name { get; set; }

        /// <summary>
        /// Truyền giá trị mặc định khi gọi DateTimePicker
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Tùy chỉnh lại định dạng cho DateTimePicker như: độ rộng, chiều cao....
        /// </summary>
        public string StyleCss { get; set; }

        /// <summary>
        /// Tắt/Mở DatePicker mặc định là True
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        /// <summary>
        /// Loại DatePicker hiển thị theo từng quốc gia
        /// Xem chi tiết trong thư mục Scripts/kendo/'VersionKendo'/cultures
        /// Nếu muốn sử dụng DateTimePicker cho Tiếng Việt thì dùng: vi-VN
        /// DatePicker mặc định là: en-US
        /// </summary>
        public string Culture
        {
            get { return _culture; }
            set { _culture = value; }
        }

        /// <summary>
        /// Định dạng hiển thị cho DateTimePicker
        /// </summary>
        public string Format
        {
            get { return _format; }
            set { _format = value; }
        }
        public string TimeFormat
        {
            get { return _timeFormat; }
            set { _timeFormat = value; }
        }
        /// <summary>
        /// Giá trị lớn nhất cho DateTimePicker
        /// </summary>
        public DateTime Min
        {
            get { return _min; }
            set { _min = value; }
        }

        /// <summary>
        /// Ngày nhỏ nhất cho DateTimePicker
        /// </summary>
        public DateTime Max
        {
            get { return _max; }
            set { _max = value; }
        }

        /// <summary>
        /// Kiểu hiển thị của DateTimePicker khi bắt đầu
        /// </summary>
        public CalendarView Start
        {
            get { return _start; }
            set { _start = value; }
        }

        /// <summary>
        /// Kiểu hiển thị của DateTimePicker khi chuyển hướng
        /// </summary>
        public CalendarView Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }
        #endregion

    }
    #endregion 
}
