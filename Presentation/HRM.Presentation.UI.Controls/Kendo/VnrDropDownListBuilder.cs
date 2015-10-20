using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public static class VnrDropDownListBuilder
    {
        #region Vnr DropDownList Builder
        
        public static DropDownListBuilder VnrDropDownList<TModel>(this HtmlHelper<TModel> helper, DropDownListBuilderInfo builderInfo)
        {
            if (!string.IsNullOrEmpty(builderInfo.Enum))
            {
                builderInfo.Data = "FilterDataEnumDropdownList('" + builderInfo.Enum + "')";
            }
            var dataSource = new Action<ReadOnlyDataSourceBuilder>(d =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.Url))
                {
                    if (!string.IsNullOrWhiteSpace(builderInfo.Data))
                    {
                        d.Read(read => read.Url(builderInfo.Url).Type(HttpVerbs.Post).Data(builderInfo.Data)).ServerFiltering(builderInfo.ServerFilter);
                    }
                    else
                    {
                        d.Read(read => read.Url(builderInfo.Url).Type(HttpVerbs.Post));
                    }
                }
                else if (!string.IsNullOrWhiteSpace(builderInfo.ActionName) && !string.IsNullOrWhiteSpace(builderInfo.Controller))
                {
                    d.Read(builderInfo.ActionName, builderInfo.Controller);
                }
            });

            var events = new Action<DropDownListEventBuilder>(e =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.EventChange))
                {
                    e.Change(builderInfo.EventChange);
                }
                if (!string.IsNullOrWhiteSpace(builderInfo.EventSelect))
                {
                    e.Select(builderInfo.EventSelect);
                }
                if (!string.IsNullOrWhiteSpace(builderInfo.EventDataBound))
                {
                    e.DataBound(builderInfo.EventDataBound);
                }
            });



            var dropDownListBuilder = helper.Kendo().DropDownList()
                .OptionLabel(builderInfo.OptionLabel)
                .Events(events)
                .Enable(builderInfo.Enable)
                .AutoBind(builderInfo.Autobind)
                .DataSource(dataSource);
               
            if (!string.IsNullOrWhiteSpace(builderInfo.DataTextField))
            {
                dropDownListBuilder.DataTextField(builderInfo.DataTextField);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.DataValueField))
            {
                dropDownListBuilder.DataValueField(builderInfo.DataValueField);
            }
            if (builderInfo.BindTo != null)
            {
                dropDownListBuilder.BindTo(builderInfo.BindTo);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
            {
                dropDownListBuilder.HtmlAttributes(new { style = builderInfo.StyleCss });
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                dropDownListBuilder.Name(builderInfo.Name);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Value))
            {
                dropDownListBuilder.Value(builderInfo.Value);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.CascadeFrom))
            {
                dropDownListBuilder.CascadeFrom(builderInfo.CascadeFrom);
            }
            if (builderInfo.ShowButtonCreate)
            {
                dropDownListBuilder.ValueTemplate("<span style='float:left;max-width:50px;'>#: data." + builderInfo.DataTextField + " #</span><button class='k-button' style='width: 25px;float:right;border-radius:0px;margin: 0;margin-top: -2px;border-color: !important;padding-top: 0px;height: 24px;'  onclick=\"" + builderInfo.EvenClickButtonCreate + "('" + builderInfo.Name + "')\" ><img src=\"../Content/images/icons/add-new-icon.png\" alt=\"icon\" class=\"k-image\"></button>");
            }
            return dropDownListBuilder;
        }
        /// <summary>
        /// VnrDropDownListFor sử dụng trong trường hợp load dữ liệu tự động theo Model
        /// Trong trường hợp không cần thiết thì không cần gán Name vì Name đã được xử lý tự động sinh
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="builderInfo"></param>
        /// <returns></returns>
        public static DropDownListBuilder VnrDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, DropDownListBuilderInfo builderInfo)
        {
            if (!string.IsNullOrEmpty(builderInfo.Enum))
            {
                builderInfo.Data = "FilterDataEnumDropdownList('" + builderInfo.Enum + "')";
            }
            var dataSource = new Action<ReadOnlyDataSourceBuilder>(d =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.Url))
                {
                    if (!string.IsNullOrWhiteSpace(builderInfo.Data))
                    {
                        d.Read(read => read.Url(builderInfo.Url).Type(HttpVerbs.Post).Data(builderInfo.Data)).ServerFiltering(builderInfo.ServerFilter);
                    }
                    else
                    {
                        d.Read(read => read.Url(builderInfo.Url).Type(HttpVerbs.Post));
                    }
                }
                else if (!string.IsNullOrWhiteSpace(builderInfo.ActionName) && !string.IsNullOrWhiteSpace(builderInfo.Controller))
                {
                    d.Read(builderInfo.ActionName, builderInfo.Controller);
                }
            });

            var events = new Action<DropDownListEventBuilder>(e =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.EventChange))
                {
                    e.Change(builderInfo.EventChange);
                }
                if (!string.IsNullOrWhiteSpace(builderInfo.EventSelect))
                {
                    e.Select(builderInfo.EventSelect);
                }
                if (!string.IsNullOrWhiteSpace(builderInfo.EventDataBound))
                {
                    e.DataBound(builderInfo.EventDataBound);
                }
            });

            var dropDownListBuilder = helper.Kendo().DropDownListFor(expression)
                 .OptionLabel(builderInfo.OptionLabel)
                 .Events(events)
                 .Enable(builderInfo.Enable)
                 .AutoBind(builderInfo.Autobind)
                 .DataSource(dataSource);
            if (!string.IsNullOrWhiteSpace(builderInfo.DataTextField))
            {
                dropDownListBuilder.DataTextField(builderInfo.DataTextField);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.DataValueField))
            {
                dropDownListBuilder.DataValueField(builderInfo.DataValueField);
            }
            if (builderInfo.BindTo != null)
            {
                dropDownListBuilder.BindTo(builderInfo.BindTo);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
            {
                dropDownListBuilder.HtmlAttributes(new { style = builderInfo.StyleCss });
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                dropDownListBuilder.Name(builderInfo.Name);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Value))
            {
                dropDownListBuilder.Value(builderInfo.Value);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.CascadeFrom))
            {
                dropDownListBuilder.CascadeFrom(builderInfo.CascadeFrom);
            }
            if (builderInfo.ShowButtonCreate)
            {
                dropDownListBuilder.ValueTemplate("<span style='float:left;max-width:50px;'>#: data." + builderInfo.DataTextField + " #</span><button class='k-button' style='width: 25px;float:right;border-radius:0px;margin: 0;margin-top: -2px;border-color: !important;padding-top: 0px;height: 24px;'  onclick=\"" + builderInfo.EvenClickButtonCreate + "('" + builderInfo.Name + "')\" ><img src=\"../Content/images/icons/add-new-icon.png\" alt=\"icon\" class=\"k-image\"></button>");
            }
            return dropDownListBuilder;
        }
        #endregion
    }

    #region DropDownList Information
    public class DropDownListBuilderInfo : VnrBaseBuilderInfo
    {
        #region Constructors
        public DropDownListBuilderInfo()
        {
            Enable = true;
            OptionLabel = ConstantDisplay.PlaceHolder.TranslateString();
            ServerFilter = false;
            Autobind = true;
            StyleCss = "width:200px;";
            ShowButtonCreate = false;
            //ShowButtonEdit = false;
            EvenClickButtonReload = "ReloadDropDownList";
            Enum = string.Empty;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Tắt/Mở control Default: true
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// Hiển thị text mờ trong control Default: EN: "Select Option..." ; VI:"Vui lòng chọn...";
        /// </summary>
        public string OptionLabel { get; set; }
        /// <summary>
        /// Field sẽ dùng làm giá trị để xử lý khi chọn dữ liệu
        /// </summary>
        public string DataValueField { get; set; }
        /// <summary>
        /// Field dữ liệu hiển thị cho người dùng thấy
        /// </summary>
        public string DataTextField { get; set; }
        /// <summary>
        /// Dữ liệu hiễn thị sẻ lọc theo dữ liệu của control được chọn
        /// </summary>
        public string CascadeFrom { get; set; }
        /// <summary>
        /// Lọc dữ liệu từ server mỗi khi filter Default: false
        /// </summary>
        public bool ServerFilter { get; set; }
        /// <summary>
        /// Tự động load dữ liệu khi vào trang Default: true
        /// </summary>
        public bool Autobind { get; set; }
        /// <summary>
        /// Tên hàm chữ dự liệu sẽ mang theo mỗi khi lấy dữ liệu cho Dropdownlist
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// Tên hàm xử lý sự kiện change Dopdownlist
        /// </summary>
        public string EventChange { get; set; }
        /// <summary>
        /// Tên hàm xử lý sự kiện select Dropdownlist
        /// </summary>
        public string EventSelect { get; set; }
        /// <summary>
        /// Tên hàm xử lý xự kiện khi dự liệu được load lên
        /// </summary>
        public string EventDataBound { get; set; }
        /// <summary>
        /// Action xử lý lấy dữ liệu cho DropDownList
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// Nếu không dùng ActionName và Controller có thể sử dụng Url để lấy dữ liệu
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Truyền giá trị mặc định khi gọi DropDownList
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Tùy chỉnh lại định dạng cho DropDownList như: độ rộng, chiều cao....
        /// </summary>
        public string StyleCss { get; set; }
        /// <summary>
        /// Hiển thị nút tao mới (default=false)
        /// </summary>
        public bool ShowButtonCreate { get; set; }
        /// <summary>
        /// Hiển thị nút chỉnh sửa (default=false)
        /// </summary>
        //public bool ShowButtonEdit { get; set; }
        /// <summary>
        /// Even click button edit
        /// </summary>
        //public string EvenClickButtonEdit { get; set; }
        /// <summary>
        /// Even click button create
        /// </summary>
        public string EvenClickButtonCreate { get; set; }
        /// <summary>
        /// Even click button Reload
        /// </summary>
        public string EvenClickButtonReload { get; set; }
        /// <summary>
        /// Nếu không dùng các gọi đến server để lấy dữ liệu trực tiếp có thể Bind dữ liệu từ 1 IList SelectListItem
        /// Thường sử dụng load các Enum
        /// </summary>
        public IList<SelectListItem> BindTo { get; set; }
        /// <summary>
        /// Giá trị Enum muốn đưa lên server
        /// </summary>
        public string Enum { get; set; }
        /// <summary>
        /// Xây dựng name lại cho DropDownList, mặc định name sẻ là tên của field đang sử dụng
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
