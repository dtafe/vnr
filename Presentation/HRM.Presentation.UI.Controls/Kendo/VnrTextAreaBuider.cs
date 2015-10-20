using System;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public  static class VnrTextAreaBuider
    {
        /// <summary>
        /// TextArea chỉ có thuộc tính Name
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MvcHtmlString VnrTextArea(this HtmlHelper helper, string name)
        {
            var result = new StringBuilder();
            result.Append(helper.TextArea(name, new { @class = "k-textbox" , style="width:300px; height: 65px"}));
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString VnrTextArea(this HtmlHelper helper, TextAreaBuilderInfo builderInfo)
        {
            string str = "<textarea class='k-textbox' ";
            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                str += "name='" + builderInfo.Name + "' id='" + builderInfo.Name + "' ";
            }
            builderInfo.CssStyle = "width:" + builderInfo.Width + "px;height:"+builderInfo.Height+"px;" + builderInfo.CssStyle;
            if (!string.IsNullOrWhiteSpace(builderInfo.CssStyle))
            {
                str += "style='" + builderInfo.CssStyle + "'";
            }
            str += "></textarea>";
            return MvcHtmlString.Create(str.ToString());
        }

        public static MvcHtmlString VnrTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
           Expression<Func<TModel, TProperty>> expression, TextAreaBuilderInfo builderInfo)
        {
            var strId = helper.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                strId = builderInfo.Name;
            }
            string str = "<textarea class='k-textbox' name='" + strId + "' id='" + strId + "' ";
            builderInfo.CssStyle = "width:" + builderInfo.Width + "px;height:" + builderInfo.Height + "px;" + builderInfo.CssStyle;
            if (!string.IsNullOrWhiteSpace(builderInfo.CssStyle))
            {
                str += "style='" + builderInfo.CssStyle + "'";
            }
            str += "></textarea>";
            return MvcHtmlString.Create(str.ToString());
        }


        /// <summary>
        /// TextArea hỗ trợ hiển thị dữ liệu theo model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString VnrTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression)
        {
            var result = new StringBuilder();
            result.Append(helper.TextAreaFor(expression, new { @class = "k-textbox", style="width:300px; height: 65px"}));
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString VnrTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, string styles)
        {
            var result = new StringBuilder();
            result.Append(helper.TextAreaFor(expression, new { @class = "k-textbox", style = styles }));
            return MvcHtmlString.Create(result.ToString());
        }
    }
    #region TextArea Infomation
    public class TextAreaBuilderInfo
    {
        public TextAreaBuilderInfo()
        {
            Width = 300;
            Height = 65;
        }
        public string Name { get; set; }
        public string CssStyle { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
    #endregion
}
