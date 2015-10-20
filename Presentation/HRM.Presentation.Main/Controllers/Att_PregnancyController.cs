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
using HRM.Presentation.Service;
namespace HRM.Presentation.Main.Controllers
{
    public class Att_PregnancyController : MainBaseController
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
                var service = new RestServiceClient<Att_PregnancyModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Att_Pregnancy/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }

        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table AttPregnancy
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Att_PregnancyModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_Pregnancy/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một AttPregnancy
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Att_PregnancyModel model)
        {
            var service = new RestServiceClient<Att_PregnancyModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Put(_Hrm_Hre_Service, "api/Att_Pregnancy/", model);
            return Json(result);
        }

        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Att_PregnancyModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Remove(_Hrm_Hre_Service, "api/Att_Pregnancy/", id);
            return Json(result);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Att_PregnancyModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_PregnancyModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_Pregnancy/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Attendance_Pregnancy_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Att_PregnancyModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Att_Pregnancy/", id);
            return View(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Att_PregnancyModel>(_Hrm_Hre_Service, "api/Att_Pregnancy/", selectedIds,
                ConstantPermission.Att_Pregnancy, DeleteType.Remove.ToString());
        }
    }
}