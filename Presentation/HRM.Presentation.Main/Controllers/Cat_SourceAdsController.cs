﻿using System.Web.Mvc;
using HRM.Presentation.Category.Models;
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

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Cat_SourceAdsController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;

        // GET: /Rec_SourceAds/
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult Cat_SourceAdsInfo(string id) 
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_SourceAdsModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Cat_SourceAds/", id1);
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
            var service = new RestServiceClient<IEnumerable<Cat_SourceAdsModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_SourceAds/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mới một Rec_Tag
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] Cat_SourceAdsModel model)
        {

            var service = new RestServiceClient<Cat_SourceAdsModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Cat_SourceAds/", model);
            return Json(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {

            var service = new RestServiceClient<Cat_SourceAdsModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Cat_SourceAds/", id);
            return Json(result);
        }

        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {

            var service = new RestServiceClient<Cat_SourceAdsModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_SourceAds/", id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Cat_SourceAdsModel user)
        {

            var service = new RestServiceClient<Cat_SourceAdsModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Cat_SourceAds/", user);
            return Json(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
         
            var user = new List<Cat_SourceAdsModel>();
            if (selectedIds != null)
            {
                //var ids = selectedIds
                //    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                //    .Select(x => Common.ConvertToGuid(x))
                //    .ToArray();
                var service = new RestServiceClient<Cat_SourceAdsModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                foreach (var id in selectedIds)
                {
                    service.Delete(_hrm_Hr_Service, "api/Cat_SourceAds/", id);
                }
            }
            return Json("");
        }
    }
}