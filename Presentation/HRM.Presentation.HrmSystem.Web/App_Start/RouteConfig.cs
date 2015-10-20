using System.Web.Mvc;
using System.Web.Routing;

namespace HRM.Presentation.HrmSystem.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Sys_DynamicColumn", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
