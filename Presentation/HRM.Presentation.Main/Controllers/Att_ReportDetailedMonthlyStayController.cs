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
    public class Att_ReportDetailedMonthlyStayController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;

        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult ReportLeaveMonth([DataSourceRequest] DataSourceRequest request, Att_ReportLeaveMonthModel model)
        //{
        //    var service = new RestServiceClient<IEnumerable<Att_ReportLeaveMonthModel>>(UserLogin);
        //    service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
        //    var result = service.Post(_Hrm_Hre_Service, "api/Att_ReportLeaveMonth/", model);

        //    return Json(result.ToDataSourceResult(request));
        //}
	}
}