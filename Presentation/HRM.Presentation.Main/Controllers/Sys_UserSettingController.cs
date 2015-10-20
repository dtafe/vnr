using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;

using VnResource.ARTS.Library;
using VnResource.Helper.Security;
using HRM.Infrastructure.Security;
using System.Collections;
using System.Web.Caching;
using HRM.Presentation.Main.Controllers;
using VnResource.Helper.Utility;

namespace HRM.Presentation.Category.Web.Controllers
{
    public class Sys_UserSettingController : MainBaseController
    {
        readonly string _hrm_Sys_Service = ConstantPathWeb.Hrm_Sys_Service;
        //
        // GET: /UserSetting/
        public ActionResult Index()
        {
           


            //if (UserId != Guid.Empty)
            //{
            // var id1 = Common.ConvertToGuid(UserId);
            //var service = new RestServiceClient<Sys_UserSettingModel>();
            //service.SetCookies(Request.Cookies, _hrm_Sys_Service);
            //var modelResult = service.Get(_hrm_Sys_Service, "api/Sys_UserSetting/", UserId);
            //if (modelResult != null)
            //{
            //    return View(modelResult);
            //}

            //}
            //else
            //{
            //    return View();
            //}


            return View();
        }

        public ActionResult ChangeLanguage(Sys_UserSettingModel model)
        {
            LanguageHelper.LanguageCode = model.LanguageValue;
            Session[SessionObjects.LanguageCode + (Session[SessionObjects.UserInfoName] == null ? string.Empty : Session[SessionObjects.UserInfoName].ToString())] = model.LanguageValue;
            //TranslateService.LanguageCode = model.LanguageValue;

            Sys_AllSettingModel modelLang = new Sys_AllSettingModel()
            {
                Name = AppConfig.HRM_SYS_USERSETTING_LANGUAGE.ToString(),
                Value1 = model.LanguageValue,
                Value2 = null,
                UserID = model.UserCreateID,
                ModuleName = ""
            };


            //Sys_AllSettingModel modelTheme = new Sys_AllSettingModel()
            //{
            //    Name = AppConfig.HRM_SYS_USERSETTING_THEME.ToString(),
            //    Value1 = string.IsNullOrEmpty(model.ThemeName) ? EnumDropDown.ETheme.VnResourceDefault.ToString() : model.ThemeName,
            //    Value2 = null,
            //    UserID = model.UserCreateID,
            //    ModuleName = ""
            //};

            List<Sys_AllSettingModel> listModel = new List<Sys_AllSettingModel>();
            listModel.Add(modelLang);
            //listModel.Add(modelTheme);

            var service = new RestServiceClient<List<Sys_AllSettingModel>>(UserLogin);
            service.SetCookies(this.Request.Cookies, _hrm_Sys_Service);
            var result = service.Post(_hrm_Sys_Service, "api/Sys_UserSetting/", listModel);

            return RedirectToAction("Index");
        }

        public ActionResult ClearCache()
        {
            List<string> keys = new List<string>();

            // retrieve application Cache enumerator
            IDictionaryEnumerator enumerator = HttpContext.Cache.GetEnumerator();

            // copy all keys that currently exist in Cache
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }

            // delete every key from cache
            for (int i = 0; i < keys.Count; i++)
            {
                HttpContext.Cache.Remove(keys[i]);
            }
            return Json("");
        }

        //public void ClearApplicationCache()
        //{
        //    List<string> keys = new List<string>();

        //    // retrieve application Cache enumerator
        //    IDictionaryEnumerator enumerator = HttpContext.Cache.GetEnumerator();

        //    // copy all keys that currently exist in Cache
        //    while (enumerator.MoveNext())
        //    {
        //        keys.Add(enumerator.Key.ToString());
        //    }

        //    // delete every key from cache
        //    for (int i = 0; i < keys.Count; i++)
        //    {
        //        HttpContext.Cache.Remove(keys[i]);
        //    }
        //}
    }
}