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
    public class Can_CateringController : MainBaseController
    {
        readonly string _hrm_Can_Service = ConstantPathWeb.Hrm_Hre_Service;        
        // GET: /Can_Catering/
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table Can_Catering
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Can_CateringModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Can_Service);
            var result = service.Get(_hrm_Can_Service, "api/Can_Catering/");
            return Json(result.ToDataSourceResult(request));
        }

        public ActionResult CanteringInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Can_CateringModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Can_Service);
                var result = service.Get(_hrm_Can_Service, "api/Can_Cantering/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Tạo mời một Can_Catering
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Can_CateringModel model)
        {
            var service = new RestServiceClient<Can_CateringModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Put(_hrm_Can_Service, "api/Can_Catering/", model);
            return Json(result);
        }  
       
        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Can_CateringModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Delete(_hrm_Can_Service, "api/Can_Catering/", id);
            return Json(result);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Can_CateringModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Can_CateringModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
                var result = service.Put(_hrm_Can_Service, "api/Can_Catering/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Canteen_Catering_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Can_CateringModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Get(_hrm_Can_Service, "api/Can_Catering/", id);            
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Can_CateringModel Can_Catering)
        {
            if(ModelState.IsValid)
            {
                var service = new RestServiceClient<Can_CateringModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
                var result = service.Put(_hrm_Can_Service, "api/Can_Catering/", Can_Catering);
                //return Json(result);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Canteen_Catering_UpdateSuccess.TranslateString();
            }
            return View();
        }
    
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Can_CateringModel>(_hrm_Can_Service, "api/Can_Catering/", selectedIds,
                ConstantPermission.Can_Catering, DeleteType.Remove.ToString());
        }

        public ActionResult CreateOrUpdate(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Can_CateringModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Can_Service);
                var modelResult = service.Get(_hrm_Can_Service, "api/Can_Catering/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
    }
    
}