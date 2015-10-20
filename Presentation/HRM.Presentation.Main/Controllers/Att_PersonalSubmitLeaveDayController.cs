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

namespace HRM.Presentation.Main.Controllers
{
    public class Att_PersonalSubmitLeaveDayController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table AttLeaveday
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Att_LeaveDayModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_Leaveday/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một AttLeaveday
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Att_LeaveDayModel model)
        {
            var service = new RestServiceClient<Att_LeaveDayModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Att_Leaveday/", model);
            return Json(result);
        }


        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Att_LeaveDayModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Remove(_Hrm_Hre_Service, "api/Att_Leaveday/", id);
            return Json(result);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult Create(Att_LeaveDayModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_LeaveDayModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_Leaveday/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Attendance_LeaveDay_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Att_LeaveDayModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_Leaveday/", id);
            if (result != null)
            {
                result.HoursFrom = result.DateStart;
                result.HoursTo = result.DateEnd;
            }
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Att_LeaveDayModel AttLeaveday)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_LeaveDayModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_Leaveday/", AttLeaveday);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Attendance_LeaveDay_UpdateSuccess.TranslateString();
            }
            return View();
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Att_LeaveDayModel>(_Hrm_Hre_Service, "api/Att_Leaveday/", selectedIds, ConstantPermission.Att_LeaveDay, DeleteType.Remove.ToString());
        }

        /// <summary>
        /// Xử lí thay doi trang thai cua Att_Leaveday
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>      
        public ActionResult SetStatusSelected(List<Guid> selectedIds, string status, string userApproved)
        {
            var model = new Att_LeaveDayModel();
            if (selectedIds != null || selectedIds.Count>0)
            {

                var service = new RestServiceClient<Att_LeaveDayModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                model.ProfileIds = String.Join(",", selectedIds);
                model.Status = status;
                Guid Approved = Guid.Empty;
                Guid.TryParse(userApproved, out Approved);
                if (Approved == Guid.Empty)
                {
                    model.ActionStatus = "NoPermission";
                    return Json(model);
                }
                model.UserApproveID = Approved;
                model.UserApproveID2 = Approved;
                model = service.Put(_Hrm_Hre_Service, "api/Att_Leaveday/", model);
            }
            return Json(model);
        }
        /// <summary>
        /// Cập nhật tổng số ngày nghĩ
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        public ActionResult UpdateSumLeaveday(List<Guid> selectedIds)
        {
            var leaveDay = new Att_LeaveDayModel();
            if (selectedIds != null && selectedIds.Count>0)
            {
                leaveDay.lstLeaveIDs = selectedIds;
                var service = new RestServiceClient<Att_LeaveDayModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                leaveDay = service.Post(_Hrm_Hre_Service, "api/Att_LeavedayCustom/", leaveDay);
            }
            return Json(leaveDay);
        }
	}
}