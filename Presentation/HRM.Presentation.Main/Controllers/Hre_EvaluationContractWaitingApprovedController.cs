using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Hr.Models;
using HRM.Presentation.Main.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Security;

namespace HRM.Presentation.Main.Controllers
{
    public class Hre_EvaluationContractWaitingApprovedController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        // GET: /Hre_EvaluationContractWaitingApproved/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdateEvaContractInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var service = new RestServiceClient<Hre_ContractModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_Contract/", id);
                return View(result);
            }
            else
            {
                return View();
            }
        }
	}
}