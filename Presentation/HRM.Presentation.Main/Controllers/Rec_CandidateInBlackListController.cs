using System.Web.Mvc;
using HRM.Presentation.Recruitment.Models;
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

namespace HRM.Presentation.Main.Controllers
{
    public class Rec_CandidateInBlackListController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;

        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult Create()
        {
           
            return View();
        }
        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Rec_CandidateModel model)
        {
          
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Rec_CandidateModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Rec_Candidate/", model);
                ViewBag.MsgInsert = ConstantDisplay.HRM_Rec_Candidate_InsertSuccess.TranslateString();
            }
            return View();
        }
        
        public ActionResult Edit(string id)
        {

            var service = new RestServiceClient<Rec_CandidateModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Rec_Candidate/", id);
            return View(result);
        }
    
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Rec_CandidateModel Rec_Candidate)
        {
            
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Rec_CandidateModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Rec_Candidate/", Rec_Candidate);
                //return Json(result);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Rec_Candidate_UpdateSuccess.TranslateString();
            }
            return View();
        }
        /// <summary>
        /// Chuyển trạng thái IsDelete = true
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Remove(string id)
        {

            var service = new RestServiceClient<Rec_CandidateModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Delete(_Hrm_Hre_Service, "api/Rec_Candidate/", id);
            return Json(result);
        }


        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Rec_CandidateModel>(_Hrm_Hre_Service, "api/Rec_Candidate/", string.Join(",", selectedIds), ConstantPermission.Rec_Candidate, DeleteType.Remove.ToString());
        }

        public ActionResult RemoveSelecteds(List<Guid> selectedIds)
        {
            var user = new List<Rec_SourceAdsModel>();
            if (selectedIds != null)
            {
                var service = new RestServiceClient<Rec_CandidateModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                foreach (var id in selectedIds)
                {
                    service.Delete(_Hrm_Hre_Service, "api/Rec_Candidate/", id);
                }
            }
            return Json("");
        }

        public ActionResult SetStatusSelected(List<Guid> selectedIds, string status, string userApproved)
        {
           
            var Candidate = new Rec_CandidateModel();
            if (selectedIds != null || selectedIds.Count > 0)
            {
                var service = new RestServiceClient<Rec_CandidateModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                Candidate.ProfileIds = String.Join(",", selectedIds);
                Candidate.Status = status;
                Guid Approved = Guid.Empty;
                Guid.TryParse(userApproved, out Approved);
                if (Approved == Guid.Empty)
                {
                    Candidate.ActionStatus = "NoPermission";
                    return Json(Candidate);
                }
                Candidate.UserApproveID = Approved;
                Candidate.UserApproveID2 = Approved;
                Candidate = service.Put(_Hrm_Hre_Service, "api/Rec_Candidate/", Candidate);
            }
            return Json(Candidate);
        }

        public Rec_CandidateModel GetById(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return null;
            }
            else
            {
                var service = new RestServiceClient<Rec_CandidateModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var result = service.Get(_Hrm_Hre_Service, "api/Rec_Candidate/", (Guid)id);
                return result;
            }
        }
   
        public ActionResult Tab_DetailInfoView(Guid? id)
        {
            
            return id == Guid.Empty ? View() : View(GetById(id));
       
            //return id == Guid.Empty ? View() : View(GetById(id));
        }
        public ActionResult Tab_RecruitmentHistory(Guid? id)
        {
           
            if (id != null)
            {

                ViewBag.CandidateID = id;
            }
            return View();
        }
        public ActionResult Tab_CandidateQualification(Guid? id)
        {
           
            if (id != null)
            {

                ViewBag.CandidateID = id;
            }
            return View();
        }

      

        public ActionResult Tab_RecruitmentHealth(Guid? id) 
        {
           
            return id == Guid.Empty ? View() : View(GetById(id));
        }
        public ActionResult Tab_ResultView(Guid? id)
        {
          
            return id == Guid.Empty ? View() : View(GetById(id));
        } 

        public ActionResult FilterCandidateInfo() 
        {
            
            return View();
          
        }
        public ActionResult Tab_RecruitmentRelative(Guid? id)
        {
          
            return id == Guid.Empty ? View() : View(GetById(id));
        }

        public ActionResult Tab_CandidateHistory(Guid? id)
        {
           
            return id == Guid.Empty ? View() : View(GetById(id));
        }
        public ActionResult Tab_Interview(Guid? id)
        {
           
            return id == Guid.Empty ? View() : View(GetById(id));
        }
    }
}