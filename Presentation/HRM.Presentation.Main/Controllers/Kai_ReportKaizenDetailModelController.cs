using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Controllers
{

    public class Kai_ReportKaizenDetailController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Kai_ReportKaizenDetail/
        public ActionResult Index()
        {
            return View();
        }
    }
}