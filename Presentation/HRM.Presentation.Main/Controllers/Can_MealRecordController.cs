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
using HRM.Presentation.Service;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Can_MealRecordController : MainBaseController
    {
        private readonly string _hrm_Can_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAll(Guid id)
        {
            if (id != null && id != Guid.Empty)
            {
                //var id1 = int.Parse(id);
                var service = new RestServiceClient<Can_MealRecordModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Can_Service);
                var modelResult = service.Get(_hrm_Can_Service, "api/Can_MealRecord/", id);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// [Hieu.Van] - Chuyển trạng thái IsDelete của các record được chọn sang True
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Can_MealRecordModel>(_hrm_Can_Service, "api/Can_MealRecord/", selectedIds, ConstantPermission.Can_MealRecord, DeleteType.Remove.ToString());
        }

        public ActionResult CreateOrUpdate(Guid id)
        {
            if (id != null && id != Guid.Empty)
            {
                //var id1 = int.Parse(id);
                var service = new RestServiceClient<Can_MealRecordModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Can_Service);
                var modelResult = service.Get(_hrm_Can_Service, "api/Can_MealRecord/", id);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Can_MealRecordModel model)
        {
            var service = new RestServiceClient<Can_MealRecordModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Post(_hrm_Can_Service, "api/Can_MealRecord/", model);
            return View();
        }

        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Can_MealRecordModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Get(_hrm_Can_Service, "api/Can_MealRecord/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Can_MealRecordModel model)
        {
            var service = new RestServiceClient<Can_MealRecordModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Post(_hrm_Can_Service, "api/Can_MealRecord/", model);
            return View();
        }

        public ActionResult MealRecordInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Can_MealRecordModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Can_Service);
                var modelResult = service.Get(_hrm_Can_Service, "api/Can_MealRecord/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
    }

}