using System;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace HRM.Presentation.UI.Controls.Kendo
{
    public static class VnrRadioButtonBuilder
    {

        public static MvcHtmlString VnrRadioButton<TModel>(this HtmlHelper<TModel> helper, string names)
        {
            var result = new StringBuilder();
            result.Append(helper.RadioButton(names,false));
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString VnrRadioButtonFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression)
        {
            var result = new StringBuilder();
            result.Append(helper.RadioButtonFor(expression,false));
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString VnrRadioButtonFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, string names)
        {

            var result = new StringBuilder();
            result.Append(helper.RadioButtonFor(expression, false, new { Name = names }));
            return MvcHtmlString.Create(result.ToString());
        }
    }
}
