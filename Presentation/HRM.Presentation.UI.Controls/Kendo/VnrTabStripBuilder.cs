using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

namespace HRM.Presentation.UI.Controls.Kendo
{
    #region VnrTabStripBuilder
    public static class VnrTabStripBuilder
    {
        public static TabStripBuilder VnrTabStrip(this HtmlHelper helper, TabStripBuilderInfo builderInfo)
        {
            var tabsFactory = new Action<TabStripItemFactory>(tab =>
            {
                foreach (var tabItem in builderInfo.TabItems)
                {
                    if (!string.IsNullOrEmpty(tabItem.Value.Controller) && !string.IsNullOrEmpty(tabItem.Value.Action))
                    {
                        if (tabItem.Value.Parameter!=null)
                        {
                            tab.Add()
                            .Text(tabItem.Key)
                            .LoadContentFrom(tabItem.Value.Action, tabItem.Value.Controller,tabItem.Value.Parameter)
                            .Selected(tabItem.Value.Selected);
                        }
                        else
                        {
                            tab.Add()
                            .Text(tabItem.Key)
                            .LoadContentFrom(tabItem.Value.Action, tabItem.Value.Controller)
                            .Selected(tabItem.Value.Selected);
                        }
                    }
                    else
                    {
                        tab.Add()
                        .Text(tabItem.Key)
                        .Content(tabItem.Value.Content)
                        .Selected(tabItem.Value.Selected);
                    }
                }
            });

            var eventTabs = new Action<TabStripEventBuilder>(e =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.EActivate))
                {
                    e.Activate(builderInfo.EActivate);
                } 
                
                if (!string.IsNullOrWhiteSpace(builderInfo.ESelect))
                {
                    e.Select(builderInfo.ESelect);
                } 
                
                if (!string.IsNullOrWhiteSpace(builderInfo.EError))
                {
                    e.Error(builderInfo.EError);
                }
                if (!string.IsNullOrWhiteSpace(builderInfo.EContentLoad))
                {
                    e.ContentLoad(builderInfo.EContentLoad);
                }

            });

            var tabStripBuilder = helper.Kendo()
                .TabStrip()
                .Items(tabsFactory)
                .Events(eventTabs);
            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                tabStripBuilder.Name(builderInfo.Name);
            }
            return tabStripBuilder;
        }
    }
    #endregion
        
    #region TabStripBuilderInfo
    public class ItemInformation : VnrBaseBuilderInfo
    {
        public ItemInformation()
        {
            Selected = false;
        }

        /// <summary>
        /// Có chọn tab khi load tabstrip không, default là False
        /// </summary>
        public bool Selected { get; set; }
        /// <summary>
        /// Action load dữ liệu cho tabstrip
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// Nội dung cho tabstrip
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Tham số kèm theo khi load dữ liệu
        /// </summary>
        public object Parameter { get; set; }
        /// <summary>
        /// Tên cho tab, nếu không nhập thì mặc định là tên của tabstrip-(index của tab)
        /// </summary>
        public override string Name { get; set; }
    }
    #endregion

    #region TabStripBuilderInfo
    public class TabStripBuilderInfo : VnrBaseBuilderInfo
    {
        #region Properties
        Dictionary<string, ItemInformation> _tabItems;

        /// <summary>
        /// Danh sách các tab trong tabstrip
        /// </summary>
        public Dictionary<string, ItemInformation> TabItems
        {
            get
            {
                if (_tabItems == null)
                {
                    _tabItems = new Dictionary<string, ItemInformation>();
                }
                return _tabItems;
            }
            internal set
            {
                _tabItems = value;
            }
        }

        /// <summary>
        /// Sự kiện khi tab activate
        /// </summary>
        public string EActivate { get; set; }
        /// <summary>
        /// Sự kiện khi tab select
        /// </summary>
        public string ESelect { get; set; }
        /// <summary>
        /// Sự kiện khi xảy ra lỗi trong quá trình xử lý tabstrip
        /// </summary>
        public string EError { get; set; }
        /// <summary>
        /// Sự kiện khi load dữ liệu
        /// </summary>
        public string EContentLoad { get; set; }
        /// <summary>
        /// Tên của tabstrip
        /// </summary>
        public override string Name { get; set; }
        #endregion
    }
    #endregion
}
