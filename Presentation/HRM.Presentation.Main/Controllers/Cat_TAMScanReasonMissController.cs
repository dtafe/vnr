using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Web.Controllers
{

    public class Cat_TAMScanReasonMissController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        //
        // GET: /CatShift/
        public ActionResult Index()
        {
            
            return View();
        }


        public ActionResult Cat_TAMScanReasonMissInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_TAMScanReasonMissModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Cat_TAMScanReasonMiss/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Cat_TAMScanReasonMissModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Cat_TAMScanReasonMiss/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mới một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add([Bind] Cat_TAMScanReasonMissModel model)
        {

            var service = new RestServiceClient<Cat_TAMScanReasonMissModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Cat_TAMScanReasonMiss/", model);
            return Json(result);
        }
      
        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit([Bind] Cat_TAMScanReasonMissModel model)
        {

            var service = new RestServiceClient<Cat_TAMScanReasonMissModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Cat_TAMScanReasonMiss/", model);
            return Json(result);
        }
      
        /// <summary>
        /// Chuyển trạng thái IsDelete = true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {
          
            bool delete = false;
            var service = new RestServiceClient<Cat_TAMScanReasonMissModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Remove(_hrm_Hr_Service, "api/Cat_TAMScanReasonMiss/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Cat_TAMScanReasonMissModel>(_hrm_Hr_Service, "api/Cat_TAMScanReasonMiss/", selectedIds, ConstantPermission.Cat_Shift, DeleteType.Remove.ToString());
        }


    }
    
}