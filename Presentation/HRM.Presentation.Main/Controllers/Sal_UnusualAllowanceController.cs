using System.Web.Mvc;
using HRM.Presentation.Payroll.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace HRM.Presentation.Main.Controllers
{
    public class Sal_UnusualAllowanceController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;

        // GET: /Sal_UnusualAllowance/
        public ActionResult Index()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sal_UnusualAllowance);
            //if (!isAccess)
            //{
            //    return PartialView(ConstantMessages.AccessDenied);
            //}
            return View();
        }

        public ActionResult Sal_UnusualAllowanceInfo(string id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sal_UnusualAllowance);
            //if (!isAccess)
            //{
            //    return PartialView(ConstantMessages.AccessDenied);
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sal_UnusualAllowanceModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Sal_UnusualAllowance/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult Hre_UnusualAllowanceInfo(string id)
        {
          
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sal_UnusualAllowanceModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Sal_UnusualAllowance/", id1);
                if (modelResult.ProfileID != Guid.Empty)
                {
                    return View(modelResult);
                }
                else
                {
                    string aaa = id;
                    if (!string.IsNullOrEmpty(aaa))
                    {
                        ViewBag.profileID = aaa;
                    }
                    return View();
                }
            }
            else
            {
                if (Request["profileID"] != null)
                {
                    string aaa = Request["profileID"].ToString();
                    if (!string.IsNullOrEmpty(aaa))
                    {
                        ViewBag.profileID = aaa;
                    }
                }
                return View();
            }
        }
        public ActionResult Sal_UnusualAllowanceRelativeInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id).ToString();
                ViewBag.profileID = id1;
            }
            else
            {
                if (Request["profileID"] != null)
                {
                    string aaa = Request["profileID"].ToString();
                    if (!string.IsNullOrEmpty(aaa))
                    {
                        ViewBag.profileID = aaa;
                    }
                }
            }
            return View();
        }

        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Delete, ConstantPermission.Sal_UnusualAllowance);
            //if (!isAccess)
            //{
            //    return PartialView(ConstantMessages.AccessDenied);
            //}
            var user = new List<Sal_UnusualAllowanceModel>();
            if (selectedIds != null)
            {
                var service = new RestServiceClient<Sal_UnusualAllowanceModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                foreach (var id in selectedIds)
                {
                    service.Delete(_hrm_Hr_Service, "api/Sal_UnusualAllowance/", id);
                }
            }
            return Json("");
        }
    }
}