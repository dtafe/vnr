using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.HrmSystem.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;

namespace HRM.Presentation.HrmSystem.Web.Controllers
{
    public class Sys_ReleaseNoteController : BaseController
    {
        #region Properties
        
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;

        public const string Route = "api/Sys_ReleaseNote/";
        
        #endregion

        #region Action

        // GET: /Sys_ReleaseNote/
        public ActionResult Index()
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sys_ReleaseNote);
            if (!isAccess)
            {
                return PartialView(ConstantMessages.AccessDenied);
            }
            return View();
        }

        /// <summary> Lấy tất cả dữ liệu trong table CatPosition </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Sys_ReleaseNoteModel>>();
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, Route);
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary> Tạo mời một CatPosition </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Sys_ReleaseNoteModel model)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_ReleaseNote);
            if (!isAccess)
            {
                return PartialView(ConstantMessages.AccessDenied);
            }
            var service = new RestServiceClient<Sys_ReleaseNoteModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, Route, model);
            return Json(result);
        }

        public ActionResult Remove(Guid id)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Sys_ReleaseNote);
            if (!isAccess)
            {
                return PartialView(ConstantMessages.AccessDenied);
            }
            var service = new RestServiceClient<Sys_ReleaseNoteModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Delete(_hrm_Sys_Service, Route, id);
            return Json(result);
        }

        public ActionResult Create()
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_ReleaseNote);
            if (!isAccess)
            {
                return PartialView(ConstantMessages.AccessDenied);
            }
            return View();
        }

        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Sys_ReleaseNoteModel model)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_ReleaseNote);
            if (!isAccess)
            {
                return PartialView(ConstantMessages.AccessDenied);
            }
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Sys_ReleaseNoteModel>();
                service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                var result = service.Put(_hrm_Sys_Service, Route, model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Category_Position_CreateSuccess.TranslateString();
            }
            return View();
        }

        public ActionResult Edit(Guid id)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sys_ReleaseNote);
            if (!isAccess)
            {
                return PartialView(ConstantMessages.AccessDenied);
            }
            var service = new RestServiceClient<Sys_ReleaseNoteModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, Route, id);
            return View(result);
        }
        
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Sys_ReleaseNoteModel catPosition)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sys_ReleaseNote);
            if (!isAccess)
            {
                return PartialView(ConstantMessages.AccessDenied);
            }
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Sys_ReleaseNoteModel>();
                service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                var result = service.Put(_hrm_Sys_Service, Route, catPosition);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Category_Position_UpdateSuccess.TranslateString();
            }
            return View();
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Sys_ReleaseNote);
            if (!isAccess)
            {
                return PartialView(ConstantMessages.AccessDenied);
            }
            var Grade = new List<Sys_ReleaseNoteModel>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Common.ConvertToGuid(x))
                    .ToArray();
                var service = new RestServiceClient<Sys_ReleaseNoteModel>();
                service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                foreach (var id in ids)
                {
                    service.Delete(_hrm_Sys_Service, Route, id);
                }
            }
            return Json(string.Empty);
        } 
     
        public ActionResult SaveLoginInfo(string userName, string password, Guid userId)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_User);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            Sys_ReleaseNoteModel model = new Sys_ReleaseNoteModel();
            model.UserCreateID = userId;
            model.Code = userName;
            model.ReleaseNoteName = password;
            model.UserNameCreate = "LoginTemp";
            model.UserNameUpdate = "LoginTemp";

            var service = new RestServiceClient<Sys_ReleaseNoteModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Post(_hrm_Sys_Service, "api/Sys_ReleaseNote/", model);
            if (result!=null)
            {
                Session["LoginUserNameSession"] = userName;
                Session["UserIdSession"] = userId;
                Session["LoginPasswordSession"] = password;
            }

            return Json ("");
        }
        #endregion
        
	}
}