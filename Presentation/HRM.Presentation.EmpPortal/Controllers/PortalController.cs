using HRM.Infrastructure.Utilities;
using HRM.Presentation.Hr.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VnResource.Helper.Setting;
using VnResource.Helper.Data;
using HRM.Presentation.EmpPortal.Models;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Presentation.HrmSystem.Models;
using HRM.Business.Hr.Domain;
using HRM.Business.Attendance.Models;

namespace HRM.Presentation.EmpPortal.Controllers
{
    public class PortalController : BasePortalController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        readonly string _portal_Permission = ConstantPathWeb.Hrm_Portal_Permission;

        public void ChangeLanguage(Sys_UserSettingModel model)
        {
            if (string.IsNullOrEmpty(model.LanguageValue))
            {
                model.LanguageValue = Constant.VN;
            }
            VnResource.Helper.Utility.LanguageHelper.LanguageCode = model.LanguageValue;
        }
        public ActionResult Index(string userLogin)
        {
            var userId = Session[SessionObjects.UserId] == null ? Guid.Empty : (Guid)Session[SessionObjects.UserId];
            var listPermission = new List<string>();
            if (userId != Guid.Empty)
            {
                var service = new RestServiceClient<TempPermissionModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                var result = service.Get(_hrm_Sys_Service, "api/SysPermission/", userId);
                if (result != null && result.Data.Any())
                {
                    if (!string.IsNullOrEmpty(_portal_Permission))
                    {
                        List<string> listConfigPermission = _portal_Permission.Split(',').ToList();
                        listPermission = result.Data.Where(listConfigPermission.Contains).ToList();
                    }
                    else
                    {
                        listPermission = result.Data;
                    }
                }
                ViewBag.ListPermission = listPermission;

                #region Xử lý code để kiểm tra có cấu hình riêng cho khách hàng ko
                ActionService actionService = new ActionService(UserLogin);
                Sys_AllSettingModel model = new Sys_AllSettingModel();
                string status = string.Empty;
                var logoPath = "/Content/images/logo/";
                var logoFile = "logo-vnr.png";
                var defaultLogo = logoPath + logoFile;
                string clientId = null;
                string chartDefault = null;
                var setting = actionService.GetData<Sys_AllSettingEntity>("CLIENT_ID",
                    ConstantSql.hrm_sys_sp_get_AllSettingByKey, ref status);

          

                var objOvertime = new List<object>();
                objOvertime.AddRange(new object[13]);
                objOvertime[5] = "E_SUBMIT,E_FIRST_APPROVED";
                objOvertime[8] = userId;
                objOvertime[11] = 1;
                objOvertime[12] = int.MaxValue - 1;

                var listOvertime = actionService.GetData<Att_OvertimeEntity>(objOvertime, ConstantSql.hrm_att_sp_get_Overtime, ref status).ToList();

                var listOvertimeEntity = listOvertime.Where(s => !(s.Status == LeaveDayStatus.E_FIRST_APPROVED.ToString() && s.UserApproveID == userId)).ToList();



                var objLeaveday = new List<object>();
                objLeaveday.AddRange(new object[11]);
                objLeaveday[2] = "E_SUBMIT,E_FIRST_APPROVED";
                objLeaveday[7] = userId;
                objLeaveday[9] = 1;
                objLeaveday[10] = int.MaxValue - 1;
                var lstLeaveday = actionService.GetData<Att_LeaveDayEntity>(objLeaveday, ConstantSql.hrm_att_sp_get_Leaveday, ref status).ToList();

                var listLeavedayOvertime = lstLeaveday.Where(s => !(s.Status == OverTimeStatus.E_FIRST_APPROVED.ToString() && s.UserApproveID == userId)).ToList();

                model.Value3 = listOvertimeEntity.Count().ToString();
                model.Value4 = listLeavedayOvertime.Count().ToString();
                if (setting != null && setting.Count > 0)
                {
                    model = setting.FirstOrDefault().CopyData<Sys_AllSettingModel>();
                    if (string.IsNullOrEmpty(model.Value2))
                    {
                        model.Value2 = defaultLogo;
                    }
                    else
                    {
                        model.Value2 = logoPath + model.Value2;
                    }
                    defaultLogo = model.Value2;
                    clientId = model.Value1;
                    if (!string.IsNullOrEmpty(model.Value4))
                    {
                        chartDefault = logoPath + "charts/" + model.Value4;
                    }
                   
                }
                Session["Chart"] = chartDefault;
                Session["CLIENT_ID"] = clientId;
                Session["LogoPath"] = defaultLogo; 
                #endregion
                
                GeneralProfileDetail(userLogin); 

                return GetView(model);
            }

            return GetView();
        }

