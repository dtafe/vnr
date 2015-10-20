using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Cat_LaundryController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;

        //
        // GET: /CatLaundry/
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult LaundryInfo(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);

                var service = new RestServiceClient<CatLaundryModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/CatLaundry/", id1);
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

            var service = new RestServiceClient<CatLaundryModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Remove(_hrm_Hr_Service, "api/CatLaundry/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatLaundryModel>(_hrm_Hr_Service, "api/CatLaundry/", selectedIds, ConstantPermission.Lau_Locker,DeleteType.Remove.ToString());
        
        }

    }
}