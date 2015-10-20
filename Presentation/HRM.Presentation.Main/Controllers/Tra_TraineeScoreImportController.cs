using System.Collections;
using System.Web.Mvc;
using System.Web.Routing;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;

namespace HRM.Presentation.Main.Controllers
{
    public class Tra_TraineeScoreImportController : MainBaseController
    {
        public const string Route = "api/CatImport/";
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /CatImport/
        public ActionResult Index()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Cat_Import);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}

            //var serviceCat_Shift = new RestServiceClient<IEnumerable<CatChildFieldsMultiModel>>();
            //serviceCat_Shift.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            //var catShift = serviceCat_Shift.Get(_hrm_Hr_Service, "Cat__GetData/GetMultiChildField/");
            //ViewData["Cat_Shift"] = catShift;


            return View();
        }

        public ActionResult Tab_ImportScore()
        {
            return View();
        }


        /// <summary>
        /// Lấy tất cả dữ liệu trong table CatCountry
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAllCatImport([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<CatImportModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatImport/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Lấy tất cả dữ liệu cho control multiselect
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult GetMultiselect()
        {
            var service = new RestServiceClient<IEnumerable<CatImportModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatImport/");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult Create(CatImportModel model)
        {

            var service = new RestServiceClient<CatImportModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Post(_hrm_Hr_Service, "api/CatImport/", model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {

            var service = new RestServiceClient<CatImportModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/CatImport/", Common.ConvertToGuid(id));
            return Json(result);
        }

        public ActionResult CatImportInfo(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatImportModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/CatImport/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }


        public ActionResult ImportItemInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatImportModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/CatImportItem/", idModel);
                return View(result);
            }
            else
            {
                return View();
            }
        }

        public ActionResult Edit(Guid id)
        {

            var service = new RestServiceClient<CatImportModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatImport/", id);
            return View(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {

            var service = new RestServiceClient<CatImportModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Remove(_hrm_Hr_Service, "api/CatImport/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatImportModel>(_hrm_Hr_Service, "api/CatImport/", selectedIds, ConstantPermission.Cat_Import, DeleteType.Remove.ToString());

        }

        public ActionResult Import_List()
        {
            TempData["PreController"] = "Tra_TraineeScore";
            return View();
        }
        public ActionResult Tab_Import()
        {
            return View();
        }
    

        public ActionResult ListImportData()
        {
            return View();
        }

        public ActionResult ListInvalidData()
        {
            return View();
        }
    }
}