using System;
using System.Collections.Generic;

namespace HRM.Presentation.UI.Controls.Kendo.ListView
{
    #region ListViewBuilder
    class VnrListView
    {
        
    }
    #endregion
   
    #region ListViewInformation
    public class ListViewInformation
    {
        #region Properties
        private string listName = string.Empty;
        private string controllerName = string.Empty;
        private string dataActionName = string.Empty;
        private Type objectType;
        private string url = string.Empty;
        private int pageSize = 20;
        private string[] valueFields;
        private Boolean autoBind = true;
        private string selectable = string.Empty;

        Dictionary<string, string> groupFields;
        Dictionary<string, string> formatFields;
        Dictionary<string, int> sizeFields;


        /// <summary>
        /// Tên list control được tạo ra.
        /// </summary>
        public string ListName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(listName))
                {
                    listName = DateTime.Now.ToOADate().ToString();
                }
                return listName;
            }
            set { listName = value; }
        }
        /// <summary>
        /// Tên controller dữ liệu.
        /// </summary>
        public string ControllerName
        {
            get { return controllerName; }
            set { controllerName = value; }
        }
        /// <summary>
        /// Tên method load dữ liệu.
        /// </summary>
        public string DataActionName
        {
            get { return dataActionName; }
            set { dataActionName = value; }
        }
        /// <summary>
        /// Kieu du lieu.
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
        /// Số dòng hiển thị trên list
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
        /// <summary>
        /// Danh sách field dữ liệu hiển thị trên lưới.
        /// </summary>
        public string[] ValueFields
        {
            get { return valueFields; }
            set { valueFields = value; }
        }

        /// <summary>
        /// Có tự động load dữ liệu hay không.
        /// </summary>
        public Boolean AutoBind
        {
            get { return autoBind; }
            set { autoBind = value; }
        }
        /// <summary>
        /// select dữ liệu trên lưới (chọn 1 or nhiều dòng cùng lúc)
        /// </summary>
        public string Selectable
        {
            get { return selectable; }
            set { selectable = value; }
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
        /// <summary>
        /// Danh sách chuỗi format theo valuefield.
        /// </summary>
        public Dictionary<string, string> FormatFields
        {
            get
            {
                if (formatFields == null)
                {
                    formatFields = new Dictionary<string, string>();
                }

                return formatFields;
            }
            internal set
            {
                formatFields = value;
            }
        }
        /// <summary>
        /// Danh sách độ rộng theo valuefield.
        /// </summary>
        public Dictionary<string, int> SizeFields
        {
            get
            {
                if (sizeFields == null)
                {
                    sizeFields = new Dictionary<string, int>();
                }

                return sizeFields;
            }
            internal set
            {
                sizeFields = value;
            }
        }
        #endregion


        #region Method
       
        #endregion


        #region Events

        #endregion
    }
    #endregion
}
