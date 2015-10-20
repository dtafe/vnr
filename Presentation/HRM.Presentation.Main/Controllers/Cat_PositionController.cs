using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using HRM.Infrastructure.Security;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using System.IO;
using System.Linq;
using HRM.Presentation.Main.Controllers;
//using HRM.Infrastructure.Utilities;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Cat_PositionController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;        
        // GET: /CatPosition/
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult PositionInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatPositionModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/CatPosition/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table CatPosition
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<CatPositionModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatPosition/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một CatPosition
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] CatPositionModel model)
        {
           
            var service = new RestServiceClient<CatPositionModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/CatPosition/", model);
            return Json(result);
        }  
       
        public ActionResult Remove(Guid id)
        {
            
            var service = new RestServiceClient<CatPositionModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/CatPosition/", id);
            return Json(result);
        }
        public ActionResult Create()
        {
           
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(CatPositionModel model)
        {
           
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<CatPositionModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/CatPosition/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Category_Position_CreateSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            
            var service = new RestServiceClient<CatPositionModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatPosition/", id);            
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, CatPositionModel catPosition)
        {
            
            if(ModelState.IsValid)
            {
                var service = new RestServiceClient<CatPositionModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/CatPosition/", catPosition);
                //return Json(result);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Category_Position_UpdateSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatPositionModel>(_hrm_Hr_Service, "api/CatPosition/", selectedIds, ConstantPermission.Cat_Position,DeleteType.Remove.ToString());
       

        } 
    }
    
}