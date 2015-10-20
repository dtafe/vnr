using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using HRM.Presentation.Main.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VnResource.Helper.Utility;

namespace HRM.Presentation.Main.Controllers
{
    public class SettingController : MainBaseController
    {
        //
        // GET: /Session/
        public ActionResult Index()
        {
            return View();
        }

        public bool setSession(SessionObjectsModel model)
        {
            this.Session.Add(SessionObjectsModel.FieldsName.UserID, model.UserID);
            this.Session.Add(SessionObjectsModel.FieldsName.LoginUserName, model.LoginUserName);
            this.Session.Add(SessionObjectsModel.FieldsName.IsActive, model.IsActive);

            return true;
        }

        //public ActionResult setLanguage(string Language, string userName)
        //{
        //    this.Session.Add(SessionObjects.LanguageCode, Language);
        //    LanguageHelper.LanguageCode = Language;
        //    return Json("");
        //}
    }
}