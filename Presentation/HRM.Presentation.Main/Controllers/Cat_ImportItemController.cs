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
    public class Cat_ImportItemController : MainBaseController
    {

        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /CatImportItem/
        public ActionResult Index()
        {
          
            //var catProvince = GetAllCatProvince();
            //ViewData["catProvince"] = catProvince;
            return View();
        }

       
        /// <summary>
        /// Lấy tất cả dữ liệu trong table CatImportItem
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAllCatImportItem([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<CatImportItemModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatImportItem/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Lấy tất cả dữ liệu cho control multiselect
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult GetMultiselect()
        {
            var service = new RestServiceClient<IEnumerable<CatImportItemModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatImportItem/");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Tạo mời một CatImportItem
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] CatImportItemModel model)
        {
           
            var service = new RestServiceClient<CatImportItemModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Post(_hrm_Hr_Service, "api/CatImportItem/", model);
            return Json(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
          

            string selectedIds = string.Join(",",id.ToString());
            return RemoveOrDeleteAndReturn<CatImportItemModel>(_hrm_Hr_Service, "api/CatImportItem/", selectedIds, ConstantPermission.Cat_ImportItem, DeleteType.Remove.ToString());

            //return Json(result);
        }

    }
}