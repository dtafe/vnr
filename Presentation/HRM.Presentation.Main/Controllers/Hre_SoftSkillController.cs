using HRM.Infrastructure.Utilities;
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
    public class Hre_SoftSkillController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        //
        // GET: /AttInOut/
        public ActionResult Index()
        {
          
            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table AttInOut
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Hre_SoftSkillModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_SoftSkill/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một AttInOut
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Hre_SoftSkillModel model)
        {
           
            var service = new RestServiceClient<Hre_SoftSkillModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Hre_SoftSkill/", model);
            return Json(result);
        }


        public ActionResult Remove(Guid id)
        {
           
            var service = new RestServiceClient<Hre_SoftSkillModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Hre_SoftSkill/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_SoftSkillModel>(_hrm_Hr_Service, "api/Hre_SoftSkill/",string.Join(",",selectedIds), ConstantPermission.Hre_SoftSkill, DeleteType.Remove.ToString());
        }

        public ActionResult Create()
        {
          
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Hre_SoftSkillModel model)
        {
           
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_SoftSkillModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_SoftSkill/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_HR_SoftSkill_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
           
            var service = new RestServiceClient<Hre_SoftSkillModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_SoftSkill/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Hre_SoftSkillModel SoftSkill)
        {
           
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_SoftSkillModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_SoftSkill/", SoftSkill);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_HR_SoftSkill_UpdateSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult SoftSkillInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_SoftSkillModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Hre_SoftSkill/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
    }
}