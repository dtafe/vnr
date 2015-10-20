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
    public class Rec_InterviewCampaignDetailController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
          
            return View();
        }
        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            if (selectedIds != null)
            {
                var service = new RestServiceClient<Rec_InterviewCampaignDetailModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                RemoveOrDeleteAndReturn<Rec_InterviewCampaignDetailModel>(_Hrm_Hre_Service, "api/Rec_InterviewCampaignDetail/", selectedIds, ConstantPermission.Rec_InterviewCampaignDetail, DeleteType.Remove.ToString());
            }
            return Json("");
        }
	}
}