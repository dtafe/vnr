using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.HrmSystem.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System;
using System.Linq;

namespace HRM.Presentation.Main.Controllers
{
    public class Sys_UserController : MainBaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /Sys_User/
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult SysUserInfo(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_UserModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Sys_Service);
                var modelResult = service.Get(_hrm_Sys_Service, "api/Sys_User/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Lấy tất cả dữ liệu trong table Sys_User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult Get([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Sys_UserModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_User/");
            return Json(result.ToDataSourceResult(request));
        }

 

        /// <summary>
        /// Tạo mới một Sys_User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] Sys_UserModel model)
        {

            var service = new RestServiceClient<Sys_UserModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_User/", model);
            return Json(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {

            var service = new RestServiceClient<Sys_UserModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Delete(_hrm_Sys_Service, "api/Sys_User/", id);
            return Json(result);
        }

        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public ActionResult Edit(Guid id)
        {

            var service = new RestServiceClient<Sys_UserModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_User/", id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Sys_UserModel user)
        {

            //if (ModelState.IsValid)
            //{
            var service = new RestServiceClient<Sys_UserModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                var result = service.Put(_hrm_Sys_Service, "api/Sys_User/", user);
                //return Json(result);
                //ViewBag.MsgUpdate = "Update success";
            //}
                return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            
            var user = new List<Sys_UserModel>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Common.ConvertToGuid(x))
                    .ToArray();
                var service = new RestServiceClient<Sys_UserModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                foreach (var id in ids)
                {
                    service.Delete(_hrm_Sys_Service, "api/Sys_User/", id);
                }
            }
            return Json("");
            //var service = new RestServiceClient<Hre_ProfileModel>();
            //service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            //var result = service.DeleteSelected(_hrm_Hr_Service, "api/Hre_Profile/", selectedIds);
            //return Json(result);
        }      
    }
}