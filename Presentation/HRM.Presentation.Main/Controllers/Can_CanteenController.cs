using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using HRM.Infrastructure.Security;
using HRM.Presentation.Canteen.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;

using VnResource.Helper.Security;
using System;
using HRM.Presentation.Main.Controllers;

//using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Can_CanteenController : MainBaseController
    {
        readonly string _hrm_Can_Service = ConstantPathWeb.Hrm_Hre_Service;
        // GET: /Can_Canteen/
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table Can_Canteen
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Can_CanteenModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Can_Service);
            var result = service.Get(_hrm_Can_Service, "api/Can_Canteen/");
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult CanteenInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Can_CanteenModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Can_Service);
                var result = service.Get(_hrm_Can_Service, "api/Can_Canteen/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Tạo mời một Can_Canteen
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Can_CanteenModel model)
        {
            var service = new RestServiceClient<Can_CanteenModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Put(_hrm_Can_Service, "api/Can_Canteen/", model);
            return Json(result);
        }

        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Can_CanteenModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Delete(_hrm_Can_Service, "api/Can_Canteen/", id);
            return Json(result);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Can_CanteenModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Can_CanteenModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
                var result = service.Put(_hrm_Can_Service, "api/Can_Canteen/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Canteen_Canteen_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Can_CanteenModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Get(_hrm_Can_Service, "api/Can_Canteen/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Can_CanteenModel Can_Canteen)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Can_CanteenModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
                var result = service.Put(_hrm_Can_Service, "api/Can_Canteen/", Can_Canteen);
                //return Json(result);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Canteen_Canteen_UpdateSuccess.TranslateString();
            }
            return View();
        }
        
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Can_CanteenModel>(_hrm_Can_Service, "api/Can_Canteen/", selectedIds,
                ConstantPermission.Can_Canteen, DeleteType.Remove.ToString());
        }

        public ActionResult CreateOrUpdate(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Can_CanteenModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Can_Service);
                var modelResult = service.Get(_hrm_Can_Service, "api/Can_Canteen/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
    }

}