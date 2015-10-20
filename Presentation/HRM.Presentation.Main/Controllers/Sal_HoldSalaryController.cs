using HRM.Infrastructure.Utilities;
using HRM.Presentation.Payroll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Presentation.Main.Controllers
{
    public class Sal_HoldSalaryController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Sal_ReportSalaryTableMonth/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Analyze()
        {
            return View();
        }

        public ActionResult ComputeHoldSalary()
        {
            return View();
        }
	}
}