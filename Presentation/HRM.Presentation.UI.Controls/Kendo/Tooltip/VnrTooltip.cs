using HRM.Presentation.UI.Controls.Kendo.ControlBase;
using Kendo.Mvc.UI.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kendo.Mvc.UI;

namespace HRM.Presentation.UI.Controls.Kendo.Tooltip
{

    #region VnrTooltipBuilder
    public static class VnrTooltipControl
    {
        public static TooltipBuilder VnrTooltip(this HtmlHelper helper, TooltipInfomation tooltipInfo)
        {

            
            return helper.Kendo().Tooltip()
                .AutoHide(tooltipInfo.AutoHide)
                .Content(tooltipInfo.Content)
               .Width(tooltipInfo.Width)
               .Height(tooltipInfo.Height)
               .Position(tooltipInfo.Position)
              .For(tooltipInfo.Selector);
        }
    }
    #endregion

    #region TooltipInfomation
    public class TooltipInfomation : VnrPropertiesBase
    {
        #region Properties

        private bool autoHide;
        private string[] animation;
        private string content;
        private string contentTemplateId;
        private TooltipPosition position;
        private string filter;
        private string showOn;
        private string selector;


        public string Selector
        {

            get
            {
                return selector;
            }
            set
            {
                selector = value;
            }
        }
        /// <summary>
        /// Quy định cách thức để gọi tooltip 
        /// </summary>
        public string ShowOn
        {
            get { return showOn; }
            set { showOn = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Filter
        {
            get { return filter; }
            set { filter = value; }
        }

        /// <summary>
        /// Xác định vị trí hiển thị của tooltip so với đối tượng
        /// </summary>
        public TooltipPosition Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Gọi template được khởi tạo bên ngoài để sử dụng
        /// </summary>
        public string ContentTemplateId
        {
            get { return contentTemplateId; }
            set { contentTemplateId = value; }
        }

        /// <summary>
        /// Quy định nội dung chứa trong template
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        /// <summary>
        /// Quy định thay đổi các thuộc tính hiển thị mặc định của tooltip
        /// </summary>
        public string[] Animation
        {
            get { return animation; }
            set { animation = value; }
        } 

        /// <summary>
        /// Mặc định tooltip sẽ ẩn đi khi rê chuột ra khỏi đối tượng
        /// </summary>
        public bool AutoHide
        {
            get { return autoHide; }
            set { autoHide = value; }
        }

        #endregion

        #region Methods

        #endregion

        #region Events

        #endregion

    }
    #endregion

}
