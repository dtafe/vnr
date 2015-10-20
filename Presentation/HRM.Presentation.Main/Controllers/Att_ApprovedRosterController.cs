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
using System.IO;
namespace HRM.Presentation.Main.Controllers
{
    public class Att_ApprovedRosterController : MainBaseController
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