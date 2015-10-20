using System.Web.Mvc;

using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using HRM.Infrastructure.Security;
using HRM.Presentation.Canteen.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;

using VnResource.Helper.Security;
using System.IO;
using System.Linq;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Can_BackPayController : MainBaseController
    {
        private readonly string _hrm_Can_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Can_BackPayModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Can_BackPayModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
                var result = service.Put(_hrm_Can_Service, "api/Can_BackPay/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Category_Position_CreateSuccess.TranslateString();
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Can_BackPayModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Get(_hrm_Can_Service, "api/Can_BackPay/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Can_BackPayModel Can_BackPay)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Can_BackPayModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
                var result = service.Put(_hrm_Can_Service, "api/Can_BackPay/", Can_BackPay);
                //return Json(result);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Category_Position_UpdateSuccess.TranslateString();
            }
            return View();
        }

        /// <summary>
        /// [Hieu.Van] - Chuyển trạng thái IsDelete của các record được chọn sang True
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Can_BackPayModel>(_hrm_Can_Service, "api/Can_BackPay/", selectedIds, ConstantPermission.Can_BackPay, DeleteType.Remove.ToString());
        }       
        
    }

}