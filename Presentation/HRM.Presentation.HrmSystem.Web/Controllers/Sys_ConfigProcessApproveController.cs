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

namespace HRM.Presentation.HrmSystem.Web.Controllers
{
    public class Sys_ConfigProcessApproveController : BaseController
    {
        private readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        public ActionResult Index()
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sys_ConfigProcessApprove);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            return View();
        }

      

        public ActionResult SysConfigPApproveInfo(string id) 
        {
            bool isAccess;
            if (!string.IsNullOrEmpty(id))
            {
                isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sys_ConfigProcessApprove);
            }
            else
            {
                isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_ConfigProcessApprove);
            }
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_ConfigProcessApproveModel>();
                service.SetCookies(Request.Cookies, _hrm_Sys_Service);
                var modelResult = service.Get(_hrm_Sys_Service, "api/Sys_ConfigProcessApprove/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// [Phuoc.Le] - Chuyển trạng thái IsDelete của các record được chọn sang True
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sys_ConfigProcessApproveModel>(_hrm_Sys_Service, "api/Sys_ConfigProcessApprove/", string.Join(",", selectedIds), ConstantPermission.Sys_ConfigProcessApprove, DeleteType.Remove.ToString());
        }
    }
}