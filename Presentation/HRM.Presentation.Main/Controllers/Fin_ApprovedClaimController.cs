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
    public class Fin_ApprovedClaimController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Fin_ApprovedClaim/
        public ActionResult Index()
        {
            return View();
        }
        
        //public ActionResult ApprovedClaim_RequestInfo(string id)
        //{
        //    //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Rec_SourceAds);
        //    //if (!isAccess)
        //    //{
        //    //    return PartialView(ConstantMessages.AccessDenied);
        //    //}
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        var id1 = Common.ConvertToGuid(id);
        //        var service = new RestServiceClient<FIN_ClaimModel>();
        //        service.SetCookies(Request.Cookies, _hrm_Hr_Service);
        //        var modelResult = service.Get(_hrm_Hr_Service, "api/FIN_Claim/", id1);
        //        return View(modelResult);
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}
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
        public ActionResult ApprovedClaim_RequestApprovedInfo(string id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Rec_SourceAds);
            //if (!isAccess)
            //{
            //    return PartialView(ConstantMessages.AccessDenied);
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<FIN_ClaimModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/FIN_Claim/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult ApprovedClaim_RequestRejectInfo(string id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Rec_SourceAds);
            //if (!isAccess)
            //{
            //    return PartialView(ConstantMessages.AccessDenied);
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<FIN_ClaimModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/FIN_Claim/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult DetailClaim(string id, Guid? TravelRequestID, Guid? ProfileID)
        {
            ViewData["TravelRequestID"] = TravelRequestID;
            ViewData["ProfileID"] = ProfileID;

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<FIN_ClaimModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/FIN_Claim/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

	}
}