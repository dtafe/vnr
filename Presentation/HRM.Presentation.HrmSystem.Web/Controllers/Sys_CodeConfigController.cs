using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;

namespace HRM.Presentation.HrmSystem.Web.Controllers
{
    /// <summary>Cấu hình sinh mã tự động (phiên bản mới)</summary>
    public class Sys_CodeConfigController : BaseController
    {
        private readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sys_CodeConfigInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var guidId = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_CodeConfigModel>();
                service.SetCookies(Request.Cookies, _hrm_Sys_Service);
                var modelResult = service.Get(_hrm_Sys_Service, "api/Sys_CodeConfig/", guidId);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sys_CodeConfigModel>(_hrm_Sys_Service, "api/Sys_CodeConfig/", selectedIds, ConstantPermission.Sys_CodeConfig, DeleteType.Remove.ToString());
        }
	}
}