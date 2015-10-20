using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System.Collections;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public static class VnrMultiSelectBuilder
    {
        public static MultiSelectBuilder VnrMultiSelect(this HtmlHelper helper, MultiSelectBuilderInfo builderInfo)
        {
            var dataSource = new Action<ReadOnlyDataSourceBuilder>(d =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.Url))
                {
                    d.Read(read => read.Url(builderInfo.Url).Type(HttpVerbs.Post)).ServerFiltering(builderInfo.ServerFiltering);
                }
                else if (!string.IsNullOrWhiteSpace(builderInfo.ActionName))
                {
                    d.Read(builderInfo.ActionName, builderInfo.Controller).ServerFiltering(builderInfo.ServerFiltering);
                }
            });

            var multiSelectBuilder = helper.Kendo().MultiSelect()
                .Enable(builderInfo.Enable)
                .Filter(builderInfo.FilterType)
                .AutoBind(builderInfo.Autobind)
                .DataSource(dataSource).ValuePrimitive(true)
                .Height(builderInfo.Height)
                .HtmlAttributes(new { style = "width:" + builderInfo.Width + "px;" })
                .MaxSelectedItems(builderInfo.MaxSelectedItems);

            if (builderInfo.BindTo!=null)
            {
                multiSelectBuilder.BindTo(builderInfo.BindTo);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.EventChange))
            {
                multiSelectBuilder.Events(m => m.Change(builderInfo.EventChange));
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.EventSelect))
            {
                multiSelectBuilder.Events(m => m.Change(builderInfo.EventSelect));
            }
            if (builderInfo.Value!=null)
            {
                multiSelectBuilder.Value(builderInfo.Value);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.Placeholder))
            {
                multiSelectBuilder.Placeholder(builderInfo.Placeholder);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                multiSelectBuilder.Name(builderInfo.Name);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.DataTextField))
            {
                multiSelectBuilder.DataTextField(builderInfo.DataTextField);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.DataValueField))
            {
                multiSelectBuilder.DataValueField(builderInfo.DataValueField);
            }

            return multiSelectBuilder;
        }


        public static MultiSelectBuilder VnrMultiSelectFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, MultiSelectBuilderInfo builderInfo)
        {
               var dataSource = new Action<ReadOnlyDataSourceBuilder>(d =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.Url))
                {
                    d.Read(read => read.Url(builderInfo.Url).Type(HttpVerbs.Post).Data(builderInfo.ReadData)).ServerFiltering(builderInfo.ServerFiltering);
                }
                else if (!string.IsNullOrWhiteSpace(builderInfo.ActionName))
                {
                    d.Read(builderInfo.ActionName, builderInfo.Controller).ServerFiltering(builderInfo.ServerFiltering);
                }
            });

               var multiSelectBuilder = helper.Kendo().MultiSelectFor(expression)
                .Enable(builderInfo.Enable)
                .Filter(builderInfo.FilterType)
                .MinLength(builderInfo.MinLength)
                .DataSource(dataSource).ValuePrimitive(true)
                .AutoBind(builderInfo.Autobind)
                .Height(builderInfo.Height)
                .HtmlAttributes(new { style = "width:" + builderInfo.Width + "px;" })
                .MaxSelectedItems(builderInfo.MaxSelectedItems);

            if (builderInfo.BindTo!=null)
            {
                multiSelectBuilder.BindTo(builderInfo.BindTo);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.EventDataBound))
            {
                multiSelectBuilder.Events(e => e.DataBound(builderInfo.EventDataBound));
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.EventChange))
            {
                multiSelectBuilder.Events(e => e.Change(builderInfo.EventChange));
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.EventOpen))
            {
                multiSelectBuilder.Events(e => e.Open(builderInfo.EventOpen));
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.EventSelect))
            {
                multiSelectBuilder.Events(e => e.Select(builderInfo.EventSelect));
            }
            if (builderInfo.Value != null)
            {
                multiSelectBuilder.Value(builderInfo.Value);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.Placeholder))
            {
                multiSelectBuilder.Placeholder(builderInfo.Placeholder);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                multiSelectBuilder.Name(builderInfo.Name);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.DataTextField))
            {
                multiSelectBuilder.DataTextField(builderInfo.DataTextField);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.DataValueField))
            {
                multiSelectBuilder.DataValueField(builderInfo.DataValueField);
            }

            return multiSelectBuilder;
        }

    }

    #region MultiSelectInformation
    public class MultiSelectBuilderInfo : VnrBaseBuilderInfo
    {
        private bool _enable = true;
        private bool _autobind = false;
        private int _width = 200;
        private int _height = 200;
        private string _placeholder = ConstantDisplay.PlaceHolder.TranslateString();
        private string _dataValueField = string.Empty;
        private string _dataTextField = string.Empty;
        private int _maxSelectedItems = int.MaxValue;
        private int _minLength = 1;
        private bool serverFiltering = true;
        private FilterType _filterType = FilterType.Contains;

        public string ReadData { get; set; }
        public override string Name { get; set; }
        public string EventDataBound { get; set; }
        public string EventChange { get; set; }
        public string EventOpen { get; set; }
        public string EventSelect { get; set; }
        public bool ServerFiltering
            {
                get { return serverFiltering; }
                set { serverFiltering = value; }
            }
            public bool Autobind
            {
                get { return _autobind; }
                set { _autobind = value; }
            }
            /// <summary>
            /// Độ rộng của MultiSelect
            /// </summary>
            public int Width
            {
                get { return _width; }
                set { _width = value; }
            }

            /// <summary>
            /// Chiều cao của MultiSelect
            /// </summary>
            public int Height
            {
                get { return _height; }
                set { _height = value; }
            }
            /// <summary>
            /// Tắt/Mở MultiSelect, mặc định là True
            /// </summary>
            public bool Enable
            {
                get { return _enable; }
                set { _enable = value; }
            }
            /// <summary>
            /// Phần mội dung chỉ dẫn hiển thị trong MultiSelect khi chưa focus vào
            /// </summary>
            public int MaxSelectedItems
            {
                get { return _maxSelectedItems; }
                set { _maxSelectedItems = value; }
            }

            /// <summary>
            /// The minimum number of characters the user must type before a search is performed. Set to higher value than 1 if the search could match a lot of 
            /// </summary>
            public int MinLength
            {
                get { return _minLength; }
                set { _minLength = value; }
            }

            /// <summary>
            /// Phần mội dung chỉ dẫn hiển thị trong MultiSelect khi chưa focus vào
            /// </summary>
            public string Placeholder
            {
                get { return _placeholder; }
                set { _placeholder = value; }
            }

            /// <summary>
            /// Kiểu lọc dữ liệu trên MultiSelect, mặc định là Contains
            /// </summary>
            public FilterType FilterType
            {
                get { return _filterType; }
                set { _filterType = value; }
            }

            /// <summary>
            /// Action xử lý lấy dữ liệu cho MultiSelect
            /// </summary>
            public string ActionName { get; set; }

            /// <summary>
            /// Nếu không dùng ActionName và Controller có thể sử dụng Url để lấy dữ liệu
            /// </summary>
            public string Url { get; set; }

            /// <summary>
            /// Truyền giá trị mặc định khi gọi MultiSelect
            /// </summary>
            //public string[] Value { get; set; }
            public IEnumerable Value { get; set; }

            /// <summary>
            /// Phần dữ liệu để xử lý viết code
            /// </summary>
            public string DataValueField
            {
                get { return _dataValueField; }
                set { _dataValueField = value; }
            }

            /// <summary>
            /// Phần dữ liệu hiển thị lên cho người dùng thấy
            /// </summary>
            public string DataTextField
            {
                get { return _dataTextField; }
                set { _dataTextField = value; }
            }

            /// <summary>
            /// Nếu không dùng các gọi đến server để lấy dữ liệu trực tiếp có thể Bind dữ liệu từ 1 IList SelectListItem
            /// Thường sử dụng load các Enum
            /// </summary>
            public IList<SelectListItem> BindTo { get; set; }

        }
    #endregion 
}
