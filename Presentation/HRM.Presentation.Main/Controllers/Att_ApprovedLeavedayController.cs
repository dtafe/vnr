using HRM.Infrastructure.Utilities;
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
using System.IO;

namespace HRM.Presentation.Main.Controllers
{
    public class Att_ApprovedLeavedayController : MainBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProcessApprovedPage(Guid loginID, Guid recordID)
        {
            ViewBag.Login = loginID;
            ViewBag.Record = recordID;
            return View();
        }

        public ActionResult ProcessRejectPage(Guid loginID, Guid recordID)
        {
            ViewBag.Login = loginID;
            ViewBag.Record = recordID;
            return View();
        }
	}
}