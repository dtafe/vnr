﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using HRM.Presentation.HrmSystem.Web.Controllers;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;

namespace HRM.Presentation.Category.Web.Controllers
{
    public class Sys_ConfigDBLauController : BaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /UserSetting/
        public ActionResult Index()
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sys_ConfigDBLau);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            string str = "HRM_SYS_LAU_TAMSCANLOG_";
            var service = new RestServiceClient<Sys_ConfigDBLauModel>();
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var modelResult = service.Get(_hrm_Sys_Service, "api/Sys_ConfigDBLau/", str);
            return View(modelResult);
        }

        public ActionResult ConfigDBLau(Sys_ConfigDBLauModel model)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_ConfigDBLau);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_ConfigDBLauModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Post(_hrm_Sys_Service, "api/Sys_ConfigDBLau/", model);
            return RedirectToAction("Index");
        }
    }
}