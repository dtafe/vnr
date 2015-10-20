﻿using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Service;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Controllers
{

    public class Hre_CandidateHistoryController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        //
        // GET: /CandidateHistory/
        public ActionResult Index()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Hre_CandidateHistory);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table CandidateHistory
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Hre_CandidateHistoryModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_CandidateHistory/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một CandidateHistory
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Hre_CandidateHistoryModel model)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_CandidateHistory);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_CandidateHistoryModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Hre_CandidateHistory/", model);
            return Json(result);
        }


        public ActionResult Remove(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Hre_CandidateHistory);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_CandidateHistoryModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Remove(_hrm_Hr_Service, "api/CandidateHistory/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_CandidateHistoryModel>(_hrm_Hr_Service, "api/Hre_CandidateHistory/",string.Join(",",selectedIds), ConstantPermission.Hre_CandidateHistory, DeleteType.Remove.ToString());
        }

        public ActionResult Create()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_CandidateHistory);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }
        [HttpPost,  ValidateInput(true)]
        public ActionResult Create(Hre_CandidateHistoryModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_CandidateHistoryModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_CandidateHistory/", model);
                ViewBag.MsgInsert = "Insert success";
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_CandidateHistory);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_CandidateHistoryModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_CandidateHistory/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Hre_CandidateHistoryModel CandidateHistory)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_CandidateHistoryModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_CandidateHistory/", CandidateHistory);
                ViewBag.MsgUpdate = "Update success";
            }
            return View();
        }
       

        //[System.Web.Mvc.HttpPost]
        //[System.Web.Mvc.RouteAttribute("api/Hre_CandidateHistory")]
        //public Hre_CandidateHistoryModel Post([Bind]Hre_CandidateHistoryModel model)
        //{
        //    ActionService service = new ActionService();
        //    return service.UpdateOrCreate<Hre_CandidateHistoryEntity, Hre_CandidateHistoryModel>(model);
        //}
        /// <summary>
        /// [Quoc.Do] Lấy đối tượng CandidateHistory theo ProfileID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CreateOrUpdateCandidateHistory(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_CandidateHistory);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_CandidateHistory);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var service = new RestServiceClient<Hre_CandidateHistoryModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_CandidateHistory/", id);
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