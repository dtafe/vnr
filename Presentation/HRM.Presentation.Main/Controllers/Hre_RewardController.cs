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

    public class Hre_RewardController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;

        //
        //
        // GET: /Hre_Reward/

        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult RewardAddInfo(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_RewardModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Hre_Reward/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult HreRewardInfo(Guid id)
        {
          
            if (id != null && id != Guid.Empty)
            {
                //var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_RewardModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_Reward/", id);
                return View(result);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table Hre_Reward
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 
        public ActionResult Get([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Hre_RewardModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_Reward/");
            return Json(result.ToDataSourceResult(request));
        }


        /// <summary>
        /// Lấy reward theo profile Id
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
        public ActionResult Add([Bind] Hre_RewardModel model)
        {
           
            var service = new RestServiceClient<Hre_RewardModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Hre_Reward/", model);
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
        public ActionResult Create(Hre_RewardModel model)
        {
           
            //model.DateOfIssuance = DateTime.Now;
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_RewardModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_Reward/", model);
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
           
            var service = new RestServiceClient<Hre_RewardModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_Reward/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Hre_RewardModel Reward)
        {
          
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_RewardModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_Reward/", Reward);
                //return Json(result);
                ViewBag.MsgUpdate = "Update success";
            }
            return View();
        }

        public ActionResult Remove(Guid id)
        {
           
            var service = new RestServiceClient<Hre_RewardModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/Hre_Reward/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_RewardModel>(_hrm_Hr_Service, "api/Hre_Reward/", string.Join(",", selectedIds), ConstantPermission.Hre_Reward, DeleteType.Remove.ToString());
            
        }

    }
    
}