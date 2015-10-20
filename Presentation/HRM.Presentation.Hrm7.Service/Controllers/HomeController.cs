using HRM.Presentation.Hrm7.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace HRM.Presentation.Hrm7.Service.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            var dbhr7 = new HRM7Entities();
            var list = dbhr7.Hre_Profile.ToList();
            //VnrHrmDataContext dbMain = new VnrHrmDataContext();
            List<HRM.Presentation.Hr.Models.Hre_ProfileModel> listProfiles = new List<Hr.Models.Hre_ProfileModel>();
            ViewBag.Title = "Home Page";
            HRM.Presentation.Hr.Models.Hre_ProfileModel profile = null;
            foreach (var item in list)
            {
                profile = new HRM.Presentation.Hr.Models.Hre_ProfileModel();
                profile.ProfileName = item.ProfileName;
                profile.CodeEmp = item.CodeEmp;
                profile.CodeAttendance = item.CodeAttendance;
                profile.IDNo = item.IDNo;
                listProfiles.Add(profile);
            }
            return View();
        }
    }
}
