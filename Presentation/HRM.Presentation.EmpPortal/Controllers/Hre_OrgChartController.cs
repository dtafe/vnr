using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Presentation.EmpPortal.Controllers
{
    public class Hre_OrgChartController : BasePortalController
    {
        public ActionResult Index()
        {
            return GetView();
        }

        public ActionResult OrgChart()
        {
            return GetView();
        }
	}
}