using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Presentation.HrmSystem.Models;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;

using System;
using System.Linq;

namespace HRM.Presentation.Main.Controllers
{
    public class Sys_AllSettingController : MainBaseController
    {

        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /Setting/
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult HomeIndex()
        {
          
            return View();
        }
        public ActionResult Sys_AllSettingInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_AllSettingModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Sys_Service);
                var result = service.Get(_hrm_Sys_Service, "api/Sys_AllSetting/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table Setting
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAllSetting([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Sys_AllSettingModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_AllSetting/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một Setting
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] Sys_AllSettingModel model)
        {
           
            var service = new RestServiceClient<Sys_AllSettingModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_AllSetting/", model);
            return Json(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
           
            var service = new RestServiceClient<Sys_AllSettingModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Delete(_hrm_Sys_Service, "api/Sys_AllSetting/", id);
            return Json(result);
        }
        /// <summary>
        /// [Tam.Le] - 15.8.2014
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            
            var service = new RestServiceClient<Sys_AllSettingModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_AllSetting/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Sys_AllSettingModel model)
        {
            
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Sys_AllSettingModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                var result = service.Put(_hrm_Sys_Service, "api/Sys_AllSetting/", model);
                //return Json(result);
                //ViewBag.MsgUpdate = ConstantDisplay.HRM_Category_Position_UpdateSuccess.TranslateString();
            }
            return View();
        }


        public ActionResult ConfigDefault(string lstConfigIds)
        {
            List<Dictionary<string, string>> lstParam = new List<Dictionary<string, string>>();
            lstParam.Add(new Dictionary<string, string>() { { "lstConfigIds", lstConfigIds.ToString() } });
            var lstAppConfig = Enum.GetValues(typeof(AppConfig)).Cast<AppConfig>().ToList();

            foreach (var _config in lstAppConfig)
            {
                lstParam.Add(
                    new Dictionary<string, string>() 
                    { 
                        { _config.ToString() , (System.Configuration.ConfigurationManager.AppSettings[_config.ToString()] ?? string.Empty) } 
                    });
            }
            var service = new RestServiceClient<Dictionary<string, string>>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Post(_hrm_Sys_Service, "api/Sys_AllSetting/", lstParam);
            return View(result);
        }
    }
}