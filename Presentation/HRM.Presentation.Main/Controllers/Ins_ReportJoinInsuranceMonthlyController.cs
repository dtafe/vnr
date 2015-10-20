using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Presentation.Service;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Insurance.Models;

namespace HRM.Presentation.Main.Controllers
{
    /// <summary> BC Tham Gia Bảo Hiểm Tháng </summary>
    public class Ins_ReportJoinInsuranceMonthlyController : MainBaseController
    {
        public ActionResult Index()
        {           
            return View();
        }
       
	}
}