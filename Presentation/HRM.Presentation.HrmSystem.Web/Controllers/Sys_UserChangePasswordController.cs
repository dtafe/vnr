using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using HRM.Presentation.HrmSystem.Models;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System;
using System.Linq;

namespace HRM.Presentation.HrmSystem.Web.Controllers
{
    public class Sys_UserChangePasswordController : BaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
       
        #region Change Password

        public ActionResult SysUserChangePassword(Guid id)
        {
            var service = new RestServiceClient<Sys_UserModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var user = service.Get(_hrm_Sys_Service, "api/Sys_User/", id);
           var result = new Sys_UserChangePasswordModel
            {
                UserInfoName = user.UserInfoName,
                UserLogin = user.UserLogin,
                ID = user.ID
            };
            return View(result);
        }
        #endregion
    }
}