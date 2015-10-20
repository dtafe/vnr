using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Presentation.Attendance.Models;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Controllers
{

    public class GeneralProfileController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        
        //
        //
        // GET: /Hre_Profile/
        public ActionResult Index()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.GeneralProfile);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }
  
        public ActionResult Tab_Hre(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.GeneralProfile);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_Profile/", id);
            return View(result);
        }
        public ActionResult GeneralProfileBasic(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.GeneralProfile);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (id != Guid.Empty) return View();
            var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_Profile/", id);
            if (result.ImagePath==null)
            {
                result.ImagePath = "no_avatar.jpg";
            }
            return View(result);
        }

        /// <summary>
        /// Lấy tất cả dữ liệu trong table Hre_Profile
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Get([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Hre_ProfileModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_Profile/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mới một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
         
        //[HttpGet]
        //public ActionResult Create()
        //{
        //    return View();
        //}
        [HttpPost]
        public ActionResult Create([Bind] Hre_ProfileModel model)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.GeneralProfile);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Hre_Profile/", model);
            return Json(result);
        }

        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GeneralProfileEdit(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.GeneralProfile);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_Profile/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult GeneralProfileEdit([Bind] Hre_ProfileModel model)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.GeneralProfile);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Hre_Profile/", model);
            return View(result);
        }
      
        /// <summary>
        /// Chuyển trạng thái IsDelete = true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.GeneralProfile);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            bool delete = false;
            var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Remove(_hrm_Hr_Service, "api/Hre_Profile/", id);
            return Json(result);
        }      
        
        public ActionResult GetPartialView(string partialName)
        {
            return PartialView(partialName);
        }

    }
}