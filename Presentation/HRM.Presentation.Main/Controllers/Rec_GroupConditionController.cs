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
    public class Rec_GroupConditionController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Rec_GroupCondition/
        public ActionResult Index()
        {
           
            return View();
        }

     
        public ActionResult Create()
        {
           
            return View();
        }

        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Rec_GroupConditionModel model)
        {
         
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Rec_GroupConditionModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Rec_GroupCondition/", model);

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

            var service = new RestServiceClient<Rec_GroupConditionModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Rec_GroupCondition/", id);
            
            ViewBag.MsgInsert = "Insert success";
            //}
            return View(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Rec_GroupConditionModel>(_Hrm_Hre_Service, "api/Rec_GroupCondition/", selectedIds, ConstantPermission.Rec_GroupCondition, DeleteType.Remove.ToString());
        }
        public ActionResult Rec_GroupConditionInformation(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_GroupConditionModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_GroupCondition/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult GroupConditionCondition(string JobConditionIDs, Guid? GroupConditionID)
        {
           
            ViewData["JobConditionIDs"] = "";
            if (!string.IsNullOrEmpty(JobConditionIDs))
            {
                ViewData["JobConditionIDs"] = JobConditionIDs;
            }
            ViewData["GroupConditionID"] = GroupConditionID;
            return View();
        }

        public ActionResult Rec_GroupConditionCondition(string id, Guid? GroupConditionID)
        {
            ViewData["GroupConditionID"] = GroupConditionID;
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_JobConditionModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_JobCondition/", id1);
                ViewData["GroupConditionID"] = modelResult.ID;
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult ConditionSelect(Guid? GroupConditionID)
        {
            ViewData["GroupConditionID"] = GroupConditionID;
            return View();
        }
	}
}