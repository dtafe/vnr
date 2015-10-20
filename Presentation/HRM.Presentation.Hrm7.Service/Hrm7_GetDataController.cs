using HRM.Presentation.Hrm7.Service.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Presentation.Hrm7.Service
{
    public class Hrm7_GetDataController : BaseController
    {
        /// <summary>
        /// [Chuc.Nguyen] - Lấy dữ liệu bảng Thai Sản(Att_Pregnancy) bẳng Store
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProfiles()
        {
            var dbhr7 = new HRM7Entities();
            var list = dbhr7.Hre_Profile.ToList();
            //VnrHrmDataContext dbMain = new VnrHrmDataContext();
            List<HRM.Presentation.Hr.Models.Hre_ProfileModel> listProfiles = new List<Hr.Models.Hre_ProfileModel>();
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
            return Json(listProfiles, JsonRequestBehavior.AllowGet);
        }
    }
}