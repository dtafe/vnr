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
    public class Rec_RecruitmentHistoryController : MainBaseController
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
        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            if (selectedIds != null)
            {
                var service = new RestServiceClient<Rec_RecruitmentHistoryModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                RemoveOrDeleteAndReturn<Rec_RecruitmentHistoryModel>(_Hrm_Hre_Service, "api/Rec_RecruitmentHistory/", selectedIds, ConstantPermission.Rec_RecruitmentHistory, DeleteType.Remove.ToString());
            }
            return Json("");
        }

      
        public ActionResult Edit(string id)
        {
            var service = new RestServiceClient<Rec_RecruitmentHistoryModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var result = service.Get(_Hrm_Hre_Service, "api/Rec_RecruitmentHistory/", id);
            return View(result);
        }
    }
}