using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Cat_ExportController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /CatExport/
        public ActionResult Index()
        {
            
            return View();
        }


        /// <summary>
        /// Lấy tất cả dữ liệu trong table CatExport
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAllCatExport([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<CatExportModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatExport/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Lấy tất cả dữ liệu cho control multiselect
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult GetMultiselect()
        {
            var service = new RestServiceClient<IEnumerable<CatExportModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatExport/");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {
           
            var service = new RestServiceClient<CatExportModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/CatExport/", id);
            return Json(result);
        }

        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Create(CatExportModel model)
        {
            
            var service = new RestServiceClient<CatExportModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Post(_hrm_Hr_Service, "api/CatExport/", model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(string id)
        {
           
            var service = new RestServiceClient<CatExportModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatExport/", id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, CatExportModel model)
        {
            
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<CatExportModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/CatExport/", model);
            }
            return View();
        }

        public ActionResult CreateOrUpdate(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatExportModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/CatExport/", idModel);
                return View(result);
            }
            else
            {
                return View();
            }
        }

        public ActionResult ExportItemInfo(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatExportItemModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/CatExportItem/", idModel);
                return View(result);
            }
            else
            {
                //ViewBag.ExportID = ExportID;
                return View();
            }
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatExportModel>(_hrm_Hr_Service, "api/CatExport/", selectedIds, ConstantPermission.Cat_Export, DeleteType.Remove.ToString());
        }
	}
}