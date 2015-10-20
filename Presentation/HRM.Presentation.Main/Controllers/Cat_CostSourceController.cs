using HRM.Infrastructure.Utilities;
using HRM.Presentation.Category.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Presentation.Main.Controllers
{
    public class Cat_CostSourceController : MainBaseController
    {
        private readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Cat_COSTSOURCE/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Cat_CostSourceInfo(string id)
        {
            if (id == null || id == string.Empty)
            {
                return View();
            }
            else
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_NameEntityModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Cat_NameEntity/", id1);
                modelResult.CostSourceName = modelResult.NameEntityName;
                return View(modelResult);
            }
        }

	}
}