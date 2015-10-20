using System;
using System.Web.Mvc;
using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Laundry.Models;
using VnResource.Helper.Security;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Controllers
{
    public class Lau_LockerController : MainBaseController
    {
        private readonly string _hrm_Lau_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Provider/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LockerInfo(Guid id)
        {
            if (id != Guid.Empty)
            {
                var service = new RestServiceClient<Lau_LockerModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Lau_Service);
                var modelResult = service.Get(_hrm_Lau_Service, "api/Lau_Locker/", id);
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
            var service = new RestServiceClient<Lau_LockerModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Lau_Service);
            var result = service.Remove(_hrm_Lau_Service, "api/Lau_Locker/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Lau_LockerModel>(_hrm_Lau_Service, "api/Lau_Locker/", selectedIds, ConstantPermission.Lau_Locker, DeleteType.Remove.ToString());
        }
       
    }
}
