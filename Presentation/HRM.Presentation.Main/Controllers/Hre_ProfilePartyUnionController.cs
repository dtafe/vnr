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
    public class Hre_ProfilePartyUnionController : MainBaseController
    {
        readonly string _hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        //
        // GET: /Hre_ProfilePartyUnion/
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult CreateOrUpdate(string id)
        {
           
           
            if (!string.IsNullOrEmpty(id))
            {
                var service = new RestServiceClient<Hre_ProfilePartyUnionModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Hr_Service);
                var result = service.Get(_hrm_Hr_Service, "api/Hre_ProfilePartyUnion/", id);
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
        public ActionResult RemoveSelected(List<Guid> selectedIds)
        {
            return RemoveOrDeleteAndReturn<Hre_ProfilePartyUnionModel>(_hrm_Hr_Service, "api/Hre_ProfilePartyUnion/",string.Join(",",selectedIds), ConstantPermission.Hre_ProfilePartyUnion, DeleteType.Remove.ToString());
        }
	}
}