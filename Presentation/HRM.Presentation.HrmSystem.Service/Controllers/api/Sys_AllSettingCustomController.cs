using HRM.Business.HrmSystem.Models;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using HRM.Presentation.Service;
using System.Web.Http;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{

    public class Sys_AllSettingCustomController : ApiController
    {
        #region MyRegion
        private string userLogin = string.Empty;
        public string UserLogin
        {
            get
            {
                if (Request.Headers != null)
                {
                    var headerValues = Request.Headers.GetValues(HeaderObject.UserLogin);
                    userLogin = headerValues.FirstOrDefault();
                }
                return userLogin;
            }
        }
        #endregion
        /// <summary>
        /// [Tam.Le] - 15.8.2014 - Tao moi Sys_AllSetting
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Sys_AllSetting")]
        public Sys_AllSettingModel Post([Bind]Sys_AllSettingModel model)
        {
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Sys_AllSettingEntity, Sys_AllSettingModel>(model);
        }
    }
}