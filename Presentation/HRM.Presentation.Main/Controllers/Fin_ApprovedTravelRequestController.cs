using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.Hr.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
namespace HRM.Presentation.Main.Controllers
{
    public class Fin_ApprovedTravelRequestController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Fin_ApprovedClaim/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProcessApprovedPage(Guid loginID, Guid userApprovedID, Guid recordID)
        {
            ViewBag.Login = loginID;
            ViewBag.UserApproved = userApprovedID;
            ViewBag.Record = recordID;
            return View();
        }

        public ActionResult ProcessRejectPage(Guid loginID, Guid recordID, Guid userApprovedID)
        {
            ViewBag.Login = loginID;
            ViewBag.Record = recordID;
            ViewBag.UserApproved = userApprovedID;

            return View();
        }
        public ActionResult TravelRequestApprovedInfo(string id) 
        {

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<FIN_TravelRequestModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Fin_TravelRequest/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult TravelRequestRejectInfo(string id) 
        {

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<FIN_TravelRequestModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Fin_TravelRequest/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult DetailTravelRequest(Guid id)
        {

            var service = new RestServiceClient<FIN_TravelRequestModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/FIN_TravelRequest/", id);
            return View(result);
        }
	}
}