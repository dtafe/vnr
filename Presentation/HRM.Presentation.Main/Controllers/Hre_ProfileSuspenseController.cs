using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Controllers
{

    public class Hre_ProfileSuspenseController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;

        // GET: /Hre_StopWorking/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HreStopWorkingInfo(Guid id)
        {
            if (id != null && id != Guid.Empty)
            {
                //var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_StopWorkingModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_StopWorking/", id);
                return View(result);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table Hre_StopWorking
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 
        public ActionResult Get([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Hre_StopWorkingModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_StopWorking/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Lấy StopWorking theo profile Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 
        public ActionResult GetByProfileId()
        {
            return View();
        }

        /// <summary>
        /// Tạo mới một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add([Bind] Hre_StopWorkingModel model)
        {
           
            var service = new RestServiceClient<Hre_StopWorkingModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Hre_StopWorking/", model);
            return Json(result);
        }

        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Tạo một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Hre_StopWorkingModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_StopWorkingModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_StopWorking/", model);
                ViewBag.MsgInsert = "Insert success";
            }
            return View();
        }

        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public ActionResult Edit(Guid id)
        {
           
            var service = new RestServiceClient<Hre_StopWorkingModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_StopWorking/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Hre_StopWorkingModel StopWorking)
        {
          
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_StopWorkingModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_StopWorking/", StopWorking);
                ViewBag.MsgUpdate = "Update success";
            }
            return View();
        }

        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Hre_StopWorkingModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Hre_StopWorking/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_StopWorkingModel>(_hrm_Hr_Service, "api/Hre_StopWorking/",string.Join(",",selectedIds), ConstantPermission.Hre_RegisterSuspense, DeleteType.Remove.ToString());
        }

        public ActionResult RegisterSusoense_RegisterComeBackInfo(string id)
        {
            var userId = Session["UserId"];
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_StopWorkingModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Hre_StopWorking/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult RegisterSuspenseInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_SoftSkillModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Hre_StopWorking/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }

        }

    }
    
}