        public ActionResult GeneralProfileDetail(string userLogin)
        {
            if (!CheckPermission()) return RedirectToAction("Denied", "Portal");

            var id = Session[SessionObjects.ProfileID];
            var service = new Hre_ProfileServices();
            string status = string.Empty;
            var model = service.GetData<Hre_ProfileModelPortal>(HRM.Infrastructure.Utilities.Common.DotNetToOracle(id.ToString()), ConstantSql.hrm_hr_sp_get_ProfileById, userLogin, ref status).FirstOrDefault();
            if (model != null)
            {
                Session["ProfileName"] = model.ProfileName;
                model.ActionStatus = status;

                var info = new Hre_NotificationModel();
                info.ProfileName = model.ProfileName;
                info.UserLogin = Session[SessionObjects.LoginUserName].ToString();
                info.EmployeeTypeName = model.EmployeeTypeName;
                info.JobTitleName = model.JobTitleName;
                info.OrgStructureName = model.OrgStructureName;

                Session["LoginInfo"] = info;

            }
            return GetOnlyView(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        
        public ActionResult Denied()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return GetView();
        }
        [HttpGet]
        public ActionResult ChangePasswordDefault()
        {
            return View();
        }

        [HttpPost]
        public string ChangePassword(ChangePasswordModel model)
        {
            var userId = Session["UserId"];
            var strMessages = string.Empty;
            if (userId != null)
            {
                if (model != null)
                {
                    if (string.IsNullOrEmpty(model.OldPassword))
                    {
                        return ShowMessages(NotificationType.E_NotNull, "OldPassword");
                    }
                    if (string.IsNullOrEmpty(model.NewPassword))
                    {
                        return ShowMessages(NotificationType.E_NotNull, "NewPassword");
                    }
                    if (model.NewPassword != model.ReNewPassword)
                    {
                        return ShowMessages(NotificationType.E_Mismatch, "ReNewPassword");
                    }
                    ActionService actionService = new ActionService(UserLogin);
                    string status = string.Empty;
                    var userInfo = actionService.GetByIdUseStore<Sys_UserInfoEntity>((Guid)userId, ConstantSql.hrm_sys_sp_get_UserbyId, ref status);
                    if (userInfo != null)
                    {
                        var oldPass = userInfo.Password;
                        if (oldPass == EncryptUtil.MD5(model.OldPassword))
                        {
                            var newPass = EncryptUtil.MD5(model.NewPassword);
                            if (oldPass == newPass)
                            {
                                return ShowMessages(NotificationType.E_Messages, ConstantDisplay.NewPassTheSameOldPass);
                            }
                            userInfo.Password = newPass;
                            userInfo.DateChangePasssword = DateTime.Now;
                            var service = new Sys_UserServices();
                            var result = service.Edit<Sys_UserInfoEntity>(userInfo);

                            return ShowMessages(NotificationType.E_ChangePass_Success, ConstantDisplay.ChangePassword);
                        }
                        else
                        {
                            return ShowMessages(NotificationType.E_Incorrect, "OldPassword");
                        }
                    }
                    else
                    {
                        return ShowMessages(NotificationType.E_NotFound, ConstantDisplay.User);
                    }
                }
            }
            return strMessages;
        }

        public ActionResult Home(string userLogin)
        {
            //var service = new BaseService();
            var service = new Hre_ProfileServices();
            var info = new Hre_NotificationModel();
            info = (Hre_NotificationModel)Session["LoginInfo"];
            //count yeu cau tang ca cho duyet
            #region Hien thi du lieu tren trang chu
            string status = string.Empty;
            //Count ds tang ca cho duyet
            var userId = Session[SessionObjects.UserId] == null ? Guid.Empty : (Guid)Session[SessionObjects.UserId];
            var objOvertime = new List<object>();
            objOvertime.AddRange(new object[13]);
            objOvertime[5] = "E_SUBMIT,E_FIRST_APPROVED";
            objOvertime[8] = userId;
            objOvertime[11] = 1;
            objOvertime[12] = int.MaxValue - 1;
            var listOvertimeEntity = service.GetData<Att_OvertimeEntity>(objOvertime, ConstantSql.hrm_att_sp_get_Overtime, userLogin, ref status).ToList();
            var listOvertime = listOvertimeEntity.Where(s => !(s.Status == LeaveDayStatus.E_FIRST_APPROVED.ToString() && s.UserApproveID == userId)).ToList();
            if (listOvertime != null)
                info.CountOvertime = listOvertime.Count();
           
            //count ds ngay nghi cho duyet
            var objLeaveday = new List<object>();
            objLeaveday.AddRange(new object[11]);
            objLeaveday[2] = "E_SUBMIT,E_FIRST_APPROVED";
            objLeaveday[7] = userId;
            objLeaveday[9] = 1;
            objLeaveday[10] = int.MaxValue - 1;
            var lstLeavedayEntity = service.GetData<Att_LeaveDayEntity>(objLeaveday, ConstantSql.hrm_att_sp_get_Leaveday, userLogin, ref status).ToList();
            var listLeaveday = lstLeavedayEntity.Where(s => !(s.Status == OverTimeStatus.E_FIRST_APPROVED.ToString() && s.UserApproveID == userId)).ToList();
            if (listLeaveday != null)
                info.CountLeaveday = listLeaveday.Count();
            #endregion


            return GetView(info);
        }

        #region Login
        [HttpGet]
        public ActionResult Login()
        {
            if (Session[SessionObjects.UserId] != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (model != null)
            {
                if (string.IsNullOrEmpty(model.UserName))
                {
                    ModelState.AddModelError("", ConstantDisplay.Hrm_Portal_Login_Enter_Uername.TranslateString());
                    return View(model);
                }
                if (string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError("", ConstantDisplay.Hrm_Portal_Login_Enter_Password.TranslateString());
                    return View(model);
                }

                var passwordEncrypt = EncryptUtil.MD5(model.Password);
                var loginInfo = new LoginModel
                {
                    UserName = model.UserName,
                    Password = passwordEncrypt
                };
                var service = new RestServiceClient<LoginModel>(UserLogin);
                service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
                var result = service.Post(_hrm_Sys_Service, "api/Login/", loginInfo);
                if (result != null)
                {
                    if (!result.IsActive)
                    {
                        ModelState.AddModelError("", ConstantDisplay.Hrm_Portal_Login_User_Not_Active.TranslateString());
                        return View(model);
                    }
                    if (string.IsNullOrEmpty(model.Language))
                    {
                        model.Language = Constant.VN;
                    }
                    

                    Session[SessionObjects.IsFirstLogin] = result.IsFirstLogin;
                    Session[SessionObjects.UserId] = result.ID;
                    Session[SessionObjects.LoginUserName] = model.UserName;
                    //Session[SessionObjects.LoginUserName] = result.UserName;
                    Session[SessionObjects.FullName] = result.FullName;
                    //Session[SessionObjects.UserInfoName] = result.UserInfoName;
                    Session[SessionObjects.ProfileID] = result.ProfileID;
                    VnResource.Helper.Utility.LanguageHelper.LanguageCode = model.Language;
                    Session[SessionObjects.LanguageCode + model.UserName] = model.Language;
                    if (result.IsFirstLogin)
                    {
                        return RedirectToAction("ChangePasswordDefault");
                    }
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", ConstantDisplay.Hrm_Portal_Login_User_Not_Found.TranslateString());
                return View(model);
            }
            return View();
        } 
        #endregion

        public ActionResult GetFieldRequired(string tableName)
        {
            List<HRM.Business.Main.Domain.DataFieldInfo> listSettingField = Common.GetDataFromXml<HRM.Business.Main.Domain.DataFieldInfo>(Common.GetPathPortal(@"Settings\FIELD_INFO.XML"), SettingConstants.Field).Where(d => d.TableName == tableName).ToList();
            if (listSettingField.Count > 0)
            {
                return Json(listSettingField, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

	}
}