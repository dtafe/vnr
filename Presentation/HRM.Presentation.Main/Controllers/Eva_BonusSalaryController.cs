using System.Web.Mvc;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Evaluation.Models;
using HRM.Presentation.HrmSystem.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Evaluation.Models;

namespace HRM.Presentation.Main.Controllers
{
    public class Eva_BonusSalaryController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Eva_BonusSalary);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }
        public ActionResult EvaBonusSalaryInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Eva_BonusSalary);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Eva_BonusSalary);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var service = new RestServiceClient<Eva_BonusSalaryModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Eva_BonusSalary/", id);
                return View(result);
            }
            else
            {
                return View();
            }
        }
       
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Eva_BonusSalaryModel>(_hrm_Hr_Service, "api/Eva_BonusSalary/", selectedIds, ConstantPermission.Eva_BonusSalary, DeleteType.Remove.ToString());
        }

        #region Create/Edit

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Eva_BonusSalary);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Eva_BonusSalaryModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Eva_BonusSalary/", id);
            ViewBag.MsgInsert = "Insert success";
            return View(result);
        }

        public ActionResult Create()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Eva_BonusSalary);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }

        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Eva_BonusSalaryModel model)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Eva_BonusSalary);
            //if (!isAccess)
            //{
            //    return PartialView(ConstantMessages.AccessDenied);
            //}

            var service = new RestServiceClient<Eva_BonusSalaryModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Post(_hrm_Hr_Service, "api/Eva_BonusSalary/", model);
            ViewBag.MsgInsert = "Insert success";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion


	}
}