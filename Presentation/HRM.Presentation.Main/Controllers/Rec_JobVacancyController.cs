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
    public class Rec_JobVacancyController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Rec_JobVacancy/
        public ActionResult Index()
        {
           
            return View();
        }

     
        public ActionResult Create()
        {
           
            return View();
        }

        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Rec_JobVacancyModel model)
        {
         
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Rec_JobVacancyModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Rec_JobVacancy/", model);

                ViewBag.MsgInsert = "Insert success";
            }
            return View();
        }


        /// <summary>
        /// Edit một Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(Guid id)
        {

            var service = new RestServiceClient<Rec_JobVacancyModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Rec_JobVacancy/", id);
            
            ViewBag.MsgInsert = "Insert success";
            //}
            return View(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Rec_JobVacancyModel>(_Hrm_Hre_Service, "api/Rec_JobVacancy/", selectedIds, ConstantPermission.Rec_JobVacancy, DeleteType.Remove.ToString());
        }
        public ActionResult Rec_JobVacancyInformation(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_JobVacancyModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_JobVacancy/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult JobVacancyCondition(string JobConditionIDs, Guid? JobVacancyID)
        {
           
            ViewData["JobConditionIDs"] = "";
            if (!string.IsNullOrEmpty(JobConditionIDs))
            {
                ViewData["JobConditionIDs"] = JobConditionIDs;
            }
            ViewData["JobVacancyID"] = JobVacancyID;
            return View();
        }
        public ActionResult Rec_JobVacancyCondition(string id, Guid? JobVacancyID)
        {
            ViewData["JobVacancyID"] = JobVacancyID;
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_JobConditionModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_JobCondition/", id1);
                ViewData["JobVacancyID"] = modelResult.ID;
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult ConditionSelect(Guid? JobVacancyID)
        {
            ViewData["JobVacancyID"] = JobVacancyID;
            return View();
        }

        

	}
}