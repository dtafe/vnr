using HRM.Infrastructure.Utilities;
using System;
using System.Linq;
using System.Web.Mvc;
using HRM.Presentation.HrmSystem.Models;
using VnResource.Helper.Data;
using VnResource.Helper.Security;
using System.Globalization;

namespace HRM.Presentation.EmpPortal.Controllers
{
    public class BasePortalController : Controller
    {
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

        //public ActionResult CheckLogin()
        //{
        //    var userId = System.Web.HttpContext.Current.Session["UserId"];
        //    var userName = System.Web.HttpContext.Current.Session["UserLogin"];
        //    if (userId == null && userName == null)
        //    {
        //        return RedirectToAction("Login", "Portal");
        //    }
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //    return RedirectToAction(actionName, controllerName);
        //}
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        public bool CheckPermission(PrivilegeType privilegeType, string permission)
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
                return true;
            }

            if (permission.Contains("_GetData"))
            {
                //khi click len lưới thì không phân quyền
                return true;
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

            return isAccess;
        }

        public JsonResult CheckPermissionAction(PrivilegeType privilegeType, string permission)
        {
            var isAccess = CheckPermission(privilegeType, permission);
            return Json(isAccess, JsonRequestBehavior.AllowGet);
        }

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

        #region Get Messages
        public string ShowMessages(NotificationType type, params string[] objectName)
        {
            var strMessages = string.Empty;
            if (objectName != null)
            {
                strMessages = objectName.FirstOrDefault() + "|";
                var count = objectName.Count();
                var arrayObjectName = new string[count];
                for (int i = 0; i < count; i++)
                {
                    arrayObjectName[i] = objectName[i].TranslateString();
                }

                var messages = string.Format(type.ToString().TranslateString(), arrayObjectName);
                var strType = "alert-danger";
                var strClose = Constant.Close.TranslateString();

                if (type == NotificationType.Success)
                {
                    strType = "alert-success";
                }
                if (type == NotificationType.E_ChangePass_Success)
                {
                    strType = "alert-success";
                }

                strMessages += "<div class=\"alert " + strType + " alert-dismissible\" role=\"alert\">" +
                                  "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"><span aria-hidden=\"true\">×</span><span class=\"sr-only\">" + strClose + "</span></button>" + messages +
                                  "</div>";
            }

            return strMessages;
        }
        #endregion

        #region Get View
        public ActionResult GetView()
        {
            return GetView(string.Empty);
        }

        public ActionResult GetView(object data)
        {
            return GetView(string.Empty, data);
        }

        public ActionResult GetView(string permission)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            //string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            return GetView(actionName, permission, null);
        }

        public ActionResult GetView(string permission, object data)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            //string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            return GetView(actionName, permission, data);
        }

        public ActionResult GetView(string actionName, string permission, object data)
        {
            var sessionId = Session["UserId"];

            if (sessionId == null)
            {
                return RedirectToAction("Login", "Portal");
            }
            else
            {
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                var privilegeName = controllerName+"_"+actionName+"_Portal";
                if (CheckPermission(PrivilegeType.View, privilegeName))
                {
                    if (data != null)
                    {
                        return View(actionName, data);
                    }
                    return View(actionName);
                }
                else
                {
                    return GetOnlyView("Denied", "Denied");
                }


                #region code Cũ
                //if (CheckPermission(permission))
                //{
                //    if (data != null)
                //    {
                //        return View(actionName, data);
                //    }
                //    return View(actionName);
                //}
                //else
                //{
                //    return GetView("Denied", "Denied");
                //} 
                #endregion
            }
        }
        #endregion

        #region Get Only View
        public ActionResult GetOnlyView()
        {
            return GetOnlyView(null);
        }

        public ActionResult GetOnlyView(object data)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            return GetOnlyView(actionName, data);
        }

        public ActionResult GetOnlyView(string actionName, object data)
        {
            var sessionId = Session["UserId"];

            if (sessionId == null)
            {
                return RedirectToAction("Login", "Portal");
            }
            else
            {
                if (data != null)
                {
                    return View(actionName, data);
                }
                return View(actionName);
            }
        }
        #endregion

        #region Check Permission
        public bool CheckPermission()
        {
            return CheckPermission(string.Empty);
        }
        public bool CheckPermission(string permission)
        {
            var sessionId = Session["UserId"];
            if (sessionId != null && permission != "Denied")
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Validate
        public T Validate<T>(T model, string validateName)
        {
            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<T>(model, validateName, ref message);
            if (!checkValidate)
            {
                model.SetPropertyValue("ActionStatus", message);
            }

            return model; 
        }
        #endregion

    }
}