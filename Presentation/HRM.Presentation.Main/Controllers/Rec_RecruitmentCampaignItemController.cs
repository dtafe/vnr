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
    public class Rec_RecruitmentCampaignItemController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Rec_RecruitmentCampaignItemRec/
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult RecruitmentCampaignItemInfo(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_RecruitmentCampaignItemModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_RecruitmentCampaignItem/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }


        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            if (selectedIds != null)
            {
                var service = new RestServiceClient<Rec_RecruitmentCampaignItemModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                RemoveOrDeleteAndReturn<Rec_RecruitmentCampaignItemModel>(_Hrm_Hre_Service, "api/Rec_RecruitmentCampaignItem/", selectedIds, ConstantPermission.Rec_RecruitmentCampaignItem, DeleteType.Remove.ToString());
            }
            return Json("");
        }
	}
}