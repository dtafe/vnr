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
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using VnResource.Helper.Data;
using HRM.Presentation.Category.Models;
using HRM.Presentation.HrmSystem.Models;

namespace HRM.Presentation.Main.Controllers
{
    public class Att_OvertimeController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table AttOvertime
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Att_OvertimeModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_Overtime/");
            return Json(result.ToDataSourceResult(request));
        }
     
        /// <summary>
        /// Tạo mời một AttOvertime
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Att_OvertimeModel model)
        {
            var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Att_Overtime/", model);
            return Json(result);
        }

        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Delete(_Hrm_Hre_Service, "api/Att_Overtime/", id);            
            return Json(result);
        }
        public ActionResult Create()
        {
            return View();
        }
    
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Att_OvertimeModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_Overtime/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Attendance_Overtime_CreateSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_Overtime/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Att_OvertimeModel AttOvertime)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_Overtime/", AttOvertime);             
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Attendance_Overtime_UpdateSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Analysis()
        {
            var service = new RestServiceClient<IEnumerable<CatOvertimeTypeMultiModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/CatOvertimeType/");
            ViewData["Cat_OvertimeType"] = result;

            var service1 = new RestServiceClient<IEnumerable<Sys_UserMultiModel>>(UserLogin);
            service1.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result1 = service1.Get(_hrm_Sys_Service, "api/Sys_User/");
            ViewData["Sys_User"] = result1;

            return View();
        }
        public ActionResult AnalysisProfile(string ProfileID, string ProfileName)
        {
            var service = new RestServiceClient<IEnumerable<CatOvertimeTypeMultiModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/CatOvertimeType/");
            ViewData["Cat_OvertimeType"] = result;

            var service1 = new RestServiceClient<IEnumerable<Sys_UserMultiModel>>(UserLogin);
            service1.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result1 = service1.Get(_hrm_Sys_Service, "api/Sys_User/");
            ViewData["Sys_User"] = result1;

            Att_OvertimeAnalysisModel model = new Att_OvertimeAnalysisModel();
            model.ProfileID = ProfileID;
            model.ProfileName = ProfileName;
            return View(model);          
        }
        [HttpPost]
        public ActionResult Analysis([DataSourceRequest] DataSourceRequest request, Att_OvertimeAnalysisModel model)
        {
            var service = new RestServiceClient<IEnumerable<Att_OvertimeModel>>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Post(_Hrm_Hre_Service, "api/Att_Overtime/", model);
            return Json(result.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewDetail(int id)
        {
            ViewBag.ProfileId = id;
            return View();
        }

        public ActionResult GetByProfileIdGrid([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            var service = new RestServiceClient<IEnumerable<Att_OvertimeModel>>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_OvertimeCustom/", id);
            return Json(result.ToDataSourceResult(request));
        }
      
        [HttpPost]
        public ActionResult MultiCreate([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Att_OvertimeModel> attOvertime)
        {
            var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            if (attOvertime != null)
            {
                foreach (var item in attOvertime)
                {
                    service.Put(_Hrm_Hre_Service, "api/Att_Overtime/", item);
                }
            }
            return Json(attOvertime);
        }

        [HttpPost]
        public ActionResult GetOvertimeList([DataSourceRequest] DataSourceRequest request, Att_OvertimeModel model123)
        {

            return View();
        }

        /// <summary>
        /// Tạo mời chỉnh sửa xác nhận tăng ca
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult CreateInline([Bind] Att_OvertimeModel model)
        {
            var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Att_Overtime/", model);
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Att_OvertimeModel>(_Hrm_Hre_Service, "api/Att_Overtime/", selectedIds, ConstantPermission.Att_ExceptionOvertime, DeleteType.Remove.ToString());
        }

        [HttpPost]
        public ActionResult CreateAnalysis([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Att_OvertimeModel> attOvertime)
        {
            var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            if (attOvertime != null)
            {
                foreach (var item in attOvertime)
                {
                    service.Put(_Hrm_Hre_Service, "api/Att_Overtime/", item);
                }
            }

            return Json(attOvertime);
        }
        /// <summary>
        /// Xử lí thay doi trang thai cua overtime
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>    
        /// 
        public ActionResult SetStatusSelected(List<Guid> selectedIds, string status, string userApproved)
        {
            var model = new Att_OvertimeModel();
            if (selectedIds != null)
            {
                var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
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
                model = service.Put(_Hrm_Hre_Service, "api/Att_Overtime/", model);
            }
            return Json(model);
        }

        public ActionResult SetMethodPaymentSelected(List<Guid> selectedIds, string status)
        {
            var model = new Att_OvertimeModel();
            if (selectedIds != null || selectedIds.Count >0)
            {

                var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                model.ProfileIds = String.Join(",", selectedIds);
                model.MethodPayment = status;
                model = service.Put(_Hrm_Hre_Service, "api/Att_Overtime/", model);
            }
            return Json(model);
        }

        public ActionResult SetAllowOvertimeSelected(List<Guid> selectedIds, string status)
        {
            var model = new Att_OvertimeModel();
            if (selectedIds != null || selectedIds.Count > 0)
            {

                var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                model.ProfileIds = String.Join(",", selectedIds);
                if (status == "true")
                    model.IsNonOvertime = false;
                else
                    model.IsNonOvertime = true;
                model = service.Put(_Hrm_Hre_Service, "api/Att_Overtime/", model);
            }
            return Json(model);
        }

        public ActionResult MethodPaymentInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Att_Overtime/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        

        public ActionResult AttOvertimeInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Att_Overtime/", id1);
                var result = modelResult.CopyData<Att_OvertimeUpdateModel>();
                return View(result);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// [Hien.Nguyen] Chức năng tách làm thêm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeMethodOverTime_Manual(Guid? id)
        {
            if (id == null)
                return null;
            var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var Ot = service.Get(_Hrm_Hre_Service, "api/Att_Overtime/", id.Value);
            if (Ot != null)
            {
                Ot.ProfileName = Ot.ProfileName + " (" + Ot.CodeEmp + ")";
                Ot.TimeStartOverTime = string.Format("{0:HH:mm:ss}", Ot.WorkDate);
                Ot.OvertimeTypeTimeOffRateLit = Ot.TimeOffInLieuRate != null ? (Ot.TimeOffInLieuRate * 100).ToString()+"%" : "0" + "%";     
                if (Ot.MethodPayment == MethodOption.E_TIMEOFF.ToString())
                {

                    if (Ot.Status == OverTimeStatus.E_APPROVED.ToString())
                    {
                        Ot.HourToTimeOff = (Ot.ApproveHours ?? 0);
                        if (Ot.OvertimeTypeID != Guid.Empty)
                        {
                            Ot.TimeOffReal = ((Ot.TimeOffInLieuRate ?? 0) * (Ot.ApproveHours ?? 0));
                        }
                        else
                        {
                            Ot.TimeOffReal = 0;
                        }
                    }
                    else if (Ot.Status == OverTimeStatus.E_CONFIRM.ToString())
                    {
                        Ot.HourToTimeOff = Ot.ConfirmHours;
                        if (Ot.OvertimeTypeID != Guid.Empty)
                        {
                            Ot.TimeOffReal = ((Ot.TimeOffInLieuRate ?? 0) * Ot.ConfirmHours);
                        }
                        else
                        {
                            Ot.TimeOffReal = 0;
                        }
                    }
                    else
                    {
                        Ot.HourToTimeOff = Ot.RegisterHours;
                        if (Ot.OvertimeTypeID != Guid.Empty)
                        {
                            Ot.TimeOffReal = ((Ot.TimeOffInLieuRate ?? 0) * Ot.RegisterHours);
                        }
                        else
                        {
                            Ot.TimeOffReal = 0;
                        }
                    }
                    Ot.TimeRegister = 0;

                }

                if (Ot.MethodPayment == MethodOption.E_CASHOUT.ToString() || string.IsNullOrEmpty(Ot.MethodPayment))
                {
                    if (Ot.Status == OverTimeStatus.E_APPROVED.ToString())
                    {
                        Ot.TimeRegister = (Ot.ApproveHours ?? 0);
                    }
                    else if (Ot.Status == OverTimeStatus.E_CONFIRM.ToString())
                    {
                        Ot.TimeRegister = Ot.ConfirmHours;
                    }
                    else
                    {
                        Ot.TimeRegister = Ot.RegisterHours;
                    }
                    Ot.HourToTimeOff = 0;
                    Ot.TimeOffReal = 0;
                }
            }
            return View(Ot);
        }

        /// <summary>
        /// [Kiet.Chung] Chức năng đăng ký nghỉ bù
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ChangeMethodOverTime_Manual_Leave(Guid? id)
        {
            if (id == null)
                return null;
            var service = new RestServiceClient<Att_OvertimeModel>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var Ot = service.Get(_Hrm_Hre_Service, "api/Att_Overtime/", id.Value);
            if (Ot != null)
            {
                Ot.ProfileName = Ot.ProfileName + " (" + Ot.CodeEmp + ")";
                Ot.TimeStartOverTime = string.Format("{0:HH:mm:ss}", Ot.WorkDate);
                Ot.OvertimeTypeTimeOffRateLit = Ot.TimeOffInLieuRate != null ? (Ot.TimeOffInLieuRate * 100).ToString() + "%" : "0" + "%";
            }
            return View(Ot);
        }
    }
}