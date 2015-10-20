using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Presentation.Service;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Evaluation.Models;

namespace HRM.Presentation.Main.Controllers
{
    public class Eva_EvaluatorController : MainBaseController
    {
        readonly string _Hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {           
            return View();
        }
       
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Eva_EvaluatorModel>(_Hrm_Hr_Service, "api/Eva_Evaluator/", selectedIds, ConstantPermission.Eva_Evaluator, DeleteType.Remove.ToString());
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Eva_EvaluatorModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hr_Service);
            var result = service.Get(_Hrm_Hr_Service, "api/Eva_Evaluator/", id);
            ViewBag.MsgInsert = "Insert success";
            return View(result);
        }

        public ActionResult Create()
        {           
            return View();
        }

        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Eva_EvaluatorModel model)
        {
            var service = new RestServiceClient<Eva_EvaluatorModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hr_Service);
            var result = service.Post(_Hrm_Hr_Service, "api/Eva_Evaluator/", model);
            ViewBag.MsgInsert = "Insert success";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

	}
}