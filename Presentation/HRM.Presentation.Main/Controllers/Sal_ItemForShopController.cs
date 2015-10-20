using System.Web.Mvc;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using HRM.Infrastructure.Security;
using HRM.Presentation.Payroll.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using System.IO;
using System;
using System.Linq;
using HRM.Presentation.HrmSystem.Models;

namespace HRM.Presentation.Main.Controllers
{
    public class Sal_ItemForShopController : MainBaseController 
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        readonly string _Hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sal_ItemForShopInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sal_ItemForShop);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sal_ItemForShop);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sal_ItemForShopModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Sal_ItemForShop/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }


        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sal_ItemForShopModel>(_hrm_Hr_Service, "api/Sal_ItemForShop/",String.Join(",",selectedIds), ConstantPermission.Sal_ItemForShop, DeleteType.Remove.ToString());
        }

        public ActionResult ReportElementDynamicConfig(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_AllSettingModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Sys_Service);
                var modelResult = service.Get(_Hrm_Sys_Service, "api/Sal_ItemForShop/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult ReportSalUnsualAllowanceCfgConfig(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_AllSettingModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Sys_Service);
                var modelResult = service.Get(_Hrm_Sys_Service, "api/Sal_ItemForShop/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

	}
}