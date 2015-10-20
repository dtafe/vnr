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
    public class Rec_RelativeController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Rec_Relative/
       

        public ActionResult RelativeBranchInfo(string id,Guid? CandidateID)
        {
            ViewData["CandidateID"] = CandidateID;
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_RelativeModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_Relative/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult RelativeOpponentInfo(string id, Guid? CandidateID)
        {
            ViewData["CandidateID"] = CandidateID;
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_RelativeModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_Relative/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult RelativeSupplierInfo(string id, Guid? CandidateID)
        {
            ViewData["CandidateID"] = CandidateID;
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_RelativeModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_Relative/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }



        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Rec_RelativeModel>(_Hrm_Hre_Service, "api/Rec_Relative/", selectedIds, ConstantPermission.Rec_Relative, DeleteType.Remove.ToString());
        }
	}
}