﻿using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;
using HRM.Presentation.Main.Controllers;
namespace HRM.Presentation.Main.Web.Controllers
{

    public class Cat_LateEarlyRuleController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        //
        // GET: /CatShift/
        public ActionResult Index()
        {
            
            return View();
        }


        public ActionResult CatLateEarlyRuleInfo(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatLateEarlyRuleModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/CatLateEarlyRule/", id1);
                return View(result);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table CatShift
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<CatLateEarlyRuleModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/CatLateEarlyRule/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mới một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add([Bind] CatLateEarlyRuleModel model)
        {

            var service = new RestServiceClient<CatLateEarlyRuleModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/CatLateEarlyRule/", model);
            return Json(result);
        }
      
        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit([Bind] CatLateEarlyRuleModel model)
        {

            var service = new RestServiceClient<CatLateEarlyRuleModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/CatLateEarlyRule/", model);
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
            var service = new RestServiceClient<CatLateEarlyRuleModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Remove(_hrm_Hr_Service, "api/CatLateEarlyRule/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatGradeModel>(_hrm_Hr_Service, "api/CatLateEarlyRule/", selectedIds, ConstantPermission.Cat_Shift,DeleteType.Remove.ToString());
      
        }
       


    }
    
}