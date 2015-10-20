using System;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

namespace HRM.Presentation.UI.Controls.Kendo
{
    #region VnrWindowBuilder
    public static class VnrWindowBuilder
    {
        public static WindowBuilder VnrWindow(this HtmlHelper helper, WindowBuilderInfo builderInfo)
        {
            var windowAction = new Action<WindowActionsBuilder>(w => w.Close());
            var windowBuilder = helper.Kendo()
                    .Window()
                    .Name(builderInfo.Name)
                    .Width(builderInfo.Width)
                    .Height(builderInfo.Height)
                    .Draggable()
                    .Visible(builderInfo.Visible)
                    .Modal(true)
                    .Title(builderInfo.Title)
                    .Actions(windowAction);
            if (!string.IsNullOrWhiteSpace(builderInfo.LoadContentFrom))
            {
                windowBuilder.LoadContentFrom(builderInfo.LoadContentFrom);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Content))
            {
                windowBuilder.Content(builderInfo.Content);
            }
            if (builderInfo.ContentFunc!=null)
            {
                windowBuilder.Content(builderInfo.ContentFunc);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.EvenClose))
            {
                windowBuilder.Events(m => m.Close(builderInfo.EvenClose));
            }
            return windowBuilder;
        }
    }
    #endregion

    #region WindowBuilderInfo
    public class WindowBuilderInfo : VnrBaseBuilderInfo
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Title { get; set; }
        public string LoadContentFrom { get; set; }
        public string Content { get; set; }
        public bool Visible { get; set; }
        public Func<object, object> ContentFunc { get; set; }
        public string EvenClose { get; set; }
    }
    #endregion
}
