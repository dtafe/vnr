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
    public class Cat_ImportCandidateController : MainBaseController
    {
        public const string Route = "api/CatImport/";
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /CatImport/
        public ActionResult Index()
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

        public ActionResult Import_ListCandidate()
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