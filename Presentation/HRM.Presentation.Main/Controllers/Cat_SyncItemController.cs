
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
    public class Cat_SyncItemController : MainBaseController
    {

        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Lấy tất cả dữ liệu trong table Cat_SyncItem
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAllCatImportItem([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Cat_SyncItemModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_SyncItem/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Lấy tất cả dữ liệu cho control multiselect
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult GetMultiselect()
        {
            var service = new RestServiceClient<IEnumerable<Cat_SyncItemModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_SyncItem/");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Tạo mời một Cat_SyncItem
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Create([Bind] Cat_SyncItemModel model)
        {
            var service = new RestServiceClient<Cat_SyncItemModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Post(_hrm_Hr_Service, "api/Cat_SyncItem/", model);
            return Json(result);
        }

        /// <summary>
        /// Delete bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid id)
        {
            string selectedIds = string.Join(",", id.ToString());
            return RemoveOrDeleteAndReturn<Cat_SyncItemModel>(_hrm_Hr_Service, "api/Cat_SyncItem/", selectedIds, ConstantPermission.Cat_SyncItem, DeleteType.Remove.ToString());
        }   
    }
}