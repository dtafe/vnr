using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Presentation.HrmSystem.Web.Controllers
{
    public class Sys_LockObjectItemController : BaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /Sys_LockObjectItem/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sys_LockObjectItemModel>(_hrm_Sys_Service, "api/Sys_LockObjectItem/", string.Join(",", selectedIds), ConstantPermission.Sys_LockObject, DeleteType.Remove.ToString());
        }
	}
}