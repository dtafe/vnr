using HRM.Presentation.UI.Controls.Kendo.ControlBase;
using Kendo.Mvc.UI.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace HRM.Presentation.UI.Controls.Kendo.AutoComplete
{
    #region VnrAutoCompleteBuilder
    public static class VnrAutoCompleteControl
    {
        public static AutoCompleteBuilder VnrAutoComplete(this HtmlHelper helper, VnrAutoCompleteInformation autoCompleteInfo)
        {
            Action<ReadOnlyDataSourceBuilder> dataSource = new Action<ReadOnlyDataSourceBuilder>(d =>
            {
                if (!string.IsNullOrWhiteSpace(autoCompleteInfo.Url))
                {
                    d.Read(p => p.Url(autoCompleteInfo.Url).Type(HttpVerbs.Post));
                }
                else if (!string.IsNullOrWhiteSpace(autoCompleteInfo.DataActionName))
                {
                    d.Read(autoCompleteInfo.DataActionName, autoCompleteInfo.ControllerName);
                }
            });

            return helper.Kendo().AutoComplete()
                .Filter(FilterType.Contains)
                .Name(autoCompleteInfo.Name)
                //.HeaderTemplate(autoCompleteInfo.Headertemplate)
                .Placeholder(autoCompleteInfo.Placeholder)
                .Height(autoCompleteInfo.Height)
                .HtmlAttributes(new {style="width:"+autoCompleteInfo.Width+"px;" })
                .DataTextField(autoCompleteInfo.FilterField)
               .DataSource(dataSource)
               .Separator(autoCompleteInfo.Separator);
        }
    }
    #endregion

    #region VnrAutoCompleteInformation
   public class VnrAutoCompleteInformation : VnrPropertiesBase
    {

       #region Properties
        private Type objectType;
        private string url = string.Empty;
        private string placeholder = string.Empty;
        private string[] valueFields;
        private string filterField = string.Empty;
        private string dataActionName = string.Empty;
        private string controllerName = string.Empty;
        private string headertemplate = string.Empty;
        private string separator = string.Empty;

        Dictionary<string, string> displayFields;
        Dictionary<string, string> groupFields;


        /// <summary>
        ///     Kí tự cách nhau khi chọn nhiều ( Ví dụ abc, aaa)
        /// </summary>
        public string Separator
        {
            get { return separator; }
            set { separator = value; }
        }
        /// <summary>
        ///     Tên Controller
        /// </summary>
        public string ControllerName
        {
            get { return controllerName; }
            set { controllerName = value; }
        }
        /// <summary>
        /// Tên action của controller
        /// </summary>
        public string DataActionName
        {
            get { return dataActionName; }
            set { dataActionName = value; }
        }
        /// <summary>
        /// Kiểu dữ liệu
        /// </summary>
        public Type ObjectType
        {
            get { return objectType; }
            set { objectType = value; }
        }
        /// <summary>
        ///     Url Controller.
        /// </summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        /// <summary>
        ///     Text hiển thị mặc định trên textbox.
        /// </summary>
        public string Placeholder
        {
            get { return placeholder; }
            set { placeholder = value; }
        }
        /// <summary>
        /// Danh sách field dữ liệu hiển thị trên textbox.
        /// </summary>
        public string[] ValueFields
        {
            get { return valueFields; }
            set { valueFields = value; }
        }
        /// <summary>
        /// filter dữ liệu
        /// </summary>
        public string FilterField
        {
            get { return filterField; }
            set { filterField = value; }
        }


        /// <summary>
        /// Tên header hiển thị tương ứng theo valuefield.
        /// </summary>
        public string Headertemplate
        {
            get { return headertemplate; }
            set { headertemplate = value; }
        }

        /// <summary>
        /// Tên hiển thị tương ứng theo valuefield.
        /// </summary>
        public Dictionary<string, string> DisplayFields
        {
            get
            {
                if (displayFields == null)
                {
                    displayFields = new Dictionary<string, string>();
                }

                return displayFields;
            }
            internal set
            {
                displayFields = value;
            }
        }
        /// <summary>
        /// Danh sách field dữ liệu được group mặc định.
        /// </summary>
        public Dictionary<string, string> GroupFields
        {
            get
            {
                if (groupFields == null)
                {
                    groupFields = new Dictionary<string, string>();
                }

                return groupFields;
            }
            internal set
            {
                groupFields = value;
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
