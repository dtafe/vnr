using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Presentation.Category.Web;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Category.Models;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Cat_SalaryRankController : MainBaseController
    {
        private readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service; 
        //
        // GET: /Cat_SalaryRank/
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult CreateOrUpdate(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var idGuid = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_SalaryRankModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Cat_SalaryRank/", idGuid);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Cat_SalaryRankModel>(_hrm_Hr_Service, "api/Cat_SalaryRank/", selectedIds, ConstantPermission.Cat_SalaryClass, DeleteType.Remove.ToString());
        }
	}
}