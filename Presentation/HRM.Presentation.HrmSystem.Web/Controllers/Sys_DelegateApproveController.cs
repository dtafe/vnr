using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.HrmSystem.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System;
using System.Linq;

namespace HRM.Presentation.HrmSystem.Web.Controllers
{
    public class Sys_DelegateApproveController : BaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /Sys_DelegateApprove/
        public ActionResult Index()
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sys_DelegateApprove);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            return View();
        }

        public ActionResult SysDelegateInfo(string id)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_DelegateApprove);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_DelegateApproveModel>();
                service.SetCookies(Request.Cookies, _hrm_Sys_Service);
                var modelResult = service.Get(_hrm_Sys_Service, "api/Sys_DelegateApprove/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Lấy tất cả dữ liệu trong table Sys_DelegateApprove
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Get([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Sys_DelegateApproveModel>>();
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_DelegateApprove/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mới một Sys_DelegateApprove
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] Sys_DelegateApproveModel model)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_DelegateApprove);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_DelegateApproveModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_DelegateApprove/", model);
            return Json(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Sys_DelegateApprove);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_DelegateApproveModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Delete(_hrm_Sys_Service, "api/Sys_DelegateApprove/", id);
            return Json(result);
        }

        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public ActionResult Edit(Guid id)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sys_DelegateApprove);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_DelegateApproveModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_DelegateApprove/", id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Sys_DelegateApproveModel user)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sys_DelegateApprove);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }

            var service = new RestServiceClient<Sys_DelegateApproveModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_DelegateApprove/", user);
            return Json(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            if (selectedIds != null)
            {
                var service = new RestServiceClient<Sys_DelegateApproveModel>();
                service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                RemoveOrDeleteAndReturn<Sys_DelegateApproveModel>(_hrm_Sys_Service, "api/Sys_DelegateApprove/", selectedIds, ConstantPermission.Sys_DelegateApprove, DeleteType.Remove.ToString());
            }
            return Json("");
        }
    }
}