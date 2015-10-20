using HRM.Infrastructure.Utilities;
using HRM.Presentation.Attendance.Models;
using HRM.Presentation.Hr.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;

using HRM.Infrastructure.Security;
using VnResource.Helper.Security;
using System.IO;

namespace HRM.Presentation.Main.Controllers
{
    public class Att_ProfileNotRosterController : MainBaseController
    {
        readonly string _Hrm_Hre_Service = ConstantPathWeb.Hrm_Hre_Service;
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(Guid? id)
        {
            if (id != null)
            {
                var serviceProfile = new RestServiceClient<Hre_ProfileModel>(UserLogin);
                serviceProfile.SetCookies(this.Request.Cookies, _hrm_Hr_Service);
                var resultProfile = serviceProfile.Get(_hrm_Hr_Service, "api/Hre_Profile/", id.Value);
                Att_RosterModel result = new Att_RosterModel();
                if (resultProfile != null)
                {
                    var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    result.ProfileID = resultProfile.ID;
                    result.ProfileName = resultProfile.ProfileName;
                    result.DateStart = date;
                    result.DateEnd = date.AddMonths(1).AddDays(-1);
                }
                return View(result);
            }
            else
            {
                Att_RosterModel result = new Att_RosterModel();
                return View(result);
            }    
        }
        [HttpPost]
        public ActionResult Edit([DataSourceRequest] DataSourceRequest request, Att_RosterModel AttRoster)
        {
            if (ModelState.IsValid)
            {
                var service = new RestServiceClient<Att_RosterModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _Hrm_Hre_Service);
                var result = service.Put(_Hrm_Hre_Service, "api/Att_Roster/", AttRoster);
                ViewBag.MsgUpdate = ConstantDisplay.HRM_Attendance_Roster_UpdateSuccess.TranslateString();
            }
            return View();
        }

	}
}