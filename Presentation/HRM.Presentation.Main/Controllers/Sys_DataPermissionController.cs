using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.HrmSystem.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Infrastructure.Security;
using VnResource.Helper.Security;

namespace HRM.Presentation.Main.Controllers
{
   
    public class Sys_DataPermissionController : MainBaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        //
        // GET: /Sys_DataPermission/
        public ActionResult Index(Guid id)
        {
            
            var service = new RestServiceClient<Sys_UserModel>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_User/", id);

            Sys_DataPermissionModel dataPermission = new Sys_DataPermissionModel
            {
               UserID = result.ID,
               UserName = result.UserInfoName
            };

            return View(dataPermission);
        }

        //public ActionResult ReadGrid([DataSourceRequest] DataSourceRequest request, int id)
        //{
        //    var service = new RestServiceClient<IEnumerable<Sys_DataPermissionModel>>(UserLogin);
        //    service.SetCookies(Request.Cookies, _hrm_Sys_Service);
        //    var result = service.Put(_hrm_Sys_Service, "api/SysDataPermission/", id).ToList();
        //    return Json(result.ToDataSourceResult(request));
        //} 

        // <summary>
        /// Lấy tất cả dữ liệu trong table Sys_DataPermission
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Sys_DataPermissionModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/SysDataPermission/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một Sys_DataPermission
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] Sys_DataPermissionModel model)
        {
           
            var service = new RestServiceClient<Sys_DataPermissionModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/SysDataPermission/", model);
            return Json(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            
            var service = new RestServiceClient<Sys_DataPermissionModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Delete(_hrm_Sys_Service, "api/SysDataPermission/", id);
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
                var service = new RestServiceClient<Sys_DataPermissionModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                foreach (var id in ids)
                {
                    service.Delete(_hrm_Sys_Service, "api/SysDataPermission/", id);
                }
            }
            return Json("");
            //var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
            //service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            //var result = service.DeleteSelected(_hrm_Hr_Service, "api/Hre_Profile/", selectedIds);
            //return Json(result);
        }


        public ActionResult SysDataPermissionInfo(string id ,string userName,string userId)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_DataPermissionModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Sys_Service);
                var result = service.Get(_hrm_Sys_Service, "api/SysDataPermission/", id1);
                return View(result);
            }
            var iUserId =0;
            ViewBag.userName = userName;
            int.TryParse(userId, out iUserId);
            ViewBag.userId =  iUserId;
            return View();
        }
       
    }
    
}