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
    public class Att_CutOffDurationController : MainBaseController
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
                var service = new RestServiceClient<Att_CutOffDurationModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Att_CutOffDuration/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table Att_CutOffDuration
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Att_CutOffDurationModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_CutOffDuration/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một Att_CutOffDuration
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Att_CutOffDurationModel model)
        {
            var service = new RestServiceClient<Att_CutOffDurationModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Att_CutOffDuration/", model);
            return Json(result);
        }


        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Att_CutOffDurationModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Delete(_Hrm_Hre_Service, "api/Att_CutOffDuration/", id);
            return Json(result);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Att_CutOffDurationModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_CutOffDurationModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_CutOffDuration/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Attendance_CutOffDuration_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Att_CutOffDurationModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_CutOffDuration/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Att_CutOffDurationModel AttTAMScanLog)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_CutOffDurationModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_CutOffDuration/", AttTAMScanLog);
                //return Json(result);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Attendance_CutOffDuration_UpdateSuccess.TranslateString();
            }
            return View();
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Att_CutOffDurationModel>(_Hrm_Hre_Service, "api/Att_CutOffDuration/", selectedIds, ConstantPermission.Att_CutOffDuration, DeleteType.Remove.ToString());
        }

	}
}