using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Presentation.Service;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Evaluation.Models;


namespace HRM.Presentation.Main.Controllers
{
    public class Eva_PerformanceEvaWaitingEvaController : MainBaseController
    {
        readonly string _Hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Eva_PerformanceEvaWaitingEva);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            
            return View();
        }
       
        public ActionResult PerformanceEvaWaitingEvaInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Eva_PerformanceEvaWaitingEva);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Eva_PerformanceEvaWaitingEva);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
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
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Eva_PerformanceEvaWaitingEva);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Eva_PerformanceEvaModel model)
        {
            //bool isAccess;
            // isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Eva_PerformanceEvaWaitingEva);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
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
        [HttpPost]
        public ActionResult CheckOrder(Guid id)
        {
            var isAllow = true;
            var service = new RestServiceClient<Eva_PerformanceEvaModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hr_Service);
            var result = service.Get(_Hrm_Hr_Service, "api/Eva_PerformanceEva/", id);
            if (result != null && result.IsEvaluation.HasValue && result.IsEvaluation == false)
            {
                result = null;
            }
            if (result == null)
            {
                isAllow = false;
            }
            return Json(isAllow, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]

        public ActionResult Edit(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Eva_PerformanceEvaWaitingEva);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Eva_PerformanceEvaModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hr_Service);
            var result = service.Get(_Hrm_Hr_Service, "api/Eva_PerformanceEva/", id);
            ViewBag.MsgUpdate = "Update success";
            
            return View(result);
        }
         [HttpPost, ValidateInput(true)]
        public ActionResult Save([Bind(Prefix = "performanceEvaModel")] Eva_PerformanceEvaModel model, [Bind(Prefix = "models")] List<Eva_PerformanceEvaDetailModel> PerformanceEvaDetailModels)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Eva_PerformanceEvaWaitingEva);
            //if (!isAccess)
            //{
            //    return PartialView(ConstantMessages.AccessDenied);
            //}

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