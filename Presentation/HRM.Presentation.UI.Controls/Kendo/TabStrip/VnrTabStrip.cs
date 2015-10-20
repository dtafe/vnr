using HRM.Presentation.UI.Controls.Kendo.ControlBase;
using System;
using Kendo.Mvc.UI.Fluent;
using System.Collections.Generic;
using Kendo.Mvc.UI;
using System.Web.Mvc;


namespace HRM.Presentation.UI.Controls.Kendo.TabStrip
{
    #region VnrTabStripBuilder

    public static class VnrTabStripBuilder
    {
        public static TabStripBuilder TabStripControl(this HtmlHelper helper, TabStripInfomation tab) 
        {

            Action<TabStripItemFactory> tabfact = new Action<TabStripItemFactory>(t =>
            {
                foreach (var s in tab.TabItems)
                {
                    //var str = "/" + s.Value.ControllerName + "/" + s.Value.ActionName;

                    //if (!string.IsNullOrEmpty(s.Value.ControllerName) || !string.IsNullOrEmpty(s.Value.ActionName))
                    if (!string.IsNullOrEmpty(s.Value.LoadContentFrom))
                    {
                        if (s.Value.Parameter!=null)
                        {
                            t.Add()
                            .Text(s.Key)
                                //.LoadContentFrom(s.Value.UrlService + "/" + s.Value.ActionName + "/" + s.Value.ControllerName + "/" + s.Value.Id)
                            .LoadContentFrom(s.Value.LoadContentFrom)
                            .Selected(s.Value.Selected);
                        }
                        t.Add()
                            .Text(s.Key)
                            //.LoadContentFrom(s.Value.UrlService + "/" + s.Value.ActionName + "/" + s.Value.ControllerName + "/" + s.Value.Id)
                            .LoadContentFrom(s.Value.LoadContentFrom)
                            .Selected(s.Value.Selected);
                    }
                    else
                    {
                        t.Add()
                           .Text(s.Key)
                           .Content(s.Value.Content)
                           .Selected(s.Value.Selected);
                    }
                }
               
            });

            Action<TabStripEventBuilder> tabe = new Action<TabStripEventBuilder>(e =>
            {
                if (!string.IsNullOrWhiteSpace(tab.Activate))
                {
                    e.Activate(tab.Activate);
                }
                if (!string.IsNullOrWhiteSpace(tab.ContentLoad))
                {
                    e.ContentLoad(tab.ContentLoad);
                } 
                if (!string.IsNullOrWhiteSpace(tab.Select))
                {
                    e.Select(tab.Select);
                }

            });

            return helper.Kendo().TabStrip()
                .Name(tab.Name)
                .Items(tabfact)
                .Events(tabe)
                ;
        }
    }
    #endregion

    #region TabStripInfomation

    public class ItemsInformation
    {
        private bool _selected;
        //private string _controllerName = string.Empty;
        //private string _actionName = string.Empty;
        private string _content = string.Empty;
        //private string _urlService = string.Empty;
        //private int _id;


        /// <summary>
        /// Quy định thuộc tính Selected là True hay False
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        public object Parameter { get; set; }

        ///// <summary>
        ///// Controller name
        ///// </summary>
        //public string ControllerName
        //{
        //    get { return _controllerName; }
        //    set { _controllerName = value; }
        //}

        ///// <summary>
        ///// Action name
        ///// </summary>
        //public string ActionName
        //{
        //    get { return _actionName; }
        //    set { _actionName = value; }
        //}
        /// <summary>
        /// Load content khi không sử dụng action
        /// </summary>
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }     
        
        /// <summary>
        /// Load content
        /// </summary>
        public string LoadContentFrom { get; set; }

        ///// <summary>
        ///// Đường dẫn đến service
        ///// </summary>
        //public string UrlService
        //{
        //    get { return _urlService; }
        //    set { _urlService = value; }
        //}

        ///// <summary>
        ///// Id của đối tượng muốn lấy dữ liệu tại service
        ///// </summary>
        //public int Id
        //{
        //    get { return _id; }
        //    set { _id = value; }
        //}

    }
    public class TabStripInfomation : VnrPropertiesBase
    {
        #region Properties

        Dictionary<string, ItemsInformation> _tabItems;

        /// <summary>
        /// Thông tin load dữ liệu của tabItems
        /// </summary>
        public Dictionary<string, ItemsInformation> TabItems
        {
            get
            {
                if (_tabItems == null)
                {
                    _tabItems = new Dictionary<string, ItemsInformation>();
                }
                return _tabItems;
            }
            internal set
            {
                _tabItems = value;
            }
        }
        
        #endregion

        #region Methods

        #endregion

        #region Event
        private string _activate = string.Empty;
        public string Select { get; set; }
        public string ContentLoad { get; set; }

        /// <summary>
        /// Sự kiện khi chọn tab
        /// </summary>
        public string Activate
        {
            get { return _activate; }
            set { _activate = value; }
        }
        #endregion
    }
    #endregion
}
