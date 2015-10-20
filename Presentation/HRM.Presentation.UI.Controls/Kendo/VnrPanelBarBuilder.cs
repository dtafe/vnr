using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System.Web.Mvc;

namespace HRM.Presentation.UI.Controls.Kendo
{
    #region Builder
    public static class VnrPanelBarBuilder
    {
        public static PanelBarBuilder VnrPanelBar(this HtmlHelper helper, PanelBarBuilderInfo builderInfo)
        {
            Action<PanelBarItemFactory> items = new Action<PanelBarItemFactory>(d =>
            {
                if (builderInfo.Items != null && builderInfo.Items.Count > 0)
                {
                    foreach (var item in builderInfo.Items)
                    {
                        var itemInfo = d.Add();
                        if (!string.IsNullOrEmpty(item.Text))
                        {
                            itemInfo.Text(item.Text);
                        }
                        if (!string.IsNullOrEmpty(item.Content))
                        {
                            itemInfo.Content(item.Content);
                        }
                        else 
                        {
                            if (!string.IsNullOrEmpty(item.LoadContentFrom))
                            {
                                itemInfo.LoadContentFrom(item.LoadContentFrom);
                            }
                        }
                        if (!string.IsNullOrEmpty(item.ImageUrl))
                        {
                            itemInfo.ImageUrl(item.ImageUrl);
                        }
                        if (item.ChildItems != null && item.ChildItems.Count > 0)
                        {
                            Action<PanelBarItemFactory> childItems = new Action<PanelBarItemFactory>(i =>
                             {
                                 foreach (var ch in item.ChildItems)
                                 {
                                     var chidlItem = i.Add();
                                     if (!string.IsNullOrEmpty(ch.Text))
                                     {
                                         chidlItem.Text(ch.Text);
                                     }
                                     if (!string.IsNullOrEmpty(ch.Content))
                                     {
                                         chidlItem.Content(ch.Content);
                                     }
                                     else
                                     {
                                         if (!string.IsNullOrEmpty(ch.LoadContentFrom))
                                         {
                                             chidlItem.LoadContentFrom(ch.LoadContentFrom);
                                         }
                                     }
                                     if (!string.IsNullOrEmpty(ch.ImageUrl))
                                     {
                                         chidlItem.ImageUrl(ch.ImageUrl);
                                     }
                                 }
                             });
                            itemInfo.Items(childItems);
                        }

                        itemInfo.Expanded(item.Expanded);
                    }
                }
            });

            var panelBarBuilder = helper.Kendo().PanelBar()
                .Name(builderInfo.Name)
                .Items(items)
                .ExpandMode(builderInfo.ExpandMode);
            if (builderInfo.HtmlAttributes != null)
            {
                panelBarBuilder.HtmlAttributes(builderInfo.HtmlAttributes);
            }
            return panelBarBuilder;
        }
    }
    #endregion
    #region Info
    public class PanelBarItemsInfo
    {
        public PanelBarItemsInfo()
        {
            Expanded = false;
        }
        public string Text { get; set; }
        public bool Expanded { get; set; }
        public string Content { get; set; }
        public string LoadContentFrom { get; set; }
        public string ImageUrl { get; set; }
        public List<PanelBarItemsInfo> ChildItems { get; set; }
    }

    public class PanelBarBuilderInfo : VnrBaseBuilderInfo
    {
        public PanelBarBuilderInfo() 
        {
            ExpandMode = PanelBarExpandMode.Multiple;
        }
        public List<PanelBarItemsInfo> Items { get; set; }
        public object HtmlAttributes { get; set; }
        public PanelBarExpandMode ExpandMode { get; set; }
    }
    #endregion
}
