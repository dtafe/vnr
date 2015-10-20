using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;

namespace HRM.Presentation.UI.Controls
{
    public partial class Extensions
    {
        public static DropDownListBuilder DropDownListControl(this HtmlHelper helper, string name, string textField, string valueField)
        {
            return helper.Kendo().DropDownList()
                .Name(name)
                .DataTextField(textField)
                .DataValueField(valueField)
                .HtmlAttributes(new { @class = HtmlInputClass });
        }
    }
}
