using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using HRM.Presentation.HrmSystem.Web.Controllers;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;

namespace HRM.Presentation.Category.Web.Controllers
{
    public class Sys_ConfigGeneralController : BaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /UserSetting/
        public ActionResult Index()
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sys_ConfigDBLau);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            string str = "HRM_SYS_LAU_TAMSCANLOG_";
            var service = new RestServiceClient<Sys_ConfigDBLauModel>();
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var modelResult = service.Get(_hrm_Sys_Service, "api/Sys_ConfigDBLau/", str);
            return View(modelResult);
        }

        public ActionResult ConfigDBLau(Sys_ConfigDBLauModel model)
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.Create, ConstantPermission.Sys_ConfigDBLau);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_ConfigDBLauModel>();
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Post(_hrm_Sys_Service, "api/Sys_ConfigDBLau/", model);
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            var isAccess = CheckPermission(UserId, PrivilegeType.View, ConstantPermission.Sys_AttendanceConfig);
            if (!isAccess)
            {
                return PartialView("AccessDenied");
            }
            var service = new RestServiceClient<Sys_GeneralConfigModel>();
            service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            var result = service.Get(_hrm_Sys_Service, "api/Sys_GeneralConfig/", UserId);
            if (result != null)
            {
                return View(result);
            }
            return View();
        }

        [HttpPost]
        public ActionResult CheckMailServer(string Host,int Port,string MailFrom ,string MailUserName, string MailPassword, string EmailTest, bool IsSSL)
        {
            var isSuccess = false;
            #region Sent Mail
            try
            {
                Console.WriteLine("Mail To");
                MailAddress to = new MailAddress(EmailTest);

                Console.WriteLine("Mail From");
                MailAddress from = new MailAddress(MailFrom);

                MailMessage mail = new MailMessage(from, to);

                Console.WriteLine("Subject");
                mail.Subject = "Test Subject";

                Console.WriteLine("Your Message");
                mail.Body = "Body";

                SmtpClient smtp = new SmtpClient();
                smtp.Host = Host;
                smtp.Port = Port;

                smtp.Credentials = new NetworkCredential(MailFrom, MailPassword);
                smtp.EnableSsl = IsSSL;
                Console.WriteLine("Sending email...");
                smtp.Send(mail);
                isSuccess = true;
            }
            catch (Exception)
            {
                isSuccess = false;
            }
            #endregion

            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        
    }


}