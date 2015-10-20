using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using HRM.Presentation.UI.Controls.Kendo.ControlBase;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

namespace HRM.Presentation.UI.Controls.Kendo.DropDownList
{
    #region VnrDropDownListBuilder
    public static class VnrDownListBuilder
    {
        public static DropDownListBuilder DropDownListControl<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, DropDownListInfomation ddl)
        {
            Action<ReadOnlyDataSourceBuilder> dataSource = new Action<ReadOnlyDataSourceBuilder>(d =>
            {
                if (!string.IsNullOrWhiteSpace(ddl.Url))
                {
                    d.Read(read => read.Url(ddl.Url).Type(HttpVerbs.Post));
                }
                else if (!string.IsNullOrWhiteSpace(ddl.DataActionName))
                {
                    d.Read(ddl.DataActionName, ddl.ControllerName);
                }
            });
            return helper.Kendo().DropDownListFor(expression)
                .Name(ddl.Name)
                .Enable(ddl.Enable)
                .OptionLabel(ddl.OptionLabel)
                .DataTextField(ddl.DataTextField)
                .DataValueField(ddl.DataValueField)
                .DataSource(dataSource)
                .HtmlAttributes(new {style="width:"+ddl.Width+"px;"})
                .Value(ddl.Value);
        }
    }
    #endregion

    #region DropDownListInfomation

    public class DropDownListInfomation : VnrPropertiesBase
    {
        #region Properties

        private string _dataActionName = string.Empty;
        private string _value = string.Empty;
        private string _controllerName = string.Empty;
        private string _dataTextField = string.Empty;
        private string _dataValueField = string.Empty;
        private string _optionLabel = string.Empty;
        private string _cascadeFrom = string.Empty;
        private string _url = string.Empty;
        private Type _objectType;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
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
        /// Text mặc định hiển thị trong DropDownList khi chưa chọn giá trị
        /// </summary>
        public string OptionLabel
        {
            get { return _optionLabel; }
            set { _optionLabel = value; }
        }

        /// <summary>
        /// Lọc dữ liệu hiển thị theo DropDownList cha dựa vào tên của cha
        /// </summary>
        public string CascadeFrom
        {
            get { return _cascadeFrom; }
            set { _cascadeFrom = value; }
        }

        /// <summary>
        /// Đường dẫn đến method xử lý tại service
        /// </summary>
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// Loại dự liệu hiển thị trên DropDownList.
        /// </summary>
        public Type ObjectType
        {
            get { return _objectType; }
            set { _objectType = value; }
        }

        #endregion

        #region Method

        #endregion

        #region Event

        #endregion
    }

    #endregion
}
