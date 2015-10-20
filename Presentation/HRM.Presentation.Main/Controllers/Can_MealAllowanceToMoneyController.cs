using System;
using System.Linq;
using System.Web.Mvc;
using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Canteen.Models;
using HRM.Presentation.Hr.Models;
using VnResource.Helper.Security;
using System.Collections.Generic;
using Kendo.Mvc.UI;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Can_MealAllowanceToMoneyController : MainBaseController
    {
        private readonly string _hrm_Can_Service = ConstantPathWeb.Hrm_Hre_Service;
        private readonly string _hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MealInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 =Common.ConvertToGuid(id);
              
                var service = new RestServiceClient<Can_MealAllowanceToMoneyModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Can_Service);
                var modelResult = service.Get(_hrm_Can_Service, "api/Can_MealAllowanceToMoney/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// Chuyển trạng thái IsDelete = true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(Guid id)
        {
            var service = new RestServiceClient<Can_MealAllowanceToMoneyModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Remove(_hrm_Can_Service, "api/Can_MealAllowanceToMoney/", id); 
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Can_MealAllowanceToMoneyModel>(_hrm_Can_Service, "api/Can_MealAllowanceToMoney/", selectedIds, ConstantPermission.Can_MealRecordMissing, DeleteType.Remove.ToString());
           
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Can_MealAllowanceToMoneyModel model)
        {

            var service = new RestServiceClient<Can_MealAllowanceToMoneyModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Post(_hrm_Can_Service, "api/Can_MealAllowanceToMoney/", model);
            return View();
        }

        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Can_MealAllowanceToMoneyModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Get(_hrm_Can_Service, "api/Can_MealAllowanceToMoney/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Can_MealAllowanceToMoneyModel model)
        {
            var service = new RestServiceClient<Can_MealAllowanceToMoneyModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Post(_hrm_Can_Service, "api/Can_MealAllowanceToMoney/", model);
            return View();
        }
    }

}
