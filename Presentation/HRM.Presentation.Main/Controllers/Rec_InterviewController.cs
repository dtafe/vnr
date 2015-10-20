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
    public class Rec_InterviewController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Rec_Interview/
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult InterviewInfo(string id, Guid? CandidateID)
        {
            ViewData["CandidateID"] = CandidateID;
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_InterviewModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_Interview/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult InterviewInfoDetail(string id, Guid? CandidateID)
        {
            ViewData["CandidateID"] = CandidateID;
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_InterviewModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_Interview/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Rec_InterviewModel>(_Hrm_Hre_Service, "api/Rec_Interview/", String.Join(",", selectedIds), ConstantPermission.Rec_Interview, DeleteType.Remove.ToString());
        }
        public ActionResult RemoveSelecteds(List<Guid> selectedIds)
        {
          
            var user = new List<Rec_SourceAdsModel>();
            if (selectedIds != null)
            {
                var service = new RestServiceClient<Rec_InterviewModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                foreach (var id in selectedIds)
                {
                    service.Delete(_Hrm_Hre_Service, "api/Rec_Interview/", id);
                }
            }
            return Json("");

        }
        public Rec_InterviewModel GetById(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return null;
            }
            else
            {
                var service = new RestServiceClient<Rec_InterviewModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var result = service.Get(_Hrm_Hre_Service, "api/Rec_Candidate/", (Guid)id);
                return result;
            }
        }
        public ActionResult Tab_Interview(Guid? id, Guid? CandidateID)
        {
            
            ViewData["CandidateID"] = CandidateID;
            return id == Guid.Empty ? View() : View(GetById(id));
           
        }
	}
}