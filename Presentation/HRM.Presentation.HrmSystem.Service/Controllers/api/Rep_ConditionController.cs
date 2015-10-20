using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HRM.Business.Hr.Models;
using VnResource.Helper.Data;
using System.Net.Http;
using System.Net;
using System.Web.Mvc;
using HRM.Infrastructure.Utilities;
using HRM.Presentation.Service;
using System;
using HRM.Business.Category.Domain;
using HRM.Business.Category.Models;
using System.Web;
using HRM.Presentation.HrmSystem.Models;
using HRM.Business.HrmSystem.Models;
using HRM.Business.Report.Models;

namespace HRM.Presentation.HrmSystem.Service.Controllers.api
{
    public class Rep_ConditionController : ApiController
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
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Rep_Condition")]
        public Rep_ConditionModel Post([Bind]Rep_ConditionModel model)
        {

            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Rep_ConditionModel>(model, "Rep_Condition", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Rep_ConditionEntity, Rep_ConditionModel>(model);
        }
	}
}