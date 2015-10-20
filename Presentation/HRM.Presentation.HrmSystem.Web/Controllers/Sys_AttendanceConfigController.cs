using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Security;
using HRM.Presentation.HrmSystem.Models;

namespace HRM.Presentation.HrmSystem.Web.Controllers
{
    public class Sys_AttendanceConfigController : BaseController
    {
        readonly string _hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /SysAttendanceConfig/
        public ActionResult Index()
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sys_AttendanceConfig);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_AttConfigModel>();
            service.SetCookies(Request.Cookies, _hrm_Hre_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Att_OvertimePermitConfig/", UserId);
            return View(result);
        }
        public ActionResult Create()
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sys_AttendanceConfig);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_AttConfigModel>();
            service.SetCookies(Request.Cookies, _hrm_Hre_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Att_OvertimePermitConfig/", UserId);
            if (result != null)
            {
                return View(result);
            }
            return View();
        }

        // Att_OvertimePermit
        public ActionResult Att_OvertimePermit()
        {
            return View();
        }

	}
}