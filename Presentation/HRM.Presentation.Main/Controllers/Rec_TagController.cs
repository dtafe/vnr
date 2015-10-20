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
    public class Rec_TagController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;     

        // GET: /Rec_Tag/
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult RecTagInfo(string id) 
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_TagModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_Tag/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Lấy tất cả dữ liệu trong table Rec_Tag
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Get([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Rec_TagModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Rec_Tag/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mới một Rec_Tag
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] Rec_TagModel model)
        {

            var service = new RestServiceClient<Rec_TagModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Rec_Tag/", model);
            return Json(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {

            var service = new RestServiceClient<Rec_TagModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Delete(_Hrm_Hre_Service, "api/Rec_Tag/", id);
            return Json(result);
        }

        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {

            var service = new RestServiceClient<Rec_TagModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Rec_Tag/", id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Rec_TagModel user)
        {

            var service = new RestServiceClient<Rec_TagModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Rec_Tag/", user);
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
           
            var user = new List<Rec_TagModel>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Common.ConvertToGuid(x))
                    .ToArray();
                var service = new RestServiceClient<Rec_TagModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                foreach (var id in ids)
                {
                    service.Delete(_Hrm_Hre_Service, "api/Rec_Tag/", id);
                }
            }
            return Json("");
        }
    }
}