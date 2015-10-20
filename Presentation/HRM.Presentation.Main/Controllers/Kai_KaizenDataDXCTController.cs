using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Main.Controllers;
using HRM.Presentation.Payroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Security;

namespace HRM.Presentation.Main.Controllers
{
    public class Kai_KaizenDataDXCTController : MainBaseController
    {
        readonly string _Hrm_Kai_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Kai_KaizenDataDXCT/
        public ActionResult Index()
        {
            return View();
        }
	}
}