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
    public class FIN_ApproverECLAIMController : MainBaseController
    {
        readonly string _Hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {           
            return View();
        }
       
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<FIN_ApproverECLAIMModel>(_Hrm_Hr_Service, "api/FIN_ApproverECLAIM/", selectedIds, ConstantPermission.FIN_ApproverECLAIM, DeleteType.Remove.ToString());
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<FIN_ApproverECLAIMModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hr_Service);
            var result = service.Get(_Hrm_Hr_Service, "api/FIN_ApproverECLAIM/", id);
            ViewBag.MsgInsert = "Insert success";
            return View(result);
        }

        public ActionResult Create()
        {           
            return View();
        }

        [HttpPost, ValidateInput(true)]
        public ActionResult Create(FIN_ApproverECLAIMModel model)
        {
            var service = new RestServiceClient<FIN_ApproverECLAIMModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hr_Service);
            var result = service.Post(_Hrm_Hr_Service, "api/FIN_ApproverECLAIM/", model);
            ViewBag.MsgInsert = "Insert success";
            return Json(result, JsonRequestBehavior.AllowGet);
        }


	}
}