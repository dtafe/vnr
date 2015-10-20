using System;
using System.Collections.Generic;
using System.Linq;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using VnResource.Helper.Data;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;


namespace HRM.Presentation.HrmSystem.Web.Controllers
{
    public class Sys_GroupUserController : BaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        //
        // GET: /Sys_GroupUser/
        public ActionResult Index(int id)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sys_GroupUser);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<IEnumerable<Sys_GroupUserModel>>();
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_GroupUser/", id);
            return View(result);
        }  
        
        public ActionResult ReadGrid([DataSourceRequest] DataSourceRequest request, int id)
        {
            var service = new RestServiceClient<IEnumerable<Sys_GroupUserModel>>();
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_GroupUser/", id).ToList();
            return Json(result.ToDataSourceResult(request));
        } 

        public ActionResult SysGroupUserInfo(string id)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_GroupUser);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_GroupUserModel>();
                service.SetCookies(Request.Cookies, _hrm_Sys_Service);
                var result = service.Get(_hrm_Sys_Service, "api/Sys_GroupUser/", id1);
                return View(result);
            }
            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table Sys_GroupUser
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 
        public ActionResult Get([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Sys_GroupUserModel>>();
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_GroupUser/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mới một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add([Bind] Sys_GroupUserModel model)
        {
            var service = new RestServiceClient<Sys_GroupUserModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_GroupUser/", model);
            return Json(result);
        }

        public ActionResult Create()
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_GroupUser);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult Create(Sys_GroupUserModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Sys_GroupUserModel>();
                service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                var result = service.Put(_hrm_Sys_Service, "api/Sys_GroupUser/", model);
                ViewBag.MsgInsert = "Insert success";
            }
            return View();
        }


        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public ActionResult Edit(Guid id)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sys_GroupUser);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_GroupUserModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_GroupUser/", id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit([Bind] Sys_GroupUserModel GroupUser1)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sys_GroupUser);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_GroupUserModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_GroupUser/", GroupUser1);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Sys_GroupUserModel model)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sys_GroupUser);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            //if (ModelState.IsValid)
            //{
            var service = new RestServiceClient<Sys_GroupUserModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_GroupUser/", model);
            //return Json(result);
            //ViewBag.MsgUpdate = "Update success";
            //}
            return Json(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Sys_GroupUser);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_GroupUserModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Delete(_hrm_Sys_Service, "api/Sys_GroupUser/", id);
            return Json(result);
        }


    }
}