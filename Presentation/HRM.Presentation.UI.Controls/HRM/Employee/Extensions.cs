using Kendo.Mvc.UI.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HRM.Presentation.UI.Controls
{
    public partial class Extensions
    {
        public static DropDownListBuilder DepartmentDropDownListControl(this HtmlHelper helper, string name, string baseCRUD)
        {
            return DropDownListControl(helper, name, "Name", "Id")
                .Template("<table><tr><td>#: data.Name #</td><td></td><td></td><td>#: data.Code #</td></tr></table>");

        }
    }
}
