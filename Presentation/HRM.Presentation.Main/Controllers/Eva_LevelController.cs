using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Presentation.Service;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using HRM.Presentation.Evaluation.Models;
namespace HRM.Presentation.Main.Controllers
{
    public class Eva_LevelController : MainBaseController
    {
        readonly string _Hrm_Hr_Service = ConstantPathWeb.Hrm_Hre_Service;
        public ActionResult Index()
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Eva_Level);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            return View();
        }
        public ActionResult CreateOrUpdate(string id)
        {
            //bool isAccess;
            //if (!string.IsNullOrEmpty(id))
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Modify, ConstantPermission.Eva_Level);
            //}
            //else
            //{
            //    isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Eva_Level);
            //}
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(id))
            {
                var service = new RestServiceClient<Eva_LevelModel>(UserLogin);
                service.SetCookies(Request.Cookies, _Hrm_Hr_Service);
                var result = service.Get(_Hrm_Hr_Service, "api/Eva_Level/", id);
                return View(result);
            }
            else
            {
                //if (Request["profileID"] != null)
                //{
                //    string aaa = Request["profileID"].ToString();
                //    if (!string.IsNullOrEmpty(aaa))
                //    {

                //        ViewBag.profileID = aaa;

                //    }

                //}
                return View();
            }
        }
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Eva_LevelModel>(_Hrm_Hr_Service, "api/Eva_Level/", selectedIds, ConstantPermission.Eva_Level, DeleteType.Remove.ToString());
        }
	}
}