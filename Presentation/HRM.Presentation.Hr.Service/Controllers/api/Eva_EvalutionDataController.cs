using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Presentation.Evaluation.Models;
using HRM.Presentation.Service;
using HRM.Business.Evaluation.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Eva_EvalutionDataController : ApiController
    {
        #region UserLogin
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
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Eva_EvalutionData")]
        public Eva_EvalutionDataModel Post([Bind]Eva_EvalutionDataModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Eva_EvalutionDataModel>(model, "Eva_EvalutionData", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Eva_EvalutionDataEntity, Eva_EvalutionDataModel>(model);
        }
        public Eva_EvalutionDataModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Eva_EvalutionDataEntity, Eva_EvalutionDataModel>(id);
            return result;
        }
        
	}
}