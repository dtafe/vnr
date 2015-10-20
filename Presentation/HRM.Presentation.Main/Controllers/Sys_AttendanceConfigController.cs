using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Security;
using HRM.Presentation.HrmSystem.Models;

namespace HRM.Presentation.Main.Controllers
{
    public class Sys_AttendanceConfigController : MainBaseController
    {
        readonly string _hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /SysAttendanceConfig/
        public ActionResult Index()
        {
          
            var service = new RestServiceClient<Sys_AttConfigModel>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hre_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Att_OvertimePermitConfig/", UserId);
            return View(result);
        }
        public ActionResult Create()
        {
            
            var service = new RestServiceClient<Sys_AttConfigModel>(UserLogin);
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