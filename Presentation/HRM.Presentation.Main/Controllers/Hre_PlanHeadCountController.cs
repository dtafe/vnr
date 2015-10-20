using HRM.Infrastructure.Utilities;
using HRM.Presentation.Hr.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Controllers
{
    public class Hre_PlanHeadCountController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        //
        // GET: /AttInOut/
        public ActionResult Index()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Hre_PlanHeadCount);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table AttInOut
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Hre_PlanHeadCountModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_PlanHeadCount/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một AttInOut
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Hre_PlanHeadCountModel model)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_PlanHeadCount);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_PlanHeadCountModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Hre_PlanHeadCount/", model);
            return Json(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_PlanHeadCountModel>(_hrm_Hr_Service, "api/Hre_PlanHeadCount/",string.Join(",",selectedIds), ConstantPermission.Hre_PlanHeadCount, DeleteType.Remove.ToString());
        }

        public ActionResult Create()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_PlanHeadCount);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Hre_PlanHeadCountModel model)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_PlanHeadCount);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_PlanHeadCountModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_PlanHeadCount/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_HR_Accident_InsertSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_PlanHeadCount);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_PlanHeadCountModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_PlanHeadCount/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Hre_PlanHeadCountModel Accident)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_PlanHeadCount);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_PlanHeadCountModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_PlanHeadCount/", Accident);
                //return Json(result);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_HR_Accident_UpdateSuccess.TranslateString();
            }
            return View();
        }

        /// <summary>
        /// [Tho.Bui] Lấy đối tượng Accident theo ProfileID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PlanHeadCountInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Cat_Category);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Cat_Category);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_PlanHeadCountModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_PlanHeadCount/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }
    }
}