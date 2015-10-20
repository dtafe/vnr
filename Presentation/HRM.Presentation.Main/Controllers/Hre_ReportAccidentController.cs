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
    public class Hre_ReportAccidentController : MainBaseController
    {
        //
        // GET: /Hre_ReportAccident/
        public ActionResult Index()
        {
           
            return View();
        }
	}
}