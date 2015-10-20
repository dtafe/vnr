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
    public class Cat_GradeCfgController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Grade/
        public ActionResult Index()
        {
            
            return View();
        }

        /// <summary>
        /// Lấy tất cả dữ liệu trong table CatCountry
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAllGrade([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<CatGradeModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatGrade/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Lấy tất cả dữ liệu cho control multiselect
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult GetMultiselect()
        {
            var service = new RestServiceClient<IEnumerable<CatGradeModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatGrade/");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Tạo mời một CatCountry
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] CatGradeModel model)
        {
          
            var service = new RestServiceClient<CatGradeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/CatGrade/", model);
            return Json(result);
        }
    
        public ActionResult Delete(string id)
        {
          
            var service = new RestServiceClient<CatGradeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/CatGrade/", id);
            return Json(result);
        }

        public ActionResult GradeInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatGradeModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/CatGrade/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {
          
            var service = new RestServiceClient<CatCountryModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Remove(_hrm_Hr_Service, "api/CatGrade/", id);
            return Json(result);
        }


        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatGradeModel>(_hrm_Hr_Service, "api/CatGrade/", selectedIds, ConstantPermission.Cat_GradeCfg, DeleteType.Remove.ToString());
 
        }
    }
}