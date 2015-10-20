using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;

using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;
namespace HRM.Presentation.Main.Controllers
{
    public class Att_TAMController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table AttTAMScanLog
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Att_TAMModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_TAM/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một AttTAMScanLog
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Att_TAMModel model)
        {
            var service = new RestServiceClient<Att_TAMModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Att_TAM/", model);
            return Json(result);
        }
        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Att_TAMModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Remove(_Hrm_Hre_Service, "api/Att_TAM/", id);
            return Json(result);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Att_TAMModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_TAMModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_TAM/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Attendance_TAMScanLog_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Att_TAMModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_TAM/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Att_TAMModel Att_TAMData)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_TAMModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_TAM/", Att_TAMData);
                //return Json(result);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Attendance_TAMScanLog_UpdateSuccess.TranslateString();
            }
            return View();
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            var TAMScanLog = new List<Att_TAMModel>();
            if (selectedIds != null)
            {
                var service = new RestServiceClient<Att_TAMModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                foreach (var id in selectedIds)
                {
                    service.Delete(_Hrm_Hre_Service, "api/Att_TAM/", id);
                }
            }
            return Json("");
        }

	}
}