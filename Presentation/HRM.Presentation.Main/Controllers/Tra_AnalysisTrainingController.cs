﻿using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using HRM.Presentation.Main.Controllers;
using HRM.Presentation.Training.Models;
namespace HRM.Presentation.Main.Web.Controllers
{
    public class Tra_AnalysisTrainingController : MainBaseController
    {
        private readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(string CodeEmps, DateTime? DateSeniority)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Att_Grade);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }
    }
}