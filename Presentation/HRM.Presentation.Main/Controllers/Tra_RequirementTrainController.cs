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
    public class Tra_RequirementTrainController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;

        // GET: /Cat_TrainCategory/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Tra_RequirementTrainModel>(_hrm_Hr_Service, "api/Tra_RequirementTrain/", selectedIds, ConstantPermission.Tra_RequirementTrain, DeleteType.Remove.ToString());
        }

        public ActionResult Create(Tra_RequirementTrainModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Tra_RequirementTrainModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Tra_RequirementTrain/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Attendance_Overtime_CreateSuccess.TranslateString();
            }
            return View();
        }
        //public ActionResult Edit(Tra_RequirementTrainModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var service = new RestServiceClient<Tra_RequirementTrainModel>();
        //        service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
        //        var result = service.Put(_hrm_Hr_Service, "api/Tra_RequirementTrain/", model);
        //        ViewBag.MsgUpdate = ConstantDisplay.HRM_Attendance_Overtime_UpdateSuccess.TranslateString();
        //    }
        //    return View();
        //}
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Tra_RequirementTrainModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Tra_RequirementTrain/", id);
            return View(result);
        }
    }
}