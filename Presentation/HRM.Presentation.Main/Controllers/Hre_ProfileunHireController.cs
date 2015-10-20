using System.IO;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Presentation.Attendance.Models;
using System;
using System.Linq;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.Xml.Linq;
using System.Xml;
using HRM.Presentation.Main.Controllers;


namespace HRM.Presentation.Main.Controllers
{
    public class Hre_ProfileunHireController : MainBaseController
    {
        //
        // GET: /Hre_ProfileunHire/
        public ActionResult Index()
        {
            return View();
        }
	}
}