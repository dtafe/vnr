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
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Controllers
{

    public class Hre_ContractController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        //
        // GET: /HreContarct/
        public ActionResult Index()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Hre_Contract);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }
        /// <summary>
        /// Lấy tất cả dữ liệu trong table HreContarct
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetAll([DataSourceRequest] DataSourceRequest request)
        {
            var service = new RestServiceClient<IEnumerable<Hre_ContractModel>>(UserLogin);
            service.SetCookies(Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_Contract/");
            return Json(result.ToDataSourceResult(request));
        }

        /// <summary>
        /// Tạo mời một HreContarct
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Add([Bind] Hre_ContractModel model)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Contract);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_ContractModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Put(_hrm_Hr_Service, "api/Hre_Contract/", model);
            return Json(result);
        }


        public ActionResult Remove(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Hre_Contract);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_ContractModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Remove(_hrm_Hr_Service, "api/Hre_Contract/", id);
            return Json(result);
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_ContractModel>(_hrm_Hr_Service, "api/Hre_Contract/", string.Join(",", selectedIds), ConstantPermission.Hre_Contract, DeleteType.Remove.ToString());
        }

        public ActionResult UpdateStatusSelected(List<Guid> selectedIds, string status)
        {
            Hre_ContractModel model = new Hre_ContractModel();
            model.Status = status;
            model.selecteIds = selectedIds;
            if (selectedIds.Count >= 0)
            {
                var service = new RestServiceClient<Hre_ContractModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var dataReturn = service.Put(_hrm_Hr_Service, "api/Hre_Contract/", model);

                return Json(dataReturn, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
        }

        public ActionResult Create()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Contract);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Hre_ContractModel model)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Contract);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_ContractModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_Contract/", model);
                ViewBag.MsgInsert = "Insert success";
            }
            return View();
        }
        public ActionResult Edit(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Contract);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Hre_ContractModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
            var result = service.Get(_hrm_Hr_Service, "api/Hre_Contract/", id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Hre_ContractModel Contract)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Contract);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Hre_ContractModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var result = service.Put(_hrm_Hr_Service, "api/Hre_Contract/", Contract);
                ViewBag.MsgUpdate = "Update success";
            }
            return View();
        }


        public ActionResult AppendixContractInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Contract);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Contract);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_AppendixContractModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_AppendixContract/", idModel);
                if (result.ContractID == Guid.Empty)
                    result.ContractID = idModel;

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

        public ActionResult ProfileAppendixContractInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Contract);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Contract);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_AppendixContractModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_AppendixContract/", idModel);
                if (result.ContractID == Guid.Empty)
                    result.ContractID = idModel;

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

        public ActionResult ExpiryAppendixContractInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Contract);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Contract);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_AppendixContractModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_AppendixContract/", idModel);
                if (result.ContractID == Guid.Empty)
                    result.ContractID = idModel;

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


        public ActionResult ContractInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Qualification);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Qualification);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_ContractModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_Contract/", id1);
                if (result.ProfileID == Guid.Empty)
                {
                    result.ProfileID = id1;
                }

                return View(result);
            }
            else
            {
                if (Request["ProfileID"] != null)
                {
                    string aaa = Request["ProfileID"].ToString();
                    if (!string.IsNullOrEmpty(aaa))
                    {

                        ViewBag.profileID = aaa;

                    }

                }
                return View();
            }
        }

        #region TabStrip Contract
        public Hre_ContractModel GetById(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return null;
            }
            else
            {
                var service = new RestServiceClient<Hre_ContractModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_Contract/", (Guid)id);
                return result;
            }
        }

        public ActionResult CreateOrUpdate(Guid? id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Contract);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Contract);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult EvaContract(Guid? id)
        {
            //bool isAccess;
            //if (id != Guid.Empty)
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Hre_Contract);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Hre_Contract);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return id == Guid.Empty ? View() : View(GetById(id));
        }
        #endregion


        public ActionResult Profile_ContractInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_ContractModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_Contract/", id1);
                if (result.ProfileID == Guid.Empty)
                {
                    result.ProfileID = id1;
                }
                return View(result);
            }
            else
            {
                if (Request["ProfileID"] != null)
                {
                    string aaa = Request["ProfileID"].ToString();
                    if (!string.IsNullOrEmpty(aaa))
                    {

                        ViewBag.profileID = aaa;
                    }
                }
                return View();
            }
        }

        public ActionResult TabContractInfo(Guid? id)
        {
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult TabEvaContractInfo(Guid? id)
        {
            return id == Guid.Empty ? View() : View(GetById(id));
        }
    }

}