using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Infrastructure.Security;
using HRM.Presentation.Payroll.Models;

namespace HRM.Presentation.Main.Controllers
{
    public class Sal_ProductSalaryController : MainBaseController
    {
        //
        // GET: /Sal_ProductSalary/
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sal_ProductSalaryInfo()
        {
            return View();
        }
        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sal_ProductSalaryModel>(_hrm_Hr_Service, "api/Sal_ProductSalary/", selectedIds, ConstantPermission.Sal_ProductSalary, DeleteType.Remove.ToString());
        }
	}
}