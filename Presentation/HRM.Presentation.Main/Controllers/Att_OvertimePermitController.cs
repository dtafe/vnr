using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace HRM.Presentation.Main.Controllers
{
    public class Att_OvertimePermitController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;

        public ActionResult Index()
        {
            var service = new RestServiceClient<Att_OvertimePermitModel>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_OvertimePermitConfig/", UserId);
            return View(result);
        }
	}
}