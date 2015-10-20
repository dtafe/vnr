using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public static class VnrComboBoxBuilder
    {
        #region Vnr ComboBox Builder

        /// <summary>
        /// VnrComboBox sử dụng như các Combobox thông thường khác
        /// Trong trường hợp không cần thiết thì không cần gán Name vì Name đã được xử lý tự động sinh
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="helper"></param>
        /// <param name="builderInfo"></param>
        /// <returns></returns>
        public static ComboBoxBuilder VnrComboBox<TModel>(this HtmlHelper<TModel> helper, ComboBoxBuilderInfo builderInfo)
        {
            var dataSource = new Action<ReadOnlyDataSourceBuilder>(d =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.Url))
                {
                    d.Read(read => 
                        {
                            var readItem = read.Url(builderInfo.Url).Type(HttpVerbs.Post);
                             if (!string.IsNullOrWhiteSpace(builderInfo.Data))
                            {
                                readItem.Data(builderInfo.Data);
                            }
                        }).ServerFiltering(builderInfo.ServerFilter);
                   
                }
                else if (!string.IsNullOrWhiteSpace(builderInfo.ActionName) && !string.IsNullOrWhiteSpace(builderInfo.Controller))
                {
                    d.Read(builderInfo.ActionName, builderInfo.Controller);
                }
            });
            var events = new Action<ComboBoxEventBuilder>(e =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.EventChange))
                {
                    e.Change(builderInfo.EventChange);
                }
                if (!string.IsNullOrWhiteSpace(builderInfo.EventDataBound))
                {
                    e.DataBound(builderInfo.EventDataBound);
                }
                if (!string.IsNullOrWhiteSpace(builderInfo.EventOpen))
                {
                    e.Open(builderInfo.EventOpen);
                }
            });

            var comboBoxBuilder = helper.Kendo().ComboBox()
                .Enable(builderInfo.Enable)
                .AutoBind(builderInfo.Autobind)
                .Filter(builderInfo.FilterType)
                .DataTextField(builderInfo.DataTextField)
                .DataValueField(builderInfo.DataValueField)
                .DataSource(dataSource)
                .Events(events)
                .SelectedIndex(builderInfo.Index);

            if (builderInfo.BindTo != null)
            {
                comboBoxBuilder.BindTo(builderInfo.BindTo);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.CascadeFrom))
            {
                comboBoxBuilder.CascadeFrom(builderInfo.CascadeFrom);
            } 

            if (!string.IsNullOrWhiteSpace(builderInfo.Placeholder))
            {
                comboBoxBuilder.Placeholder(builderInfo.Placeholder);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
            {
                comboBoxBuilder.HtmlAttributes(new { style = builderInfo.StyleCss });

            }
            else if (builderInfo.HtmlAttributes != null)
            {
                comboBoxBuilder.HtmlAttributes(builderInfo.HtmlAttributes);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                comboBoxBuilder.Name(builderInfo.Name);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.Value))
            {
                comboBoxBuilder.Value(builderInfo.Value);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Template))
            {
                comboBoxBuilder.Template(builderInfo.Template);
            }
            return comboBoxBuilder;
        }


        /// <summary>
        /// VnrComboBoxFor sử dụng trong trường hợp load dữ liệu tự động theo Model
        /// Trong trường hợp không cần thiết thì không cần gán Name vì Name đã được xử lý tự động sinh
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="builderInfo"></param>
        /// <returns></returns>
        public static ComboBoxBuilder VnrComboBoxFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, ComboBoxBuilderInfo builderInfo)
        {
            var dataSource = new Action<ReadOnlyDataSourceBuilder>(d =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.Url))
                {
                    d.Read(read =>
                    {
                        var readItem = read.Url(builderInfo.Url).Type(HttpVerbs.Post);
                        if (!string.IsNullOrWhiteSpace(builderInfo.Data))
                        {
                            readItem.Data(builderInfo.Data);
                        }
                    }).ServerFiltering(builderInfo.ServerFilter);
                }
                else if (!string.IsNullOrWhiteSpace(builderInfo.ActionName) && !string.IsNullOrWhiteSpace(builderInfo.Controller))
                {
                    d.Read(builderInfo.ActionName, builderInfo.Controller);
                }
            });

            var events = new Action<ComboBoxEventBuilder>(e =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.EventChange))
                {
                    e.Change(builderInfo.EventChange);
                }
                if (!string.IsNullOrWhiteSpace(builderInfo.EventDataBound))
                {
                    e.DataBound(builderInfo.EventDataBound);
                }
                if (!string.IsNullOrWhiteSpace(builderInfo.EventOpen))
                {
                    e.Open(builderInfo.EventOpen);
                }
            });
            var comboBoxBuilder = helper.Kendo().ComboBoxFor(expression)
                .Enable(builderInfo.Enable)
                .AutoBind(builderInfo.Autobind)
                .Filter(builderInfo.FilterType)
                .DataTextField(builderInfo.DataTextField)
                .DataValueField(builderInfo.DataValueField)
                .DataSource(dataSource)
                .Events(events)
                .SelectedIndex(builderInfo.Index);

                if (builderInfo.BindTo!=null)
                {
                    comboBoxBuilder.BindTo(builderInfo.BindTo);
                }
                if (!string.IsNullOrWhiteSpace(builderInfo.CascadeFrom))
                {
                    comboBoxBuilder.CascadeFrom(builderInfo.CascadeFrom);
                } 

                if (!string.IsNullOrWhiteSpace(builderInfo.Placeholder))
                {
                    comboBoxBuilder.Placeholder(builderInfo.Placeholder);

                }

                if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
                {
                    comboBoxBuilder.HtmlAttributes(new { style = builderInfo.StyleCss });

                }
                if (builderInfo.HtmlAttributes != null)
                {
                    comboBoxBuilder.HtmlAttributes(builderInfo.HtmlAttributes);
                }
                if (!string.IsNullOrWhiteSpace(builderInfo.Name))
                {
                   comboBoxBuilder.Name(builderInfo.Name);
                }

                if (!string.IsNullOrWhiteSpace(builderInfo.Value))
                {
                    comboBoxBuilder.Value(builderInfo.Value);
                }

                if (!string.IsNullOrWhiteSpace(builderInfo.Template))
                {
                    comboBoxBuilder.Template(builderInfo.Template);
                }
            return comboBoxBuilder;
        }
        #endregion

        
    }


    #region ComboBox Information

    public class ComboBoxBuilderInfo : VnrBaseBuilderInfo
    {
        public ComboBoxBuilderInfo()
        {
            Index = -1;
        }
        #region Properties

        private bool _enable = true;
        private bool _serverFilter = false;
        private string _placeholder = string.Empty;
        private string _dataValueField = string.Empty;
        private string _dataTextField = string.Empty;
        private bool _autobind = false;
        private string _cascadeFrom = string.Empty;
        private FilterType _filterType = FilterType.Contains;

        public int Index { get; set; }
        public string EventChange { get; set; }
        public string EventDataBound { get; set; }
        public string EventOpen { get; set; }

        public string Data { get; set; }
        public bool ServerFilter
        {
            get { return _serverFilter; }
            set { _serverFilter = value; }
        }
        public bool Autobind
        {
            get { return _autobind; }
            set { _autobind = value; }
        }
        /// <summary>
        /// Action xử lý lấy dữ liệu cho Combobox
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Nếu không dùng ActionName và Controller có thể sử dụng Url để lấy dữ liệu
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Truyền giá trị mặc định khi gọi Combobox
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Tùy chỉnh lại định dạng cho Combobox như: độ rộng, chiều cao....
        /// </summary>
        public string StyleCss { get; set; }

        public string Template { get; set; }

        /// <summary>
        /// Nếu không dùng các gọi đến server để lấy dữ liệu trực tiếp có thể Bind dữ liệu từ 1 IList SelectListItem
        /// Thường sử dụng load các Enum
        /// </summary>
        public IList<SelectListItem> BindTo { get; set; }

        /// <summary>
        /// Tắt/Mở Combobox, mặc định là True
        /// </summary>
        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        /// <summary>
        /// Kiểu lọc dữ liệu trên Combobox, mặc định là Contains
        /// </summary>
        public FilterType FilterType
        {
            get { return _filterType; }
            set { _filterType = value; }
        }

        /// <summary>
        /// Phần mội dung chỉ dẫn hiển thị trong Combobox khi chưa focus vào
        /// </summary>
        public string Placeholder
        {
            get { return _placeholder; }
            set { _placeholder = value; }
        }

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
        /// Hiển thị dữ liệu theo một Combobox cha nào đó
        /// Vd: Quốc gia và tỉnh thành, chọn quốc gia sẻ hiển thị lên tỉnh thành của quốc gia đó
        /// </summary>
        public string CascadeFrom
        {
            get { return _cascadeFrom; }
            set { _cascadeFrom = value; }
        }

        public object HtmlAttributes { get; set; }
        /// <summary>
        /// Xây dựng name lại cho Combobox, mặc định name sẻ là tên của field đang sử dụng
        /// </summary>
        public override string Name { get; set; }

        #endregion

        #region Method

        #endregion

        #region Event

        #endregion
    }

    #endregion
}
