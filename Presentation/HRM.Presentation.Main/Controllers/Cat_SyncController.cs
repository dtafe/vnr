using System.Web.Mvc;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Category.Models;
using HRM.Presentation.HrmSystem.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Hr.Models;

namespace HRM.Presentation.Main.Controllers
{
    /// <summary>cấu hình đồng bộ dữ liệu</summary>
    public class Cat_SyncController : MainBaseController
    {
        public const string Route = "api/Cat_Sync/";
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
       
        public ActionResult Index()
        {           
            return View();
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Cat_SyncModel model)
        {
            var service = new RestServiceClient<Cat_SyncModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Post(_hrm_Hr_Service, "api/Cat_Sync/", model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            var service = new RestServiceClient<Cat_SyncModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Cat_Sync/", Common.ConvertToGuid(id));
            return Json(result);
        }

        public ActionResult CatSyncInfo(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_SyncModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Cat_Sync/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }


        public ActionResult Cat_SyncItemInfo(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_SyncModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Cat_SyncItem/", idModel);
                return View(result);
            }
            else
            {
                return View();
            }
        }

        public ActionResult Edit(Guid id)
        {

            var service = new RestServiceClient<Cat_SyncModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_Sync/", id);
            return View(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {

            var service = new RestServiceClient<Cat_SyncModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Remove(_hrm_Hr_Service, "api/Cat_Sync/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Cat_SyncModel>(_hrm_Hr_Service, "api/Cat_Sync/", selectedIds, ConstantPermission.Cat_Sync, DeleteType.Remove.ToString());
        }

        public ActionResult Import_List()
        {
            return View();
        }     
    }
}