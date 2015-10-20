using HRM.Presentation.UI.Controls.Kendo.ControlBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Presentation.UI.Controls.Kendo.TreeView
{

    #region VnrTreeViewBuilder
    public static class VnrTreeView
    {
    }
    #endregion

    #region TreeViewInfomation
    public class TreeViewInfomation : VnrPropertiesBase
    {
        #region Properties

        private Type objectType;
        private string[] animation;
        private bool autoBind;
        private bool checkboxes;
        private string[] dataImageUrlField;
        private string url = string.Empty;
        private string[] dataTextField;
        private string[] dataUrlField;
        private string[] messages;

        /// <summary>
        /// 
        /// </summary>
        public string[] Messages
        {
            get { return messages; }
            set { messages = value; }
        }

        /// <summary>
        /// Thiết lập trường dữ liệu chứa liên kết URL của mỗi item
        /// </summary>
        public string[] DataUrlField
        {
            get { return dataUrlField; }
            set { dataUrlField = value; }
        }

        /// <summary>
        /// Dử liệu hiển thị trên treeview
        /// </summary>
        public string[] DataTextField
        {
            get { return dataTextField; }
            set { dataTextField = value; }
        }

        /// <summary>
        /// datasource
        /// </summary>
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        /// <summary>
        /// Loại dự liệu hiển thị
        /// </summary>
        public Type ObjectType
        {
            get { return objectType; }
            set { objectType = value; }
        }

        /// <summary>
        /// Quy định hình ảnh cho mỗi item bên trong treeview
        /// </summary>
        public string[] DataImageUrlField
        {
            get { return dataImageUrlField; }
            set { dataImageUrlField = value; }
        }

        /// <summary>
        /// Có hiển thị checkbox trước mỗi item của treeview hay không?
        /// </summary>
        public bool Checkboxes
        {
            get { return checkboxes; }
            set { checkboxes = value; }
        }

        /// <summary>
        /// Mặc định có load dữ liệu hay không
        /// </summary>
        public bool AutoBind
        {
            get { return autoBind; }
            set { autoBind = value; }
        }

        /// <summary>
        /// @@
        /// </summary>
        public string[] Animation
        {
            get { return animation; }
            set { animation = value; }
        }
        

        #endregion

        #region Methods

        #endregion

        #region Events

        #endregion

    }
    #endregion

}
