using System.Web.Mvc;
using HRM.Presentation.Category.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using HRM.Presentation.Evaluation.Models;
using HRM.Presentation.Main.Controllers;
namespace HRM.Presentation.Main.Web.Controllers
{
    public class Cat_NameEntityController : MainBaseController
    {
        private readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        private readonly string _Hrm_Eva_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
          
            return View();
        }

        public ActionResult Cat_NameEntityInfo(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_NameEntityModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Cat_NameEntity/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult Edit(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Cat_NameEntityModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var modelResult = service.Get(_hrm_Hr_Service, "api/Cat_NameEntity/", id1);
                if (modelResult != null)
                    modelResult.NameEntityID = modelResult.ID;
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult Create()
        {
          
            return View();
        }

        public ActionResult EvalKPIInfo(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var service = new RestServiceClient<Eva_KPIModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Eva_Service);
                var result = service.Get(_Hrm_Eva_Service, "api/Eva_KPI/", id);
                return View(result);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// [Chuc.Nguyen] - Chuyển trạng thái IsDelete của các record được chọn sang True
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Cat_NameEntityModel>(_hrm_Hr_Service, "api/Cat_NameEntity/", selectedIds, ConstantPermission.Cat_NameEntity, DeleteType.Remove.ToString());
        }
    }
}