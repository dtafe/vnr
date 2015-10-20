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
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Controllers
{

    public class Hre_HDTJobController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;

        //
        //
        // GET: /Hre_HDTJob/

        public ActionResult Index()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Hre_HDTJob);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }


        public ActionResult HreHDTJobInfo(Guid id)
        {
            //bool isAccess;
            //if (id != null && id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_HDTJob);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_HDTJob);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (id != null && id != Guid.Empty)
            {
               // var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_HDTJobModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_HDTJob/", id);
                return View(result);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table Hre_HDTJob
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 
        public ActionResult Get([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Hre_HDTJobModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_HDTJob/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Lấy HDTJob theo ProfileId
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 

        public ActionResult GetByProfileId()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_HDTJob);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();

        }

        public ActionResult GetByProfileIdGrid([DataSourceRequest] DataSourceRequest request, Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_HDTJob);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<IEnumerable<Hre_HDTJobModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_HDTJobCustom/", id);
            return Json(result.ToDataSourceResult(request));

        }

        /// <summary>
        /// Tạo mới một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add([Bind] Hre_HDTJobModel model)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_HDTJob);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_HDTJobModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Hre_HDTJob/", model);
            return Json(result);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Hre_HDTJobModel model)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_HDTJob);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            //if (ModelState.IsValid)
            //{
            //    var service = new RestServiceClient<Hre_HDTJobModel>(UserLogin);
            //    service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            //    var result = service.Put(_hrm_Hr_Service, "api/Hre_HDTJob/", model);
            //    ViewBag.MsgInsert = "Insert success";
            //}
            //return View();
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_HDTJob);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_HDTJobModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_HDTJob/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Hr_HDTJob_CreateSuccess.TranslateString();
            }
            return View();
        }

        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public ActionResult Edit(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_HDTJob);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_HDTJobModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_HDTJob/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Hre_HDTJobModel HDTJob)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_HDTJob);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_HDTJobModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_HDTJob/", HDTJob);
                //return Json(result);
                ViewBag.MsgUpdate = "Update success";
            }
            return View();
        }

        public ActionResult Remove(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Hre_HDTJob);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_HDTJobModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Hre_HDTJob/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_HDTJobModel>(_hrm_Hr_Service, "api/Hre_HDTJob/",string.Join(",",selectedIds), ConstantPermission.Hre_HDTJob, DeleteType.Remove.ToString());
        }

    }
    
}