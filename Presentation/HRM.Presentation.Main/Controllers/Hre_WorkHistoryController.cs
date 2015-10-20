using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Controllers
{

    public class Hre_WorkHistoryController : MainBaseController
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
        /// Lấy tất cả dữ liệu trong WorkHistory
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Hre_WorkHistoryModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_WorkHistory/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một WorkHistory
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Hre_WorkHistoryModel model)
        {

            var service = new RestServiceClient<Hre_WorkHistoryModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Hre_WorkHistory/", model);
            return Json(result);
        }


        public ActionResult Remove(Guid id)
        {

            var service = new RestServiceClient<Hre_WorkHistoryModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Hre_WorkHistory/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_WorkHistoryModel>(_hrm_Hr_Service, "api/Hre_WorkHistory/", string.Join(",", selectedIds), ConstantPermission.Hre_WorkHistory, DeleteType.Remove.ToString());

        }
        //public ActionResult RemoveSelected(List<Guid> selectedIds)
        //{

        //    var user = new List<Hre_WorkHistoryModel>(UserLogin);
        //    if (selectedIds != null)
        //    {
        //        var service = new RestServiceClient<Hre_WorkHistoryModel>(UserLogin);
        //        service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
        //        foreach (var id in selectedIds)
        //        {
        //            service.Delete(_hrm_Hr_Service, "api/Hre_WorkHistory/", id);
        //        }
        //    }
        //    return Json("");
        //}


        public ActionResult Create()
        {

            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Hre_WorkHistoryModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_WorkHistoryModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_WorkHistory/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_HR_SoftSkill_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {

            var service = new RestServiceClient<Hre_WorkHistoryModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_WorkHistory/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Hre_WorkHistoryModel WorkHistory)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_WorkHistoryModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_WorkHistory/", WorkHistory);
                ViewBag.MsgInsert = ConstantDisplay.HRM_HR_SoftSkill_InsertSuccess.TranslateString();
            }
            return View();
        }
    }

}