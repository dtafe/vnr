using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Category.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Security;

namespace HRM.Presentation.Main.Controllers
{
    public class Att_ComputeLeaveLateEarlyController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        public ActionResult Index()
        {
            var serviceCat_LeaveDayType = new RestServiceClient<IEnumerable<CatLeaveDayTypeModel>>(UserLogin);
            serviceCat_LeaveDayType.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var resultCat_LeaveDayType = serviceCat_LeaveDayType.Get(_Hrm_Hre_Service, "api/CatLeaveDayType/");
            ViewData["Cat_LeaveDayType"] = resultCat_LeaveDayType;

            var service = new RestServiceClient<IEnumerable<CatShiftMultiModel>>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var catShift = service.Get(_Hrm_Hre_Service, "api/CatShift/");
            ViewData["Cat_Shift"] = catShift; 
            return View();
        }
	}
}