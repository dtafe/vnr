using System;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public static class VnrButtonBuilder
    {
        public static ButtonBuilder VnrButton<TModel>(this HtmlHelper<TModel> helper,
            ButtonBuilderInfo builderInfo)
        {
            var events = new Action<ButtonEventBuilder>(e =>
            {
                if (!string.IsNullOrWhiteSpace(builderInfo.EventClick))
                {
                    e.Click(builderInfo.EventClick);
                }
            });
            var buttonBuilder = helper.Kendo().Button()
                .Name(builderInfo.Name)
                .HtmlAttributes(new { type = "button" , permission = builderInfo.Permission })
                .Events(events);
            if (!string.IsNullOrWhiteSpace(builderInfo.Content))
            {
                buttonBuilder.Content(builderInfo.Content);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Icon))
            {
                buttonBuilder.Icon(builderInfo.Icon);
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.ImageUrl))
            {
                buttonBuilder.ImageUrl(builderInfo.ImageUrl);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.SpriteCssClass))
            {
                buttonBuilder.SpriteCssClass(builderInfo.SpriteCssClass);
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.CssStyle))
            {
                buttonBuilder.HtmlAttributes(new { style=builderInfo.CssStyle});
            }
            return buttonBuilder;
        }
    }

    #region Button Infomation
    public class ButtonBuilderInfo : VnrBaseBuilderInfo
    {
        public string Icon { get; set; }
        public string ImageUrl { get; set; }
        public string Content { get; set; }
        public string EventClick { get; set; }
        public string SpriteCssClass { get; set; }
        public string CssStyle { get; set; }

        public string Permission { get; set; }
    }
    #endregion
}
