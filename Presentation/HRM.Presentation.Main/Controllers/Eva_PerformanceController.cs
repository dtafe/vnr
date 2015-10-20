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
    public class Eva_PerformanceController : MainBaseController
    {
        readonly string _Hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Eva_Performance);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }
       
        public ActionResult EvalPerformanceInfo(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Eva_Performance);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Eva_Performance);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var service = new RestServiceClient<Eva_PerformanceModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hr_Service);
                var result = service.Get(_Hrm_Hr_Service, "api/Eva_Performance/", id);
                return View(result);
            }
            else
            {
                return View();
            }
        }
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Eva_PerformanceModel>(_Hrm_Hr_Service, "api/Eva_Performance/", selectedIds, ConstantPermission.Eva_Performance, DeleteType.Remove.ToString());
        }
        //public ActionResult CreateOrUpdate(string id)
        //{
           
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        var id1 = Common.ConvertToGuid(id);
        //        var service = new RestServiceClient<Eva_PerformanceModel>();
        //        service.SetCookies(Request.Cookies, _Hrm_Hr_Service);
        //        var modelResult = service.Get(_Hrm_Hr_Service, "api/Eva_Performance/", id1);
        //        return View(modelResult);
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}
        public ActionResult KPIBuilding(string id)
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
                return View();
            }
        }
        #region Create/Edit

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Eva_Performance);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            var service = new RestServiceClient<Eva_PerformanceModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hr_Service);
            var result = service.Get(_Hrm_Hr_Service, "api/Eva_Performance/", id);
            ViewBag.MsgInsert = "Insert success";
            return View(result);
        }

        public ActionResult Create()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Eva_Performance);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }

        [HttpPost, ValidateInput(true)]
        public ActionResult Create([Bind(Prefix = "performanceModel")] Eva_PerformanceModel model, [Bind(Prefix = "models")] List<Eva_KPIModel> kpiModels)
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Eva_Performance);
            //if (!isAccess)
            //{
            //    return PartialView(ConstantMessages.AccessDenied);
            //}

            if (model != null && kpiModels != null)
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

    }
}