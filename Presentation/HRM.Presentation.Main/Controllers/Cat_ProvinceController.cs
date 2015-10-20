using System.Linq;
using System.Web.Mvc;
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
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Cat_ProvinceController : MainBaseController
    {

        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /CatProvince/
        public ActionResult Index()
        {
            
            return View();
        }


        /// <summary>
        /// Lấy tất cả dữ liệu trong table CatProvince
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAllCatProvince([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<CatProvinceModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatProvince/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Lấy tất cả dữ liệu cho control multiselect
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMultiselect()
        {
            var service = new RestServiceClient<IEnumerable<CatProvinceModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatProvince/");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAll()
        {
            var service = new RestServiceClient<IEnumerable<CatProvinceModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatProvince/");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Tạo mời một CatProvince
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] CatProvinceModel model)
        {
          
            var service = new RestServiceClient<CatProvinceModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/CatProvince/", model);
            return Json(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {
           
            var service = new RestServiceClient<CatProvinceModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/CatProvince/", id);
            return Json(result);
        }

        public ActionResult CreateOrUpdate(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatProvinceModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/CatProvince/", idModel);
                return View(result);
            }
            else
            {
                return View();
            }
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatProvinceModel>(_hrm_Hr_Service, "api/CatProvince/", selectedIds, ConstantPermission.Cat_Province, DeleteType.Remove.ToString());
   
        }
    }
}