using System.Web.Mvc;
using HRM.Presentation.UI.Controls.Kendo.ControlBase;
using System;
using System.Collections.Generic;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

namespace HRM.Presentation.UI.Controls.Kendo.MultiSelect
{
    #region VnrMultiSelectBuilder
    public static class VnrMultiSelect
    {
        public static MultiSelectBuilder MultiSelectListControl(this HtmlHelper helper, MultiSelectInformation multi)
        {
            Action<ReadOnlyDataSourceBuilder> dataSource = new Action<ReadOnlyDataSourceBuilder>(d =>
            {
                if (!string.IsNullOrWhiteSpace(multi.Url))
                {
                    d.Read(read => read.Url(multi.Url).Type(HttpVerbs.Post));
                }
                else if (!string.IsNullOrWhiteSpace(multi.DataActionName))
                {
                    d.Read(multi.DataActionName, multi.ControllerName);
                }
            });
            Action<MultiSelectEventBuilder> multie = new Action<MultiSelectEventBuilder>(e =>
            {
                if (!string.IsNullOrWhiteSpace(multi.Change))
                {
                    e.Change(multi.Change);
                }
                if (!string.IsNullOrWhiteSpace(multi.Close))
                {
                    e.Close(multi.Close);
                }
                if (!string.IsNullOrWhiteSpace(multi.Open))
                {
                    e.Open(multi.Open);
                }
                if (!string.IsNullOrWhiteSpace(multi.Select))
                {
                    e.Select(multi.Select);
                }

            });


            return helper.Kendo().MultiSelect()
                .Name(multi.Name)
                .Filter(FilterType.Contains)
                //.Enable(multi.Enable)
                //.BindTo(multi.BindTo)
                .HeaderTemplate(multi.HeaderTemplate)
                .Placeholder(multi.Placeholder)
                //.Value(multi.Value)
                .DataTextField(multi.DataTextField)
                .DataValueField(multi.DataValueField)
                .DataSource(dataSource).ValuePrimitive(true)
                .Height(multi.Height)
                .Events(multie)
                .Enable(true)
                .MaxSelectedItems(multi.MaxSelectedItems)
                .HtmlAttributes(new { style = "width:" + multi.Width + "px;" })
                .Value(new string[] { multi.ValueMultiselect });
        }
    }
    #endregion

    #region MultiSelectInformation
    public class MultiSelectInformation : VnrPropertiesBase
    {

        #region Properties

        private string _url = string.Empty;
        private string _placeholder = string.Empty;
        private Array _value;
        private string _orderField = string.Empty;
        private string _filterField = string.Empty;
        private string _dataActionName = string.Empty;
        private string _controllerName = string.Empty;
        private string _dataTextField = string.Empty;
        private string _dataValueField = string.Empty;
        private string _headerTemplate = string.Empty;
        private int _maxSelectedItems;
        private int _width = 300;
        private int _height = 200;
        private string _valueMultiselect = string.Empty;



        private List<SelectListItem> _bindTo;

        
        /// <summary>
        ///     overite property width
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        ///     overite property width
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        /// <summary>
        ///     Value multiselect.
        /// </summary>
        public string ValueMultiselect
        {
            get { return _valueMultiselect; }
            set { _valueMultiselect = value; }
        }
        /// <summary>
        ///      C Url Controller.
        /// </summary>
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        /// <summary>
        ///     Text mặc định hiển thị trên Textbox
        /// </summary>
        public string Placeholder
        {
            get { return _placeholder; }
            set { _placeholder = value; }
        }
        /// <summary>
        /// Danh sách field dữ liệu hiển thị trên textbox.
        /// </summary>
        public Array Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        ///     Cột sắp xếp dữ liệu.
        /// </summary>
        public string OrderField
        {
            get { return _orderField; }
            set { _orderField = value; }
        }
        /// <summary>
        /// filter dữ liệu
        /// </summary>
        public string FilterField
        {
            get { return _filterField; }
            set { _filterField = value; }
        }

        /// <summary>
        /// Tên controller dữ liệu.
        /// </summary>
        public string ControllerName
        {
            get { return _controllerName; }
            set { _controllerName = value; }
        }

        /// <summary>
        /// Tên method load dữ liệu.
        /// </summary>
        public string DataActionName
        {
            get { return _dataActionName; }
            set { _dataActionName = value; }
        }

        /// <summary>
        /// Tên field hiển thị cho người dùng xem.
        /// </summary>
        public string DataTextField
        {
            get { return _dataTextField; }
            set { _dataTextField = value; }
        }

        /// <summary>
        /// Tên field lấy giá trị để xử lý.
        /// </summary>
        public string DataValueField
        {
            get { return _dataValueField; }
            set { _dataValueField = value; }
        }

        /// <summary>
        /// Header của dữ liệu khi xỗ ra
        /// </summary>
        public string HeaderTemplate
        {
            get { return _headerTemplate; }
            set { _headerTemplate = value; }
        }

        /// <summary>
        /// Số lượng item tối đa được chọn
        /// </summary>
        public int MaxSelectedItems
        {
            get { return _maxSelectedItems; }
            set { _maxSelectedItems = value; }
        }

        /// <summary>
        /// Nguồn dữ liệu để sử dụng khi không dùng data source
        /// </summary>
        public List<SelectListItem> BindTo
        {
            get { return _bindTo; }
            set { _bindTo = value; }
        }
        #endregion

        #region Methods

        #endregion

        #region Events
        private string _change = string.Empty;
        private string _close = string.Empty;
        private string _open = string.Empty;
        private string _select = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Change
        {
            get { return _change; }
            set { _change = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Close
        {
            get { return _close; }
            set { _close = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Open
        {
            get { return _open; }
            set { _open = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Select
        {
            get { return _select; }
            set { _select = value; }
        }
        #endregion


    }
    #endregion
}
