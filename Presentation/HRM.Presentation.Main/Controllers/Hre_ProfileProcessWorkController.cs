using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Infrastructure.Security;
using VnResource.Helper.Security;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Hr.Models;
using HRM.Presentation.Main.Controllers;

namespace HRM.Presentation.Main.Controllers
{
    public class Hre_ProfileProcessWorkController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Hre_ProfileProcessWork/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateOrUpdate(string id)
        {
           
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Hre_RewardModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_Reward/", id1);
                if (result.ProfileID == Guid.Empty)
                    result.ProfileID = id1;
                return View(result);
            }
            else
            {
                if (Request["profileID"] != null)
                {
                    string aaa = Request["profileID"].ToString();
                    if (!string.IsNullOrEmpty(aaa))
                    {

                        ViewBag.profileID = aaa;

                    }

                }
                return View();
            }
        }
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_RewardModel>(_hrm_Hr_Service, "api/Hre_Reward/", selectedIds, ConstantPermission.Hre_Reward, DeleteType.Remove.ToString());
        }
    }
}