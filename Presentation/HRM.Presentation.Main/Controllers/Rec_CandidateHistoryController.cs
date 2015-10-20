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
    public class Rec_CandidateHistoryController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Rec_CandidateHistoryModel>(_Hrm_Hre_Service, "api/Rec_CandidateHistory/", String.Join(",", selectedIds), ConstantPermission.Rec_CandidateHistory, DeleteType.Remove.ToString());
        }
        public ActionResult CandidateHistoryInfo(string id, Guid? CandidateID)
        {
            ViewData["CandidateID"] = CandidateID;
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Rec_CandidateHistoryModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hre_Service);
                var modelResult = service.Get(_Hrm_Hre_Service, "api/Rec_CandidateHistory/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }
    }
}