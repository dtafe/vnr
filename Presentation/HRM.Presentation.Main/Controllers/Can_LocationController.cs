using System.Web.Mvc;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using HRM.Infrastructure.Security;
using HRM.Presentation.Canteen.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;

using VnResource.Helper.Security;
using System.IO;
using System.Linq;
using HRM.Presentation.Service;
using HRM.Presentation.Main.Controllers;
//using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Can_LocationController : MainBaseController
    {
        readonly string _hrm_Can_Service = ConstantPathWeb.Hrm_Hre_Service;        
        // GET: /Can_Location/
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table Can_Location
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Can_LocationModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Can_Service);
            var result = service.Get(_hrm_Can_Service, "api/Can_Location/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một Can_Location
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Can_LocationModel model)
        {
            var service = new RestServiceClient<Can_LocationModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Put(_hrm_Can_Service, "api/Can_Location/", model);
            return Json(result);
        }

        public ActionResult LocationInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Can_LocationModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Can_Service);
                var result = service.Get(_hrm_Can_Service, "api/Can_Location/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }



        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Can_LocationModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Delete(_hrm_Can_Service, "api/Can_Location/", id);
            return Json(result);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Can_LocationModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Can_LocationModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
                var result = service.Put(_hrm_Can_Service, "api/Can_Location/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Canteen_Location_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Can_LocationModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Get(_hrm_Can_Service, "api/Can_Location/", id);            
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Can_LocationModel Can_Location)
        {
            if(ModelState.IsValid)
            {
                var service = new RestServiceClient<Can_LocationModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
                var result = service.Put(_hrm_Can_Service, "api/Can_Location/", Can_Location);
                //return Json(result);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Canteen_Location_UpdateSuccess.TranslateString();
            }
            return View();
        }
        



        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Can_LocationModel>(_hrm_Can_Service, "api/Can_Location/", selectedIds,
               ConstantPermission.Can_Location, DeleteType.Remove.ToString());
        }
    }
    
}