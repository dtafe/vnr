using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Presentation.Main.Controllers
{
    public class Sal_ReportPlanningPayrollController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        readonly string _Hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /Sal_ReportPlanningPayroll/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReportPlanningPayrollConfigElementIfno(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sal_ItemForShop);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sal_ItemForShop);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_AllSettingModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Sys_Service);
                var modelResult = service.Get(_Hrm_Sys_Service, "api/Sal_ItemForShop/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
	}
}