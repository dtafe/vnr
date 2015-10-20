using HRM.Infrastructure.Utilities;
using HRM.Presentation.HrmSystem.Models;
using HRM.Presentation.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class MenuOptionController : ApiController
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
        public Sys_AllSettingModel Get(string id)
        {
            string status = string.Empty;
            ActionService service = new ActionService(UserLogin);
            var model = service.GetFirstData<Sys_AllSettingModel>(id, ConstantSql.hrm_sys_sp_get_AllSettingByKey, ref status);
            if (model != null)
            {
                return (Sys_AllSettingModel)model;
            }

            return null;
        }
    }
}
