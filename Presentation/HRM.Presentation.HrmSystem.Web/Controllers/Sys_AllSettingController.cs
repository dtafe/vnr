using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Presentation.HrmSystem.Models;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Business.Main.Domain;
using System;
using System.Linq;

namespace HRM.Presentation.HrmSystem.Web.Controllers
{
    public class Sys_AllSettingController : BaseController
    {

        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /Setting/
        public ActionResult Index()
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sys_AllSetting);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            return View();
        }

        public ActionResult HomeIndex()
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sys_AllSetting);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            return View();
        }
        public ActionResult Sys_AllSettingInfo(string id)
        {
            bool isAccess;
            if (!string.IsNullOrEmpty(id))
            {
                isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sys_AllSetting);
            }
            else
            {
                isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_AllSetting);
            }
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_AllSettingModel>();
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
            var service = new RestServiceClient<IEnumerable<Sys_AllSettingModel>>();
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
            var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_AllSetting);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_AllSettingModel>();
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
            var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Sys_AllSetting);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_AllSettingModel>();
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
            var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sys_AllSetting);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_AllSettingModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_AllSetting/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Sys_AllSettingModel model)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sys_AllSetting);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Sys_AllSettingModel>();
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
            var service = new RestServiceClient<Dictionary<string, string>>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Post(_hrm_Sys_Service, "api/Sys_AllSetting/", lstParam);
            return View(result);
        }
    }
}