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

    public class Hre_RelativesController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        //
        // GET: /Hre_Relatives/
        public ActionResult Index()
        {
          
            return View();
           
        }

        public ActionResult RelativesInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_RelativesModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Hre_Relatives/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult Hre_RelativesInfo(Guid id)
        {
            
            if (id != null && id != Guid.Empty)
            {
                //var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_RelativesModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_Relatives/", id);
                return View(result);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table Hre_Relatives
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 
        public ActionResult Get([DataSourceRequest] DataSourceRequest request)        
        {
            var service = new RestServiceClient<IEnumerable<Hre_RelativesModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_Relatives/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Lấy Relatives theo profileId
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 

        public ActionResult GetByProfileId()
        {
           
            return View();

        }

        //public ActionResult GetByProfileIdGrid([DataSourceRequest] DataSourceRequest request, int id)
        //{
        //    var service = new RestServiceClient<IEnumerable<Hre_RelativesModel>>(UserLogin);
        //    service.SetCookies(Request.Cookies, _hrm_Hr_Service);
        //    var result = service.Get(_hrm_Hr_Service, "api/Hre_RelativesCustom/", id);
        //    return Json(result.ToDataSourceResult(request));

        //}

        /// <summary>
        /// Tạo mới một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add([Bind] Hre_RelativesModel model)
        {
           
            var service = new RestServiceClient<Hre_RelativesModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Hre_Relatives/", model);
            return Json(result);
        }

        public ActionResult Create()
        {
          
            return View();
        }

        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Hre_RelativesModel model)
        {
          
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_RelativesModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_Relatives/", model);
                ViewBag.MsgInsert = "Insert success";
            }
            return View();
        }


        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
       
        public ActionResult Edit(Guid id)
        {
          
            var service = new RestServiceClient<Hre_RelativesModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_Relatives/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Hre_RelativesModel Relatives)
        {
           
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_RelativesModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_Relatives/", Relatives);
                //return Json(result);
                ViewBag.MsgUpdate = "Update success";
            }
            return View();
        }

        public ActionResult Remove(Guid id)
        {
           
            var service = new RestServiceClient<Hre_RelativesModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Hre_Relatives/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_RelativesModel>(_hrm_Hr_Service, "api/Hre_Relatives/", selectedIds, ConstantPermission.Hre_Relatives, DeleteType.Remove.ToString());
        }
    }
    
}