using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Category.Web.Controllers
{
    public class Sys_ConfigDBCanController : MainBaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /UserSetting/
        public ActionResult Index()
        {
            
            string str = "HRM_SYS_CAN_TAMSCANLOG_";
            var service = new RestServiceClient<Sys_ConfigDBCanModel>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var modelResult = service.Get(_hrm_Sys_Service, "api/Sys_ConfigDBCan/", str);
            return View(modelResult);
        }

        public ActionResult ConfigDBCan(Sys_ConfigDBCanModel model)
        {
            
            var service = new RestServiceClient<Sys_ConfigDBCanModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Post(_hrm_Sys_Service, "api/Sys_ConfigDBCan/", model);
            return RedirectToAction("Index");
        }
    }
}