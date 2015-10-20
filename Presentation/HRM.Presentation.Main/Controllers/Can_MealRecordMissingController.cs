using System.Web.Mvc;
using System;
using HRM.Infrastructure.Security;
using HRM.Presentation.Canteen.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Security;
using System.Linq;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Can_MealRecordMissingController : MainBaseController
    {
        readonly string _hrm_Can_Service = ConstantPathWeb.Hrm_Hre_Service;
        // GET: /Can_BackPay/
        public ActionResult Index()
        {

            return View();
        }
        

        public ActionResult MealRecordMissingInfo(Guid id)
        {
            if (id != null && id != Guid.Empty)
            {
                //int id1 = 0;
               // Int32.TryParse(id, out id1);
                var service = new RestServiceClient<Can_MealRecordMissingModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Can_Service);
                var modelResult = service.Get(_hrm_Can_Service, "api/Can_MealRecordMissing/", id);
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

            var service = new RestServiceClient<Can_MealRecordMissingModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Can_Service);
            var result = service.Remove(_hrm_Can_Service, "api/Can_MealRecordMissing/", id);
            return Json(result);
        }

   
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Can_MealRecordMissingModel>(_hrm_Can_Service, "api/Can_MealRecordMissing/", selectedIds, ConstantPermission.Can_MealRecordMissing, DeleteType.Remove.ToString());
        }


    }

}