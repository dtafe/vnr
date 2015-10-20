using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public static class VnrEditorBuilder
    {
        public static EditorBuilder VnrEditor(this HtmlHelper helper, VnrEditorBuilderInfo builderInfo)
        {
            var editorBuilder = helper.Kendo().Editor()
                .Name(builderInfo.Name);

            #region Xử lý width, height
            var strStyle = string.Empty;
            if (builderInfo.Width > 0)
            {
                strStyle += "width:" + builderInfo.Width + "px; ";
            }
            if (builderInfo.Height > 0)
            {
                strStyle += "height:" + builderInfo.Height + "px; ";
            }
            if (!string.IsNullOrEmpty(strStyle))
            {
                if (builderInfo.HtmlAttributes == null)
                {
                    builderInfo.HtmlAttributes = new Dictionary<string, object>();
                }
                builderInfo.HtmlAttributes.Add("style", strStyle);
            } 
            #endregion

            if (builderInfo.HtmlAttributes != null && builderInfo.HtmlAttributes.Any())
            {
                editorBuilder.HtmlAttributes(builderInfo.HtmlAttributes);
            }
            if (!string.IsNullOrEmpty(builderInfo.Value))
            {
                editorBuilder.Value(builderInfo.Value);
            }
            return editorBuilder;
        }
    }

    public class VnrEditorBuilderInfo : VnrBaseBuilderInfo
    {
        public IDictionary<string, object> HtmlAttributes { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Value { get; set; }
    }
}
