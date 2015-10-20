using System.Web.Mvc;
using HRM.Presentation.Recruitment.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Controllers
{
    public class Rec_SourceAdsController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;

        // GET: /Rec_SourceAds/
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult Rec_SourceAdsInfo(string id) 
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_SourceAdsModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_SourceAds/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Lấy tất cả dữ liệu trong table Rec_SourceAds
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Get([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Rec_SourceAdsModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Rec_SourceAds/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mới một Rec_Tag
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] Rec_SourceAdsModel model)
        {

            var service = new RestServiceClient<Rec_SourceAdsModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Rec_SourceAds/", model);
            return Json(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {

            var service = new RestServiceClient<Rec_SourceAdsModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Delete(_Hrm_Hre_Service, "api/Rec_SourceAds/", id);
            return Json(result);
        }

        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {

            var service = new RestServiceClient<Rec_SourceAdsModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Rec_SourceAds/", id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Rec_TagModel user)
        {

            var service = new RestServiceClient<Rec_SourceAdsModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Rec_SourceAds/", user);
            return Json(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
           
            var user = new List<Rec_SourceAdsModel>();
            if (selectedIds != null)
            {
                //var ids = selectedIds
                //    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                //    .Select(x => Common.ConvertToGuid(x))
                //    .ToArray();
                var service = new RestServiceClient<Rec_SourceAdsModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                foreach (var id in selectedIds)
                {
                    service.Delete(_Hrm_Hre_Service, "api/Rec_SourceAds/", id);
                }
            }
            return Json("");
        }
    }
}