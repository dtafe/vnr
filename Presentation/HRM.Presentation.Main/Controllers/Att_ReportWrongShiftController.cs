﻿using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;

namespace HRM.Presentation.Main.Controllers
{
    public class Att_ReportWrongShiftController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReportWrongShift([DataSourceRequest] DataSourceRequest request, Att_ReportWrongShiftModel model)
        {
            var service = new RestServiceClient<IEnumerable<Att_ReportWrongShiftModel>>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Post(_Hrm_Hre_Service, "api/Att_ReportWrongShift/", model);

            return Json(result.ToDataSourceResult(request));
        }
	}
}