using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Presentation.HrmSystem.Models;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Business.Main.Domain;
using System;
using System.Linq;
using System.Xml;

namespace HRM.Presentation.HrmSystem.Web.Controllers
{
    public class Sys_SQLCommanderController : BaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /Sys_CustomReport/
        public ActionResult Index()
        {
            return View();
        }

  

    }
}