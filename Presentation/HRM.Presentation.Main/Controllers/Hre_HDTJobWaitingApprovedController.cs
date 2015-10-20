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

    public class Hre_HDTJobWaitingApprovedController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;

        public ActionResult Index()
        {
            return View();
        }
     
        /// <summary>
        /// Lấy tất cả dữ liệu trong table Hre_HDTJob
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 
        public ActionResult Get([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Hre_HDTJobModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_HDTJob/");
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Hre_HDTJobModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_HDTJobModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_HDTJob/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Hr_HDTJob_CreateSuccess.TranslateString();
            }
            return View();
        }

        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Hre_HDTJobModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_HDTJob/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Hre_HDTJobModel HDTJob)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_HDTJobModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_HDTJob/", HDTJob);
                ViewBag.MsgUpdate = "Update success";
            }
            return View();
        }

        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Hre_HDTJobModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Hre_HDTJob/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_HDTJobModel>(_hrm_Hr_Service, "api/Hre_HDTJob/",string.Join(",",selectedIds), ConstantPermission.Hre_HDTJob, DeleteType.Remove.ToString());
        }

        public ActionResult RegisterOutInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var service = new RestServiceClient<Hre_HDTJobModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_HDTJob/", id);
                return View(result);
            }
            else
            {
                return View();
            }
        }
    }
}