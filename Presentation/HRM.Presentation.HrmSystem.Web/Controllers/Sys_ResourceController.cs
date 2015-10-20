﻿using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.HrmSystem.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;

namespace HRM.Presentation.HrmSystem.Web.Controllers
{
   
    public class Sys_ResourceController : BaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        //
        // GET: /Sys_Resource/
        public ActionResult Index()
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sys_Resource);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            return View();
        }


        // <summary>
        /// Lấy tất cả dữ liệu trong table Sys_Resource
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Sys_ResourceModel>>();
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/SysResource/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một Sys_Resource
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] Sys_ResourceModel model)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_Resource);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_ResourceModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/SysResource/", model);
            return Json(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Sys_Resource);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_ResourceModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Delete(_hrm_Sys_Service, "api/SysResource/", id);
            return Json(result);
        }

       
    }
    
}