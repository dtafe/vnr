using System.Collections.Generic;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using HRM.Infrastructure.Security;
using VnResource.Helper.Security;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
namespace HRM.Presentation.HrmSystem.Web.Controllers
{
    public class Sys_ProcessApprovedController : BaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;

        public ActionResult Index()
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sys_ProcessApproved);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            return View();
        }


        public ActionResult Create()
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_ProcessApproved);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult Create(Sys_ProcessApprovedModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Sys_ProcessApprovedModel>();
                service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                var result = service.Put(_hrm_Sys_Service, "api/Sys_ProcessApproved/", model);
                ViewBag.MsgInsert = "Insert success";
            }
            return View();
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Sys_ProcessApproved);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_ProcessApprovedModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Delete(_hrm_Sys_Service, "api/Sys_ProcessApproved/", id);
            return Json(result);
        }

        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public ActionResult Edit(Guid id)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sys_ProcessApproved);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_ProcessApprovedModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_ProcessApproved/", id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit([Bind] Sys_ProcessApprovedModel group1)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sys_ProcessApproved);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_ProcessApprovedModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_ProcessApproved/", group1);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Sys_ProcessApprovedModel model)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sys_ProcessApproved);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_ProcessApprovedModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_ProcessApproved/", model);
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sys_ProcessApprovedModel>(_hrm_Sys_Service, "api/Sys_ProcessApproved/", selectedIds, ConstantPermission.Sys_ProcessApproved, DeleteType.Remove.ToString());
        }

        public ActionResult SysProcessApprovedInfo(string id)
        {
            bool isAccess;
            if (!string.IsNullOrEmpty(id))
            {
                isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sys_ProcessApproved);
            }
            else
            {
                isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_ProcessApproved);
            }
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_ProcessApprovedModel>();
                service.SetCookies(Request.Cookies, _hrm_Sys_Service);
                var modelResult = service.Get(_hrm_Sys_Service, "api/Sys_ProcessApproved/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

    }
}