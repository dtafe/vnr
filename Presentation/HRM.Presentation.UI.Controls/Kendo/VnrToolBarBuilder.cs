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
    public static class VnrToolBarBuilder
    {
        public static ToolBarBuilder VnrToolBar(this HtmlHelper helper, ToolBarBuilderInfo builderInfo)
        {
            Action<ToolBarItemFactory> items = new Action<ToolBarItemFactory>(d =>
            {
                if (builderInfo.ButtonInfo != null && builderInfo.ButtonInfo.Count > 0)
                {
                    var buttonInfo = builderInfo.ButtonInfo;
                    foreach (var item in buttonInfo)
                    {
                        var button = d.Add();
                        if (!string.IsNullOrEmpty(item.Text))
                        {
                            button.Text(item.Text);
                        }
                        if (!string.IsNullOrEmpty(item.Template))
                        {
                            button.Template(item.Template);
                        }
                        if (!string.IsNullOrEmpty(item.Id))
                        {
                            button.Id(item.Id);
                        }
                        if (!string.IsNullOrEmpty(item.Click))
                        {
                            button.Click(item.Click);
                        }
                        if (item.HtmlAttributes != null)
                        {
                            button.HtmlAttributes(item.HtmlAttributes);
                        }
                        if (!string.IsNullOrEmpty(item.ImageUrl))
                        {
                            button.ImageUrl(item.ImageUrl);
                        }
                        button.Togglable(item.Togglable);
                        button.Type(item.Type);
                        button.Overflow(item.Overflow);

                        #region MenuButtons
                        if (item.MenuButtons != null && item.MenuButtons.Count > 0)
                        {
                            Action<ToolBarItemMenuButtonFactory> menuButtons = new Action<ToolBarItemMenuButtonFactory>(m =>
                            {
                                foreach (var mn in item.MenuButtons)
                                {
                                    var menuButton = m.Add();
                                    if (!string.IsNullOrEmpty(mn.Text))
                                    {
                                        menuButton.Text(mn.Text);
                                    }
                                    if (!string.IsNullOrEmpty(mn.ImageUrl))
                                    {
                                        menuButton.ImageUrl(mn.ImageUrl);
                                    }
                                    if (!string.IsNullOrEmpty(mn.SpriteCssClass))
                                    {
                                        menuButton.SpriteCssClass(mn.SpriteCssClass);
                                    }
                                    if (!string.IsNullOrEmpty(mn.Id))
                                    {
                                        menuButton.Id(mn.Id);
                                    }
                                }
                            });

                            button.MenuButtons(menuButtons);
                        }
                        #endregion
                        #region Buttons
                        if (item.Buttons != null && item.Buttons.Count > 0)
                        {
                            Action<ToolBarItemButtonFactory> buttons = new Action<ToolBarItemButtonFactory>(b =>
                            {
                                foreach (var bt in item.Buttons)
                                {
                                    var chilButton = b.Add();
                                    if (!string.IsNullOrEmpty(bt.Text))
                                    {
                                        chilButton.Text(bt.Text);
                                    }
                                    if (!string.IsNullOrEmpty(bt.ImageUrl))
                                    {
                                        chilButton.ImageUrl(bt.ImageUrl);
                                    }
                                    if (!string.IsNullOrEmpty(bt.Group))
                                    {
                                        chilButton.Group(bt.Group);
                                    }
                                    if (!string.IsNullOrEmpty(bt.SpriteCssClass))
                                    {
                                        chilButton.SpriteCssClass(bt.SpriteCssClass);
                                    }
                                    if (!string.IsNullOrEmpty(bt.Id))
                                    {
                                        chilButton.Id(bt.Id);
                                    }
                                    if (!string.IsNullOrEmpty(bt.Click))
                                    {
                                        chilButton.Click(bt.Click);
                                    }

                                    chilButton.ShowText(bt.ShowText);
                                    chilButton.Togglable(bt.Togglable);
                                }
                            });

                            button.Buttons(buttons);
                        }
                        #endregion
                    }
                }
            });
            Action<ToolBarEventBuilder> events = new Action<ToolBarEventBuilder>(e =>
            {
                if (!string.IsNullOrEmpty(builderInfo.EventClick))
                {
                    e.Click(builderInfo.EventClick);
                }
            });

            var toolBarBuilder = helper.Kendo().ToolBar()
                .Name(builderInfo.Name)
                .Items(items)
                .Events(events);

            return toolBarBuilder;
        }
    }

    public class ButtonInfo
    {
        public ButtonInfo()
        {
            Togglable = false;
            Type = CommandType.Button;
            Overflow = ShowInOverflowPopup.Auto;
        }
        public string Text { get; set; }
        public bool Togglable { get; set; }
        public CommandType Type { get; set; }
        public ShowInOverflowPopup Overflow { get; set; }
        public string Template { get; set; }
        public List<MenuButtonsInfo> MenuButtons { get; set; }
        public List<ButtonsInfo> Buttons { get; set; }
        public string Id { get; set; }
        public string Click { get; set; }
        public string ImageUrl { get; set; }
        public object HtmlAttributes { get; set; }
    }
    public class MenuButtonsInfo
    {
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public string SpriteCssClass { get; set; }
        public string Id { get; set; }
    }
    public class ButtonsInfo
    {
        public ButtonsInfo()
        {
            ShowText = ShowIn.Both;
            Togglable = false;
        }
        public string Text { get; set; }
        public bool Togglable { get; set; }
        public string SpriteCssClass { get; set; }
        public ShowIn ShowText { get; set; }
        public string ImageUrl { get; set; }
        public string Group { get; set; }
        public string Id { get; set; }
        public string Click { get; set; }
    }

    public class ToolBarBuilderInfo : VnrBaseBuilderInfo
    {
        public List<ButtonInfo> ButtonInfo { get; set; }
        public string EventClick { get; set; }
    }
}
