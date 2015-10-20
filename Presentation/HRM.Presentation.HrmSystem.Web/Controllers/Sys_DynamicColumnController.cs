using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.HrmSystem.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using HRM.Infrastructure.Security;
using VnResource.Helper.Security;
using System.IO;
using System.Linq;
using System;

namespace HRM.Presentation.HrmSystem.Web.Controllers
{
    public class Sys_DynamicColumnController : BaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /Sys_DynamicColumn/
        public ActionResult Index()
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sys_DynamicColumn);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            return View();
        }
      
        public ActionResult Sys_DynamicInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_DynamicColumnModel>();
                service.SetCookies(Request.Cookies, _hrm_Sys_Service);
                var result = service.Get(_hrm_Sys_Service, "api/Sys_DynamicColumn/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table Sys_DynamicColumn
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAllSys_DynamicColumn([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Sys_DynamicColumnModel>>();
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_DynamicColumn/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Lấy tất cả dữ liệu cho control multiselect
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult GetMultiselect()
        {
            var service = new RestServiceClient<IEnumerable<Sys_DynamicColumnModel>>();
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_DynamicColumn/");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// Tạo mới một Sys_DynamicColumn
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
      
        public ActionResult Create(Sys_DynamicColumnModel model)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_DynamicColumn);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_DynamicColumnModel>();
            var sys_DynamicColumnModel = new Sys_DynamicColumnModel();
            sys_DynamicColumnModel = model;        
            if(model.Length != null)
            {
                if (model.DataType == "Decimal") sys_DynamicColumnModel.DataType = model.DataType + "(" + model.Length + ",0)";
                else {
                    sys_DynamicColumnModel.DataType = model.DataType + "(" + model.Length + ")";
                }
            }
            else {
                sys_DynamicColumnModel.DataType = model.DataType;
            }
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_DynamicColumn/", sys_DynamicColumnModel);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Sys_DynamicColumn);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_DynamicColumnModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Delete(_hrm_Sys_Service, "api/Sys_DynamicColumn/", id);
            return Json(result);
        }
        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public ActionResult Edit(Guid id)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sys_DynamicColumn);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_DynamicColumnModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_DynamicColumn/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Sys_DynamicColumnModel DynamicColumn)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sys_DynamicColumn);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_DynamicColumnModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Put(_hrm_Sys_Service, "api/Sys_DynamicColumn/", DynamicColumn);
            return Json(result);
        }
        public ActionResult RemoveSelected(string selectedIds)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Sys_DynamicColumn);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var Grade = new List<Sys_DynamicColumnModel>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Common.ConvertToGuid(x))
                    .ToArray();
                var service = new RestServiceClient<Sys_DynamicColumnModel>();
                service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                foreach (var id in ids)
                {
                    service.Delete(_hrm_Sys_Service, "api/Sys_DynamicColumn/", id);
                }
            }
            return Json("");
            //var service = new RestServiceClient<Hre_ProfileModel>();
            //service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            //var result = service.DeleteSelected(_hrm_Hr_Service, "api/Hre_Profile/", selectedIds);

        } 
    }
}