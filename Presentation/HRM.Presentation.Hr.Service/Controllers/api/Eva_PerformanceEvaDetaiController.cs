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
    public class Eva_PerformanceEvaDetailController : ApiController
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
        public Eva_PerformanceEvaDetailModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Eva_PerformanceEvaDetailModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Eva_PerformanceEvaDetailEntity>(id, ConstantSql.hrm_eva_sp_get_PerformanceEvaDetailById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Eva_PerformanceEvaDetailModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Eva_PerformanceEvaDetailModel DeleteOrRemove(string id)
        {
            ActionService service = new ActionService(UserLogin);
            var result = service.DeleteOrRemove<Eva_PerformanceEvaDetailEntity, Eva_PerformanceEvaDetailModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Eva_PerformanceEvaDetail")]
        public Eva_PerformanceEvaDetailModel Post([Bind]Eva_PerformanceEvaDetailModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Eva_PerformanceEvaDetailModel>(model, "Eva_PerformanceEvaDetail", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }

            #endregion
            ActionService service = new ActionService(UserLogin);
            return service.UpdateOrCreate<Eva_PerformanceEvaDetailEntity, Eva_PerformanceEvaDetailModel>(model);
        }
	}
}