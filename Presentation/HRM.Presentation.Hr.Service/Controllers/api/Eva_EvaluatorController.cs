using System;
using System.Web.Http;
using System.Web.Mvc;
using HRM.Business.Evaluation.Domain;
using HRM.Presentation.Evaluation.Models;
using HRM.Presentation.Service;
using HRM.Business.Evaluation.Models;
using HRM.Infrastructure.Utilities;
using VnResource.Helper.Data;
using System.Linq;

namespace HRM.Presentation.Hr.Service.Controllers.api
{
    public class Eva_EvaluatorController : ApiController
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
        public Eva_EvaluatorModel GetById(Guid id)
        {
            string status = string.Empty;
            var model = new Eva_EvaluatorModel();
            var service = new ActionService(UserLogin);
            var entity = service.GetByIdUseStore<Eva_EvaluatorEntity>(id, ConstantSql.hrm_eva_sp_get_EvaluatorById, ref status);
            if (entity != null)
            {
                model = entity.CopyData<Eva_EvaluatorModel>();
            }
            model.ActionStatus = status;
            return model;
        }

        public Eva_EvaluatorModel DeleteOrRemove(string id)
        {
            var service1 = new ActionService(UserLogin);
            var result = service1.DeleteOrRemove<Eva_EvaluatorEntity, Eva_EvaluatorModel>(id);
            return result;
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.RouteAttribute("api/Eva_Evaluator")]
        public Eva_EvaluatorModel Post([Bind]Eva_EvaluatorModel model)
        {
            #region Validate

            string message = string.Empty;
            var checkValidate = HRM.Business.Main.Domain.ValidatorService.OnValidateData<Eva_EvaluatorModel>(model, "Eva_Evaluator", ref message);
            if (!checkValidate)
            {
                model.ActionStatus = message;
                return model;
            }
            #endregion


            Eva_EvaluatorServices evaluatorService = new Eva_EvaluatorServices();
            var evaluatorEntity = model.CopyData<Eva_EvaluatorEntity>();
            evaluatorService.AddEvaluators(evaluatorEntity);
            return model;
            //ActionService service = new ActionService(UserLogin);
            //return service.UpdateOrCreate<Eva_EvaluatorEntity, Eva_EvaluatorModel>(model);
        }

    }
}