using HRM.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HRM.Presentation.HrmSystem.Web.Controllers
{
    public class SettingController : Controller
    {
        //
        // GET: /Setting/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult setLanguage(string Language, string userName)
        {
            Session[SessionObjects.LoginUserName] = userName;
            this.Session.Add(SessionObjects.LoginUserName, userName);
            TranslateService.LanguageCode = Language;
            return Json("");
        }
	}
}