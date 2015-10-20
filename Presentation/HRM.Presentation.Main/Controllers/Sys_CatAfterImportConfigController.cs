using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Security;
using HRM.Presentation.HrmSystem.Models;

namespace HRM.Presentation.Main.Controllers
{
    public class Sys_CatAfterImportConfigController : MainBaseController
    {
        readonly string _hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;

        public ActionResult Create()
        {
           
            var service = new RestServiceClient<Sys_CatTastAfterImportConfigModel>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_CatTastAfterImportConfig/", UserId);
            if (result != null)
            {
                return View(result);
            }
            return View();
        }
	}
}