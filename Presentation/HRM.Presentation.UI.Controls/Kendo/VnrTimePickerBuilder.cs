using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using HRM.Presentation.UI.Controls.Kendo;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public static class VnrTimePickerBuilder
    {
        public static TimePickerBuilder VnrTimePicker<TModel>(this HtmlHelper<TModel> helper, TimePickerBuilderInfo builderInfo)
        {
            var events = new Action<DatePickerEventBuilderBase>(e =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.EventChange))
                {
                    e.Change(builderInfo.EventChange);
                }

            });
            var timePickerBuilder = helper.Kendo().TimePicker()
                .Enable(builderInfo.Enable)
                .Format(builderInfo.Format)
                .Events(events);

            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                timePickerBuilder.Name(builderInfo.Name);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.Value))
            {
                timePickerBuilder.Value(builderInfo.Value);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
            {
                timePickerBuilder.HtmlAttributes(new { style = builderInfo.StyleCss, placeholder = builderInfo.Placeholder });
            }
            return timePickerBuilder;
        }

        public static TimePickerBuilder VnrTimePickerFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, DateTime>> expression, TimePickerBuilderInfo builderInfo)
        {
             var events = new Action<DatePickerEventBuilderBase>(e =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.EventChange))
                {
                    e.Change(builderInfo.EventChange);
                }
                
            });
            var timePickerBuilder = helper.Kendo().TimePickerFor(expression)
                .Enable(builderInfo.Enable)
                .Format(builderInfo.Format)
                .Events(events);

            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                timePickerBuilder.Name(builderInfo.Name);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.Value))
            {
                timePickerBuilder.Value(builderInfo.Value);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
            {
                timePickerBuilder.HtmlAttributes(new { style = builderInfo.StyleCss, placeholder = builderInfo.Placeholder });
            }
            return timePickerBuilder;
        }

        public static TimePickerBuilder VnrTimePickerFor<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, DateTime?>> expression, TimePickerBuilderInfo builderInfo)
        {
            var events = new Action<DatePickerEventBuilderBase>(e =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.EventChange))
                {
                    e.Change(builderInfo.EventChange);
                }

            });
            var timePickerBuilder = helper.Kendo().TimePickerFor(expression)
                .Enable(builderInfo.Enable)
                .Format(builderInfo.Format)
                .Events(events);
            
            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                timePickerBuilder.Name(builderInfo.Name);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Value))
            {
                timePickerBuilder.Value(builderInfo.Value);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
            {
                timePickerBuilder.HtmlAttributes(new { style = builderInfo.StyleCss, placeholder = builderInfo.Placeholder });
            }

            return timePickerBuilder;
        }
    }
}
#region TimePicker Information
public class TimePickerBuilderInfo : VnrBaseBuilderInfo
{
    #region Properties

    private string _format = "HH:mm";
    private bool _enable = true;
    public override string Name { get; set; }
    public string Placeholder { get; set; }

    /// <summary>
    /// Truyền giá trị mặc định khi gọi TimePicker
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Tùy chỉnh lại định dạng cho TimePicker như: độ rộng, chiều cao....
    /// </summary>
    public string StyleCss { get; set; }

    /// <summary>
    /// Sự kiện khi thay đổi giá trị
    /// </summary>
    public string EventChange { get; set; }

    /// <summary>
    /// Tắt/Mở DatePicker mặc định là True
    /// </summary>
    public bool Enable
    {
        get { return _enable; }
        set { _enable = value; }
    }

    /// <summary>
    /// Định dạng hiển thị cho DatePicker
    /// </summary>
    public string Format
    {
        get { return _format; }
        set { _format = value; }
    }
    #endregion

}
#endregion 