using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using HRM.Infrastructure.Security;
using VnResource.Helper.Security;

namespace HRM.Presentation.Main.Controllers
{
    public class Att_ReportMonthlyRosterController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReportMonthlyRoster([DataSourceRequest] DataSourceRequest request, Att_ReportMonthlyRosterModel model)
        {
            var service = new RestServiceClient<IEnumerable<Att_ReportMonthlyRosterModel>>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Post(_Hrm_Hre_Service, "api/Att_ReportMonthlyRoster/", model);

            return Json(result.ToDataSourceResult(request));
        }
	}
}