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

namespace HRM.Presentation.Main.Controllers
{
    public class Cat_ElementCommissionController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        //
        // GET: /CatElement/
        public ActionResult Index()
        {

            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table CatElement
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<CatElementModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatElement/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một CatElement
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] CatElementModel model)
        {

            var service = new RestServiceClient<CatElementModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/CatElement/", model);
            return Json(result);
        }


        public ActionResult Remove(string id)
        {

            var service = new RestServiceClient<CatElementModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/CatElement/", id);
            return Json(result);
        }
        public ActionResult Create(string x)
        {
            if (x == null)
            {

                return View();
            }
            else
            {

                return View();
            }
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(CatElementModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<CatElementModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/CatElement/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Category_Element_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(string id)
        {

            var id1 = Common.ConvertToGuid(id);
            var service = new RestServiceClient<CatElementModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatElement/", id1);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, CatElementModel CatElement)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<CatElementModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/CatElement/", CatElement);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Category_Element_UpdateSuccess.TranslateString();
            }
            return View();
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatElementModel>(_hrm_Hr_Service, "api/CatElement/", selectedIds, ConstantPermission.Cat_Element, DeleteType.Remove.ToString());

        }

        /// <summary>
        /// Load Gird for Page Cat_Element tabstrip
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult GridCatElement(string type)
        {
            return View(type);
        }
	}
}