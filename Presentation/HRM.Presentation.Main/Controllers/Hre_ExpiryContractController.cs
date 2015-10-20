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
    public class Hre_ExpiryContractController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Hre_ReportHDTJobModel/
        public ActionResult Index()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Hre_ReportExpiryContract);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
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

        public ActionResult ExpiryContractInfo(string id)
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

        public ActionResult EvaluationContractInfo(string id)
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

        public ActionResult ContractExtendInfo(string id)
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
    }
}