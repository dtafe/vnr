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
    public class Att_AnnualLeaveController : MainBaseController
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
                var service = new RestServiceClient<Att_AnnualLeaveModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Att_AnnualLeave/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }

        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table AttAnnualLeave
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Att_AnnualLeaveModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_AnnualLeave/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một AttAnnualLeave
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Att_AnnualLeaveModel model)
        {
            var service = new RestServiceClient<Att_AnnualLeaveModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Att_AnnualLeave/", model);
            return Json(result);
        }


        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Att_AnnualLeaveModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Delete(_Hrm_Hre_Service, "api/Att_AnnualLeave/", id);
            return Json(result);
        }

        /// <summary>
        /// Tạo một record AttAnnualLeave
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult Create(Att_AnnualLeaveModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_AnnualLeaveModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_AnnualLeave/", model);
                ViewBag.MsgInsert = "Insert success";
            }
            return View();
        }

        /// <summary>
        /// Sữa đỗi một record AttAnnualLeave
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Att_AnnualLeaveModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_AnnualLeave/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Att_AnnualLeaveModel AttAnnualLeave)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_AnnualLeaveModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_AnnualLeave/", AttAnnualLeave);
                //return Json(result);
                ViewBag.MsgUpdate = "Update success";
            }
            return View();
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Att_AnnualLeaveModel>(_Hrm_Hre_Service, "api/Att_AnnualLeave/", selectedIds, ConstantPermission.Att_AnnualLeave, DeleteType.Remove.ToString());
        }
	}
}