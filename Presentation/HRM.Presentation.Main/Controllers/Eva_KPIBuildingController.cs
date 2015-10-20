using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Presentation.Service;
using Kendo.Mvc.UI;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Evaluation.Models;

namespace HRM.Presentation.Main.Controllers
{
    public class Eva_KPIBuildingController : MainBaseController
    {
        readonly string _Hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult EvalPerformanceInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var service = new RestServiceClient<Eva_KPIBuildingModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hr_Service);
                var result = service.Get(_Hrm_Hr_Service, "api/Eva_KPIBuilding/", id);
                return View(result);
            }
            else
            {
                return View();
            }
        }
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Eva_KPIBuildingModel>(_Hrm_Hr_Service, "api/Eva_KPIBuilding/", selectedIds, ConstantPermission.Eva_Performance, DeleteType.Remove.ToString());
        }
        public ActionResult RemoveEva_PerformanceForDetailSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Eva_KPIBuildingModel>(_Hrm_Hr_Service, "api/Eva_PerformanceForDetail/", selectedIds, ConstantPermission.Eva_PerformanceDetail, DeleteType.Remove.ToString());
        }
        public ActionResult CreateOrUpdate(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Eva_PerformanceModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hr_Service);
                var modelResult = service.Get(_Hrm_Hr_Service, "api/Eva_Performance/", id1);
                return View(modelResult);
            }
            else
            {
                return View(new Eva_PerformanceModel());
            }
        }
        #region Create/Edit

        [HttpGet]
        public ActionResult Edit(Guid id)
        {

            var service = new RestServiceClient<Eva_PerformanceModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hr_Service);
            var result = service.Get(_Hrm_Hr_Service, "api/Eva_Performance/", id);
            ViewBag.MsgInsert = "Insert success";
            return View(result);
        }

        public ActionResult Create()
        {
            
            return View();
        }

        [HttpPost, ValidateInput(true)]
        public ActionResult Create([Bind(Prefix = "performanceModel")] Eva_PerformanceModel model, [Bind(Prefix = "models")] List<Eva_KPIModel> kpiModels)
        {
          
            if(kpiModels!=null&&kpiModels.Count!=0)
            {
                model.KPIs = kpiModels;
            }
            var service = new RestServiceClient<Eva_PerformanceModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hr_Service);
            var result = service.Post(_Hrm_Hr_Service, "api/Eva_Performance/", model);
            ViewBag.MsgInsert = "Insert success";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
        public ActionResult AddPerformanceForDetail(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Eva_KPI);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Eva_KPI);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var service = new RestServiceClient<Eva_PerformanceForDetailModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hr_Service);
                var result = service.Get(_Hrm_Hr_Service, "api/Eva_KPI/", id);
                return View(result);
            }
            else
            {
                return View();
            }
        }
    }
}