using HRM.Infrastructure.Utilities;
using HRM.Presentation.Payroll.Models;
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
namespace HRM.Presentation.Main.Controllers
{
    public class Sal_InsuranceSalaryController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        //
        // GET: /AttGrade/
        public ActionResult Index()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Att_Grade);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table AttGrade
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Sal_InsuranceSalaryModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Sal_InsuranceSalary/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một AttGrade
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Sal_GradeModel model)
        {
            var service = new RestServiceClient<Sal_InsuranceSalaryModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Sal_InsuranceSalary/", model);
            return Json(result);
        }

        public ActionResult Remove(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Att_Grade);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
            var service = new RestServiceClient<Sal_InsuranceSalaryModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Sal_InsuranceSalary/", id);
            return Json(result);
        }
        public ActionResult Create()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Att_Grade);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult Create(Sal_InsuranceSalaryModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Sal_InsuranceSalaryModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Sal_InsuranceSalary/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Att_Grade_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Att_Grade);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Sal_InsuranceSalaryModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Sal_InsuranceSalary/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Sal_InsuranceSalaryModel Sal_Grade)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Att_Grade);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Sal_InsuranceSalaryModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Sal_InsuranceSalary/", Sal_Grade);
                //return Json(result);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Att_Grade_UpdateSuccess.TranslateString();
            }
            return View();
        }

        public ActionResult SalInsuranceSalaryInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sal_CostCentre);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sal_CostCentre);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sal_InsuranceSalaryModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Sal_InsuranceSalary/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sal_InsuranceSalaryModel>(_hrm_Hr_Service, "api/Sal_InsuranceSalary/", String.Join(",", selectedIds), ConstantPermission.Sal_InsuranceSalary, DeleteType.Remove.ToString());
        }
	}
}