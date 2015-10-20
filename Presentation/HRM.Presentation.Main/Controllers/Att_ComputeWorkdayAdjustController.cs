using HRM.Infrastructure.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using HRM.Presentation.Category.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Security;

namespace HRM.Presentation.Main.Controllers
{
    public class Att_ComputeWorkdayAdjustController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;

        public ActionResult Index()
        {
            #region [Hien.Nguyen]

            var serviceCat_Shift = new RestServiceClient<IEnumerable<CatShiftMultiModel>>(UserLogin);
            serviceCat_Shift.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var catShift = serviceCat_Shift.Get(_Hrm_Hre_Service, "api/CatShift/");
            ViewData["Cat_Shift"] = catShift;

            var serviceCat_TamScan = new RestServiceClient<IEnumerable<Cat_TAMScanReasonMissMuitlModel>>(UserLogin);
            serviceCat_TamScan.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var Cat_TamScan = serviceCat_TamScan.Get(_Hrm_Hre_Service, "api/Cat_TAMScanReasonMiss/");
            ViewData["Cat_TamScan"] = Cat_TamScan;

            var serviceCat_LeadayType = new RestServiceClient<IEnumerable<CatLeaveDayTypeMultiModel>>(UserLogin);
            serviceCat_LeadayType.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
            var Cat_LeadayType = serviceCat_LeadayType.Get(_Hrm_Hre_Service, "api/CatLeaveDayType/");
            ViewData["Cat_LeadayType"] = Cat_LeadayType; 

            #endregion

            return View();
        }

	}
}