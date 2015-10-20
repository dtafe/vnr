using System;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.HrmSystem.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;


namespace HRM.Presentation.Main.Controllers
{
    public class Sys_UserApproveController : MainBaseController
    {
        private readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        public ActionResult Index()
        {
            
            return View();
        }


        public ActionResult SysUserApproveInfo(string id)
        {
            
            if (!string.IsNullOrEmpty(id))
            {
                var id1 = Common.ConvertToGuid(id);
                var service = new RestServiceClient<Sys_UserApproveModel>(UserLogin);
                service.SetCookies(Request.Cookies, _hrm_Sys_Service);
                var modelResult = service.Get(_hrm_Sys_Service, "api/Sys_UserApprove/", id1);
                return View(modelResult);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// [Quoc.Do] - Chuyển trạng thái IsDelete của các record được chọn sang True
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        public ActionResult RemoveSelected(string selectedIds)
        {
            return RemoveOrDeleteAndReturn<Sys_UserApproveModel>(_hrm_Sys_Service, "api/Sys_UserApprove/", selectedIds, ConstantPermission.Sys_UserApprove, DeleteType.Remove.ToString());
        }
	}
}