using System.Web.Mvc;
using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Canteen.Models;
using VnResource.Helper.Security;
using System;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Can_MealPriceTypeSettingController : MainBaseController
    {
        private readonly string _hrm_Can_Service = ConstantPathWeb.Hrm_Hre_Service;

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Can_MealAllowanceTypeSettingModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Get(_hrm_Can_Service, "api/Can_MealPriceTypeSetting/", id);
            return View(result);
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Can_MealPriceTypeSettingInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Can_MealAllowanceTypeSettingModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Can_Service);
                var modelResult = service.Get(_hrm_Can_Service, "api/Can_MealPriceTypeSetting/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }

        }
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Can_MealAllowanceTypeSettingModel>(_hrm_Can_Service, "api/Can_MealPriceTypeSetting/", selectedIds,
                ConstantPermission.Can_MealAllowanceTypeSetting, DeleteType.Remove.ToString());
        }
    }
}
