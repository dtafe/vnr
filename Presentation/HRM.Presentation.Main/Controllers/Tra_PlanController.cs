using System.Web.Mvc;
using HRM.Presentation.Training.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Tra_PlanController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;

        // GET: /Tra_Plan/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Tra_PlanModel>(_hrm_Hr_Service, "api/Tra_Plan/", selectedIds, ConstantPermission.Tra_Plan, DeleteType.Remove.ToString());
        }

        public ActionResult Create(Tra_PlanModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Tra_PlanModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Tra_Plan/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Attendance_Overtime_CreateSuccess.TranslateString();
            }
            return View();
        }
   
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Tra_PlanModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Tra_Plan/", id);
            return View(result);
        }
    }
}