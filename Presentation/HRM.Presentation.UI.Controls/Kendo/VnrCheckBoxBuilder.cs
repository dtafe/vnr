using System;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public static class VnrCheckBoxBuilder
    {

        /// <summary>
        /// CheckBox chỉ có thuộc tính Name
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MvcHtmlString VnrCheckBox(this HtmlHelper helper, string name)
        {
            var result = new StringBuilder();
            result.Append(helper.CheckBox(name, new { @class = "k-checkbox" }));
            return MvcHtmlString.Create(result.ToString());
        }

        /// <summary>
        /// VnrCheckBoxFor hỗ trợ hiển thị dữ liệu theo model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString VnrCheckBoxFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression)
        {
            var result = new StringBuilder();
            result.Append(helper.EditorFor(expression, new { @class = "k-checkbox" }));
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString VnrCheckBoxFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, CheckBoxBuilderInfo builderInfo)
        {
           // string htmlAttributes = "value='" + builderInfo.Value + "'";
           // if (!string.IsNullOrWhiteSpace(builderInfo.Class))
           // {
           //     htmlAttributes += ",@class = 'k-checkbox " + builderInfo.Class+"'";
           // }
           // if (!string.IsNullOrWhiteSpace(builderInfo.Id))
           // {
           //     htmlAttributes += ",id='"+builderInfo.Id+"'";
           // }
           // if (!string.IsNullOrWhiteSpace(builderInfo.StyleCss))
           // {
           //     htmlAttributes += ",style='"+builderInfo.Id+"'";
           // }
           // object atttributes = (object)htmlAttributes;
           
           //// object htmlAttributes = new object {  };
           // var result = new StringBuilder();
           // result.Append(helper.EditorFor(expression));
            var strId = helper.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            if (builderInfo.Id != null && builderInfo.Id != string.Empty)
            {
                strId = builderInfo.Id;
            }
            var str = "<input class='k-checkbox'  id='" + strId + "' name='" + strId + "' type='checkbox'";
            if (builderInfo.Value == true)
            {
                str += " value ='" + builderInfo.Value + "' checked />";
            }
            else {
                str += " value ='" + builderInfo.Value + "' />";
            }
           
       
           // // id.Replace('[', '_').Replace(']', '_');
            
            return MvcHtmlString.Create(str.ToString());
        }

        //public static MvcHtmlString VnrCheckBoxFor<TModel>(this HtmlHelper<TModel> helper,
        //    Expression<Func<TModel, bool>> expression)
        //{
        //    var result = new StringBuilder();
        //    result.Append(helper.CheckBoxFor(expression, new { @class = "k-checkbox" }));
        //    return MvcHtmlString.Create(result.ToString());
        //} 
        
        public static MvcHtmlString VnrCheckBoxFor<TModel>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> expression, string cssStyle, string classCustom)
        {
            var result = new StringBuilder();
            result.Append(helper.CheckBoxFor(expression, new { @class = classCustom, style = cssStyle }));
            return MvcHtmlString.Create(result.ToString());
        }


    }
    #region Builder Infomation
    public class CheckBoxBuilderInfo 
    {
        public CheckBoxBuilderInfo()
        {
            Value = false;
        }
        public string Class { get; set; }
        public string Id { get; set; }
        public string StyleCss { get; set; }
        public bool Value { get; set; }

        
    }
    #endregion

}
