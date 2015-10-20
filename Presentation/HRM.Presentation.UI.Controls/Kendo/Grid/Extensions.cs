using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HRM.Presentation.UI.Controls
{
    public partial class Extensions
    {
        private static readonly int _defaultGridHeight = 320;
        private static readonly int _defaultColumnWidth = 100;
        private static readonly int _defaultColumnTypeStringWidth = 200;
        private static readonly int _defaultColumnTypeNumberWidth = 200;
        private static readonly int _defaultColumnTypeDateWidth = 150;
        private static GridTemplateColumnBuilder<T> ColumnTemplate<T>(this GridColumnFactory<T> factory, string template) where T : class
        {
            var builder = factory.Template(x =>
            {
                return string.Format(@"<text></text>");
            });
            return builder.ClientTemplate(template);
        }

        public static GridBuilder<T> GridControl<T>(this HtmlHelper helper, string name) where T : class
        {
            //return helper.Kendo().Grid<T>()
            //    .Name(name)
            //    //.Groupable()
            //    .Pageable(pager => { pager.PageSizes(true); })
            //    .Sortable()
            //    .Scrollable()
            //    .Selectable(selectable => selectable.Mode(GridSelectionMode.Single));


            System.Reflection.PropertyInfo[] properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
             return helper.Kendo().Grid<T>()
                 .Name(name)
                 .Columns(columns =>
                 {
                     foreach (var item in properties)
                     {
                         if (item.PropertyType.Name.Equals("String"))
                         {
                             columns.Bound(item.Name).Width(_defaultColumnTypeStringWidth);
                         }
                         else if (item.PropertyType.Name.Equals("Int") || item.PropertyType.Name.Equals("Float") || item.PropertyType.Name.Equals("Double"))
                         {
                             columns.Bound(item.Name).Width(_defaultColumnTypeNumberWidth);
                         }
                         else
                         {
                             columns.Bound(item.Name).Width(_defaultColumnWidth);
                         }
                     }
                 });
        }
    }
}