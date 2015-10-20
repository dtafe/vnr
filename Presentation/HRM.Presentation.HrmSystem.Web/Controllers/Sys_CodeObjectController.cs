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
    public class Sys_CodeObjectController : BaseController
    {
        private readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /Sys_CodeObject/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sys_CodeObjectInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var guidId = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_CodeObjectModel>();
                service.SetCookies(Request.Cookies, _hrm_Sys_Service);
                var modelResult = service.Get(_hrm_Sys_Service, "api/Sys_CodeObject/", guidId);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sys_CodeObjectModel>(_hrm_Sys_Service, "api/Sys_CodeObject/", selectedIds, ConstantPermission.Sys_User, DeleteType.Remove.ToString());
        }
	}
}