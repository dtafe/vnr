using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using HRM.Presentation.Main.Controllers;


namespace HRM.Presentation.Main.Web.Controllers
{
    public class Cat_EmployeeTypeController : MainBaseController
    {
        private readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /CatEmployeeType/
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult CatEmployeeTypeInfo(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<CatEmployeeTypeModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/CatEmployeeType/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult Delete(Guid id)
        {
            
            var service = new RestServiceClient<CatEmployeeTypeModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Delete(_hrm_Hr_Service, "api/CatEmployeeType/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<CatEmployeeTypeModel>(_hrm_Hr_Service, "api/CatEmployeeType/", selectedIds, ConstantPermission.Cat_ContractType,DeleteType.Remove.ToString());

        }
    }
}