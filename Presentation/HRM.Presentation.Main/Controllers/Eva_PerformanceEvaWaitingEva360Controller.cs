using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Presentation.Service;
using Kendo.Mvc.UI;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Evaluation.Models;

namespace HRM.Presentation.Main.Controllers
{
    /// <summary>Chờ Đánh Giá 360</summary>
    public class Eva_PerformanceEvaWaitingEva360Controller : MainBaseController
    {
        readonly string _Hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PerformanceEvaWaitingEvaInfo360(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var service = new RestServiceClient<Eva_PerformanceEvaModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hr_Service);
                var result = service.Get(_Hrm_Hr_Service, "api/Eva_PerformanceEva/", id);

                return View(result);
            }
            else
            {

                return View();
            }
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Eva_PerformanceEvaModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Eva_PerformanceEvaModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hr_Service);
                var result = service.Post(_Hrm_Hr_Service, "api/Eva_PerformanceEva/", model);
                ViewBag.MsgInsert = "Insert success";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return View();
        }


        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Eva_PerformanceEvaModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hr_Service);
            var result = service.Get(_Hrm_Hr_Service, "api/Eva_PerformanceEva/", id);
            ViewBag.MsgUpdate = "Update success";

            return View(result);
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Save([Bind(Prefix = "performanceEvaModel")] Eva_PerformanceEvaModel model, [Bind(Prefix = "models")] List<Eva_PerformanceEvaDetailModel> PerformanceEvaDetailModels)
        {
            if (model != null && PerformanceEvaDetailModels != null)
            {
                model.PerformanceEvaDetails = PerformanceEvaDetailModels;
            }

            var service = new RestServiceClient<Eva_PerformanceEvaModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hr_Service);
            var result = service.Post(_Hrm_Hr_Service, "api/Eva_PerformanceEva/", model);
            ViewBag.MsgInsert = "Insert success";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}