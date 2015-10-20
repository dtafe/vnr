using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Payroll.Models;
using HRM.Presentation.HrmSystem.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;

namespace HRM.Presentation.Main.Controllers
{
    public class Sal_UnusualEDChildCareController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Sal_UnusualEDChildCare/
        public ActionResult Index()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sal_UnusualEDChildCare);
            //if (!isAccess)
            //{
            //    return PartialView(ConstantMessages.AccessDenied);
            //}
            return View();
        }
        public ActionResult UnusualEDChildCareInfo(string id) 
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sal_UnusualEDChildCare);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sal_UnusualEDChildCare);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sal_UnusualAllowanceModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Sal_UnusualEDChildCare/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult Hre_UnusualEDChildCareInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sal_UnusualAllowanceModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Sal_UnusualEDChildCare/", id1);
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
           
        
        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sal_UnusualAllowanceModel>(_hrm_Hr_Service, "api/Sal_UnusualEDChildCare/", selectedIds, ConstantPermission.Sal_UnusualEDChildCare, DeleteType.Remove.ToString());

        }
     
    }
}