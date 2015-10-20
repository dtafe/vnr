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
    public class Att_RosterGroupController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table AttRosterGroup
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Att_RosterGroupModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_RosterGroup/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một AttRosterGroup
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Att_RosterGroupModel model)
        {
            var service = new RestServiceClient<Att_RosterGroupModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Att_RosterGroup/", model);
            return Json(result);
        }


        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Att_RosterGroupModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Remove(_Hrm_Hre_Service, "api/Att_RosterGroup/", id);
            return Json(result);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Att_RosterGroupModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_RosterGroupModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_RosterGroup/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Attendance_RosterGroup_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Att_RosterGroupModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_RosterGroup/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Att_RosterGroupModel AttRosterGroup)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_RosterGroupModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_RosterGroup/", AttRosterGroup);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Attendance_RosterGroup_UpdateSuccess.TranslateString();
            }
            return View();
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Att_RosterGroupModel>(_Hrm_Hre_Service, "api/Att_RosterGroup/", selectedIds, ConstantPermission.Att_RosterGroup, DeleteType.Remove.ToString());
        }
        /// <summary>
        /// Xử lí thay doi trang thai cua roster group 
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>      
        public ActionResult SetStatusSelected(List<Guid> selectedIds, string status)
        {
            var rosterGroup = new Att_RosterGroupModel();
            if (selectedIds != null || selectedIds.Count > 0)
            {
                var service = new RestServiceClient<Att_RosterGroupModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                rosterGroup.ProfileIds = Common.DotNetToOracle(String.Join(",", selectedIds));
                rosterGroup.Status = status;
                service.Put(_Hrm_Hre_Service, "api/Att_RosterGroup/", rosterGroup);
            }
            return Json("");
        } 
    }
}