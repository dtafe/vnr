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
    public class Rec_InterviewCampaignController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Rec_InterviewCampaign/
        public ActionResult Index()
        {
          
            return View();
        }

        public ActionResult InterviewCampaignInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_InterviewCampaignModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_InterviewCampaign/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Rec_InterviewCampaignModel>(_Hrm_Hre_Service, "api/Rec_InterviewCampaign/", String.Join(",", selectedIds), ConstantPermission.Rec_InterviewCampaign, DeleteType.Remove.ToString());
        }

        //[HttpPost, ValidateInput(true)]
        public ActionResult Create(string Selectedids)
        {
            if (!string.IsNullOrEmpty(Selectedids))
            {
                ViewData["CandidateIds"] = Selectedids;
            }
            return View();
        }
        /// <summary>
        /// Tạo mời một CatExportItem
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult CreateInLine([Bind] Rec_InterviewCampaignDetailModel model)
        {

            var service = new RestServiceClient<Rec_InterviewCampaignDetailModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Post(_Hrm_Hre_Service, "api/Rec_InterviewCampaignDetail/", model);
            return Json(result);
        }
    }
}