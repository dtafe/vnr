using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Presentation.Service;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Insurance.Models;

namespace HRM.Presentation.Main.Controllers
{
    public class Ins_ChildSickController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {           
            return View();
        }
        public ActionResult ChildSickInfo(string id)
        {     
            if (!string.IsNullOrEmpty(id))
            {
                var service = new RestServiceClient<Ins_ChildSickModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var result = service.Get(_Hrm_Hre_Service, "api/Ins_ChildSick/", id);
                return View(result);
            }
            else
            {
                return View();
            }
        }
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Ins_ChildSickModel>(_Hrm_Hre_Service, "api/Ins_ChildSick/", selectedIds, ConstantPermission.Ins_ChildSick, DeleteType.Remove.ToString());
        }
	}
}