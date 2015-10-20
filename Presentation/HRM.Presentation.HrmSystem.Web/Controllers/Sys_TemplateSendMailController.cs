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
    public class Sys_TemplateSendMailController : BaseController
    {
        private readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        public ActionResult Index()
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sys_TemplateSendMail);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            return View();
        }

        public ActionResult SysTemplateSendMailInfo(string id) 
        {
            bool isAccess;
            if (!string.IsNullOrEmpty(id))
            {
                isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sys_TemplateSendMail);
            }
            else
            {
                isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_TemplateSendMail);
            }
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_TemplateSendMailModel>();
                service.SetCookies(Request.Cookies, _hrm_Sys_Service);
                var modelResult = service.Get(_hrm_Sys_Service, "api/Sys_TemplateSendMail/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }


        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sys_TemplateSendMailModel>(_hrm_Sys_Service, "api/Sys_TemplateSendMail/", selectedIds, ConstantPermission.Sys_TemplateSendMail, DeleteType.Remove.ToString());
        }

    }
}