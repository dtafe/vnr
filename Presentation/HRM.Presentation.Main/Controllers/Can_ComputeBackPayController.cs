using System.Web.Mvc;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using HRM.Infrastructure.Security;
using HRM.Presentation.Canteen.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Presentation.Main.Controllers;
using VnResource.Helper.Security;
using System.IO;
using System.Linq;
using HRM.Presentation.Service;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Can_ComputeBackPayController : MainBaseController
    {
        private readonly string _hrm_Can_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            return View();
        }

    
    }

}