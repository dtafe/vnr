using System;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using VnResource.Helper.Data;
using System.Linq;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public static class VnrLabelBuider
    {
        public static MvcHtmlString VnrLabel<TModel>(this HtmlHelper<TModel> helper, string text)
        {
            return VnrLabel<TModel>(helper, text, "lbl" + Common.ChuyenTVKhongDau(text));
        }

        public static MvcHtmlString VnrLabel<TModel>(this HtmlHelper<TModel> helper, string text, string lblId)
        {
            ////object htmlAttributes
            //string displayText = expression.GetString().Split('.').LastOrDefault();
            //var displayName = (System.ComponentModel.DisplayNameAttribute)typeof(TModel).GetProperty(displayText).GetCustomAttributes(false).Where(d => d.GetType() == typeof(System.ComponentModel.DisplayNameAttribute)).FirstOrDefault();
            //var description = displayText;
            //if (displayName != null)
            //{
            //    description = displayName.DisplayName.TranslateString();
            //    displayText = description;
            //}
            //if (displayText != null)
            //{
            //    displayText = displayText.TranslateString();
            //}
            var result = new StringBuilder();
            ////var url = HttpContext.Current.Request.Url.Authority;
            ////var urlSrc = "http://"+url +"/Content/images/ico-help.gif";
            ////result.Append("<img title='" + description + "'src='" + urlSrc + "' alt='" + description + "'>");


            result.Append(helper.Label(text, new { id = lblId }));
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString VnrLabelFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            //object htmlAttributes
            string displayText = expression.GetString().Split('.').LastOrDefault();
            var displayName = (System.ComponentModel.DisplayNameAttribute)typeof(TModel).GetProperty(displayText).GetCustomAttributes(false).Where(d => d.GetType() == typeof(System.ComponentModel.DisplayNameAttribute)).FirstOrDefault();
            var description = displayText; 
            if(displayName != null)
            {
                description = displayName.DisplayName.TranslateString();
                displayText = description;
            }
            if(displayText != null)
            {
                displayText = displayText.TranslateString();
            }
            var result = new StringBuilder();
            //var url = HttpContext.Current.Request.Url.Authority;
            //var urlSrc = "http://"+url +"/Content/images/ico-help.gif";
            //result.Append("<img title='" + description + "'src='" + urlSrc + "' alt='" + description + "'>");

            var strId = helper.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            result.Append(helper.LabelFor(expression, displayText, new { id = "lbl" + strId }));
            return MvcHtmlString.Create(result.ToString());
        }

        #region Label Icon

        /// <summary> Hiển thị Label có Icon </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="helper"></param>
        /// <param name="text">Text hiển thị</param>
        /// <returns></returns>
        public static MvcHtmlString VnrLabelIcon<TModel>(this HtmlHelper<TModel> helper, string text)
        {
            return VnrLabelIcon(helper, text, text);
        }

        /// <summary> Hiển thị Label có Icon </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="helper"></param>
        /// <param name="text">Text hiển thị</param>
        /// <param name="description">Mô tả dữ liệu trên icon</param>
        /// <returns></returns>
        public static MvcHtmlString VnrLabelIcon<TModel>(this HtmlHelper<TModel> helper, string text , string description)
        {
            var result = new StringBuilder();
            var url = HttpContext.Current.Request.Url.Authority;
            var urlSrc = "http://" + url + "/Content/images/ico-help.gif";
            result.Append("<img title='" + description + "'src='" + urlSrc + "' alt='" + description + "'>");
            result.Append(helper.Label(text));
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString VnrLabelIconFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            //object htmlAttributes
            string displayText = expression.GetString().Split('.').LastOrDefault();
            var displayName = (System.ComponentModel.DisplayNameAttribute)typeof(TModel).GetProperty(displayText).GetCustomAttributes(false).Where(d => d.GetType() == typeof(System.ComponentModel.DisplayNameAttribute)).FirstOrDefault();
            var descriptionName = (System.ComponentModel.DescriptionAttribute)typeof(TModel).GetProperty(displayText).GetCustomAttributes(false).Where(d => d.GetType() == typeof(System.ComponentModel.DescriptionAttribute)).FirstOrDefault();

            #region displayText - Hiển thị tiêu đề
            if (displayName != null)
            {
                displayText = displayName.DisplayName.TranslateString();
            }
            if (displayText != null)
            {
                displayText = displayText.TranslateString();
            } 
            #endregion

            #region description - Hiển thị mô tả cho icon
            var description = displayText;
            if (descriptionName != null)
            {
                description = descriptionName.Description.TranslateString();
            }
            if (descriptionName == null)
            {
                description = displayText;
            }
            #endregion
            
            var result = new StringBuilder();
            //Hien thị mô tả cho icon
            var url = HttpContext.Current.Request.Url.Authority;
            var urlSrc = "http://" + url + "/Content/images/ico-help.gif";
            result.Append("<img title='" + description + "'src='" + urlSrc + "' alt='" + description + "'>");

            //hien thị text
            var strId = helper.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            result.Append(helper.LabelFor(expression, displayText, new { id = "lbl" + strId }));
            return MvcHtmlString.Create(result.ToString());
        }

        #endregion

        

        public static MvcHtmlString VnrLabelForNoneIcon<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            string displayText = expression.GetString().Split('.').LastOrDefault();
            var displayName = (System.ComponentModel.DisplayNameAttribute)typeof(TModel).GetProperty(displayText).GetCustomAttributes(false).Where(d => d.GetType() == typeof(System.ComponentModel.DisplayNameAttribute)).FirstOrDefault();
            var description = displayText;
            if (displayName != null)
            {
                description = displayName.DisplayName.TranslateString();
                displayText = description;
            }
            if (displayText != null)
            {
                displayText = displayText.TranslateString();
            }
            var result = new StringBuilder();

            var strId = helper.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            result.Append(helper.LabelFor(expression, displayText.TranslateString(), new { id = "lbl" + strId }));
            return MvcHtmlString.Create(result.ToString());
        }
    }
}
