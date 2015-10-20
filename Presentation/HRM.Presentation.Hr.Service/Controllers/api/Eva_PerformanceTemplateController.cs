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
    public class Eva_PerformanceTemplateController : ApiController
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
        public Eva_PerformanceTemplateModel  GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Eva_PerformanceTemplateModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Eva_PerformanceTemplateEntity>(id, ConstantSql.hrm_cat_sp_get_TemplateById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Eva_PerformanceTemplateModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Eva_PerformanceTemplateModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Eva_PerformanceTemplateEntity, Eva_PerformanceTemplateModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Eva_PerformanceTemplate")]
        public Eva_PerformanceTemplateModel Post([Bind]Eva_PerformanceTemplateModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Eva_PerformanceTemplateModel>(model, "Eva_PerformanceTemplate", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion


            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Eva_PerformanceTemplateEntity, Eva_PerformanceTemplateModel>(model);
        }
	}
}