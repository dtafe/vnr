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
    public class Fin_PurchaseRequestController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Fin_PurchaseRequest/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Fin_PurchaseRequestModel model)
        {
           
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Fin_PurchaseRequestModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Fin_PurchaseRequest/", model);
                ViewBag.MsgInsert = "Insert success";
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {

            var service = new RestServiceClient<Fin_PurchaseRequestModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Fin_PurchaseRequest/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Fin_PurchaseRequestModel Contract)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Fin_PurchaseRequestModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Fin_PurchaseRequest/", Contract);
                ViewBag.MsgUpdate = "Update success";
            }
            return View();
        }
                

        public ActionResult Add([Bind] Fin_PurchaseRequestModel model)
        {

            var service = new RestServiceClient<Fin_PurchaseRequestModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Fin_PurchaseRequest/", model);
            return Json(result);
        }


        public ActionResult PurchaseRequest_RequestInfo(string id)
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
 
        public ActionResult PurchaseRequestItemInfo(string id)
        {
        
            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Fin_PurchaseRequestItemModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Fin_PurchaseRequestItem/", idModel);
                if (result.PurchaseRequestID == Guid.Empty)
                    result.PurchaseRequestID = idModel;

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
            //var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Cat_SourceAds);
            //if (!isAccess)
            //{
            //    return PartialView(ConstantMessages.AccessDenied);
            //}
            //var user = new List<Fin_PurchaseRequestModel>(UserLogin);
            //if (selectedIds != null)
            //{
            //    //var ids = selectedIds
            //    //    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            //    //    .Select(x => Common.ConvertToGuid(x))
            //    //    .ToArray();
            //    var service = new RestServiceClient<Fin_PurchaseRequestModel>(UserLogin);
            //    service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            //    foreach (var id in selectedIds)
            //    {
            //        service.Delete(_hrm_Hr_Service, "api/Fin_PurchaseRequest/", id);
            //    }
            //}
            //return Json("");
            return RemoveOrDeleteAndReturn<Fin_PurchaseRequestModel>(_hrm_Hr_Service, "api/Fin_PurchaseRequest/", selectedIds, ConstantPermission.Hre_ProfileAll, DeleteType.Remove.ToString());
        }

        public ActionResult RemoveSelectedPRItem(string selectedIds)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Cat_SourceAds);
            //if (!isAccess)
            //{
            //    return PartialView(ConstantMessages.AccessDenied);
            //}
            //var user = new List<Fin_PurchaseRequestItemModel>(UserLogin);
            //if (selectedIds != null)
            //{
            //    //var ids = selectedIds
            //    //    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            //    //    .Select(x => Common.ConvertToGuid(x))
            //    //    .ToArray();
            //    var service = new RestServiceClient<Fin_PurchaseRequestItemModel>(UserLogin);
            //    service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            //    foreach (var id in selectedIds)
            //    {
            //        service.Delete(_hrm_Hr_Service, "api/Fin_PurchaseRequestItem/", id);
            //    }
            //}
            //return Json("");
            return RemoveOrDeleteAndReturn<Fin_PurchaseRequestItemModel>(_hrm_Hr_Service, "api/Fin_PurchaseRequestItem/", selectedIds, ConstantPermission.Hre_ProfileAll, DeleteType.Remove.ToString());
        }

	}
}