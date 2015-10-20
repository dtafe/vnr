using System.Web;
using HRM.Infrastructure.Utilities;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using HRM.Presentation.HrmSystem.Models;
using VnResource.Helper.Security;
using System;
using System.Globalization;

namespace HRM.Presentation.Main.Controllers
{
    public class MainBaseController : Controller
    {
        #region Properties

        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;

        public string UserLogin
        {
            get
            {
                var sessionName = SessionObjects.LoginUserName;
                string userLogin = string.Empty;

                if (Session != null && Session[sessionName] != null)
                {
                    userLogin = Session[sessionName].ToString();
                }
                return userLogin;
            }
        }

        public Guid UserId
        {
            get
            {
                var sessionName = SessionObjects.UserId;
                Guid UserId = Guid.Empty;

                if (Session[sessionName] != null)
                {
                    UserId = Common.ConvertToGuid(Session[sessionName].ToString());
                }
                return UserId;
            }
        }
        #endregion

        #region Methods

        [HttpPost]
        public JsonResult GetProfileID()
        {

            string s = null;
            Guid ProfileID = Guid.Empty;
            try
            {
                s = Session[SessionObjects.ProfileID].ToString();
                ProfileID = Common.ConvertToGuid(s);
            }
            catch
            { }

            return Json(ProfileID, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetView(string viewName, string permission)
        //{
        //    var sessionId = Session["userId"];

        //    if (sessionId == null)
        //    {
        //        return View("Login");
        //    }
        //    else
        //    {
        //        if (CheckPermission(permission))
        //        {
        //            return View(viewName);
        //        }
        //        else
        //        {
        //            return GetView("Denied", "");
        //        }
        //    }
        //}

        //public bool CheckPermission(string permission)
        //{
        //    return true;
        //}

        //<summary> Kiểm tra quyền cho user </summary>
        //<param name="privilegeType"></param>
        //<param name="permission"></param>
        //<returns></returns>
        //public ActionResult RemoveOrDeleteAndReturn<TModel>(string urlService, string api, List<Guid> selectedIds, string constantPermission, string deleteType) where TModel : class
        //{

        //    if (selectedIds.Count >= 0)
        //    {
        //        //selectedIds = deleteType + "," + selectedIds;
        //        //var service = new RestServiceClient<TModel>();
        //        //service.SetCookies(this.Request.Cookies, urlService);
        //        //var dataReturn = service.Delete(urlService, api, selectedIds);

        //        var service = new RestServiceClient<TModel>();
        //        service.SetCookies(this.Request.Cookies, urlService);
        //        for (int i = 0; i < selectedIds.Count - 1; i++)
        //        {
        //            service.Delete(urlService, api, deleteType + "," + selectedIds[i]);
        //        }
        //        var dataReturn = service.Delete(urlService, api, deleteType + "," + selectedIds[selectedIds.Count - 1]);
        //        return Json(dataReturn, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(null);
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

            if (permission.Contains("_GetData"))
            {
                //khi click len lưới thì không phân quyền
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


        public bool HasPermission(PrivilegeType privilegeType, string permission)
        {
            var isAllow = false;
            var a = CheckPermission(privilegeType, permission);
            if (a != null && a.Data is bool)
            {
                isAllow = (bool)a.Data;
            }
            return isAllow;
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
        public ActionResult RemoveOrDeleteAndReturn<TModel>(string urlService, string api, List<Guid> selectedIds, string constantPermission, string deleteType) where TModel : class
        {


            if (selectedIds.Count >= 0)
            {
                int _total = selectedIds.Count;
                int _totalPage = _total / 5 + 1;
                int _pageSize = 5;
                var dataReturn = new object();
                for (int _page = 1; _page <= _totalPage; _page++)
                {
                    int _skip = _pageSize * (_page - 1);
                    var _listCurrenPage = selectedIds.Skip(_skip).Take(_pageSize).ToList();
                    var _strDelIDs = string.Join(",", _listCurrenPage);
                    var service = new RestServiceClient<TModel>(UserLogin);
                    service.SetCookies(this.Request.Cookies, urlService);
                    dataReturn = service.Delete(urlService, api, deleteType + "," + _strDelIDs);
                }
                return Json(dataReturn, JsonRequestBehavior.AllowGet);

            }
            return Json(null);
        }

        /// <summary>
        /// [Chuc.Nguyen]- Xóa hoặc chuyển đổi trạng thái IsDelete = true của một đối tượng bất kỳ
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="urlService"></param>
        /// <param name="api"></param>
        /// <param name="selectedIds"></param>
        /// <param name="constantPermission"></param>
        /// <param name="deleteType"></param>
        /// <returns></returns>
        public ActionResult RemoveOrDeleteAndReturn<TModel>(string urlService, string api, string selectedIds, string constantPermission, string deleteType) where TModel : class
        {
            //var isAccess = CheckPermission(UserId, PrivilegeType.Delete, constantPermission);
            //if (!isAccess)
            //{
            //    return PartialView("AccessDenied");
            //}
            if (!string.IsNullOrEmpty(selectedIds))
            {
                var strIDs = selectedIds.Split(new char[] { ',' });
                List<Guid> lstDelIDs = new List<Guid>();

                foreach (var item in strIDs)
                {
                    lstDelIDs.Add(new Guid(item));
                }
                RemoveOrDeleteAndReturn<TModel>(urlService, api, lstDelIDs, constantPermission, deleteType);
                //selectedIds = deleteType + "," + selectedIds;
                //var service = new RestServiceClient<TModel>();
                //service.SetCookies(this.Request.Cookies, urlService);
                //var dataReturn = service.Delete(urlService, api, selectedIds);

                ////var service = new RestServiceClient<TModel>();
                ////service.SetCookies(this.Request.Cookies, urlService);
                ////for (int i = 0; i < selectedIds.Count - 1; i++)
                ////{
                ////    service.Delete(urlService, api, selectedIds[i]);
                ////}
                ////var dataReturn = service.Delete(urlService, api, selectedIds[selectedIds.Count - 1]);
                //return Json(dataReturn, JsonRequestBehavior.AllowGet);
            }
            return Json(null);
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
            string userName = Session[SessionObjects.LoginUserName] == null ? string.Empty : Session[SessionObjects.LoginUserName].ToString();

            string strResult = string.Empty;
            strResult += Constant.ServerUpdate + "=" + clientName + "&";
            strResult += Constant.DateUpdate + "=" + DateTime.Now + "&";
            strResult += Constant.IPUpdate + "=" + clientIP + "&";
            strResult += Constant.UserUpdate + "=" + userName + "&";
            strResult += Constant.ServerCreate + "=" + clientName + "&";
            //strResult += Constant.UserUpdate + "=" + userid + "&";
            //strResult += Constant.DateCreate + "=" + DateTime.Now + "&";
            strResult += Constant.IPCreate + "=" + clientIP + "&";
            strResult += Constant.UserCreate + "=" + userName + "&";

            return Json(strResult);
        }
        /// <summary> Kiểm tra quyền cho user </summary>
        /// <param name="userId"></param>
        /// <param name="privilegeType"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        //public bool CheckPermission(Guid userId, PrivilegeType privilegeType, string permission)
        //{

        //    var permissionModel = new PermissionModel
        //    {
        //        UserID = userId,
        //        PrivilegeType = privilegeType,
        //        Permission = permission
        //    };
        //    var service = new RestServiceClient<PermissionModel>();
        //    service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
        //    PermissionModel result = service.Post(_hrm_Sys_Service, "api/SysPermission/", permissionModel);
        //    return result.IsAllowAccess;
        //}

        #endregion

        /// <summary> Kiểm tra quyền cho user </summary>
        /// <param name="userId"></param>
        /// <param name="privilegeType"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        //public bool CheckPermission(Guid userId, PrivilegeType privilegeType, string permission)
        //{
        //    return true;
        //    var permissionModel = new PermissionModel
        //    {
        //        UserID = userId,
        //        PrivilegeType = privilegeType,
        //        Permission = permission
        //    };
        //    var service = new RestServiceClient<PermissionModel>();
        //    service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
        //    PermissionModel result = service.Post(_hrm_Sys_Service, "api/SysPermission/", permissionModel);
        //    return result.IsAllowAccess;
        //}
    }
}
