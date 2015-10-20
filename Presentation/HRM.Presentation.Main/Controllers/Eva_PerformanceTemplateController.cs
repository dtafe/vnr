using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Category.Models;
using HRM.Presentation.Evaluation.Models;
using VnResource.Helper.Security;

namespace HRM.Presentation.Main.Controllers
{
    public class Eva_PerformanceTemplateController : MainBaseController
    {
        readonly string _Hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;

        // GET: /Eva_PerformanceTemplate/
        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult EvaPerformanceTemplateInfo(string id)
        {          
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Eva_PerformanceTemplateModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hr_Service);
                var modelResult = service.Get(_Hrm_Hr_Service, "api/EvaPerformanceTemplate/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        ///  Chuyển trạng thái IsDelete của các record được chọn sang True
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Eva_PerformanceTemplateModel>(_Hrm_Hr_Service, "api/Eva_PerformanceTemplate/", selectedIds, ConstantPermission.Eva_PerformanceTemplate, DeleteType.Remove.ToString());
        }

        public ActionResult RemoveSelectedTemplateDetail(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Eva_PerformanceDetailModel>(_Hrm_Hr_Service, "api/Eva_PerformanceDetail/", selectedIds, ConstantPermission.Eva_PerformanceDetail, DeleteType.Remove.ToString());
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var service = new RestServiceClient<Eva_PerformanceTemplateModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hr_Service);
            var result = service.Get(_Hrm_Hr_Service, "api/Eva_PerformanceTemplate/", id);
            ViewBag.MsgInsert = "Insert success";
            return View(result);
        }

        public ActionResult Create()
        {          
            return View();
        }

        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Eva_PerformanceTemplateModel model)
        {
            var service = new RestServiceClient<Eva_PerformanceTemplateModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hr_Service);
            var result = service.Post(_Hrm_Hr_Service, "api/Eva_PerformanceTemplate/", model);
            ViewBag.MsgInsert = "Insert success";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

	}
}