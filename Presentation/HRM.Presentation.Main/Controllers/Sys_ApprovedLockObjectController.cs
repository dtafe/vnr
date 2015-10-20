using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Security;

namespace HRM.Presentation.Main.Controllers
{
    public class Sys_ApprovedLockObjectController : MainBaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /Sys_LockObject/
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Create([Bind] Sys_LockObjectModel model)
        {
            
            //var service = new RestServiceClient<Sys_LockObjectModel>(UserLogin);
            //service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            //var result = service.Put(_hrm_Sys_Service, "api/Sys_LockObject/", model);
            return View();
        }

        public ActionResult Edit(string id)
        {
            
            var id1 = Common.ConvertToGuid(id);
            var service = new RestServiceClient<Sys_LockObjectModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_LockObject/", id1);
            return View(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sys_LockObjectModel>(_hrm_Sys_Service, "api/Sys_LockObject/", string.Join(",", selectedIds), ConstantPermission.Sys_LockObject, DeleteType.Remove.ToString());
        }

        public ActionResult LockObjectItemInfo(string id, string ShiftName)
        {
            
            //if (!string.IsNullOrEmpty(id) && id != Guid.Empty.ToString())
            //{
            //    var id1 = Common.ConvertToGuid(id);
            //    var service = new RestServiceClient<Sys_LockObjectItemModel>(UserLogin);
            //    service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            //    var modelResult = service.Get(_hrm_Sys_Service, "api/Sys_LockObjectItem/", id1);
            //    if (modelResult.LockObjectID == Guid.Empty)
            //        modelResult.LockObjectID = id1;
            //    return View(modelResult);
            //}
            //else
            //{
            //    if (Request["LockObjectID"] != null)
            //    {
            //        string aaa = Request["LockObjectID"].ToString();
            //        if (!string.IsNullOrEmpty(aaa))
            //        {
            //            ViewBag.LockObjectID = aaa;
            //        }
            //    }
            //    return View();
            //}

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_LockObjectItemModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Sys_Service);
                var modelResult = service.Get(_hrm_Sys_Service, "api/Sys_LockObjectItem/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }


        }

        public ActionResult LockObjectConfigObjectInfo(string id)
        {
            return View();
            //if (!string.IsNullOrEmpty(id))
            //{
            //    var id1 = Common.ConvertToGuid(id);
            //    var service = new RestServiceClient<Sys_AllSettingModel>(UserLogin);
            //    service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            //    var modelResult = service.Get(_hrm_Sys_Service, "api/Sal_ItemForShop/", id1);
            //    return View(modelResult);
            //}
            //else
            //{
            //    return View();
            //}
        }

        public ActionResult RemoveSelectedShiftItem(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sys_LockObjectItemModel>(_hrm_Sys_Service, "api/Sys_LockObjectItem/", selectedIds, ConstantPermission.Sys_LockObject, DeleteType.Remove.ToString());
        }
	}
}