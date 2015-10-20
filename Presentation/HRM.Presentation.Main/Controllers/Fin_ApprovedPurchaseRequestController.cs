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
    public class Fin_ApprovedPurchaseRequestController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Fin_PurchaseRequest/
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

        public ActionResult ProcessRejectPage(Guid loginID, Guid recordID)
        {
            ViewBag.Login = loginID;
            ViewBag.Record = recordID;
            return View();
        }
        public ActionResult PurchaseRequestApprovedInfo(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Fin_PurchaseRequestModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Fin_PurchaseRequest/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult PurchaseRequestRejectInfo(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Fin_PurchaseRequestModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Fin_PurchaseRequest/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult DetailPurchaseRequest(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Fin_PurchaseRequestModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Fin_PurchaseRequest/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

	}
}