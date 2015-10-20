using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HRM.Infrastructure.Utilities
{
    public class HrmMvcController : System.Web.Mvc.Controller
    {
        protected override void OnResultExecuted(System.Web.Mvc.ResultExecutedContext filterContext)
        {
            HttpContextBase context = filterContext.HttpContext;
            //if (context.Request.Headers[HrmCorsHandler.Origin] != null)
            //    context.Response.Headers.Add(HrmCorsHandler.AccessControlAllowOrigin, context.Request.Headers[HrmCorsHandler.Origin]);
            base.OnResultExecuted(filterContext);
        }
    }
}
 