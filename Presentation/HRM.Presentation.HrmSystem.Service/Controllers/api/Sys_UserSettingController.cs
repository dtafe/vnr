using HRM.Business.Hr.Models;
using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Main.Domain;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Service;
using VnResource.Helper.Data;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{

    public class Sys_UserSettingController : ApiController
    {
        public Sys_UserSettingModel Getbyid(Guid id)
        {
            var service = new Sys_AttOvertimePermitConfigServices();
            var language = service.GetConfigValue<string>(AppConfig.HRM_SYS_USERSETTING_LANGUAGE.ToString()+"_"+id.ToString());
            var theme = service.GetConfigValue<string>(AppConfig.HRM_SYS_USERSETTING_THEME + "_" + id.ToString());
            var userSetting = new Sys_UserSettingModel()
            {
                LanguageName = language,
                ThemeName = theme,
                UserCreateID = id
            };
            return userSetting;
        }


        public Sys_AllSettingModel Put(Sys_AllSettingModel model)
        {
            var sysAllSetting = new Sys_AllSettingEntity
            {
                ID = model.ID,
                Name = model.Name + "_" + model.UserID.ToString(),
                Value1 = model.Value1,
                Value2 = model.Value2,
                ModuleName = model.ModuleName,
                UserID = model.UserID
            };
            //var service = new Sys_AllSettingServices();
            //if (model.ID != Guid.Empty)
            //{
            //    sysAllSetting.ID = model.ID;
            //    service.Edit<Sys_AllSettingEntity>(sysAllSetting);
            //}
            //else
            //{
            //    service.Add<Sys_AllSettingEntity>(sysAllSetting);
            //}

            var sysAllSettingServices = new Sys_AttOvertimePermitConfigServices();
            sysAllSettingServices.CreateOrUpdateSysAllSetting(sysAllSetting.Name, model.Value1);
            return model;
        }

        public void Post(List<Sys_AllSettingModel> model)
        {
            foreach (var item in model)
            {
                Put(item);
            }
        }

    }
}