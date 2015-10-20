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
using System;
using System.Linq;
namespace HRM.Presentation.Main.Controllers
{
    public class Att_TimeSheetController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table AttTimeSheet
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Att_TimeSheetModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_TimeSheet/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một AttTimeSheet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Att_TimeSheetModel model)
        {
            var service = new RestServiceClient<Att_TimeSheetModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Att_TimeSheet/", model);
            return Json(result);
        }


        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Att_TimeSheetModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Delete(_Hrm_Hre_Service, "api/Att_TimeSheet/", id);
            return Json(result);
        }

        /// <summary>
        /// Tạo một record AttTimeSheet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult Create(Att_TimeSheetModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_TimeSheetModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_TimeSheet/", model);
                ViewBag.MsgInsert = "Insert success";
            }
            return View();
        }

        /// <summary>
        /// Sữa đỗi một record AttTimeSheet
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Att_TimeSheetModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_TimeSheet/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Att_TimeSheetModel AttTimeSheet)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_TimeSheetModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_TimeSheet/", AttTimeSheet);
                //return Json(result);
                ViewBag.MsgUpdate = "Update success";
            }
            return View();
        }

        public ActionResult Att_TimeSheetInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Att_TimeSheetModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Att_TimeSheet/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Att_TimeSheetModel>(_Hrm_Hre_Service, "api/Att_TimeSheet/", selectedIds, ConstantPermission.Att_TimeSheet, DeleteType.Remove.ToString());
        }
	}
}