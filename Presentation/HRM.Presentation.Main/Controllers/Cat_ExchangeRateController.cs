using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Web.Controllers
{
    public class Cat_ExchangeRateController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;

        // GET: /Cat_ExchangeRate/
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult Cat_ExchangeRateInfo(string id) 
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_ExchangeRateModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Cat_ExchangeRate/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

            public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Cat_ExchangeRateModel>(_hrm_Hr_Service, "api/Cat_ExchangeRate/", string.Join(",", selectedIds), ConstantPermission.Cat_ExchangeRate, DeleteType.Remove.ToString());
        }
    }
}