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
    public class FIN_ApproveTravelCostController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /FIN_TravelRequest/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(FIN_ApproveTravelCostModel model)
        {
           
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<FIN_TravelRequestModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/FIN_ApproveTravelCost/", model);
                ViewBag.MsgInsert = "Insert success";
            }
            return View();
        }

       
        public ActionResult Edit(Guid id)
        {

            var service = new RestServiceClient<FIN_ApproveTravelCostModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/FIN_ApproveTravelCost/", id);
            return View(result);
        }

        public ActionResult EditPayment(Guid id)
        {

            var service = new RestServiceClient<FIN_ClaimModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/FIN_Claim/", id);
            return View(result);
        }
        public ActionResult CreatePayment(Guid? id)
        {

            var service = new RestServiceClient<FIN_ClaimModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/FIN_ApproveTravelCost/", id.Value);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, FIN_TravelRequestModel Contract)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<FIN_TravelRequestModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/FIN_ApproveTravelCost/", Contract);
                ViewBag.MsgUpdate = "Update success";
            }
            return View();
        }
                

        public ActionResult Add([Bind] FIN_TravelRequestModel model)
        {

            var service = new RestServiceClient<FIN_TravelRequestModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/FIN_ApproveTravelCost/", model);
            return Json(result);
        }


        public ActionResult TravelRequest_RequestInfo(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<FIN_TravelRequestModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/FIN_ApproveTravelCost/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult FIN_ClaimInfo(string id,Guid? TravelRequestID,Guid? ProfileID)
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
        public ActionResult FIN_ClaimItemInfo(string id, Guid? ClaimID)
        {
            ViewData["ClaimID"] = ClaimID;
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<FIN_ClaimItemModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/FIN_ClaimItem/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult TravelRequestItemInfo(string id)
        {
        
            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<FIN_TravelRequestItemModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/FIN_TravelRequestItem/", idModel);
                if (result.TravelRequestID == Guid.Empty)
                    result.TravelRequestID = idModel;

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

        public ActionResult Claim_TravelRequestItemInfo(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<FIN_TravelRequestItemModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/FIN_TravelRequestItem/", idModel);
                if (result.TravelRequestID == Guid.Empty)
                    result.TravelRequestID = idModel;

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

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<FIN_TravelRequestModel>(_hrm_Hr_Service, "api/FIN_ApproveTravelCost/", selectedIds, ConstantPermission.Hre_ProfileAll, DeleteType.Remove.ToString());
        }

        public ActionResult RemoveSelectedPRItem(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<FIN_TravelRequestItemModel>(_hrm_Hr_Service, "api/FIN_TravelRequestItem/", selectedIds, ConstantPermission.Hre_ProfileAll, DeleteType.Remove.ToString());
        }
        public ActionResult RemoveFIN_ClaimSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<FIN_ClaimModel>(_hrm_Hr_Service, "api/FIN_Claim/", selectedIds, ConstantPermission.FIN_Claim, DeleteType.Remove.ToString());
        }
        public ActionResult RemoveFIN_ClaimItemSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<FIN_ClaimItemModel>(_hrm_Hr_Service, "api/FIN_ClaimItem/", selectedIds, ConstantPermission.FIN_ClaimItem, DeleteType.Remove.ToString());
        }


	}
}