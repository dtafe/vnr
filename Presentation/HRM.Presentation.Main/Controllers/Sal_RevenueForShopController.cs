﻿using System.Web.Mvc;
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

namespace HRM.Presentation.Main.Controllers
{
    public class Sal_RevenueForShopController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sal_RevenueForShopInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Sal_RevenueForShop);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sal_RevenueForShop);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sal_RevenueForShopModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Sal_RevenueForShop/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }


        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sal_RevenueForShopModel>(_hrm_Hr_Service, "api/Sal_RevenueForShop/", String.Join(",", selectedIds), ConstantPermission.Sal_RevenueForShop, DeleteType.Remove.ToString());
        }

    }
}