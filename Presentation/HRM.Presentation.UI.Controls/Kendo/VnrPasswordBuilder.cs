using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public static class VnrPasswordBuilder
    {
        public static MvcHtmlString VnrPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
           Expression<Func<TModel, TProperty>> expression)
        {
            var result = new StringBuilder();
            result.Append(helper.PasswordFor(expression, new { @class = "k-textbox", style = "width:200px;" }));
            return MvcHtmlString.Create(result.ToString());
        }
        public static MvcHtmlString VnrPassword(this HtmlHelper helper, PasswordBuilderInfo builderInfo)
        {
            var str = "<input class='k-textbox " + builderInfo.Class + "' type='password' ";
            if (!string.IsNullOrWhiteSpace(builderInfo.Id))
            {
                str += "id='" + builderInfo.Id + "'";
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                str += "name='"+builderInfo.Name+"'";
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
            {
                str += "style='"+builderInfo.StyleCss+"'";
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.Value))
            {
                str += "value='"+builderInfo.Value+"'";
            }
            str += "/>";
            return MvcHtmlString.Create(str.ToString());
        }

        public static MvcHtmlString VnrPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
           Expression<Func<TModel, TProperty>> expression,PasswordBuilderInfo builderInfo)
        {
            var strId = helper.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            if (!string.IsNullOrWhiteSpace(builderInfo.Id))
            {
                strId  = builderInfo.Id;
            }
            var str = "<input class='k-textbox " + builderInfo.Class + "' id='" + strId + "' name='" + strId + "' type='password' ";
           
            if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
            {
                str += "style='" + builderInfo.StyleCss + "'";
            }

            if (!string.IsNullOrWhiteSpace(builderInfo.Value))
            {
                str += "value='" + builderInfo.Value + "'";
            }
            str += "/>";
            return MvcHtmlString.Create(str.ToString());
        }
    }
    #region Builder Infomation
    public class PasswordBuilderInfo
    {
        public string Class { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string StyleCss { get; set; }
        public string Value { get; set; }
    }
    #endregion
}
