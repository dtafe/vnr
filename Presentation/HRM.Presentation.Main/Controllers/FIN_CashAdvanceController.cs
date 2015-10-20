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
using HRM.Presentation.Service;

namespace HRM.Presentation.Main.Controllers
{
    public class FIN_CashAdvanceController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /FIN_TravelRequest/
        public ActionResult Index()
        {
            return View();
        }

        
        
        public ActionResult FIN_ClaimInfo(string id,Guid? TravelRequestID,Guid? ProfileID)
        {
            ViewData["CashAdvanceID"] = TravelRequestID;
            ViewData["ProfileID"] = ProfileID;

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<FIN_CashAdvanceModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/FIN_CashAdvance/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult ApprovedCashAdvance_RequestInfo(string id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Rec_SourceAds);
            //if (!isAccess)
            //{
            //    return PartialView(ConstantMessages.AccessDenied);
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<FIN_CashAdvanceModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/FIN_CashAdvance/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }


        public ActionResult Create(FIN_CashAdvanceModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<FIN_CashAdvanceModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/FIN_CashAdvance/", model);
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {

            var service = new RestServiceClient<FIN_CashAdvanceModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/FIN_CashAdvance/", id);
            return View(result);
        }

        public ActionResult FIN_CashAdvanceItemInfo(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<FIN_CashAdvanceItemModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/FIN_CashAdvanceItem/", idModel);
                if (result.CashAdvanceID == Guid.Empty)
                    result.CashAdvanceID = idModel;

                return View(result);
            }
            else
            {
                if (Request["ContractID"] != null)
                {
                    string s = Request["ContractID"].ToString();
                    ViewBag.contractID = s;
                }
                return View();
            }
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<FIN_CashAdvanceModel>(_hrm_Hr_Service, "api/FIN_CashAdvance/", selectedIds, ConstantPermission.Hre_ProfileAll, DeleteType.Remove.ToString());
        }
        public ActionResult RemoveSelectedItem(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<FIN_CashAdvanceItemModel>(_hrm_Hr_Service, "api/FIN_CashAdvanceItem/", selectedIds, ConstantPermission.Hre_ProfileAll, DeleteType.Remove.ToString());
        }


	}
}