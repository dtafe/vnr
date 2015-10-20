using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public static class VnrSplitterBuilder
    {
        public static SplitterBuilder VnrSplitter(this HtmlHelper helper, SplitterBuilderInfo builderInfo)
        {
            var pane = new Action<SplitterPaneFactory>(p =>
            {
                foreach (var item in builderInfo.Panes)
                {
                    if (!string.IsNullOrWhiteSpace(item.Value.Content))
                    {
                        p.Add()
                            .Content(item.Value.Content)
                            .Collapsible(item.Value.Collapsible)
                            .Scrollable(item.Value.Scrollable)
                            .Size(item.Value.Size)
                            .Resizable(item.Value.Resizable);
                    }
                    else if (!string.IsNullOrEmpty(item.Value.Controller) || !string.IsNullOrEmpty(item.Value.ActionName))
                    {
                        p.Add()
                            .LoadContentFrom(item.Value.ActionName, item.Value.Controller)
                            .Collapsible(item.Value.Collapsible)
                            .Scrollable(item.Value.Scrollable)
                            .Size(item.Value.Size)
                            .Resizable(item.Value.Resizable);
                    }
                }
            });
            var splitterBuilder = helper.Kendo().Splitter()
                .Orientation(builderInfo.Orientation)
                .Name(builderInfo.Name)
                .HtmlAttributes(new {style="height:"+builderInfo.Height+"px;"})
                .Panes(pane);
            return splitterBuilder;
        }
    }

    public class PaneInfo : VnrBaseBuilderInfo
    {
        private bool _scrollable = true;
        private bool _collapsible = true;
        private string _content = string.Empty;
        private bool _resizable = true;
        private string _size = string.Empty;

        /// <summary>
        /// Action name
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Nội dung hiển thị là text tùy chỉnh
        /// </summary>
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        /// <summary>
        /// Hiển thị thanh scrollbar default = true
        /// </summary>
        public bool Scrollable
        {
            get { return _scrollable; }
            set { _scrollable = value; }
        }

        public bool Resizable
        {
            get { return _resizable; }
            set { _resizable = value; }
        }

        /// <summary>
        /// Cho phép resize hay không, default = true
        /// </summary>
        public bool Collapsible
        {
            get { return _collapsible; }
            set { _collapsible = value; }
        }

        /// <summary>
        /// Độ rộng của Panes
        /// </summary>
        public string Size
        {
            get { return _size; }
            set { _size = value; }
        }
    }

    public class SplitterBuilderInfo : VnrBaseBuilderInfo
    {
        #region Properties
        private int _height = 500;
        private SplitterOrientation _orientation = SplitterOrientation.Horizontal;
        Dictionary<string, PaneInfo> _panes;

        /// <summary>
        /// Nội dung chứ trong panes
        /// </summary>
        public Dictionary<string, PaneInfo> Panes
        {
            get
            {
                if (_panes == null)
                {
                    _panes = new Dictionary<string, PaneInfo>();
                }
                return _panes;
            }
            internal set
            {
                _panes = value;
            }
        }

        /// <summary>
        /// Dạng paner horizontal/vertical
        /// </summary>
        public SplitterOrientation Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        #endregion
    }
}
