using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;

using HRM.Infrastructure.Security;
using VnResource.Helper.Security;
using System.IO;
using HRM.Presentation.Category.Models;
namespace HRM.Presentation.Main.Controllers
{
    public class Att_RosterController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        
        public ActionResult Index()
        {
            var serviceCat_Shift = new RestServiceClient<IEnumerable<CatShiftMultiModel>>(UserLogin);
            var catShift = serviceCat_Shift.Get(_Hrm_Hre_Service, "api/CatShift/");
            ViewData["Cat_Shift"] = catShift;
            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table AttRoster
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Att_RosterModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_Roster/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một AttRoster
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Att_RosterModel model)
        {
            var service = new RestServiceClient<Att_RosterModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Att_Roster/", model);
            return Json(result);
        }

        public ActionResult Remove(Guid id)
        {
            return View();
            var service = new RestServiceClient<Att_RosterModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Delete(_Hrm_Hre_Service, "api/Att_Roster/", id);
            return Json(result);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult Create(Att_RosterModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_RosterModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_Roster/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Attendance_Roster_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Att_RosterModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_Roster/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Att_RosterModel AttRoster)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_RosterModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_Roster/", AttRoster);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Attendance_Roster_UpdateSuccess.TranslateString();
            }
            return View();
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds) 
        {
            return RemoveOrDeleteAndReturn<Att_RosterModel>(_Hrm_Hre_Service, "api/Att_Roster/", selectedIds, ConstantPermission.Att_Roster, DeleteType.Remove.ToString());

        }
        /// <summary>
        /// Xử lí thay doi trang thai cua roster
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>      
        public ActionResult SetStatusSelected(List<Guid> selectedIds, string status, string userApproved)
        {
            var roster = new Att_RosterModel();
            if (selectedIds != null || selectedIds.Count>0 )
            {
                var service = new RestServiceClient<Att_RosterModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                roster.ProfileIds = String.Join(",", selectedIds);
                roster.Status = status;
                Guid Approved = Guid.Empty;
                Guid.TryParse(userApproved, out Approved);
                if (Approved == Guid.Empty)
                {
                    roster.ActionStatus = "NoPermission";
                    return Json(roster);           
                }
                roster.UserApproveID = Approved;
                roster.UserApproveID2 = Approved;
                roster = service.Put(_Hrm_Hre_Service, "api/Att_Roster/", roster);
            }
            return Json(roster);           
        }

        public ActionResult CreateInlineAdjust([Bind(Prefix = "models")] List<Att_RosterModel> model)
        {
            var service = new RestServiceClient<Att_RosterModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            Att_RosterModel result = new Att_RosterModel();
            for (int i = 0; i < model.Count; i++)
            {
                result = service.Post(_Hrm_Hre_Service, "api/Att_Roster/", model[i]);
            }
            return Json(result);
        }
	}
}