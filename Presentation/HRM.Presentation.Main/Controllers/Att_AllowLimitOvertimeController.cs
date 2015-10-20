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
    public class Att_AllowLimitOvertimeController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateOrUpdate(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Att_AllowLimitOvertimeModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Att_AllowLimitOvertime/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table Att_AllowLimitOvertime
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Att_AllowLimitOvertimeModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_AllowLimitOvertime/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một Att_AllowLimitOvertime
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Att_AllowLimitOvertimeModel model)
        {
            var service = new RestServiceClient<Att_AllowLimitOvertimeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Att_AllowLimitOvertime/", model);
            return Json(result);
        }


        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Att_AllowLimitOvertimeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Delete(_Hrm_Hre_Service, "api/Att_AllowLimitOvertime/", id);
            return Json(result);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Att_AllowLimitOvertimeModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_AllowLimitOvertimeModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_AllowLimitOvertime/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Att_AllowLimitOvertime_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Att_AllowLimitOvertimeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_AllowLimitOvertime/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Att_AllowLimitOvertimeModel AttAllowLimitOver)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_AllowLimitOvertimeModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_AllowLimitOvertime/", AttAllowLimitOver);
                //return Json(result);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Att_AllowLimitOvertime_UpdateSuccess.TranslateString();
            }
            return View();
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Att_AllowLimitOvertimeModel>(_Hrm_Hre_Service, "api/Att_AllowLimitOvertime/", selectedIds, ConstantPermission.Att_AllowLimitOvertime, DeleteType.Remove.ToString());
        }
	}
}