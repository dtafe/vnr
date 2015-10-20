using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Security;
using HRM.Presentation.HrmSystem.Models;

namespace HRM.Presentation.HrmSystem.Web.Controllers
{
    public class Sys_TraConfigController : BaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;

        public ActionResult Create()
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sys_HreConfig);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_TraConfigModel>();
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_TraConfig/", UserId);
            if (result != null)
            {
                return View(result);
            }
            return View();
        }

	}
}