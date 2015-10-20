using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System.Globalization;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public static class VnrDatePickerBuilder
    {
        public static DatePickerBuilder VnrDatePicker<TModel>(this HtmlHelper<TModel> helper, DatePickerBuilderInfo builderInfo)
        {
            Action<DatePickerEventBuilderBase> events = new Action<DatePickerEventBuilderBase>(d =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.EventSelect))
                {
                    d.Change(builderInfo.EventSelect);
                }
            });
            var datePickerBuilder = helper.Kendo().DatePicker()
                .Enable(builderInfo.Enable)
                .Start(builderInfo.Start)
                .Depth(builderInfo.Depth)
                .Min(builderInfo.Min)
                .Max(builderInfo.Max)
                .Events(events);

            //.Culture(builderInfo.Culture);


            if (!string.IsNullOrWhiteSpace(builderInfo.Format))
            {
                datePickerBuilder.Format(builderInfo.Format);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                datePickerBuilder.Name(builderInfo.Name);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.Value))
            {
                datePickerBuilder.Value(builderInfo.Value);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss) || !string.IsNullOrWhiteSpace(builderInfo.Placeholder))
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss) && !string.IsNullOrWhiteSpace(builderInfo.Placeholder))
                {
                    datePickerBuilder.HtmlAttributes(new { style = builderInfo.StyleCss, placeholder = builderInfo.Placeholder });
                }
                else if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
                {
                    datePickerBuilder.HtmlAttributes(new { style = builderInfo.StyleCss });
                }
                else if (!string.IsNullOrWhiteSpace(builderInfo.Placeholder))
                {
                    datePickerBuilder.HtmlAttributes(new { placeholder = builderInfo.Placeholder });
                }
            }

            return datePickerBuilder;
        }

        //public static DatePickerBuilder VnrDatePickerFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, DateTime?>> expression, DatePickerBuilderInfo builderInfo)
        //{
        //    var datePickerBuilder = helper.Kendo().DatePickerFor(expression)
        //        .Enable(builderInfo.Enable)
        //        .Format(builderInfo.Format)
        //        .Start(builderInfo.Start)
        //        .Depth(builderInfo.Depth)
        //        .Min(builderInfo.Min)
        //        .Max(builderInfo.Max)
        //        .Culture(builderInfo.Culture);

        //    if (!string.IsNullOrWhiteSpace(builderInfo.Name))
        //    {
        //        datePickerBuilder.Name(builderInfo.Name);
        //    }

        //    if (!string.IsNullOrWhiteSpace(builderInfo.Value))
        //    {
        //        datePickerBuilder.Value(builderInfo.Value);
        //    }

        //    if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
        //    {
        //        datePickerBuilder.HtmlAttributes(new { style = builderInfo.StyleCss });
        //    }

        //    return datePickerBuilder;
        //}

        public static DatePickerBuilder VnrDatePickerFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, DateTime>> expression, DatePickerBuilderInfo builderInfo)
        {

            Action<DatePickerEventBuilderBase> events = new Action<DatePickerEventBuilderBase>(d =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.EventSelect))
                {
                    d.Change(builderInfo.EventSelect);
                }
            });
            var datePickerBuilder = helper.Kendo().DatePickerFor(expression)
                .Enable(builderInfo.Enable)
                .Start(builderInfo.Start)
                .Depth(builderInfo.Depth)
                .Min(builderInfo.Min)
                .Max(builderInfo.Max)
                .Events(events); 
            //.Culture(builderInfo.Culture);
            if (!string.IsNullOrWhiteSpace(builderInfo.Format))
            {
                datePickerBuilder.Format(builderInfo.Format);
                //[Hien.Nguyen] 21/10/2014 - Đồng nhất lại định dạng là dd/MM/yyyy
                //datePickerBuilder.Format(ConstantFormat.HRM_Format_DayMonthYear);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                datePickerBuilder.Name(builderInfo.Name);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.Value))
            {
                datePickerBuilder.Value(builderInfo.Value);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss) || !string.IsNullOrWhiteSpace(builderInfo.Placeholder))
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss) && !string.IsNullOrWhiteSpace(builderInfo.Placeholder))
                {
                    datePickerBuilder.HtmlAttributes(new { style = builderInfo.StyleCss, placeholder = builderInfo.Placeholder });
                }
                else if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
                {
                    datePickerBuilder.HtmlAttributes(new { style = builderInfo.StyleCss });
                }
                else if (!string.IsNullOrWhiteSpace(builderInfo.Placeholder))
                {
                    datePickerBuilder.HtmlAttributes(new { placeholder = builderInfo.Placeholder });
                }
            }

            return datePickerBuilder;
        }

        public static DatePickerBuilder VnrDatePickerFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, DateTime?>> expression, DatePickerBuilderInfo builderInfo)
        {
            Action<DatePickerEventBuilderBase> events = new Action<DatePickerEventBuilderBase>(d =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.EventSelect))
                {
                    d.Change(builderInfo.EventSelect);
                }
                else
                {
                    d.Change("ValidationDatetime");
                }
            });
            var datePickerBuilder = helper.Kendo().DatePickerFor(expression)
                .Enable(builderInfo.Enable)
                .Start(builderInfo.Start)
                .Depth(builderInfo.Depth)
                .Min(builderInfo.Min)
                .Max(builderInfo.Max)
                .Events(events);
            //.Culture(builderInfo.Culture)
           // .Format("dd/MM/yyyy")
            ;
            if (!string.IsNullOrWhiteSpace(builderInfo.Format))
            {
                datePickerBuilder.Format(builderInfo.Format);
                //[Hien.Nguyen] 21/10/2014 - Đồng nhất lại định dạng là dd/MM/yyyy
                //datePickerBuilder.Format(ConstantFormat.HRM_Format_DayMonthYear);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                datePickerBuilder.Name(builderInfo.Name);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.Value))
            {
                datePickerBuilder.Value(builderInfo.Value);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss) || !string.IsNullOrWhiteSpace(builderInfo.Placeholder))
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss) && !string.IsNullOrWhiteSpace(builderInfo.Placeholder))
                {
                    datePickerBuilder.HtmlAttributes(new { style = builderInfo.StyleCss, placeholder = builderInfo.Placeholder });
                }
                else if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
                {
                    datePickerBuilder.HtmlAttributes(new { style = builderInfo.StyleCss });
                }
                else if (!string.IsNullOrWhiteSpace(builderInfo.Placeholder))
                {
                    datePickerBuilder.HtmlAttributes(new { placeholder = builderInfo.Placeholder });
                }
            }
            
            return datePickerBuilder;
        }
    }

    #region DatePicker Information
    public class DatePickerBuilderInfo : VnrBaseBuilderInfo
    {
        #region Properties

        //private string _format = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        //[Hien.Nguyen] Thay đổi lại format của DatePicker
        private string _format = ConstantFormat.HRM_Format_DayMonthYear;
        private bool _enable = true;
        private string _culture = CultureInfo.CurrentCulture.ToString();
        private DateTime _min = new DateTime(1970, 1, 1);
        private DateTime _max = new DateTime(2050, 1, 1);
        private CalendarView _start = CalendarView.Month;
        private CalendarView _depth = CalendarView.Month;

        public override string Name { get; set; }
        public string Placeholder { get; set; }

        /// <summary>
        /// Truyền giá trị mặc định khi gọi DatePicker
        /// </summary>
        public string Value { get; set; }

        public string EventSelect { get; set; }

        /// <summary>
        /// Tùy chỉnh lại định dạng cho DatePicker như: độ rộng, chiều cao....
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
        /// Nếu muốn sử dụng DatePicker cho Tiếng Việt thì dùng: vi-VN
        /// DatePicker mặc định là: en-US
        /// </summary>
        public string Culture
        {
            get { return _culture; }
            set { _culture = value; }
        }

        /// <summary>
        /// Định dạng hiển thị cho DatePicker
        /// </summary>
        public string Format
        {
            get { return _format; }
            set { _format = value; }
        }

        /// <summary>
        /// Giá trị lớn nhất cho DatePicker
        /// </summary>
        public DateTime Min
        {
            get { return _min; }
            set { _min = value; }
        }

        /// <summary>
        /// Ngày nhỏ nhất cho DatePicker
        /// </summary>
        public DateTime Max
        {
            get { return _max; }
            set { _max = value; }
        }

        /// <summary>
        /// Kiểu hiển thị của DatePicker khi bắt đầu
        /// </summary>
        public CalendarView Start
        {
            get { return _start; }
            set { _start = value; }
        }

        /// <summary>
        /// Kiểu hiển thị của DatePicker khi chuyển hướng
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
