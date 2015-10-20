using System.Collections.Generic;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using HRM.Infrastructure.Security;
using VnResource.Helper.Security;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
namespace HRM.Presentation.Main.Controllers
{
    public class Sys_ConditionApprovedController : MainBaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        
        public ActionResult Index()
        {
            
            return View();
        }


        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult Create(Sys_ConditionApprovedModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Sys_ConditionApprovedModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                var result = service.Put(_hrm_Sys_Service, "api/Sys_ConditionApproved/", model);
                ViewBag.MsgInsert = "Insert success";
            }
            return View();
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
           
            var service = new RestServiceClient<Sys_ConditionApprovedModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Delete(_hrm_Sys_Service, "api/Sys_ConditionApproved/", id);
            return Json(result);
        }

        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public ActionResult Edit(Guid id)
        {
            
            var service = new RestServiceClient<Sys_ConditionApprovedModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_ConditionApproved/", id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit([Bind] Sys_ConditionApprovedModel group1)
        {
            
            var service = new RestServiceClient<Sys_ConditionApprovedModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_ConditionApproved/", group1);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Sys_ConditionApprovedModel model)
        {
            var service = new RestServiceClient<Sys_ConditionApprovedModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_ConditionApproved/", model);
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sys_ConditionApprovedModel>(_hrm_Sys_Service, "api/Sys_ConditionApproved/", selectedIds, ConstantPermission.Sys_ConditionApproved, DeleteType.Remove.ToString());
        }

        public ActionResult SysConditionApprovedInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_ConditionApprovedModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Sys_Service);
                var modelResult = service.Get(_hrm_Sys_Service, "api/Sys_ConditionApproved/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

    }
}