﻿using HRM.Infrastructure.Utilities;
using HRM.Presentation.Hr.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Controllers
{
    public class Hre_LanguageController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        //
        // GET: /AttInOut/
        public ActionResult Index()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Hre_Qualification);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }

        /// <summary>
        /// Lấy tất cả dữ liệu trong table AttInOut
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Hre_ProfileLanguageLevelModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_ProfileLanguage/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một AttInOut
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Hre_ProfileLanguageLevelModel model)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Qualification);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_ProfileLanguageLevelModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Hre_ProfileLanguage/", model);
            return Json(result);
        }


        public ActionResult Remove(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Hre_Qualification);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_ProfileLanguageLevelModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Hre_ProfileLanguage/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_ProfileLanguageLevelModel>(_hrm_Hr_Service, "api/Hre_ProfileLanguage/",string.Join(",",selectedIds), 
                ConstantPermission.Hre_Qualification, DeleteType.Remove.ToString());
        }
        public ActionResult RemoveSelecteds(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_ProfileLanguageLevelModel>(_hrm_Hr_Service, "api/Hre_ProfileLanguage/", selectedIds,
                ConstantPermission.Hre_Qualification, DeleteType.Remove.ToString());
        }
        public ActionResult Create()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Qualification);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Hre_ProfileLanguageLevelModel model)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Qualification);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_ProfileLanguageLevelModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_ProfileLanguage/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Hr_Qualification_CreateSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Qualification);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_ProfileLanguageLevelModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_ProfileLanguage/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Hre_ProfileLanguageLevelModel Qualification)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Qualification);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_ProfileLanguageLevelModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_ProfileLanguage/", Qualification);
                //return Json(result);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Hr_Qualification_UpdateSuccess.TranslateString();
            }
            return View();
        }

        public ActionResult LanguageInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Qualification);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Qualification);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var  id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_ProfileLanguageLevelModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_ProfileLanguage/", id1);
                if (result.ProfileID == null)
                    result.ProfileID = id1;
                return View(result);
            }
            else
            {
                if (Request["profileID"] != null)
                {
                    string aaa = Request["profileID"].ToString();
                    if (!string.IsNullOrEmpty(aaa))
                    {
                        ViewBag.profileID = aaa;
                    }
                }
                return View();
            }
        }
    }
}