using HRM.Infrastructure.Security;
using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HRM.Presentation.Main
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
           AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            LicenseService.SetLicense();
        }
    }
}