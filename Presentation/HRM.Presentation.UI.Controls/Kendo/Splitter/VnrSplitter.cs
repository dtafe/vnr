using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HRM.Presentation.UI.Controls.Kendo.ControlBase;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

namespace HRM.Presentation.UI.Controls.Kendo.Splitter
{

    #region VnrSplitterBuilder

    public static class VnrSplitter
    {
        public static SplitterBuilder SplitterControl(this HtmlHelper helper, SplitterInformation splitter)
        {
            Action<SplitterPaneFactory> pane=new Action<SplitterPaneFactory>(p =>
            {
                foreach (var item in splitter.Panes)
                {
                    if (string.IsNullOrEmpty(item.Value.WidthPanes))
                    {
                        item.Value.WidthPanes = "100%";
                    }
                    if (!string.IsNullOrEmpty(item.Value.ControllerName) || !string.IsNullOrEmpty(item.Value.ActionName))
                    {
                        p.Add()
                            .LoadContentFrom(item.Value.ActionName, item.Value.ControllerName)
                            .Collapsible(item.Value.Collapsible)
                            .Scrollable(item.Value.Scrollable)
                            .Size(item.Value.WidthPanes);
                    }
                    else
                    {
                        p.Add()
                            .Content(item.Value.Content)
                            .Collapsible(item.Value.Collapsible)
                            .Scrollable(item.Value.Scrollable)
                            .Size(item.Value.WidthPanes);
                    }
                }
            });
            return helper.Kendo().Splitter()
                .Name(splitter.Name)
                .HtmlAttributes(new {style="height:"+splitter.Height+"px;"})
                .Panes(pane);
        }
    }

    #endregion
    public class PanesInformation : VnrPropertiesBase
    {
        private string _controllerName = string.Empty;
        private string _actionName = string.Empty;
        private bool _scrollable = true;
        private bool _collapsible = true;
        private string _content = string.Empty;
        private string _widthPanes = string.Empty;

        /// <summary>
        /// Controller name
        /// </summary>
        public string ControllerName
        {
            get { return _controllerName; }
            set { _controllerName = value; }
        }

        /// <summary>
        /// Action name
        /// </summary>
        public string ActionName
        {
            get { return _actionName; }
            set { _actionName = value; }
        }

        /// <summary>
        /// Hiển thị thanh scrollbar default = true
        /// </summary>
        public bool Scrollable
        {
            get { return _scrollable; }
            set { _scrollable = value; }
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
        /// Nội dung hiển thị là text tùy chỉnh
        /// </summary>
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        /// <summary>
        /// Độ rộng của Panes
        /// </summary>
        public string WidthPanes
        {
            get { return _widthPanes; }
            set { _widthPanes = value; }
        }


    }
    #region SplitterInformation

    public class SplitterInformation : VnrPropertiesBase
    {
        #region Properties

        private string _orientation = "H";
        private string _style = string.Empty;
        Dictionary<string, PanesInformation> _panes;

        /// <summary>
        /// Nội dung chứ trong panes
        /// </summary>
        public Dictionary<string, PanesInformation> Panes
        {
            get
            {
                if (_panes == null)
                {
                    _panes = new Dictionary<string, PanesInformation>();
                }
                return _panes;
            }
            internal set
            {
                _panes = value;
            }
        }
        public string Style
        {
            get { return _style; }
            set { _style = value; }
        }

        /// <summary>
        /// Dạng paner horizontal/vertical
        /// </summary>
        public string Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        #endregion

        #region Method

        #endregion

        #region Event

        #endregion
    }

    #endregion
}
