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
    public class FIN_ClaimCostPaymentApproveController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FIN_ClaimCostPaymentApproveInfo(string id, Guid? TravelRequestID, Guid? ProfileID)
        {
            ViewData["TravelRequestID"] = TravelRequestID;
            ViewData["ProfileID"] = ProfileID;

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<FIN_ClaimCostPaymentApproveModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/FIN_ClaimCostPaymentApprove/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult FIN_ClaimCostPayment_Approve(string id)
        {            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<FIN_ClaimCostPaymentApproveModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/FIN_ClaimCostPaymentApprove/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        
        public ActionResult Create(FIN_ClaimCostPaymentApproveModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<FIN_ClaimCostPaymentApproveModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/FIN_ClaimCostPaymentApprove/", model);
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<FIN_ClaimCostPaymentApproveModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/FIN_ClaimCostPaymentApprove/", id);
            return View(result);
        }

        public ActionResult FIN_ClaimItemCostPaymentApproveInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<FIN_ClaimItemModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/FIN_ClaimCostPaymentApproveItem/", idModel);
                if (result.ClaimID == Guid.Empty)
                    result.ClaimID = idModel;

                return View(result);
            }
            else
            {
                if (Request["ClaimID"] != null)
                {
                    string s = Request["ClaimID"].ToString();
                    ViewBag.contractID = s;
                }
                return View();
            }
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<FIN_ClaimCostPaymentApproveModel>(_hrm_Hr_Service, "api/FIN_ClaimCostPaymentApprove/", selectedIds, ConstantPermission.Hre_ProfileAll, DeleteType.Remove.ToString());
        }
        public ActionResult RemoveSelectedItem(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<FIN_ClaimItemModel>(_hrm_Hr_Service, "api/FIN_ClaimCostPaymentApproveItem/", selectedIds, ConstantPermission.Hre_ProfileAll, DeleteType.Remove.ToString());
        }


	}
}