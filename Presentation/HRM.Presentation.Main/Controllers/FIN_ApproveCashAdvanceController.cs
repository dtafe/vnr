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
    public class FIN_ApproveCashAdvanceController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
       
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
                var service = new RestServiceClient<FIN_ApproveCashAdvanceModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/FIN_ApproveCashAdvance/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult ApprovedCashAdvance_RequestInfo(string id)
        {           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<FIN_ApproveCashAdvanceModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/FIN_ApproveCashAdvance/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }


        public ActionResult Create(FIN_ApproveCashAdvanceModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<FIN_ApproveCashAdvanceModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/FIN_ApproveCashAdvance/", model);
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<FIN_ApproveCashAdvanceModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/FIN_ApproveCashAdvance/", id);
            return View(result);
        }

        public ActionResult FIN_CashAdvanceItemInfo(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<FIN_CashAdvanceItemModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/FIN_ApproveCashAdvance/", idModel);
                if (result.CashAdvanceID == Guid.Empty)
                    result.CashAdvanceID = idModel;

                return View(result);
            }
            else
            {
                if (Request["CashAdvanceID"] != null)
                {
                    string s = Request["CashAdvanceID"].ToString();
                    ViewBag.contractID = s;
                }
                return View();
            }
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<FIN_ApproveCashAdvanceModel>(_hrm_Hr_Service, "api/FIN_ApproveCashAdvance/", selectedIds, ConstantPermission.Hre_ProfileAll, DeleteType.Remove.ToString());
        }
        public ActionResult RemoveSelectedItem(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<FIN_CashAdvanceItemModel>(_hrm_Hr_Service, "api/FIN_CashAdvanceItem/", selectedIds, ConstantPermission.Hre_ProfileAll, DeleteType.Remove.ToString());
        }


	}
}