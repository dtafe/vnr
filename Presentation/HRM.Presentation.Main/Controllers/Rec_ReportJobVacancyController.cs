using HRM.Infrastructure.Security;
using HRM.Presentation.Main.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Security;

namespace HRM.Presentation.Main.Controllers
{
    public class Rec_ReportJobVacancyController : MainBaseController
    {
        // GET: /Rec_ReportJobVacancy/
        public ActionResult Index()
        {
            return View();
        }
	}
}