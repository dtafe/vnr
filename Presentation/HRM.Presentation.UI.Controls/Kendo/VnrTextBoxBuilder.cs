using System;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public static class VnrTextBoxBuilder
    {
        /// <summary>
        /// Texbox chỉ có thuộc tính Name
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MvcHtmlString VnrTextBox(this HtmlHelper helper, string name)
        {
            var result = new StringBuilder();
            result.Append(helper.TextBox(name,null, new { @class = "k-textbox" }));
            return MvcHtmlString.Create(result.ToString());
        }

        /// <summary>
        /// Texbox hỗ trợ tùy chỉnh định dạng
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <param name="styles"></param>
        /// <returns></returns>
        public static MvcHtmlString VnrTextBox(this HtmlHelper helper, string name, string styles)
        {
            var result = new StringBuilder();
            if (!string.IsNullOrEmpty(styles) || !string.IsNullOrWhiteSpace(styles))
            {
                result.Append(helper.TextBox(name,null, new { @class = "k-textbox", style = styles }));
            }
            return MvcHtmlString.Create(result.ToString());
        }


        /// <summary>
        /// TexboxFor hỗ trợ hiển thị dữ liệu theo model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString VnrTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression)
        {
            var result = new StringBuilder();
            result.Append(helper.TextBoxFor(expression, new { @class = "k-textbox",style ="width:200px;" }));
            return MvcHtmlString.Create(result.ToString());
        }

        /// <summary>
        /// TexboxFor hỗ trợ hiển thị dữ liệu theo model có cho chỉnh sửa định dạng
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="styles"></param>
        /// <returns></returns>
        public static MvcHtmlString VnrTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, string styles)
        {
            var result = new StringBuilder();
            if (!string.IsNullOrEmpty(styles) || !string.IsNullOrWhiteSpace(styles))
            {
                result.Append(helper.TextBoxFor(expression, new { @class = "k-textbox", style = styles }));
            }
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString VnrTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, string styles, string required)
        {
            var result = new StringBuilder();
            //if (!string.IsNullOrWhiteSpace(styles))
            //{
                result.Append(helper.TextBoxFor(expression, new { @class = "k-textbox", style = styles, required = required }));
            //}
            return MvcHtmlString.Create(result.ToString());
        }

        //public static MvcHtmlString VnrTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
        //    Expression<Func<TModel, TProperty>> expression, string required)
        //{
        //    var result = new StringBuilder();
        //    if (!string.IsNullOrWhiteSpace(required))
        //    {
        //        result.Append(helper.TextBoxFor(expression, new { @class = "k-textbox", required = required }));
        //    }
        //    return MvcHtmlString.Create(result.ToString());
        //}

        public static MvcHtmlString VnrTextBox(this HtmlHelper helper, TextBoxBuilderInfo builderInfo)
        {
            string classDefault = "k-textbox ";
            if (builderInfo.ClassName != null || builderInfo.ClassName != string.Empty)
            {
                classDefault += builderInfo.ClassName;
            }
            string str = "<input type='text' class='" + classDefault + "' ";
            if (builderInfo.Disabled)
            {
                str += " disabled='" + builderInfo.Disabled + "' ";
                builderInfo.CssStyle = "background-color:#f8f8f8;" + builderInfo.CssStyle;
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                str += "name='" + builderInfo.Name + "' id='" + builderInfo.Name + "' ";
            }
            builderInfo.CssStyle = "width:" + builderInfo.Width + "px;" + builderInfo.CssStyle;
            if (!string.IsNullOrWhiteSpace(builderInfo.CssStyle))
            {
                str += "style='" + builderInfo.CssStyle + "'";
            }
            str += "/>";
            return MvcHtmlString.Create(str.ToString());
        }

        public static MvcHtmlString VnrTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression, TextBoxBuilderInfo builderInfo)
        {
            var strId = helper.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            string classDefault = "k-textbox ";
            if (builderInfo.ClassName != null || builderInfo.ClassName != string.Empty)
            {
                classDefault += builderInfo.ClassName;
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Name))
            {
                strId = builderInfo.Name;
            }
            string str = "<input type='text' name='" + strId + "' id='" + strId + "' class='" + classDefault + "' ";
            if (builderInfo.Disabled)
            {
                str += " disabled='" + builderInfo.Disabled + "' ";
                builderInfo.CssStyle = "background-color:#f8f8f8;" + builderInfo.CssStyle;
            }      
            builderInfo.CssStyle = "width:" + builderInfo.Width + "px;" + builderInfo.CssStyle;
            if (!string.IsNullOrWhiteSpace(builderInfo.CssStyle))
            {
                str += "style='" + builderInfo.CssStyle + "'";
            }
            if (!string.IsNullOrWhiteSpace(builderInfo.Value))
            {
                str += "value='" + builderInfo.Value + "'";
            }
            if (builderInfo.Readonly)
            {
                str += "readonly";
            }
            str += "/>";
            return MvcHtmlString.Create(str.ToString());
        }

    }

    #region TextBox Infomation
    public class TextBoxBuilderInfo
    {
        public TextBoxBuilderInfo()
        {
            Width = 200;
            Readonly = false;
        }
        public string Name { get; set; }
        public string CssStyle { get; set; }
        public bool Disabled { get; set; }
        public int Width { get; set; }
        public string Value { get; set; }
        public bool Readonly { get; set; }
        public string ClassName { get; set; }
    }
    #endregion
}
