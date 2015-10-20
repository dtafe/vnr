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
    public class Eva_PerformanceForDetailController : ApiController
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
        public Eva_PerformanceForDetailModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Eva_PerformanceForDetailEntity, Eva_PerformanceForDetailModel>(id);
            return result;
        }
        public Eva_PerformanceForDetailModel Post([Bind]Eva_PerformanceForDetailModel model)
        {
            if (!string.IsNullOrEmpty(model.DescriptionKP)) // luu tam chuoi KPIID
                model.KPIID = Guid.Parse(model.DescriptionKP);
            model.DescriptionKP = "";
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Eva_PerformanceForDetailModel>(model, "Eva_PerformanceForDetail", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Eva_PerformanceForDetailEntity, Eva_PerformanceForDetailModel>(model);
        }
	}
}