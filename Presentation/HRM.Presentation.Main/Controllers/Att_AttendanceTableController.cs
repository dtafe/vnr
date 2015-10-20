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

namespace HRM.Presentation.Main.Controllers
{
    public class Att_AttendanceTableController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table Att_AttendanceTable
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Att_AttendanceTableModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_AttendanceTable/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một Att_AttendanceTable
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Att_AttendanceTableModel model)
        {
            var service = new RestServiceClient<Att_AttendanceTableModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Att_AttendanceTable/", model);
            return Json(result);
        }


        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Att_AttendanceTableModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Delete(_Hrm_Hre_Service, "api/Att_AttendanceTable/", id);
            return Json(result);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Att_AttendanceTableModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_AttendanceTableModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_AttendanceTable/", model);
                ViewBag.MsgInsert = "Insert success";
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Att_AttendanceTableModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_AttendanceTable/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Att_AttendanceTableModel Att_AttendanceTable)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_AttendanceTableModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_AttendanceTable/", Att_AttendanceTable);
                ViewBag.MsgUpdate = "Update success";
            }
            return View();
        }
	}
}