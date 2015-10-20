using System.Collections.Generic;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using HRM.Infrastructure.Security;
using VnResource.Helper.Security;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
namespace HRM.Presentation.Main.Controllers
{
    public class Sys_GroupController : MainBaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        //
        // GET: /Sys_Group/
        public ActionResult Index()
        {
            
            return View();
        }


        public ActionResult SysGroupInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_GroupModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Sys_Service);
                var result = service.Get(_hrm_Sys_Service, "api/Sys_Group/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table Sys_Group
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 
        public ActionResult Get([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Sys_GroupModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_Group/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mới một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add([Bind] Sys_GroupModel model)
        {
            var service = new RestServiceClient<Sys_GroupModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_Group/", model);
            return Json(result);
        }

        public ActionResult Create()
        {
           
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult Create(Sys_GroupModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Sys_GroupModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                var result = service.Put(_hrm_Sys_Service, "api/Sys_Group/", model);
                ViewBag.MsgInsert = "Insert success";
            }
            return View();
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            
            var service = new RestServiceClient<Sys_GroupModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Delete(_hrm_Sys_Service, "api/Sys_Group/", id);
            return Json(result);
        }

        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public ActionResult Edit(Guid id)
        {
           
            var service = new RestServiceClient<Sys_GroupModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_Group/", id);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit([Bind] Sys_GroupModel group1)
        {
            
            var service = new RestServiceClient<Sys_GroupModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_Group/", group1);
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Sys_GroupModel model)
        {
           
            //if (ModelState.IsValid)
            //{
            var service = new RestServiceClient<Sys_GroupModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_Group/", model);
            //return Json(result);
            //ViewBag.MsgUpdate = "Update success";
            //}
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
           
            var group = new List<Sys_GroupModel>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Common.ConvertToGuid(x))
                    .ToArray();
                var service = new RestServiceClient<Sys_GroupModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                foreach (var id in ids)
                {
                    service.Delete(_hrm_Sys_Service, "api/Sys_Group/", id);
                }
            }
            return Json("");
            //var service = new RestServiceClient<Hre_ProfileModel>(UserLogin);
            //service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            //var result = service.DeleteSelected(_hrm_Hr_Service, "api/Hre_Profile/", selectedIds);
            //return Json(result);
        }

    }
}