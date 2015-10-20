using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Service;
using HRM.Presentation.Payroll.Models;

namespace HRM.Presentation.Main.Controllers
{
    public class Sal_SalaryDepartmentController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Sal_SalaryDepartment/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {

            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Sal_SalaryDepartmentModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Sal_SalaryDepartmentModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Sal_SalaryDepartment/", model);
                ViewBag.MsgInsert = "Insert success";
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {

            var service = new RestServiceClient<Sal_SalaryDepartmentModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Sal_SalaryDepartment/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Sal_SalaryDepartmentModel Contract)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Sal_SalaryDepartmentModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Sal_SalaryDepartment/", Contract);
                ViewBag.MsgUpdate = "Update success";
            }
            return View();
        }

        public ActionResult RemoveSelected(string selectedIds)
        {

            return RemoveOrDeleteAndReturn<Sal_SalaryDepartmentModel>(_hrm_Hr_Service, "api/Sal_SalaryDepartment/", selectedIds, ConstantPermission.Hre_ProfileAll, DeleteType.Remove.ToString());
        }

        public ActionResult RemoveSelectedItem(string selectedIds)
        {

            return RemoveOrDeleteAndReturn<Sal_SalaryDepartmentModel>(_hrm_Hr_Service, "api/Sal_SalaryDepartmentItem/", selectedIds, ConstantPermission.Hre_ProfileAll, DeleteType.Remove.ToString());
        }

        //public ActionResult SalaryDepartmentItemInfo(string id)
        //{

        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        var id1 = Common.ConvertToGuid(id);
        //        var service = new RestServiceClient<Sal_SalaryDepartmentItemModel>();
        //        service.SetCookies(Request.Cookies, _hrm_Hr_Service);
        //        var modelResult = service.Get(_hrm_Hr_Service, "api/Sal_SalaryDepartmentItem/", id1);
        //        return View(modelResult);
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}

        public ActionResult SalaryDepartmentItemInfo(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sal_SalaryDepartmentItemModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Sal_SalaryDepartmentItem/", idModel);
                if (result.SalaryDepartmentID == Guid.Empty)
                    result.SalaryDepartmentID = idModel;

                return View(result);
            }
            else
            {
                if (Request["ContractID"] != null)
                {
                    string s = Request["ContractID"].ToString();
                    ViewBag.contractID = s;
                }
                return View();
            }
        }
	}
}