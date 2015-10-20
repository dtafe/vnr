using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;

using HRM.Infrastructure.Security;
using VnResource.Helper.Security;
using Kendo.Mvc.UI;
using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Main.Controllers
{
    public class Sal_ReportGroupPayrollCostCentreController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        // GET: /Sal_ReportGroupPayrollCostCentre/
        public ActionResult Index()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sal_ReportGroupPayrollCostCentre);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }
	}
}