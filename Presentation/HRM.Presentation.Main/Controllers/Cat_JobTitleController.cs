using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
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

    public class Cat_JobTitleController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        //
        // GET: /CatJobTitle/
        public ActionResult Index()
        {
          
            return View();
        }
        public ActionResult CreateOrUpdate(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_JobTitleModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Cat_JobTitle/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }

        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table CatJobTitle
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAllCatJobTitle([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Cat_JobTitleModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_JobTitle/");
            return Json(result.ToDataSourceResult(request));
        }
        public ActionResult Create()
        {
            
            return View();
        }
        /// <summary>
        /// Tạo mời một CatJobTitle
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Cat_JobTitleModel model)
        {
            
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Cat_JobTitleModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Cat_JobTitle/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Category_JobTitle_CreateSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
           
            var service = new RestServiceClient<Cat_JobTitleModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_JobTitle/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Cat_JobTitleModel catJobTitle)
        {
           
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Cat_JobTitleModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Cat_JobTitle/", catJobTitle);
                //return Json(result);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Category_JobTitle_UpdateSuccess.TranslateString();
            }
            return View();
        }      
      
        public ActionResult Delete(Guid id)
        {
          
            var service = new RestServiceClient<Cat_JobTitleModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Cat_JobTitle/", id);
            return Json(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {
         
            var service = new RestServiceClient<Cat_JobTitleModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Cat_JobTitle/", id);
            return Json(result);
        }
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Cat_JobTitleModel>(_hrm_Hr_Service, "api/Cat_JobTitle/", selectedIds, ConstantPermission.Cat_JobTitle, DeleteType.Remove.ToString());
         

        } 
    }    
}