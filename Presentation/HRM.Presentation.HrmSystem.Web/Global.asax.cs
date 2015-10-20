using HRM.Infrastructure.Security;
using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HRM.Presentation.HrmSystem.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            LicenseService.SetLicense();
        }

        protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
        {
            //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("vi-VN");
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("vi-VN");

        }
    }
}
