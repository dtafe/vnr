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
    public class Tra_PlanDetailController : MainBaseController
    {
        private readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TraPlanDetailInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Tra_PlanDetailModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Tra_PlanDetail/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
     
        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Tra_PlanDetailModel>(_hrm_Hr_Service, "api/Tra_PlanDetail/", selectedIds, ConstantPermission.Tra_PlanDetail, DeleteType.Remove.ToString());
        }
    }
}