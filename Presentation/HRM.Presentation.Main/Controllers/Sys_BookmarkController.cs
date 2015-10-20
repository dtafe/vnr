using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.HrmSystem.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;


namespace HRM.Presentation.Main.Controllers
{
    public class Sys_BookmarkController : MainBaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /SysBookmark/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Tạo mời một Sys_Bookmark
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create(String name, String url, String HotKey)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_Resource);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            //var service = new RestServiceClient<Sys_ResourceModel>(UserLogin);
            //service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            //var result = service.Put(_hrm_Sys_Service, "api/SysResource/", model);
            //return Json(result);
            Sys_BookmarkModel mode = new Sys_BookmarkModel 
            { 
                Link = url,
                Name = name,
                UserCreate = HotKey,
                
            };
            var service = new RestServiceClient<Sys_BookmarkModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service,"api/Sys_Bookmark",mode);
            return Json(result);
        }

	}
}