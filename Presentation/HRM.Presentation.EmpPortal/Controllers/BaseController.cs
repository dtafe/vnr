using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using System.Web.Mvc;
using HRM.Presentation.HrmSystem.Models;
using VnResource.Helper.Security;
using System;
using System.Globalization;

namespace HRM.Presentation.Main.Controllers
{
    public class BaseController : Controller
    {
        #region Properties
      
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;

        public string UserLogin
        {
            get
            {
                var sessionName = SessionObjects.LoginUserName;
                string userLogin = string.Empty;

                if (Session[sessionName] != null)
                {
                    userLogin = Session[sessionName].ToString();
                }
                return userLogin;
            }
        }

        public int UserId
        {
            get
            {
                var sessionName = SessionObjects.UserId;
                int UserId = 0;

                if (Session[sessionName] != null)
                {
                    UserId = (int)Session[sessionName];
                }
                return UserId;
            }
        }

        #endregion

        #region Methods

         //<summary> Kiểm tra quyền cho user </summary>
         //<param name="privilegeType"></param>
         //<param name="permission"></param>
         //<returns></returns>
        public JsonResult CheckPermission(PrivilegeType privilegeType, string permission)
        {
//            return Json(true, JsonRequestBehavior.AllowGet);
            var isSuccess = true;
            Guid userId = Guid.Empty;
            if (Request[SessionObjects.UserId] != null)
            {
                Guid.TryParse(Request[SessionObjects.UserId].ToString(), out userId);
                if (userId != Guid.Empty)
                {
                    Session[SessionObjects.UserId] = userId;
                }
            }
            else
            {
                if (Session[SessionObjects.UserId] != null)
                {
                    Guid.TryParse(Session[SessionObjects.UserId].ToString(), out userId);
                }    
            }

            if (permission == "#" || permission == "Home")
            {
                //dấu # : khi click vào phân trang (|< < >  >|) thì permission sẽ trả về '#'
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            var permissionModel = new PermissionModel
            {
                UserID = userId,
                PrivilegeType = privilegeType,
                Permission = permission
            };
            var service = new RestServiceClient<PermissionModel>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            PermissionModel result = service.Post(_hrm_Sys_Service, "api/SysPermission/", permissionModel);
            bool isAccess = result == null ? false : result.IsAllowAccess;

            return Json(isAccess, JsonRequestBehavior.AllowGet);
        }
        ///// <summary>
        ///// [Hien.Nguyen] Lấy format ngày của server hiện tại
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult GetFormatDate()
        //{
        //    return Json(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
        //}
        /// <summary>
        /// [Hien.Nguyen] Lấy format ngày của server hiện tại
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetFormatDate(string value, bool IsTo = false)
        {

            try
            {
                DateTime result = new DateTime();
                if (value.IndexOf('-') != -1)
                {
                    if (value.Length > 10)
                    {
                        result = DateTime.ParseExact(value, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        result = DateTime.ParseExact(value, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    }

                }
                else
                {
                    if (value.Length > 10)
                    {
                        result = DateTime.ParseExact(value, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        result = DateTime.ParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                if (IsTo == true)
                    result = result.AddDays(1).AddMilliseconds(-1);
                return Json(result.ToString());
            }
            catch
            {
                return Json(value);
            }


        }

        /// <summary>
        /// [Hien.Pham] Lấy thông tin client User
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetUserInfo()
        {
            string clientIP;
            string clientName = Request.ServerVariables["REMOTE_HOST"];
            clientIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (clientIP == "" || clientIP == null)
            {
                clientIP = Request.ServerVariables["HTTP_CLIENT_IP"];
            }

            string userid = Session[SessionObjects.UserId] == null ? string.Empty : Session[SessionObjects.UserId].ToString();
            string strResult = string.Empty;
            strResult += Constant.ServerUpdate + "=" + clientName + "&";
            strResult += Constant.DateUpdate + "=" + DateTime.Now + "&";
            strResult += Constant.IPUpdate + "=" + clientIP + "&";
            strResult += Constant.UserUpdate + "=" + userid + "&";
            strResult += Constant.ServerCreate + "=" + clientName + "&";
            strResult += Constant.DateCreate + "=" + DateTime.Now + "&";
            strResult += Constant.IPCreate + "=" + clientIP + "&";
            strResult += Constant.UserCreate + "=" + userid + "&";

            return Json(strResult);
        }
        #endregion
    }
}
