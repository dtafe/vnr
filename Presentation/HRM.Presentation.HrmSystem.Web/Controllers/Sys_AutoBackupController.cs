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
    public class Sys_AutoBackupController : BaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /SysBookmark/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Delete(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sys_AutoBackupModel>(_hrm_Sys_Service, "api/Sys_AutoBackup/", selectedIds, ConstantPermission.Cat_ExportItem, DeleteType.Remove.ToString());
        }


        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sys_AutoBackupModel>(_hrm_Sys_Service, "api/Sys_AutoBackup/", selectedIds, ConstantPermission.Sys_AutoBackup, DeleteType.Remove.ToString());
        }

        public ActionResult Create([Bind] Sys_AutoBackupModel model)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Cat_ExportItem);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_AutoBackupModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Post(_hrm_Sys_Service, "api/Sys_AutoBackup/", model);
            return Json(result);
        }

        public ActionResult Edit(Sys_AutoBackupModel model)
        {
          
            var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Cat_ExportItem);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_AutoBackupModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Post(_hrm_Sys_Service, "api/Sys_AutoBackup/", model);
            
            return Json(result);

        }

        public ActionResult AutoBackupInfo(string id)
        {
            bool isAccess;
            if (!string.IsNullOrEmpty(id))
            {
                isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Cat_ExportItem);
            }
            else
            {
                isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Cat_ExportItem);
            }
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_AutoBackupModel>();
                service.SetCookies(Request.Cookies, _hrm_Sys_Service);
                var result = service.Get(_hrm_Sys_Service, "api/Sys_AutoBackup/", idModel);
                return View(result);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult CreateList(List<Sys_AutoBackupModel> TotalCat_ElementModel, [Bind(Prefix = "models")] List<Sys_AutoBackupModel> updatedCat_ElementModel)
        {
            var service = new RestServiceClient<Sys_AutoBackupModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            if (updatedCat_ElementModel != null)
            {
                for (int i = 0; i < updatedCat_ElementModel.Count; i++)
                {
                    service.Post(_hrm_Sys_Service, "api/Sys_AutoBackup/", updatedCat_ElementModel[i]);
                }
            }
            return Json(updatedCat_ElementModel);
        }

	}
}