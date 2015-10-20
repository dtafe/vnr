using HRM.Presentation.UI.Controls.Kendo.ControlBase;
using Kendo.Mvc.UI.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using VnResource.Helper.Data;
using Kendo.Mvc.UI;

namespace HRM.Presentation.UI.Controls.Kendo.Window
{
    #region VnrWindowBuilder
    public static class VnrWindowControl
    {
        public static WindowBuilder VnrWindow(this HtmlHelper helper, WindowInfomation windowInfo)
        {
            Action<WindowActionsBuilder> actions = new Action<WindowActionsBuilder>(d =>
            {
                //Hàm clear để reset lại WindowActionsBuilder
                WindowActionsBuilder actionsBuilder = d.Clear();
              
                if (windowInfo.PinButton)
                {
                    actionsBuilder = actionsBuilder.Pin();
                }
               
                if (windowInfo.MinimizeBox)
                {
                    actionsBuilder = actionsBuilder.Minimize();
                }

                if (windowInfo.MaximizeBox)
                {
                    actionsBuilder = actionsBuilder.Maximize();
                }

                if (windowInfo.CloseButton)
                {
                    actionsBuilder = actionsBuilder.Close();
                }
            });

            WindowBuilder builder = helper.Kendo().Window()
                .Name(windowInfo.Name)
                .Title(windowInfo.Title)
                .Draggable(windowInfo.Draggable)
                .Width(windowInfo.Width)
                .Height(windowInfo.Height)
                .Content(windowInfo.Content)
                .Resizable()
                .Position(p =>
                {
                    p.Left(windowInfo.PositionLeft);
                    p.Top(windowInfo.PositionTop);
                })
                .Modal(windowInfo.Modal)
                .Actions(actions)
                ;

            return builder;
        }
    }
    #endregion

    #region WindowInfomation
    public class WindowInfomation : VnrPropertiesBase
    {
        #region Properties

        private string title;
        private string actions;
        private string[] animation;
        private string content;
        private string[] appendTo;
        private bool draggable;
        private bool modal;
        private int positionTop;
        private int positionLeft;
        private bool resizable;
        private bool visible;
        private bool minimizeBox;
        private bool maximizeBox;
        private bool closeButton;
        private bool pinButton;

        public bool PinButton
        {
            get { return pinButton; }
            set { pinButton = value; }
        }

        public bool CloseButton
        {
            get { return closeButton; }
            set { closeButton = value; }
        }

        public bool MaximizeBox
        {
            get { return maximizeBox; }
            set { maximizeBox = value; }
        }

        public bool MinimizeBox
        {
            get { return minimizeBox; }
            set { minimizeBox = value; }
        }

        /// <summary>
        /// Quy định hiển thị control
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        /// <summary>
        /// Quy định có cho phép thay đổi kích thước widget hay không
        /// </summary>
        public bool Resizable
        {
            get { return resizable; }
            set { resizable = value; }
        }

        /// <summary>
        /// Quy định vị trí xuất hiện của widget 
        /// </summary>
        public int PositionTop
        {
            get { return positionTop; }
            set { positionTop = value; }
        }
        /// <summary>
        /// Quy định vị trí xuất hiện của widget 
        /// </summary>
        public int PositionLeft
        {
            get { return positionLeft; }
            set { positionLeft = value; }
        }
        /// <summary>
        /// Khi widget hiển thị, thì phần còn lại của page sẽ bị mờ đi và không thể tương tác
        /// </summary>
        public bool Modal
        {
            get { return modal; }
            set { modal = value; }
        }

        /// <summary>
        /// Quy định khả năng có thể di chuyển widget hay không 
        /// </summary>
        public bool Draggable
        {
            get { return draggable; }
            set { draggable = value; }
        }

        /// <summary>
        /// Gọi nội dung từ 1 form khác vào hiển thị bên trong window
        /// </summary>
        public string[] AppendTo
        {
            get { return appendTo; }
            set { appendTo = value; }
        }

        /// <summary>
        /// Nội dung chứa bên trong window
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }


        /// <summary>
        /// @@
        /// </summary>
        public string[] Animation
        {
            get { return animation; }
            set { animation = value; }
        }

        /// <summary>
        /// Quy định các phím chức năng được phép xuất hiện trên window
        /// </summary>
        public string Actions
        {
            get { return actions; }
            set { actions = value; }
        }

        /// <summary>
        /// Tiêu đề của control
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        #endregion

        #region Methods

        #endregion

        #region Events

        #endregion

    }
    #endregion
}
