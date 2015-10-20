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
    public class Rec_RecruitmentCampaignController : MainBaseController
    {
     

        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Rec_RecruitmentCampaign/
        public ActionResult Index()
        {

            return View();
        }


        public ActionResult Create()
        {

            return View();
        }

        [HttpPost, ValidateInput(true)]
        public ActionResult Create(Rec_RecruitmentCampaignModel model)
        {

            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Rec_RecruitmentCampaignModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Rec_RecruitmentCampaign/", model);

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

            var service = new RestServiceClient<Rec_RecruitmentCampaignModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Rec_RecruitmentCampaign/", id);

            ViewBag.MsgInsert = "Insert success";
            //}
            return View(result);
        }

        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Rec_RecruitmentCampaignModel>(_Hrm_Hre_Service, "api/Rec_RecruitmentCampaign/", selectedIds, ConstantPermission.Rec_RecruitmentCampaign, DeleteType.Remove.ToString());
        }
        public ActionResult Rec_RecruitmentCampaignInformation(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_RecruitmentCampaignModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_RecruitmentCampaign/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        public ActionResult CostDetailInfo(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var idModel = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_RecCostDetailModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var result = service.Get(_Hrm_Hre_Service, "api/Rec_RecCostDetail/", idModel);
                if (result.RecCampaignID == Guid.Empty)
                {
                    result.RecCampaignID = idModel;
                }
                return View(result);
            }
            else
            {
                if (Request["ContractID"] != null)
                {
                    string s = Request["ContractID"].ToString();
                    ViewBag.contractID = s;
                }
                return View();
            }
        }
	}
}