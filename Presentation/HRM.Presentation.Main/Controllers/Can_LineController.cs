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
using HRM.Presentation.Main.Controllers;
//using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Can_LineController : MainBaseController
    {
        readonly string _hrm_Can_Service = ConstantPathWeb.Hrm_Hre_Service;        
        // GET: /Can_Line/
        public ActionResult Index()
        {

            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table Can_Line
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Can_LineModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Can_Service);
            var result = service.Get(_hrm_Can_Service, "api/Can_Line/");
            return Json(result.ToDataSourceResult(request));
        }
        public ActionResult Create()
        {

            return View();
        }

        public ActionResult Edit(Guid id)
        {

            var service = new RestServiceClient<Can_LineModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Get(_hrm_Can_Service, "api/Can_Line/", id);
            return View(result);
        }
        /// <summary>
        /// Tạo mời một Can_Line
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Can_LineModel model)
        {

            var service = new RestServiceClient<Can_LineModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Put(_hrm_Can_Service, "api/Can_Line/", model);
            return Json(result);
        }  
       
        public ActionResult Remove(Guid id)
        {

            var service = new RestServiceClient<Can_LineModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Delete(_hrm_Can_Service, "api/Can_Line/", id);
            return Json(result);
        }
        
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Can_LineModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Can_LineModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
                var result = service.Put(_hrm_Can_Service, "api/Can_Line/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Canteen_Line_InsertSuccess.TranslateString();
            }
            return View();
        }
      
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Can_LineModel Can_Line)
        {

            if(ModelState.IsValid)
            {
                var service = new RestServiceClient<Can_LineModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
                var result = service.Put(_hrm_Can_Service, "api/Can_Line/", Can_Line);
                //return Json(result);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Canteen_Line_UpdateSuccess.TranslateString();
            }
            return View();
        }
        
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Can_LineModel>(_hrm_Can_Service, "api/Can_Line/", selectedIds,
                ConstantPermission.Can_Line, DeleteType.Remove.ToString());
        }

        public ActionResult LineInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Can_LineModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Can_Service);
                var modelResult = service.Get(_hrm_Can_Service, "api/Can_Line/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
    }
    
}