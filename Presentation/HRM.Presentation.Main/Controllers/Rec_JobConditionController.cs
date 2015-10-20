using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Main.Controllers;
using HRM.Presentation.Recruitment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Security;

namespace HRM.Presentation.Main.Controllers
{
    public class Rec_JobConditionController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Rec_JobCondition/

        public ActionResult Index(string JobConditionIDs, Guid? JobVacancyID)
        {
           
            ViewData["JobConditionIDs"] = "";
            if (!string.IsNullOrEmpty(JobConditionIDs))
            {
                ViewData["JobConditionIDs"] = JobConditionIDs;
            }
            ViewData["JobVacancyID"] = JobVacancyID;
            return View();
        }
        public ActionResult JobConditionInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_JobConditionModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_JobCondition/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }


        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Rec_JobConditionModel>(_Hrm_Hre_Service, "api/Rec_JobCondition/", selectedIds, ConstantPermission.Rec_JobCondition, DeleteType.Remove.ToString());
        }
	}
}