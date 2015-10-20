using HRM.Business.HrmSystem.Domain;
using HRM.Business.HrmSystem.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using HRM.Presentation.Service;
using System.Web.Http;
using System.Web.Mvc;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class Sys_AppConfigController : ApiController
    {
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sys_AppConfig")]
        public Sys_AppConfigModel Post([Bind]Sys_AppConfigModel model)
        {
            //Sys_AttOvertimePermitConfigServices Sys_Services = new Sys_AttOvertimePermitConfigServices();
            //Sys_Services.CreateOrUpdateSysAllSetting(AppConfig.HRM_SAL_CONFIG_VALUE52, model.Value52);
            //Sys_Services.CreateOrUpdateSysAllSetting(AppConfig.HRM_SAL_CONFIG_VALUE53, model.Value53);
            //Sys_Services.CreateOrUpdateSysAllSetting(AppConfig.HRM_SAL_CONFIG_VALUE54, model.Value54);
            //Sys_Services.CreateOrUpdateSysAllSetting(AppConfig.HRM_SAL_CONFIG_VALUE55, model.Value55);
            //Sys_Services.CreateOrUpdateSysAllSetting(AppConfig.HRM_SAL_CONFIG_VALUE56, model.Value56);
            //Sys_Services.CreateOrUpdateSysAllSetting(AppConfig.HRM_SAL_CONFIG_VALUE57, model.Value57);
            //Sys_Services.CreateOrUpdateSysAllSetting(AppConfig.HRM_SAL_CONFIG_VALUE58, model.Value58);
            //Sys_Services.CreateOrUpdateSysAllSetting(AppConfig.HRM_SAL_CONFIG_VALUE59, model.Value59);
            return new Sys_AppConfigModel();
        }


   
	}
}